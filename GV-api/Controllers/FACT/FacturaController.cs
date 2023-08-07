using GV_api.Class;
using GV_api.Class.FACT;
using GV_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("api/Factura/Datos")]
        [HttpGet]
        public string Datos()
        {
            return _Datos();
        }


        private string _Datos()
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
            return _DatosCredito(CodCliente);
        }

        private string _DatosCredito(string CodCliente)
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
            return _ClienteClave(CodCliente);
        }

        private string _ClienteClave(string CodCliente)
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
        public string CargarProductos(string CodCliente, decimal Tc)
        {
            return _CargarProductos(CodCliente, Tc);
        }

        private string _CargarProductos(string CodCliente, decimal Tc)
        {
            string json = string.Empty;
            if (CodCliente == null) CodCliente = string.Empty;
            try
            {
                using (INVESCASANEntities _Conexion = new INVESCASANEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Cls_Datos datos = new Cls_Datos();
                    string strTipo = "Publico";
                    bool EsDolar = false;




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



                    List<Cls_Precio> qPrecios = (from _q in _Conexion.Listadeprecios
                                      select new Cls_Precio()
                                      {
                                          CodProducto = _q.CodiProd.TrimStart().TrimEnd(),
                                          Tipo = _q.Tipo.TrimStart().TrimEnd(),
                                          PrecioCordoba = ((decimal)(!EsDolar? _q.Precio : _q.Precio * Tc)),
                                          PrecioDolar = ((decimal)(EsDolar ? _q.Precio : _q.Precio / Tc)),
                                          Principal = (_q.Tipo.TrimStart().TrimEnd() == strTipo ? true : false)
                                      }).ToList();



                    datos = new Cls_Datos();
                    datos.Nombre = "PRECIOS";
                    datos.d = qPrecios;
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