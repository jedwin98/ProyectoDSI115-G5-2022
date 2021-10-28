using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProyectoDSI115_G5_2021.GestionUsuarios;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;

namespace ProyectoDSI115_G5_2021
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Usuario sesion;
        GestionClientes.GestionClientes gc;
        GestionEmpleados.GestionEmpleados ge;
        GestionUsuarios.GestionUsuarios gu;
        Inventario verInventario;
        //Historial.Historial historial;
        //CotizacionRecibo.Cotizacion cotizacion;
        CotizacionRecibo.CrearRecibo recibo;



        Nullable<bool> gca = false, gea = false, gua = false, inv = false, rb = false;


        //CREADO ESPECIFICAMENTE PARA GENERAR NOTIFICACIONES 
        //AUTOR: FRANCISCO ESCOBAR
        SQLiteConnection con = new SQLiteConnection(@"data source=C:/FYSIEX/FYSIEX.db");
        
        internal Usuario Sesion { get => sesion; set => sesion = value; }

        public MainWindow()
        {
            InitializeComponent();
            
            
        }


        //SOBREESCRIBO EL EVENTO CUANDO LA VENTANA PRINCIPAL ESTA ACTIVADA
        //AUTOR: FRANCISCO ESCOBAR
        private void MainWindows_Activated(object sender, System.EventArgs e)
        {
            GenerarNotificacion();
        }

        //MUESTRA U OCULTA LOS CONTROLES SI EXISTEN O NO NOTIFICACIONES
        //AUTOR: FRANCISCO ESCOBAR
        public void GenerarNotificacion()
        {
            int notificaciones = ContarRegistros();
            if (notificaciones != 0)
            {
                imgBurbuja.Visibility = Visibility.Visible;
                lblContador.Visibility = Visibility.Visible;
                lblContador.Content = notificaciones.ToString();
                
            }
            else
            {
                imgBurbuja.Visibility = Visibility.Hidden;
                lblContador.Visibility = Visibility.Hidden;
                lblContador.Content = "0";
            } 
        }
        //REALIZA EL PROCESO DE CREAR UN OBJETO DATA TABLE Y CUENTA LAS FILAS DE LA TABLA
        //LUEGO ASIGNA ESE VALOR A UNA VARIABLE Y ESTA SE RETORNA
        //AUTOR: FRANCISCO ESCOBAR
        public int ContarRegistros()
        {
            int filas;
            DataTable data = new DataTable();

           con.Open();
           string sql = @"SELECT ESTADO_SOLICITUD FROM SOLICITUD_INSUMO WHERE ESTADO_SOLICITUD = 'Pendiente' ";
           SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con);
           da.Fill(data);
           filas = data.Rows.Count;
           con.Close();
           return filas;

        }

        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Usuarios
            //Solo puede entrar la gerencia.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                gu = new GestionUsuarios.GestionUsuarios
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gu.sesion = this.sesion;
                gua = gu.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.","Error de acceso",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Clientes
            //Autorizado para gerencia y administración.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                gc = new GestionClientes.GestionClientes
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gca = gc.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            //Botón para cierre de ventana.
            //Cerrando desde la barra de título también activará el proceso de cierre.
            this.Close();
        }

        private void CerrarSesion()
        {
            //¿Cómo manejar las sesiones en la BD?
        }

        private void BtnInsumos_Click(object sender, RoutedEventArgs e)
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                Autorizacion.ConsultarSolicitudes solicitudes = new Autorizacion.ConsultarSolicitudes
                {
                    WindowState = WindowState.Maximized
                };
                solicitudes.sesion = this.sesion;
                solicitudes.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //Al cerrar la ventana, la sesión debe cerrarse.
            //Después, debe mostrar la ventana de inicio de sesión.
            bool gcr = gca ?? false;
            bool ger = gea ?? false;
            bool gur = gua ?? false;
            if (!gcr && !ger && !gur)
            {
                ControlBD control = new ControlBD();
                control.Desbloquear(sesion.correoElectronico);
                Login lg = new Login();
                lg.Show();
            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("Cierre sus ventanas y guarde su trabajo antes de salir.","Sesión activa",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                ge = new GestionEmpleados.GestionEmpleados()
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                gea = ge.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnInventario_Click(object sender, RoutedEventArgs e)
        {
            verInventario = new Inventario(sesion.tipoUsuario.codTipoUsuario, sesion.codigoEmpleado)
            {
                WindowState = WindowState.Maximized
                
        };
            verInventario.Sesion = this.sesion;
            inv = verInventario.ShowDialog();
        }

        //MODIFICAR ESTE CODIGO.
        private void BtnHistorial_Click(object sender, RoutedEventArgs e)
        {
            
        }
        //MODIFICAR ESTE CODIGO
        private void BtnCotizacion_Click(object sender, RoutedEventArgs e)
        {
            
        }
        //MODIFICAR ESTE CODIGO
        private void BtnRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                recibo = new CotizacionRecibo.CrearRecibo
                {
                    WindowState = WindowState.Maximized
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                rb = recibo.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
