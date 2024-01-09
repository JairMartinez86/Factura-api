using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.FACT
{
    public class Cls_Productos_Liberados_INVESCASAN
    {
        public int ID { set; get; }
        public string Codigo { set; get; }
        public decimal Precio { set; get; }
        public int Cantidad { set; get; }
        public int Bonificado { set; get; }
    }
}