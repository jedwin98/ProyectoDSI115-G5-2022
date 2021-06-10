using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionUsuarios
{
    class EmpleadoItem
    {
        public string codEmpleado { get; set; }
        public string nombreEmpleado { get; set; }

        public EmpleadoItem()
        {
        }

        public EmpleadoItem(string codEmpleado, string nombreEmpleado)
        {
            this.codEmpleado = codEmpleado;
            this.nombreEmpleado = nombreEmpleado;
        }
    }
}
