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
    
    public partial class Perfiles1
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Cargo { get; set; }
        public string Ubicacion { get; set; }
        public string Bodega { get; set; }
        public bool Facturacion { get; set; }
        public bool Caja { get; set; }
        public bool CuentasxCobrar { get; set; }
        public bool ReportesdeVenta { get; set; }
        public bool Inventarios { get; set; }
        public bool Importaciones { get; set; }
        public bool Utiles { get; set; }
        public bool Presupuesto { get; set; }
        public bool Contabilidad { get; set; }
        public bool ReportesdeVentas { get; set; }
        public bool Graba { get; set; }
        public bool Modifica { get; set; }
        public bool Anula { get; set; }
        public bool Consulta { get; set; }
        public bool Facturas { get; set; }
        public bool ModificaFacturas { get; set; }
        public bool AnulaFacturas { get; set; }
        public bool ReporesFactura { get; set; }
        public bool ReimprimeFacturas { get; set; }
        public bool Tablas { get; set; }
        public bool ParametrosFacturacion { get; set; }
        public bool InterfazContable { get; set; }
        public bool TransferenciaInformacion { get; set; }
        public bool Auditoria { get; set; }
        public bool Usuarios { get; set; }
        public bool CatalogoProductos { get; set; }
        public bool Proveedores { get; set; }
        public bool Compras { get; set; }
        public bool Traslados { get; set; }
        public bool Ajustes { get; set; }
        public bool Devoluciones { get; set; }
        public bool Armados { get; set; }
        public bool Consignaciones { get; set; }
        public bool ReportesInventario { get; set; }
        public bool TarjetasKardex { get; set; }
        public bool InsertaInventario { get; set; }
        public bool ActualizaInventario { get; set; }
        public bool ReportesCosto { get; set; }
        public bool CerrarInv { get; set; }
        public bool ConsultaInventario { get; set; }
        public bool AnulaInventario { get; set; }
        public bool InsertaFacturacion { get; set; }
        public bool ConsultaFacturacion { get; set; }
        public bool AnulaFacturacion { get; set; }
        public bool ActualizaFacturacion { get; set; }
        public bool InsertaCxC { get; set; }
        public bool ActualizaCxC { get; set; }
        public bool ConsultaCxC { get; set; }
        public bool AnulaCxC { get; set; }
        public bool ModificaPreciosVenta { get; set; }
        public bool Clientes { get; set; }
        public bool NotasCredito { get; set; }
        public bool Proceso { get; set; }
        public bool PorIva { get; set; }
        public bool ACTIVO { get; set; }
        public bool Fisico { get; set; }
        public bool CatalogoCuentas { get; set; }
        public bool Diarios { get; set; }
        public bool Cheque { get; set; }
        public bool EstadosFinancieros { get; set; }
        public bool ActualizarSaldosLibro { get; set; }
        public bool ReportesC { get; set; }
        public bool CierreCuentas { get; set; }
        public bool Nomina { get; set; }
        public bool Gestion { get; set; }
        public bool ProcesosNomina { get; set; }
        public bool Prefarmaco { get; set; }
        public bool PreEquipos { get; set; }
        public bool Preconsumo { get; set; }
        public bool Pedidos { get; set; }
        public bool ProcesoNomina { get; set; }
        public bool EditaClientes { get; set; }
        public bool CuentaporPagar { get; set; }
        public bool PresupuestoGastos { get; set; }
        public bool InformacionCrediticia { get; set; }
        public bool FichaCliente { get; set; }
        public bool AbonoSuper { get; set; }
        public bool AutorizaCr { get; set; }
        public bool InformacionCredicticia { get; set; }
        public bool EstadoCuenta { get; set; }
        public bool InformesRecupera { get; set; }
        public bool InformesCartera { get; set; }
        public bool ModificaPunto { get; set; }
        public bool EscFacturacion { get; set; }
        public bool EscCaja { get; set; }
        public bool EscCartera { get; set; }
        public bool EscInventario { get; set; }
        public bool EscReporteria { get; set; }
        public bool EscCorrecciones { get; set; }
        public bool EscFacturas { get; set; }
        public bool EscReporteVentas { get; set; }
        public bool EscReimprime { get; set; }
        public bool EscAnula { get; set; }
        public bool EscPedidos { get; set; }
        public bool EscFacturaRuta { get; set; }
        public bool EscProforma { get; set; }
        public bool EscCertificado { get; set; }
        public bool EscExistenciasBodega { get; set; }
        public bool EscTransInventario { get; set; }
        public bool EscReporteInventario { get; set; }
        public bool ValidaInventarios { get; set; }
        public bool AplicarDeslizamiento { get; set; }
        public bool Certificados { get; set; }
        public bool Lista { get; set; }
        public bool OtrasUtilidades { get; set; }
        public bool ProformaProveedores { get; set; }
        public bool IngresosPolizas { get; set; }
        public bool Liquidaciones { get; set; }
        public bool CxP { get; set; }
        public bool Syscompras { get; set; }
        public bool SysRequisa { get; set; }
        public bool SysReempaque { get; set; }
        public bool SysMovimientos { get; set; }
        public bool SysDevoluciones { get; set; }
        public bool Syskardex { get; set; }
        public bool SysreImprimir { get; set; }
        public bool Sysimprimirpedido { get; set; }
        public bool Syscancelarpedido { get; set; }
        public bool Syscertificado { get; set; }
        public bool EscReporteVenta { get; set; }
        public bool EscPresupuesto { get; set; }
        public bool Web { get; set; }
        public string Correo { get; set; }
        public string Nivel { get; set; }
        public bool LE { get; set; }
        public bool LF { get; set; }
        public bool LC { get; set; }
        public bool LP { get; set; }
        public bool Margenes { get; set; }
        public bool CuentasPorPagar { get; set; }
        public bool Expica { get; set; }
        public bool Proforma { get; set; }
        public bool CorrigeCheques { get; set; }
        public bool UserAgenda { get; set; }
        public bool Agenda { get; set; }
        public bool SSV { get; set; }
        public bool TodasPestMovil { get; set; }
    }
}
