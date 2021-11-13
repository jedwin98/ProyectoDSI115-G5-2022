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

namespace ProyectoDSI115_G5_2021.GestionMateriales
{
    /// <summary>
    /// Lógica de interacción para EliminarMaterial.xaml
    /// </summary>
    public partial class EliminarMaterial : Window
    {
        ControlBD control = new ControlBD();
        public EliminarMaterial()
        {
            InitializeComponent();
        }

        //*************************** METODO DE BOTONES ***************************************//
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("¿Está seguro que desea eliminar este material?", "Confirmación de Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string respuesta = control.EliminarMaterial(txtCodigo.Text);
                MessageBox.Show(respuesta, "Eliminar Empleado", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
        }
    }
}
