using Pacioli.Language.Resources;
using Pacioli.WindowsApp.Util;

namespace Pacioli.WindowsApp.Controls
{
    /*
     * Shows the logo of the application in the middle of an empty screen, and above it a
     * text that tells the user what to do.
     *
     * The user sees this panel before the first document is open. The panel keeps the
     * relation of the two sides of the logo, and it makes the logo again at each new size,
     * thus the lines stay sharp. The text and the logo make one group, and that group sits
     * in the middle of the panel.
     */
    internal sealed class LogoPanel : Panel
    {
        /* The name that the project gives to the SVG in the assembly. */
        public const string LogoResourceName = "Pacioli.WindowsApp.Assets.Logo.svg";

        /* The logo covers this part of the smaller side of the panel. */
        private const float Part = 0.45f;

        /* A logo below the first value stays away, a logo above the second one stops to grow. */
        private const int SmallestSide = 64;
        private const int LargestSide = 640;

        /* The text is larger than the text of the other controls, thus the user sees it. */
        private const float TextSize = 1.5f;

        /* The text keeps this part of the width free at each side. */
        private const float TextMargin = 0.1f;

        private const TextFormatFlags TextLayout =
            TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;

        private readonly SvgImage? logo;

        private Font? textFont;

        public LogoPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = SystemColors.Window;

            logo = SvgImage.FromResource(LogoResourceName);
        }

        private string Hint => Resources.DateiHierAblegen;

        private Font TextFont => textFont ??= new Font(Font.FontFamily, Font.Size * TextSize, FontStyle.Regular);

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            textFont?.Dispose();
            textFont = null;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            /*
             * The text keeps its place even if the logo is absent, because the text tells
             * the user what to do and the logo does not.
             */
            int textWidth = Math.Max(1, Width - (int)(Width * TextMargin * 2.0f));
            Size textSize = TextRenderer.MeasureText(Hint, TextFont, new Size(textWidth, int.MaxValue), TextLayout);

            Size logoSize = LogoSize();
            Bitmap? picture = logoSize.Width > 0 ? logo?.Render(logoSize.Width, logoSize.Height) : null;

            if (picture == null)
            {
                logoSize = Size.Empty;
            }

            /* The space between the text and the logo has the height of one line. */
            int gap = picture == null ? 0 : textSize.Height;
            int top = (Height - textSize.Height - gap - logoSize.Height) / 2;

            TextRenderer.DrawText(
                e.Graphics,
                Hint,
                TextFont,
                new Rectangle((Width - textSize.Width) / 2, top, textSize.Width, textSize.Height),
                SystemColors.GrayText,
                TextLayout);

            if (picture != null)
            {
                e.Graphics.DrawImage(
                    picture,
                    (Width - logoSize.Width) / 2,
                    top + textSize.Height + gap,
                    logoSize.Width,
                    logoSize.Height);
            }
        }

        /*
         * Gives the size of the logo for the size of the panel.
         *
         * The logo fits in a square box of a part of the smaller side. In the box, the
         * logo keeps the relation of its own two sides.
         */
        private Size LogoSize()
        {
            if (logo == null)
            {
                return Size.Empty;
            }

            int side = (int)(Math.Min(Width, Height) * Part);

            if (side < SmallestSide)
            {
                return Size.Empty;
            }

            side = Math.Min(side, LargestSide);

            float relation = logo.Width / logo.Height;

            return relation >= 1.0f
                ? new Size(side, Math.Max(1, (int)(side / relation)))
                : new Size(Math.Max(1, (int)(side * relation)), side);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                logo?.Dispose();
                textFont?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
