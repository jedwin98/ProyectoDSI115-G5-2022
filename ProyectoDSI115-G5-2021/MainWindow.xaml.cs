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
        GestionClientes.GestionClientes gc;
        GestionEmpleados.GestionEmpleados ge;
        GestionUsuarios.GestionUsuarios gu;
        Inventario verInventario;

        Nullable<bool> gca = false, gea = false, gua = false;
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
                gu = new GestionUsuarios.GestionUsuarios
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gu.sesion = this.sesion;
                gua = gu.ShowDialog();
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
                gc = new GestionClientes.GestionClientes
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gca = gc.ShowDialog();
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
            bool gcr = gca ?? false;
            bool ger = gea ?? false;
            bool gur = gua ?? false;
            if (!gcr && !ger && !gur)
            {
                ControlBD control = new ControlBD();
                control.Desbloquear(sesion.correoElectronico);
                Login lg = new Login();
                lg.Show();
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Cierre sus ventanas y guarde su trabajo antes de salir.","Sesión activa",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                ge = new GestionEmpleados.GestionEmpleados()
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gea = ge.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnInventario_Click(object sender, RoutedEventArgs e)
        {
            verInventario.Show();
        }
    }
}
