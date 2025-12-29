using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Pacioli.Preview.ImageGeneration
{
    public class PdfToImageConverter
    {
        public byte[]? PdfData = null;
        public int PageCount { get; set; } = 0;
        public SizeF PageSize { get; set; }

        IEnumerable<SKBitmap> Bitmaps { get; set; }
        IEnumerable<Image> FastBitmaps { get; set; }


        public PdfToImageConverter(string pdfFile)
        {
            PdfData = File.ReadAllBytes(pdfFile);
            PageSize = PDFtoImage.Conversion.GetPageSize(PdfData, 0);
            PageCount = PDFtoImage.Conversion.GetPageCount(PdfData);
            Bitmaps = PDFtoImage.Conversion.ToImages(PdfData!);
            FastBitmaps = Bitmaps.Select(x => Bitmap.FromStream(x.Encode(SKEncodedImageFormat.Png, 80).AsStream()));
        }

        public void Convert(string imgFile)
        {
            PDFtoImage.Conversion.SavePng(imgFile, PdfData!, page: 0);
        }

        public Stream ConvertToStream(ref int pageNo)
        {
            if (pageNo < 0)
            {
                pageNo = 0;
            }
            if (pageNo > PageCount - 1)
            {
                pageNo = PageCount - 1;
            }
            //PDFtoImage.Conversion.SavePng(imgFile, PdfData!, page: pageNo);
            SKData skData = Bitmaps.ElementAt(pageNo).Encode(SKEncodedImageFormat.Png, 100);
            return skData.AsStream();
        }

        public Image GetPage(ref int pageNo)
        {
            if (pageNo < 0)
            {
                pageNo = 0;
            }
            if (pageNo > PageCount - 1)
            {
                pageNo = PageCount - 1;
            }
            return FastBitmaps.ElementAt(pageNo);
        }
    }
}
