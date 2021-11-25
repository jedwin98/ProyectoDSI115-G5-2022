using ProyectoDSI115_G5_2021.GestionUsuarios;
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

namespace ProyectoDSI115_G5_2021.SolicitarInsumos
{
    /// <summary>
    /// Lógica de interacción para VerSolicitudes.xaml
    /// </summary>
    public partial class VerSolicitudes : Window
    {

        ControlBD control = new ControlBD();
        DataTable dat = new DataTable();
        DataTable detalles = new DataTable();
        internal Usuario sesion;
        internal Usuario Sesion { get => sesion; set => sesion = value; }
        public string codigoEmpleado { get; set; }


        System.Windows.Threading.DispatcherTimer dispatcherInv = new System.Windows.Threading.DispatcherTimer(); //OBJETO PARA EJECUTAR CADA CIERTO TIEMPO UN METODO

        public VerSolicitudes(string codE)
        {
            InitializeComponent();
            codigoEmpleado = codE;
          // MessageBox.Show(codE);
            CargarTabla(codE);
        }

        //SOBREESCRIBO EL EVENTO ACTIVATED
        //AUTOR: FRANCISCO ESCOBAR
        private void VerSolicitudes_Activated(object sender, System.EventArgs e)
        {

            //Ejecutando Metodo cada 10 segundos

            // AL ATRIBUTO TICK LE ASIGNAMOS EL EVENTO DISPATCHERTIMER_TICK EN EL CUAL VA TODO EL CODIGO A EJECUTAR
            dispatcherInv.Tick += new EventHandler(dispatcherTimer_Tick);
            //ASIGNAMOS HORA SEGUN EL PATRON TIMESPAN (HORA, MINUTO, SEGUNDO)
            dispatcherInv.Interval = new TimeSpan(0, 0, 10);
            //INICIAMOS EL METODO
            dispatcherInv.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //ACA SE INGRESA  TODO EL CODIGO QUE SE QUIERE EJECUTAR CADA CIERTO TIEMPO

            CargarTabla(codigoEmpleado);
        }

        private void BtnNueva_Click(object sender, RoutedEventArgs e)
        {
            CrearSolicitudInsumos crear = new CrearSolicitudInsumos();
            
            crear.Sesion = sesion;
            crear.ShowDialog();
            CargarTabla(codigoEmpleado);
           // this.Close();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataSolicitudes.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero una Solicitud", "Seleccione una Solicitud", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                CargarDetalles(row.Row.ItemArray[0].ToString(), codigoEmpleado);
                
            }
        }
        private void dgDetalles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataRowView row = grid.SelectedItem as DataRowView;
                   
                    CargarDetalles(row.Row.ItemArray[0].ToString(), codigoEmpleado);
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            detalles.Clear();
            dataDetalles.ItemsSource = null;
        }
        public void CargarTabla(string cod)
        {
            // MessageBox.Show(":" + Sesion.codigoEmpleado);
            dat.Clear();
            dat = control.ConsultarSolicitudes2( cod);
            dataSolicitudes.ItemsSource = dat.DefaultView;
        }
        public void CargarDetalles(string codigoSolicitud, string empleado)
        {
            ControlBD control2 = new ControlBD();
            detalles.Clear();
            dataDetalles.ItemsSource = null;
            
            detalles = control2.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = detalles.DefaultView;
           // CargarTabla(empleado);
        }

        
    }
}
