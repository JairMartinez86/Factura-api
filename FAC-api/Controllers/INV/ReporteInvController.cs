﻿using FAC_api.Class;
using FAC_api.Class.CXP;
using FAC_api.Class.FACT;
using FAC_api.Class.INV;
using FAC_api.Class.PRE;
using FAC_api.Models;
using FAC_api.Reporte.INV;
using FAC_api.Reporte.INV.DsetInventarioTableAdapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace FAC_api.Controllers.INV
{
    public class ReporteInvController : ApiController
    {

        [Route("api/INV/Reporte/GetDatos")]
        [HttpGet]
        public string GetDatos()
        {
            return V_GetDatos();
        }


        private string V_GetDatos()
        {
            string json = string.Empty;

            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos>   lstDatos = new List<Cls_Datos>();



                    var qBodegas = (from _q in _Conexion.Bodegas
                                    orderby _q.Codigo
                                    select new Cls_Bodega()
                                    {
                                        IdBodega = _q.IdBodega,
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Bodega = _q.Bodega.TrimStart().TrimEnd(),
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Bodega.TrimStart().TrimEnd())
                                    }).ToList();




                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "BODEGA";
                    datos.d = qBodegas;
                    lstDatos.Add(datos);


                    var qProductos = (from _q in _Conexion.Productos
                                      join _p in _Conexion.Proveedor on _q.CodProveedorEscasan equals _p.Codigo into u_q_p
                                      from u in u_q_p.DefaultIfEmpty()
                                      where _q.Activo == true
                                      select new
                                      {
                                          _q.IdProducto,
                                          Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                          Producto = _q.Producto.TrimStart().TrimEnd(),
                                          _q.NoParte,
                                          Proveedor = u.Proveedor1.TrimStart().TrimEnd(),
                                          Key = string.Concat(_q.Codigo, " ", _q.Producto.TrimStart().TrimEnd(), " ", _q.NoParte.TrimStart().TrimEnd(), " ", u.Proveedor1.TrimStart().TrimEnd()),
                                          _q.IdGrupoPresupuestario,
                                          _q.IdGrupo,
                                          _q.IdSubGrupo,
                                          _q.CodProveedorEscasan
                                      }).ToList();

                    datos = new Cls_Datos();
                    datos.Nombre = "PRODUCTOS";
                    datos.d = qProductos;
                    lstDatos.Add(datos);





                    var qProveedor = (from _q in _Conexion.Proveedor
                                    orderby _q.Codigo
                                    select new Cls_Proveedor()
                                    {
                                        IdProveedor = _q.IdProveedor,
                                        Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                        Proveedor = _q.Proveedor1.TrimStart().TrimEnd(),
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Proveedor1.TrimStart().TrimEnd())
                                    }).ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "PROVEEDOR";
                    datos.d = qProveedor;
                    lstDatos.Add(datos);

                    var qGrupoPre = (from _q in _Conexion.GrupoPresupuestarios
                                      orderby _q.Codigo
                                      select new Cls_GrupoPresupuesto()
                                      {
                                          IdGrupoPresupuestario = _q.IdGrupoPresupuestario,
                                          Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                          GrupoPresupuestario = _q.GrupoPresupuestario.TrimStart().TrimEnd(),
                                          Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.GrupoPresupuestario.TrimStart().TrimEnd())
                                      }).ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "GRUPO PRESUPUESTO";
                    datos.d = qGrupoPre;
                    lstDatos.Add(datos);

                    var qGrupos = (from _q in _Conexion.Grupos
                                     orderby _q.Codigo
                                     select new Cls_Grupo()
                                     {
                                         IdGrupo = _q.IdGrupo,
                                         Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                         Grupo = _q.Grupo.TrimStart().TrimEnd(),
                                         Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Grupo.TrimStart().TrimEnd())
                                     }).ToList();

                    datos = new Cls_Datos();
                    datos.Nombre = "GRUPO";
                    datos.d = qGrupos;
                    lstDatos.Add(datos);


                    var qSubGrupos = (from _q in _Conexion.SubGrupos
                                   orderby _q.Codigo
                                   select new Cls_SubGrupo()
                                   {
                                       IdSubGrupo = _q.IdSubGrupo,
                                       IdGrupo = _q.IdGrupo,
                                       Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                       SubGrupo = _q.SubGrupo.TrimStart().TrimEnd(),
                                       Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.SubGrupo.TrimStart().TrimEnd())
                                   }).ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "SUB GRUPO";
                    datos.d = qSubGrupos;
                    lstDatos.Add(datos);


                    var qSerie = (from _q in _Conexion.Serie
                                  join _b in _Conexion.BodegaSerie on _q.IdSerie equals _b.Serie
                                  where !_b.EsCaja && !_b.EsColector
                                  group _q by new { _q.IdSerie } into g
                                  select new
                                  {
                                      g.Key.IdSerie
                                  }
                                     ).ToList();



                    datos = new Cls_Datos();
                    datos.Nombre = "SERIE";
                    datos.d = qSerie;
                    lstDatos.Add(datos);




                    var qTipoMov = _Conexion.TipoMov.ToList();



                    datos = new Cls_Datos();
                    datos.Nombre = "TIPO MOV";
                    datos.d = qTipoMov;
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



        [Route("api/INV/Reporte/Imprimir")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Imprimir(Cls_ParamReporte d)
        {

            if (ModelState.IsValid)
            {

                return Ok(V_Imprimir(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string V_Imprimir(Cls_ParamReporte d)
        {
            string json = string.Empty;



            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {

                     Cls_Datos DatosReporte = new Cls_Datos();

                    DsetInventario DsetReporte = new DsetInventario();

                     MemoryStream stream = new MemoryStream();





                     switch (d.TipoReporte)
                     {

                         case "Detalle Transacciones Inventario":

                            if (d.Param[0] == null) d.Param[0] = string.Empty;
                            if (d.Param[1] == null) d.Param[1] = string.Empty;
                            if (d.Param[2] == null) d.Param[2] = string.Empty;
                            if (d.Param[3] == null) d.Param[3] = string.Empty;



                            xrDetTransaccDiarias xrpTransaccInv = new xrDetTransaccDiarias();
                            RPT_DetTransaccDiariasTableAdapter adpTransaccInv = new RPT_DetTransaccDiariasTableAdapter();
                            adpTransaccInv.Fill(DsetReporte.RPT_DetTransaccDiarias, d.Param[0].ToString(), d.Param[1].ToString(), d.Param[2].ToString(), d.Param[3].ToString(), Convert.ToDateTime(d.Param[4]), Convert.ToDateTime(d.Param[5]));
                            xrpTransaccInv.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[4]);
                            xrpTransaccInv.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[5]);
                            xrpTransaccInv.DataSource = DsetReporte;


                            xrpTransaccInv.ShowPrintMarginsWarning = false;
                            xrpTransaccInv.ExportToPdf(stream, null);

                             stream.Seek(0, SeekOrigin.Begin);

                             DatosReporte.d = stream.ToArray();
                             DatosReporte.Nombre = d.TipoReporte;
                             break;

                         case "2":



                             break;

                         case "4":



                             break;


                         case "5":
                             break;
                         case "6":
                             break;
                         case "7":
                             break;
                     }



                    json = Cls_Mensaje.Tojson(DatosReporte, 1, string.Empty, string.Empty, 0);
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