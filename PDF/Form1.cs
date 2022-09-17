using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string filtro = txtFiltro.Text;
            creaarPdf(filtro);
            axAcroPDF.src = "E:/proyectos visual/PDF/PDF/bin/Debug/ReporteProducto.pdf";

        }
        private void creaarPdf(string filtro)
        {
            PdfWriter pdfWriter = new PdfWriter("reporte.pdf");
            PdfDocument pdf = new PdfDocument(pdfWriter);
            // esto es tamaño carta  Document documento = new Document(pdf, PageSize.LETTER);//
            PageSize tamanioH = new PageSize(792, 612);
            Document documento = new Document(pdf, tamanioH);

            documento.SetMargins(60, 20, 55, 20);
            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            string[] columnas = {"Codigo","Nombre","Precio","Existencias","Categoria"};

            float[] tamanios = {2, 4, 2, 2, 4 };

            Table tabla = new Table(UnitValue.CreatePercentArray(tamanios));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            foreach (string columna in columnas)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }

            string sql = "SELECT p.codigo, p.nombre, p.precio_publico, p.existencias, c.nombre as categoria FROM productos AS p INNER JOIN categorias AS c ON p.idCategoria=c.id WHERE c.nombre='" + filtro +"'";
            MySqlConnection conexionBD = conexion.Conexion();
            conexionBD.Open();

            MySqlCommand comando = new MySqlCommand(sql, conexionBD);
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                for (int x = 1; x < 100; x++)
                {
                   tabla.AddCell(new Cell().Add(new Paragraph(reader["codigo"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(reader["nombre"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(reader["precio_publico"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(reader["existencias"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(reader["categoria"].ToString()).SetFont(fontContenido)));
           
                }
            }

            documento.Add(tabla);
            documento.Close();

            var logo = new iText.Layout.Element.Image(ImageDataFactory.Create("C:/Users/jose/Pictures/Saved Pictures/Captura.png")).SetWidth(50);
            var plogo = new Paragraph("").Add(logo);

            var titulo = new Paragraph("Reporte de productos");
            titulo.SetTextAlignment(TextAlignment.CENTER);
            titulo.SetFontSize(12);

            var dfecha =  DateTime.Now.ToString("dd-MM-yyyy");
            var dhora = DateTime.Now.ToString("hh:mm:ss");
            var fecha = new Paragraph("Fecha" + dfecha + "\nHora" + dhora);
            fecha.SetFontSize(12);

            PdfDocument pdfDoc = new PdfDocument(new PdfReader("Reporte.pdf"), new PdfWriter("ReporteProducto.pdf"));
            Document doc = new Document(pdfDoc);

            int numeros = pdfDoc.GetNumberOfPages();

            for (int i = 1; i <= numeros; i++)
            {
                PdfPage pagina = pdfDoc.GetPage(i);

                float y = (pdfDoc.GetPage(i).GetPageSize().GetTop() - 15);
                doc.ShowTextAligned(plogo, 40, y, i, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
                doc.ShowTextAligned(titulo, 150, y - 15, i, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
                doc.ShowTextAligned(fecha, 520, y - 15, i, TextAlignment.CENTER, VerticalAlignment.TOP, 0);

                doc.ShowTextAligned(new Paragraph(String.Format("Pagina {0} de {1}", i, numeros)), pdfDoc.GetPage(i).GetPageSize().GetWidth() / 2, pdfDoc.GetPage(i).GetPageSize().GetBottom() + 30, i, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
            }

            doc.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
