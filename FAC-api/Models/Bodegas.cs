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
    
    public partial class Bodegas
    {
        public int IdBodega { get; set; }
        public string Codigo { get; set; }
        public string Bodega { get; set; }
        public Nullable<bool> ColaImpresion { get; set; }
        public Nullable<System.DateTime> FechaFacturacion { get; set; }
        public Nullable<bool> FacturarAnterior { get; set; }
        public string Encabezado_1 { get; set; }
        public string Encabezado_2 { get; set; }
        public string Encabezado_3 { get; set; }
        public string Encabezado_4 { get; set; }
        public string Encabezado_5 { get; set; }
        public string Encabezado_6 { get; set; }
        public string Pie_1 { get; set; }
        public string Pie_2 { get; set; }
        public string Pie_3 { get; set; }
        public string Pie_4 { get; set; }
        public string Pie_5 { get; set; }
        public string Pie_6 { get; set; }
        public Nullable<int> BodegaSuma { get; set; }
        public Nullable<bool> BodegaDelivery { get; set; }
        public Nullable<bool> BodegaExportacion { get; set; }
        public bool BodegaContraEntrega { get; set; }
        public Nullable<bool> PermitirExp { get; set; }
        public string CodVendedor { get; set; }
        public string CodCliente { get; set; }
        public Nullable<bool> BodForanea { get; set; }
        public Nullable<bool> BodegaCEDI { get; set; }
        public string Correo { get; set; }
        public Nullable<bool> EsConsignacion { get; set; }
        public Nullable<int> BodSumConsignacion { get; set; }
        public string CuentaInventario { get; set; }
        public string ImpresoraFact { get; set; }
    }
}
