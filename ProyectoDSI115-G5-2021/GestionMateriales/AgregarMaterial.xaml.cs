﻿using System;
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
using ProyectoDSI115_G5_2021.GestionMateriales;

namespace ProyectoDSI115_G5_2021.GestionMateriales
{
    /// <summary>
    /// Lógica de interacción para AgregarMaterial.xaml
    /// </summary>
    public partial class AgregarMaterial : Window
    {
        ControlBD control = new ControlBD();

        public AgregarMaterial()
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
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(txtCodigo.Text == "" || txtNombre.Text == "" || txtCantidad.Text == "" || txtUnidad.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos del formulario", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                    string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    Material material = new Material(txtCodigo.Text, txtNombre.Text, txtCantidad.Text, txtUnidad.Text, fecha, true);
                    String respuesta = control.AgregarMaterial(material);
                    MessageBox.Show(respuesta, "Resultado del Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //EVENTO QUE IMPIDE INGRESAR LETRAS EN EL CAMPO DE CANTIDAD EN EXISTENCIA
        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtCantidad.Text, "[^0-9]"))
            {
                MessageBox.Show("En este campo solamente puede utilizar numeros\nPor favor ingrese de forma correcta la cantidad de material.","Advertencia",MessageBoxButton.OK,MessageBoxImage.Warning);
                txtCantidad.Text = txtCantidad.Text.Remove(txtCantidad.Text.Length - 1);
            }
        }
    }
}
