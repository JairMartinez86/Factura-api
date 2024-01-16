﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BalancesEntities : DbContext
    {
        public BalancesEntities()
            : base("name=BalancesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bodegas> Bodegas { get; set; }
        public virtual DbSet<Estaciones> Estaciones { get; set; }
        public virtual DbSet<Serie> Serie { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<UsuariosBodegas> UsuariosBodegas { get; set; }
        public virtual DbSet<BodegaSerie> BodegaSerie { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Vendedor> Vendedor { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Impuestos> Impuestos { get; set; }
        public virtual DbSet<LiberarPrecio> LiberarPrecio { get; set; }
        public virtual DbSet<ConceptoPrecio> ConceptoPrecio { get; set; }
        public virtual DbSet<BodegaLaboratorioKardex> BodegaLaboratorioKardex { get; set; }
        public virtual DbSet<Kardex> Kardex { get; set; }
        public virtual DbSet<Bonificaciones> Bonificaciones { get; set; }
        public virtual DbSet<PrecioVenta> PrecioVenta { get; set; }
        public virtual DbSet<PrecioVentaEscala> PrecioVentaEscala { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<Descuentos> Descuentos { get; set; }
        public virtual DbSet<DireccionCliente> DireccionCliente { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<CorreoAutorizaFactura> CorreoAutorizaFactura { get; set; }
        public virtual DbSet<PermisoFactura> PermisoFactura { get; set; }
        public virtual DbSet<LiberarBonificacion> LiberarBonificacion { get; set; }
        public virtual DbSet<FacturaVenta> FacturaVenta { get; set; }
        public virtual DbSet<ProformaVenta> ProformaVenta { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<VentaDetalle> VentaDetalle { get; set; }
        public virtual DbSet<VentaLote> VentaLote { get; set; }
        public virtual DbSet<FormaPago> FormaPago { get; set; }
        public virtual DbSet<Bancos> Bancos { get; set; }
        public virtual DbSet<Monedas> Monedas { get; set; }
    
        public virtual ObjectResult<sp_GrabarFactura_Web_Result1> sp_GrabarFactura_Web(Nullable<int> p_IdFactura, Nullable<int> p_IdProforma, Nullable<int> p_IdBodega, string p_IdSerie, string p_NoFactura, Nullable<System.DateTime> p_Fecha, string p_CodCliente, string p_NomCliente, string p_Nombre, string p_Ruc, string p_Telefono, string p_Celular, string p_Correo, Nullable<int> p_IdVendedor, string p_NomVendedor, string p_TipoVenta, string p_IdMoneda, Nullable<decimal> p_TasaCambio, string p_DireccionEntrega, string p_Observacion, Nullable<bool> p_Exportacion, Nullable<bool> p_Exoneracion, string p_NoExoneracion, string p_OrdenCompra, Nullable<bool> p_ContraEntrega, Nullable<bool> p_EsDelivery, string p_Estado, string p_DatosDetalle, string p_DatosLote, string p_DatosPago, Nullable<int> p_IdUsuario, string p_Longitud, string p_Latitud, Nullable<int> p_IdDireccion)
        {
            var p_IdFacturaParameter = p_IdFactura.HasValue ?
                new ObjectParameter("P_IdFactura", p_IdFactura) :
                new ObjectParameter("P_IdFactura", typeof(int));
    
            var p_IdProformaParameter = p_IdProforma.HasValue ?
                new ObjectParameter("P_IdProforma", p_IdProforma) :
                new ObjectParameter("P_IdProforma", typeof(int));
    
            var p_IdBodegaParameter = p_IdBodega.HasValue ?
                new ObjectParameter("P_IdBodega", p_IdBodega) :
                new ObjectParameter("P_IdBodega", typeof(int));
    
            var p_IdSerieParameter = p_IdSerie != null ?
                new ObjectParameter("P_IdSerie", p_IdSerie) :
                new ObjectParameter("P_IdSerie", typeof(string));
    
            var p_NoFacturaParameter = p_NoFactura != null ?
                new ObjectParameter("P_NoFactura", p_NoFactura) :
                new ObjectParameter("P_NoFactura", typeof(string));
    
            var p_FechaParameter = p_Fecha.HasValue ?
                new ObjectParameter("P_Fecha", p_Fecha) :
                new ObjectParameter("P_Fecha", typeof(System.DateTime));
    
            var p_CodClienteParameter = p_CodCliente != null ?
                new ObjectParameter("P_CodCliente", p_CodCliente) :
                new ObjectParameter("P_CodCliente", typeof(string));
    
            var p_NomClienteParameter = p_NomCliente != null ?
                new ObjectParameter("P_NomCliente", p_NomCliente) :
                new ObjectParameter("P_NomCliente", typeof(string));
    
            var p_NombreParameter = p_Nombre != null ?
                new ObjectParameter("P_Nombre", p_Nombre) :
                new ObjectParameter("P_Nombre", typeof(string));
    
            var p_RucParameter = p_Ruc != null ?
                new ObjectParameter("P_Ruc", p_Ruc) :
                new ObjectParameter("P_Ruc", typeof(string));
    
            var p_TelefonoParameter = p_Telefono != null ?
                new ObjectParameter("P_Telefono", p_Telefono) :
                new ObjectParameter("P_Telefono", typeof(string));
    
            var p_CelularParameter = p_Celular != null ?
                new ObjectParameter("P_Celular", p_Celular) :
                new ObjectParameter("P_Celular", typeof(string));
    
            var p_CorreoParameter = p_Correo != null ?
                new ObjectParameter("P_Correo", p_Correo) :
                new ObjectParameter("P_Correo", typeof(string));
    
            var p_IdVendedorParameter = p_IdVendedor.HasValue ?
                new ObjectParameter("P_IdVendedor", p_IdVendedor) :
                new ObjectParameter("P_IdVendedor", typeof(int));
    
            var p_NomVendedorParameter = p_NomVendedor != null ?
                new ObjectParameter("P_NomVendedor", p_NomVendedor) :
                new ObjectParameter("P_NomVendedor", typeof(string));
    
            var p_TipoVentaParameter = p_TipoVenta != null ?
                new ObjectParameter("P_TipoVenta", p_TipoVenta) :
                new ObjectParameter("P_TipoVenta", typeof(string));
    
            var p_IdMonedaParameter = p_IdMoneda != null ?
                new ObjectParameter("P_IdMoneda", p_IdMoneda) :
                new ObjectParameter("P_IdMoneda", typeof(string));
    
            var p_TasaCambioParameter = p_TasaCambio.HasValue ?
                new ObjectParameter("P_TasaCambio", p_TasaCambio) :
                new ObjectParameter("P_TasaCambio", typeof(decimal));
    
            var p_DireccionEntregaParameter = p_DireccionEntrega != null ?
                new ObjectParameter("P_DireccionEntrega", p_DireccionEntrega) :
                new ObjectParameter("P_DireccionEntrega", typeof(string));
    
            var p_ObservacionParameter = p_Observacion != null ?
                new ObjectParameter("P_Observacion", p_Observacion) :
                new ObjectParameter("P_Observacion", typeof(string));
    
            var p_ExportacionParameter = p_Exportacion.HasValue ?
                new ObjectParameter("P_Exportacion", p_Exportacion) :
                new ObjectParameter("P_Exportacion", typeof(bool));
    
            var p_ExoneracionParameter = p_Exoneracion.HasValue ?
                new ObjectParameter("P_Exoneracion", p_Exoneracion) :
                new ObjectParameter("P_Exoneracion", typeof(bool));
    
            var p_NoExoneracionParameter = p_NoExoneracion != null ?
                new ObjectParameter("P_NoExoneracion", p_NoExoneracion) :
                new ObjectParameter("P_NoExoneracion", typeof(string));
    
            var p_OrdenCompraParameter = p_OrdenCompra != null ?
                new ObjectParameter("P_OrdenCompra", p_OrdenCompra) :
                new ObjectParameter("P_OrdenCompra", typeof(string));
    
            var p_ContraEntregaParameter = p_ContraEntrega.HasValue ?
                new ObjectParameter("P_ContraEntrega", p_ContraEntrega) :
                new ObjectParameter("P_ContraEntrega", typeof(bool));
    
            var p_EsDeliveryParameter = p_EsDelivery.HasValue ?
                new ObjectParameter("P_EsDelivery", p_EsDelivery) :
                new ObjectParameter("P_EsDelivery", typeof(bool));
    
            var p_EstadoParameter = p_Estado != null ?
                new ObjectParameter("P_Estado", p_Estado) :
                new ObjectParameter("P_Estado", typeof(string));
    
            var p_DatosDetalleParameter = p_DatosDetalle != null ?
                new ObjectParameter("P_DatosDetalle", p_DatosDetalle) :
                new ObjectParameter("P_DatosDetalle", typeof(string));
    
            var p_DatosLoteParameter = p_DatosLote != null ?
                new ObjectParameter("P_DatosLote", p_DatosLote) :
                new ObjectParameter("P_DatosLote", typeof(string));
    
            var p_DatosPagoParameter = p_DatosPago != null ?
                new ObjectParameter("P_DatosPago", p_DatosPago) :
                new ObjectParameter("P_DatosPago", typeof(string));
    
            var p_IdUsuarioParameter = p_IdUsuario.HasValue ?
                new ObjectParameter("P_IdUsuario", p_IdUsuario) :
                new ObjectParameter("P_IdUsuario", typeof(int));
    
            var p_LongitudParameter = p_Longitud != null ?
                new ObjectParameter("P_Longitud", p_Longitud) :
                new ObjectParameter("P_Longitud", typeof(string));
    
            var p_LatitudParameter = p_Latitud != null ?
                new ObjectParameter("P_Latitud", p_Latitud) :
                new ObjectParameter("P_Latitud", typeof(string));
    
            var p_IdDireccionParameter = p_IdDireccion.HasValue ?
                new ObjectParameter("P_IdDireccion", p_IdDireccion) :
                new ObjectParameter("P_IdDireccion", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GrabarFactura_Web_Result1>("sp_GrabarFactura_Web", p_IdFacturaParameter, p_IdProformaParameter, p_IdBodegaParameter, p_IdSerieParameter, p_NoFacturaParameter, p_FechaParameter, p_CodClienteParameter, p_NomClienteParameter, p_NombreParameter, p_RucParameter, p_TelefonoParameter, p_CelularParameter, p_CorreoParameter, p_IdVendedorParameter, p_NomVendedorParameter, p_TipoVentaParameter, p_IdMonedaParameter, p_TasaCambioParameter, p_DireccionEntregaParameter, p_ObservacionParameter, p_ExportacionParameter, p_ExoneracionParameter, p_NoExoneracionParameter, p_OrdenCompraParameter, p_ContraEntregaParameter, p_EsDeliveryParameter, p_EstadoParameter, p_DatosDetalleParameter, p_DatosLoteParameter, p_DatosPagoParameter, p_IdUsuarioParameter, p_LongitudParameter, p_LatitudParameter, p_IdDireccionParameter);
        }
    
        public virtual ObjectResult<sp_AnularFacturaVenta_WEB_Result> sp_AnularFacturaVenta_WEB(Nullable<int> p_IdFactura, string p_Serie, string p_NoFactura, Nullable<int> p_IdBodega, string p_MotivoAnulacion, Nullable<int> p_IdusuarioAnula)
        {
            var p_IdFacturaParameter = p_IdFactura.HasValue ?
                new ObjectParameter("P_IdFactura", p_IdFactura) :
                new ObjectParameter("P_IdFactura", typeof(int));
    
            var p_SerieParameter = p_Serie != null ?
                new ObjectParameter("P_Serie", p_Serie) :
                new ObjectParameter("P_Serie", typeof(string));
    
            var p_NoFacturaParameter = p_NoFactura != null ?
                new ObjectParameter("P_NoFactura", p_NoFactura) :
                new ObjectParameter("P_NoFactura", typeof(string));
    
            var p_IdBodegaParameter = p_IdBodega.HasValue ?
                new ObjectParameter("P_IdBodega", p_IdBodega) :
                new ObjectParameter("P_IdBodega", typeof(int));
    
            var p_MotivoAnulacionParameter = p_MotivoAnulacion != null ?
                new ObjectParameter("P_MotivoAnulacion", p_MotivoAnulacion) :
                new ObjectParameter("P_MotivoAnulacion", typeof(string));
    
            var p_IdusuarioAnulaParameter = p_IdusuarioAnula.HasValue ?
                new ObjectParameter("P_IdusuarioAnula", p_IdusuarioAnula) :
                new ObjectParameter("P_IdusuarioAnula", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_AnularFacturaVenta_WEB_Result>("sp_AnularFacturaVenta_WEB", p_IdFacturaParameter, p_SerieParameter, p_NoFacturaParameter, p_IdBodegaParameter, p_MotivoAnulacionParameter, p_IdusuarioAnulaParameter);
        }
    
        public virtual ObjectResult<sp_AnularProformaVenta_Web_Result> sp_AnularProformaVenta_Web(Nullable<int> p_IdProforma, string p_Serie, string p_NoProforma, string p_CodBodega, string p_MotivoAnulacion, Nullable<int> p_IdusuarioAnula)
        {
            var p_IdProformaParameter = p_IdProforma.HasValue ?
                new ObjectParameter("P_IdProforma", p_IdProforma) :
                new ObjectParameter("P_IdProforma", typeof(int));
    
            var p_SerieParameter = p_Serie != null ?
                new ObjectParameter("P_Serie", p_Serie) :
                new ObjectParameter("P_Serie", typeof(string));
    
            var p_NoProformaParameter = p_NoProforma != null ?
                new ObjectParameter("P_NoProforma", p_NoProforma) :
                new ObjectParameter("P_NoProforma", typeof(string));
    
            var p_CodBodegaParameter = p_CodBodega != null ?
                new ObjectParameter("P_CodBodega", p_CodBodega) :
                new ObjectParameter("P_CodBodega", typeof(string));
    
            var p_MotivoAnulacionParameter = p_MotivoAnulacion != null ?
                new ObjectParameter("P_MotivoAnulacion", p_MotivoAnulacion) :
                new ObjectParameter("P_MotivoAnulacion", typeof(string));
    
            var p_IdusuarioAnulaParameter = p_IdusuarioAnula.HasValue ?
                new ObjectParameter("P_IdusuarioAnula", p_IdusuarioAnula) :
                new ObjectParameter("P_IdusuarioAnula", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_AnularProformaVenta_Web_Result>("sp_AnularProformaVenta_Web", p_IdProformaParameter, p_SerieParameter, p_NoProformaParameter, p_CodBodegaParameter, p_MotivoAnulacionParameter, p_IdusuarioAnulaParameter);
        }
    
        public virtual ObjectResult<sp_GrabarProforma_Web_Result> sp_GrabarProforma_Web(Nullable<int> p_IdProforma, string p_CodBodega, string p_IdSerie, string p_NoProforma, string p_CodCliente, string p_NomCliente, string p_Atencion, string p_Nombre, string p_TipoVenta, string p_Ruc, string p_Cedula, string p_Telefono, string p_Celular, string p_Correo, Nullable<int> p_IdVendedor, string p_NomVendedor, string p_IdMoneda, Nullable<System.DateTime> p_Fecha, Nullable<int> p_IdPlazo, Nullable<int> p_Plazo, Nullable<decimal> p_TasaCambio, string p_Observacion, Nullable<bool> p_Exoneracion, string p_NoExoneracion, Nullable<bool> p_EsPublica, string p_Estado, string p_DatosDetalle, Nullable<int> p_IdUsuario, string p_OrdenTaller, string p_Longitud, string p_Latitud)
        {
            var p_IdProformaParameter = p_IdProforma.HasValue ?
                new ObjectParameter("P_IdProforma", p_IdProforma) :
                new ObjectParameter("P_IdProforma", typeof(int));
    
            var p_CodBodegaParameter = p_CodBodega != null ?
                new ObjectParameter("P_CodBodega", p_CodBodega) :
                new ObjectParameter("P_CodBodega", typeof(string));
    
            var p_IdSerieParameter = p_IdSerie != null ?
                new ObjectParameter("P_IdSerie", p_IdSerie) :
                new ObjectParameter("P_IdSerie", typeof(string));
    
            var p_NoProformaParameter = p_NoProforma != null ?
                new ObjectParameter("P_NoProforma", p_NoProforma) :
                new ObjectParameter("P_NoProforma", typeof(string));
    
            var p_CodClienteParameter = p_CodCliente != null ?
                new ObjectParameter("P_CodCliente", p_CodCliente) :
                new ObjectParameter("P_CodCliente", typeof(string));
    
            var p_NomClienteParameter = p_NomCliente != null ?
                new ObjectParameter("P_NomCliente", p_NomCliente) :
                new ObjectParameter("P_NomCliente", typeof(string));
    
            var p_AtencionParameter = p_Atencion != null ?
                new ObjectParameter("P_Atencion", p_Atencion) :
                new ObjectParameter("P_Atencion", typeof(string));
    
            var p_NombreParameter = p_Nombre != null ?
                new ObjectParameter("P_Nombre", p_Nombre) :
                new ObjectParameter("P_Nombre", typeof(string));
    
            var p_TipoVentaParameter = p_TipoVenta != null ?
                new ObjectParameter("P_TipoVenta", p_TipoVenta) :
                new ObjectParameter("P_TipoVenta", typeof(string));
    
            var p_RucParameter = p_Ruc != null ?
                new ObjectParameter("P_Ruc", p_Ruc) :
                new ObjectParameter("P_Ruc", typeof(string));
    
            var p_CedulaParameter = p_Cedula != null ?
                new ObjectParameter("P_Cedula", p_Cedula) :
                new ObjectParameter("P_Cedula", typeof(string));
    
            var p_TelefonoParameter = p_Telefono != null ?
                new ObjectParameter("P_Telefono", p_Telefono) :
                new ObjectParameter("P_Telefono", typeof(string));
    
            var p_CelularParameter = p_Celular != null ?
                new ObjectParameter("P_Celular", p_Celular) :
                new ObjectParameter("P_Celular", typeof(string));
    
            var p_CorreoParameter = p_Correo != null ?
                new ObjectParameter("P_Correo", p_Correo) :
                new ObjectParameter("P_Correo", typeof(string));
    
            var p_IdVendedorParameter = p_IdVendedor.HasValue ?
                new ObjectParameter("P_IdVendedor", p_IdVendedor) :
                new ObjectParameter("P_IdVendedor", typeof(int));
    
            var p_NomVendedorParameter = p_NomVendedor != null ?
                new ObjectParameter("P_NomVendedor", p_NomVendedor) :
                new ObjectParameter("P_NomVendedor", typeof(string));
    
            var p_IdMonedaParameter = p_IdMoneda != null ?
                new ObjectParameter("P_IdMoneda", p_IdMoneda) :
                new ObjectParameter("P_IdMoneda", typeof(string));
    
            var p_FechaParameter = p_Fecha.HasValue ?
                new ObjectParameter("P_Fecha", p_Fecha) :
                new ObjectParameter("P_Fecha", typeof(System.DateTime));
    
            var p_IdPlazoParameter = p_IdPlazo.HasValue ?
                new ObjectParameter("P_IdPlazo", p_IdPlazo) :
                new ObjectParameter("P_IdPlazo", typeof(int));
    
            var p_PlazoParameter = p_Plazo.HasValue ?
                new ObjectParameter("P_Plazo", p_Plazo) :
                new ObjectParameter("P_Plazo", typeof(int));
    
            var p_TasaCambioParameter = p_TasaCambio.HasValue ?
                new ObjectParameter("P_TasaCambio", p_TasaCambio) :
                new ObjectParameter("P_TasaCambio", typeof(decimal));
    
            var p_ObservacionParameter = p_Observacion != null ?
                new ObjectParameter("P_Observacion", p_Observacion) :
                new ObjectParameter("P_Observacion", typeof(string));
    
            var p_ExoneracionParameter = p_Exoneracion.HasValue ?
                new ObjectParameter("P_Exoneracion", p_Exoneracion) :
                new ObjectParameter("P_Exoneracion", typeof(bool));
    
            var p_NoExoneracionParameter = p_NoExoneracion != null ?
                new ObjectParameter("P_NoExoneracion", p_NoExoneracion) :
                new ObjectParameter("P_NoExoneracion", typeof(string));
    
            var p_EsPublicaParameter = p_EsPublica.HasValue ?
                new ObjectParameter("P_EsPublica", p_EsPublica) :
                new ObjectParameter("P_EsPublica", typeof(bool));
    
            var p_EstadoParameter = p_Estado != null ?
                new ObjectParameter("P_Estado", p_Estado) :
                new ObjectParameter("P_Estado", typeof(string));
    
            var p_DatosDetalleParameter = p_DatosDetalle != null ?
                new ObjectParameter("P_DatosDetalle", p_DatosDetalle) :
                new ObjectParameter("P_DatosDetalle", typeof(string));
    
            var p_IdUsuarioParameter = p_IdUsuario.HasValue ?
                new ObjectParameter("P_IdUsuario", p_IdUsuario) :
                new ObjectParameter("P_IdUsuario", typeof(int));
    
            var p_OrdenTallerParameter = p_OrdenTaller != null ?
                new ObjectParameter("P_OrdenTaller", p_OrdenTaller) :
                new ObjectParameter("P_OrdenTaller", typeof(string));
    
            var p_LongitudParameter = p_Longitud != null ?
                new ObjectParameter("P_Longitud", p_Longitud) :
                new ObjectParameter("P_Longitud", typeof(string));
    
            var p_LatitudParameter = p_Latitud != null ?
                new ObjectParameter("P_Latitud", p_Latitud) :
                new ObjectParameter("P_Latitud", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GrabarProforma_Web_Result>("sp_GrabarProforma_Web", p_IdProformaParameter, p_CodBodegaParameter, p_IdSerieParameter, p_NoProformaParameter, p_CodClienteParameter, p_NomClienteParameter, p_AtencionParameter, p_NombreParameter, p_TipoVentaParameter, p_RucParameter, p_CedulaParameter, p_TelefonoParameter, p_CelularParameter, p_CorreoParameter, p_IdVendedorParameter, p_NomVendedorParameter, p_IdMonedaParameter, p_FechaParameter, p_IdPlazoParameter, p_PlazoParameter, p_TasaCambioParameter, p_ObservacionParameter, p_ExoneracionParameter, p_NoExoneracionParameter, p_EsPublicaParameter, p_EstadoParameter, p_DatosDetalleParameter, p_IdUsuarioParameter, p_OrdenTallerParameter, p_LongitudParameter, p_LatitudParameter);
        }
    }
}
