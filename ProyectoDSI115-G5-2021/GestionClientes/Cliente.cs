using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionClientes
{
    class Cliente
    {
        public string codigo { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string empresa { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string estado { get; set; }

        public Cliente(string cod , string nom, string ape, string emp, string tel, string corr, string estad)
        {
            codigo = cod;
            nombres = nom;
            apellidos = ape;
            empresa = emp;
            telefono = tel;
            correo = corr;
            estado = estad;
        }
        public Cliente()
        {

        }
        public override string ToString()
        {
            return string.Format(" {0} - {1} - {2} - {3}-{4} - {5} - {6}", codigo, nombres, apellidos,empresa, telefono, correo, estado);
        }
    }
    
}
