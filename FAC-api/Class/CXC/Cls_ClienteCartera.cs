﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class.CXC
{
    public class Cls_ClienteCartera
    {

        public string Codigo;
        public string Cliente;
        public string Ruc;
        public string Correo;
        public int? IdPlazo;
        public int Plazo;
        public string Telefono;
        public string Direccion { get; set; }
        public string Moneda;
        public decimal Limite;
        public decimal Disponible;
        public decimal SaldoDolar { get; set; }
        public decimal SaldoCordoba { get; set; }
        public string UltimoRoc { get; set; }
        public DateTime? UltimoRocFecha { get; set; }
        public decimal UltimoRocMonto { get; set; }
        public string UltimoRocMoneda { get; set; }
        public string MonedaSistema { get; set; }
        public string MonedaLocal { get; set; }

        public int? IdConceptoPrecio;
        public string CodVendedor;
        public bool? CuentaClave;
        public bool? SuspendidoMoroso;
        public bool? ConfianzaFactSiempre;
        public bool? ConfianzaFactVencido;
        public bool? ConfianzaFactUnaVez;
        public string Estado;
        public string[] Bodegas;
        public string Observaciones;
        public string Usuario;
    }
   
}