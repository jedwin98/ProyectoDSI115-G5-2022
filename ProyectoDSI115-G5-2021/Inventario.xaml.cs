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
using ProyectoDSI115_G5_2021.GestionMateriales;
using ProyectoDSI115_G5_2021.GestionProductos;
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021
{
    /// <summary>
    /// Lógica de interacción para Inventario.xaml
    /// </summary>
    public partial class Inventario : Window
    {

        string tipo;
        GestionMateriales.GestionMateriales gm;
        GestionProductos.GestionProductos gp;
        Nullable<bool> gma = false;
        Nullable<bool> gpa = false;
        private Usuario sesion;
        internal Usuario Sesion { get => sesion; set => sesion = value; }

        DataTable dt = new DataTable();
        ControlBD control = new ControlBD();

        public Inventario(string tipoUsuario)
        {
            InitializeComponent();
            tipo = tipoUsuario;
            if (checkProducto.IsChecked == true && checkMaterial.IsChecked == true)
                cargarTabla();
            else if (checkProducto.IsChecked == true && checkMaterial.IsChecked == false)
                cargarTablaProd();
            else if (checkMaterial.IsChecked == true && checkProducto.IsChecked == false)
                cargarTablaMat();
            else
                MessageBox.Show("Marque una de las oociones para ver en el inventario");

        }

        public void cargarTabla()
        {
            dt = control.consultarInventario();
            dataInventario.ItemsSource = dt.DefaultView;
            
        }

        public void cargarTablaProd()
        {
            dt = control.consultarInventarioProd();
            dataInventario.ItemsSource = dt.DefaultView;

        }

        public void cargarTablaMat()
        {
            dt = control.consultarInventarioMat();
            dataInventario.ItemsSource = dt.DefaultView;

        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnMateriales_Click(object sender, RoutedEventArgs e)
        {
            if (tipo.Equals("A") || tipo.Equals("G"))
            {
                gm = new GestionMateriales.GestionMateriales()
                {
                    WindowState = WindowState.Maximized
                };
                gma = gm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnProductos_Click(object sender, RoutedEventArgs e)
        {
            if (tipo.Equals("A") || tipo.Equals("G"))
            {
                gp = new GestionProductos.GestionProductos()
                {
                    WindowState = WindowState.Maximized
                };
                gpa = gp.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSolicitar_Click(object sender, RoutedEventArgs e)
        {
            SolicitarInsumos.VerSolicitudes solicitudes = new SolicitarInsumos.VerSolicitudes()
            {
                WindowState = WindowState.Maximized
            };
            solicitudes.Sesion = sesion;

            solicitudes.Show();
        }

        private void BuscarInventario()
        {
            dt.Clear();
            if (checkProducto.IsChecked==true && checkMaterial.IsChecked == true)
            {
                dt = control.BuscarInventario(textBusquedaInve.Text);
                dataInventario.ItemsSource = dt.DefaultView;
            }
            else if (checkMaterial.IsChecked==true && checkProducto.IsChecked==false)
            {
                dt = control.BuscarInventarioMat(textBusquedaInve.Text);
                dataInventario.ItemsSource = dt.DefaultView;
            }
            else if (checkProducto.IsChecked==true && checkMaterial.IsChecked == false)
            {
                dt = control.BuscarInventarioPro(textBusquedaInve.Text);
                dataInventario.ItemsSource = dt.DefaultView;
            }
            
        }

        private void BtnBusquedaInv_Click(object sender, RoutedEventArgs e)
        {
            BuscarInventario();
        }

        public DataTable CrearDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Código");
            dataTable.Columns.Add("Nombre");
            dataTable.Columns.Add("Existencia");
            string[] nombre = new string[3];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nombre[0] = dt.Rows[i][0].ToString();
                nombre[1] = dt.Rows[i][1].ToString();
                nombre[2] = dt.Rows[i][2].ToString();
                dataTable.Rows.Add(new Object[] { nombre[0], nombre[1], nombre[2]});
            }
            return dataTable;

        }

        public void CrearPDF()
        {


            DataTable info = CrearDataTable();
            CreadorPDF creador = new CreadorPDF(info, "Inventario");

        }

        private void BtnReporteInv_Click(object sender, RoutedEventArgs e)
        {
            CrearPDF();
        }
    }
}
