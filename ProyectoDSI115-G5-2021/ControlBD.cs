using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;



namespace ProyectoDSI115_G5_2021
{
    class ControlBD 
    {
        SQLiteConnection cn;
        public ControlBD()
        {
            cn = new SQLiteConnection("data source=C:/Users/Gabri/Desktop/dsi/v2/ProyectoDSI115-G5-2021/ProyectoDSI115-G5-2021/BD.db");
        }
        public List<GestionClientes.Cliente> consultarClientes()
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            GestionClientes.Cliente cliente = new GestionClientes.Cliente();
            try {
                cn.Open();
                SQLiteCommand da = new SQLiteCommand("select * from cliente", cn);
                SQLiteDataReader dr = da.ExecuteReader();
               
                while (dr.Read())
                {
                    cliente.codigo = Convert.ToString(dr[0]);
                    cliente.codservicio = Convert.ToString(dr[1]);
                    cliente.nombres = Convert.ToString(dr[2]);
                    cliente.apellidos = Convert.ToString(dr[3]);
                    cliente.empresa = Convert.ToString(dr[4]);
                    clientes.Add(cliente);

                }
                
            } catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                cn.Close();

            }
            cn.Close();
            return clientes;
           
        }

    }




}
