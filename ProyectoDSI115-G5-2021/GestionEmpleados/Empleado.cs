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
        public Cargo cargoE { get; set; }
        public Area areaE  { get; set; }
        

        public Empleado()
        {

        }
        public Empleado(string codigo, string nombres, string apellidos, string estado, DateTime fecha, Cargo cargo, Area area)
        {
            codigoEmpleado = codigo;
            nombreEmpleado = nombres;
            apellidoEmpleado = apellidos;
            estadoEmpleado = estado;
            fechaContratacion = fecha;
            cargoE = cargo;
            areaE = area;
        }
    }
}
