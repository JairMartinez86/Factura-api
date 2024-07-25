using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.INV
{
    public class Cls_UltimasCompras
    {

        public string NoPoliza;
        public string Codigo;
        public string Producto;
        public DateTime Fecha;
        public int Cantidad;
        public int Bonificacion;
        public decimal CostoFOB;
        public decimal CostoCIF;
        public decimal CostoFinal;
        public string NoLote;
        public Nullable<DateTime> Vence;
    }
}