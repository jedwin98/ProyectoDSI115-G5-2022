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

        string tipo;
        GestionMateriales.GestionMateriales gm;
        Nullable<bool> gma = false;

        public Inventario(string tipoUsuario)
        {
            InitializeComponent();
            tipo = tipoUsuario;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnMateriales_Click(object sender, RoutedEventArgs e)
        {
            if (tipo.Equals("A") || tipo.Equals("G"))
            {
                gm = new GestionMateriales.GestionMateriales()
                {
                    WindowState = WindowState.Maximized
                };
                gma = gm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No posee los permisos necesarios para entrar.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
