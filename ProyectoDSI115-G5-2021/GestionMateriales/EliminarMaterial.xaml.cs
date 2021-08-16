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

namespace ProyectoDSI115_G5_2021.GestionMateriales
{
    /// <summary>
    /// Lógica de interacción para EliminarMaterial.xaml
    /// </summary>
    public partial class EliminarMaterial : Window
    {
        public EliminarMaterial()
        {
            InitializeComponent();
        }

        //*************************** METODO DE BOTONES ***************************************//
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            GestionMateriales gestionMateriales = new GestionMateriales()
            {
                WindowState = WindowState.Maximized
            };
            gestionMateriales.Show();
            this.Close();
        }
    }
}
