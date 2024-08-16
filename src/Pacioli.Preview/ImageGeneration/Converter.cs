using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Pacioli.Preview.ImageGeneration
{
    public class Converter
    {
        public byte[]? PdfData = null;
        public int PageCount { get; set; } = 0;
        public SizeF PageSize { get; set; }


        public Converter(string pdfFile)
        {
            PdfData = File.ReadAllBytes(pdfFile);
            PageSize = PDFtoImage.Conversion.GetPageSize(PdfData, 0);
            PageCount = PDFtoImage.Conversion.GetPageCount(PdfData);
        }

        public void Convert(string pdfFile, string imgFile)
        {
            PDFtoImage.Conversion.SavePng(imgFile, PdfData!, page: 0);
        }

        public int Convert(string pdfFile, string imgFile, int pageNo)
        {
            if (pageNo < 0)
            {
                pageNo = 0;
            }
            if (pageNo > PageCount - 1)
            {
                pageNo = PageCount - 1;
            }
            PDFtoImage.Conversion.SavePng(imgFile, PdfData!, page: pageNo);
            return pageNo;
        }
    }
}
