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

namespace ProyectoDSI115_G5_2021.Autorizacion
{
    /// <summary>
    /// Lógica de interacción para VerificarSolicitud.xaml
    /// </summary>
    public partial class VerificarSolicitud : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        internal GestionUsuarios.Usuario sesion;
        private string codigoSolicitud, nombreSolicitante, fechaEntrada, numeroOrden, estadoSolicitud, autorizador, cliente, empresa;
        public VerificarSolicitud(string codigoSeleccionado, string nombreSeleccionado, string representante, string empresaRep, string fechaSeleccionada, string numeroSeleccionado, string estadoSeleccionado)
        {
            InitializeComponent();
            codigoSolicitud = codigoSeleccionado;
            nombreSolicitante = nombreSeleccionado;
            fechaEntrada = fechaSeleccionada;
            numeroOrden = numeroSeleccionado;
            cliente = representante;
            empresa = empresaRep;
            estadoSolicitud = estadoSeleccionado;
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
            if (estadoSolicitud.Equals("Pendiente"))
            {
                btnImprimir.SetCurrentValue(IsEnabledProperty, false);
                imgImprimir.SetCurrentValue(OpacityProperty, 0.35);
                // Se revisa cada entrada para verificar que no se superen las existencias.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    float existencia = float.Parse(dt.Rows[i][6].ToString()),
                        cantidad = float.Parse(dt.Rows[i][5].ToString());
                    // Si se supera, no se puede aprobar hasta que se tenga suficiente material.
                    if (existencia < cantidad)
                    {
                        btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                        imgAprobar.SetCurrentValue(OpacityProperty, 0.35);
                    }
                }
            }
            else
            {
                btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                imgAprobar.SetCurrentValue(OpacityProperty, 0.35);
                btnDenegar.SetCurrentValue(IsEnabledProperty, false);
                imgDenegar.SetCurrentValue(OpacityProperty, 0.35);
                if (estadoSolicitud.Equals("Denegado"))
                {
                    btnImprimir.SetCurrentValue(IsEnabledProperty, false);
                    imgImprimir.SetCurrentValue(OpacityProperty, 0.35);
                }
                else
                {
                    // Revisar empleado autorizador y actual
                    autorizador = control.ObtenerEmpleadoAutorizador(codigoSolicitud);
                }
            }
        }

        private void BtnDenegar_Click(object sender, RoutedEventArgs e)
        {
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
            // Actualizar estado
            if (control.ActualizarEstadoSolicitud(codigoSolicitud, "Denegado", sesion.codigoEmpleado))
            {
                btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                imgAprobar.SetCurrentValue(OpacityProperty, 0.35);
                btnDenegar.SetCurrentValue(IsEnabledProperty, false);
                imgDenegar.SetCurrentValue(OpacityProperty, 0.35);
            }
            else
            {
                MessageBox.Show("Ocurrió un error al actualizar el estado de la solicitud.", "Error al actualizar estado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
        }

        private void BtnAprobar_Click(object sender, RoutedEventArgs e)
        {
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
            // Actualizar estado
            bool error = false, actual = true;
            while (actual)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    float existencia = float.Parse(dt.Rows[i][6].ToString()),
                        cantidad = float.Parse(dt.Rows[i][5].ToString());
                    if (existencia < cantidad) { actual = false; }
                }
            }
            if (actual)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    float cantidad = float.Parse(dt.Rows[i][5].ToString());
                    error = !control.ActualizarExistencias(dt.Rows[i][1].ToString(), cantidad);
                }
                if (control.ActualizarEstadoSolicitud(codigoSolicitud, "Aprobado", sesion.codigoEmpleado) && !error)
                {

                    btnAprobar.SetCurrentValue(IsEnabledProperty, false);
                    imgAprobar.SetCurrentValue(OpacityProperty, 0.35);
                    btnDenegar.SetCurrentValue(IsEnabledProperty, false);
                    imgDenegar.SetCurrentValue(OpacityProperty, 0.35);
                    btnImprimir.SetCurrentValue(IsEnabledProperty, true);
                    imgImprimir.SetCurrentValue(OpacityProperty, 1.0);
                    autorizador = sesion.empleado;
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al actualizar el estado de la solicitud.", "Error al actualizar estado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Durante el proceso, se realizaron otras extracciones que impiden realizar la extracción actual. Verifique las existencias.", "Error al actualizar estado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dt = control.ConsultarDetalleSolicitudes(codigoSolicitud);
            dataDetalles.ItemsSource = dt.DefaultView;
            // Mostrar diálogo de impresión
            MessageBoxResult imprimir = MessageBox.Show("Se aprobó la solicitud. ¿Desea imprimir la solicitud?", "Solicitud Aprobada", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (imprimir == MessageBoxResult.Yes)
            {
                GenerarImpresion();
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            GenerarImpresion();
        }

        private void GenerarImpresion()
        {
            // Adecuar cabeceras de tablas.
            DataTable aImprimir = new DataTable();
            aImprimir.Columns.Add("Código de Material");
            aImprimir.Columns.Add("Descripción");
            aImprimir.Columns.Add("Presentación");
            aImprimir.Columns.Add("Cantidad");
            string[] descripcion = new string[4];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                descripcion[0] = dt.Rows[i][1].ToString();
                descripcion[1] = dt.Rows[i][2].ToString();
                descripcion[2] = dt.Rows[i][3].ToString();
                descripcion[3] = dt.Rows[i][5].ToString();
                // Agregando detalle a la tabla de la impresión.
                aImprimir.Rows.Add(new Object[] { descripcion[0], descripcion[1], descripcion[2], descripcion[3] });
            }
            CreadorPDF impresion = new CreadorPDF();
            impresion.PrepararImpresionSolicitud(aImprimir, nombreSolicitante, autorizador, cliente, empresa, codigoSolicitud, numeroOrden, fechaEntrada);
            //MessageBox.Show("Se ha generado el archivo de la solicitud.", "Generación de solicitud", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
