﻿using DevExpress.Data;
using DevExpress.DataProcessing;
using DevExpress.DirectX.Common.Direct2D;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraCharts.Native;
using FAC_api.Class;
using FAC_api.Class.FACT;
using FAC_api.Class.INV;
using FAC_api.Class.SIS;
using FAC_api.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Objects;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using IsolationLevel = System.Transactions.IsolationLevel;
using RouteAttribute = System.Web.Http.RouteAttribute;


namespace FAC_api.Controllers.FACT
{
    public class FacturaController : ApiController
    {


        [Route("api/Factura/DatosSucursal")]
        [HttpGet]
        public string DatosSucursal(string CodBodega, string TipoFactura)
        {
            return v_DatosSucursal(CodBodega, TipoFactura);
        }

        private string v_DatosSucursal(string CodBodega, string TipoFactura)
        {
            string json = string.Empty;
            if (CodBodega == null) CodBodega = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "FECHA FACTURA";
                    datos.d = DateTime.Now;
                    lstDatos.Add(datos);


                    datos = new Cls_Datos();
                    datos.Nombre = "TASA CAMBIO";
                    datos.d = f_TasaCambio(_Conexion, DateTime.Now);
                    lstDatos.Add(datos);

                    string NoDoc = string.Empty;
                    string Serie = string.Empty;

                    if(CodBodega != string.Empty)
                    {
                        if (TipoFactura == "Factura")
                        {
                            BodegaSerie cons = _Conexion.BodegaSerie.FirstOrDefault(f => f.CodBodega == CodBodega && f.EsFact);
                            Serie _num = _Conexion.Serie.FirstOrDefault(f => f.IdSerie == cons.Serie);


                            NoDoc = _num == null ? string.Empty : string.Concat(_num.IdSerie, _num.Factura + 1);
                            Serie = _num == null ? string.Empty : _num.IdSerie;
                        }
                        else
                        {
                            BodegaSerie cons = _Conexion.BodegaSerie.FirstOrDefault(f => f.CodBodega == CodBodega && f.EsFact);
                            Serie _num = _Conexion.Serie.FirstOrDefault(f => f.IdSerie == cons.Serie);


                            NoDoc = _num == null ? string.Empty : string.Concat(_num.IdSerie, _num.Proforma + 1);
                            Serie = _num == null ? string.Empty : _num.IdSerie.TrimStart().TrimEnd();
                        }
                    }
                    


                    datos = new Cls_Datos();
                    datos.Nombre = "CONSECUTIVO";
                    datos.d = string.Concat(NoDoc, ";", Serie);
                    lstDatos.Add(datos);



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



        private decimal f_TasaCambio(BalancesEntities _Conexion, DateTime fecha)
        {           
            List<decimal> lst = _Conexion.Database.SqlQuery<decimal>($"SELECT TasaCambio FROM CON.TasaCambio WHERE Fecha = CAST('{string.Format("{0:yyyy-MM-dd}", fecha)}' AS DATE)").ToList();

            decimal tc = 1;
            if (lst.Count > 0) tc = lst.First();
            return tc;
        }

        [Route("api/Factura/Datos")]
        [HttpGet]
        public string Datos(string user)
        {
            return v_Datos(user);
        }


        private string v_Datos(string user)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    Usuarios U = _Conexion.Usuarios.FirstOrDefault( f=> f.Usuario == user);
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();

          
                    var qClientes = (from _q in _Conexion.Cliente
                                 select new
                                 {
                                     Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                     Cliente = _q.Nombre.TrimStart().TrimEnd(),
                                     Ruc = string.Empty,
                                     Cedula = _q.NoCedula.TrimStart().TrimEnd(),
                                     Contacto = string.Concat(_q.Celular.TrimStart().TrimEnd(),"/",_q.Telefono1.TrimStart().TrimEnd(), "/", _q.Telefono2.TrimStart().TrimEnd(), "/", _q.Telefono3.TrimStart().TrimEnd()),
                                     Limite = _q.Limite,
                                     Moneda = _q.IdMoneda,
                                     CodVendedor = (_q.Vendedor == null ? string.Empty : _q.Vendedor.TrimStart().TrimEnd()),
                                     EsClave = _q.ClienteClave,
                                     Filtro = string.Concat(_q.Codigo, _q.Nombre.TrimStart().TrimEnd(), _q.NoCedula.TrimStart().TrimEnd(), _q.Telefono1.TrimStart().TrimEnd(), _q.Telefono2.TrimStart().TrimEnd(), _q.Telefono3.TrimStart().TrimEnd(), _q.Celular.TrimStart().TrimEnd()),
                                     Key = string.Concat(_q.Codigo, " ", _q.Nombre.TrimStart().TrimEnd())
                                 }).ToList();

                    datos.Nombre = "CLIENTES";
                    datos.d = qClientes;
                    lstDatos.Add(datos);



                    List<string> ub = (from _q in _Conexion.UsuariosBodegas
                                       join _b in _Conexion.Bodegas on _q.IdBodega equals _b.IdBodega
                                       where _q.IdUsuario == U.IdUsuario
                                       select _b.Codigo).ToList();



                    var qBodegas = (from _q in _Conexion.Bodegas
                                    where ub.Contains(_q.Codigo)
                                    select new Cls_Bodega()
                                     {
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Bodega = _q.Bodega.TrimStart().TrimEnd(),
                                        ClienteContado = _q.CodCliente,
                                        Vendedor = _q.CodVendedor,
                                        EsContraEntrega = _q.BodegaContraEntrega,
                                        EsExportacion = _q.BodegaExportacion == null ? false : (bool)_q.BodegaExportacion,
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Bodega.TrimStart().TrimEnd())
                                    }).ToList();


                    foreach(Cls_Bodega b in qBodegas)
                    {
                        BodegaSerie c = _Conexion.BodegaSerie.FirstOrDefault(f => f.CodBodega.TrimStart().TrimEnd() == b.Codigo && f.EsFact);

                        b.Facturar = false;
                        if(c != null) b.Facturar = true;

                    }
                    Cls_Bodega[] bEliminar = qBodegas.FindAll(f => !f.Facturar).ToArray();

                    foreach (Cls_Bodega b in bEliminar)
                    {
                        qBodegas.Remove(b);
                    }
                        



                    datos = new Cls_Datos();
                    datos.Nombre = "BODEGAS";
                    datos.d = qBodegas;
                    lstDatos.Add(datos);


                    var qVendedores = (from _q in _Conexion.Vendedor
                                    select new
                                    {
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Vendedor = _q.Nombre.TrimStart().TrimEnd(),
                                        Key = string.Concat(_q.Codigo, " ", _q.Nombre.TrimStart().TrimEnd())
                                    }).ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "VENDEDORES";
                    datos.d = qVendedores;
                    lstDatos.Add(datos);


                    

                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        [Route("api/Factura/DatosCredito")]
        [HttpGet]
        public string DatosCredito(string CodCliente)
        {
            return v_DatosCredito(CodCliente);
        }

        private string v_DatosCredito(string CodCliente)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();

                    Cliente cl = _Conexion.Cliente.FirstOrDefault(f => f.Codigo == CodCliente);
                    decimal Tc = f_TasaCambio(_Conexion, DateTime.Now);
                    decimal Techo = cl.Limite;
                    decimal Disponible = 0m;
                    decimal Saldo = 0m;
                    ObjectParameter output = new ObjectParameter("dResultado", typeof(decimal));



                    string Sql = $"DECLARE @SaldoVencido DECIMAL(18,2),\r\n\t\t@Saldo DECIMAL(18,2)\r\n\t\t,@P_Fecha AS DATETIME = GETDATE()\r\n\t\t, @dResultado MONEY\r\n\r\n\r\n\r\n\t\tEXEC INVESCASAN.[dbo].[RetornaSaldoVencidoAmbasMonedas] {CodCliente}, @P_Fecha, 15, @dResultado OUT\r\n\r\n\t\tSET @dResultado = ISNULL(@dResultado, 0)\r\n\r\n\t\tIF @dResultado < 5 \r\n\t\tBEGIN\r\n\t\t\tSET @dResultado = 0\r\n\t\tEND\r\n\r\n\r\n\r\n\t\tEXEC [INVESCASAN].dbo.[RetornaSaldoAmbasMonedas] {CodCliente},  @P_Fecha, 30, @Saldo OUT\r\n\r\n\t\tSET @Saldo = ISNULL(@Saldo, 0)\r\n\r\n\t\tSELECT @Saldo";



