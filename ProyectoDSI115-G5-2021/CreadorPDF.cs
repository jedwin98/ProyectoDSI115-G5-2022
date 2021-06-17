using iTextSharp.text;

using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProyectoDSI115_G5_2021
{
    class CreadorPDF
    {
        public CreadorPDF(DataTable dataTable, string direccion, string tipoReporte)
        {
            CrearPDF(dataTable, direccion, tipoReporte);
        }

        public void CrearPDF(DataTable dataTable, string direccion, string tipoReporte)
        {
            FileStream fs = new FileStream(direccion, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Cabecera del reporte
            BaseFont baseFontHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontHead = new iTextSharp.text.Font(baseFontHead, 18, 1, BaseColor.BLACK);
            Paragraph empresa = new Paragraph();
            empresa.Alignment = Element.ALIGN_CENTER;
            empresa.Add(new Chunk("Fuego y Seguridad Industrial", fontHead));
            document.Add(empresa);

            //Datos generales del reporte
            BaseFont baseFontData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontHeadData = new iTextSharp.text.Font(baseFontHead, 14, 1, BaseColor.BLACK);
            Paragraph datos = new Paragraph();
            datos.Alignment = Element.ALIGN_CENTER;
            datos.Add(new Chunk("\nReporte de " + tipoReporte, fontHeadData));
            datos.Add(new Chunk("\nFecha: " + DateTime.Now.ToShortDateString(), fontHeadData));
            document.Add(datos);
            document.Add(new Chunk("\n", fontHead));//inserta un salto de linea antes de imprimir tabla

            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            BaseFont baseFontColumns = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontColumns = new iTextSharp.text.Font(baseFontColumns, 10, 1, BaseColor.BLACK);
            //agregando la cabecera de la tabla
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                
                cell.AddElement(new Chunk(dataTable.Columns[i].ColumnName, fontColumns));
                table.AddCell(cell);
            }
            //agregando la información de la tabla al documento

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {

                    table.AddCell(dataTable.Rows[i][j].ToString());
                }

            }
            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
             
           // MessageBox.Show("Reporte creado en: " + direccion, "Resultado de la creacion del reporte", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }

}
