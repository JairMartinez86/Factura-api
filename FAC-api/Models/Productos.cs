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
    
    public partial class Productos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Productos()
        {
            this.Bonificaciones = new HashSet<Bonificaciones>();
            this.Descuentos = new HashSet<Descuentos>();
        }
    
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string CodIPSA { get; set; }
        public string Producto { get; set; }
        public Nullable<int> IdUnidad { get; set; }
        public int IdGrupo { get; set; }
        public int IdSubGrupo { get; set; }
        public Nullable<int> IdGrupoPresupuestario { get; set; }
        public Nullable<int> IdImpuesto { get; set; }
        public Nullable<int> IdProveedor { get; set; }
        public Nullable<decimal> CostoPromedio { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<bool> Estrategico { get; set; }
        public string CodigoBarra { get; set; }
        public string Clasificacion { get; set; }
        public Nullable<decimal> Existencia { get; set; }
        public Nullable<bool> ControladoPorLote { get; set; }
        public Nullable<bool> FacturaNegativo { get; set; }
        public string SAC { get; set; }
        public string CodigoProoveedor { get; set; }
        public decimal CostoInicial { get; set; }
        public string GRUPO { get; set; }
        public string SUBGRUPO { get; set; }
        public string GrupoPresu { get; set; }
        public string UNIDAD { get; set; }
        public Nullable<bool> Servicios { get; set; }
        public bool DescontidiadoProv { get; set; }
        public string CodProveedorEscasan { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionESN { get; set; }
        public string DescripcionENG { get; set; }
        public string DescripcionPT { get; set; }
        public string NoParte { get; set; }
        public string CodBarDescripcion { get; set; }
        public string Ubicacion { get; set; }
        public Nullable<int> DiasDevolucion { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public Nullable<decimal> Largo { get; set; }
        public Nullable<decimal> Ancho { get; set; }
        public Nullable<decimal> Alto { get; set; }
        public Nullable<decimal> Volumen { get; set; }
        public Nullable<int> Cajas { get; set; }
        public Nullable<int> EmpaquexCajas { get; set; }
        public Nullable<int> UnidadesxEmpaque { get; set; }
        public Nullable<int> UnidadesxCajas { get; set; }
        public Nullable<decimal> VolumenCaja { get; set; }
        public Nullable<decimal> PesoCaja { get; set; }
        public Nullable<decimal> AltoCaja { get; set; }
        public Nullable<decimal> AnchoCaja { get; set; }
        public Nullable<decimal> LargoCaja { get; set; }
        public Nullable<int> CajasxCamas { get; set; }
        public Nullable<int> CajasxEstibas { get; set; }
        public Nullable<int> CajasxTarimas { get; set; }
        public Nullable<decimal> CantidadMinCompra { get; set; }
        public Nullable<decimal> StockMinGlobal { get; set; }
        public Nullable<decimal> StockMaxGlobal { get; set; }
        public Nullable<decimal> StockReordenGlobal { get; set; }
        public string Rotacion { get; set; }
        public Nullable<int> DiasInventario { get; set; }
        public Nullable<decimal> VariacionPrecioFOBCIF { get; set; }
        public Nullable<bool> AplicarDescuentoPP { get; set; }
        public Nullable<bool> AplicarBonificacion { get; set; }
        public Nullable<bool> AplicarDescuento { get; set; }
        public Nullable<bool> AplicarPagoComision { get; set; }
        public string PrintManifiesto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bonificaciones> Bonificaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Descuentos> Descuentos { get; set; }
    }
}