﻿using GV_api.Class;
using GV_api.Class.FACT;
using GV_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;


namespace GV_api.Controllers.FACT
{
    public class FacturaController : ApiController
    {

    
        private decimal f_TasaCambio()
        {
            decimal tc = 36.9566m;
            return tc;
        }

        [Route("api/Factura/Datos")]
        [HttpGet]
        public string Datos()
        {
            return v_Datos();
        }


        private string v_Datos()
        {
            string json = string.Empty;
            try
            {
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();

          
                    var qClientes = (from _q in _Conexion.Clientes
                                 select new
                                 {
                                     Codigo = _q.CODCTA.TrimStart().TrimEnd(),
                                     Cliente = _q.DESCTA.TrimStart().TrimEnd(),
                                     Ruc = _q.RazonSocial.TrimStart().TrimEnd(),
                                     Cedula = _q.Cedula.TrimStart().TrimEnd(),
                                     Contacto = string.Concat(_q.Celular.TrimStart().TrimEnd(),"/",_q.TELEFO.TrimStart().TrimEnd(), "/", _q.TELEFONO1.TrimStart().TrimEnd()),
                                     Limite = _q.Techo,
                                     Moneda = _q.Moneda.TrimStart().TrimEnd(),
                                     CodVendedor = _q.Vendedor.TrimStart().TrimEnd(),
                                     EsClave = _q.Clave,
                                     Filtro = string.Concat(_q.CODCTA.TrimStart().TrimEnd(), _q.DESCTA.TrimStart().TrimEnd(), _q.Cedula.TrimStart().TrimEnd(), _q.TELEFO.TrimStart().TrimEnd(), _q.TELEFONO1.TrimStart().TrimEnd(), _q.Celular.TrimStart().TrimEnd()),
                                     Key = string.Concat(_q.CODCTA.TrimStart().TrimEnd(), " ", _q.DESCTA.TrimStart().TrimEnd())
                                 }).ToList();

                    datos.Nombre = "CLIENTES";
                    datos.d = qClientes;
                    lstDatos.Add(datos);



                    var qBodegas = (from _q in _Conexion.TbBodega
                             select new
                             {
                                 Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                 Bodega = _q.titulo.TrimStart().TrimEnd(),
                                 Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.titulo.TrimStart().TrimEnd())
                             }).ToList();

                    datos = new Cls_Datos();
                    datos.Nombre = "BODEGAS";
                    datos.d = qBodegas;
                    lstDatos.Add(datos);


                    var qVendedores = (from _q in _Conexion.tbVendedores
                                    select new
                                    {
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Vendedor = _q.Titulo.TrimStart().TrimEnd(),
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Titulo.TrimStart().TrimEnd())
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
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();


                    var qCredito = (from _q in _Conexion.Clientes
                                     where _q.CODCTA.TrimStart().TrimEnd() == CodCliente
                                     select new
                                     {
                                         CodCliente = _q.CODCTA.TrimStart().TrimEnd(),
                                         Limite = _q.Techo,
                                         Plazo = _q.Plazo,
                                         Gracia = _q.Gracia,
                                         Moneda = _q.Moneda.TrimStart().TrimEnd(),
                                         Disponible = 0.0
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
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();

                    var qClienteClave = (from _q in _Conexion.Clientes
                                         join _v in _Conexion.tbVendedores on _q.Vendedor equals _v.Codigo
                                         where _q.CODCTA.TrimStart().TrimEnd() == CodCliente
                                         select new
                                         {
                                             CodVendedor = _v.Codigo.TrimStart().TrimEnd(),
                                             Vendedor = string.Concat(_v.Codigo.TrimStart().TrimEnd(), " ", _v.Titulo.TrimStart().TrimEnd()),
                                             EsClave = _q.Clave
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
        public string CargarProductos()
        {
            return v_CargarProductos();
        }

        private string v_CargarProductos()
        {
            string json = string.Empty;
            
            try
            {
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Cls_Datos datos = new Cls_Datos();
             



                    var qProductos = (from _q in _Conexion.Catalogo
                                         where _q.FUERADELINEA == false
                                         select new
                                         {
                                             Codigo = _q.SSSCTA.TrimStart().TrimEnd(),
                                             Producto = _q.DESCTA.TrimStart().TrimEnd(),
                                             ConImpuesto = _q.COBIVA,
                                             Key = string.Concat(_q.SSSCTA.TrimStart().TrimEnd(), " ", _q.DESCTA.TrimStart().TrimEnd()),
                                         }).ToList();



                    datos.Nombre = "PRODUCTOS";
                    datos.d = qProductos;
                    lstDatos.Add(datos);


                    datos = new Cls_Datos();
                    datos.Nombre = "TASACAMBIO";
                    datos.d = f_TasaCambio();
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
        public string DatosProducto(string CodProducto, string CodBodega, string CodCliente)
        {
            return v_DatosProducto(CodProducto, CodBodega, CodCliente);
        }


        private string v_DatosProducto(string CodProducto, string CodBodega, string CodCliente)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Clientes cl = _Conexion.Clientes.FirstOrDefault(f => f.CODCTA == CodCliente);

                    Cls_Datos datos = new Cls_Datos();
                    int x = 0;
                    string strTipo = "Publico";
                    bool EsDolar = false;
                    decimal Tc = f_TasaCambio();

                    if (cl != null)
                    {
                        if (cl.Distribuidor) strTipo = "Distribuid";
                        if (cl.Especial) strTipo = "Especial";
                    }


                    var qExistencia = (from _q in _Conexion.Kardex
                                     where _q.CodiProd.TrimStart().TrimEnd() == CodProducto
                                       group _q by  _q.Bodega.TrimStart().TrimEnd() into g
                                       orderby g.Key
                                     select new
                                     { 
                                         CodProducto = CodProducto,
                                         Bodega = g.Key,
                                         Existencia = g.Sum( s=> s.Entrada - s.Salidas),
                                         EsPrincipal = (g.FirstOrDefault().Bodega == CodBodega ? true  : false)
                                     }).ToList();

                    datos.Nombre = "EXISTENCIA";
                    datos.d = qExistencia;
                    lstDatos.Add(datos);



                    List<Cls_Bonificacion> qBonificacion = (from _q in _Conexion.Bonificados
                                         where _q.Codigo.TrimStart().TrimEnd() == CodProducto
                                         orderby _q.Desde
                                         select new Cls_Bonificacion()
                                         {
                                             CodProducto = _q.Codigo.TrimStart().TrimEnd(),
                                             Escala = string.Concat(_q.Desde, "+", _q.Bonificacion),
                                             Desde = _q.Desde,
                                             Hasta = 0,
                                             Bonifica = _q.Bonificacion,
                                             Descripcion = string.Empty
                                         }).ToList();

                    foreach(var b in qBonificacion.OrderBy(o => o.Desde))
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



                    List<Cls_Precio> qPrecios = (from _q in _Conexion.Listadeprecios
                                                 where _q.CodiProd.TrimStart().TrimEnd() == CodProducto
                                                 orderby _q.Tipo
                                                 select new Cls_Precio()
                                                 {
                                                     CodProducto = _q.CodiProd.TrimStart().TrimEnd(),
                                                     Tipo = _q.Tipo.TrimStart().TrimEnd(),
                                                     PrecioCordoba = ((decimal)(!EsDolar ? _q.Precio : _q.Precio * Tc)),
                                                     PrecioDolar = ((decimal)(EsDolar ? _q.Precio : _q.Precio / Tc)),
                                                     EsPrincipal = (_q.Tipo.TrimStart().TrimEnd() == strTipo ? true : false)
                                                 }).ToList();



                    Cls_Precio iPrecio = qPrecios.FirstOrDefault(f => f.EsPrincipal);

                    if(qPrecios != null)
                    {
                        if (iPrecio.PrecioCordoba == 0 && iPrecio.Tipo == "Especial" && cl.Distribuidor)
                        {
                            iPrecio.EsPrincipal = false;
                            iPrecio = qPrecios.FirstOrDefault(f => f.Tipo == "Distribuid");
                            if (iPrecio != null) iPrecio.EsPrincipal = true;
                        }

                        if (iPrecio.PrecioCordoba == 0)
                        {
                            iPrecio.EsPrincipal = false;
                            iPrecio = qPrecios.FirstOrDefault(f => f.Tipo == "Publico");
                            if (iPrecio != null) iPrecio.EsPrincipal = true;
                        }

                    }

                    



                    datos = new Cls_Datos();
                    datos.Nombre = "PRECIOS";
                    datos.d = qPrecios;
                    lstDatos.Add(datos);



                    List<Cls_Descuento> qDescuentoProd = (from _q in _Conexion.Catalogo
                                          where _q.SSSCTA.TrimStart().TrimEnd() == CodProducto
                                          select new Cls_Descuento()
                                          {
                                              Index = 0,
                                              Descripcion = "GENERAL",
                                              PorcDescuento = _q.Descuento == null ? 0m : (decimal)_q.Descuento
                                          }).Union(

                        (from _q in _Conexion.Clientes
                         where _q.CODCTA.TrimStart().TrimEnd() == CodCliente
                         select new Cls_Descuento()
                        {
                             Index = 1,
                            Descripcion = "ADICIONAL",
                            PorcDescuento = _q.Descuento
                        })
                        ).ToList();



                    if(qDescuentoProd.FindIndex(f => f.Descripcion == "GENERAL" && f.PorcDescuento == 0m) != -1)
                    {

                        decimal PorcDescMargen = 0;
                        decimal PrecioP = 0m;
                        decimal PrecioD = 0m;
                        Cls_Precio PPublico = qPrecios.FirstOrDefault(f => f.Tipo == "Publico");
                        Cls_Precio PDist = qPrecios.FirstOrDefault(f => f.Tipo == "Distribuid");

                        if (PPublico != null) PrecioP = PPublico.PrecioCordoba;
                        if (PDist != null) PrecioD = PDist.PrecioCordoba;

                        if (PrecioP > 0) PorcDescMargen = Math.Abs(Math.Round((((PrecioD / PrecioP) * 100m) - 100m), 2, MidpointRounding.ToEven));



                        Cls_Descuento Margen = new Cls_Descuento();
                        Margen.Index = 0;
                        Margen.Descripcion = "MARGEN";
                        Margen.PorcDescuento = PorcDescMargen;

                        qDescuentoProd.Add(Margen);
                    }



                    datos = new Cls_Datos();
                    datos.Nombre = "DESCUENTO";
                    datos.d = qDescuentoProd;
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



    }
}