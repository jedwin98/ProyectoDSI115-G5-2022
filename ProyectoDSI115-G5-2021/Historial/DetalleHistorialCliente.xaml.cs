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
using ProyectoDSI115_G5_2021.SolicitarInsumos;
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021.Historial
{
    /// <summary>
    /// Lógica de interacción para DetalleHistorialCliente.xaml
    /// </summary>
    public partial class DetalleHistorialCliente : Window
    {
        ControlBDS3 control = new ControlBDS3();
        
        string codigoCliente { get; set; }
        List<SolicitudInsumos> solicituds = new List<SolicitudInsumos>();
        List<DetalleSolicitudInsumos> detalles = new List<DetalleSolicitudInsumos>();
        SolicitudInsumos solicitudSelected { get; set; }
        public DetalleHistorialCliente(string cod, string nombre, string empresa, string telefono)
        {
            InitializeComponent();
            txtNombres.Text = nombre;
            txtRazon.Text = empresa;
            txtTel.Text = telefono;
            codigoCliente = cod;
            solicituds = control.SolicitudesDelCliente(cod);
            dataSolicitudes.ItemsSource = solicituds;
          //  MessageBox.Show(solicituds[0].solicitante.nombre);

               
            // CargarTabla(codigoCliente);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
          //  DataGrid grid = sender as DataGrid;
            SolicitudInsumos soli = dataSolicitudes.SelectedItem as SolicitudInsumos;
            if (soli != null)
            {
                CargarDetalles(soli.codigo);
                solicitudSelected = soli;
            }
            else
            {
               
                MessageBox.Show("Debe seleccionar una solicitud primero", "Seleccione una solicitud", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                //detail.ShowDialog();

            }
        }
        private void dgDetalles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            SolicitudInsumos soli = dataSolicitudes.SelectedItem as SolicitudInsumos;
            if (soli != null && grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                CargarDetalles(soli.codigo);
                solicitudSelected = soli;
               
            }
            else
            {

                MessageBox.Show("Debe seleccionar una solicitud primero", "Seleccione una solicitud", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // DetalleHistorialCliente detail = new DetalleHistorialCliente(client.codigo, client.nombres + " " + client.apellidos, client.empresa, client.telefono);
                //detail.ShowDialog();

            }
        }
        public void CargarDetalles(string codigoSolicitud)
        {
            
            detalles.Clear();
            dataDetalles.ItemsSource = null;

            detalles = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = detalles;
            
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            
                GenerarImpresion();
            
        }
        private void GenerarImpresion()
        {
            CreadorPDF impresion = new CreadorPDF();
            // Adecuar cabeceras de tablas.

            //  aImprimir.Columns.Add("Observaciones");
            SolicitudInsumos tempSoli = new SolicitudInsumos();
            DateTime hoy = DateTime.Now;
            string fecha = hoy.ToShortDateString();
            Console.WriteLine("\n" + solicituds.Count().ToString());
              string[] descripcion = new string[4];
               
                DataTable aImprimir = new DataTable();
                aImprimir.Columns.Add("Código de Solicitud");
                aImprimir.Columns.Add("Fecha");
                aImprimir.Columns.Add("Elaborado por");
                aImprimir.Columns.Add("Aprobado por");
                for (int i = 0; i < solicituds.Count(); i++)
                {
                tempSoli = solicituds[i];
                descripcion[0] = tempSoli.codigoReq;
                    descripcion[1] = tempSoli.fechaSolicitud;
                    descripcion[2] = tempSoli.solicitante.nombre;
                    descripcion[3] = tempSoli.autorizador.nombre;
                    // Agregando detalle a la tabla de la impresión.
                    aImprimir.Rows.Add(new Object[] { descripcion[0], descripcion[1], descripcion[2], descripcion[3] });
                }
                Console.WriteLine("pasó xaml");
                impresion.ImpresionSolicitud(aImprimir, txtNombres.Text, txtRazon.Text, tempSoli.codigo, fecha, 1);
                
                
                //MessageBox.Show("Se ha generado el archivo de la solicitud.", "Generación de solicitud", MessageBoxButton.OK, MessageBoxImage.Information);
            
            Console.Write("Salio");
        }

        /*private void GenerarImpresion()
        {
            CreadorPDF impresion = new CreadorPDF();
            // Adecuar cabeceras de tablas.
            
            //  aImprimir.Columns.Add("Observaciones");
           
            SolicitudInsumos tempSoli = new SolicitudInsumos();
            List<DetalleSolicitudInsumos> details = new List<DetalleSolicitudInsumos>();
            Console.WriteLine("\n"+solicituds.Count().ToString());
            for (int s = 0; s < solicituds.Count(); s++)
            {
                string[] descripcion = new string[4];
                Console.Write("\n"+s.ToString());
                tempSoli = solicituds[s];
                details = control.ConsultarDetalleSolicitudes(tempSoli.codigo);

                DataTable aImprimir = new DataTable();
                aImprimir.Columns.Add("Código de Material");
                aImprimir.Columns.Add("Descripción");
                aImprimir.Columns.Add("Presentación");
                aImprimir.Columns.Add("Cantidad");
                for (int i = 0; i < details.Count(); i++)
                {
                    descripcion[0] = details[i].material.codigo;
                    descripcion[1] = details[i].material.nombre;
                    descripcion[2] = details[i].material.unidad;
                    descripcion[3] = details[i].cantidad.ToString();
                    // Agregando detalle a la tabla de la impresión.
                    aImprimir.Rows.Add(new Object[] { descripcion[0], descripcion[1], descripcion[2], descripcion[3] });
                }
                Console.WriteLine("pasó xaml");
                impresion.ImpresionSolicitud(aImprimir, tempSoli.solicitante.nombre, tempSoli.autorizador.nombre, txtNombres.Text, txtRazon.Text, tempSoli.codigo, tempSoli.codigoReq, tempSoli.fechaSolicitud, solicituds.Count()-s);
                aImprimir = null;
                descripcion = null;
                details.Clear();
                //MessageBox.Show("Se ha generado el archivo de la solicitud.", "Generación de solicitud", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Console.Write("Salio");
        }
     
    */

    }
}