                    Saldo =  _Conexion.Database.SqlQuery<decimal>(Sql).Single();


                    Disponible =  Math.Round(Techo - Saldo, 2, MidpointRounding.ToEven);



                    var qCredito = (from _q in _Conexion.Cliente
                                     where _q.Codigo == CodCliente
                                     select new
                                     {
                                         CodCliente = _q.Codigo,
                                         Limite = _q.Limite,
                                         Plazo = _q.Plazo,
                                         Gracia = _q.DiasG,
                                         Moneda = _q.IdMoneda,
                                         Disponible = Disponible,
                                         FacturarVencido = _q.FacturarVencido,
                                         SaldoVencido = Saldo
                                     }).ToList();

                    datos.Nombre = "CREDITO";
                    datos.d = qCredito;
                    lstDatos.Add(datos);

                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        [Route("api/Factura/ClienteClave")]
        [HttpGet]
        public string ClienteClave(string CodCliente)
        {
            return v_ClienteClave(CodCliente);
        }

        private string v_ClienteClave(string CodCliente)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();

                    var qClienteClave = (from _q in _Conexion.Cliente
                                         join _v in _Conexion.Vendedor on _q.Vendedor equals _v.Codigo into _q_v
                                         from _c in _q_v.DefaultIfEmpty()
                                         where _q.Codigo == CodCliente
                                         select new
                                         {
                                             CodVendedor = (_c == null ? string.Empty : _c.Codigo),
                                             Vendedor = (_c == null? string.Empty : string.Concat(_c.Codigo, " ", _c.Nombre.TrimStart().TrimEnd())),
                                             EsClave = (_c == null ? false : true)
                                         }).ToList();



                    datos.Nombre = "ESCALVE";
                    datos.d = qClienteClave;
                    lstDatos.Add(datos);

                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        [Route("api/Factura/CargarProductos")]
        [HttpGet]
        public string CargarProductos(string CodBodega)
        {
            return v_CargarProductos(CodBodega);
        }

        private string v_CargarProductos(string CodBodega)
        {
            string json = string.Empty;
            
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Cls_Datos datos = new Cls_Datos();
                    Bodegas bo = _Conexion.Bodegas.FirstOrDefault(f => f.Codigo == CodBodega);



                    var qProductos = (from _q in _Conexion.Productos
                                      join _i in _Conexion.Impuestos on _q.IdImpuesto equals _i.IdImpuesto into _q_i
                                      from _T in _q_i.DefaultIfEmpty()
                                      where _q.Activo == true
                                      select new
                                         {
                                             Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                             Producto = _q.Producto.TrimStart().TrimEnd(),
                                             ConImpuesto = (_T == null ? true :  _T.Impuesto == "NO IVA"? false : true),
                                             Key = string.Concat(_q.Codigo, " ", _q.Producto.TrimStart().TrimEnd()),
                                             Bonificable = _q.AplicarBonificacion,
                                             FacturaNegativo = (_q.Servicios == null ? (_q.FacturaNegativo == null ? false : _q.FacturaNegativo ) : ((bool)!_q.Servicios ? (_q.FacturaNegativo == null ? false : _q.FacturaNegativo) : false ) )
                                         }).ToList();



                    datos.Nombre = "PRODUCTOS";
                    datos.d = qProductos;
                    lstDatos.Add(datos);


                    datos = new Cls_Datos();
                    datos.Nombre = "TASACAMBIO";
                    datos.d = f_TasaCambio(_Conexion, DateTime.Now);
                    lstDatos.Add(datos);



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        [Route("api/Factura/DatosProducto")]
        [HttpGet]
        public string DatosProducto(string CodProducto, string CodBodega, string CodCliente, string User)
        {
            return v_DatosProducto(CodProducto, CodBodega, CodCliente, User);
        }


        private string v_DatosProducto(string CodProducto, string CodBodega, string CodCliente,  string User)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Cliente cl = _Conexion.Cliente.FirstOrDefault(f => f.Codigo.TrimStart().TrimEnd() == CodCliente);
                    Bodegas bo = _Conexion.Bodegas.FirstOrDefault(f => f.Codigo.TrimStart().TrimEnd() == CodBodega);
                    ConceptoPrecio cp = _Conexion.ConceptoPrecio.FirstOrDefault(f => f.IdConceptoPrecio == cl.IdConceptoPrecio);
                    List<Parametros> lstParametro = _Conexion.Parametros.ToList();
                    Usuarios U = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == User);
                    string str_MonedaLocal = lstParametro[0].MonedaLocal;
          

                    Cls_Datos datos = new Cls_Datos();
                    int x = 0;
                    string strTipo = cp.Descripcion.TrimStart().TrimEnd();
                
                    decimal Tc = f_TasaCambio(_Conexion, bo.FechaFacturacion.Value);
                    bool LiberadoPrecio = false;
                    int IdLiberacion = 0;


                    List<LiberarPrecio> lip  = (from _q in _Conexion.LiberarPrecio
                                               join _p in _Conexion.Productos on _q.IdProducto equals _p.IdProducto
                                               where _p.Codigo.TrimStart().TrimEnd() == CodProducto
                                               select _q).ToList();

                    LiberadoPrecio = false;

                    if (lip.Count > 0)
                    {
                        if (lip.FirstOrDefault(w => w.IdBodega == bo.IdBodega) != null)
                        {
                            LiberadoPrecio = true;
                            IdLiberacion = lip.FirstOrDefault(w => w.IdBodega == bo.IdBodega).IdLiberarPrecio;
                        }
                        if (lip.FirstOrDefault(w => w.IdCliente == cl.IdCliente) != null && !LiberadoPrecio)
                        {
                            LiberadoPrecio = true;
                            IdLiberacion = lip.FirstOrDefault(w => w.IdCliente == cl.IdCliente).IdLiberarPrecio;
                        }
                    }


                    List<ExistenciaUbicacion> qUbicacion;

                   List<string> ub = (from _q in _Conexion.UsuariosBodegas
                                              join _b in _Conexion.Bodegas on _q.IdBodega equals _b.IdBodega
                                              where  _q.IdUsuario == U.IdUsuario
                                              select _b.Codigo.TrimStart().TrimEnd()).ToList();

                    if (!EsINVESCASAN(CodProducto, CodBodega, _Conexion))
                    {
                        qUbicacion = (from _q in _Conexion.Kardex
                                      where _q.CodProducto.TrimStart().TrimEnd() == CodProducto  && ub.Contains(_q.Bodega) && _q.Anulado == false
                                      group _q by new { _q.CodProducto, _q.Bodega} into g
                                      select new ExistenciaUbicacion
                                      {
                                          Key = string.Concat(g.Key.CodProducto.TrimStart().TrimEnd(), g.Key.Bodega),
                                          CodProducto = g.Key.CodProducto.TrimStart().TrimEnd(),
                                          Bodega = g.Key.Bodega.TrimStart().TrimEnd(),
                                          Ubicacion = string.Empty,
                                          Existencia = g.Sum(s => s.Cantidad),
                                          NoLote = string.Empty,
                                          Vence = null,
                                          EsPrincipal = g.Key.Bodega.TrimStart().TrimEnd() == CodBodega? true : false,
                                      }).ToList();


                            qUbicacion = qUbicacion.Where(w => w.Existencia > 0).ToList();

                 
                    }
                    else
                    {

                        string where_bodega = string.Empty;


                        foreach( string _bodega in ub)
                        {
                            where_bodega +=  $"'{_bodega}',";
                        }

                        where_bodega = where_bodega.Substring(0, where_bodega.Length - 1);


                        List<ExistenciaUbicacion> qExistencia = _Conexion.Database.SqlQuery<ExistenciaUbicacion>($"SELECT CONCAT(CAST(LTRIM(RTRIM(T.CodiProd)) AS NVARCHAR(12)), RTRIM(LTRIM(T.Bodega))) AS [Key], T.CodiProd AS CodProducto, RTRIM(LTRIM(T.Bodega)) AS Bodega, '' AS Ubicacion,  SUM(T.Entrada - T.Salidas)  AS Existencia,  '' AS NoLote, NULL AS Vence, CAST(IIF(RTRIM(LTRIM(T.Bodega)) = '{CodBodega}', 1, 0) AS BIT) AS EsPrincipal \r\nFROM INVESCASAN.DBO.Kardex AS T\r\nWHERE T.CodiProd = '{CodProducto}' AND T.Bodega IN( {where_bodega}) \r\nGROUP BY T.CodiProd, T.Bodega").ToList();

                        foreach (ExistenciaUbicacion neg in qExistencia.Where(w => w.Existencia < 0))
                        {

                            

                            foreach (ExistenciaUbicacion ex in qExistencia.Where(w => w.Existencia > 0))
                            {
                                if (ex != null)
                                {
                                    if (ex.Existencia + neg.Existencia >= 0)
                                    {
                                        ex.Existencia += neg.Existencia;
                                        neg.Existencia = 0;
                                    }
                                    else
                                    {
                                        neg.Existencia += ex.Existencia;
                                        ex.Existencia = 0;
                                    }


                                }


                            }


                        }
                        qUbicacion = qExistencia.Where(w => w.Existencia > 0).ToList();

                    }


                    
                if (qUbicacion.Count == 0)
                    {
                        qUbicacion = new List<ExistenciaUbicacion>();
                        ExistenciaUbicacion u = new ExistenciaUbicacion();
                        u.Key = "S/L";
                        u.CodProducto = CodProducto;
                        u.Bodega = CodBodega;
                        u.Ubicacion = "A00-00";
                        u.NoLote = "S/L";
                        u.Vence = null;
                        u.Existencia = 0;
                        qUbicacion.Add(u);
                    }




