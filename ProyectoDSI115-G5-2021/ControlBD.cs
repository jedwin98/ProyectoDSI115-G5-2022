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
using System.Data.SqlClient;

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
            //cn = new SQLiteConnection(@"Data Source=Z:\FYSIEX.db;Version=3;Compress=True;");     // CONEXION EN UNIDAD DE RED
            //cn = new SQLiteConnection(@"data source=//Frank-PC\fysiex\FYSIEX.db;Version=3;Compress=True;");      //CONEXION EN RED
            cn = new SQLiteConnection("data source=C:/FYSIEX/FYSIEX.db");   //CONEXION NORMAL

        }


        public  DataTable ConsultarClientes()
        {
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * from CLIENTE WHERE ESTADO_CLIENTE='Activo'", cn);
                da.Fill(dt);           
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar tabla de clientes "+ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }            
            cn.Close();
            return dt;           
       }

        public String AgregarCliente(GestionClientes.Cliente client)
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
               
                
                cn.Close();
                return "Ha ocurrido un error al agregar cliente "+ex.Message.ToString();

            }
            cn.Close();
            return "Cliente Registrado correctamente";
        }
        public String EliminarCliente(String client)
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
                
                cn.Close();
                return "Ha ocurrido un error al eliminar cliente " + ex.Message.ToString();


            }
            cn.Close();
            return "Cliente Eliminado correctamente";
        }
        public String ActualizarCliente(GestionClientes.Cliente client)
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
                
                cn.Close();
                return "Ha ocurrido un error al actualizar cliente " + ex.Message.ToString();


            }
            cn.Close();
            return "Cliente Actualizado correctamente";
        }
        public DataTable BuscarCliente(string nombreCliente)
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT * from CLIENTE WHERE NOMBRE_CLIENTE LIKE @nombre AND ESTADO_CLIENTE='Activo';", cn);
                comando.Parameters.Add(new SQLiteParameter("@nombre","%"+ nombreCliente + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);
                
                
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar cliente "+ex.Message.ToString());
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
            string comandoString = "SELECT e.cod_empleado,e.nombre_empleado,e.apellido_empleado,a.nombre_area as narea,c.nombre_cargo as ncargo FROM (empleado AS e INNER JOIN cargo AS c ON e.cod_cargo = c.cod_cargo) INNER JOIN area AS a ON e.cod_area = a.cod_area WHERE estado_empleado='Activo' AND NOT(e.cod_cargo = 'ST' AND e.cod_area = 'L')";
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
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT cod_usuario, correo_usuario, t.cod_tipousuario as COD_TIPOUSUARIO, t.nombre_tipousuario as NOM_TIPOUSUARIO, u.cod_empleado as COD_EMPLEADO, e.nombre_empleado as NOM_EMPLEADO, e.apellido_empleado as APE_EMPLEADO FROM (usuario as u INNER JOIN tipo_usuario AS t ON u.cod_tipousuario = t.cod_tipousuario) INNER JOIN empleado AS e ON u.cod_empleado = e.cod_empleado WHERE NOT estado_usuario = 'O'", cn);
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
                return "Ha ocurrido un error. Verifique su conexión e intente de nuevo.";
            }
            cn.Close();
            return "Usuario registrado correctamente";
        }

        public bool VerificarCorreo(string correo)
        {
            bool valido = false;
            try
            {
                cn.Open();
                SQLiteCommand conteo = new SQLiteCommand("SELECT COUNT(*) as cuenta FROM usuario WHERE correo_usuario=@correo AND NOT estado_usuario = 'O'", cn);
                conteo.Parameters.Add(new SQLiteParameter("@correo", correo));
                SQLiteDataReader sqlReader = conteo.ExecuteReader();
                while (sqlReader.Read())
                {
                    int cuentas = int.Parse(sqlReader["cuenta"].ToString());
                    if (cuentas > 0) valido = false;
                    else valido = true;
                }
                sqlReader.Close();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
                return false;
            }
            return valido;
        }

        public Usuario CrearSesion(string usuario, string contrasena)
        {
            //Se selecciona al usuario a partir del correo obtenido
            //Se asegura que la contraseña coincida con la BD.
            Usuario sesion = new Usuario();
            string comando = "SELECT cod_usuario, correo_usuario, u.cod_tipousuario as cod_tipo, t.nombre_tipousuario as tipo, e.nombre_empleado as nom_empleado, e.apellido_empleado as ape_empleado, contrasena_usuario, estado_usuario FROM (usuario as u INNER JOIN tipo_usuario as t ON u.cod_tipousuario = t.cod_tipousuario) INNER JOIN empleado AS e ON u.cod_empleado = e.cod_empleado WHERE correo_usuario='" + usuario+ "' AND NOT estado_usuario = 'O'";
            cn.Open();
            SQLiteCommand iniciarCmd = new SQLiteCommand(comando, cn);
            SQLiteDataReader iniciarReader = iniciarCmd.ExecuteReader();
            if (!iniciarReader.HasRows)
            {
                //Si no hay usuarios registrados, se retorna null y se maneja en login.
                sesion = null;
            }
            else
            {
                while (iniciarReader.Read())
                {
                    //
                    if (contrasena.Equals(iniciarReader["contrasena_usuario"].ToString()))
                    {
                        //No se pasa la contraseña, sólo se compara
                        sesion.codigo = iniciarReader["cod_usuario"].ToString();
                        sesion.correoElectronico = iniciarReader["correo_usuario"].ToString();
                        sesion.empleado = iniciarReader["nom_empleado"].ToString() + " " + iniciarReader["ape_empleado"].ToString();
                        sesion.tipoUsuario = new TipoUsuario(iniciarReader["cod_tipo"].ToString(), iniciarReader["tipo"].ToString());
                        sesion.estado = iniciarReader["estado_usuario"].ToString();
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
        public DataTable ConsultarEmpleados()
        {
            DataTable data = new DataTable();

            try
            {
                cn.Open();
                string comando = "SELECT e.COD_EMPLEADO, e.COD_AREA, a.NOMBRE_AREA, e.COD_CARGO,c.NOMBRE_CARGO, e.NOMBRE_EMPLEADO, e.APELLIDO_EMPLEADO, e.FECHA_CONTRATACION,e.ESTADO_EMPLEADO FROM EMPLEADO AS e INNER JOIN AREA AS a INNER JOIN CARGO AS c  WHERE e.COD_AREA= a.COD_AREA AND e.COD_CARGO= c.COD_CARGO AND e.ESTADO_EMPLEADO = 'Activo'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(comando, cn);
                da.Fill(data);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al carga tabla de empleados "+ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return data;
        }

        public List<GestionEmpleados.Area> ConsultarArea()
        {
            List<GestionEmpleados.Area> areas = new List<GestionEmpleados.Area>();


            try
            {
                cn.Open();
                string comando = "SELECT * FROM  AREA";
                SQLiteCommand command = new SQLiteCommand(comando, cn);
                SQLiteDataReader dr = command.ExecuteReader();
                 
                while (dr.Read())
                {
                     
                    areas.Add(new GestionEmpleados.Area(Convert.ToString(dr[0]), Convert.ToString(dr[1])));  //se realiza de esta forma para evitar los datos replicados en la lista            
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar areas de trabajo "+ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            
            cn.Close();
            return areas;
        }
        public List<GestionEmpleados.Cargo> ConsultarCargo()
        {
            List<GestionEmpleados.Cargo> cargos = new List<GestionEmpleados.Cargo>();
            GestionEmpleados.Cargo cargo = new GestionEmpleados.Cargo();
            try
            {
                cn.Open();
                string comando = "SELECT * FROM  CARGO";
                SQLiteCommand command = new SQLiteCommand(comando, cn);
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    cargos.Add(new GestionEmpleados.Cargo(Convert.ToString(dr[0]), Convert.ToString(dr[1])));
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar cargos de trabajo "+ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return cargos;
        }
        public String AgregarEmpleado(GestionEmpleados.Empleado empleado)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("INSERT INTO EMPLEADO (COD_EMPLEADO, COD_AREA, COD_CARGO, NOMBRE_EMPLEADO, APELLIDO_EMPLEADO, FECHA_CONTRATACION, ESTADO_EMPLEADO) VALUES (@id,@idA,@idC,@nombre,@ape,@fecha,@est)", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", empleado.codigoEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@idA", empleado.areaE.codigoArea));
                comando.Parameters.Add(new SQLiteParameter("@idC", empleado.cargoE.codigoCargo));
                comando.Parameters.Add(new SQLiteParameter("@nombre", empleado.nombreEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@ape", empleado.apellidoEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@fecha", empleado.fechaContratacion));
                comando.Parameters.Add(new SQLiteParameter("@est", empleado.estadoEmpleado));
                comando.ExecuteNonQuery();
                    
            }
            catch (SQLiteException ex)
            {
                cn.Close();
                return "Ha ocurrido un error al agregar empleado " + ex.Message.ToString();

            }
            cn.Close();
            return "Empleado Registrado correctamente";
        }
      public String EliminarEmpleado(String idEmpleado)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("UPDATE EMPLEADO SET ESTADO_EMPLEADO = 'Oculto' WHERE COD_EMPLEADO = @id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", idEmpleado));

                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
               
                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error al Eliminar " + ex.Message.ToString();

            }
            cn.Close();
            return "Empleado Eliminado correctamente";
        }
       public String ActualizarEmpleado(GestionEmpleados.Empleado empleado)
   {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("UPDATE EMPLEADO SET COD_AREA = @idA, COD_CARGO= @idC, NOMBRE_EMPLEADO= @nombre, APELLIDO_EMPLEADO =@ape, FECHA_CONTRATACION=@fecha WHERE COD_EMPLEADO = @id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", empleado.codigoEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@idA", empleado.areaE.codigoArea));
                comando.Parameters.Add(new SQLiteParameter("@idC", empleado.cargoE.codigoCargo));
                comando.Parameters.Add(new SQLiteParameter("@nombre", empleado.nombreEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@ape", empleado.apellidoEmpleado));
                comando.Parameters.Add(new SQLiteParameter("@fecha", empleado.fechaContratacion));
                

                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                
               
                cn.Close();
                Console.WriteLine(ex.Message.ToString());
                return "Ha ocurrido un error al Actualizar " + ex.Message.ToString();
                

            }
            cn.Close();
            return "Empleado Actualizado correctamente";
        }
        public DataTable BuscarEmpleado(string nombreEmpleado)
        {
            List<GestionClientes.Cliente> clientes = new List<GestionClientes.Cliente>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT e.COD_EMPLEADO, e.COD_AREA, a.NOMBRE_AREA, e.COD_CARGO,c.NOMBRE_CARGO, e.NOMBRE_EMPLEADO, e.APELLIDO_EMPLEADO, e.FECHA_CONTRATACION,e.ESTADO_EMPLEADO FROM EMPLEADO AS e INNER JOIN AREA AS a INNER JOIN CARGO AS c  WHERE e.COD_AREA= a.COD_AREA AND e.COD_CARGO= c.COD_CARGO AND e.ESTADO_EMPLEADO = 'Activo' AND e.NOMBRE_EMPLEADO LIKE @nombre", cn);
                comando.Parameters.Add(new SQLiteParameter("@nombre", "%"+nombreEmpleado + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);


            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar empleado " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return dt;
         }


        public String EliminarUsuario(String usuario)
        {
            try
            {
                cn.Open();
                SQLiteCommand borra = new SQLiteCommand("UPDATE usuario SET estado_usuario='O' WHERE cod_usuario = @id", cn);
                borra.Parameters.Add(new SQLiteParameter("@id", usuario));
                borra.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException)
            {
                cn.Close();
                return "Ha ocurrido un error. Verifique su conexión e intente de nuevo.";
            }
            return "Se eliminó al usuario correctamente";
        }

        //Reservando para usuarios logueados.
 /*       public bool CambiarContrasena(String id, String contrasena)
        {
            try
            {
                cn.Open();
                SQLiteCommand cambio = new SQLiteCommand("UPDATE usuario SET contrasena_usuario = @contrasena WHERE cod_usuario = @id", cn);
                cambio.Parameters.Add(new SQLiteParameter("@contrasena", contrasena));
                cambio.Parameters.Add(new SQLiteParameter("@id", id));
                cambio.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                cn.Close();
                return false;
            }
            return true;
        }
        */
        public bool CambiarContrasenaEmail(String email, String contrasena)
        {
            try
            {
                cn.Open();
                SQLiteCommand cambio = new SQLiteCommand("UPDATE usuario SET contrasena_usuario = @contrasena WHERE correo_usuario = @email AND NOT estado_usuario = 'O'", cn);
                cambio.Parameters.Add(new SQLiteParameter("@contrasena", contrasena));
                cambio.Parameters.Add(new SQLiteParameter("@email", email));
                cambio.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException)
            {
                cn.Close();
                return false;
            }
            return true;
        }

        public DataTable BuscarUsuario(string clave)
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT cod_usuario, correo_usuario, t.cod_tipousuario as COD_TIPOUSUARIO, t.nombre_tipousuario as NOM_TIPOUSUARIO, u.cod_empleado as COD_EMPLEADO, e.nombre_empleado as NOM_EMPLEADO, e.apellido_empleado as APE_EMPLEADO FROM (usuario as u INNER JOIN tipo_usuario AS t ON u.cod_tipousuario = t.cod_tipousuario) INNER JOIN empleado AS e ON u.cod_empleado = e.cod_empleado WHERE estado_usuario = 'O' AND (correo_usuario LIKE @clave OR nom_empleado LIKE @clave OR ape_empleado LIKE @clave);", cn);
                comando.Parameters.Add(new SQLiteParameter("@clave", "%" + clave + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar usuario: " + ex.Message.ToString());
                cn.Close();
            }
            cn.Close();
            return dt;
        }

        public bool BuscarUsuarioActivo(string email)
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT cod_usuario FROM usuario WHERE estado_usuario = 'D' AND correo_usuario = @email", cn);
                comando.Parameters.Add(new SQLiteParameter("@email", email));
                adapter.SelectCommand = comando;
                dt.Clear();
                adapter.Fill(dt);
                cn.Close();
                if (dt.Rows.Count == 1) return true;
                else return false;
            }
            catch
            {
                cn.Close();
                return false;
            }
        }

        public Remitente ObtenerServicio()
        {
            cn.Open();
            SQLiteCommand remitente = new SQLiteCommand("SELECT correo_usuario, contrasena_usuario FROM usuario WHERE cod_usuario = 'U0000'", cn);
            SQLiteDataReader dr = remitente.ExecuteReader();
            Remitente res = new Remitente("","");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    res.correo = Convert.ToString(dr[0]);
                    res.contrasena = Convert.ToString(dr[1]);
                }
            }
            dr.Close();
            cn.Close();
            return res;
        }

        public string ObtenerNombre(string email)
        {
            try
            {
                cn.Open();
                SQLiteCommand nombreDestinatario = new SQLiteCommand("SELECT e.nombre_empleado FROM usuario as u INNER JOIN empleado as e ON u.cod_empleado = e.cod_empleado WHERE correo_usuario='" + email + "' AND NOT estado_usuario = 'O'", cn);
                SQLiteDataReader dr = nombreDestinatario.ExecuteReader();
                string nombre = "Usuario";
                while (dr.Read())
                {
                    nombre = Convert.ToString(dr[0]);
                }
                dr.Close();
                cn.Close();
                return nombre;
            }
            catch
            {
                cn.Close();
                return "Usuario";
            }
        }

        public void Bloquear(string email, string estado)
        {
            try
            {
                cn.Open();
                SQLiteCommand block = new SQLiteCommand("UPDATE usuario SET estado_usuario = @estado WHERE correo_usuario = @email AND NOT estado_usuario = 'O'", cn);
                block.Parameters.Add(new SQLiteParameter("@email", email));
                block.Parameters.Add(new SQLiteParameter("@estado", estado));
                block.ExecuteNonQuery();
                cn.Close();
            }
            catch (SqlException)
            {
                cn.Close();
                MessageBox.Show("La sesión de recuperación terminó incorrectamente. Si no puede iniciar sesión, consulte con gerencia.", "Error al recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Desbloquear(string email)
        {
            try
            {
                cn.Open();
                SQLiteCommand desblock = new SQLiteCommand("UPDATE usuario SET estado_usuario = 'D' WHERE correo_usuario = @email AND (estado_usuario = 'B' OR estado_usuario = 'C')", cn);
                desblock.Parameters.Add(new SQLiteParameter("@email", email));
                desblock.ExecuteNonQuery();
                cn.Close();
            }
            catch (SqlException)
            {
                cn.Close();
                MessageBox.Show("La sesión de recuperación terminó incorrectamente. Si no puede iniciar sesión, consulte con gerencia.", "Error al recuperar contraseña", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AjustarEstado(string id)
        {
            try
            {
                cn.Open();
                SQLiteCommand buscar = new SQLiteCommand("SELECT cod_usuario, estado_usuario FROM usuario WHERE cod_usuario = @id AND (estado_usuario='B' OR estado_usuario = 'C')", cn);
                buscar.Parameters.Add(new SQLiteParameter("@id", id));
                SQLiteDataReader buscaReader = buscar.ExecuteReader();
                if (buscaReader.HasRows)
                {
                    SQLiteCommand desblock = new SQLiteCommand("UPDATE usuario SET estado_usuario = 'D' WHERE cod_usuario = @id AND (estado_usuario = 'B' OR estado_usuario = 'C')", cn);
                    desblock.Parameters.Add(new SQLiteParameter("@id", id));
                    desblock.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("El usuario ha sido desbloqueado.", "Desbloquear Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    cn.Close();
                    MessageBox.Show("El usuario no está bloqueado.", "Desbloquear Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException)
            {
                cn.Close();
                MessageBox.Show("Ocurrió un error en la consulta. Intente de nuevo más tarde.", "Error al desbloquear", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //**************************************  ACA EMPIEZA GESTION DE MATERIALES E INSUMOS  ******************************************************************//
        //METODO PARA GENERAR CONSULTAS 
        public DataTable consultarMateriales()
        {
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT COD_MATERIAL, NOMBRE_MATERIAL, UNIDAD_MEDIDA_MATERIAL, EXISTENCIA_MATERIAL, FECHA_MODF_MATERIAL FROM MATERIAL WHERE ESTADO_MATERIAL='1'", cn);
                da.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar la tabla de Materiales " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return dt;
        }

        public String AgregarMaterial(GestionMateriales.Material material)
        {
            try
            {
                cn.Open();
                //  SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT C.CODCLIENTE, C.NOMBRECLIENTE, C.APELLIDOCLIENTE, C.EMPRESACLIENTE, T.NOMBRESERVICIO,T.CODSERVICIO from CLIENTE as C INNER JOIN TIPOSERVICIO AS T WHERE C.CODSERVICIO = T.CODSERVICIO", cn);
                SQLiteCommand comando = new SQLiteCommand("INSERT INTO MATERIAL (COD_MATERIAL, NOMBRE_MATERIAL, EXISTENCIA_MATERIAL, UNIDAD_MEDIDA_MATERIAL, FECHA_MODF_MATERIAL, ESTADO_MATERIAL) VALUES (@id,@nom,@exis,@uni,@fecha,@estado)", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", material.codigo));
                comando.Parameters.Add(new SQLiteParameter("@nom", material.nombre));
                comando.Parameters.Add(new SQLiteParameter("@exis", material.cantidad));
                comando.Parameters.Add(new SQLiteParameter("@uni", material.unidad));
                comando.Parameters.Add(new SQLiteParameter("@fecha", material.fecha));
                comando.Parameters.Add(new SQLiteParameter("@estado", material.estado));
                comando.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {
                cn.Close();
                return "Ha ocurrido un error al agregar el material" + ex.Message.ToString();

            }
            cn.Close();
            return "Material Registrado correctamente";
        }

        public String ActualizarMaterial(GestionMateriales.Material material)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("UPDATE MATERIAL SET NOMBRE_MATERIAL = @nom, EXISTENCIA_MATERIAL= @exis, UNIDAD_MEDIDA_MATERIAL= @uni, FECHA_MODF_MATERIAL =@fecha WHERE COD_MATERIAL=@codigo ", cn);
                comando.Parameters.Add(new SQLiteParameter("@nom", material.nombre));
                comando.Parameters.Add(new SQLiteParameter("@exis", material.cantidad));
                comando.Parameters.Add(new SQLiteParameter("@uni", material.unidad));
                comando.Parameters.Add(new SQLiteParameter("@fecha", material.fecha));
                comando.Parameters.Add(new SQLiteParameter("@codigo", material.codigo));
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {


                cn.Close();
                Console.WriteLine(ex.Message.ToString());
                return "Ha ocurrido un error al Actualizar " + ex.Message.ToString();


            }
            cn.Close();
            return "Material Actualizado correctamente";
        }

        public String EliminarMaterial(String codMaterial)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("UPDATE MATERIAL SET ESTADO_MATERIAL = 0 WHERE COD_MATERIAL=@id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", codMaterial));
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {

                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error al Eliminar " + ex.Message.ToString();

            }
            cn.Close();
            return "Material eliminado correctamente.";
        }

        public DataTable BuscarMaterial(string nombreMaterial)
        {
            List<GestionMateriales.Material> material = new List<GestionMateriales.Material>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT e.COD_MATERIAL, e.NOMBRE_MATERIAL, e.EXISTENCIA_MATERIAL, e.UNIDAD_MEDIDA_MATERIAL, e.FECHA_MODF_MATERIAL FROM MATERIAL AS e WHERE e.NOMBRE_MATERIAL LIKE @nombre AND ESTADO_MATERIAL='1'", cn);
                comando.Parameters.Add(new SQLiteParameter("@nombre", "%" + nombreMaterial + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar MATERIAL " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return dt;
        }
        // *************************** FIN DE LA HISTORIA GESTION DE MATERIALES **********************************************************************

        //**************************************  ACA EMPIEZA GESTION PRODUCTOS  ******************************************************************//
        //METODO PARA GENERAR CONSULTAS 
        public DataTable consultarProductos()
        {
            try
            {
                cn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT COD_PRODUCTO, NOMBRE_PRODUCTO, UNIDAD_MEDIDA_PRODUCTO, EXISTENCIA_PRODUCTO, MARCA_PRODUCTO, PRECIO_PRODUCTO, FECHA_MODF_PRODUCTO FROM PRODUCTO WHERE ESTADO_PRODUCTO='1'", cn);
                da.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar la tabla de Productos " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return dt;
        }

        //METODO PARA AGREGAR PRODUCTO
        public String AgregarProducto(GestionProductos.Producto producto)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("INSERT INTO PRODUCTO (COD_PRODUCTO, NOMBRE_PRODUCTO, EXISTENCIA_PRODUCTO, UNIDAD_MEDIDA_PRODUCTO, MARCA_PRODUCTO, PRECIO_PRODUCTO, FECHA_MODF_PRODUCTO, ESTADO_PRODUCTO) VALUES (@id,@nom,@exis,@uni,@marc,@prec,@fecha,@estado)", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", producto.codigoProd));
                comando.Parameters.Add(new SQLiteParameter("@nom", producto.nombreProd));
                comando.Parameters.Add(new SQLiteParameter("@exis", producto.cantidadProd));
                comando.Parameters.Add(new SQLiteParameter("@uni", producto.unidadProd));
                comando.Parameters.Add(new SQLiteParameter("@marc", producto.marcaProd));
                comando.Parameters.Add(new SQLiteParameter("@prec", producto.precioProd));
                comando.Parameters.Add(new SQLiteParameter("@fecha", producto.fechaProd));
                comando.Parameters.Add(new SQLiteParameter("@estado", producto.estadoProd));
                comando.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {
                cn.Close();
                return "Ha ocurrido un error al agregar el producto" + ex.Message.ToString();

            }
            cn.Close();
            return "Producto Registrado correctamente";
        }

        //METODO PARA ACTUALIZAR PRODUCTO
        public String ActualizarProducto(GestionProductos.Producto producto)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("UPDATE PRODUCTO SET NOMBRE_PRODUCTO = @nom, EXISTENCIA_PRODUCTO= @exis, UNIDAD_MEDIDA_PRODUCTO= @uni, MARCA_PRODUCTO= @marc, PRECIO_PRODUCTO= @prec, FECHA_MODF_PRODUCTO =@fecha WHERE COD_PRODUCTO=@codigo ", cn);
                comando.Parameters.Add(new SQLiteParameter("@nom", producto.nombreProd));
                comando.Parameters.Add(new SQLiteParameter("@exis", producto.cantidadProd));
                comando.Parameters.Add(new SQLiteParameter("@uni", producto.unidadProd));
                comando.Parameters.Add(new SQLiteParameter("@marc", producto.marcaProd));
                comando.Parameters.Add(new SQLiteParameter("@prec", producto.precioProd));
                comando.Parameters.Add(new SQLiteParameter("@fecha", producto.fechaProd));
                comando.Parameters.Add(new SQLiteParameter("@codigo", producto.codigoProd));
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                cn.Close();
                Console.WriteLine(ex.Message.ToString());
                return "Ha ocurrido un error al Actualizar el Producto" + ex.Message.ToString();
            }
            cn.Close();
            return "Producto Actualizado correctamente";
        }

        //METODO PARA ELIMINAR PRODCUTO
        public String EliminarProducto(String codProducto)
        {
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("UPDATE PRODUCTO SET ESTADO_PRODUCTO = 0 WHERE COD_PRODUCTO=@id", cn);
                comando.Parameters.Add(new SQLiteParameter("@id", codProducto));
                comando.ExecuteNonQuery();
                cn.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine();
                cn.Close();
                return "Ha ocurrido un error al Eliminar el Producto" + ex.Message.ToString();
            }
            cn.Close();
            return "Producto eliminado correctamente.";
        }

        //METODO PARA BUSCAR PRODUCTO
        public DataTable BuscarProducto(string nombreProducto)
        {
            List<GestionProductos.Producto> producto = new List<GestionProductos.Producto>();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            try
            {
                cn.Open();
                SQLiteCommand comando = new SQLiteCommand("SELECT e.COD_PRODUCTO, e.NOMBRE_PRODUCTO, e.EXISTENCIA_PRODUCTO, e.UNIDAD_MEDIDA_PRODUCTO, e.MARCA_PRODUCTO, e.PRECIO_PRODUCTO, e.FECHA_MODF_PRODUCTO FROM PRODUCTO AS e WHERE e.NOMBRE_PRODUCTO LIKE @nombre AND ESTADO_PRODUCTO='1'", cn);
                comando.Parameters.Add(new SQLiteParameter("@nombre", "%" + nombreProducto + "%"));
                adapter.SelectCommand = comando;
                adapter.Fill(dt);
            }

            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al buscar el Producto " + ex.Message.ToString());
                Console.WriteLine();
                cn.Close();
            }
            cn.Close();
            return dt;
        }
        // *************************** FIN DE LA HISTORIA GESTION DE PRODUCTOS **********************************************************************

        //**************************************  SOLICITUDES DE INSUMOS Y APROBACIÓN  ******************************************************************

        public DataTable ConsultarSolicitudes(int opcion)
        {
            SQLiteDataAdapter da;
            try
            {
                cn.Open();
                if (opcion == 0)//Gabriel
                {
                    da = new SQLiteDataAdapter("SELECT s.COD_SOLICITUD,e.COD_EMPLEADO,e.NOMBRE_EMPLEADO,s.FECHA_SOLICITUD, s.ESTADO_SOLICITUD FROM EMPLEADO AS e INNER JOIN SOLICITUD_INSUMO AS s WHERE e.COD_EMPLEADO=s.COD_EMPLEADO", cn);
                }
                else {//Felix
                    da = new SQLiteDataAdapter("SELECT s.COD_SOLICITUD,e.COD_EMPLEADO,e.NOMBRE_EMPLEADO,s.FECHA_SOLICITUD, s.ESTADO_SOLICITUD FROM EMPLEADO AS e INNER JOIN SOLICITUD_INSUMO AS s WHERE e.COD_EMPLEADO=s.COD_EMPLEADO AND s.ESTADO_SOLICITUD='Pendiente'", cn);

                }

                da.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar tabla de solicitudes " + ex.Message.ToString());
                
                cn.Close();
            }
            cn.Close();
            return dt;
        }

        public DataTable ConsultarDetalleSolicitudes(string codigoSolicitud)
        {
            
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            dt.Clear();
            try
            {
               cn.Open();
                    SQLiteCommand comando = new SQLiteCommand("SELECT  d.COD_DETALLE, d.COD_MATERIAL, m.NOMBRE_MATERIAL,d.COD_SOLICITUD, d.CANTIDAD_DETALLE  FROM MATERIAL AS m INNER JOIN DETALLE_SOLICITUD_INSUMO AS d  WHERE d.COD_SOLICITUD=@codSolicitud AND d.COD_MATERIAL=m.COD_MATERIAL", cn);
                comando.Parameters.Add(new SQLiteParameter("@codSolicitud",  codigoSolicitud));
                    adapter.SelectCommand = comando;
                    adapter.Fill(dt);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar el detalle de las solicitudes " + ex.Message.ToString());

                cn.Close();
            }
            cn.Close();
            return dt;
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


        /*

         public List<GestionEmpleados.Empleado> ConsultarEmpleados()
         {
             GestionEmpleados.Empleado empleado = new GestionEmpleados.Empleado();
             List<GestionEmpleados.Empleado> empleados = new List<GestionEmpleados.Empleado>();
             try
             {
                 cn.Open();
                 string comando = "SELECT e.COD_EMPLEADO, e.COD_AREA, a.NOMBRE_AREA, e.COD_CARGO,c.NOMBRE_CARGO, e.NOMBRE_EMPLEADO, e.APELLIDO_EMPLEADO, e.FECHA_CONTRATACION,e.ESTADO_EMPLEADO FROM EMPLEADO AS e INNER JOIN AREA AS a INNER JOIN CARGO AS c  WHERE e.COD_AREA= a.COD_AREA AND e.COD_CARGO= c.COD_CARGO AND e.ESTADO_EMPLEADO = 'Activo'";
                 SQLiteCommand command = new SQLiteCommand(comando, cn);
                 SQLiteDataReader dr = command.ExecuteReader();


                 while (dr.Read())
                 {

                     empleado.codigoEmpleado = Convert.ToString(dr[0]);
                     empleado.area.codigoArea = Convert.ToString(dr[1]);
                     empleado.area.nombreArea = Convert.ToString(dr[2]);
                     empleado.cargo.codigoCargo = Convert.ToString(dr[3]);
                     empleado.cargo.nombreCargo = Convert.ToString(dr[4]);
                     empleado.nombreEmpleado = Convert.ToString(dr[5]);
                     empleado.apellidoEmpleado = Convert.ToString(dr[6]);
                     empleado.fechaContratacion = Convert.ToDateTime(dr[7]);
                     empleado.estadoEmpleado = Convert.ToString(dr[8]);
                     empleados.Add(empleado);

                 }

             }
             catch (SQLiteException ex)
             {
                 MessageBox.Show(ex.Message.ToString());
                 Console.WriteLine();
                 cn.Close();
             }
             cn.Close();


             return empleados;
         }*/


    }




}
