using ProyectoDSI115_G5_2021.GestionUsuarios;
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

namespace ProyectoDSI115_G5_2021.SolicitarInsumos
{
    /// <summary>
    /// Lógica de interacción para VerSolicitudes.xaml
    /// </summary>
    public partial class VerSolicitudes : Window
    {
        private Usuario sesion;
        internal Usuario Sesion { get => sesion; set => sesion = value; }
        public VerSolicitudes()
        {
            InitializeComponent();
        }

        private void BtnNueva_Click(object sender, RoutedEventArgs e)
        {
            CrearSolicitudInsumos crear = new CrearSolicitudInsumos()
            {
                WindowState = WindowState.Maximized
            };
            crear.Sesion = sesion;
            crear.Show();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
