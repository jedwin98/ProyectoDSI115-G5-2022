using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.CotizacionRecibo
{
    class DetalleCotizacion
    {
        public float cantidad { get; set; }
        public string concepto { get; set; }
        public float precio { get; set; }
        public float subtotal { get; set; }

        public DetalleCotizacion(float cantidad, string concepto, float precio, float subtotal)
        {
            this.cantidad = cantidad;
            this.concepto = concepto;
            this.precio = precio;
            this.subtotal = subtotal;
        }

        public DetalleCotizacion()
        {

        }
    }
}
