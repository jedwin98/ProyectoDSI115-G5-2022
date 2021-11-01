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
    public partial class CrearRecibo : Window
    {
        private GestionUsuarios.Usuario sesion;
        internal GestionUsuarios.Usuario Sesion { get => sesion; set => sesion = value; }
        ControlBD control;
        DataTable dt = new DataTable();
        DataTable dataTable = new DataTable();
        List<DetalleRecibo> detalles = new List<DetalleRecibo>();

        string codigoSolicitud { get; set; }
        private float totalTotal = 0;//Variable global para guardar el total de la compra del recibo
        private float existenciaSelected = 0;//Variable global para validar la existencia de la cantidad

        public CrearRecibo()
        {
            InitializeComponent();
            CargarTabla();
            txtFecha.Text = GenerarFecha();
            txtCodigoRecibo.Text = GenerarCodigoRecibo();
        }

        public void CargarTabla()
        {
            control = new ControlBD();
            dt = control.consultarMateriales();
            dt = control.consultarProductosRecibo();
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataMateriales.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un material", "Seleccione un material", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                txtNombre.Text = row.Row.ItemArray[1].ToString();
                txtPresentacion.Text = row.Row.ItemArray[2].ToString();
                existenciaSelected = float.Parse(row.Row.ItemArray[3].ToString());
                txtPrecio.Text = row.Row.ItemArray[4].ToString();
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarMaterial();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarMaterial();
            }
        }

        private void BuscarMaterial()
        {
            dt.Clear();
            dt = control.BuscarMatYProRecibo(txtBuscar.Text);
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPresentacion.Text = "";
            txtPrecio.Text = "";
            txtCliente.IsEnabled = false;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCliente.Text == "")//Verifica que llene el campo de cliente está vacio
                {
                    MessageBox.Show("Debe seleccionar un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (txtCantidad.Text == "")//Verifica si el campo de cantidad está vacia
                    {
                        MessageBox.Show("Debe ingresar un valor a la cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        if (Convert.ToSingle(txtCantidad.Text) == 0.0)//Verifica que han ingresado valor CERO en el campo cantidad
                        {
                            MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            if (Convert.ToSingle(txtCantidad.Text) < 0)//Verifica que ingresado un valor MENOR a CERO en el campo cantidad
                            {
                                MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                txtCantidad.Text = "";
                            }
                            else
                            {
                                if (Convert.ToSingle(txtCantidad.Text) > existenciaSelected)//verifica que la cantidad no exceda de la existencia del inventario
                                {
                                    MessageBox.Show("La cantidad que desea extraer del inventario supera de su existencia", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtCantidad.Text = "";
                                }
                                else
                                {
                                    SolicitudRecibo solicitud = new SolicitudRecibo();
                                    DetalleRecibo detalle = new DetalleRecibo();

                                    detalle.cantidad = Convert.ToSingle(txtCantidad.Text);
                                    detalle.precio = Convert.ToSingle(txtPrecio.Text);

                                    GestionMateriales.Material mate = new GestionMateriales.Material();

                                    mate.precio = txtPrecio.Text;
                                    mate.nombre = txtNombre.Text;
                                    mate.unidad = txtPresentacion.Text;

                                    detalle.material = mate;
                                    detalle.subtotal = detalle.cantidad * detalle.precio;

                                    detalles.Add(detalle);

                                    dataSoli.ItemsSource = null;
                                    dataSoli.ItemsSource = detalles;

                                    //Limpia los campos luego de agregar un insumo
                                    txtCantidad.Text = "";
                                    txtPrecio.Text = "";
                                    txtNombre.Text = "";
                                    txtPresentacion.Text = "";
                                    txtCliente.IsEnabled = false;
                                    btnImprimir.IsEnabled = true;

                                    existenciaSelected = 0;

                                    totalTotal = totalTotal + detalle.subtotal;
                                    txtTotalRecibo.Text = "$ " + Convert.ToString(totalTotal);
                                }
                            }
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

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (txtCliente.Text == "")
            {
                MessageBox.Show("Debe seleccionar un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (txtCodigoRecibo.Text == "") {
                    MessageBox.Show("Debe agregar el codigo de la solicitud", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SolicitudRecibo solicitud = new SolicitudRecibo();
                    solicitud.fechaSolicitudRecibo = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    solicitud.codigo = txtCodigoRecibo.Text;
                    solicitud.nombreCliente = txtCliente.Text;
                    solicitud.totalRecibo = totalTotal;
                    
                    solicitud.setListDetalles(detalles);
                    dataSoli.ItemsSource = detalles;

                    GenerarImpresion(solicitud.fechaSolicitudRecibo, solicitud.codigo, solicitud.nombreCliente, solicitud.totalRecibo);

                    detalles.Clear();
                    dataSoli.ItemsSource = null;

                    txtCodigoRecibo.Text = GenerarCodigoRecibo();//Genera nuevo codigo del Recibo
                    txtCliente.Text = "";
                    txtTotalRecibo.Text = "";
                    txtPresentacion.Text = "";
                    txtCliente.IsEnabled = true;
                    btnImprimir.IsEnabled = false;

                    existenciaSelected = 0;
                    totalTotal = 0;
                }
            }
        }

        private void GenerarImpresion(string fechaSolicitudRecibo, string codigo, string nombreCliente, float totalRecibo)
        {
            //Adecuar cabeceras de tablas.
            DataTable aImprimir = new DataTable();
            aImprimir.Columns.Add("Material");
            aImprimir.Columns.Add("Presentación");
            aImprimir.Columns.Add("Cantidad");
            aImprimir.Columns.Add("Precio");
            aImprimir.Columns.Add("Subtotal");
            string[] descripcion = new string[5];
            for (int i = 0; i < detalles.Count(); i++)
            {
                descripcion[0] = detalles[i].material.nombre;
                descripcion[1] = detalles[i].material.unidad;
                descripcion[2] = detalles[i].cantidad.ToString();
                descripcion[3] = detalles[i].material.precio;
                descripcion[4] = detalles[i].subtotal.ToString();
                
                // Agregando detalle a la tabla de la impresión.
                aImprimir.Rows.Add(new Object[] { descripcion[0], descripcion[1], descripcion[2], descripcion[3], descripcion[4] });
            }
            CreadorPDF impresion = new CreadorPDF();
            impresion.PrepararImpresionRecibo(aImprimir, fechaSolicitudRecibo, codigo, nombreCliente, totalRecibo);
            //MessageBox.Show("Se ha generado el archivo de la solicitud.", "Generación de solicitud", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string GenerarCodigoRecibo()
        {
            DateTime fecha = DateTime.Now;
            string anio = fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            string hora = fecha.Hour.ToString();
            string min = fecha.Minute.ToString();
            string seg = fecha.Second.ToString();

            return anio + mes + dia + hora + min + seg;
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

            return dia +"/"+ mes + "/" + anio;
        }
        
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea cancelar? se borrará todos los datos del Recibo", "Confirmacion",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                txtCliente.Text = "";
                txtCantidad.Text = "";
                txtPrecio.Text = "";
                txtNombre.Text = "";
                txtPresentacion.Text = "";
                txtTotalRecibo.Text = "";
                txtCliente.IsEnabled = true;
                btnImprimir.IsEnabled = false;

                totalTotal = 0;
                existenciaSelected = 0;

                detalles.Clear();
                dataSoli.ItemsSource = null;
            }
            else
            {

            }
        }

        private void dgMateriales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataRowView row = grid.SelectedItem as DataRowView;
                    // DataRowView row = dataMateriales.SelectedItem as DataRowView;
                   // DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    
                    txtNombre.Text = row.Row.ItemArray[1].ToString();
                    txtPrecio.Text = row.Row.ItemArray[4].ToString();
                    txtPresentacion.Text = row.Row.ItemArray[2].ToString();
                    existenciaSelected = float.Parse(row.Row.ItemArray[3].ToString());
                }
            }
        }
    }
}
