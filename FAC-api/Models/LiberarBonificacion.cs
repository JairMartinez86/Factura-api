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
    
    public partial class LiberarBonificacion
    {
        public int IdLiberarBonificacion { get; set; }
        public Nullable<int> IdBodega { get; set; }
        public int IdProducto { get; set; }
        public int IdCliente { get; set; }
        public int CantMax { get; set; }
        public int Facturada { get; set; }
        public bool Activo { get; set; }
        public System.DateTime FechaAsignacion { get; set; }
        public int IdUsuarioCrea { get; set; }
        public Nullable<System.DateTime> FechaInactiva { get; set; }
        public Nullable<int> IdUsuarioInactiva { get; set; }
    }
}
