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


        public Cotizacion()
        {
            InitializeComponent();
            cargarTabla();
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
                txtNombreProducto.Text = row.Row.ItemArray[1].ToString();
                txtPrecio.Text = row.Row.ItemArray[3].ToString();
            }
        }

    }
}
