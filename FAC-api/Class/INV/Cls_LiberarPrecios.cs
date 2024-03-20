using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.INV
{
    public class Cls_LiberarPrecios
    {
        public int IdLiberarPrecio;
        public int IdBodega;
        public string Bodega;
        public int IdProducto;
        public string CodProducto;
        public string Producto;
        public int IdCliente;
        public string Cliente;
        public string Motivo;
        public decimal PrecioP;
        public decimal PrecioD;
        public DateTime FechaAsignacion;
        public bool Activo;
        public string Usuario;
    }
}