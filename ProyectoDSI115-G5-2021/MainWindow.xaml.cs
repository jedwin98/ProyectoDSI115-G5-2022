using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoDSI115_G5_2021
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Usuarios
            GestionUsuarios.GestionUsuarios gu = new GestionUsuarios.GestionUsuarios();
            gu.WindowState = WindowState.Maximized;
            gu.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Clientes
            GestionClientes.GestionClientes gc = new GestionClientes.GestionClientes();
            gc.WindowState = WindowState.Maximized;
            gc.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Cerrando sesión.
            Login lg = new Login();
            lg.Show();
            this.Close();
        }
    }
}
