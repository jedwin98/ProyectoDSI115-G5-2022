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
using System.Media;


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
        Historial.HistorialCliente historial;
        CotizacionRecibo.Cotizacion cotizacion;
        CotizacionRecibo.CrearRecibo recibo;
        Inventario verInventario;

        //RECORDAR BORRAR ESTO
        private SoundPlayer _soundPlayer;

        Nullable<bool> gca = false, gea = false, gua = false, inv = false, rb = false, cot = false;

        //CREADO ESPECIFICAMENTE PARA GENERAR NOTIFICACIONES 
        //AUTOR: FRANCISCO ESCOBAR
        SQLiteConnection con = new SQLiteConnection(@"data source=C:/FYSIEX/FYSIEX.db");
        //SQLiteConnection con = new SQLiteConnection(@"data source=//KATYA\fysiex\FYSIEX.db;Version=3;Compress=True;");      //CONEXION EN RED

        System.Windows.Threading.DispatcherTimer dispatcher = new System.Windows.Threading.DispatcherTimer(); //OBJETO PARA EJECUTAR CADA CIERTO TIEMPO UN METODO

        internal Usuario Sesion { get => sesion; set => sesion = value; }

        public MainWindow()
        {
            InitializeComponent();
            _soundPlayer = new SoundPlayer(@"C:\FYSIEX\music\take_on_me.wav");
        }
        
        //SOBREESCRIBO EL EVENTO CUANDO LA VENTANA PRINCIPAL ESTA ACTIVADA
        //AUTOR: FRANCISCO ESCOBAR
        private void MainWindows_Activated(object sender, System.EventArgs e)
        {
            //INICIA SIN SONIDO, SEGUIDO DEL MISMO METODO PERO CON SONIDO
            GenerarNotificacion();
            //Ejecutando Metodo cada 5 segundos
            // AL ATRIBUTO TICK LE ASIGNAMOS EL EVENTO DISPATCHERTIMER_TICK EN EL CUAL VA TODO EL CODIGO A EJECUTAR
            dispatcher.Tick += new EventHandler(dispatcherTimer_Tick);
            //ASIGNAMOS HORA SEGUN EL PATRON TIMESPAN (HORA, MINUTO, SEGUNDO)
            dispatcher.Interval = new TimeSpan(0,0,5);
            //INICIAMOS EL METODO
            dispatcher.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //ACA SE INGRESA  TODO EL CODIGO QUE SE QUIERE EJECUTAR CADA CIERTO TIEMPO
            GenerarNotificacionSonido();
            
        }

        //MUESTRA U OCULTA LOS CONTROLES SI EXISTEN O NO NOTIFICACIONES
        //AUTOR: FRANCISCO ESCOBAR

        
        //EL METODO ACTUAL GENERA UNA NOTIFICACION Y MANDA UN SONIDO DE ALERTA
        public void GenerarNotificacionSonido()
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                imgBurbuja.Visibility = Visibility.Hidden;
                lblContador.Visibility = Visibility.Hidden;

                int notificaciones = ContarRegistros();

                if (notificaciones != 0)
                {
                    imgBurbuja.Visibility = Visibility.Visible;
                    lblContador.Visibility = Visibility.Visible;
                    lblContador.Content = notificaciones.ToString();
                    SystemSounds.Beep.Play();
                    dispatcher.Stop();

                    //Añadiendo mas tiempo para alertar de la solicitud
                    dispatcher.Interval = new TimeSpan(0, 0, 30);
                    dispatcher.Start();
                    //NOTA: No se detendra el sonido hasta verificar la solicitud existente

                }
                else
                {
                    imgBurbuja.Visibility = Visibility.Hidden;
                    lblContador.Visibility = Visibility.Hidden;
                    lblContador.Content = "0";
                }

            }
        }

        //UNICAMENTE PARA PRECARGARSE AL ACTIVAR LA VENTANA
        public void GenerarNotificacion()
        {
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                imgBurbuja.Visibility = Visibility.Hidden;
                lblContador.Visibility = Visibility.Hidden;

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

        private void Reproductor_Click(object sender, RoutedEventArgs e)
        {
            if (reproductor.Content.ToString() == "Play")
            {
                
                
                _soundPlayer.Play();
                reproductor.Content = "Stop";
            }
            else
            {
                _soundPlayer.Stop();
                reproductor.Content = "Play";
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
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                historial=new Historial.HistorialCliente
                { 
                    WindowState = WindowState.Maximized
                };
               
                historial.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        
        private void BtnCotizacion_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Cotizaciones.
            //Autorizado para gerencia y administración.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                cotizacion = new CotizacionRecibo.Cotizacion
                {
                    WindowState = WindowState.Normal
                };
                //Aunque la función bloquea las acciones en esta ventana
                //Se tiene esta variable que se define al cerrar la ventana
                cot = cotizacion.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //CREADOR DE BOTÓN: Gustavo Ernesto H. Corvera
        private void BtnRecibo_Click(object sender, RoutedEventArgs e)
        {
            //Autorizado para gerencia y administración.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                recibo = new CotizacionRecibo.CrearRecibo
                {
                    WindowState = WindowState.Normal
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
