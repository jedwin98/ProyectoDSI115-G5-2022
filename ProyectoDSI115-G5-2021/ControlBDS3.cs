using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProyectoDSI115_G5_2021.SolicitarInsumos;

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
                    clientes.Add(new GestionClientes.Cliente(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]), Convert.ToString(dr[5])));  //se realiza de esta forma para evitar los datos replicados en la lista            
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
                string comando = "SELECT s.COD_SOLICITUD, s.COD_EMPLEADO,e.NOMBRE_EMPLEADO, e.APELLIDO_EMPLEADO," +
                    "s.FECHA_SOLICITUD, s.ESTADO_SOLICITUD, s.COD_REQ," +
                    " s.EMP_COD_EMPLEADO, e2.NOMBRE_EMPLEADO AS NAPR, e2.APELLIDO_EMPLEADO AS AEMP" +
                    " FROM SOLICITUD_INSUMO AS s  INNER JOIN EMPLEADO AS e INNER JOIN EMPLEADO AS e2 INNER JOIN CLIENTE AS c" +
                    " ON s.COD_CLIENTE=@cod AND c.COD_CLIENTE=@cod AND (s.EMP_COD_EMPLEADO= e2.COD_EMPLEADO)" +
                    " AND (s.COD_EMPLEADO= e.COD_EMPLEADO) AND ESTADO_SOLICITUD='Aprobado'";
                SQLiteCommand comand = new SQLiteCommand(comando, cn);
                comand.Parameters.Add(new SQLiteParameter("@cod", codigoCliente));

                SQLiteDataReader dr = comand.ExecuteReader();
                while (dr.Read())
                {
                    //Console.WriteLine(Convert.ToString(dr[1]));
                   // solicitudes.Add(new GestionClientes.Cliente(Convert.ToString(dr[0]), Convert.ToString(dr[1]), Convert.ToString(dr[2]), Convert.ToString(dr[3]), Convert.ToString(dr[4]), Convert.ToString(dr[5])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();

                // adapter.Fill(dt);
                //  MessageBox.Show(dt.Rows[0][1].ToString());
                // MessageBox.Show("entró" + codigoEmpleado);

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar tabla de solicitudes " + ex.Message.ToString());

                cn.Close();
            }
            cn.Close();
            //   MessageBox.Show(dt.Rows[1][1].ToString());
            return solicitudes;
        }

    }
}
