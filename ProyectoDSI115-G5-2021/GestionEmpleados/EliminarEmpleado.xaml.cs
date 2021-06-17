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

namespace ProyectoDSI115_G5_2021.GestionEmpleados
{
    /// <summary>
    /// Lógica de interacción para EliminarEmpleado.xaml
    /// </summary>
    public partial class EliminarEmpleado : Window
    {
        ControlBD control = new ControlBD();
        List<Area> areas = new List<Area>();
        List<Cargo> cargos = new List<Cargo>();
       
        
        public EliminarEmpleado( string idArea, string idCargo)
        {
            InitializeComponent();
            CargarCombos(idArea,idCargo);
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (txtId.Text == "")
            {
                MessageBox.Show("Debe seleccionar un nuevo empleado en la pantalla de administración de empleados", "Eliminar Empleado", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
               result= MessageBox.Show("¿Está seguro que desea eliminar este empleado?", "Confirmación de Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result== MessageBoxResult.Yes)
                {
                    string respuesta = control.EliminarEmpleado(txtId.Text);
                    MessageBox.Show(respuesta, "Eliminar Empleado", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtId.Text = null;
                    txtNombre.Text = null;
                    txtApellido.Text = null;
                    datePicker1.SelectedDate = null;
                    cmbArea.SelectedItem = null;
                    cmbCargo.SelectedItem = null;
                }

            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            GestionEmpleados nueva = new GestionEmpleados()
            {
                WindowState = WindowState.Maximized
                
            };
            nueva.Show();


        }
        public void CargarCombos(string idA, string idC)
        {
            cargos = control.ConsultarCargo();
            areas = control.ConsultarArea();
            cmbArea.ItemsSource = areas;
            cmbCargo.ItemsSource = cargos;
            
            Area ar = new Area();
            Cargo car = new Cargo();
            for (int i =0; i<areas.Count(); i++)
            {
                ar = areas[i];
                if (ar.codigoArea== idA)
                {
                    cmbArea.SelectedIndex = i;
                }
            }

            for (int i = 0; i < cargos.Count(); i++)
            {
                car = cargos[i];
                if (car.codigoCargo == idC)
                {
                    cmbCargo.SelectedIndex = i;
                }
            }


        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
