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
    
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            this.DireccionCliente = new HashSet<DireccionCliente>();
        }
    
        public int IdCliente { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string NoCedula { get; set; }
        public string Correo { get; set; }
        public string Correo2 { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Telefono3 { get; set; }
        public string Fax { get; set; }
        public string Celular { get; set; }
        public string Contacto { get; set; }
        public string Contacto2 { get; set; }
        public int IdConceptoPrecio { get; set; }
        public Nullable<int> IdPlazo { get; set; }
        public int Plazo { get; set; }
        public Nullable<int> DiasG { get; set; }
        public string IdMoneda { get; set; }
        public decimal Limite { get; set; }
        public Nullable<int> IdSector { get; set; }
        public Nullable<int> IdCategoriaCliente { get; set; }
        public Nullable<int> IdBodega { get; set; }
        public bool PedirNombre { get; set; }
        public string Estado { get; set; }
        public Nullable<bool> FacturarVencido { get; set; }
        public Nullable<int> IdUsuarioCrea { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<int> IdUsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public bool FactVencido { get; set; }
        public bool EsMoroso { get; set; }
        public bool FactSiempre { get; set; }
        public Nullable<int> SolicitadoPor { get; set; }
        public Nullable<int> IdCargo { get; set; }
        public Nullable<bool> ClienteClave { get; set; }
        public Nullable<decimal> DesCentralizacion { get; set; }
        public string Vendedor { get; set; }
        public Nullable<bool> FacOC { get; set; }
        public bool ClienteConfFacSiempre { get; set; }
        public Nullable<bool> ReportarCentralRiesgo { get; set; }
        public string CodTipoCliente { get; set; }
        public Nullable<int> IdContribuyente { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DireccionCliente> DireccionCliente { get; set; }
    }
}
