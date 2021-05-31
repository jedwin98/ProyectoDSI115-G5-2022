using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionClientes
{
    class TipoServicio

    {
        public string id { set; get; }
        public string nombre { set; get; }
        public TipoServicio()
        {

        }
        public TipoServicio(string codigo, string name)
        {
            id = codigo;
            nombre = name;
        }

    }
}
