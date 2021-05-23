using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data;

namespace ProyectoDSI115_G5_2021
{
    class ControlBD 
    {
        SQLiteConnection cn;
        List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
        GestionClientes.Cliente cliente = new GestionClientes.Cliente();
        DataTable dt = new DataTable();
        public ControlBD()
        {
            cn = new SQLiteConnection("data source=C:/Users/Gabri/Desktop/dsi/v2/ProyectoDSI115-G5-2021/ProyectoDSI115-G5-2021/BD.db");
        }
     

        public  DataTable consultarClientes()
        {
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from cliente", cn);
                da.Fill(dt);
              
                
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Console.WriteLine();
                cn.Close();

            }
            
            cn.Close();
            return dt;           
       }
        /*   public List<GestionClientes.Cliente> consultarClientes()
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
                    MessageBox.Show(cliente.ToString());
                    clientes.Add(cliente);

                }
                
            } catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
              //  Console.WriteLine();
                cn.Close();

            }
            cn.Close();
            MessageBox.Show("funcionó ?");
            return clientes;
           
        }*/



    }




}
