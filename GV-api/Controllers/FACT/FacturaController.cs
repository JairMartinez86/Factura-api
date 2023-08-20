using GV_api.Class;
using GV_api.Class.FACT;
using GV_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using IsolationLevel = System.Transactions.IsolationLevel;
using RouteAttribute = System.Web.Http.RouteAttribute;


namespace GV_api.Controllers.FACT
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
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "FECHA FACTURA";
                    datos.d = DateTime.Now;
                    lstDatos.Add(datos);


                    datos = new Cls_Datos();
                    datos.Nombre = "TASA CAMBIO";
                    datos.d = f_TasaCambio();
                    lstDatos.Add(datos);

                    string NoDoc = string.Empty;
                    string Serie = string.Empty;

                    if(TipoFactura == "Factura")
                    {
                        ConfiguraFacturacion Consecutivo = _Conexion.ConfiguraFacturacion.FirstOrDefault(f => f.Bodegas.TrimStart().TrimEnd() == CodBodega && f.EmiteFactura.TrimStart().TrimEnd() == "S");

                        NoDoc = Consecutivo == null ? string.Empty : string.Concat(Consecutivo.Serie.TrimStart().TrimEnd(), Consecutivo.Secuencia);
                        Serie = Consecutivo == null ? string.Empty : Consecutivo.Serie.TrimStart().TrimEnd();
                    }
                    else
                    {
                        ControlInventario Consecutivo = _Conexion.ControlInventario.FirstOrDefault(f => f.Bodegas.TrimStart().TrimEnd() == CodBodega);

                        NoDoc = Consecutivo == null ? string.Empty : string.Concat(Consecutivo.Serie.TrimStart().TrimEnd(), Consecutivo.Secuencia);
                        Serie = Consecutivo == null ? string.Empty : Consecutivo.Serie.TrimStart().TrimEnd();
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

                                     select new Cls_Bodega()
                                     {
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Bodega = _q.titulo.TrimStart().TrimEnd(),
                                        ClienteContado = string.Empty,
                                        Vendedor = string.Empty,
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.titulo.TrimStart().TrimEnd())
                                    }).ToList();


                    foreach(Cls_Bodega b in qBodegas)
                    {
                        ConfiguraFacturacion c = _Conexion.ConfiguraFacturacion.FirstOrDefault(f => f.Bodegas.TrimStart().TrimEnd() == b.Codigo && f.EmiteFactura.TrimStart().TrimEnd() == "S");

                        b.Facturar = false;
                        if(c != null)
                        {
                            b.ClienteContado = c.ClienteContado.TrimStart().TrimEnd();
                            b.Vendedor = c.Vendedor.TrimStart().TrimEnd();
                            b.Facturar = true;
                        }

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

                    Clientes cl = _Conexion.Clientes.FirstOrDefault(f => f.CODCTA.TrimStart().TrimEnd() == CodCliente);
                    decimal saldoCor = 0m;
                    decimal saldoDol = 0m;
                    decimal Tc = f_TasaCambio();
                    decimal Techo = cl.Techo;
                    decimal Disponible = 0m;
                    decimal SaldoVencido = 0m;
                    ObjectParameter output = new ObjectParameter("dResultado", typeof(decimal));
         

                    var qSaldoCor = (from _q in _Conexion.spu_ObtenerSaldoCuenta(CodCliente, DateTime.Now.Date, "C")
                                 select new
                                 {
                                     Sald = _q,
                                 }).ToList();

                    var qSaldoDol = (from _q in _Conexion.spu_ObtenerSaldoCuenta(CodCliente, DateTime.Now.Date, "D")
                                     select new
                                     {
                                         Sald = _q,
                                     }).ToList();


                    _Conexion.RetornaSaldoVencidoAmbasMonedas(CodCliente, DateTime.Now.Date, 15, output);
                    SaldoVencido = (decimal)output.Value;



                    if (qSaldoCor != null)
                    {
                       if(qSaldoCor.First().Sald != null) saldoCor = qSaldoCor.First().Sald.Value;
                    }

                    if (qSaldoDol != null)
                    {
                        if (qSaldoDol.First().Sald != null) saldoDol = qSaldoDol.First().Sald.Value;
                    }


                    if(cl.Moneda == "C")
                    {
                        Disponible = (Techo - saldoCor) - Math.Round(saldoDol * Tc, 2, MidpointRounding.ToEven);
                    }
                    else
                    {
                        Disponible = (Techo - saldoDol) - Math.Round(saldoCor / Tc, 2, MidpointRounding.ToEven);
                    }
              


                    var qCredito = (from _q in _Conexion.Clientes
                                     where _q.CODCTA.TrimStart().TrimEnd() == CodCliente
                                     select new
                                     {
                                         CodCliente = _q.CODCTA.TrimStart().TrimEnd(),
                                         Limite = _q.Techo,
                                         Plazo = _q.Plazo,
                                         Gracia = _q.Gracia,
                                         Moneda = _q.Moneda.TrimStart().TrimEnd(),
                                         Disponible = Disponible,
                                         FacturarVencido = _q.FacturarVencido,
                                         SaldoVencido = SaldoVencido
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
                                             Bonificable = _q.Bonificable
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
                    bool LiberadoPrecio = false;

                    Catalogo ct = _Conexion.Catalogo.FirstOrDefault(f => f.SSSCTA.TrimStart().TrimEnd() == CodProducto);
                    if (ct != null) LiberadoPrecio = ct.LIBERAR == null ? false : (bool)ct.LIBERAR;

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
                                                     EsPrincipal = (_q.Tipo.TrimStart().TrimEnd() == strTipo ? true : false),
                                                     Liberado = false
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

                        if (iPrecio.EsPrincipal) iPrecio.Liberado = LiberadoPrecio;

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
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    var qDireccion = (from _q in _Conexion.Clientes
                                      join _d in _Conexion.TbDepartamento on _q.DPTO.TrimStart().TrimEnd() equals _d.CODCTA.TrimStart().TrimEnd()
                                      where _q.CODCTA.TrimStart().TrimEnd() == CodCliente
                                      select new
                                      {
                                          Departamento = _d.DESCTA.TrimStart().TrimEnd(),
                                          Municipio = string.Empty,
                                          Direccion = _q.DIRECC.TrimStart().TrimEnd(),
                                          Descripcion = "PRINCIPAL",
                                          Filtro = string.Concat(_d.DESCTA.TrimStart().TrimEnd(), _q.DIRECC.TrimStart().TrimEnd())
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



        [Route("api/Factura/Get")]
        [HttpGet]
        public string Get()
        {
            return v_Get();
        }


        private string v_Get()
        {
            string json = string.Empty;

            try
            {
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

                    var qDoc = (from _q in _Conexion.Venta
                                      select new
                                      {
                                          _q.NoFactura,
                                          _q.NoPedido,
                                          _q.Fecha,
                                          _q.CodCliente,
                                          _q.NomCliente,
                                          _q.Nombre,
                                          _q.CodBodega,
                                          _q.NomBodega,
                                          _q.CodVendedor,
                                          _q.NomVendedor,
                                          _q.TipoVenta,
                                          _q.Vence,
                                          _q.TotalCordoba,
                                          _q.TotalDolar,
                                          _q.Estado,
                                          _q.UsuarioRegistra
                                      }).ToList();


                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "DOCUENTOS";
                    datos.d = qDoc;
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
                    using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                    {

                       
                        bool esNuevo = false;
          
                        Venta _v = _Conexion.Venta.Find(d.IdVenta);

                        if (_v == null)
                        {
                            _Conexion.Database.ExecuteSqlCommand($"UPDATE DBO.{(d.TipoDocumento == "Factura" ? "ConfiguraFacturacion" : "ControlInventario")} SET Secuencia += 1    WHERE  Serie = '{d.Serie}' AND Bodegas = '{d.CodBodega}'");
                            _Conexion.SaveChanges();

                            int Consecutivo = _Conexion.Database.SqlQuery<int>($"SELECT Secuencia - 1 FROM {(d.TipoDocumento == "Factura" ? "ConfiguraFacturacion" : "ControlInventario")} WHERE Serie = '{d.Serie}' AND Bodegas = '{d.CodBodega}'").First();

                            _v = new Venta();
                            d.IdVenta = Guid.NewGuid();
                            d.NoFactura = string.Empty;
                            d.NoPedido = string.Empty;
                            d.Estado = "Solicitado";
                            if (d.TipoDocumento == "Factura") d.NoFactura = string.Concat(d.Serie, Consecutivo);
                            if (d.TipoDocumento == "Pedido") d.NoPedido = string.Concat(d.Serie, Consecutivo);
                            esNuevo = true;
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
                        _v.Contanto = d.Contanto;
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
                        _v.FechaRegistro = DateTime.Now;
                        _v.UsuarioRegistra = d.UsuarioRegistra;
                        _v.Estado = d.Estado;
                 

                        if (esNuevo)
                        {
                            _Conexion.Venta.Add(_v);
                        }
                        else
                        {
                            _Conexion.VentaDetalle.RemoveRange(_v.VentaDetalle);
                        }

                        


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
                            _vDet.EsExonerado = det.EsExonerado;
                            _vDet.PrecioLiberado = det.PrecioLiberado;
                            _vDet.IndexUnion = det.IndexUnion;


                            _v.VentaDetalle.Add(_vDet);
                        }




                        List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "DOCUMENTO";
                        datos.d =  _v.TipoDocumento == "Factura" ? _v.NoFactura : _v.NoPedido;
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


    }
}