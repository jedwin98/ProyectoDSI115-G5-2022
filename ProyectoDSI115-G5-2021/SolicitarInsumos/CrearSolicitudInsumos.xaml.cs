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

namespace ProyectoDSI115_G5_2021.SolicitarInsumos
{
    /// <summary>
    /// Lógica de interacción para CrearSolicitudInsumos.xaml
    /// </summary>
    public partial class CrearSolicitudInsumos : Window
    {
        private GestionUsuarios.Usuario sesion;
        internal GestionUsuarios.Usuario Sesion { get => sesion; set => sesion = value; }
        ControlBD control;
        DataTable dt = new DataTable();
        DataTable dataTable = new DataTable();
        string codigoSolicitud;
        public CrearSolicitudInsumos()
        {
            InitializeComponent();
            CargarTabla();
        }
        public void CargarTabla()
        {
            control = new ControlBD();
            dt = control.consultarMateriales();
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
                txtCodigo.Text = row.Row.ItemArray[0].ToString();
                txtNombre.Text = row.Row.ItemArray[1].ToString();
                txtCantidad.Text = row.Row.ItemArray[2].ToString();
                txtPresentacion.Text = row.Row.ItemArray[3].ToString();
               
            }

        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = sender as DataRowView;
            txtCodigo.Text = row.Row.ItemArray[0].ToString();
            txtNombre.Text = row.Row.ItemArray[1].ToString();
            txtCantidad.Text = row.Row.ItemArray[2].ToString();
            txtPresentacion.Text = row.Row.ItemArray[3].ToString();
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
            dt = control.BuscarMaterial(txtBuscar.Text);
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            DetalleSolicitudInsumos detalle = new DetalleSolicitudInsumos();
            detalle.cantidad = Convert.ToSingle(txtCantidad);
            



           /* dataTable.Columns.Add("Nombres");
            dataTable.Columns.Add("Apellidos");
            dataTable.Columns.Add("Empresa");

            dataTable.Columns.Add("Teléfono");

            string[] nombre = new string[4];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nombre[0] = dt.Rows[i][1].ToString();
                nombre[1] = dt.Rows[i][2].ToString();
                nombre[2] = dt.Rows[i][3].ToString();
                nombre[3] = dt.Rows[i][4].ToString();

                dataTable.Rows.Add(new Object[] { nombre[0], nombre[1], nombre[2], nombre[3] });


            }*/
        }

        private void BtnSolicitar_Click(object sender, RoutedEventArgs e)
        {
             List<DetalleSolicitudInsumos> detalles;
            //usa la funcion que ocupé para generar el pdf 
        }
        public string GenerarCodigoS()
        {
            DateTime fecha = DateTime.Now;
            string anio= fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            string hora = fecha.Hour.ToString();
            string min = fecha.Minute.ToString();
            string seg = fecha.Second.ToString();

            return dia + mes + anio + hora + min + seg;



        }
    }
}
