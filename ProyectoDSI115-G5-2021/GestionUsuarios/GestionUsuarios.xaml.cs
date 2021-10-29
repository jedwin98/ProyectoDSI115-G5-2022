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

namespace ProyectoDSI115_G5_2021.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para GestionUsuarios.xaml
    /// </summary>
    public partial class GestionUsuarios : Window
    {
        private DataTable dt = new DataTable();
        private ControlBD control = new ControlBD();
        internal Usuario sesion;
        internal CambiarCorreo cc;
        private bool guardando, contrasenaCorrecta, emailCorrecto, abrir = false, combo1 = false, combo2 = false;
        private Thickness ogTabla, ogUsuario, ogRoles, ogContra, ogConfirma, ogMail, ogEmp, ogGuarda, oglUsuario, oglRoles, oglContra, oglConfirma, oglMail, oglEmp;

        // Inicialización de ventana
        public GestionUsuarios()
        {
            InitializeComponent();
            CargarTabla();
            // Definiendo posiciones iniciales en caso que se reduzca el tamaño de la ventana.
            ogTabla = dataUsuarios.Margin;
            ogUsuario = cuadroUsuario.Margin;
            ogRoles = comboRoles.Margin;
            ogContra = cuadroContrasena.Margin;
            ogConfirma = cuadroContrasenaConfirmar.Margin;
            ogMail = cuadroEmail.Margin;
            ogEmp = comboEmpleado.Margin;
            ogGuarda = botonGuardar.Margin;
            oglUsuario = labelUsuario.Margin;
            oglRoles = labelRoles.Margin;
            oglContra = labelContrasena.Margin;
            oglConfirma = labelContrasenaConfirmar.Margin;
            oglMail = labelEmail.Margin;
            oglEmp = labelEmpleado.Margin;
        }

        // Borrado de usuario. No permite que el usuario actual se elimine.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (!(dataUsuarios.SelectedItem is DataRowView row))
            {
                MessageBox.Show("No hay usuario seleccionado. Debe seleccionar un usuario.", "Error al eliminar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (sesion.codigo.Equals(row.Row.ItemArray[0].ToString()))
                {
                    MessageBox.Show("No puede eliminar su cuenta. Solicite esta operación a otro usuario autorizado.", "Error al eliminar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult res = MessageBox.Show("¿Está seguro que desea eliminar este usuario", "Eliminar Usuario", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        string mensaje = control.EliminarUsuario(row.Row.ItemArray[0].ToString());
                        if (mensaje.Equals("Ha ocurrido un error. Verifique su conexión e intente de nuevo.")) MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            MessageBox.Show(mensaje, "Eliminar usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                            CargarTabla();
                        }
                    }
                }
            }
        }

        // Envía comando de carga. Recibe la tabla de datos para usarla como ItemsSource
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void CargarTabla()
        {
            dt.Clear();
            dt = control.ConsultarUsuarios();
            dataUsuarios.ItemsSource = dt.DefaultView;
        }

        // Acción del botón de búsqueda
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            RealizarBusqueda();
        }

        // Busca una clave ingresada en el campo de búsqueda en nombre, apellido o correo.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void RealizarBusqueda()
        {
            dt.Clear();
            dt = control.BuscarUsuario(cuadroBuscar.Text);
            dataUsuarios.ItemsSource = dt.DefaultView;
        }

        // Acción de tecla "Enter" en cuadro de búsqueda
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void CuadroBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                RealizarBusqueda();
            }
        }

        // Botón de desbloqueo de cuentas. Usar en caso de fallas fuera del rango de control.
        // Posibles causas como: Fallos de conexión, fallos eléctricos, fallos de sistema u otros.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnDesbloquear_Click(object sender, RoutedEventArgs e)
        {
            if (!(dataUsuarios.SelectedItem is DataRowView row))
            {
                MessageBox.Show("No hay usuario seleccionado. Debe seleccionar un usuario.", "Error al desbloquear",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                control.AjustarEstado(row.Row.ItemArray[0].ToString());
            }
        }

        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnCambiarCorreo_Click(object sender, RoutedEventArgs e)
        {
            cc = new CambiarCorreo();
            cc.sesion = this.sesion;
            cc.ShowDialog();
        }

        // Llenado de ComboBox para tipos de usuario.
        private void LlenarComboTipos()
        {
            // Se recibe un listado de los tipos de usuario.
            List<TipoUsuario> tipos = control.ConsultarTipoUsuario();
            // Se vincula al combobox, define los títulos y valores
            comboRoles.ItemsSource = tipos;
            comboRoles.DisplayMemberPath = "nombreTipoUsuario";
            comboRoles.SelectedValuePath = "codTipoUsuario";
        }

        // Llenado de ComboBox para empleados disponibles.
        private void LlenarComboEmpleados()
        {
            // Igual que LlenarComboTipos, pero aplicado a empleados
            List<EmpleadoItem> empleados = control.ConsultarEmpleadosLista();
            // Vinculando al combobox, definiendo títulos y valores
            comboEmpleado.ItemsSource = empleados;
            comboEmpleado.DisplayMemberPath = "nombreEmpleado";
            comboEmpleado.SelectedValuePath = "codEmpleado";
        }

        // Activa el formulario de registro de usuario.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Variables para habilitar guardado.
            guardando = false;
            if (!abrir)
            {
                // Si no se está editando, pasa a modo de edición y cambia la imagen del botón.
                contrasenaCorrecta = false;
                emailCorrecto = false;
                // Inicializando formulario para agregar usuario
                HabilitarEdicion(true);
                LlenarComboTipos();
                LlenarComboEmpleados();
                // Booleano para determinar el resultado de guardar
                var uriSource = new Uri("/Images/regreso.png", UriKind.Relative);
                labelAgregar.Content = "Cancelar";
                imgAgregar.Source = new BitmapImage(uriSource);
                abrir = true;
            }
            else
            {
                // Si se edita y se pulsa el botón, sale del modo de edición.
                comboEmpleado.ItemsSource = null;
                comboEmpleado.Items.Clear();
                comboRoles.ItemsSource = null;
                comboRoles.Items.Clear();
                HabilitarEdicion(false);
                cuadroEmail.Background = Brushes.White;
                cuadroContrasena.Background = Brushes.White;
                cuadroContrasenaConfirmar.Background = Brushes.White;
                var uriSource = new Uri("/Images/agregar.png", UriKind.Relative);
                labelAgregar.Content = "Agregar";
                imgAgregar.Source = new BitmapImage(uriSource);
                abrir = false;
            }
        }

        // Botón de guardado. Solo se activa cuando los datos solicitados son válidos.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BotonGuardar_Click(object sender, RoutedEventArgs e)
        {
            guardando = true;
            // Lógica de guardado de usuarios
            // Crea una instancia de usuario e inicializa con los valores a definir.
            Usuario aGuardar = new Usuario();
            aGuardar.tipoUsuario = new TipoUsuario();
            aGuardar.codigo = cuadroUsuario.Text;
            aGuardar.correoElectronico = cuadroEmail.Text;
            aGuardar.empleado = comboEmpleado.SelectedValue.ToString();
            aGuardar.tipoUsuario.codTipoUsuario = comboRoles.SelectedValue.ToString();
            // Lógica de creación de registro
            if (control.VerificarCorreo(aGuardar.correoElectronico))
            {
                String mensaje = control.AgregarUsuario(aGuardar, cuadroContrasena.Password.ToString());
                if (mensaje.Equals("Ha ocurrido un error. Verifique su conexión e intente de nuevo."))
                {
                    MessageBox.Show(mensaje, "Error al registrar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(mensaje, "Registro de usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                    comboEmpleado.ItemsSource = null;
                    comboEmpleado.Items.Clear();
                    comboRoles.ItemsSource = null;
                    comboRoles.Items.Clear();
                    HabilitarEdicion(false);
                    cuadroEmail.Background = Brushes.White;
                    cuadroContrasena.Background = Brushes.White;
                    cuadroContrasenaConfirmar.Background = Brushes.White;
                    botonGuardar.SetCurrentValue(IsEnabledProperty, false);
                    var uriSource = new Uri("/Images/agregar.png", UriKind.Relative);
                    labelAgregar.Content = "Agregar";
                    imgAgregar.Source = new BitmapImage(uriSource);
                    guardando = false;
                    CargarTabla();
                }
            }
            else MessageBox.Show("Ocurrió un error. Verifique si está conectado o si el correo electrónico no está siendo usado",
                "Error al registrar", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Ajuste central de propiedad de edición en formulario. Toma un booleano para habilitar o deshabilitar.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void HabilitarEdicion(bool valor)
        {
            // Cuadro de usuario.
            cuadroUsuario.SetCurrentValue(IsEnabledProperty, valor);
            cuadroUsuario.Text = "";
            // Cuadro de contraseña y confirmación.
            cuadroContrasena.SetCurrentValue(IsEnabledProperty, valor);
            cuadroContrasena.Clear();
            cuadroContrasenaConfirmar.SetCurrentValue(IsEnabledProperty, valor);
            cuadroContrasenaConfirmar.Clear();
            // Cuadro de email.
            cuadroEmail.SetCurrentValue(IsEnabledProperty, valor);
            cuadroEmail.Text = "";
            // Lista de empleados, roles y otros botones de gestión
            comboEmpleado.SetCurrentValue(IsEnabledProperty, valor);
            comboRoles.SetCurrentValue(IsEnabledProperty, valor);
            btnBorrar.SetCurrentValue(IsEnabledProperty, !valor);
        }

        // Verifica validez de correo registrado. Necesario para habilitar guardado.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void CorreoValido(string mail)
        {
            try
            {
                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(mail);
                cuadroEmail.Background = Brushes.White;
                emailCorrecto = true;
            }
            catch
            {
                cuadroEmail.Background = Brushes.LightPink;
                emailCorrecto = false;
            }
        }

        // Cierra esta instancia de la ventana.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CuadroEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CorreoValido(cuadroEmail.Text);
            if (comboEmpleado.SelectedItem!=null)
            {
                PermitirGuardado();
            }
        }

        // Si la dirección de correo, las contraseñas o los combos no están ingresados...
        // Evita el ingreso de la información.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void PermitirGuardado()
        {
            if (!guardando)
            {
                if (contrasenaCorrecta && emailCorrecto && comboEmpleado.SelectedItem != null &&
                    comboRoles.SelectedItem != null && cuadroUsuario.Text != null && !cuadroEmail.IsFocused)
                {
                    botonGuardar.SetCurrentValue(IsEnabledProperty, true);
                }
                else
                {
                    botonGuardar.SetCurrentValue(IsEnabledProperty, false);
                }
            }
        }

        private void CuadroUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            PermitirGuardado();
        }

        private void ComboRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combo2 = !comboEmpleado.IsDropDownOpen;
            PermitirGuardado();
        }

        // Ajuste del formulario según tamaño de ventana.
        // AUTOR: Félix Eduardo Henríquez Cruz
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Height > 636 || this.WindowState == WindowState.Maximized)
            {
                // Si la ventana supera 636 unidades de altura, cambia los márgenes del formulario.
                dataUsuarios.Margin = new Thickness(224, 158, 23, 307);
                labelUsuario.Margin = new Thickness(224, 0, 23, 246);
                cuadroUsuario.Margin = new Thickness(349, 0, 0, 246);
                labelRoles.Margin = new Thickness(517, 0, 0, 246);
                comboRoles.Margin = new Thickness(626, 0, 23, 246);
                labelEmail.Margin = new Thickness(224, 0, 0, 216);
                cuadroEmail.Margin = new Thickness(349, 0, 23, 216);
                labelContrasena.Margin = new Thickness(224, 0, 0, 186);
                cuadroContrasena.Margin = new Thickness(349, 0, 23, 186);
                labelContrasenaConfirmar.Margin = new Thickness(224, 0, 0, 156);
                cuadroContrasenaConfirmar.Margin = new Thickness(349, 0, 23, 156);
                labelEmpleado.Margin = new Thickness(224, 0, 0, 126);
                comboEmpleado.Margin = new Thickness(349, 0, 171, 126);
                botonGuardar.Margin = new Thickness(0, 0, 23, 126);
            }
            else
            {
                // De lo contrario, vuelve a los márgenes originales.
                dataUsuarios.Margin = ogTabla;
                labelUsuario.Margin = oglUsuario;
                cuadroUsuario.Margin = ogUsuario;
                labelRoles.Margin = oglRoles;
                comboRoles.Margin = ogRoles;
                labelContrasena.Margin = oglContra;
                cuadroContrasena.Margin = ogContra;
                labelContrasenaConfirmar.Margin = oglConfirma;
                cuadroContrasenaConfirmar.Margin = ogConfirma;
                labelEmail.Margin = oglMail;
                cuadroEmail.Margin = ogMail;
                labelEmpleado.Margin = oglEmp;
                comboEmpleado.Margin = ogEmp;
                botonGuardar.Margin = ogGuarda;
            }
        }

        private void ComboRoles_DropDownClosed(object sender, EventArgs e)
        {
            if (combo2) PermitirGuardado();
            combo2 = true;
        }

        private void ComboEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combo1 = !comboEmpleado.IsDropDownOpen;
            PermitirGuardado();
        }

        private void ComboEmpleado_DropDownClosed(object sender, EventArgs e)
        {
            if (combo1) PermitirGuardado();
            combo1 = true;
        }

        private void CuadroContrasenaConfirmar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string nuevaContra = cuadroContrasena.Password.ToString();
            // Comprobar que la contraseña sea de al menos 6 caracteres y coincidan.
            PruebaContrasena(nuevaContra);
        }

        private void PruebaContrasena(string contra)
        {
            contrasenaCorrecta = false;
            if (contra.Length > 5)
            {
                if (contra.Equals(cuadroContrasenaConfirmar.Password.ToString()))
                {
                    // Las contraseñas son correctas.
                    cuadroContrasena.Background = Brushes.White;
                    cuadroContrasenaConfirmar.Background = Brushes.White;
                    contrasenaCorrecta = true;
                }
                else
                {
                    // La confirmación no es correcta.
                    cuadroContrasena.Background = Brushes.LightGoldenrodYellow;
                    cuadroContrasenaConfirmar.Background = Brushes.LightPink;
                }
            }
            else
            {
                // La contraseña es muy corta.
                cuadroContrasena.Background = Brushes.LightPink;
                cuadroContrasenaConfirmar.Background = Brushes.LightPink;
            }
            PermitirGuardado();
        }

        private void CuadroContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Comprobando que la contraseña sea de 6 caracteres.
            string nuevaContra = cuadroContrasena.Password.ToString();
            PruebaContrasena(nuevaContra);
        }
    }
}
