//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GV_api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kardex
    {
        public System.Guid IdKardex { get; set; }
        public int Index { get; set; }
        public string CodProducto { get; set; }
        public decimal Cantidad { get; set; }
        public bool EsBonificado { get; set; }
        public string NoLote { get; set; }
        public Nullable<System.DateTime> Vence { get; set; }
        public string Ubicacion { get; set; }
        public string NoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Serie { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Bodega { get; set; }
        public decimal Costo { get; set; }
        public decimal CostoTotal { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public Nullable<int> IdUsuarioAnula { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public bool Cerrado { get; set; }
        public bool Anulado { get; set; }
        public string BodegaSolicita { get; set; }
        public string OT { get; set; }
    }
}
