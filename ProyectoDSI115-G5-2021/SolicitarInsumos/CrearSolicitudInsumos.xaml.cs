﻿using System;
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
    /// Lógica de interacción para CrearSolicitudInsumos.xaml
    /// </summary>
    public partial class CrearSolicitudInsumos : Window
    {
        private GestionUsuarios.Usuario sesion;
        internal GestionUsuarios.Usuario Sesion { get => sesion; set => sesion = value; }
        ControlBD control;
        DataTable dt = new DataTable();
        DataTable dataTable = new DataTable();    
        string codigoSolicitud { get; set; }
        List<DetalleSolicitudInsumos> detalles = new List<DetalleSolicitudInsumos>();



        public CrearSolicitudInsumos()
        {
            InitializeComponent();
            CargarTabla();
            codigoSolicitud = GenerarCodigoS();
            txtBuscar.Text = codigoSolicitud;
        }
        public void CargarTabla()
        {
            control = new ControlBD();
            dt = control.consultarMateriales();
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataMateriales.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Seleccione primero un material", "Seleccione un material", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
            else
            {
                txtCodigo.Text = row.Row.ItemArray[0].ToString();
                txtNombre.Text = row.Row.ItemArray[1].ToString();
               
                txtPresentacion.Text = row.Row.ItemArray[2].ToString();
               
            }

        }
      

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarMaterial();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarMaterial();
            }
        }

        private void BuscarMaterial()
        {
            dt.Clear();
            dt = control.BuscarMaterial(txtBuscar.Text);
            dataMateriales.ItemsSource = dt.DefaultView;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCantidad.Text == "")
                {
                    MessageBox.Show("Debe ingresar un valor a la cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (Convert.ToSingle(txtCantidad.Text) == 0.0)
                    {
                        MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        if (Convert.ToSingle(txtCantidad.Text) < 0)
                        {
                            MessageBox.Show("Debe ingresar un valor mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            DetalleSolicitudInsumos detalle = new DetalleSolicitudInsumos();
                            detalle.cantidad = Convert.ToSingle(txtCantidad.Text);
                            detalle.codigo = GenerarCodigoS();
                            detalle.codigoSolicitud = codigoSolicitud;

                            GestionMateriales.Material mate = new GestionMateriales.Material();
                            mate.codigo = txtCodigo.Text;
                            mate.nombre = txtNombre.Text;
                            mate.unidad = txtPresentacion.Text;

                            detalle.material = mate;
                            
                            
                            detalles.Add(detalle);
                            dataSoli.ItemsSource = null;
                            dataSoli.ItemsSource = detalles;
                            txtCantidad.Text = "";
                            txtCodigo.Text = "";
                            txtNombre.Text = "";
                            txtPresentacion.Text = "";


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Solo se permiten numeros en el campo cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCantidad.Text = "";
            }
           
             
        }

        private void BtnSolicitar_Click(object sender, RoutedEventArgs e)
        {
            SolicitudInsumos solicitud = new SolicitudInsumos();
            solicitud.codigo = codigoSolicitud;
            
            solicitud.solicitante = sesion;
            solicitud.autorizador = new GestionUsuarios.Usuario();
            solicitud.autorizador.codigo = "";
            solicitud.fechaSolicitud = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            solicitud.estado = "Pendiente";
            solicitud.setListDetalles(detalles);
            string respuesta = control.AgregarSolicudInsumos(solicitud);
            MessageBox.Show(respuesta, "Resultado de la solicitud", MessageBoxButton.OK, MessageBoxImage.Information);
         
        }
        public string GenerarCodigoS()
        {
            DateTime fecha = DateTime.Now;
            string anio= fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            string hora = fecha.Hour.ToString();
            string min = fecha.Minute.ToString();
            string seg = fecha.Second.ToString();

            return dia + mes + anio + hora + min + seg;



        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Está seguro que desea cancelar? se borrará todos los datos de la solicitud", "Confirmacion",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                detalles.Clear();
                dataSoli.ItemsSource = null;
                dataSoli.ItemsSource = detalles;
                dataSoli.UpdateLayout();
            }
            else
            {

            }
          
        }
    }
}