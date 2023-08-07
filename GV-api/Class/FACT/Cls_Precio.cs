using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GV_api.Class.FACT
{
    public class Cls_Precio
    {
        public string CodProducto = string.Empty;
        public string Tipo = string.Empty;
        public decimal PrecioCordoba = 0;
        public decimal PrecioDolar = 0;
        public bool Principal = false;
    }
}