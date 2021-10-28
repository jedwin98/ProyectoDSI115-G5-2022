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
using ProyectoDSI115_G5_2021.GestionClientes;
using ProyectoDSI115_G5_2021.SolicitarInsumos;
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021.Historial
{
    /// <summary>
    /// Lógica de interacción para DetalleHistorialCliente.xaml
    /// </summary>
    public partial class DetalleHistorialCliente : Window
    {
        ControlBDS3 control = new ControlBDS3();
        
        string codigoCliente { get; set; }
        List<SolicitudInsumos> solicituds = new List<SolicitudInsumos>();
        List<DetalleSolicitudInsumos> detalles = new List<DetalleSolicitudInsumos>();
        public DetalleHistorialCliente(string cod, string nombre, string empresa, string telefono)
        {
            InitializeComponent();
            txtNombres.Text = nombre;
            txtRazon.Text = empresa;
            txtTel.Text = telefono;
            codigoCliente = cod;
            solicituds = control.SolicitudesDelCliente(cod);
            dataSolicitudes.ItemsSource = solicituds;
          //  MessageBox.Show(solicituds[0].solicitante.nombre);

               
            // CargarTabla(codigoCliente);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            SolicitudInsumos soli = dataSolicitudes.SelectedItem as SolicitudInsumos;
            if (soli != null && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                CargarDetalles(soli.codigo);
            }
            else
            {
               
                MessageBox.Show("Debe seleccionar una solicitud primero", "Seleccione una solicitud", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                //detail.ShowDialog();

            }
        }
        private void dgDetalles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            SolicitudInsumos soli = dataSolicitudes.SelectedItem as SolicitudInsumos;
            if (soli != null && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                CargarDetalles(soli.codigo);
               
            }
            else
            {

                MessageBox.Show("Debe seleccionar una solicitud primero", "Seleccione una solicitud", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                //detail.ShowDialog();

            }
        }
        public void CargarDetalles(string codigoSolicitud)
        {
            
            detalles.Clear();
            dataDetalles.ItemsSource = null;

            detalles = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = detalles;
            
        }

    }
}
