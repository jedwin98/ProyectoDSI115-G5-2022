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
    /// Lógica de interacción para ListadoRecibo.xaml
    /// </summary>
    public partial class ListadoRecibo : Window
    {
        CrearRecibo recibo;
        Nullable<bool> recibom;
        DataTable dt = new DataTable();
        ControlBD cn = new ControlBD();
        DetalleRecibos vs;
        internal GestionUsuarios.Usuario sesion;

        public ListadoRecibo()
        {
            InitializeComponent();
            ConsultarRecibo();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            recibo = new CrearRecibo()
            {
                WindowState = WindowState.Normal
            };
            recibom = recibo.ShowDialog();
        }



        private void ConsultarRecibo()
        {
            dt = cn.consultarRecibo();
            dataRecibos.ItemsSource = dt.DefaultView;
        }

        private void DataRecibos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(dataRecibos.SelectedItem is DataRowView row))
            {
                MessageBox.Show("No hay solicitud seleccionada. Debe seleccionar una solicitud.", "Error al verificar",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
                // Agregar id de solicitud.
                vs = new DetalleRecibos(row.Row.ItemArray[0].ToString());
                vs.sesion = this.sesion;
                vs.WindowState = WindowState.Normal;
                // Bloquea la ejecución de esta instancia de ventana y se transfiere a la ventana creada.
                vs.ShowDialog();
                // Actualizar después de cambiar el estado de las solicitudes.
                dt = cn.consultarDetalleRecibo(row.Row.ItemArray[0].ToString());
                dataRecibos.ItemsSource = dt.DefaultView;
            }
        }

        private void DataRecibos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
