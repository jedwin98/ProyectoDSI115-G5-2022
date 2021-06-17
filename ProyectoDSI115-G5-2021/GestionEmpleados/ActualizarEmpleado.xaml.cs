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
    /// Lógica de interacción para ActualizarEmpleado.xaml
    /// </summary>
    public partial class ActualizarEmpleado : Window
    {
        ControlBD control = new ControlBD();
        List<Area> areas = new List<Area>();
        List<Cargo> cargos = new List<Cargo>();
        public ActualizarEmpleado(string idArea, string idCargo)
        {
            InitializeComponent();
            CargarCombos(idArea, idCargo);
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCargo.SelectedItem == null || cmbArea.SelectedItem == null || txtId.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || datePicker1.SelectedDate == null)
            { // validacion de campos nulos
                MessageBox.Show("Debe llenar todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Cargo cargo = (Cargo)cmbCargo.SelectedItem;
                Area area = (Area)cmbArea.SelectedItem;
                Empleado empleado = new Empleado(txtId.Text, txtNombre.Text, txtApellido.Text, txtEstado.Text, datePicker1.SelectedDate.Value, cargo, area);
                String respuesta = control.ActualizarEmpleado(empleado);
                MessageBox.Show(respuesta, "Resultado de la actualización de datos", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

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
            for (int i = 0; i < areas.Count(); i++)
            {
                ar = areas[i];
                if (ar.codigoArea == idA)
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

    }
}
