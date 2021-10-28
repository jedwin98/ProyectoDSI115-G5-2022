using ProyectoDSI115_G5_2021.GestionClientes;
using ProyectoDSI115_G5_2021.SolicitarInsumos;
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

namespace ProyectoDSI115_G5_2021.Historial
{
    /// <summary>
    /// Lógica de interacción para HistorialCliente.xaml
    /// </summary>
    public partial class HistorialCliente : Window
    {

        List<SolicitudInsumos> solicitudes = new List<SolicitudInsumos>();
        ControlBDS3 control = new ControlBDS3();
        List<Cliente> clientes = new List<Cliente>();
        public HistorialCliente()
        {
            InitializeComponent();
            cargarTabla();

        }
        public void cargarTabla()
        {
            clientes.Clear();
            clientes = control.ListaClientes();
            dataClientes.ItemsSource = clientes;
        }



        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {

            DataGrid grid = sender as DataGrid;
            Cliente client = dataClientes.SelectedItem as Cliente;
            if (client != null && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                detail.ShowDialog();
                
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente primero", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }
        private void dgDetalles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            Cliente client = dataClientes.SelectedItem as Cliente;
            if (client != null && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                detail.ShowDialog();

            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente primero", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);

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
            clientes.Clear();
            // clientes = control.BuscarCliente(textBuscar.Text);
            //dataClientes.ItemsSource = dt.DefaultView;

        }
    
}
}
