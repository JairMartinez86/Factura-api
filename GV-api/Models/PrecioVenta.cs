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
    
    public partial class PrecioVenta
    {
        public int IdPrecioFAC { get; set; }
        public string CodigoProducto { get; set; }
        public int IdConceptoPrecio { get; set; }
        public string IdMoneda { get; set; }
        public decimal Precio { get; set; }
        public System.DateTime FechaAsignacion { get; set; }
        public Nullable<System.DateTime> FechaInactiva { get; set; }
        public bool Activo { get; set; }
        public int IdUsuarioCrea { get; set; }
        public Nullable<int> IdUsuarioInactiva { get; set; }
    
        public virtual ConceptoPrecio ConceptoPrecio { get; set; }
    }
}
