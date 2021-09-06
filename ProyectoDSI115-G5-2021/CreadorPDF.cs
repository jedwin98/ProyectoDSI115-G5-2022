using iTextSharp.text;

using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProyectoDSI115_G5_2021
{
    class CreadorPDF
    {
        public CreadorPDF(DataTable dataTable, string tipoReporte)
        {
            CrearPDF(dataTable, tipoReporte);
        }

        public CreadorPDF()
        {
            
        }


        public void CrearPDF(DataTable dataTable, string tipoReporte)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivo Pdf |*.pdf";
            if (sfd.ShowDialog() == true)
            {


                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                Document document = new Document();
                document.SetPageSize(iTextSharp.text.PageSize.LETTER);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                //Cabecera del reporte
                BaseFont baseFontHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontHead = new iTextSharp.text.Font(baseFontHead, 18, 1, BaseColor.BLACK);
                iTextSharp.text.Paragraph empresa = new iTextSharp.text.Paragraph();
                empresa.Alignment = Element.ALIGN_CENTER;
                empresa.Add(new Chunk("Fuego y Seguridad Industrial", fontHead));
                document.Add(empresa);

                //Datos generales del reporte
                BaseFont baseFontData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontHeadData = new iTextSharp.text.Font(baseFontHead, 14, 1, BaseColor.BLACK);
                iTextSharp.text.Paragraph datos = new iTextSharp.text.Paragraph();
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
            }
           // MessageBox.Show("Reporte creado en: " + direccion, "Resultado de la creacion del reporte", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public void PrepararImpresionSolicitud(DataTable dataTable, string nombreSolicitante, string autorizador, string cliente, string empresaR, string codigoSolicitud, string numeroOrden, string fecha)
        {
            /*
            string path = "C:/FYSIEX/Solicitudes/" + conv.Year + "/" + conv.Month;
            DirectoryInfo di = Directory.CreateDirectory(path);
            FileStream fs = new FileStream(path + "/Solicitud" + codigoSolicitud + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Cabecera de la solicictud
            BaseFont baseFontHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontHead = new iTextSharp.text.Font(baseFontHead, 18, 1, BaseColor.BLACK);
            Paragraph empresa = new Paragraph();
            empresa.Alignment = Element.ALIGN_CENTER;
            empresa.Add(new Chunk("Fuego y Seguridad Industrial\nComprobante de autorización de extracción", fontHead));
            document.Add(empresa);

            //Datos generales de la solicitud
            BaseFont baseFontData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontHeadData = new iTextSharp.text.Font(baseFontHead, 14, 1, BaseColor.BLACK);
            document.Add(new Chunk("\n", fontHead));

            PdfPTable tableDatos = new PdfPTable(2);
            PdfPCell cellDatos = new PdfPCell();
            cellDatos.AddElement(new Chunk("Fecha de ingreso: " + fecha, fontHeadData));
            tableDatos.AddCell(cellDatos);
            cellDatos = new PdfPCell();
            cellDatos.AddElement(new Chunk("Orden #" + numeroOrden, fontHeadData));
            tableDatos.AddCell(cellDatos);
            cellDatos = new PdfPCell();
            cellDatos.AddElement(new Chunk("Representante: " + cliente, fontHeadData));
            tableDatos.AddCell(cellDatos);
            cellDatos = new PdfPCell();
            cellDatos.AddElement(new Chunk("Razón social: " + empresaR, fontHeadData));
            tableDatos.AddCell(cellDatos);
            document.Add(tableDatos);
            document.Add(new Chunk("\n", fontHead));

            //Detalles de la solicitud
            PdfPTable tableDetalle = new PdfPTable(dataTable.Columns.Count);
            BaseFont baseFontColumns = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontColumns = new iTextSharp.text.Font(baseFontColumns, 10, 1, BaseColor.BLACK);
            //Cabecera de los detalles
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.AddElement(new Chunk(dataTable.Columns[i].ColumnName, fontColumns));
                tableDetalle.AddCell(cell);
            }
            //Lista de detalles
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    tableDetalle.AddCell(dataTable.Rows[i][j].ToString());
                }
            }
            document.Add(tableDetalle);
            document.Add(new Chunk("\n", fontHead));//inserta un salto de linea antes de imprimir tabla

            //Espacio de firmas
            iTextSharp.text.Font fontDetalles = new iTextSharp.text.Font(baseFontColumns, 12, 1, BaseColor.BLACK);
            PdfPTable tableFirmas = new PdfPTable(2);
            PdfPCell cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk("Firma de solicitante", fontDetalles));
            tableFirmas.AddCell(cellFirmas);
            cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk("Firma de autorizador", fontDetalles));
            tableFirmas.AddCell(cellFirmas);
            cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk(nombreSolicitante, fontDetalles));
            tableFirmas.AddCell(cellFirmas);
            cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk(autorizador, fontDetalles));
            tableFirmas.AddCell(cellFirmas);
            iTextSharp.text.Font fontFirmas = new iTextSharp.text.Font(baseFontHead, 24, 1, BaseColor.BLACK);
            cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk(" ", fontFirmas));
            tableFirmas.AddCell(cellFirmas);
            cellFirmas = new PdfPCell();
            cellFirmas.AddElement(new Chunk(" ", fontFirmas));
            tableFirmas.AddCell(cellFirmas);
            document.Add(tableFirmas);
            //Llamar ventana de impresión
            
            document.Close();
            writer.Close();
            fs.Close();
            */
            FlowDocument fd = new FlowDocument();
            System.Windows.Documents.Paragraph p = new System.Windows.Documents.Paragraph(new Run("Fuego y Seguridad Industrial\n"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Center;
            fd.Blocks.Add(p);

            // Construcción de tabla de cabecera
            Table tableCabecera = new Table();
            fd.Blocks.Add(tableCabecera);
            tableCabecera.Background = System.Windows.Media.Brushes.White;
            for (int i = 0; i < 2; i++)
            {
                tableCabecera.Columns.Add(new TableColumn());
            }
            // Información de orden
            tableCabecera.RowGroups.Add(new TableRowGroup());
            tableCabecera.RowGroups[0].Rows.Add(new TableRow());
            TableRow actual = tableCabecera.RowGroups[0].Rows[0];
            actual.FontSize = 14;
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Fecha de ingreso: " + fecha))));
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Orden #" + numeroOrden))));
            // Información de cliente y razón social
            tableCabecera.RowGroups[0].Rows.Add(new TableRow());
            actual = tableCabecera.RowGroups[0].Rows[1];
            actual.FontSize = 14;
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Representante: " + cliente))));
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Razón social: " + empresaR))));
            // Salto de línea
            p = new System.Windows.Documents.Paragraph(new Run("\n"));
            fd.Blocks.Add(p);

            // Tabla de detalles
            Table tableDetalles = new Table();
            fd.Blocks.Add(tableDetalles);
            tableDetalles.Background = System.Windows.Media.Brushes.White;
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                tableDetalles.Columns.Add(new TableColumn());
            }
            tableDetalles.RowGroups.Add(new TableRowGroup());
            // Creación de encabezado
            tableDetalles.RowGroups[0].Rows.Add(new TableRow());
            actual = tableDetalles.RowGroups[0].Rows[0];
            actual.FontSize = 12;
            actual.FontWeight = FontWeights.Bold;
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run(dataTable.Columns[i].ColumnName))));
            }
            // Agregando información
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                tableDetalles.RowGroups[0].Rows.Add(new TableRow());
                actual = tableDetalles.RowGroups[0].Rows[i+1];
                actual.FontSize = 12;
                actual.FontWeight = FontWeights.Regular;
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run(dataTable.Rows[i][j].ToString()))));
                }
            }
            // Salto de línea
            p = new System.Windows.Documents.Paragraph(new Run("\n"));
            fd.Blocks.Add(p);

            // Tabla de firmas
            Table tableFirmas = new Table();
            fd.Blocks.Add(tableFirmas);
            tableFirmas.Background = System.Windows.Media.Brushes.White;
            for (int i = 0; i < 2; i++)
            {
                tableFirmas.Columns.Add(new TableColumn());
            }
            tableFirmas.RowGroups.Add(new TableRowGroup());
            // Cabecera
            tableFirmas.RowGroups[0].Rows.Add(new TableRow());
            actual = tableFirmas.RowGroups[0].Rows[0];
            actual.FontSize = 14;
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Firma de solicitante"))));
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("Firma de autorizador"))));
            // Nombre de empleados
            tableFirmas.RowGroups[0].Rows.Add(new TableRow());
            actual = tableFirmas.RowGroups[0].Rows[1];
            actual.FontSize = 14;
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run(nombreSolicitante))));
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run(autorizador))));
            //Espacio de firmas
            tableFirmas.RowGroups[0].Rows.Add(new TableRow());
            actual = tableFirmas.RowGroups[0].Rows[2];
            actual.FontSize = 20;
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("________________"))));
            actual.Cells.Add(new TableCell(new System.Windows.Documents.Paragraph(new Run("________________"))));
            // Impresión del documento
            MemoryStream s = new System.IO.MemoryStream();
            TextRange src = new TextRange(fd.ContentStart, fd.ContentEnd);
            src.Save(s, DataFormats.Xaml);
            FlowDocument copy = new FlowDocument();
            TextRange dst = new TextRange(copy.ContentStart, copy.ContentEnd);
            dst.Load(s, DataFormats.Xaml);
            PrintDialog printDialog = new PrintDialog();
            IDocumentPaginatorSource idpSrc = copy;
            copy.PageHeight = printDialog.PrintableAreaHeight;
            copy.PageWidth = printDialog.PrintableAreaWidth;
            copy.PagePadding = new Thickness(50);
            copy.ColumnGap = 0;
            copy.ColumnWidth = printDialog.PrintableAreaWidth;
            copy.Name = "Solicitud" + codigoSolicitud;
            if (printDialog.ShowDialog() ?? false)
            {
                // Llamar a PrintDocument para la ventana de impresión
                printDialog.PrintDocument(idpSrc.DocumentPaginator, "Impresión de solicitud");
            }
        }
    }
}
