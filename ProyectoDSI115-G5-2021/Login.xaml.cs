using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        // Preparación del inicio de sesión.
        // AUTOR: Félix Eduardo Henríquez Cruz
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
            else
            {
                // Si hay un remitente configurado y la conexión es exitosa se autorizarán cambios de contraseña.
                if (!RealizarPing())
                {
                    botonOlvide.SetCurrentValue(IsEnabledProperty, false);
                    MessageBox.Show("No se puede conectar al servicio de correo. Recuperar contraseña será deshabilitado.", "Error al conectar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Ping hacia servidores de correo para verificar acceso.
        // Se busca realizar al inicio del sistema y al presionar el botón "Cambiar Contraseña".
        // AUTOR: Félix Eduardo Henríquez Cruz
        private bool RealizarPing()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply;
                if (recuperador.correo.Contains("@hotmail") || recuperador.correo.Contains("@outlook"))
                {
                    // Ping hacia servidores de Outlook.com
                    reply = ping.Send("smtp-mail.outlook.com");
                }
                else
                {
                    // Ping hacia servidores de Gmail
                    reply = ping.Send("smtp-relay.gmail.com");
                }
                if (reply.Status != IPStatus.Success)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Función para iniciar el proceso de cambio de contraseña.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void OlvideButton_Click(object sender, RoutedEventArgs e)
        {
            // Ping antes de iniciar el proceso
            bool conectado = RealizarPing();
            if (cuadroEmail.Text.Equals(""))
            {
                // Campo de correo vacío
                MessageBox.Show("Ingrese un correo electrónico registrado e intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!conectado)
            {
                // Problemas de conexión
                MessageBox.Show("No se puede conectar al servicio de correo. Revise la conexión e intente de nuevo.", "Error al conectar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Hay texto en el campo y no hay problemas de conexión
                string aRecuperar = "";
                aRecuperar = cuadroEmail.Text;
                bool recuperable = control.BuscarUsuarioActivo(aRecuperar);
                if (!recuperable)
                {
                    MessageBox.Show("Ingrese un correo electrónico registrado e intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Cambio de visibilidad de los objetos en la ventana.
                    cuadroEmail.SetCurrentValue(IsEnabledProperty, false);
                    Random generador = new Random();
                    numeroRandom = generador.Next(1, 999999);
                    control.Bloquear(aRecuperar, "B");
                    string correoEnv = AgenteEmail.GenerarMail(control.ObtenerNombre(aRecuperar), numeroRandom),
                        asunto = "FYSIEX - Código de Seguridad";
                    AgenteEmail.EnviarMail(correoEnv, aRecuperar, recuperador, asunto);
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

        // Función para procesar el cambio de contraseña.
        // AUTOR: Félix Eduardo Henríquez Cruz
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
                    control.Bloquear(usuarioEmail, "C");
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

        // Terminación del proceso de cambio de contraseña.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BotonVolver_Click(object sender, RoutedEventArgs e)
        {
            control.Desbloquear(cuadroEmail.Text);
            ModoNormal();
        }

        // Función para restaurar el aspecto original de la pantalla.
        // AUTOR: Félix Eduardo Henríquez Cruz
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
            IniciarSesion();
        }

        // Función de acceso al sistema FYSIEX
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void IniciarSesion()
        {
            String contrasenaBox = cuadroContrasena.Password.ToString(),
                   usuarioEmail = cuadroEmail.Text;
            GestionUsuarios.Usuario sesion = control.CrearSesion(usuarioEmail, contrasenaBox);
            // Si la contraseña es correcta...
            if (sesion != null)
            {
                // Sesión de cambio de contraseña activa.
                if (sesion.estado.Equals("B"))
                {
                    MessageBox.Show("Tiene una sesión de recuperación de contraseña pendiente. Si su sesión terminó incorrectamente, consulte con gerencia.","Recuperación pendiente",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }
                /*if (sesion.estado.Equals("C"))
                {
                    MessageBox.Show("Tiene una sesión abierta. Si su sesión terminó incorrectamente, consulte con gerencia.", "Recuperación pendiente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }*/
                // Inicio normal.
                else
                {
                    MainWindow mw = new MainWindow();
                    mw.Title += sesion.empleado;
                    mw.Sesion = sesion;
                    control.Bloquear(usuarioEmail, "C");
                    mw.Show();
                    this.Close();
                }
            }
            // Si la contraseña o credencial no es correcta...
            else
            {
                MessageBox.Show("Las credenciales son incorrectas. Intente de nuevo o recupere su contraseña.", "Error al iniciar sesión", MessageBoxButton.OK, MessageBoxImage.Error);
                cuadroEmail.Clear();
                cuadroContrasena.Clear();
            }
        }

        // Cierre del sistema.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}