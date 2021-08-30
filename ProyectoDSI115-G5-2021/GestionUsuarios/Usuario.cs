using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionUsuarios
{
    class Usuario
    {
        internal string codigo { get; set; }
        internal string codigoEmpleado { get; set; }
        internal string correoElectronico { get; set; }
        internal string empleado { get; set; }
        internal TipoUsuario tipoUsuario { get; set; }
        internal string estado { get; set; }
    }
}
