using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para GestionEmpleados.xaml
    /// </summary>
    public partial class GestionEmpleados : Window
    {
        List<Empleado> empleados = new List<Empleado>();
        DataTable data = new DataTable();
        ControlBD control = new ControlBD();
        public GestionEmpleados()
        {
            InitializeComponent();
            CargarTabla();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {

        }
        public void CargarTabla()
        {

            //  empleados = control.ConsultarEmpleados();
            data = control.ConsultarEmpleados();
            dataEmpleados.ItemsSource = data.DefaultView;
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarEmpleado();
            }
        }
        private void BuscarEmpleado()
        {
            empleados.Clear();
            //empleados= control.BuscarCliente(textBuscar.Text);
           // dataEmpleados.ItemsSource = data.DefaultView;

        }
    }
}
