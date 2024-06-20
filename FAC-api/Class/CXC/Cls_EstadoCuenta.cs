using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.CXC
{
    public class Cls_EstadoCuenta
    {
        public DateTime FechaDoc { get; set; }
        public string NoDocOrigen { get; set; }
        public string AplicadoA { get; set; }
        public string Concepto { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Saldo { get; set; }
        public decimal Corriente { get; set; }
        public decimal Vencido { get; set; }
        public decimal De1a30Dias { get; set; }
        public decimal De31a60Dias { get; set; }
        public decimal De61a90Dias { get; set; }
        public decimal De91a120Dias { get; set; }
        public decimal De121aMasDias { get; set; }
        public int? DiasV { get; set; }
        public string IdMoneda = "";
        public bool Expandir = false;
    }
}