using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.INV
{
    public class ExistenciaUbicacion
    {
        public string Key { set; get; }
        public string CodProducto { set; get; }
        public string Bodega { set; get; }
        public string Ubicacion { set; get; }
        public decimal Existencia { set; get; }
        public string NoLote { set; get; }
        public Nullable<DateTime> Vence { set; get; }

        public bool EsPrincipal { set; get; }
    }
}