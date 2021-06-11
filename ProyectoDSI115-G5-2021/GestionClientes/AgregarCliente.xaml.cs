using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
          //  llenarComboBox();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "")
            {
                MessageBox.Show("Debe de llenar todos los campos", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Cliente cliente = new Cliente();
                //   TipoServicio selecteditem = (TipoServicio)cmbServicio.SelectedItem;
                string respuesta;
                cliente.codigo = txtId.Text;
                cliente.nombres = txtNombre.Text;
                cliente.apellidos = txtApellido.Text;
                cliente.empresa = txtEmpresa.Text;
                cliente.telefono = txtTelefono.Text;
                cliente.estado = txtEstado.Text;
                respuesta = control.agregarCliente(cliente);
                MessageBox.Show(respuesta);

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
            
            GestionClientes nueva = new GestionClientes() {
                WindowState = WindowState.Maximized
            };
            nueva.Show();
            

        }
    }
}
