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

namespace ProyectoDSI115_G5_2021.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para GestionClientes.xaml
    /// </summary>
    public partial class GestionClientes : Window
    {
        DataTable dt = new DataTable();
        ControlBD control = new ControlBD();
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
    }
}
