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
    /// Lógica de interacción para AgregarEmpleado.xaml
    /// </summary>
    public partial class AgregarEmpleado : Window
    {
        List<Area> areas = new List<Area>();
        List<Cargo> cargos = new List<Cargo>();
        ControlBD control = new ControlBD();


        public AgregarEmpleado()
        {
            InitializeComponent();
            CargarCombos();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCargo.SelectedItem ==null || cmbArea.SelectedItem == null || txtId.Text == null || txtNombre.Text == null || txtApellido.Text == null || datePicker1.SelectedDate == null)
            { // validacion de campos nulos
                MessageBox.Show("Debe llenar todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Cargo cargo = (Cargo)cmbCargo.SelectedItem;
                Area area = (Area)cmbArea.SelectedItem;
                Empleado empleado = new Empleado(txtId.Text, txtNombre.Text, txtApellido.Text, txtEstado.Text, datePicker1.SelectedDate.Value, cargo, area);
                String respuesta = control.AgregarEmpleado(empleado);
                MessageBox.Show(respuesta, "Resultado del Guardado", MessageBoxButton.OK, MessageBoxImage.Information);

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
        public void CargarCombos()
        {
            cargos = control.ConsultarCargo();
            areas = control.ConsultarArea();
            cmbArea.ItemsSource = areas;
            cmbCargo.ItemsSource = cargos;

        }
    }
}
