using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoDSI115_G5_2021.CotizacionRecibo
{
    /// <summary>
    /// Lógica de interacción para Cotizacion.xaml
    /// </summary>
    public partial class Cotizacion : Window
    {

        ControlBD control;
        DataTable dt = new DataTable();

        DataTable dataDetalle = new DataTable();
        List<DetalleCotizacion> detalles = new List<DetalleCotizacion>();

        private float totalCotizado = 0; //Variable global para guardar el total de lo cotizado

        public Cotizacion()
        {
            InitializeComponent();
            cargarTabla();
            labelFecha.Content = GenerarFecha();
            txtCodCotizacion.Text = GenerarCodigoCotizacion();
        }

        public void cargarTabla()
        {
            control = new ControlBD();
            dt = control.consultarInve();
            dataInventario.ItemsSource = dt.DefaultView;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BuscarProMat()
        {
            dt.Clear();
            dt = control.BuscarInve(txtBusqueda.Text);
            dataInventario.ItemsSource = dt.DefaultView;
        }

        private void BtnBusqueda_Click(object sender, RoutedEventArgs e)
        {
            BuscarProMat();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataInventario.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un producto", "Seleccione un producto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                txtNombreProducto.Text = row.Row.ItemArray[1].ToString() +", "+ row.Row.ItemArray[2].ToString();
                txtPrecio.Text = row.Row.ItemArray[3].ToString();
            }
        }

        private void DataInventario_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataRowView row = grid.SelectedItem as DataRowView;

                    txtNombreProducto.Text = row.Row.ItemArray[1].ToString() +", "+ row.Row.ItemArray[2].ToString();
                    txtPrecio.Text = row.Row.ItemArray[3].ToString();
                }
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCantidad.Text == "")
                {
                    MessageBox.Show("Debe ingresar un valor a la cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (Convert.ToSingle(txtCantidad.Text) == 0.0)
                    {
                        MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        if (Convert.ToSingle(txtCantidad.Text) < 0)
                        {
                            MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            DetalleCotizacion detalle = new DetalleCotizacion();
                            detalle.cantidad = Convert.ToSingle(txtCantidad.Text);
                            detalle.concepto = txtNombreProducto.Text;
                            detalle.precio = Convert.ToSingle(txtPrecio.Text);
                            detalle.subtotal = detalle.cantidad * detalle.precio;

                            detalles.Add(detalle);
                            dataCotizacion.ItemsSource = null;
                            dataCotizacion.ItemsSource = detalles;

                            txtCantidad.Text = "";
                            txtNombreProducto.Text = "";
                            txtPrecio.Text = "";

                            totalCotizado = totalCotizado + detalle.subtotal;
                            txtTotalCotizacion.Text = "$" + Convert.ToString(totalCotizado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Solo se permiten numeros en el campo cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCantidad.Text = "";
            }
        }

        private void BtnCancelarCot_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea cancelar cotización?. Se borrará todos los" +
                " datos de la cotización", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                txtCantidad.Text = "";
                txtCliente.Text = "";
                txtNombreProducto.Text = "";
                txtPrecio.Text = "";
                txtTotalCotizacion.Text = "";
                totalCotizado = 0;
                detalles.Clear();
                dataCotizacion.ItemsSource = null;
                dataCotizacion.ItemsSource = detalles;
            }
            else
            {

            }
        }

        public string GenerarFecha()
        {
            DateTime fecha = DateTime.Now;
            string anio = fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            string hora = fecha.Hour.ToString();
            string min = fecha.Minute.ToString();
            string seg = fecha.Second.ToString();

            return dia + "/" + mes + "/" + anio;
        }

        public string GenerarCodigoCotizacion()
        {
            DateTime fecha = DateTime.Now;
            string hora = fecha.Hour.ToString();
            string min = fecha.Minute.ToString();
            string seg = fecha.Second.ToString();
            string coti = "C";
            return coti + hora + min + seg;
        }

        private void BtnImprimirCot_Click(object sender, RoutedEventArgs e)
        {
            if (txtCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar un cliente a la cotización", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (txtCodCotizacion.Text == "")
                {
                    MessageBox.Show("Debe haber un código de cotización", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string fecha = labelFecha.Content.ToString();
                    string codigo = txtCodCotizacion.Text;
                    string cliente = txtCliente.Text;
                    float total = totalCotizado;

                    GenerarImpresion(fecha, codigo, cliente, total);
                    detalles.Clear();
                    dataCotizacion.ItemsSource = null;

                    txtCodCotizacion.Text = GenerarCodigoCotizacion();
                    txtCliente.Text = "";
                    txtTotalCotizacion.Text = "";

                    totalCotizado = 0;
                }
            }
        }

        private void GenerarImpresion(string fechaCot, string codigoCot, string clienteCot, float totalCot)
        {
            //Adecuar cabeceras de tablas
            DataTable aImprimir = new DataTable();
            aImprimir.Columns.Add("Cantidad");
            aImprimir.Columns.Add("Concepto");
            aImprimir.Columns.Add("Precio Unitario");
            aImprimir.Columns.Add("Subtotal");
            string[] descripcion = new string[4];
            for (int i=0; i < detalles.Count(); i++)
            {
                descripcion[0] = detalles[i].cantidad.ToString();
                descripcion[1] = detalles[i].concepto;
                descripcion[2] = detalles[i].precio.ToString();
                descripcion[3] = detalles[i].subtotal.ToString();

                //Agregando detalle a la tabla de impresión
                aImprimir.Rows.Add(new Object[] { descripcion[0], descripcion[1], descripcion[2], descripcion[3] });
            }
            CreadorPDF impresion = new CreadorPDF();
            impresion.PrepararImpresionCotizacion(aImprimir, fechaCot, codigoCot, clienteCot, totalCot);
        }
    }
}
