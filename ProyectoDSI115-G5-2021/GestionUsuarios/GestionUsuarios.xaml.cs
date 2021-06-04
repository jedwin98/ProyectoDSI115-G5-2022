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

namespace ProyectoDSI115_G5_2021.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para GestionUsuarios.xaml
    /// </summary>
    public partial class GestionUsuarios : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        private bool editando;
        public GestionUsuarios()
        {
            InitializeComponent();
            //CargarTabla();
        }

        private void CargarTabla()
        {
            //TODO: Hacer función de carga de usuarios.
            throw new NotImplementedException();
        }

        private void LlenarComboTipos()
        {
            //Se envía el comboBox y se recibe junto con los tipos de usuario.
            comboRoles = control.ConsultarTipoUsuario(comboRoles);
        }

        private void LlenarComboEmpleados()
        {
            //Igual que LlenarComboTipos, pero aplicado a empleados 
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Inicializando formulario para agregar usuario
            cuadroUsuario.SetCurrentValue(IsEnabledProperty, true);
            cuadroUsuario.Text = "";
            cuadroContrasena.SetCurrentValue(IsEnabledProperty, true);
            cuadroContrasena.Clear();
            cuadroEmail.SetCurrentValue(IsEnabledProperty, true);
            cuadroEmail.Text = "";
            comboEmpleado.SetCurrentValue(IsEnabledProperty, true);
            comboRoles.SetCurrentValue(IsEnabledProperty, true);
            botonGuardar.SetCurrentValue(IsEnabledProperty, true);
            botonValidar.SetCurrentValue(IsEnabledProperty, true);
            LlenarComboTipos();
            //LlenarComboUsuarios();
            // Booleano para determinar el resultado de guardar
            editando = false;
        }

        private void BotonGuardar_Click(object sender, RoutedEventArgs e)
        {
            //TO DO: Lógica de guardado de usuarios
            if (editando)
            {
                //Lógica de edición de registro
            }
            else
            {
                //Lógica de creación de registro
            }
            //Después de guardar, reiniciar los campos de edición
            cuadroUsuario.SetCurrentValue(IsEnabledProperty, false);
            cuadroUsuario.Text = "";
            cuadroContrasena.SetCurrentValue(IsEnabledProperty, false);
            cuadroContrasena.Clear();
            cuadroEmail.SetCurrentValue(IsEnabledProperty, false);
            cuadroEmail.Text = "";
            //Limpia ComboBox para llenar en la siguiente edición
            comboEmpleado.SetCurrentValue(IsEnabledProperty, false);
            comboEmpleado.Items.Clear();
            comboRoles.SetCurrentValue(IsEnabledProperty, false);
            comboRoles.Items.Clear();
            botonGuardar.SetCurrentValue(IsEnabledProperty, false);
            botonValidar.SetCurrentValue(IsEnabledProperty, false);
        }

        private void BotonValidar_Click(object sender, RoutedEventArgs e)
        {
            //Preparar algoritmo para detectar un string de correo válido
            string mail = cuadroEmail.Text;
            CorreoValido(mail);
        }

        private void CorreoValido(string mail)
        {
            try
            {
                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(mail);
                cuadroEmail.Background = Brushes.LightGreen;
            }
            catch
            {
                cuadroEmail.Background = Brushes.LightPink;
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
