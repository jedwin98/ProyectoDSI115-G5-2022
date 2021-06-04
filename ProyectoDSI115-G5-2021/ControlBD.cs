using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data;
using System.Windows.Controls;

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

            cn = new SQLiteConnection("data source=D:/FYSIEX.db");

        }
     

        public  DataTable consultarClientes()
        {
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * from CLIENTE WHERE ESTADO_CLIENTE='Activo'", cn);
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

        public String agregarCliente(GestionClientes.Cliente client)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("INSERT INTO CLIENTE (COD_CLIENTE,NOMBRE_CLIENTE,APELLIDO_CLIENTE,EMPRESA_CLIENTE, TELEFONO_CLIENTE, ESTADO_CLIENTE) VALUES (@id,@nombre,@ape,@empre,@tele,@est)", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", client.codigo));
                comando.Parameters.Add(new SQLiteParameter("@nombre", client.nombres));
                comando.Parameters.Add(new SQLiteParameter("@ape", client.apellidos));
                comando.Parameters.Add(new SQLiteParameter("@empre", client.empresa));
                comando.Parameters.Add(new SQLiteParameter("@tele", client.telefono));
                comando.Parameters.Add(new SQLiteParameter("@est", client.estado));
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error";

            }
            cn.Close();
            return "Cliente Registrado correctamente";
        }
        public String eliminarCliente(String client)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("UPDATE Cliente SET ESTADO_CLIENTE = 'Oculto' WHERE COD_CLIENTE = @id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", client));
                
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error";

            }
            cn.Close();
            return "Cliente Eliminado correctamente";
        }
        public String actualizarCliente(GestionClientes.Cliente client)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("UPDATE Cliente SET NOMBRE_CLIENTE= @nombre, APELLIDO_CLIENTE =@ape, EMPRESA_CLIENTE=@empre, TELEFONO_CLIENTE=@tele WHERE COD_CLIENTE = @id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", client.codigo));
                comando.Parameters.Add(new SQLiteParameter("@nombre", client.nombres));
                comando.Parameters.Add(new SQLiteParameter("@ape", client.apellidos));
                comando.Parameters.Add(new SQLiteParameter("@empre", client.empresa));
                comando.Parameters.Add(new SQLiteParameter("@tele", client.telefono));
               
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error";

            }
            cn.Close();
            return "Cliente Actualizado correctamente";
        }

        public ComboBox ConsultarTipoUsuario(ComboBox cbx)
        {
            //Usar SELECT cod_tipousuario,nombre_tipousuario FROM tipo_usuario
            string comandoString = "SELECT cod_tipousuario,nombre_tipousuario FROM tipo_usuario";
            SQLiteCommand sqlCmd = new SQLiteCommand(comandoString, cn);
            cn.Open();
            SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                cbx.Items.Add(sqlReader["nombre_tipousuario"].ToString());
            }
            sqlReader.Close();
            return cbx;
        }

        /*

                public List<GestionClientes.TipoServicio> consultarTipoServicio()
                {
                    List<GestionClientes.TipoServicio> tipoServ = new List<GestionClientes.TipoServicio>();
                    try
                    {
                        cn.Open();
                        SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * from  TIPOSERVICIO ", cn);
                        da.Fill(dt);

                       foreach(DataRow row in dt.Rows){
                            GestionClientes.TipoServicio ts = new GestionClientes.TipoServicio();
                            ts.id = row.ItemArray[0].ToString();
                            ts.nombre = row.ItemArray[1].ToString();
                            tipoServ.Add(ts);

                        }
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                        cn.Close();
                    }

                    cn.Close();
                    return tipoServ;
                }*/
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
