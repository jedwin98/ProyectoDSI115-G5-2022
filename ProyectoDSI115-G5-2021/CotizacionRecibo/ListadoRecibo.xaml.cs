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

namespace ProyectoDSI115_G5_2021.CotizacionRecibo
{
    /// <summary>
    /// Lógica de interacción para ListadoRecibo.xaml
    /// </summary>
    public partial class ListadoRecibo : Window
    {
        CrearRecibo recibo;
        Nullable<bool> recibom;
        DataTable dt = new DataTable();
        ControlBD cn = new ControlBD();
        DetalleRecibos vs;
        internal GestionUsuarios.Usuario sesion;

        public ListadoRecibo()
        {
            InitializeComponent();
            ConsultarRecibo();
            mesesCombo();
            cmbMeses.SelectedIndex = 0;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            recibo = new CrearRecibo()
            {
                WindowState = WindowState.Normal
            };
            recibom = recibo.ShowDialog();
        }


        private void DataRecibos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(dataRecibos.SelectedItem is DataRowView row))
            {
                MessageBox.Show("No hay solicitud seleccionada. Debe seleccionar una solicitud.", "Error al verificar",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
                // Agregar id de solicitud.
                vs = new DetalleRecibos(row.Row.ItemArray[0].ToString());
                vs.sesion = this.sesion;
                vs.WindowState = WindowState.Normal;
                // Bloquea la ejecución de esta instancia de ventana y se transfiere a la ventana creada.
                vs.ShowDialog();
                // Actualizar después de cambiar el estado de las solicitudes.
                dt = cn.consultarDetalleRecibo(row.Row.ItemArray[0].ToString());
                dataRecibos.ItemsSource = dt.DefaultView;
            }
        }

        private void DataRecibos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

            BuscarRecibo();
        }


        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbMeses.SelectedIndex = 0;
            chkPresente.IsChecked = false;
            dt.Clear();
            BuscarRecibo();
        }

        private void CmbMeses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string mes = cmbMeses.SelectedItem.ToString();
            if (mes != "" && chkPresente.IsChecked == true)
            {
                ConsultarPorMesAño();
            }
            else if (mes == "" && chkPresente.IsChecked == true)
            {
                ConsultarPorAño();
            }else if (mes != "" && chkPresente.IsChecked == false)
            {
                ConsultarPorMes();
            }
            else
            {
                ConsultarRecibo();
            }
        }

        private void ChkPresente_Checked(object sender, RoutedEventArgs e)
        {
            string mes = cmbMeses.SelectedItem.ToString();
            if (mes != "" && chkPresente.IsChecked == true)
            {
                ConsultarPorMesAño();
            }else if (mes != "" && chkPresente.IsChecked == false)
            {
                ConsultarPorMes();
            }else if (mes == "" && chkPresente.IsChecked == true)
            {
                ConsultarPorAño();
            }
            else
            {
                ConsultarRecibo();
            }
        }

        private void ConsultarRecibo()
        {
            dt.Clear();
            dt = cn.consultarRecibo();
            dataRecibos.ItemsSource = dt.DefaultView;
        }

        private void ConsultarPorAño()
        {
            string fecha;
            if (valorCheckbox())
            {
                fecha = DateTime.Now.ToString("yyyy");
                dt.Clear();
                dt = cn.BuscaFechaActual(fecha);
                dataRecibos.ItemsSource = dt.DefaultView;
            }
            else if (chkPresente.IsChecked == false)
            {
                BuscarRecibo();
            }
        }

        private void BuscarRecibo()
        {
            dt.Clear();
            dt = cn.BuscarRecibo(txtBuscar.Text);
            dataRecibos.ItemsSource = dt.DefaultView;
        }

        private void ConsultarPorMes()
        {
            string mes = cmbMeses.SelectedItem.ToString();
            switch (mes)
            {
                case "Enero": mes = "1"; break;
                case "Febrero": mes = "2"; break;
                case "Marzo": mes = "3"; break;
                case "Abril": mes = "4"; break;
                case "Mayo": mes = "5"; break;
                case "Junio": mes = "6"; break;
                case "Julio": mes = "7"; break;
                case "Agosto": mes = "8"; break;
                case "Septiembre": mes = "9"; break;
                case "Octubre": mes = "10"; break;
                case "Noviembre": mes = "11"; break;
                case "Diciembre": mes = "12"; break;
                default: mes = ""; break;

            }
            if (mes != "")
            {
                dt.Clear();
                dt = cn.BuscarMesRecibo(mes);
                dataRecibos.ItemsSource = dt.DefaultView;
            }
            else
            {
                BuscarRecibo();
            }
        }

        private void ConsultarPorMesAño()
        {
            string mes = cmbMeses.SelectedItem.ToString();
            string fecha = DateTime.Now.ToString("yyyy");

            if (mes != "" && chkPresente.IsChecked==true)
            {
                switch (mes)
                {
                    case "Enero": mes = "1"; break;
                    case "Febrero": mes = "2"; break;
                    case "Marzo": mes = "3"; break;
                    case "Abril": mes = "4"; break;
                    case "Mayo": mes = "5"; break;
                    case "Junio": mes = "6"; break;
                    case "Julio": mes = "7"; break;
                    case "Agosto": mes = "8"; break;
                    case "Septiembre": mes = "9"; break;
                    case "Octubre": mes = "10"; break;
                    case "Noviembre": mes = "11"; break;
                    case "Diciembre": mes = "12"; break;
                    default: mes = ""; break;

                }
                dt.Clear();
                dt = cn.BuscarMesAñoRecibo(mes, fecha);
                dataRecibos.ItemsSource = dt.DefaultView;
            }
            else if (mes != "")
            {
                ConsultarPorMes();
            }else if (chkPresente.IsChecked == true)
            {
                ConsultarPorAño();
            }
        }

        private void mesesCombo()
        {
            List<string> meses = new List<string>();
            meses.Add("");
            meses.Add("Enero");
            meses.Add("Febrero");
            meses.Add("Marzo");
            meses.Add("Abril");
            meses.Add("Mayo");
            meses.Add("Junio");
            meses.Add("Julio");
            meses.Add("Agosto");
            meses.Add("Septiembre");
            meses.Add("Octubre");
            meses.Add("Noviembre");
            meses.Add("Diciembre");
            cmbMeses.ItemsSource = meses;
            
        }

        private bool valorCheckbox()
        {
            bool valor = false;
            if(chkPresente.IsChecked == true)
            {
                valor = true;
                return valor;
            }
            else
            {
                return valor;
            }
        }
    }
}
