using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoDSI115_G5_2021.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class AgregarCliente : Window
    {
        List<TipoServicio> ts = new List<TipoServicio>();
        ControlBD control = new ControlBD();
        public AgregarCliente()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "")
            {
                MessageBox.Show("Debe de llenar todos los campos importantes", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Cliente cliente = new Cliente();
                string respuesta;
                cliente.codigo = txtId.Text;
                cliente.nombres = txtNombre.Text;
                cliente.apellidos = txtApellido.Text;
                cliente.empresa = txtEmpresa.Text;
                cliente.correo = txtCorreo.Text;
                cliente.telefono = txtTelefono.Text;
                cliente.estado = txtEstado.Text;
                respuesta = control.AgregarCliente(cliente);
                MessageBox.Show(respuesta);

                this.Close();
            }
         }
     
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
           /* GestionClientes nueva = new GestionClientes() {
                WindowState = WindowState.Maximized
            };
            nueva.Show();*/
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            GestionClientes nueva = new GestionClientes() {
                WindowState = WindowState.Maximized
            };
            nueva.Show();
            */
        }

        private void TxtTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
