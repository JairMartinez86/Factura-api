using FAC_api.Class;
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
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using System.Drawing;
using Azure;

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





                    var qClientes = (from _q in _Conexion.Cliente
                                    orderby _q.Codigo
                                    select new
                                    {
                                        _q.Codigo,
                                        _q.Nombre,
                                        Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Nombre.TrimStart().TrimEnd())
                                    }).ToList();




                    datos = new Cls_Datos();
                    datos.Nombre = "CLIENTES";
                    datos.d = qClientes;
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

                            RPT_DetTransaccDiariasTableAdapter adpTransaccInv = new RPT_DetTransaccDiariasTableAdapter();
                            adpTransaccInv.Fill(DsetReporte.RPT_DetTransaccDiarias, d.Param[0].ToString(), d.Param[1].ToString(), d.Param[2].ToString(), d.Param[3].ToString(), Convert.ToDateTime(d.Param[4]), Convert.ToDateTime(d.Param[5]));

                            if (!d.Exportar)
                            {
                                xrDetTransaccDiariasPDF xrpTransaccInvPDF = new xrDetTransaccDiariasPDF();
                                xrpTransaccInvPDF.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[4]);
                                xrpTransaccInvPDF.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[5]);
                                xrpTransaccInvPDF.DataSource = DsetReporte;
                                xrpTransaccInvPDF.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;


                                xrpTransaccInvPDF.ShowPrintMarginsWarning = false;
                                xrpTransaccInvPDF.ExportToPdf(stream, null);

                                stream.Seek(0, SeekOrigin.Begin);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;
                            }
                            else
                            {
                                xrDetTransaccDiariasXLS xrpTransaccInvXLS = new xrDetTransaccDiariasXLS();
                                xrpTransaccInvXLS.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[4]);
                                xrpTransaccInvXLS.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[5]);
                                xrpTransaccInvXLS.DataSource = DsetReporte;
                                xrpTransaccInvXLS.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                                xrpTransaccInvXLS.ShowPrintMarginsWarning = false;
                                xrpTransaccInvXLS.ExportToXlsx(stream, null);

                                stream.Seek(0, SeekOrigin.Begin);

                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Transacciones Diarias";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                                workbook.BeginUpdate();

                                CellRange range = worksheet["A6:L6"];
                                worksheet["A6:L6"].Style.Font.Bold = true;
                                //workbook.Worksheets[0].Range.Parse("K8:M11").Style.Font.Color = Color.Red;
                                worksheet.AutoFilter.Apply(range);
                                workbook.EndUpdate();

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;
                            }







                            break;

                        case "Transacciones En Proceso":


                            RPT_DetTransaccEnProcesoTableAdapter adpTransaccProc = new RPT_DetTransaccEnProcesoTableAdapter();
                            adpTransaccProc.Fill(DsetReporte.RPT_DetTransaccEnProceso);

                            xrTransaccionesEnProceso xrpTransaccProc = new xrTransaccionesEnProceso();
                            xrpTransaccProc.DataSource = DsetReporte;
                            xrpTransaccProc.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpTransaccProc.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpTransaccProc.ExportToPdf(stream, null);

                            }
                            else
                            {
                                xrpTransaccProc.ExportToXlsx(stream, null);


                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Transacciones En Proceso";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                                workbook.BeginUpdate();

                                CellRange range = worksheet["A5:E5"];
                                worksheet["A5:E5"].Style.Font.Bold = true;
                                worksheet.AutoFilter.Apply(range);
                                workbook.EndUpdate();

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }




                            stream.Seek(0, SeekOrigin.Begin);
                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;


                            break;

                        case "Control de Consecutividad":

                            if (d.Param[0] == null) d.Param[0] = string.Empty;
                            if (d.Param[1] == null) d.Param[1] = string.Empty;
                            if (d.Param[2] == null) d.Param[2] = string.Empty;

                            RPT_DetTRevisionConsecutivoTableAdapter adpRevConsecutivo = new RPT_DetTRevisionConsecutivoTableAdapter();
                            adpRevConsecutivo.Fill(DsetReporte.RPT_DetTRevisionConsecutivo, Convert.ToDateTime(d.Param[0]), Convert.ToDateTime(d.Param[1]), d.Param[2].ToString());

                            xrInvRevConsecutivo xrpRevConsecutivo = new xrInvRevConsecutivo();
                            xrpRevConsecutivo.DataSource = DsetReporte;
                            xrpRevConsecutivo.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpRevConsecutivo.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpRevConsecutivo.ExportToPdf(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                xrpRevConsecutivo.ExportToXlsx(stream, null);


                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Control de Consecutividad";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                                workbook.BeginUpdate();

                                CellRange range = worksheet["A5:F5"];
                                worksheet["A5:F5"].Style.Font.Bold = true;
                                worksheet.AutoFilter.Apply(range);
                                workbook.EndUpdate();

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }




                            stream.Seek(0, SeekOrigin.Begin);
                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;

                            break;


                        case "Transacciones de Inventario":


                            if (d.Param[0] == null) d.Param[0] = string.Empty;
                            if (d.Param[1] == null) d.Param[1] = string.Empty;
                            if (d.Param[2] == null) d.Param[2] = string.Empty;

                            RPT_TransaccionesInventarioTableAdapter adpRevTransInv = new RPT_TransaccionesInventarioTableAdapter();
                            adpRevTransInv.Fill(DsetReporte.RPT_TransaccionesInventario, Convert.ToDateTime(d.Param[0]), Convert.ToDateTime(d.Param[1]), d.Param[2].ToString());

                            xrTransaccionesInventario xrpTransInv = new xrTransaccionesInventario();
                            xrpTransInv.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[0]);
                            xrpTransInv.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[1]);
                            xrpTransInv.DataSource = DsetReporte;
                            xrpTransInv.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpTransInv.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpTransInv.ExportToPdf(stream, null);

                            }
                            else
                            {
                                xrpTransInv.ExportToXlsx(stream, null);



                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Transacciones de Inventario";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                     

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }



                            stream.Seek(0, SeekOrigin.Begin);
                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;

                            break;
                        case "Transacciones de Inventario Resumen":
                            if (d.Param[0] == null) d.Param[0] = string.Empty;
                            if (d.Param[1] == null) d.Param[1] = string.Empty;


                            RPT_TransaccionesInventarioResumenTableAdapter adpRevTransInvResumen = new RPT_TransaccionesInventarioResumenTableAdapter();
                            adpRevTransInvResumen.Fill(DsetReporte.RPT_TransaccionesInventarioResumen, Convert.ToDateTime(d.Param[0]), Convert.ToDateTime(d.Param[1]));

                            xrTransaccionInvResumen xrpTransInvResumen = new xrTransaccionInvResumen();
                            xrpTransInvResumen.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[0]);
                            xrpTransInvResumen.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[1]);
                            xrpTransInvResumen.DataSource = DsetReporte;
                            xrpTransInvResumen.ExportOptions.Pdf.DocumentOptions.Title = "Resumen Iventario";

                            xrpTransInvResumen.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpTransInvResumen.ExportToPdf(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                xrpTransInvResumen.ExportToXlsx(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);



                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Resumen";
                                workbook.Worksheets.ActiveWorksheet = worksheet;



                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }


                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;

                            break;
                        case "Factura Costo":
                            if (d.Param[0] == null) d.Param[0] = string.Empty;



                            RPT_DetFacturaCostoTableAdapter adpFacturaCosto = new RPT_DetFacturaCostoTableAdapter();
                            adpFacturaCosto.Fill(DsetReporte.RPT_DetFacturaCosto, d.Param[0].ToString(), Convert.ToDateTime(d.Param[1]), Convert.ToDateTime(d.Param[2]));

                            xrDetFacturaCosto xrpFacturaCosto = new xrDetFacturaCosto();
                            xrpFacturaCosto.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[1]);
                            xrpFacturaCosto.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[2]);
                            xrpFacturaCosto.DataSource = DsetReporte;
                            xrpFacturaCosto.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpFacturaCosto.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpFacturaCosto.ExportToPdf(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                xrpFacturaCosto.ExportToXlsx(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);



                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Costo Factura";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                   

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }


                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;
                            break;



                        case "Columnar Existencia":
                            if (d.Param[0] == null) d.Param[0] = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            if (d.Param[1] == null) d.Param[1] = string.Empty;
                            if (d.Param[2] == null) d.Param[2] = string.Empty;
                            if (d.Param[3] == null) d.Param[3] = string.Empty;
                            if (d.Param[4] == null) d.Param[4] = string.Empty;
                            if (d.Param[5] == null) d.Param[5] = string.Empty;
                            if (d.Param[6] == null) d.Param[6] = string.Empty;
                            if (d.Param[7] == null) d.Param[7] = string.Empty;
                            if (d.Param[8] == null) d.Param[8] = false;


                            XRP_ExistenciaColumnarTableAdapter adpConlumnarInv = new XRP_ExistenciaColumnarTableAdapter();
                            adpConlumnarInv.Fill(DsetReporte.XRP_ExistenciaColumnar, Convert.ToDateTime(d.Param[0]), d.Param[1].ToString(), d.Param[2].ToString(), d.Param[3].ToString(), d.Param[4].ToString(), d.Param[5].ToString(), d.Param[6].ToString(), d.Param[7].ToString(), Convert.ToBoolean(d.Param[8]));

                            xrExistenciaColumnar xrpColumnarInv = new xrExistenciaColumnar();
                            xrpColumnarInv.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[0]);
                            xrpColumnarInv.DataSource = DsetReporte;
                            xrpColumnarInv.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpColumnarInv.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpColumnarInv.ExportToPdf(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                xrpColumnarInv.ExportToXlsx(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);



                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Columnar Inventario";
                                workbook.Worksheets.ActiveWorksheet = worksheet;



                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }


                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;
                            break;

                        case "Ventas Por Cliente":


                            if (d.Param[0] == null) d.Param[0] = string.Empty;
                            if (d.Param[1] == null) d.Param[1] = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            if (d.Param[2] == null) d.Param[2] = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(-1));
                            if (d.Param[3] == null) d.Param[3] = string.Empty;
                            if (d.Param[4] == null) d.Param[4] = string.Empty;
                            if (d.Param[5] == null) d.Param[5] = string.Empty;
                            if (d.Param[6] == null) d.Param[6] = string.Empty;
                            if (d.Param[7] == null) d.Param[7] = string.Empty;
                            if (d.Param[8] == null) d.Param[8] = string.Empty;
                            if (d.Param[9] == null) d.Param[9] = string.Empty;
                            if (d.Param[10] == null) d.Param[10] = string.Empty;
                            if (d.Param[11] == null) d.Param[11] = false;

                            if (Convert.ToDateTime((d.Param[1])).Year == Convert.ToDateTime((d.Param[2])).Year) d.Param[2] = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddYears(-1));

                            if (d.Param[12].ToString() == "D")
                            {


                                RPT_Ventas_X_Cliente_DetTableAdapter adpVentaXCliente = new RPT_Ventas_X_Cliente_DetTableAdapter();
                                adpVentaXCliente.Fill(DsetReporte.RPT_Ventas_X_Cliente_Det, d.Param[0].ToString(), Convert.ToDateTime(d.Param[1]), Convert.ToDateTime(d.Param[2]), d.Param[3].ToString(), d.Param[4].ToString(), d.Param[5].ToString(), d.Param[6].ToString(), d.Param[7].ToString(), d.Param[8].ToString(), d.Param[9].ToString(), d.Param[10].ToString(), Convert.ToBoolean(d.Param[11]));

                                xrVentasPorClienteDet xrpVentaXClienteDet = new xrVentasPorClienteDet();
                                xrpVentaXClienteDet.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[1]);
                                xrpVentaXClienteDet.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[2]);
                                xrpVentaXClienteDet.DataSource = DsetReporte;
                                xrpVentaXClienteDet.ExportOptions.Pdf.DocumentOptions.Title = "Ventas x Cliente (D)";

                                xrpVentaXClienteDet.ShowPrintMarginsWarning = false;

                                if (!d.Exportar)
                                {
                                    xrpVentaXClienteDet.ExportToPdf(stream, null);
                                    stream.Seek(0, SeekOrigin.Begin);
                                }
                                else
                                {
                                    xrpVentaXClienteDet.ExportToXlsx(stream, null);
                                    stream.Seek(0, SeekOrigin.Begin);



                                    Workbook workbook = new Workbook();

                                    workbook.LoadDocument(stream);
                                    Worksheet worksheet = workbook.Worksheets[0];
                                    workbook.Worksheets[0].Name = "Ventas x Cliente (D)";
                                    workbook.Worksheets.ActiveWorksheet = worksheet;


                       
                                    stream = new MemoryStream();

                                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                    DatosReporte.d = stream.ToArray();
                                    DatosReporte.Nombre = d.TipoReporte;

                                }




                            }
                            else
                            {
                                RPT_Ventas_X_ClienteTableAdapter adpVentaXCliente = new RPT_Ventas_X_ClienteTableAdapter();
                                adpVentaXCliente.Fill(DsetReporte.RPT_Ventas_X_Cliente, d.Param[0].ToString(), Convert.ToDateTime(d.Param[1]), Convert.ToDateTime(d.Param[2]), d.Param[3].ToString(), d.Param[4].ToString(), d.Param[5].ToString(), d.Param[6].ToString(), d.Param[7].ToString(), d.Param[8].ToString(), d.Param[9].ToString(), d.Param[10].ToString(), Convert.ToBoolean(d.Param[11]));

                                xrVentasPorClienteC xrpVentaXClienteC = new xrVentasPorClienteC();
                                xrpVentaXClienteC.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[1]);
                                xrpVentaXClienteC.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[2]);
                                xrpVentaXClienteC.DataSource = DsetReporte;
                                xrpVentaXClienteC.ExportOptions.Pdf.DocumentOptions.Title = "Ventas por Cliente (C)";

                                xrpVentaXClienteC.ShowPrintMarginsWarning = false;

                                if (!d.Exportar)
                                {
                                    xrpVentaXClienteC.ExportToPdf(stream, null);
                                    stream.Seek(0, SeekOrigin.Begin);
                                }
                                else
                                {
                                    xrpVentaXClienteC.ExportToXlsx(stream, null);
                                    stream.Seek(0, SeekOrigin.Begin);



                                    Workbook workbook = new Workbook();

                                    workbook.LoadDocument(stream);
                                    Worksheet worksheet = workbook.Worksheets[0];
                                    workbook.Worksheets[0].Name = "Ventas por Cliente (C)";
                                    workbook.Worksheets.ActiveWorksheet = worksheet;


                                    workbook.BeginUpdate();



                                    CellRange range = worksheet["B7:D7"];
                                    worksheet["A7:H7"].Style.Font.Bold = true;


                                    workbook.EndUpdate();

                                    stream = new MemoryStream();

                                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                    DatosReporte.d = stream.ToArray();
                                    DatosReporte.Nombre = d.TipoReporte;

                                }
                            }






                            


                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;


                            break;

                        case "Ventas Por Sucursal":

                            if (d.Param[0] == null) d.Param[0] = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            if (d.Param[1] == null) d.Param[1] = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            if (d.Param[2] == null) d.Param[2] = string.Empty;
                            if (d.Param[3] == null) d.Param[3] = string.Empty;


                            XRP_VentasPorBodegaColumnarTableAdapter adpVentasSucursal = new XRP_VentasPorBodegaColumnarTableAdapter();
                            adpVentasSucursal.Fill(DsetReporte.XRP_VentasPorBodegaColumnar, Convert.ToDateTime(d.Param[0]), Convert.ToDateTime(d.Param[1]), string.Empty, d.Param[2].ToString(), d.Param[3].ToString());

                            xrVentasPorSucursal xrpVentasSucursal = new xrVentasPorSucursal();
                            xrpVentasSucursal.Parameters["P_Fecha1"].Value = Convert.ToDateTime(d.Param[0]);
                            xrpVentasSucursal.Parameters["P_Fecha2"].Value = Convert.ToDateTime(d.Param[1]);
                            xrpVentasSucursal.DataSource = DsetReporte;
                            xrpVentasSucursal.ExportOptions.Pdf.DocumentOptions.Title = d.TipoReporte;

                            xrpVentasSucursal.ShowPrintMarginsWarning = false;



                            if (!d.Exportar)
                            {
                                xrpVentasSucursal.ExportToPdf(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                xrpVentasSucursal.ExportToXlsx(stream, null);
                                stream.Seek(0, SeekOrigin.Begin);



                                Workbook workbook = new Workbook();

                                workbook.LoadDocument(stream);
                                Worksheet worksheet = workbook.Worksheets[0];
                                workbook.Worksheets[0].Name = "Ventas (SUCURSAL)";
                                workbook.Worksheets.ActiveWorksheet = worksheet;


                                workbook.BeginUpdate();

                                CellRange range = worksheet["A6:AC6"];
                                worksheet["A6:AC6"].Style.Font.Bold = true;

                                workbook.EndUpdate();

                                stream = new MemoryStream();

                                workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                DatosReporte.d = stream.ToArray();
                                DatosReporte.Nombre = d.TipoReporte;

                            }


                            DatosReporte.d = stream.ToArray();
                            DatosReporte.Nombre = d.TipoReporte;
                            break;


                        case "":
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