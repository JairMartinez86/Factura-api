﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class INVESCASANEntities : DbContext
    {
        public INVESCASANEntities()
            : base("name=INVESCASANEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<TbBodega> TbBodega { get; set; }
        public virtual DbSet<tbVendedores> tbVendedores { get; set; }
        public virtual DbSet<Catalogo> Catalogo { get; set; }
        public virtual DbSet<Listadeprecios> Listadeprecios { get; set; }
        public virtual DbSet<Kardex> Kardex { get; set; }
        public virtual DbSet<Bonificados> Bonificados { get; set; }
        public virtual DbSet<ConfiguraFacturacion> ConfiguraFacturacion { get; set; }
        public virtual DbSet<TbDepartamento> TbDepartamento { get; set; }
        public virtual DbSet<ControlInventario> ControlInventario { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<VentaDetalle> VentaDetalle { get; set; }
    
        public virtual ObjectResult<Nullable<decimal>> spu_ObtenerSaldoCuenta(string numCuenta, Nullable<System.DateTime> fecha, string dMoneda)
        {
            var numCuentaParameter = numCuenta != null ?
                new ObjectParameter("numCuenta", numCuenta) :
                new ObjectParameter("numCuenta", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var dMonedaParameter = dMoneda != null ?
                new ObjectParameter("dMoneda", dMoneda) :
                new ObjectParameter("dMoneda", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("spu_ObtenerSaldoCuenta", numCuentaParameter, fechaParameter, dMonedaParameter);
        }
    
        public virtual int RetornaSaldoVencidoAmbasMonedas(string numCuenta, Nullable<System.DateTime> fecha, Nullable<int> ddias, ObjectParameter dResultado)
        {
            var numCuentaParameter = numCuenta != null ?
                new ObjectParameter("numCuenta", numCuenta) :
                new ObjectParameter("numCuenta", typeof(string));
    
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("fecha", fecha) :
                new ObjectParameter("fecha", typeof(System.DateTime));
    
            var ddiasParameter = ddias.HasValue ?
                new ObjectParameter("Ddias", ddias) :
                new ObjectParameter("Ddias", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RetornaSaldoVencidoAmbasMonedas", numCuentaParameter, fechaParameter, ddiasParameter, dResultado);
        }
    
        public virtual ObjectResult<SP_FacturaImpresa_Result> SP_FacturaImpresa(Nullable<System.Guid> p_IdVenta)
        {
            var p_IdVentaParameter = p_IdVenta.HasValue ?
                new ObjectParameter("P_IdVenta", p_IdVenta) :
                new ObjectParameter("P_IdVenta", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_FacturaImpresa_Result>("SP_FacturaImpresa", p_IdVentaParameter);
        }
    }
}
