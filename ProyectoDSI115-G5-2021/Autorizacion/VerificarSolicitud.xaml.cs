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

namespace ProyectoDSI115_G5_2021.Autorizacion
{
    /// <summary>
    /// Lógica de interacción para VerificarSolicitud.xaml
    /// </summary>
    public partial class VerificarSolicitud : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        internal GestionUsuarios.Usuario sesion;
        private string codigoSolicitud, nombreSolicitante, apellidoSolicitante, estadoSolicitud;
        public VerificarSolicitud(string codigoSeleccionado, string nombreSeleccionado, string apellidoSeleccionado, string estadoSeleccionado)
        {
            InitializeComponent();
            codigoSolicitud = codigoSeleccionado;
            nombreSolicitante = nombreSeleccionado;
            apellidoSolicitante = apellidoSeleccionado;
            estadoSolicitud = estadoSeleccionado;
            if (estadoSolicitud.Equals("Pendiente"))
            {
                btnImprimir.SetCurrentValue(IsEnabledProperty, false);
            }
            else
            {
                btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                btnDenegar.SetCurrentValue(IsEnabledProperty, false);
                if (estadoSolicitud.Equals("Denegado"))
                {
                    btnImprimir.SetCurrentValue(IsEnabledProperty, false);
                }
            }
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
        }

        private void BtnDenegar_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar estado
            if (control.ActualizarEstadoSolicitud(codigoSolicitud, "Denegado", sesion.codigoEmpleado))
            {
                btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                btnDenegar.SetCurrentValue(IsEnabledProperty, false);
            }
            else
            {
                MessageBox.Show("Ocurrió un error al actualizar el estado de la solicitud.", "Error al actualizar estado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAprobar_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar estado
            if (control.ActualizarEstadoSolicitud(codigoSolicitud, "Aprobado", sesion.codigoEmpleado))
            {
                btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                btnDenegar.SetCurrentValue(IsEnabledProperty, false);
                btnImprimir.SetCurrentValue(IsEnabledProperty, true);
            }
            else
            {
                MessageBox.Show("Ocurrió un error al actualizar el estado de la solicitud.", "Error al actualizar estado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Mostrar diálogo de impresión

        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
