using System;
using System.Collections.Generic;
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
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021
{
    /// <summary>
    /// Lógica de interacción para Inventario.xaml
    /// </summary>
    public partial class Inventario : Window
    {
        private Usuario sesion;
        Nullable<bool> gma = false;
        internal Usuario Sesion { get => sesion; set => sesion = value; }

        GestionMateriales.GestionMateriales gm;

        public Inventario()
        {
            InitializeComponent();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnMateriales_Click(object sender, RoutedEventArgs e)
        {
            //Creando una instancia maximizada de Gestión de Materiales
            //Solo puede entrar la gerencia y administración.
            if (sesion.tipoUsuario.codTipoUsuario.Equals("A") || sesion.tipoUsuario.codTipoUsuario.Equals("G"))
            {
                gm = new GestionMateriales.GestionMateriales
                {
                    WindowState = WindowState.Maximized
                };
                //Se tiene esta variable que se define al cerrar la ventana
                gma = gm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