                    qUbicacion = qUbicacion.OrderBy(o => o.Bodega).ThenBy(o => o.Existencia).ToList();



                    datos.Nombre = "EXISTENCIA";
                    datos.d = qUbicacion;
                    lstDatos.Add(datos);



                    List<Cls_Bonificacion> qBonificacion = (from _q in _Conexion.Bonificaciones
                                                            where _q.CodigoProducto.TrimStart().TrimEnd() == CodProducto && _q.CodBodega == CodBodega
                                                            orderby _q.CantMin
                                                            group _q by new { _q.CodigoProducto, _q.CantMin } into g
                                                            select new Cls_Bonificacion()
                                                            {
                                                                CodProducto = g.Key.CodigoProducto.TrimStart().TrimEnd(),
                                                                Escala = string.Concat(g.Key.CantMin, "+", g.FirstOrDefault().Bonificar),
                                                                Desde = g.Key.CantMin,
                                                                Hasta = 0,
                                                                Bonifica = g.FirstOrDefault().Bonificar,
                                                                Descripcion = string.Empty,
                                                                IdEscala = 0
                                                            }).ToList();


                    foreach (var b in qBonificacion.OrderBy(o => o.Desde))
                    {
                        int max = 9999;
                        Cls_Bonificacion sig = qBonificacion.OrderBy(o => o.Desde).FirstOrDefault(f => f.Desde > b.Desde);

                        if (sig != null) max = sig.Desde - 1;

                        b.Index = x;
                        b.Hasta = max;
                        b.Descripcion = string.Concat("Desde ",b.Desde, " Hasta ", b.Hasta);

                        x++;
                    }




                    datos = new Cls_Datos();
                    datos.Nombre = "BONIFICACION";
                    datos.d = qBonificacion;
                    lstDatos.Add(datos);



                    List<Cls_Precio> qPrecios = (from _q in _Conexion.PrecioVenta
                                                 join _c in _Conexion.ConceptoPrecio on _q.IdConceptoPrecio equals _c.IdConceptoPrecio
                                                 where _q.CodigoProducto.TrimStart().TrimEnd() == CodProducto && _q.Activo == true
                                                 orderby _c.Descripcion
                                                 select new Cls_Precio()
                                                 {
                                                     Index = -1,
                                                     CodProducto = _q.CodigoProducto.TrimStart().TrimEnd(),
                                                     Tipo = _c.Descripcion.TrimStart().TrimEnd(),
                                                     PrecioCordoba = ((decimal)(str_MonedaLocal == _q.IdMoneda ? _q.Precio : _q.Precio * Tc)),
                                                     PrecioDolar = ((decimal)(str_MonedaLocal != _q.IdMoneda ? _q.Precio : _q.Precio / Tc)),
                                                     EsPrincipal = (_c.Descripcion.TrimStart().TrimEnd() == strTipo ? true : false),
                                                     Desde = 0,
                                                     Hasta = 0,
                                                     EsEscala = false,
                                                     Liberado = false,
                                                     IdPrecioFAC = _q.IdPrecioFAC,
                                                     IdLiberacion = 0
                                                 }).Union(
                        from _q in _Conexion.PrecioVentaEscala
                        where _q.Activo && _q.CodigoProducto == CodProducto && _q.IdConceptoPrecio == cl.IdConceptoPrecio
                        select new Cls_Precio()
                        {
                            Index = 0,
                            CodProducto = _q.CodigoProducto.TrimStart().TrimEnd(),
                            Tipo = "",
                            PrecioCordoba = ((decimal)(str_MonedaLocal == _q.IdMoneda ? _q.Precio : _q.Precio * Tc)),
                            PrecioDolar = ((decimal)(str_MonedaLocal != _q.IdMoneda ? _q.Precio : _q.Precio / Tc)),
                            EsPrincipal = false,
                            Desde = _q.CantMin,
                            Hasta = 0,
                            EsEscala = true,
                            Liberado = false,
                            IdPrecioFAC = _q.IdPrecioEscala,
                            IdLiberacion = 0
                        }
                        ).ToList();


                    x = 0;
                    foreach (var b in qPrecios.Where(w => w.EsEscala).OrderBy(o => o.Desde))
                    {
                        int max = 9999;
                        Cls_Precio sig = qPrecios.OrderBy(o => o.Desde).FirstOrDefault(f => f.Desde > b.Desde);

                        if (sig != null) max = sig.Desde - 1;

                        b.Index = x;
                        b.Hasta = max;
                        b.Tipo = string.Concat("Desde ", b.Desde, " Hasta ", b.Hasta);

                        x++;
                    }



                  

                    Cls_Precio iPrecio = qPrecios.FirstOrDefault(f => f.EsPrincipal);

                    if(qPrecios != null)
                    {

                        if (iPrecio.PrecioCordoba == 0)
                        {
                            iPrecio.EsPrincipal = false;
                            iPrecio = qPrecios.FirstOrDefault(f => f.Tipo == "Distribuidor");
                            if (iPrecio != null) iPrecio.EsPrincipal = true;
                        }

                        if (iPrecio.PrecioCordoba == 0)
                        {
                            iPrecio.EsPrincipal = false;
                            iPrecio = qPrecios.FirstOrDefault(f => f.Tipo == "Publico");
                            if (iPrecio != null) iPrecio.EsPrincipal = true;
                        }

                        if (iPrecio.EsPrincipal)
                        {
                            iPrecio.Liberado = LiberadoPrecio;
                            iPrecio.IdLiberacion = IdLiberacion;
                        }

                    }

                    qPrecios = qPrecios.OrderBy(o => o.Index).ToList();

                    



                    datos = new Cls_Datos();
                    datos.Nombre = "PRECIOS";
                    datos.d = qPrecios;
                    lstDatos.Add(datos);



                    List<Descuentos> lstDes = _Conexion.Descuentos.Where(f => f.CodigoProducto == CodProducto).ToList();
                    List<Cls_Descuento> C_Descuento = new List<Cls_Descuento>() { new Cls_Descuento()  , new Cls_Descuento() } ;

                    C_Descuento[0].Index = 0;
                    C_Descuento[0].CodProducto = CodProducto;
                    C_Descuento[0].Descripcion = "GENERAL";
                    C_Descuento[0].PorcDescuento = 0;
                    C_Descuento[0].IdDescuentoDet = 0;

                    C_Descuento[1].Index = 1;
                    C_Descuento[1].CodProducto = CodProducto;
                    C_Descuento[1].Descripcion = "ADICIONAL";
                    C_Descuento[1].PorcDescuento = 0;
                    C_Descuento[1].IdDescuentoDet = 0;

