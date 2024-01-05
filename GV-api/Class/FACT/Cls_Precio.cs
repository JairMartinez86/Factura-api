using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GV_api.Class.FACT
{
    public class Cls_Precio
    {
        public int Index = 0;
        public string CodProducto = string.Empty;
        public string Tipo = string.Empty;
        public decimal PrecioCordoba = 0;
        public decimal PrecioDolar = 0;
        public int Desde = 0;
        public int Hasta = 0;
        public bool EsPrincipal = false;
        public bool EsEscala = false;
        public bool Liberado = false;
        public int IdPrecioFAC = 0;
        public int IdLiberacion = 0;
    }
}