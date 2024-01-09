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
    
    public partial class Venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venta()
        {
            this.VentaDetalle = new HashSet<VentaDetalle>();
        }
    
        public System.Guid IdVenta { get; set; }
        public long ID { get; set; }
        public string TipoDocumento { get; set; }
        public string Serie { get; set; }
        public string NoFactura { get; set; }
        public string NoPedido { get; set; }
        public string CodCliente { get; set; }
        public string NomCliente { get; set; }
        public string Nombre { get; set; }
        public string RucCedula { get; set; }
        public string Contacto { get; set; }
        public decimal Limite { get; set; }
        public decimal Disponible { get; set; }
        public string CodBodega { get; set; }
        public string NomBodega { get; set; }
        public string CodVendedor { get; set; }
        public string NomVendedor { get; set; }
        public bool EsContraentrega { get; set; }
        public bool EsExportacion { get; set; }
        public string OrdenCompra { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Plazo { get; set; }
        public System.DateTime Vence { get; set; }
        public string Moneda { get; set; }
        public string TipoVenta { get; set; }
        public string TipoImpuesto { get; set; }
        public string TipoExoneracion { get; set; }
        public string NoExoneracion { get; set; }
        public bool EsDelivery { get; set; }
        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Exonerado { get; set; }
        public decimal TotalCordoba { get; set; }
        public decimal TotalDolar { get; set; }
        public decimal TasaCambio { get; set; }
        public bool PedirAutorizacion { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string UsuarioRegistra { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public string UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaAnula { get; set; }
        public string UsuarioAnula { get; set; }
        public string Estado { get; set; }
        public string MotivoAnulacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VentaDetalle> VentaDetalle { get; set; }
    }
}