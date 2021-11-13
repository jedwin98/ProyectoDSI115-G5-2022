using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoDSI115_G5_2021.GestionProductos
{
    /// <summary>
    /// Lógica de interacción para GestionProductos.xaml
    /// </summary>
    public partial class GestionProductos : Window
    {
        DataTable dt = new DataTable();
        ControlBD control = new ControlBD();
        List<Producto> producto = new List<Producto>();
        public GestionProductos()
        {
            InitializeComponent();
            cargarTabla();
        }
        //METODO INICIAL
        public void cargarTabla()
        {
            dt.Clear();
            dataProductos.ItemsSource = null;
            dt = control.consultarProductos();
            dataProductos.ItemsSource = dt.DefaultView;
        }

        // ******** METODOS PARA BOTONES ***************//
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            cargarTabla();
            this.Close();
        }
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ag = new AgregarProducto()
            {
                WindowState = WindowState.Normal
            };

            ag.ShowDialog();
            cargarTabla();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarProducto ac = new ActualizarProducto()
            {
                WindowState = WindowState.Normal
            };

            string fecha;
            DataRowView row = dataProductos.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un producto", "Seleccione un producto", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                ac.txtCodigo.Text = row.Row.ItemArray[0].ToString();
                ac.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ac.txtUnidad.Text = row.Row.ItemArray[2].ToString();
                ac.txtCantidad.Text = row.Row.ItemArray[3].ToString();
                ac.txtMarca.Text = row.Row.ItemArray[4].ToString();
                ac.txtPrecio.Text = row.Row.ItemArray[5].ToString();
                
                fecha = row.Row.ItemArray[6].ToString();

                ac.ShowDialog();
                cargarTabla();
            }
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            EliminarProducto ec = new EliminarProducto()
            {
                WindowState = WindowState.Normal
            };

            string fecha;
            DataRowView row = dataProductos.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un producto", "Seleccione un producto", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                ec.txtCodigo.Text = row.Row.ItemArray[0].ToString();
                ec.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ec.txtUnidad.Text = row.Row.ItemArray[2].ToString();
                ec.txtCantidad.Text = row.Row.ItemArray[3].ToString();
                ec.txtMarca.Text = row.Row.ItemArray[4].ToString();
                ec.txtPrecio.Text = row.Row.ItemArray[5].ToString();
                
                fecha = row.Row.ItemArray[6].ToString();

                ec.ShowDialog();
                cargarTabla();
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarProducto();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarProducto();
            }
        }

        private void BuscarProducto()
        {
            dt.Clear();
            dt = control.BuscarProducto(txtBuscar.Text);
            dataProductos.ItemsSource = dt.DefaultView;
        }
    }
}
