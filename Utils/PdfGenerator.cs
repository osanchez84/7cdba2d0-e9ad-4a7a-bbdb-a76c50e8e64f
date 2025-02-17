﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using iText = iTextSharp.text;
//using Microsoft.Extensions.Hosting;

namespace GuanajuatoAdminUsuarios.Utils
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PdfGenerator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }

        public (MemoryStream, string) CreatePdf<T>(string NamePdf, string Title, int SizeColumns, Dictionary<string, string> ColumnsNames, List<T> ModelData)
        {
            try
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");
                DateTime dTime = DateTime.Now;

                string strPDFFileName = string.Format(NamePdf + "-" + dTime.ToString("ddMMyyyy") + ".pdf");

                iText.Document doc = new iText.Document();
                doc.SetMargins(10, 10, 20, 10);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                #region Header
                PdfPTable tableHeader = new PdfPTable(2);
                tableHeader.DefaultCell.Border = iText.Rectangle.NO_BORDER;
                float[] headers = new float[] { 30, 60 };
                tableHeader.SetWidths(headers);
                tableHeader.WidthPercentage = 100;

                var path = Path.Combine(_hostingEnvironment.WebRootPath, "img", "logo-gto.png");
                iText.Image image = iText.Image.GetInstance(path);
                image.ScaleToFit(150f, 150f);
                image.ScaleToFitHeight = false;
                tableHeader.AddCell(new PdfPCell(image)).Border = 0;

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iText.Font fontInvoice = new iText.Font(bf, 20, iText.Font.NORMAL);
                tableHeader.AddCell(new PdfPCell(new iText.Phrase(Title, fontInvoice))).Border = 0;

                doc.Add(tableHeader);

                #endregion

                doc.Add(new Paragraph("\n"));

                //iText.Paragraph p3 = new iText.Paragraph();
                //p3.SpacingAfter = 6;
                //doc.Add(p3);

                PdfPTable tableLayout = new PdfPTable(SizeColumns);
                doc.Add(Add_Content_To_PDF(tableLayout, ModelData, ColumnsNames, SizeColumns));

                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return (workStream, strPDFFileName);
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }

        public (MemoryStream, string) CreatePdf<T>(string NamePdf, string Title, int SizeColumns, Dictionary<string, string> ColumnsNames, T ModelData)
        {
            try
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");
                DateTime dTime = DateTime.Now;

                string strPDFFileName = string.Format(NamePdf + "-" + dTime.ToString("ddMMyyyy") + ".pdf");

                iText.Document doc = new iText.Document();
                doc.SetMargins(10, 10, 20, 10);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                #region Header
                PdfPTable tableHeader = new PdfPTable(2);
                tableHeader.DefaultCell.Border = iText.Rectangle.NO_BORDER;
                float[] headers = new float[] { 30, 60 };
                tableHeader.SetWidths(headers);
                tableHeader.WidthPercentage = 100;

                var path = Path.Combine(_hostingEnvironment.WebRootPath, "img", "logo-gto.png");
                iText.Image image = iText.Image.GetInstance(path);
                image.ScaleToFit(150f, 150f);
                image.ScaleToFitHeight = false;
                tableHeader.AddCell(new PdfPCell(image)).Border = 0;

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iText.Font fontInvoice = new iText.Font(bf, 20, iText.Font.NORMAL);
                tableHeader.AddCell(new PdfPCell(new iText.Phrase(Title, fontInvoice))).Border = 0;

                doc.Add(tableHeader);

                #endregion

                doc.Add(new Paragraph("\n"));

                //iText.Paragraph p3 = new iText.Paragraph();
                //p3.SpacingAfter = 6;
                //doc.Add(p3);

                PdfPTable tableLayout = new PdfPTable(SizeColumns);
                doc.Add(Add_Content_To_PDF(tableLayout, ModelData, ColumnsNames, SizeColumns));

                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return (workStream, strPDFFileName);
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }

        private PdfPTable Add_Content_To_PDF<T>(PdfPTable tableLayout, T ModelData, Dictionary<string, string> ColumnsNames, int size)
        {
            if (ColumnsNames.Count == size)
            {

                float[] headers = new float[(size)];
                var porcentHeader = (float)100 / size;
                for (int i = 0; i < size; i++)
                {
                    headers[i] = porcentHeader;
                }

                tableLayout.SetWidths(headers);
                tableLayout.WidthPercentage = 100;
                tableLayout.HeaderRows = 1;
                var count = 1;

                //Agrega nombre de columnas
                foreach (var column in ColumnsNames)
                {
                    AddCellToTitleHeader(tableLayout, column.Value);
                }

                Type type = typeof(T);


                if (count >= 1)
                {
                    foreach (var item in ColumnsNames)
                    {
                        PropertyInfo property = type.GetProperty(item.Key);
                        var value = property.GetValue(ModelData);
                        AddCellToBody(tableLayout, count, Convert.ToString(value));
                    }
                    count++;
                }
                return tableLayout;
            }
            return null;
        }

        private PdfPTable Add_Content_To_PDF<T>(PdfPTable tableLayout, List<T> ModelData, Dictionary<string, string> ColumnsNames, int size)
        {
            if (ColumnsNames.Count == size)
            {

                float[] headers = new float[(size)];
                var porcentHeader = (float)100 / size;
                for (int i = 0; i < size; i++)
                {
                    headers[i] = porcentHeader;
                }

                tableLayout.SetWidths(headers);
                tableLayout.WidthPercentage = 100;
                tableLayout.HeaderRows = 1;
                var count = 1;

                //Agrega nombre de columnas
                foreach (var column in ColumnsNames)
                {
                    AddCellToTitleHeader(tableLayout, column.Value);
                }

                Type type = typeof(T);

                #region Comment
                //Busqueda de 
                //List<string> NameHeaders = new List<string>();
                //PropertyInfo[] propertyInfos = type.GetProperties();
                //foreach (PropertyInfo item in propertyInfos)
                //{
                //    var name = item.Name;
                //    var findPropertyBool = ColumnsNames.ContainsKey(name);
                //    //var findPropertyBool = Array.Find(ColumnsNames, n => n.Equals(name)) != null ? true : false;
                //    if (findPropertyBool)
                //    {
                //        NameHeaders.Add(name);
                //    }
                //}
                //si implemento la busqueda de propiedades tendria que ordenar la lista como viene la de ColumnsNames
                //NameHeaders.OrderBy(x => x.Equals(ColumnsNames.Keys));
                #endregion

                foreach (dynamic objectItem in ModelData)
                {
                    if (count >= 1)
                    {
                        foreach (var item in ColumnsNames)
                        {
							PropertyInfo pi = type.GetProperty(item.Key);
                            var value = pi.GetValue(objectItem);
                            AddCellToBody(tableLayout, count, Convert.ToString(
                                pi.PropertyType == typeof(DateTime?) ? string.Empty : 
                                pi.PropertyType == typeof(DateTime) ? (((DateTime)value) == DateTime.MinValue ? string.Empty : ((DateTime)value).ToString("dd-MM-yyyy")) : 
                                value ?? string.Empty));
                        }
                        count++;
                    }
                }
                return tableLayout;
            }
            return null;
        }


        private static void AddCellToTitleHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new iText.Phrase(cellText, new iText.Font(iText.Font.FontFamily.HELVETICA, 8, 1, iText.BaseColor.BLACK)))
            {
                HorizontalAlignment = iText.Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new iText.BaseColor(185, 200, 230)
            });
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new iText.Phrase(cellText, new iText.Font(iText.Font.FontFamily.HELVETICA, 8, 1, iText.BaseColor.BLACK)))
            {
                HorizontalAlignment = iText.Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new iText.BaseColor(255, 255, 255)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, int count, string cellText = null)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new iText.Phrase(cellText, new iText.Font(iText.Font.FontFamily.HELVETICA, 8, 1, iText.BaseColor.BLACK)))
                {
                    HorizontalAlignment = iText.Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iText.BaseColor(211, 211, 211)

                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new iText.Phrase(cellText, new iText.Font(iText.Font.FontFamily.HELVETICA, 8, 1, iText.BaseColor.BLACK)))
                {
                    HorizontalAlignment = iText.Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iText.BaseColor(255, 255, 255)
                });
            }
        }


        public byte[] CreatePDFByHTML(string html, string cssText, Rectangle pageSize)
        {
            byte[] pdf; // result will be here

            //var cssText = File.ReadAllText("");
            ////(MapPath("~/css/test.css"));
            //var html = File.ReadAllText("");
            //MapPath("~/css/test.html"));

            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(pageSize, 20, 20, 20, 20);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                {
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlMemoryStream, cssMemoryStream);
                    }
                }

                document.Close();

                pdf = memoryStream.ToArray();
            }

            return pdf;
        }

        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }
    }
}
