using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.INV
{
    public class Cls_LiberarBonif
    {
        public int IdLiberarBonificacion;
        public int? IdBodega;
        public string Bodega;
        public int IdProducto;
        public string CodProducto;
        public string Producto;
        public int? IdCliente;
        public string Cliente;
        public int CantMax;
        public int Facturada;
        public DateTime FechaAsignacion;
        public bool Activo;
        public string Usuario;
    }
}