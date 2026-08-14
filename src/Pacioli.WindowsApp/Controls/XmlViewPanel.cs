using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Pacioli.WindowsApp.Controls
{
    public partial class XmlViewPanel : UserControl
    {
        private static readonly Color ElementNameColor = Color.Gray;
        private static readonly Color AttributeNameColor = Color.FromArgb(255, 0, 0);
        private static readonly Color AttributeValueColor = Color.FromArgb(0, 0, 255);
        private static readonly Color TextContentColor = Color.Black;
        private static readonly Color PunctuationColor = Color.Black;
        private const float ContentFontSizeIncrease = 2f;

        private readonly Font _contentFont;

        private sealed record XmlTextSegment(string Text, Color Color, bool Bold);

        public XmlViewPanel()
        {
            InitializeComponent();
            _contentFont = new Font(F_XmlTreeView.Font.FontFamily, F_XmlTreeView.Font.Size + ContentFontSizeIncrease, FontStyle.Bold);
            F_XmlTreeView.ItemHeight = Math.Max(F_XmlTreeView.Font.Height, _contentFont.Height) + 4;
        }

        public void LoadXml(Stream xmlStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlStream);

            F_XmlTreeView.BeginUpdate();
            F_XmlTreeView.Nodes.Clear();
            var root = doc.DocumentElement;
            if (root != null)
            {
                var rootNode = new TreeNode(root.Name)
                {
                    Tag = new List<XmlTextSegment> { new(root.Name, ElementNameColor, false) }
                };
                F_XmlTreeView.Nodes.Add(rootNode);
                foreach (XmlNode childNode in root.ChildNodes)
                {
                    rootNode.Nodes.Add(ConstructNode(childNode));
                }
                rootNode.ExpandAll();
            }
            F_XmlTreeView.EndUpdate();
        }

        private TreeNode ConstructNode(XmlNode xmlNode)
        {
            var segments = new List<XmlTextSegment> { new(xmlNode.Name, ElementNameColor, false) };

            List<XmlTextSegment>? contentSegments = GetTextContentSegments(xmlNode);

            if (contentSegments != null)
            {
                segments.AddRange(contentSegments);
                var textNode = new TreeNode(BuildPlainText(segments)) { Tag = segments };
                return textNode;
            }

            var tn = new TreeNode(xmlNode.Name) { Tag = segments };
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                TreeNode n = ConstructNode(node);
                tn.Nodes.Add(n);
            }
            return tn;
        }

        private static string BuildPlainText(IEnumerable<XmlTextSegment> segments)
        {
            var sb = new StringBuilder();
            foreach (var segment in segments)
            {
                sb.Append(segment.Text);
            }
            return sb.ToString();
        }

        private void F_XmlTreeView_DrawNode(object? sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node?.Tag is not List<XmlTextSegment> segments)
            {
                e.DrawDefault = true;
                return;
            }

            bool isSelected = (e.State & TreeNodeStates.Selected) != 0;
            Color backColor = isSelected ? SystemColors.Highlight : F_XmlTreeView.BackColor;
            using (var backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            int x = e.Bounds.Left;
            foreach (var segment in segments)
            {
                if (string.IsNullOrEmpty(segment.Text))
                {
                    continue;
                }

                Font font = segment.Bold ? _contentFont : F_XmlTreeView.Font;
                Color color = isSelected ? SystemColors.HighlightText : segment.Color;
                var size = TextRenderer.MeasureText(e.Graphics, segment.Text, font, e.Bounds.Size, TextFormatFlags.NoPadding);
                int y = e.Bounds.Top + (e.Bounds.Height - font.Height) / 2;
                TextRenderer.DrawText(e.Graphics, segment.Text, font, new Point(x, y), color, TextFormatFlags.NoPadding);
                x += size.Width;
            }

            if (isSelected && e.Node.TreeView is { Focused: true })
            {
                using var pen = new Pen(SystemColors.ControlText) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
                e.Graphics.DrawRectangle(pen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }
        }

        private void LoadXmlNode(XmlNode xmlNode, TreeNode treeNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                StringBuilder nodeText = new StringBuilder(childNode.Name);

                string? textContent = GetTextContent(childNode);

                if (textContent != null)
                {
                    nodeText.Append($": {textContent}");
                }

                if (!string.IsNullOrWhiteSpace(childNode.InnerText) && childNode.ChildNodes.Count == 0)
                {
                    nodeText.Append($": {childNode.InnerText}");
                }

                if (childNode.Attributes != null)
                {
                    foreach (XmlAttribute attr in childNode.Attributes)
                    {
                        nodeText.Append($" @{attr.Name}={attr.Value}");
                    }
                }

                var childTreeNode = new TreeNode(nodeText.ToString());
                treeNode.Nodes.Add(childTreeNode);

                //if (textContent == null)
                {
                    LoadXmlNode(childNode, childTreeNode);
                }
            }
        }

        private static List<XmlTextSegment>? GetTextContentSegments(XmlNode xmlNode)
        {
            List<XmlTextSegment>? segments = null;

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!string.IsNullOrWhiteSpace(childNode.InnerText) && childNode.ChildNodes.Count == 0)
                {
                    segments ??= new List<XmlTextSegment>();
                    segments.Add(new XmlTextSegment($" {childNode.InnerText}", TextContentColor, true));

                    if (xmlNode.Attributes != null)
                    {
                        foreach (XmlAttribute attr in xmlNode.Attributes)
                        {
                            segments.Add(new XmlTextSegment(" @", PunctuationColor, false));
                            segments.Add(new XmlTextSegment(attr.Name, AttributeNameColor, false));
                            segments.Add(new XmlTextSegment("=", PunctuationColor, false));
                            segments.Add(new XmlTextSegment(attr.Value, AttributeValueColor, false));
                        }
                    }
                }
            }

            return segments;
        }

        private string? GetTextContent(XmlNode xmlNode)
        {
            bool isText = false;
            StringBuilder content = new StringBuilder();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!string.IsNullOrWhiteSpace(childNode.InnerText) && childNode.ChildNodes.Count == 0)
                {
                    content.Append($" {childNode.InnerText}");
                    isText = true;
                }
            }
            return isText ? content.ToString() : null;
        }

    }
}
