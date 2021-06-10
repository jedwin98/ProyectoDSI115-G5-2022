using System;
using System.Collections;
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


namespace ProyectoDSI115_G5_2021.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para GestionClientes.xaml
    /// </summary>
    public partial class GestionClientes : Window
    {
        DataTable dt = new DataTable();
        ControlBD control = new ControlBD();
        List<Cliente> clientes = new List<Cliente>();
        public GestionClientes()
        {
            InitializeComponent();
            cargarTabla();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public void cargarTabla()
        {
            dt = control.consultarClientes();
            dataClientes.ItemsSource = dt.DefaultView;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            ActualizarCliente ac = new ActualizarCliente() {
                WindowState = WindowState.Maximized
            };


            DataRowView row = dataClientes.SelectedItem as DataRowView;


            if (row == null)
            {
                MessageBox.Show("Seleccione primero un cliente", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                ac.txtId.Text = row.Row.ItemArray[0].ToString();
                ac.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ac.txtApellido.Text = row.Row.ItemArray[2].ToString();
                ac.txtEmpresa.Text = row.Row.ItemArray[3].ToString();
                //arreglar lo de servicio
                ac.txtTelefono.Text = row.Row.ItemArray[4].ToString();
                ac.txtEstado.Text = row.Row.ItemArray[5].ToString();

                ac.Show();
                this.Close();
            }

        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente ag = new AgregarCliente() {
                WindowState = WindowState.Maximized
            };
            ag.Show();
            this.Close();
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            EliminarCliente ec = new EliminarCliente() {
                WindowState = WindowState.Maximized
            };

            DataRowView row = dataClientes.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Debe seleccionar un cliente primero", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                ec.txtId.Text = row.Row.ItemArray[0].ToString();
                ec.txtNombre.Text = row.Row.ItemArray[1].ToString();
                ec.txtApellido.Text = row.Row.ItemArray[2].ToString();
                ec.txtEmpresa.Text = row.Row.ItemArray[3].ToString();
                //arreglar lo de servicio
                ec.txtTelefono.Text = row.Row.ItemArray[4].ToString();
                ec.txtEstado.Text = row.Row.ItemArray[5].ToString();
                ec.Show();
                this.Close();
            }



        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

            BuscarCliente();
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarCliente();
            }
        }
        private void BuscarCliente()
        {
            dt.Clear();
            dt = control.BuscarCliente(textBuscar.Text);
            dataClientes.ItemsSource = dt.DefaultView;

        }
    }
}
