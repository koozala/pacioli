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

namespace Pacioli.WindowsApp.NET8.Controls
{
    public partial class XmlViewPanel : UserControl
    {
        public XmlViewPanel()
        {
            InitializeComponent();
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
                var rootNode = new TreeNode(root.Name);
                F_XmlTreeView.Nodes.Add(rootNode);
                //LoadXmlNode(root, rootNode);
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
            StringBuilder nodeText = new StringBuilder(xmlNode.Name);

            string? text = GetTextContentFull(xmlNode);

            if (text != null)
            {
                nodeText.Append(text);
                return new TreeNode(nodeText.ToString());
            }
            else
            {
                var tn = new TreeNode(nodeText.ToString());
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    TreeNode n = ConstructNode(node);
                    tn.Nodes.Add(n);
                }
                return tn;
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

        private string? GetTextContentFull(XmlNode xmlNode)
        {
            bool isText = false;
            StringBuilder content = new StringBuilder();

            //if (!string.IsNullOrWhiteSpace(xmlNode.InnerText))
            //{
            //    content.Append($" {xmlNode.InnerText}");
            //    if (xmlNode.Attributes != null)
            //    {
            //        foreach (XmlAttribute attr in xmlNode.Attributes)
            //        {
            //            content.Append($" @{attr.Name}={attr.Value}");
            //        }
            //    }
            //}

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!string.IsNullOrWhiteSpace(childNode.InnerText) && childNode.ChildNodes.Count == 0)
                {
                    content.Append($" {childNode.InnerText}");
                    if (xmlNode.Attributes != null)
                    {
                        foreach (XmlAttribute attr in xmlNode.Attributes)
                        {
                            content.Append($" @{attr.Name}={attr.Value}");
                        }
                    }
                    isText = true;
                }
            }
            return isText ? content.ToString() : null;
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
