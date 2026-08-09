using System.Drawing;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.Controls
{
    /*
     * Draws a small cross in a box at the right side of each tab header, and tells the
     * parent which document the user wants to close.
     *
     * A tab control of Windows Forms draws its headers itself and gives no place for a
     * button of its own. Thus the control changes to OwnerDrawFixed, and this class draws
     * the name of the file and the icon, and looks at the place of each click.
     *
     * The sizes come from the font of the control. Thus the icon keeps its relation to the
     * text on a screen of high resolution, and it needs no value in pixels.
     */
    internal sealed class TabCloseIcon
    {
        private readonly TabControl tabs;

        /* The tab under the mouse. The icon of that tab gets a stronger colour. */
        private int hotIndex = -1;

        public TabCloseIcon(TabControl tabs)
        {
            this.tabs = tabs;

            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.SizeMode = TabSizeMode.Normal;

            /*
             * The padding gives the room for the icon. A tab control makes each tab as
             * wide as the text plus the padding at the left and at the right. The value
             * covers the icon and the two spaces at its sides, thus the name of the file
             * keeps its full length.
             */
            tabs.Padding = new Point(IconSide + (Gap * 2), tabs.Padding.Y);

            tabs.DrawItem += OnDrawItem;
            tabs.MouseDown += OnMouseDown;
            tabs.MouseMove += OnMouseMove;
            tabs.MouseLeave += OnMouseLeave;
        }

        /* Tells that the user asks to close the page. */
        public event EventHandler<TabPage>? CloseRequested;

        private int IconSide => (int)Math.Round(tabs.Font.Height * 0.85);

        private int Gap => Math.Max(2, IconSide / 4);

        private Rectangle IconBounds(Rectangle tab)
        {
            int side = IconSide;

            return new Rectangle(
                tab.Right - side - Gap,
                tab.Top + ((tab.Height - side) / 2),
                side,
                side);
        }

        private void OnDrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tabs.TabPages.Count)
            {
                return;
            }

            TabPage page = tabs.TabPages[e.Index];
            Rectangle tab = tabs.GetTabRect(e.Index);
            bool selected = e.Index == tabs.SelectedIndex;

            using (SolidBrush back = new SolidBrush(selected ? SystemColors.Window : SystemColors.Control))
            {
                e.Graphics.FillRectangle(back, tab);
            }

            Rectangle icon = IconBounds(tab);

            /*
             * The text starts at half of the padding. The other half stays free, thus the
             * name of the file has room even if the measure of the tab control and the
             * measure of the drawing are not exactly the same.
             */
            Rectangle text = Rectangle.FromLTRB(
                tab.Left + (tabs.Padding.X / 2),
                tab.Top,
                icon.Left - Gap,
                tab.Bottom);

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                tabs.Font,
                text,
                SystemColors.ControlText,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
                    | TextFormatFlags.NoPadding);

            DrawCrossInBox(e.Graphics, icon, e.Index == hotIndex);
        }

        /*
         * Draws the box and the cross. The cross keeps away from the box, thus the two
         * forms stay apart at a small size.
         */
        private static void DrawCrossInBox(Graphics g, Rectangle icon, bool hot)
        {
            Color colour = hot ? SystemColors.ControlText : SystemColors.ControlDark;

            if (hot)
            {
                using SolidBrush back = new SolidBrush(SystemColors.ControlLight);
                g.FillRectangle(back, icon);
            }

            using Pen pen = new Pen(colour);

            g.DrawRectangle(pen, icon.X, icon.Y, icon.Width - 1, icon.Height - 1);

            int inset = Math.Max(3, icon.Width / 4);

            int left = icon.Left + inset;
            int right = icon.Right - 1 - inset;
            int top = icon.Top + inset;
            int bottom = icon.Bottom - 1 - inset;

            g.DrawLine(pen, left, top, right, bottom);
            g.DrawLine(pen, right, top, left, bottom);
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            int index = IndexOfIconAt(e.Location);
            if (index < 0)
            {
                return;
            }

            hotIndex = -1;
            CloseRequested?.Invoke(this, tabs.TabPages[index]);
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            int index = IndexOfIconAt(e.Location);

            if (index != hotIndex)
            {
                hotIndex = index;
                tabs.Invalidate();
            }
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            if (hotIndex >= 0)
            {
                hotIndex = -1;
                tabs.Invalidate();
            }
        }

        /* Gives the tab whose icon holds the point, or -1 if no icon holds it. */
        private int IndexOfIconAt(Point point)
        {
            for (int index = 0; index < tabs.TabPages.Count; index++)
            {
                if (IconBounds(tabs.GetTabRect(index)).Contains(point))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
