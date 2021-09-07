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
using ProyectoDSI115_G5_2021.GestionProductos;

namespace ProyectoDSI115_G5_2021.GestionProductos
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : Window
    {
        ControlBD control = new ControlBD();

        public AgregarProducto()
        {
            InitializeComponent();
        }

        // +-+-+-+-+-+-+-+-+-+-+ METODO DE BOTONES +-+-+-+-+-+-+-+-+-+-+
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(txtCodigo.Text == "" || txtNombre.Text == "" || txtCantidad.Text == "" || txtUnidad.Text == "" || txtMarca.Text == "" || txtPrecio.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos del formulario de Producto", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                    string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    Producto producto = new Producto("P" + txtCodigo.Text, txtNombre.Text, txtCantidad.Text, txtUnidad.Text, txtMarca.Text, txtPrecio.Text, fecha, true);
                    String respuesta = control.AgregarProducto(producto);
                    MessageBox.Show(respuesta, "Resultado del Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();
        }
        // +-+-+-+-+-+-+-+-+-+-+ FIN METODO DE BOTONES +-+-+-+-+-+-+-+-+-+-+


        //EVENTO QUE IMPIDE INGRESAR LETRAS EN EL CAMPO DE CANTIDAD EN EXISTENCIA
        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtCantidad.Text, "[^0-9]"))
            {
                MessageBox.Show("En este campo solamente puede utilizar numeros\nPor favor ingrese de forma correcta la cantidad de producto.","Advertencia",MessageBoxButton.OK,MessageBoxImage.Warning);
                txtCantidad.Text = txtCantidad.Text.Remove(txtCantidad.Text.Length - 1);
            }
        }
        //EVENTO QUE IMPIDE INGRESAR LETRAS EN EL CAMPO DE PRECIO
        private void TxtPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPrecio.Text, "[^0-9.,]"))
            {
                MessageBox.Show("En este campo solamente puede utilizar enteros o decimales\nPor favor ingrese de forma correcta el precio del Producto.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrecio.Text = txtPrecio.Text.Remove(txtPrecio.Text.Length - 1);
            }
        }
    }
}
