using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021
{
    class ControlBD 
    {
        SQLiteConnection cn;
      //  List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
        GestionClientes.Cliente cliente = new GestionClientes.Cliente();
        DataTable dt = new DataTable();
        


        public ControlBD()
        {

            cn = new SQLiteConnection("data source=C:/FYSIEX/FYSIEX.db");
            //cn = new SQLiteConnection("data source=C:/Users/Gabri/Desktop/dsi/v6/ProyectoDSI115-G5-2021/ProyectoDSI115-G5-2021/FYSIEX.db");

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
                MessageBox.Show("Agregar cliente "+ex.Message.ToString());
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
        public DataTable BuscarCliente(string nombrecliente)
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT * from CLIENTE WHERE NOMBRE_CLIENTE LIKE @nombre AND ESTADO_CLIENTE='Activo';", cn);
                comando.Parameters.Add(new SQLiteParameter("@nombre", nombrecliente + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);
                
                
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

        public List<TipoUsuario> ConsultarTipoUsuario()
        {
            //Usar SELECT cod_tipousuario,nombre_tipousuario FROM tipo_usuario
            List<TipoUsuario> tipos = new List<TipoUsuario>();
            string comandoString = "SELECT cod_tipousuario,nombre_tipousuario FROM tipo_usuario";
            cn.Open();
            SQLiteCommand sqlCmd = new SQLiteCommand(comandoString, cn);
            SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                //Agregando tipos de usuario a lista de datos
                tipos.Add(new TipoUsuario(sqlReader["cod_tipousuario"].ToString(), sqlReader["nombre_tipousuario"].ToString()));
            }
            sqlReader.Close();
            //Cerrando conexión. Asegúrese de cerrar antes de salir de un método.
            cn.Close();
            return tipos;
        }

        public List<EmpleadoItem> ConsultarEmpleadosLista()
        {
            //Se usa una versión reducida de empleado para este caso
            List<EmpleadoItem> empleados = new List<EmpleadoItem>();
            //Sólo se buscan los atributos de empleados activos
            string comandoString = "SELECT e.cod_empleado,e.nombre_empleado,e.apellido_empleado,a.nombre_area as narea,c.nombre_area as ncargo FROM (empleado AS e INNER JOIN cargo AS c ON e.cod_cargo = c.cod_cargo) INNER JOIN area AS a ON e.cod_area = a.cod_area WHERE estado_empleado='Activo' AND NOT e.cod_cargo = 'ST'";
            //Abriendo conexión a BD
            cn.Open();
            SQLiteCommand sqlCmd = new SQLiteCommand(comandoString, cn);
            SQLiteDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                //Se agregan los empleados activos y el campo de texto toma atributos relevantes del empleado.
                empleados.Add(new EmpleadoItem(sqlReader["cod_empleado"].ToString(), sqlReader["nombre_empleado"].ToString()+" "+sqlReader["apellido_empleado"].ToString()+" ("+ sqlReader["ncargo"].ToString()+", "+sqlReader["narea"].ToString()+")"));
            }
            sqlReader.Close();
            //Cerrando conexión. Asegúrese de cerrar antes de salir de un método.
            cn.Close();
            return empleados;
        }

        public DataTable ConsultarUsuarios()
        {
            //Inicializando tabla de datos
            dt = new DataTable();
            //
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT cod_usuario, correo_usuario, t.nombre_tipousuario as NOM_TIPOUSUARIO, e.nombre_empleado as NOM_EMPLEADO, e.apellido_empleado as APE_EMPLEADO FROM (usuario as u INNER JOIN tipo_usuario AS t ON u.cod_tipousuario = t.cod_tipousuario) INNER JOIN empleado AS e ON u.cod_empleado = e.cod_empleado WHERE NOT estado_usuario = 'O'", cn);
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

        public String AgregarUsuario(Usuario usuario, string contra)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("INSERT INTO usuario(cod_usuario,cod_tipousuario,cod_empleado,correo_usuario,contrasena_usuario,estado_usuario) VALUES(@idu,@tu,@cem,@correo,@contra,'D')", cn);
                comando.Parameters.Add(new SQLiteParameter("@idu", usuario.codigo));
                comando.Parameters.Add(new SQLiteParameter("@tu", usuario.tipoUsuario.codTipoUsuario));
                comando.Parameters.Add(new SQLiteParameter("@cem", usuario.empleado));
                comando.Parameters.Add(new SQLiteParameter("@correo", usuario.correoElectronico));
                comando.Parameters.Add(new SQLiteParameter("@contra", contra));
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
            return "Usuario registrado correctamente";
        }

        public Usuario CrearSesion(string usuario, string contrasena)
        {
            Usuario sesion = new Usuario();
            string comando = "SELECT cod_usuario, correo_usuario, u.cod_tipousuario as cod_tipo, t.nombre_tipousuario as tipo, e.nombre_empleado as nom_empleado, e.apellido_empleado as ape_empleado, contrasena_usuario FROM (usuario as u INNER JOIN tipo_usuario as t ON u.cod_tipousuario = t.cod_tipousuario) INNER JOIN empleado AS e ON u.cod_empleado = e.cod_empleado WHERE correo_usuario='" + usuario+ "'";
            cn.Open();
            SQLiteCommand iniciarCmd = new SQLiteCommand(comando, cn);
            SQLiteDataReader iniciarReader = iniciarCmd.ExecuteReader();
            if (!iniciarReader.HasRows)
            {
                sesion = null;
            }
            else
            {
                while (iniciarReader.Read())
                {
                    if (contrasena.Equals(iniciarReader["contrasena_usuario"].ToString()))
                    {
                        sesion.codigo = iniciarReader["cod_usuario"].ToString();
                        sesion.correoElectronico = iniciarReader["correo_usuario"].ToString();
                        sesion.empleado = iniciarReader["nom_empleado"].ToString() + " " + iniciarReader["ape_empleado"].ToString();
                        sesion.tipoUsuario = new TipoUsuario(iniciarReader["cod_tipo"].ToString(), iniciarReader["tipo"].ToString());
                    }
                    else
                    {
                        sesion = null;
                    }
                }
            }
            iniciarReader.Close();
            cn.Close();
            return sesion;
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
