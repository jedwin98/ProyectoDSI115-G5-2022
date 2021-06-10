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
    /// Lógica de interacción para ActualizarCliente.xaml
    /// </summary>
    public partial class ActualizarCliente : Window

    {

        ControlBD control = new ControlBD();
        public ActualizarCliente()
        {
            InitializeComponent();
           
        }

        

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNombre.Text == "" || txtApellido.Text == "" || txtTelefono.Text == "")
            {
                MessageBox.Show("Debe de llenar todos los campos", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Cliente clien = new Cliente();
                clien.codigo = txtId.Text;
                clien.nombres = txtNombre.Text;
                clien.apellidos = txtApellido.Text;
                clien.empresa = txtEmpresa.Text;
                clien.telefono = txtTelefono.Text;

                String resultaod = control.actualizarCliente(clien);
                MessageBox.Show(resultaod, "Resultado de la actualizacion", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           


        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {

            GestionClientes nueva = new GestionClientes();
            nueva.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //clean up code
            GestionClientes nueva = new GestionClientes() {
                WindowState = WindowState.Maximized
            };
            
            nueva.Show();
            

        }
    }
}
