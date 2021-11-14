

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
            AgregarEmpleado ae = new AgregarEmpleado();

            ae.ShowDialog();
            CargarTabla();

        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            Cargo cargo = new Cargo();
            Area area = new Area();
            string fecha;
                             
            DataRowView row = dataEmpleados.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Debe seleccionar un cliente primero", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {   /*
                NOTA IMPORTANTE:
                Se obtienen los valores en el orden que se llena el datagrid
                ejemplo:
                en la consulta para llenar el datagrid COD_AREA esta como segundo parametro del SQL SELECT
                en el datagrid se coloca en la segunda posicion, en este caso en el indice 1
                y asi sucesivamente 
                */
                area.codigoArea = row.Row.ItemArray[1].ToString();
                area.nombreArea = row.Row.ItemArray[2].ToString();
                cargo.codigoCargo = row.Row.ItemArray[3].ToString();
                cargo.nombreCargo = row.Row.ItemArray[4].ToString();
                EliminarEmpleado ae = new EliminarEmpleado(area.codigoArea, cargo.codigoCargo)//se tienen que enviar los id para colocar los valores por defecto en los combo
                {
                    WindowState = WindowState.Normal
                };
                ae.txtId.Text = row.Row.ItemArray[0].ToString();
                
                ae.txtNombre.Text = row.Row.ItemArray[5].ToString();
                ae.txtApellido.Text = row.Row.ItemArray[6].ToString();
                fecha = row.Row.ItemArray[7].ToString();
                ae.datePicker1.SelectedDate = Convert.ToDateTime(fecha);          
                ae.txtEstado.Text = row.Row.ItemArray[8].ToString();
               
                ae.ShowDialog();
                CargarTabla();
                //this.Close();
                
            }
           
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Cargo cargo = new Cargo();
            Area area = new Area();
            string fecha;
           

            DataRowView row = dataEmpleados.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Debe seleccionar un cliente primero", "Seleccione un cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {   /*
                NOTA IMPORTANTE:
                Se obtienen los valores en el orden que se llena el datagrid
                ejemplo:
                en la consulta para llenar el datagrid COD_AREA esta como segundo parametro del SQL SELECT
                en el datagrid se coloca en la segunda posicion, en este caso en el indice 1
                y asi sucesivamente 
                */
                area.codigoArea = row.Row.ItemArray[1].ToString();
                area.nombreArea = row.Row.ItemArray[2].ToString();
                cargo.codigoCargo = row.Row.ItemArray[3].ToString();
                cargo.nombreCargo = row.Row.ItemArray[4].ToString();
                ActualizarEmpleado ae = new ActualizarEmpleado(area.codigoArea, cargo.codigoCargo)
                {
                    WindowState = WindowState.Normal
                };
                ae.txtId.Text = row.Row.ItemArray[0].ToString();

                ae.txtNombre.Text = row.Row.ItemArray[5].ToString();
                ae.txtApellido.Text = row.Row.ItemArray[6].ToString();
                fecha = row.Row.ItemArray[7].ToString();
                ae.datePicker1.SelectedDate = Convert.ToDateTime(fecha);
                ae.txtEstado.Text = row.Row.ItemArray[8].ToString();

                ae.ShowDialog();
                CargarTabla();
                //this.Close();

            }
            
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarEmpleado();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void CargarTabla()
        {
            data.Clear();
            dataEmpleados.ItemsSource = null;

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
        public void BuscarEmpleado()
        {
            data.Clear();
            data = control.BuscarEmpleado(textBuscar.Text);
            dataEmpleados.ItemsSource = data.DefaultView;

        }
        public DataTable CrearDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nombres");
            dataTable.Columns.Add("Apellidos");
            dataTable.Columns.Add("Area");
            dataTable.Columns.Add("Cargo");
            dataTable.Columns.Add("Fecha de Contratación");
            DateTime date;
            string fecha;
            string[] nombre = new string[5];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                nombre[0] = data.Rows[i][5].ToString();
                nombre[1] = data.Rows[i][6].ToString();
                nombre[2] = data.Rows[i][2].ToString();
                nombre[3] = data.Rows[i][4].ToString();
                fecha = data.Rows[i][7].ToString();
                date = Convert.ToDateTime(fecha);
                nombre[4] = date.ToShortDateString();
                dataTable.Rows.Add(new Object[] { nombre[0], nombre[1], nombre[2], nombre[3], nombre[4] });


            }
            return dataTable;
            
        }
        public void CrearPDF()
        {
            

            DataTable info = CrearDataTable();
            CreadorPDF creador = new CreadorPDF(info, "Empleados");
            
        }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e)
        {
            CrearPDF();
        }
    }
}
