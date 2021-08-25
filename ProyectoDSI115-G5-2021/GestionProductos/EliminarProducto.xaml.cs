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

namespace ProyectoDSI115_G5_2021.GestionProductos
{
    /// <summary>
    /// Lógica de interacción para EliminarProducto.xaml
    /// </summary>
    public partial class EliminarProducto : Window
    {
        ControlBD control = new ControlBD();
        public EliminarProducto()
        {
            InitializeComponent();
        }

        //*************************** METODO DE BOTONES ***************************************//
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            GestionProductos gestionProductos = new GestionProductos()
            {
                WindowState = WindowState.Maximized
            };
            gestionProductos.Show();
            this.Close();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmación de Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string respuesta = control.EliminarProducto(txtCodigo.Text);
                MessageBox.Show(respuesta, "Eliminar Empleado", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCodigo.Text = null;
                txtNombre.Text = null;
                txtCantidad.Text = null;
                txtUnidad.Text = null;
                txtPrecio.Text = null;
                txtMarca.Text = null;
            }
        }
    }
}
