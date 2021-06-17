using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionEmpleados
{
    class Cargo
    {
        public string codigoCargo { get; set; }
        public string nombreCargo { get; set; }

        public Cargo()
        {

        }
        public Cargo(string codigo, string cargo)
        {
            nombreCargo = cargo;
            codigoCargo = codigo;
        }

    }
}
