//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAC_api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VentaLote
    {
        public System.Guid IdDetLote { get; set; }
        public System.Guid IdVenta { get; set; }
        public int Index { get; set; }
        public int IndexDet { get; set; }
        public string Key { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public string NoLote { get; set; }
        public Nullable<System.DateTime> Vence { get; set; }
        public bool EsBonificado { get; set; }
        public int Existencia { get; set; }
        public bool FacturaNegativo { get; set; }
    
        public virtual Venta Venta { get; set; }
    }
}