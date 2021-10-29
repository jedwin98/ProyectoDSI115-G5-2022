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

namespace ProyectoDSI115_G5_2021.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para CambiarCorreo.xaml
    /// </summary>
    public partial class CambiarCorreo : Window
    {
        private bool emailCorrecto;
        internal Usuario sesion;
        private ControlBD control = new ControlBD();

        public CambiarCorreo()
        {
            InitializeComponent();
            btnRegistrar.SetCurrentValue(IsEnabledProperty, false);
        }

        private void CuadroEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CorreoValido(cuadroEmail.Text);
            if (emailCorrecto)
            {
                btnRegistrar.SetCurrentValue(IsEnabledProperty, true);
            }
            else
            {
                btnRegistrar.SetCurrentValue(IsEnabledProperty, false);
            }
        }

        private void CorreoValido(string mail)
        {
            try
            {
                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(mail);
                if (mail.Contains("@hotmail") || mail.Contains("@outlook") || mail.Contains("@gmail"))
                {
                    cuadroEmail.Background = Brushes.White;
                    emailCorrecto = true;
                }
                else
                {
                    cuadroEmail.Background = Brushes.LightGoldenrodYellow;
                    emailCorrecto = false;
                }
            }
            catch
            {
                cuadroEmail.Background = Brushes.LightPink;
                emailCorrecto = false;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            bool registro = control.EstablecerRemitente(cuadroEmail.Text, cuadroContrasena.Password);
            if (registro)
            {
                MessageBox.Show("Se ha establecido el remitente del sistema.", "Remitente establecido", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo establecer el remitente. Verifique su conexión e intente de nuevo.", "Error al establecer remitente", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
