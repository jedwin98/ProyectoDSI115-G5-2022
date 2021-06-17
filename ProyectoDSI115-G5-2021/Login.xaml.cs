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

namespace ProyectoDSI115_G5_2021
{
    public partial class Login : Window
    {
        private ControlBD control;
        private Thickness oglEmail, ogEmail, oglContra, ogContra;

        public Login()
        {
            InitializeComponent();
            control = new ControlBD();
            oglEmail = labelEmail.Margin;
            ogEmail = cuadroEmail.Margin;
            oglContra = labelContrasena.Margin;
            ogContra = cuadroContrasena.Margin;
            //TO DO: Iniciar conexión a la base de datos.
        }

        private void OlvideButton_Click(object sender, RoutedEventArgs e)
        {
            // Cambio de visibilidad de los objetos en la ventana.
            labelEmail.Margin = new Thickness(161, 300, 0, 0);
            cuadroEmail.Margin = new Thickness(310, 300, 0, 0);
            labelContrasena.Margin = new Thickness(150, 330, 0, 0);
            labelContrasena.Content = "Contraseña anterior";
            cuadroContrasena.Margin = new Thickness(310, 330, 0, 0);
            botonInicioSesion.Visibility = Visibility.Hidden;
            botonOlvide.Visibility = Visibility.Hidden;
            labelNuevaContrasena.Visibility = Visibility.Visible;
            cuadroNuevaContrasena.Visibility = Visibility.Visible;
            labelRestaurarContrasena.Visibility = Visibility.Visible;
            cuadroRestaurarContrasena.Visibility = Visibility.Visible;
            botonRestaurarContrasena.Visibility = Visibility.Visible;
            botonVolver.Visibility = Visibility.Visible;
            botonSalir.Visibility = Visibility.Hidden;
            labelHelp.Visibility = Visibility.Visible;
        }

        private void BotonRestaurarContrasena_Click(object sender, RoutedEventArgs e)
        {
            string nuevaContra = cuadroNuevaContrasena.Password.ToString(),
                   contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioCorreo = cuadroEmail.Text;
            GestionUsuarios.Usuario sesion = control.CrearSesion(usuarioCorreo, contrasenaBox);
            // Comprobar que la nueva contraseña sea de al menos 6 caracteres
            // y comprobar que las contraseñas coincidan.
            if (cuadroRestaurarContrasena.Password.ToString().Equals(nuevaContra) && nuevaContra.Length > 5 && sesion != null)
            {
                // El usuario ingresa su contraseña anterior y la nueva contraseña.
                if (control.CambiarContrasena(sesion.codigo, nuevaContra))
                {
                    MessageBox.Show("Contraseña cambiada. Haga clic en Aceptar para continuar.", "Cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    MainWindow mw = new MainWindow();
                    mw.Title += sesion.empleado;
                    mw.Sesion = sesion;
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error. Verifique su conexión e intente de nuevo.", "Error al cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("La credencial o las contraseñas no son correctas. Intente de nuevo.", "Error al cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                cuadroNuevaContrasena.Clear();
                cuadroRestaurarContrasena.Clear();
            }
        }

        private void BotonVolver_Click(object sender, RoutedEventArgs e)
        {
            labelEmail.Margin = oglEmail;
            cuadroEmail.Margin = ogEmail;
            labelContrasena.Margin = oglContra;
            labelContrasena.Content = "Contraseña";
            cuadroContrasena.Margin = ogContra;
            botonInicioSesion.Visibility = Visibility.Visible;
            botonOlvide.Visibility = Visibility.Visible;
            labelNuevaContrasena.Visibility = Visibility.Hidden;
            cuadroNuevaContrasena.Visibility = Visibility.Hidden;
            labelRestaurarContrasena.Visibility = Visibility.Hidden;
            cuadroRestaurarContrasena.Visibility = Visibility.Hidden;
            botonRestaurarContrasena.Visibility = Visibility.Hidden;
            botonVolver.Visibility = Visibility.Hidden;
            botonSalir.Visibility = Visibility.Visible;
            labelHelp.Visibility = Visibility.Hidden;
        }

        private void BotonInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            //Estos valores son temporales. Al implementar la conexión a BD, borrar credenciales temporales.
            String contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioCorreo = cuadroEmail.Text;
            GestionUsuarios.Usuario sesion = control.CrearSesion(usuarioCorreo, contrasenaBox);
            if (sesion != null)
            {
                //Si la contraseña es correcta
                MainWindow mw = new MainWindow();
                mw.Title += sesion.empleado;
                mw.Sesion = sesion;
                mw.Show();
                this.Close();
            }
            else
            {
                //Si la contraseña o la credencial no es correcta
                MessageBox.Show("Las credenciales son incorrectas. Intente de nuevo o recupere su contraseña.", "Error al iniciar sesión", MessageBoxButton.OK, MessageBoxImage.Error);
                cuadroEmail.Clear();
                cuadroContrasena.Clear();
            }
        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}