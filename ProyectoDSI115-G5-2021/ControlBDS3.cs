using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProyectoDSI115_G5_2021.SolicitarInsumos;
using ProyectoDSI115_G5_2021.GestionUsuarios;
using ProyectoDSI115_G5_2021.GestionMateriales;

namespace ProyectoDSI115_G5_2021
{
    class ControlBDS3
    {
        SQLiteConnection cn;           
        DataTable dt = new DataTable();

        public ControlBDS3()
        {
            //cn = new SQLiteConnection(@"Data Source=Z:\FYSIEX.db;Version=3;Compress=True;");     // CONEXION EN UNIDAD DE RED
            //cn = new SQLiteConnection(@"data source=//KATYA\fysiex\FYSIEX.db;Version=3;Compress=True;");      //CONEXION EN RED
            cn = new SQLiteConnection(@"data source=C:/FYSIEX/FYSIEX.db");   //CONEXION NORMAL

        }

        public List<GestionClientes.Cliente> ListaClientes()//regresa el listado de clientes
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            try
            {
                cn.Open();
                string comando = "SELECT * FROM  CLIENTE WHERE ESTADO_CLIENTE='Activo'";
                SQLiteCommand command = new SQLiteCommand(comando, cn);
                SQLiteDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    //Console.WriteLine(Convert.ToString(dr[1]));
                    clientes.Add(new GestionClientes.Cliente(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]), Convert.ToString(dr[6])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar los clientes " + ex.Message.ToString());
                
                cn.Close();
            }

            cn.Close();
            return clientes;
        }
        public List<SolicitudInsumos> SolicitudesDelCliente(string codigoCliente)
        {

            List<SolicitudInsumos> solicitudes = new List<SolicitudInsumos>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                string comando = "SELECT s.COD_SOLICITUD, s.COD_EMPLEADO, e.NOMBRE_EMPLEADO, e.APELLIDO_EMPLEADO," +
                    "s.FECHA_SOLICITUD, s.ESTADO_SOLICITUD, s.COD_REQ," +
                    " s.EMP_COD_EMPLEADO, e2.NOMBRE_EMPLEADO AS NAPR, e2.APELLIDO_EMPLEADO AS AEMP" +
                    " FROM SOLICITUD_INSUMO AS s  INNER JOIN EMPLEADO AS e INNER JOIN EMPLEADO AS e2 INNER JOIN CLIENTE AS c" +
                    " ON s.COD_CLIENTE=@cod AND c.COD_CLIENTE=@cod AND (s.EMP_COD_EMPLEADO= e2.COD_EMPLEADO)" +
                    " AND (s.COD_EMPLEADO= e.COD_EMPLEADO) AND ESTADO_SOLICITUD='Aprobado'";
                SQLiteCommand comand = new SQLiteCommand(comando, cn);
                comand.Parameters.Add(new SQLiteParameter("@cod", codigoCliente));

                SQLiteDataReader dr = comand.ExecuteReader();
                while (dr.Read())
                {                                       //codigo, nombre´+apellido
                    Usuario solicitante = new Usuario(Convert.ToString(dr[1]), Convert.ToString(dr[2]) +" "+ Convert.ToString(dr[3]));
                    Usuario aprobador = new Usuario(Convert.ToString(dr[7]), Convert.ToString(dr[8]) + " " + Convert.ToString(dr[9]));
                    
                                                        //codigo generado solicitud, solicitante, aprobador,fecha, codigomanual
                    solicitudes.Add(new SolicitudInsumos(Convert.ToString(dr[0]),solicitante,aprobador, Convert.ToString(dr[4]), Convert.ToString(dr[6])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar tabla de solicitudes " + ex.Message.ToString());

                cn.Close();
            }
            cn.Close();
            
            return solicitudes;
        }
        public List<DetalleSolicitudInsumos> ConsultarDetalleSolicitudes(string codigoSolicitud)//regresa el detalle de la solicitud seleccionada
        {
            List<DetalleSolicitudInsumos> detalles = new List<DetalleSolicitudInsumos>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                string comando= "SELECT  d.COD_DETALLE," +
                    " d.COD_MATERIAL, m.NOMBRE_MATERIAL, m.UNIDAD_MEDIDA_MATERIAL, " +
                    "d.COD_SOLICITUD, d.CANTIDAD_DETALLE" +
                    " FROM DETALLE_SOLICITUD_INSUMO AS d INNER JOIN  MATERIAL AS m  WHERE d.COD_SOLICITUD=@codSolicitud AND d.COD_MATERIAL=m.COD_MATERIAL" +
                    " UNION SELECT d.COD_DETALLE," +
                    " d.COD_MATERIAL, p.NOMBRE_PRODUCTO AS NOMBRE_MATERIAL, p.UNIDAD_MEDIDA_PRODUCTO AS UNIDAD_MEDIDA_MATERIAL," +
                    "d.COD_SOLICITUD, d.CANTIDAD_DETALLE " +
                    "FROM DETALLE_SOLICITUD_INSUMO AS d INNER JOIN  PRODUCTO AS p  WHERE d.COD_SOLICITUD=@codSolicitud AND d.COD_MATERIAL=p.COD_PRODUCTO";

                SQLiteCommand comand = new SQLiteCommand(comando, cn);
                comand.Parameters.Add(new SQLiteParameter("@codSolicitud", codigoSolicitud));
                adapter.SelectCommand = comand;
                SQLiteDataReader dr = comand.ExecuteReader();
                while (dr.Read())
                {                                       //codigo, nombre, presentacion
                    Material material = new Material(Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]));
                    
                    //MessageBox.Show(solicitante.empleado);
                    //codigo, codigo de solicitud , material, cantidad solicitada
                    detalles.Add(new DetalleSolicitudInsumos(Convert.ToString(dr[0]), Convert.ToString(dr[4]),material,Convert.ToSingle(dr[5])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar el detalle de las solicitudes " + ex.Message.ToString());

                cn.Close();
            }
            cn.Close();
            return detalles;
        }
        public List<GestionClientes.Cliente> BuscarCliente(string nombreCliente)
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comand = new SQLiteCommand("SELECT * from CLIENTE WHERE NOMBRE_CLIENTE LIKE @nombre OR EMPRESA_CLIENTE LIKE @nombre AND ESTADO_CLIENTE='Activo';", cn);
                comand.Parameters.Add(new SQLiteParameter("@nombre", "%" + nombreCliente + "%"));
                adapter.SelectCommand = comand;
                SQLiteDataReader dr = comand.ExecuteReader();

                while (dr.Read())
                {
                    //Console.WriteLine(Convert.ToString(dr[1]));
                    clientes.Add(new GestionClientes.Cliente(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]), Convert.ToString(dr[6])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();


            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar cliente " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return clientes;

        }

    }
}
