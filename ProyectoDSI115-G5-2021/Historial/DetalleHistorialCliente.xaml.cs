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
using ProyectoDSI115_G5_2021.GestionClientes;

namespace ProyectoDSI115_G5_2021.Historial
{
    /// <summary>
    /// Lógica de interacción para DetalleHistorialCliente.xaml
    /// </summary>
    public partial class DetalleHistorialCliente : Window
    {
        ControlBDS3 control = new ControlBDS3();
        string codigoCliente { get; set; }
        public DetalleHistorialCliente(string cod, string nombre, string empresa, string telefono)
        {
            InitializeComponent();
            txtNombres.Text = nombre;
            txtRazon.Text = empresa;
            txtTel.Text = telefono;
            codigoCliente = cod;
            // CargarTabla(codigoCliente);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void dgDetalles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataRowView row = grid.SelectedItem as DataRowView;

                    // CargarDetalles(row.Row.ItemArray[0].ToString(), codigoEmpleado);
                }
            }
        }
    
}
}
