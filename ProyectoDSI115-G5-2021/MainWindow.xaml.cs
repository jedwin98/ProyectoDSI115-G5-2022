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
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Usuario sesion;

        internal Usuario Sesion { get => sesion; set => sesion = value; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Usuarios
            //Solo puede entrar la gerencia.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                GestionUsuarios.GestionUsuarios gu = new GestionUsuarios.GestionUsuarios
                {
                    WindowState = WindowState.Maximized
                };
                gu.Show();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.","Error de acceso",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Clientes
            //Autorizado para gerencia y administración.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                GestionClientes.GestionClientes gc = new GestionClientes.GestionClientes
                {
                    WindowState = WindowState.Maximized
                };
                gc.Show();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            //Botón para cierre de ventana.
            //Cerrando desde la barra de título también activará el proceso de cierre.
            this.Close();
        }

        private void CerrarSesion()
        {
            //¿Cómo manejar las sesiones en la BD?
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //Al cerrar la ventana, la sesión debe cerrarse.
            //Después, debe mostrar la ventana de inicio de sesión.
            Login lg = new Login();
            lg.Show();
        }
    }
}