                    if (lstDes.Count > 0)
                    {
            
                        Descuentos des = lstDes.FirstOrDefault(w => w.CodigoCliente.TrimStart().TrimEnd() == cl.Codigo.TrimStart().TrimEnd() && ((w.CodBodega.TrimStart().TrimEnd() == null ? CodBodega : w.CodBodega.TrimStart().TrimEnd()) == CodBodega) && (w.FechaInicio.Date == null ? bo.FechaFacturacion.Value.Date : w.FechaInicio.Date) >= bo.FechaFacturacion.Value.Date && (w.FechaFinaliza.Value.Date == null ? bo.FechaFacturacion.Value.Date : w.FechaFinaliza.Value.Date) <= bo.FechaFacturacion.Value.Date);


                        if (des != null)//DESCUENTO POR CLIENTE
                        {
                            C_Descuento[0].PorcDescuento = des.PorcDesc;
                            C_Descuento[1].PorcDescuento = des.PorcDescA;
                            C_Descuento[0].IdDescuentoDet = des.IdDescuento;
                            C_Descuento[1].IdDescuentoDet = des.IdDescuento;
                        }
                        else
                        {
                            des = lstDes.FirstOrDefault(w => ((w.CodBodega == null ? CodBodega : w.CodBodega) == CodBodega) && (w.FechaInicio.Date == null ? bo.FechaFacturacion.Value.Date : w.FechaInicio.Date) >= bo.FechaFacturacion.Value.Date && (w.FechaFinaliza.Value.Date == null ? bo.FechaFacturacion.Value.Date : w.FechaFinaliza.Value.Date) <= bo.FechaFacturacion.Value.Date);

                            if(des == null)
                            {
                                des = lstDes.FirstOrDefault(w => ((w.CodBodega == null ? CodBodega : w.CodBodega) == CodBodega) && w.FechaInicio.Date == (new DateTime(1900,1,1).Date) );

                            }


                            if (des != null)//DESCUENTO BODEGA NULL (GENERAL)
                            {
                                C_Descuento[0].PorcDescuento = des.PorcDesc;
                                C_Descuento[1].PorcDescuento = des.PorcDescA;
                                C_Descuento[0].IdDescuentoDet = des.IdDescuento;
                                C_Descuento[1].IdDescuentoDet = des.IdDescuento;
                            }
                           
                        }


                    }






                    if(C_Descuento.FindIndex(f => f.Descripcion == "GENERAL" && f.PorcDescuento == 0m) != -1)
                    {

                        decimal PorcDescMargen = 0;
                        decimal PrecioP = 0m;
                        decimal PrecioD = 0m;
                        Cls_Precio PPublico = qPrecios.FirstOrDefault(f => f.Tipo == "Publico");
                        Cls_Precio PDist = qPrecios.FirstOrDefault(f => f.Tipo == "Distribuidor");

                        if (PPublico != null) PrecioP = PPublico.PrecioCordoba;
                        if (PDist != null) PrecioD = PDist.PrecioCordoba;

                        if (PrecioP > 0) PorcDescMargen = Math.Abs(Math.Round((((PrecioD / PrecioP) * 100m) - 100m), 2, MidpointRounding.ToEven));



                        Cls_Descuento Margen = new Cls_Descuento();
                        Margen.Index = 0;
                        Margen.Descripcion = "MARGEN";
                        Margen.PorcDescuento = PorcDescMargen;

                        C_Descuento.Add(Margen);
                    }



                    datos = new Cls_Datos();
                    datos.Nombre = "DESCUENTO";
                    datos.d = C_Descuento;
                    lstDatos.Add(datos);




                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        [Route("api/Factura/BonificacionLibre")]
        [HttpGet]
        public string Direcciones(string CodCliente, string CodBodega)
        {
            return v_BonificacionLibre(CodCliente, CodBodega);
        }


        private string v_BonificacionLibre(string CodCliente, string CodBodega)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    Cliente cl = _Conexion.Cliente.FirstOrDefault(f => f.Codigo == CodCliente);


                    var qBonificacionLibre = (from _q in _Conexion.LiberarBonificacion
                                              join _p in _Conexion.Productos on _q.IdProducto equals _p.IdProducto
                                              join _b in _Conexion.Bodegas on _q.IdBodega equals _b.IdBodega
                                              where _q.Activo && _q.CantMax >= 1 && _b.Codigo == CodBodega && (((_q.IdCliente == 0 ? cl.IdCliente : _q.IdCliente) == cl.IdCliente))
                                              orderby _p.Codigo
                                              select new
                                              {
                                                  _p.Codigo,
                                                  Producto = _p.Producto.TrimStart().TrimEnd(),
                                                  CantidadMax = (_q.IdCliente == 0 && _q.CantMax == 0 ? _q.CantMax : (_q.CantMax == 1 ? 9999 : _q.CantMax)),
                                                  Filtro = string.Concat(_p.Codigo, _p.Producto.TrimStart().TrimEnd()),
                                                  IdLiberacionBonif = _q.IdLiberarBonificacion,
                                                  Seleccionar = false

                                              }).ToList();



                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "BONIFICACION LIBRE";
                    datos.d = qBonificacionLibre;
 


                    json = Cls_Mensaje.Tojson(datos, 1, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }




        [Route("api/Factura/Direcciones")]
        [HttpGet]
        public string Direcciones(string CodCliente)
        {
            return v_Direcciones(CodCliente);
        }


        private string v_Direcciones(string CodCliente)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    var qDireccion = (from _q in _Conexion.DireccionCliente
                                      join _d in _Conexion.Departamento on _q.IdDepartamento equals _d.IdDepartamento
                                      join _m in _Conexion.Municipio on _q.IdMunicipio equals _m.IdMunicipio into _union
                                      from muni in _union.DefaultIfEmpty()
                                      where _q.Cliente.Codigo == CodCliente
                                      select new
                                      {
                                          Departamento = _d.Departamento1.TrimStart().TrimEnd(),
                                          Municipio = (muni == null ? string.Empty : muni.Municipio1),
                                          Direccion = _q.Direccion.TrimStart().TrimEnd(),
                                          Descripcion = _q.Descripcion,
                                          Filtro = string.Concat(_q.Descripcion.TrimStart().TrimEnd(), _q.Direccion.TrimStart().TrimEnd())
                                      }).ToList();


                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "DIRECCIONES";
                    datos.d = qDireccion;
                lstDatos.Add(datos);







                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }





        [Route("api/Factura/ProductoLiberadosInvEscasan")]
        [HttpGet]
        public string ProductoLiberadosInvEscasan(string CodCliente, string CodBodega)
        {
            return v_ProductoLiberadosInvEscasan(CodCliente, CodBodega);
        }


        private string v_ProductoLiberadosInvEscasan(string CodCliente, string CodBodega)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
           

                   List<Cls_Productos_Liberados_INVESCASAN> ProductosLiberados = _Conexion.Database.SqlQuery<Cls_Productos_Liberados_INVESCASAN>($"SELECT -T.id AS ID, RTRIM(LTRIM(T.Codigo)) AS Codigo, T.Precio, T.Cantidad, ISNULL(T.Bonificables, 0) AS Bonificado \r\nFROM INVESCASAN.DBO.AutorizadosPrecios AS T\r\nWHERE T.Bodega = '{CodBodega}' AND RTRIM(LTRIM(T.CodCliente))  = '{CodCliente}' AND T.Cancelado = 0").ToList();



                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "PRODUCTOS LIBERADOS INVESCASAN";
                    datos.d = ProductosLiberados;
 






                    json = Cls_Mensaje.Tojson(datos, ProductosLiberados.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }

        [Route("api/Factura/Get")]
        [HttpGet]
        public string Get(DateTime Fecha1, DateTime Fecha2, string Tipo, bool esCola)
        {
            return v_Get(Fecha1, Fecha2, Tipo, esCola);
        }


