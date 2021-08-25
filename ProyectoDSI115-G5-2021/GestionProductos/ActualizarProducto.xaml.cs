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
    /// Lógica de interacción para ActualizarProducto.xaml
    /// </summary>
    public partial class ActualizarProducto : Window
    {
        ControlBD control = new ControlBD();

        /// CONSTRUCTOR
        public ActualizarProducto()
        {
            InitializeComponent();
        }

        //EVENTO QUE IMPIDE INGRESAR LETRAS EN EL CAMPO DE CANTIDAD EN EXISTENCIA
        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtCantidad.Text, "[^0-9]"))
            {
                MessageBox.Show("En este campo solamente puede utilizar numeros\nPor favor ingrese de forma correcta la cantidad del producto.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCantidad.Text = txtCantidad.Text.Remove(txtCantidad.Text.Length - 1);
            }
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

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtCodigo.Text == "" || txtNombre.Text == "" || txtCantidad.Text == "" || txtUnidad.Text == "" || txtPrecio.Text == "" || txtMarca.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos del formulario", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                string fecha = DateTime.Now.ToShortDateString();
                Producto producto = new Producto(txtCodigo.Text, txtNombre.Text, txtCantidad.Text, txtUnidad.Text, txtPrecio.Text, txtMarca.Text, fecha, true);
                String respuesta = control.ActualizarProducto(producto);
                MessageBox.Show(respuesta, "Resultado del Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
