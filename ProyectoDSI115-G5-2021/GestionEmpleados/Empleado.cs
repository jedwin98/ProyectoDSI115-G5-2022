using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionEmpleados
{
    class Empleado
    {

        public string codigoEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string apellidoEmpleado { get; set; }
        public string estadoEmpleado { get; set; }
        public DateTime fechaContratacion { get; set; }
        public Cargo cargo { get; set; }
        public Area area  { get; set; }
        

        public Empleado()
        {

        }
    }
}
