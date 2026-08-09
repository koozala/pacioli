using Pacioli.WindowsApp.Util;

namespace Pacioli.WindowsApp.Controls
{
    /*
     * Shows the logo of the application in the middle of an empty screen.
     *
     * The user sees this panel before the first document is open. The panel keeps the
     * relation of the two sides of the logo, and it makes the logo again at each new size,
     * thus the lines stay sharp.
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

        private readonly SvgImage? logo;

        public LogoPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = SystemColors.Window;

            logo = SvgImage.FromResource(LogoResourceName);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (logo == null)
            {
                return;
            }

            Size size = LogoSize();
            if (size.Width <= 0 || size.Height <= 0)
            {
                return;
            }

            Bitmap? picture = logo.Render(size.Width, size.Height);
            if (picture == null)
            {
                return;
            }

            e.Graphics.DrawImage(
                picture,
                (Width - size.Width) / 2,
                (Height - size.Height) / 2,
                size.Width,
                size.Height);
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
            }

            base.Dispose(disposing);
        }
    }
}
