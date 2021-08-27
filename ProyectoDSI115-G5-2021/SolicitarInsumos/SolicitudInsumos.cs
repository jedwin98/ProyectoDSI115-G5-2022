using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoDSI115_G5_2021.GestionMateriales;
using ProyectoDSI115_G5_2021.GestionUsuarios;

namespace ProyectoDSI115_G5_2021.SolicitarInsumos
{
    class SolicitudInsumos
    {
        public string codigo { get; set; }
        public GestionUsuarios.Usuario solicitante { get; set; }
        public string fechaSolicitud { get; set; }
        public List<DetalleSolicitudInsumos> detalles { get; set; }
        public string estado { get; set; }
        public GestionUsuarios.Usuario autorizador { get; set; }

        public SolicitudInsumos()
        {
        }

        public SolicitudInsumos(string codigo, Usuario solicitante, Usuario autorizador, string fechaSolicitud, List<DetalleSolicitudInsumos> detalleSolicitud)
        {
            this.codigo = codigo;
            this.solicitante = solicitante;
            this.fechaSolicitud = fechaSolicitud;
            this.detalles = detalleSolicitud;
            this.autorizador = autorizador;
        }
        public void setListDetalles(List<DetalleSolicitudInsumos> d)
        {
            this.detalles = d;
        }
    }
}
