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
        private ControlBD control = new ControlBD();
        public Login()
        {
            InitializeComponent();
            //TO DO: Iniciar conexión a la base de datos.
        }

        private void OlvideButton_Click(object sender, RoutedEventArgs e)
        {
            // Cambio de visibilidad de los objetos en la ventana.
            labelContrasena.Visibility = Visibility.Hidden;
            cuadroContrasena.Visibility = Visibility.Hidden;
            botonInicioSesion.Visibility = Visibility.Hidden;
            botonOlvide.Visibility = Visibility.Hidden;
            labelNuevaContrasena.Visibility = Visibility.Visible;
            cuadroNuevaContrasena.Visibility = Visibility.Visible;
            labelRestaurarContrasena.Visibility = Visibility.Visible;
            cuadroRestaurarContrasena.Visibility = Visibility.Visible;
            botonRestaurarContrasena.Visibility = Visibility.Visible;
            labelBienvenido.Content = "Ingrese su nueva contraseña.\nUse al menos 6 caracteres.";
            botonVolver.Visibility = Visibility.Visible;
        }

        private void BotonRestaurarContrasena_Click(object sender, RoutedEventArgs e)
        {
            string nuevaContra = cuadroNuevaContrasena.Password.ToString();
            //Comprobar que la contraseña sea de al menos 6 caracteres
            if (nuevaContra.Length > 5)
            {
                //Comprobar que las contraseñas coincidan.
                if (cuadroRestaurarContrasena.Password.ToString().Equals(nuevaContra))
                {
                    //¿Cómo se va a asegurar que el usuario específico quiso recuperar su contraseña?
                    MessageBox.Show("Contraseña recuperada.", "Recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Las contraseñas no coinciden. Intente de nuevo.", "Error al recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                    cuadroNuevaContrasena.Clear();
                    cuadroRestaurarContrasena.Clear();
                }
            }
            else
            {
                labelBienvenido.Content = "La contraseña es muy corta.\nIntente de nuevo.";
                cuadroNuevaContrasena.Clear();
                cuadroRestaurarContrasena.Clear();
            }
        }

        private void BotonVolver_Click(object sender, RoutedEventArgs e)
        {
            labelContrasena.Visibility = Visibility.Visible;
            cuadroContrasena.Visibility = Visibility.Visible;
            botonInicioSesion.Visibility = Visibility.Visible;
            botonOlvide.Visibility = Visibility.Visible;
            labelNuevaContrasena.Visibility = Visibility.Hidden;
            cuadroNuevaContrasena.Visibility = Visibility.Hidden;
            labelRestaurarContrasena.Visibility = Visibility.Hidden;
            cuadroRestaurarContrasena.Visibility = Visibility.Hidden;
            botonRestaurarContrasena.Visibility = Visibility.Hidden;
            labelBienvenido.Content = "Para acceder, ingrese sus credenciales.";
            botonVolver.Visibility = Visibility.Hidden;
        }

        private void BotonInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            //Estos valores son temporales. Al implementar la conexión a BD, borrar credenciales temporales.
            String contrasenaTemp = "0xFE59AC",
                   contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioTemp = "Administrador";
            if (contrasenaBox.Equals(contrasenaTemp))
            {
                //Si la contraseña es correcta
                MainWindow mw = new MainWindow();
                //TODO: Implementar recuperación de información del usuario
                mw.Title += usuarioTemp;
                mw.Show();
                this.Close();
            }
            else
            {
                //Si la contraseña no es correcta
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