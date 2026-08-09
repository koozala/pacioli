using iText.Kernel.Pdf;
using iText.Svg.Converter;
using SkiaSharp;
using System.Drawing;
using System.Reflection;

namespace Pacioli.WindowsApp.Util
{
    /*
     * Makes a bitmap of a picture that comes in SVG form.
     *
     * Windows Forms draws bitmaps only, thus an SVG picture needs a conversion. The
     * conversion has two steps, and each step uses a library that the application has
     * already: iText makes a PDF page of the SVG, and PDFtoImage draws that page into a
     * bitmap. Thus the application needs no other package.
     *
     * The second step draws the vectors again for each size, thus the picture keeps its
     * sharpness on a screen of high resolution. The class keeps the last bitmap, because
     * a control asks for the same size at each paint.
     */
    internal sealed class SvgImage : IDisposable
    {
        /* The SVG as a PDF of one page. The first step makes it one time. */
        private readonly byte[] page;

        /* The last bitmap and its size. A new size makes a new bitmap. */
        private Bitmap? bitmap;
        private int bitmapWidth;
        private int bitmapHeight;

        private SvgImage(byte[] page, float pageWidth, float pageHeight)
        {
            this.page = page;
            Width = pageWidth;
            Height = pageHeight;
        }

        /* The size of the picture. Only the relation of the two values is of interest. */
        public float Width { get; }

        public float Height { get; }

        /*
         * Reads an SVG that is part of the assembly.
         *
         * Gives null if the resource is absent or if the SVG has an error. The picture is
         * an decoration, thus a problem must not stop the application.
         */
        public static SvgImage? FromResource(string name)
        {
            try
            {
                using Stream? svg = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                if (svg == null)
                {
                    return null;
                }

                using MemoryStream pdf = new MemoryStream();
                SvgConverter.CreatePdf(svg, pdf);

                byte[] data = pdf.ToArray();

                using PdfDocument doc = new PdfDocument(new PdfReader(new MemoryStream(data)));
                iText.Kernel.Geom.Rectangle box = doc.GetFirstPage().GetMediaBox();

                if (box.GetWidth() <= 0.0f || box.GetHeight() <= 0.0f)
                {
                    return null;
                }

                return new SvgImage(data, box.GetWidth(), box.GetHeight());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
         * Gives the picture as a bitmap of the given size in pixels.
         *
         * Gives null if the conversion fails. The caller then draws nothing.
         */
        public Bitmap? Render(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return null;
            }

            if (bitmap != null && bitmapWidth == width && bitmapHeight == height)
            {
                return bitmap;
            }

            try
            {
                /*
                 * The background stays free, thus the colour of the control shows through
                 * the parts that the picture does not cover.
                 */
                PDFtoImage.RenderOptions options = new PDFtoImage.RenderOptions(
                    Width: width,
                    Height: height,
                    WithAspectRatio: true,
                    BackgroundColor: SKColors.Transparent);

                using SKBitmap drawn = PDFtoImage.Conversion.ToImage(page, options: options);
                using MemoryStream png = new MemoryStream();

                drawn.Encode(png, SKEncodedImageFormat.Png, 100);
                png.Position = 0;

                Bitmap next = new Bitmap(png);

                bitmap?.Dispose();
                bitmap = next;
                bitmapWidth = width;
                bitmapHeight = height;

                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            bitmap?.Dispose();
            bitmap = null;
        }
    }
}
