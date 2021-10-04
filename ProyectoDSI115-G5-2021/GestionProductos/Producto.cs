using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDSI115_G5_2021.GestionProductos
{
    class Producto
    {
        public string codigoProd { get; set; }
        public string nombreProd { get; set; }
        public string cantidadProd { get; set; }
        public string unidadProd { get; set; }
        public string marcaProd { get; set; }
        public string precioProd { get; set; }
        public string fechaProd { get; set; }
        public bool estadoProd { get; set; }

        public Producto(string codP, string nomP, string canP, string uniP, string marcP, string precP, string fechaProd, bool estdP)
        {
            codigoProd = codP;
            nombreProd = nomP;
            cantidadProd = canP;
            unidadProd = uniP;
            marcaProd = marcP;
            precioProd = precP;
            this.fechaProd = fechaProd;
            estadoProd = estdP;
        }
        public Producto()
        {

        }
        public override string ToString()
        {
            return string.Format(" {0} - {1} - {2} - {3} - {4} - {5} - {6} - {7}", codigoProd, nombreProd, cantidadProd, unidadProd, marcaProd, precioProd, fechaProd);
        }
    }
}
