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
        private int numeroRandom;
        private Remitente recuperador;

        public Login()
        {
            InitializeComponent();
            control = new ControlBD();
            recuperador = control.ObtenerServicio();
            oglEmail = labelEmail.Margin;
            ogEmail = cuadroEmail.Margin;
            oglContra = labelContrasena.Margin;
            ogContra = cuadroContrasena.Margin;
            if (recuperador.correo.Equals(""))
            {
                botonOlvide.SetCurrentValue(IsEnabledProperty, false);
                MessageBox.Show("No se puede conectar al servicio de correo. Recuperar contraseña será deshabilitado.", "Error al conectar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OlvideButton_Click(object sender, RoutedEventArgs e)
        {
            // Cambio de visibilidad de los objetos en la ventana.
            if (cuadroEmail.Text.Equals(""))
            {
                MessageBox.Show("Ingrese un correo electrónico registrado e intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string aRecuperar = "";
                aRecuperar = cuadroEmail.Text;
                bool recuperable = control.BuscarUsuarioActivo(aRecuperar);
                if (!recuperable)
                {
                    MessageBox.Show("Ingrese un correo electrónico registrado e intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    cuadroEmail.SetCurrentValue(IsEnabledProperty, false);
                    Random generador = new Random();
                    numeroRandom = generador.Next(1, 999999);
                    control.Bloquear(aRecuperar, "B");
                    string correoEnv = AgenteEmail.GenerarMail(control.ObtenerNombre(aRecuperar), numeroRandom);
                    AgenteEmail.EnviarMail(correoEnv, aRecuperar, recuperador);
                    labelEmail.Margin = new Thickness(161, 300, 0, 0);
                    cuadroEmail.Margin = new Thickness(310, 300, 0, 0);
                    labelContrasena.Margin = new Thickness(145, 330, 0, 0);
                    labelContrasena.Content = "Código de seguridad";
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
            }
        }

        private void BotonRestaurarContrasena_Click(object sender, RoutedEventArgs e)
        {
            string nuevaContra = cuadroNuevaContrasena.Password.ToString(),
                   contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioEmail = cuadroEmail.Text;
            // Comprobar que la nueva contraseña sea de al menos 6 caracteres
            // que las contraseñas coincidan y que el número sea correcto.
            if (cuadroRestaurarContrasena.Password.ToString().Equals(nuevaContra) && nuevaContra.Length > 5 && !nuevaContra.Equals(contrasenaBox) && contrasenaBox.Equals(numeroRandom.ToString("000000")))
            {
                // El usuario ingresa su contraseña anterior y la nueva contraseña.
                if (control.CambiarContrasenaEmail(usuarioEmail, nuevaContra))
                {
                    MessageBox.Show("Contraseña cambiada. Haga clic en Aceptar para continuar.", "Cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    control.Desbloquear(usuarioEmail);
                    GestionUsuarios.Usuario sesion = control.CrearSesion(usuarioEmail, nuevaContra);
                    MainWindow mw = new MainWindow();
                    mw.Title += sesion.empleado;
                    mw.Sesion = sesion;
                    //control.Bloquear(usuarioEmail, "C");
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error. Verifique su conexión e intente de nuevo.", "Error al cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                    control.Desbloquear(usuarioEmail);
                    ModoNormal();
                }
            }
            else
            {
                MessageBox.Show("La credencial o las contraseñas no son correctas. Intente de nuevo.", "Error al cambiar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
                control.Desbloquear(usuarioEmail);
                ModoNormal();

            }
        }

        private void CuadroContrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (labelNuevaContrasena.Visibility == Visibility.Hidden && e.Key == Key.Return)
            {
                IniciarSesion();
            }
        }

        private void BotonVolver_Click(object sender, RoutedEventArgs e)
        {
            control.Desbloquear(cuadroEmail.Text);
            ModoNormal();
        }

        private void ModoNormal()
        {
            cuadroEmail.SetCurrentValue(IsEnabledProperty, true);
            numeroRandom = -1;
            labelEmail.Margin = oglEmail;
            cuadroEmail.Margin = ogEmail;
            cuadroEmail.Text = "";
            labelContrasena.Margin = oglContra;
            labelContrasena.Content = "Contraseña";
            cuadroContrasena.Margin = ogContra;
            cuadroContrasena.Password = "";
            botonInicioSesion.Visibility = Visibility.Visible;
            botonOlvide.Visibility = Visibility.Visible;
            labelNuevaContrasena.Visibility = Visibility.Hidden;
            cuadroNuevaContrasena.Password = "";
            cuadroNuevaContrasena.Visibility = Visibility.Hidden;
            labelRestaurarContrasena.Visibility = Visibility.Hidden;
            cuadroRestaurarContrasena.Password = "";
            cuadroRestaurarContrasena.Visibility = Visibility.Hidden;
            botonRestaurarContrasena.Visibility = Visibility.Hidden;
            botonVolver.Visibility = Visibility.Hidden;
            botonSalir.Visibility = Visibility.Visible;
            labelHelp.Visibility = Visibility.Hidden;
        }

        private void BotonInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            //Estos valores son temporales. Al implementar la conexión a BD, borrar credenciales temporales.
            IniciarSesion();
        }

        private void IniciarSesion()
        {
            String contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioEmail = cuadroEmail.Text;
            GestionUsuarios.Usuario sesion = control.CrearSesion(usuarioEmail, contrasenaBox);
            if (sesion != null)
            {
                //Si la contraseña es correcta
                if (sesion.estado.Equals("B"))
                {
                    MessageBox.Show("Tiene una sesión de recuperación de contraseña pendiente. Si su sesión terminó incorrectamente, consulte con gerencia.","Recuperación pendiente",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }
                if (sesion.estado.Equals("C"))
                {
                    MessageBox.Show("Tiene una sesión abierta. Si su sesión terminó incorrectamente, consulte con gerencia.", "Recuperación pendiente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MainWindow mw = new MainWindow();
                    mw.Title += sesion.empleado;
                    mw.Sesion = sesion;
                    //control.Bloquear(usuarioEmail, "C");
                    mw.Show();
                    this.Close();
                }
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