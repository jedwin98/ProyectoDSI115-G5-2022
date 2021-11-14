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


namespace ProyectoDSI115_G5_2021.GestionMateriales
{
    /// <summary>
    /// Lógica de interacción para GestionMateriales.xaml
    /// </summary>
    public partial class GestionMateriales : Window
    {
        DataTable dt = new DataTable();
        ControlBD control = new ControlBD();
        List<Material> material = new List<Material>();
        public GestionMateriales()
        {
            InitializeComponent();
            cargarTabla();
        }
        //METODO INICIAL
        public void cargarTabla()
        {
            dt.Clear();
            dataMateriales.ItemsSource = null;

            dt = control.consultarMateriales();
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        // ******** METODOS PARA BOTONES ***************//
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            cargarTabla();
            this.Close();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarMaterial ag = new AgregarMaterial()
            {
                WindowState = WindowState.Normal
            };

            ag.ShowDialog();
            cargarTabla();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarMaterial ac = new ActualizarMaterial()
            {
                WindowState = WindowState.Normal
            };

            string fecha;
            DataRowView row = dataMateriales.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un material", "Seleccione un material", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                ac.txtCodigo.Text = row.Row.ItemArray[0].ToString();
                ac.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ac.txtUnidad.Text = row.Row.ItemArray[2].ToString();
                ac.txtCantidad.Text = row.Row.ItemArray[3].ToString();
                ac.txtPrecio.Text = row.Row.ItemArray[4].ToString();
                fecha = row.Row.ItemArray[5].ToString();

                ac.ShowDialog();
                cargarTabla();
            }
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            EliminarMaterial ec = new EliminarMaterial()
            {
                WindowState = WindowState.Normal
            };

            string fecha;
            DataRowView row = dataMateriales.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un material", "Seleccione un material", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                ec.txtCodigo.Text = row.Row.ItemArray[0].ToString();
                ec.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ec.txtUnidad.Text = row.Row.ItemArray[2].ToString();
                ec.txtCantidad.Text = row.Row.ItemArray[3].ToString();
                ec.txtPrecio.Text = row.Row.ItemArray[4].ToString();
                fecha = row.Row.ItemArray[5].ToString();

                ec.ShowDialog();
                cargarTabla();
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
            dt = control.BuscarMaterial(txtBuscar.Text);
            dataMateriales.ItemsSource = dt.DefaultView;
        }
    }
}
