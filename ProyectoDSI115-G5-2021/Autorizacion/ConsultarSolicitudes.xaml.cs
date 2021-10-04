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
    /// Lógica de interacción para ConsultarSolicitudes.xaml
    /// </summary>
    public partial class ConsultarSolicitudes : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        internal GestionUsuarios.Usuario sesion;
        private VerificarSolicitud vs;
        public ConsultarSolicitudes()
        {
            InitializeComponent();
            dt = control.ConsultarSolicitudes();
            dataSolicitudes.ItemsSource = dt.DefaultView;
        }

        private void BtnVerificar_Click(object sender, RoutedEventArgs e)
        {
            if (!(dataSolicitudes.SelectedItem is DataRowView row))
            {
                MessageBox.Show("No hay solicitud seleccionada. Debe seleccionar una solicitud.", "Error al verificar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Agregar id de solicitud.
                vs = new VerificarSolicitud(row.Row.ItemArray[0].ToString(), row.Row.ItemArray[2].ToString(), row.Row.ItemArray[3].ToString(), row.Row.ItemArray[4].ToString(), row.Row.ItemArray[5].ToString(), row.Row.ItemArray[7].ToString(), row.Row.ItemArray[6].ToString());
                vs.sesion = this.sesion;
                vs.WindowState = WindowState.Maximized;
                // Bloquea la ejecución de esta instancia de ventana y se transfiere a la ventana creada.
                vs.ShowDialog();
                // Actualizar después de cambiar el estado de las solicitudes.
                dt = control.ConsultarSolicitudes();
                dataSolicitudes.ItemsSource = dt.DefaultView;
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
