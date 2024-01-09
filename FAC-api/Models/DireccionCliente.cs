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
    
    public partial class DireccionCliente
    {
        public int IdDireccion { get; set; }
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public int IdDepartamento { get; set; }
        public Nullable<int> IdMunicipio { get; set; }
        public bool EsDirPrincipal { get; set; }
        public bool Activo { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public int IdUsuarioCrea { get; set; }
    
        public virtual Cliente Cliente { get; set; }
    }
}