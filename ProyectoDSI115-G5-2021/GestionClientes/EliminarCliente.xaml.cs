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

namespace ProyectoDSI115_G5_2021.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para EliminarCliente.xaml
    /// </summary>
    public partial class EliminarCliente : Window

    {
        ControlBD control = new ControlBD();
        public EliminarCliente()
        {
            InitializeComponent();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Debe seleccionar un nuevo cliente en la pantalla de administración de clientes", "Eliminar Cliente", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar este cliente?", "Confirmación de eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    string respuesta = control.EliminarCliente(txtId.Text);
                    MessageBox.Show(respuesta);
                    txtId.Text = null;
                    txtNombre.Text = null;
                    txtApellido.Text = null;
                    txtEmpresa.Text = null;
                    txtCorreo.Text = null;
                    txtTelefono.Text = null;

                    MessageBox.Show("Resultado de la actualizacion", "Cliente Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GestionClientes nueva = new GestionClientes()
            {
                WindowState = WindowState.Maximized
            };
            nueva.Show();
        }

        private void BtnVolver_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
