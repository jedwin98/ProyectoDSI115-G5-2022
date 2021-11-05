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
    /// Lógica de interacción para DetalleRecibos.xaml
    /// </summary>
    public partial class DetalleRecibos : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        ListadoRecibo recibo;
        Nullable<bool> recibom = false;
        internal GestionUsuarios.Usuario sesion;
        string id;

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            recibo = new ListadoRecibo()
            {
                WindowState = WindowState.Normal
            };
            recibom = recibo.ShowDialog();
        }

        public DetalleRecibos(string idSeleccionado)
        {
            InitializeComponent();
            id = idSeleccionado;
            dt = control.consultarDetalleRecibo(id);
            dataRecibos.ItemsSource = dt.DefaultView;
        }

        private void DataRecibos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
