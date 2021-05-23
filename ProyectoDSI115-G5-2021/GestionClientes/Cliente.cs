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
        public string codservicio { get; set; }

       public Cliente(string cod , string nom, string ape, string emp, string codserv)
        {
            codigo = cod;
            nombres = nom;
            apellidos = ape;
            empresa = emp;
            codservicio = codserv;
        }
        public Cliente()
        {

        }
        public override string ToString()
        {
            return string.Format(" {0} - {1} - {2} - {3}-{4}", codigo, nombres, apellidos,empresa,codservicio );
        }
    }
    
}
