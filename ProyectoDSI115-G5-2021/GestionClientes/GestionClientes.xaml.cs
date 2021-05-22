using System;
using System.Collections.Generic;
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
        List<Cliente> cli = new List<Cliente>();
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
            ControlBD control = new ControlBD();
            cli = control.consultarClientes();
            dataClientes.ItemsSource = cli;
        }
    }
}