        private string v_Get(DateTime Fecha1, DateTime Fecha2, string Tipo, bool esCola)
        {
            string json = string.Empty;

            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    if(Tipo == "Factura")
                    {
                        if(esCola)
                        {
                            var qDoc = (from _q in _Conexion.Venta
                                        where  _q.TipoDocumento == Tipo && (_q.Estado == "Solicitado" || (_q.Estado == "Autorizado" && _q.NoFactura == string.Empty))
                                        orderby _q.CodBodega descending, _q.Fecha descending
                                        select new
                                        {
                                            _q.IdVenta,
                                            _q.TipoDocumento,
                                            _q.Serie,
                                            _q.NoFactura,
                                            _q.NoPedido,
                                            _q.CodCliente,
                                            _q.NomCliente,
                                            _q.Nombre,
                                            _q.RucCedula,
                                            _q.Contacto,
                                            _q.Limite,
                                            _q.Disponible,
                                            _q.CodBodega,
                                            _q.NomBodega,
                                            _q.CodVendedor,
                                            _q.NomVendedor,
                                            _q.EsContraentrega,
                                            _q.EsExportacion,
                                            _q.OrdenCompra,
                                            _q.Fecha,
                                            _q.Plazo,
                                            _q.Vence,
                                            _q.Moneda,
                                            _q.TipoVenta,
                                            _q.TipoImpuesto,
                                            _q.TipoExoneracion,
                                            _q.NoExoneracion,
                                            _q.EsDelivery,
                                            _q.Direccion,
                                            _q.Observaciones,
                                            _q.Impuesto,
                                            _q.Exonerado,
                                            _q.TotalCordoba,
                                            _q.TotalDolar,
                                            _q.TasaCambio,
                                            _q.PedirAutorizacion,
                                            _q.Estado,
                                            Filtro = string.Concat(_q.NoFactura, _q.NoPedido, _q.CodCliente, _q.NomCliente, _q.Nombre, _q.CodBodega, _q.NomBodega, _q.CodVendedor, _q.NomVendedor, _q.TipoVenta, _q.Estado, (_q.Estado == "Anulado" ? _q.UsuarioAnula : _q.UsuarioRegistra))
                                        }).ToList();


                            Cls_Datos datos = new Cls_Datos();
                            datos.Nombre = "DOCUMENTOS";
                            datos.d = qDoc;
                            lstDatos.Add(datos);
                        }
                        else
                        {
                            var qDoc = (from _q in _Conexion.Venta
                                        where _q.Fecha >= Fecha1 && _q.Fecha <= Fecha2 && _q.TipoDocumento == Tipo && _q.NoFactura != string.Empty
                                        orderby _q.FechaRegistro descending
                                        select new
                                        {
                                            _q.IdVenta,
                                            _q.TipoDocumento,
                                            _q.Serie,
                                            _q.NoFactura,
                                            _q.NoPedido,
                                            _q.CodCliente,
                                            _q.NomCliente,
                                            _q.Nombre,
                                            _q.RucCedula,
                                            _q.Contacto,
                                            _q.Limite,
                                            _q.Disponible,
                                            _q.CodBodega,
                                            _q.NomBodega,
                                            _q.CodVendedor,
                                            _q.NomVendedor,
                                            _q.EsContraentrega,
                                            _q.EsExportacion,
                                            _q.OrdenCompra,
                                            _q.Fecha,
                                            _q.Plazo,
                                            _q.Vence,
                                            _q.Moneda,
                                            _q.TipoVenta,
                                            _q.TipoImpuesto,
                                            _q.TipoExoneracion,
                                            _q.NoExoneracion,
                                            _q.EsDelivery,
                                            _q.Direccion,
                                            _q.Observaciones,
                                            _q.Impuesto,
                                            _q.Exonerado,
                                            _q.TotalCordoba,
                                            _q.TotalDolar,
                                            _q.TasaCambio,
                                            _q.PedirAutorizacion,
                                            _q.Estado,
                                            UsuarioRegistra = _q.Estado == "Anulado" ? _q.UsuarioAnula : _q.UsuarioRegistra,
                                            Filtro = string.Concat(_q.NoFactura, _q.NoPedido, _q.CodCliente, _q.NomCliente, _q.Nombre, _q.CodBodega, _q.NomBodega, _q.CodVendedor, _q.NomVendedor, _q.TipoVenta, _q.Estado, (_q.Estado == "Anulado" ? _q.UsuarioAnula : _q.UsuarioRegistra))
                                        }).ToList();


                            Cls_Datos datos = new Cls_Datos();
                            datos.Nombre = "DOCUMENTOS";
                            datos.d = qDoc;
                            lstDatos.Add(datos);
                        }
                        

                    }
                    else
                    {
                        var qDoc = (from _q in _Conexion.Venta
                                    where (_q.Estado == "Solicitado" || _q.Estado == "Autorizado") && _q.TipoDocumento == Tipo
                                    orderby  _q.FechaRegistro descending
                                    select new
                                    {
                                        _q.IdVenta,
                                        _q.ID,
                                        _q.TipoDocumento,
                                        _q.Serie,
                                        _q.NoFactura,
                                        _q.NoPedido,
                                        _q.CodCliente,
                                        _q.NomCliente,
                                        _q.Nombre,
                                        _q.RucCedula,
                                        _q.Contacto,
                                        _q.Limite,
                                        _q.Disponible,
                                        _q.CodBodega,
                                        _q.NomBodega,
                                        _q.CodVendedor,
                                        _q.NomVendedor,
                                        _q.EsContraentrega,
                                        _q.EsExportacion,
                                        _q.OrdenCompra,
                                        _q.Fecha,
                                        _q.Plazo,
                                        _q.Vence,
                                        _q.Moneda,
                                        _q.TipoVenta,
                                        _q.TipoImpuesto,
                                        _q.TipoExoneracion,
                                        _q.NoExoneracion,
                                        _q.EsDelivery,
                                        _q.Direccion,
                                        _q.Observaciones,
                                        _q.Impuesto,
                                        _q.Exonerado,
                                        _q.TotalCordoba,
                                        _q.TotalDolar,
                                        _q.TasaCambio,
                                        _q.PedirAutorizacion,
                                        _q.Estado,
                                        UsuarioRegistra = _q.Estado == "Anulado" ? _q.UsuarioAnula : _q.UsuarioRegistra,
                                        Filtro = string.Concat(_q.NoFactura, _q.NoPedido, _q.CodCliente, _q.NomCliente, _q.Nombre, _q.CodBodega, _q.NomBodega, _q.CodVendedor, _q.NomVendedor, _q.TipoVenta, _q.Estado, (_q.Estado == "Anulado" ? _q.UsuarioAnula : _q.UsuarioRegistra))
                                    }).ToList();


                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "DOCUMENTOS";
                        datos.d = qDoc;
                        lstDatos.Add(datos);

                    }







                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



        [Route("api/Factura/Guardar")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Guardar(Venta d)
        {
        
            if (ModelState.IsValid)
            {

                return Ok(v_Guardar(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string v_Guardar(Venta d)
        {

            string json = string.Empty;

            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {

                       
                        bool esNuevo = false;
                        bool MandarCorreo = false;
                        string ProductosMail = string.Empty;
          
                        Venta _v = _Conexion.Venta.Find(d.IdVenta);
                        

                        if (_v == null)
                        {
                            int Consecutivo = 1;

                            if (d.TipoDocumento == "Proforma")
                            {

                   


                                _Conexion.Database.ExecuteSqlCommand($"UPDATE SIS.Serie SET Proforma += 1    WHERE  IdSerie = '{d.Serie}'");
                                _Conexion.SaveChanges();


                                Consecutivo = _Conexion.Database.SqlQuery<int>($"SELECT Proforma  FROM SIS.Serie WHERE IdSerie = '{d.Serie}'").First();

                                Venta vta = _Conexion.Venta.FirstOrDefault(f => f.NoPedido == string.Concat(d.Serie, Consecutivo));

                                if (vta != null)
                                {
                                    json = Cls_Mensaje.Tojson(null, 0, "1", "<b>El pedido genera duplicado.</>", 1);
                                    return json;

                                }


                            }
                     




                            _v = new Venta();
                            d.IdVenta = Guid.NewGuid();
                            d.NoFactura = string.Empty;
                            d.NoPedido = string.Empty;
                            d.Estado = "Solicitado";
                            _v.NoPedido = d.NoPedido;
                            _v.NoFactura = d.NoFactura;
                            _v.TipoDocumento = d.TipoDocumento;
                            _v.Estado = d.Estado;
                            if (d.TipoDocumento == "Factura") d.Estado = string.Empty;
                            d.MotivoAnulacion = string.Empty;
                            _v.FechaRegistro = DateTime.Now;
                            _v.UsuarioRegistra = d.UsuarioRegistra;
                            if (d.TipoDocumento == "Factura") d.NoFactura = string.Empty;
                            if (d.TipoDocumento == "Proforma") d.NoPedido = string.Concat(d.Serie, Consecutivo);
                            esNuevo = true;
                        }

                        if( _v.NoFactura != string.Empty || _v.Estado != "Solicitado")
                        {
                            json = Cls_Mensaje.Tojson(null, 0, "1", "<b>No se permite modificacion del documento.</>", 1);
                            return json;

                        }

                        if(esNuevo && d.TipoDocumento == "Proforma")
                        {
                            if(d.PedirAutorizacion)
                            {
                                MandarCorreo = true;
                            }
                        }
                        else
                        {
                            if (_v.PedirAutorizacion && !d.PedirAutorizacion && _v.Estado == "Solicitado")
                            {
                                MandarCorreo = true;
                            }
                        }

                        _v.IdVenta = d.IdVenta;
                        _v.Serie = d.Serie;
                        _v.NoFactura = d.NoFactura;
                        _v.NoPedido = d.NoPedido;
                        _v.TipoDocumento = d.TipoDocumento;
                        _v.CodCliente = d.CodCliente;
                        _v.NomCliente = d.NomCliente;
                        _v.Nombre = d.Nombre;
                        _v.RucCedula = d.RucCedula;
                        _v.Contacto = d.Contacto;
                        _v.Limite = d.Limite;
                        _v.Disponible = d.Disponible;
                        _v.CodBodega = d.CodBodega;
                        _v.NomBodega = d.NomBodega;
                        _v.CodVendedor = d.CodVendedor;
                        _v.NomVendedor = d.NomVendedor;
                        _v.EsContraentrega = d.EsContraentrega;
                        _v.EsExportacion = d.EsExportacion;
                        _v.OrdenCompra = d.OrdenCompra;
                        _v.Fecha = d.Fecha;
                        _v.Plazo = d.Plazo;
                        _v.Vence = d.Vence;
                        _v.Moneda = d.Moneda;
                        _v.TipoVenta = d.TipoVenta;
                        _v.TipoImpuesto = d.TipoImpuesto;
                        _v.TipoExoneracion = d.TipoExoneracion;
                        _v.NoExoneracion = d.NoExoneracion;
                        _v.EsDelivery = d.EsDelivery;
                        _v.Direccion = d.Direccion;
                        _v.Observaciones = d.Observaciones;
                        _v.Impuesto = d.Impuesto;
                        _v.Exonerado = d.Exonerado;
                        _v.TotalCordoba = d.TotalCordoba;
                        _v.TotalDolar = d.TotalDolar;
                        _v.TasaCambio = d.TasaCambio;
                        _v.PedirAutorizacion = d.PedirAutorizacion;
                        _v.MotivoAnulacion = d.MotivoAnulacion;
                        _v.UsuarioModifica = d.UsuarioRegistra;
                        if (!_v.PedirAutorizacion && _v.Estado == "Solicitado") _v.Estado = "Autorizado";
                        _v.FechaModifica = DateTime.Now;


                        if (esNuevo)
                        {
                            _Conexion.Venta.Add(_v);
                        }
                        else
                        {
                            _Conexion.VentaDetalle.RemoveRange(_v.VentaDetalle);
                            _Conexion.SaveChanges();

                            _Conexion.Database.ExecuteSqlCommand($"DELETE FROM DBO.Kardex WHERE Documento = 'FAC_{_v.ID}' AND Tipo ='F01'");

                            _Conexion.SaveChanges();
                        }

                        _Conexion.SaveChanges();

                      
                        foreach (VentaDetalle det in d.VentaDetalle)
                        {
                            VentaDetalle _vDet = new VentaDetalle();
                            _vDet.IdVentaDetalle = Guid.NewGuid();
                            _vDet.IdVenta = _v.IdVenta;
                            _vDet.Index = det.Index;
                            _vDet.Codigo = det.Codigo;
                            _vDet.Producto = det.Producto;
                            _vDet.Precio = det.Precio;
                            _vDet.PrecioCordoba = det.PrecioCordoba;
                            _vDet.PrecioDolar = det.PrecioDolar;
                            _vDet.PorcDescuento = det.PorcDescuento;
                            _vDet.PorcDescuentoAdicional = det.PorcDescuentoAdicional;
                            _vDet.PorcImpuesto = det.PorcImpuesto;
                            _vDet.Cantidad = det.Cantidad;
                            _vDet.SubTotal = det.SubTotal;
                            _vDet.SubTotalCordoba = det.SubTotalCordoba;
                            _vDet.SubTotalDolar = det.SubTotalDolar;
                            _vDet.Descuento = det.Descuento;
                            _vDet.DescuentoCordoba = det.DescuentoCordoba;
                            _vDet.DescuentoDolar = det.DescuentoDolar;
                            _vDet.DescuentoAdicional = det.DescuentoAdicional;
                            _vDet.DescuentoAdicionalCordoba = det.DescuentoAdicionalCordoba;
                            _vDet.DescuentoAdicionalDolar = det.DescuentoAdicionalDolar;
                            _vDet.SubTotalNeto = det.SubTotalNeto;
                            _vDet.SubTotalNetoCordoba = det.SubTotalNetoCordoba;
                            _vDet.SubTotalNetoDolar = det.SubTotalNetoDolar;
                            _vDet.Impuesto = det.Impuesto;
                            _vDet.ImpuestoCordoba = det.ImpuestoCordoba;
                            _vDet.ImpuestoDolar = det.ImpuestoDolar;
                            _vDet.ImpuestoExo = det.ImpuestoExo;
                            _vDet.ImpuestoExoCordoba = det.ImpuestoExoCordoba;
                            _vDet.ImpuestoExoDolar = det.ImpuestoExoDolar;
                            _vDet.Total = det.Total;
                            _vDet.TotalCordoba = det.TotalCordoba;
                            _vDet.TotalDolar = det.TotalDolar;
                            _vDet.EsBonif = det.EsBonif;
                            _vDet.EsBonifLibre = det.EsBonifLibre;
                            _vDet.EsLibInvEscasan = det.EsLibInvEscasan;
                            _vDet.EsExonerado = det.EsExonerado;
                            _vDet.EsExento = det.EsExento;
                            _vDet.PrecioLiberado = det.PrecioLiberado;
                            _vDet.Lotificado = det.Lotificado;
                            _vDet.Margen = det.Margen;
                            _vDet.PedirAutorizado = det.PedirAutorizado;
                            _vDet.Autorizado = det.Autorizado;
                            _vDet.UsuarioAutoriza = det.UsuarioAutoriza;
                            _vDet.IndexUnion = det.IndexUnion;
                            _vDet.IdPrecioFAC = det.IdPrecioFAC;
                            _vDet.IdEscala = det.IdEscala;
                            _vDet.IdDescuentoDet = det.IdDescuentoDet;
                            _vDet.IdLiberacion = det.IdLiberacion;
                            _vDet.IdLiberacionBonif = det.IdLiberacionBonif;
                            _vDet.FacturaNegativo = det.FacturaNegativo;
                            _vDet.Lotificado = det.Lotificado;


                            _v.VentaDetalle.Add(_vDet);

                            if (MandarCorreo)
                            {
                                if (_vDet.Autorizado || (_vDet.PedirAutorizado || _vDet.PrecioLiberado))
                                {

                                    ProductosMail += $"<br><p>Producto: <b>{_vDet.Codigo} {_vDet.Producto}</b>";
                                    ProductosMail += $"<br>Facturado:  <b>{string.Format("{0:###,###,###.00}", _vDet.Cantidad)}</b>";


                                    if (_vDet.PrecioLiberado && !_vDet.PedirAutorizado)
                                    {

                        
                                        var lstPrecio = (from _q in _Conexion.LiberarPrecio
                                                         join _p in _Conexion.Productos on _q.IdProducto equals _p.IdProducto
                                                         join _c in _Conexion.ConceptoPrecio on _q.IdConceptoPrecio equals _c.IdConceptoPrecio
                                                         where _p.Codigo == _vDet.Codigo
                                                         select new
                                                                     {
                                                                         _p.Codigo,
                                                                         _c.Descripcion,
                                                                         _q.Precio
                                                                     }).ToList();

                                        var  iPrecio = lstPrecio.FirstOrDefault(f => f.Descripcion == "Distribuidor");

                                        ProductosMail += $"<br>Precio Liberado: <b>{string.Format("{0:###,###,###.00}", _vDet.Precio)}</b>";



                                        if (iPrecio == null)
                                        {
                                            iPrecio = lstPrecio.FirstOrDefault(f => f.Descripcion == "Publico");

                                            ProductosMail += $"<br>Precio Minimo: <b>{string.Format("{0:###,###,###.00}", iPrecio.Precio)}</b>";
                                            ProductosMail += $"<br>Precio Público: <b>0.00</b>";
                                            ProductosMail += $"<br>Precio Minimo: <b>0.00</b>";
                                        }
                                        else
                                        {
                                            var iPrecioPublico = lstPrecio.FirstOrDefault(f => f.Descripcion == "Publico");
                                            var iPrecioDistribuid = lstPrecio.FirstOrDefault(f => f.Descripcion == "Distribuidor");

                                            if (_vDet.PrecioCordoba > iPrecio.Precio)
                                            {
                                
                            
                                                if (iPrecioPublico != null)
                                                {
                                                    if(iPrecioPublico.Precio != 0)
                                                    {
                                                        ProductosMail += $"<br>Precio Publico: <b>{string.Format("{0:###,###,###.00}", iPrecioPublico.Precio)}</b>";
                                                        ProductosMail += $"<br>Precio Distribuidor: <b>{string.Format("{0:###,###,###.00}", iPrecioDistribuid.Precio)}</b>";
                                                    }
                                                    else
                                                    {
                                                        ProductosMail +=$"<br>Precio Publico: <b>{string.Format("{0:###,###,###.00}", iPrecioPublico.Precio)}</b>";
                                                        ProductosMail += $"<br>Precio Distribuidor: <b>{string.Format("{0:###,###,###.00}", iPrecio.Precio)}</b>";
                                                    }
                                                   

                                                }
                                                else
                                                {
                                                    ProductosMail += $"<br>Precio Publico: <b>{string.Format("{0:###,###,###.00}", iPrecioPublico.Precio)}</b>";
                                                    ProductosMail +=$"<br>Precio Distribuidor: <b>{string.Format("{0:###,###,###.00}", iPrecio.Precio)}</b>";
                                                }


                                               
                                            }
                                            else
                                            {
                                                ProductosMail += $"<br>Precio Publico: <b>{string.Format("{0:###,###,###.00}", iPrecioPublico.Precio)}</b>";
                                                ProductosMail +=$"<br>Precio Distribuidor: <b>{string.Format("{0:###,###,###.00}", iPrecio.Precio)}</b>";
                                            }
                                        }

                                       
                                       
                                    }
                                    else
                                    {
                                        VentaDetalle _vDetBonificado = d.VentaDetalle.FirstOrDefault(f => f.IndexUnion == _vDet.Index && f.EsBonif);

                                        if(_vDetBonificado != null) ProductosMail+=$"<br>Bonificacion:  <b>{string.Format("{0:###,###,###.00}", _vDetBonificado.Cantidad)}</b>";
                                        ProductosMail +=$"<br>Margen: <b>{string.Format("{0:###,###,###.00}", _vDet.Margen)}<b>";

                                    }


                                    ProductosMail += "</p>";

                                }

                            }






                       
                            _Conexion.SaveChanges();
                        }


                        _Conexion.SaveChanges();


                        
                        if (MandarCorreo && _v.TipoDocumento == "Proforma")
                        {

                            PermisoFactura per = _Conexion.PermisoFactura.FirstOrDefault(f => f.Usuario == d.UsuarioRegistra && f.AutorizaPedido);

                            if(per == null)
                            {
                                json = Cls_Mensaje.Tojson(null, 0, "1", "<b>No tiene permiso para autorizar.</>", 1);
                                return json;
                            }


                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("info@escasan.com.ni");

                           foreach(CorreoAutorizaFactura c in   _Conexion.CorreoAutorizaFactura.Where(w => (esNuevo ? w.AntAutorizar : w.DespAutorizar) ).ToList())
                            {
                                mail.To.Add(c.Correo);

                            }


                            //SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                            //NetworkCredential nameAndPassword = new NetworkCredential("info@escasan.com.ni", "Inf0Escasan2018!.");

                            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                            NetworkCredential nameAndPassword = new NetworkCredential("notificaciones@globalvet.com.ni", "Mam02771");


                            mail.Subject = esNuevo ?  $"Pendiente Autorizar ({_v.NoPedido})" : "Margen Autorizado";
                            mail.Body = $"<p>Pedido No. <b>{_v.NoPedido}</b>";
                            mail.Body += $"<br>CLIENTE: <b>{_v.CodCliente} { (_v.Nombre.TrimStart().TrimEnd() == string.Empty ? _v.NomCliente : _v.NomCliente) }</b>";
                            mail.Body += $"<br>Usuario Autoriza: <b>{d.UsuarioRegistra}</b>";
                            mail.Body += "<br><br><b>PRODUCTOS AUTORIZADOS</b>";
                            mail.Body += ProductosMail;
                            mail.IsBodyHtml = true;
                            mail.BodyEncoding = System.Text.Encoding.UTF8;
   

                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            smtpClient.Port = 587;
                            smtpClient.Credentials = nameAndPassword;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.EnableSsl = true;
                            smtpClient.Send(mail);

                        }




                        List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "DOCUMENTO";
                        datos.d =  _v.TipoDocumento == "Factura" ? _v.NoFactura : _v.NoPedido;
                        lstDatos.Add(datos);


                      

                        scope.Complete();



                        json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);

                    }
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }




        [Route("api/Factura/Anular")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Anular(Guid IdDoc, string Motivo, string Usuario)
        {
            if (ModelState.IsValid)
            {

                return Ok(v_Anular(IdDoc, Motivo, Usuario));

            }
            else
            {
                return BadRequest();
            }

        }

        private string v_Anular(Guid IdDoc, string Motivo, string Usuario)
        {

            string json = string.Empty;

            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {


                
                        Venta _v = _Conexion.Venta.Find(IdDoc);

                        if (_v != null)
                        {
                            if(_v.Estado == "Anulado")
                            {
                                json = Cls_Mensaje.Tojson(null, 0, "1", "<b class='error'>El documento se encuentra anulado.</b>", 1);
                                return json;
                            }

                            if (_v.TipoDocumento == "Factura" && _v.Fecha.Date != DateTime.Now.Date)
                            {
                                json = Cls_Mensaje.Tojson(null, 0, "1", "<b class='error'>No se permite anular factura con dias anteriores al del servidor.</b>", 1);
                                return json;
                            }

                            _v.Estado = "Anulado";
                            _v.MotivoAnulacion = Motivo;
                            _v.UsuarioAnula = Usuario;


                     
                            _Conexion.SaveChanges();
                        }


       

                        List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "ANULAR";
                        datos.d = "<b>Registro Anulado<b/>";
                        lstDatos.Add(datos);

                        _Conexion.SaveChanges();

                      

                        scope.Complete();



                        json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);

                    }
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }


        [Route("api/Factura/Imprimir")]
        [HttpGet]
        public string Imprimir(Guid IdVenta)
        {
            return v_Imprimir(IdVenta);
        }

        private string v_Imprimir(Guid IdVenta)
        {
            string json = string.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {
                        List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                        Venta _v = _Conexion.Venta.Find(IdVenta);


                        json = AsignarConsecutivoFactura(_v, _Conexion);

                        if (json != string.Empty) return json;

                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = string.Concat("Factura No", _v.NoFactura);

                        MemoryStream stream = new MemoryStream();

                        /*
                        DsetReporte Dset = new DsetReporte();
                    
                        List<SP_FacturaImpresa_Result>   Query = (from _q in _Conexion.SP_FacturaImpresa(_v.IdVenta).AsEnumerable()
                                     select  _q).ToList();

                        DataTable tbl = Cls_ListToDataTableConverter.ToDataTable(Query);
                        tbl.Select().CopyToDataTable(Dset.SP_FacturaImpresa, LoadOption.PreserveChanges);


                        if (_v.TipoVenta == "Contado")
                        {
                            xrpFacturaContado xrpContado = new xrpFacturaContado();
                            xrpContado.Parameters["P_Letra"].Value = Cls_Letras.NumeroALetras(_v.TotalCordoba);
                            xrpContado.DataSource = Dset;
                            xrpContado.ShowPrintMarginsWarning = false;


                     


                            xrpContado.CreateDocument(false);
                            foreach(string Impresora in PrinterSettings.InstalledPrinters)
                            {
                                if(Impresora.Contains("SRP-275"))
                                {
                                    xrpContado.Print(Impresora);
                                    break;
                                }
                            }
      
                       


                            xrpContado.ExportToPdf(stream, null);
                            stream.Seek(0, SeekOrigin.Begin);

                            datos.d = stream.ToArray();

                        }
                        else
                        {
                            xrpFacturaCredito xrpCredito = new xrpFacturaCredito();
                            xrpCredito.Parameters["P_Letra"].Value = Cls_Letras.NumeroALetras(_v.TotalCordoba);
                            xrpCredito.DataSource = Dset;
                            xrpCredito.ShowPrintMarginsWarning = false;


                            foreach (string Impresora in PrinterSettings.InstalledPrinters)
                            {
                                if (Impresora.Contains("SRP-275"))
                                {
                                    xrpCredito.Print(Impresora);
                                    break;
                                }
                            }


                            xrpCredito.ExportToPdf(stream, null);
                            stream.Seek(0, SeekOrigin.Begin);

                            datos.d = stream.ToArray();
                        }


                        lstDatos.Add(datos);




                        datos = new Cls_Datos();
                        datos.Nombre = string.Concat( "Manifiesto No ", _v.NoFactura);
                       


                        MemoryStream stream2 = new MemoryStream();
                        xrpManifiesto xrpManifiesto = new xrpManifiesto();
                        xrpManifiesto.DataSource = Dset;
                        xrpManifiesto.ShowPrintMarginsWarning = false;

                        xrpManifiesto.CreateDocument(false);
                        foreach (string Impresora in PrinterSettings.InstalledPrinters)
                        {
                            if (Impresora.Contains("tm-u220"))
                            {
                                xrpManifiesto.Print(Impresora);
                                break;
                            }
                        }



                        xrpManifiesto.ExportToPdf(stream2, null);
                        stream2.Seek(0, SeekOrigin.Begin);

                        datos.d = stream2.ToArray();
                        lstDatos.Add(datos);




                        */

                        _Conexion.SaveChanges();

                        scope.Complete();


                        json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                    }


                }

                
            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }

        

        private string AsignarConsecutivoFactura(Venta _v , BalancesEntities _Conexion)
        {
            string json = string.Empty;

            try
            {
                if (_v.NoFactura == "" && _v.Estado != "Anulado")
                {


                    if (_v.PedirAutorizacion)
                    {



                        PermisoFactura per = _Conexion.PermisoFactura.FirstOrDefault(f => f.Usuario == _v.UsuarioRegistra && f.PaseFactura);

                        if (per == null)
                        {
                            json = Cls_Mensaje.Tojson(null, 0, "1", "<b class='error'>Se requiere autorización, por favor genere una proforma.</b>", 1);
                            return json;
                        }

                        foreach (VentaDetalle det in _v.VentaDetalle)
                        {
                            _v.PedirAutorizacion = false;
                        }

                        _v.PedirAutorizacion = false;
                        _Conexion.SaveChanges();

                    }




                    string Consecutivo = "";




                    Venta vta = _Conexion.Venta.FirstOrDefault(f => f.NoFactura == string.Concat(_v.Serie, Consecutivo));

                    if (vta != null)
                    {
                        json = Cls_Mensaje.Tojson(null, 0, "1", "<b>La factura genera duplicado.</>", 1);
                        return json;

                    }




                    _v.NoFactura = string.Concat(_v.Serie, Consecutivo);
                    _v.TipoDocumento = "Factura";
                    _v.Estado = "Facturada";
                    _v.Fecha = DateTime.Now;
                    _Conexion.SaveChanges();

 
                }


            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }




        [Route("api/Factura/GetDetalle")]
        [HttpGet]
        public string GetDetalle(Guid IdVenta, string User)
        {
            return v_GetDetalle(IdVenta, User);
        }


        private string v_GetDetalle(Guid IdVenta, string User)
        {
            string json = string.Empty;

            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    var qDetalle = (from _q in _Conexion.VentaDetalle
                                    where _q.IdVenta == IdVenta
                                    orderby _q.Index ascending
                                    select new
                                    {
                                        _q.IdVentaDetalle,
                                        _q.IdVenta,
                                        _q.Index,
                                        _q.Codigo,
                                        _q.Producto,
                                        _q.Precio,
                                        _q.PrecioCordoba,
                                        _q.PrecioDolar,
                                        _q.PorcDescuento,
                                        _q.PorcDescuentoAdicional,
                                        _q.PorcImpuesto,
                                        _q.Cantidad,
                                        _q.SubTotal,
                                        _q.SubTotalCordoba,
                                        _q.SubTotalDolar,
                                        _q.Descuento,
                                        _q.DescuentoCordoba,
                                        _q.DescuentoDolar,
                                        _q.DescuentoAdicional,
                                        _q.DescuentoAdicionalCordoba,
                                        _q.DescuentoAdicionalDolar,
                                        _q.SubTotalNeto,
                                        _q.SubTotalNetoCordoba,
                                        _q.SubTotalNetoDolar,
                                        _q.Impuesto,
                                        _q.ImpuestoCordoba,
                                        _q.ImpuestoDolar,
                                        _q.ImpuestoExo,
                                        _q.ImpuestoExoCordoba,
                                        _q.ImpuestoExoDolar,
                                        _q.Total,
                                        _q.TotalCordoba,
                                        _q.TotalDolar,
                                        _q.EsBonif,
                                        _q.EsBonifLibre,
                                        _q.EsExonerado,
                                        _q.EsExento,
                                        _q.PrecioLiberado,
                                        _q.Margen,
                                        _q.PedirAutorizado,
                                        _q.Autorizado,
                                        _q.UsuarioAutoriza,
                                        _q.IndexUnion,
                                        _q.IdPrecioFAC,
                                        _q.IdEscala,
                                        _q.IdDescuentoDet,
                                        _q.IdLiberacion,
                                        _q.IdLiberacionBonif
                                        ,_q.FacturaNegativo
                                        ,_q.Lotificado
                                    }).ToList();


                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "DETALLE VENTA";
                    datos.d = qDetalle;
                    lstDatos.Add(datos);

                    PermisoFactura per = _Conexion.PermisoFactura.FirstOrDefault(f => f.Usuario == User && f.AutorizaPedido);

                    datos = new Cls_Datos();
                    datos.Nombre = "PERMISO MARGEN";
                    datos.d = string.Empty;
                    if (per == null) datos.d = "<b>No tiene permiso para autorizar.</>";

                    lstDatos.Add(datos);

                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }






        private bool EsINVESCASAN(string CodProducto, string CodBodega, BalancesEntities _Conexion)
        {

            Productos Prod = _Conexion.Productos.FirstOrDefault(w => w.Codigo == CodProducto);

            if (Prod == null) return true;

            BodegaLaboratorioKardex b = null;

            if (CodBodega == "COSTO")
            {
                b = _Conexion.BodegaLaboratorioKardex.FirstOrDefault(w => w.CodProveedor == Prod.CodProveedorEscasan);

                if (b != null) return false;
                return true;
            }

            b = _Conexion.BodegaLaboratorioKardex.FirstOrDefault(w => w.CodBodega == CodBodega && w.CodProveedor == "ALL");


            if (b == null)
            {
                b = _Conexion.BodegaLaboratorioKardex.FirstOrDefault(w => w.CodBodega == CodBodega && w.CodProveedor == Prod.CodProveedorEscasan);
                if (b != null) return false;
            }
            else
            {
                return false;
            }


            return true;
        }


    }
}