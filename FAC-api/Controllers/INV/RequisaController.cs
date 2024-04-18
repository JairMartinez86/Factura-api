
using DevExpress.DataProcessing;
using FAC_api.Class;
using FAC_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Transactions;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using Escasan_Api.Class;
using FAC_api.Class.INV;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace FAC_api.Controllers.INV
{
    public class RequisaController : ApiController
    {
        [Route("api/INV/Requisa/Get")]
        [HttpGet]
        public string Get(string usuario)
        {
            return v_Get(usuario);
        }


        private string v_Get(string usuario)
        {
            string json = string.Empty;
            if (usuario == null) usuario = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    Cls_Datos lstDatos = new Cls_Datos();
                    Usuarios U = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == usuario);

                    List<string> ub = (from _q in _Conexion.UsuariosBodegas
                                       join _b in _Conexion.Bodegas on _q.IdBodega equals _b.IdBodega
                                       where _q.IdUsuario == U.IdUsuario
                                       select _b.Codigo).ToList();


                    var qRequisa = (from _q in _Conexion.Requisa
                                      where ub.Contains(_q.CodBodegaDestino) && (_q.Transito || _q.EnProceso) && _q.Anulado == false
                                      orderby _q.NoRequisa descending
                                      select new
                                      {
                                          _q.IdRequisa,
                                          _q.NoRequisa,
                                          _q.CodBodega,
                                          _q.CodBodegaDestino,
                                          _q.Fecha,
                                          RequisaDetalle = (from _det in _q.RequisaDetalle
                                                             join _p in _Conexion.Productos on _det.CodigoProducto equals _p.Codigo
                                                             select new {
                                                                 Producto = string.Concat(_det.CodigoProducto, " - ", _p.Producto),
                                                                 _det.Ubicacion,
                                                                 _det.NoLote,
                                                                 _det.Vence,
                                                                 _det.Cantidad
                                                             }).ToList()

                                      }).ToList();


           


                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "AUTORIZAR REQUISA";
                    datos.d = qRequisa;
            






                    json = Cls_Mensaje.Tojson(datos, 1, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



        [Route("api/INV/Requisa/Autorizar")]
        [HttpGet]
        public async Task<string> Autorizar(Guid IdRequisa, string Usuario)
        {
            return await _Autorizar(IdRequisa, Usuario);

        }

        private async Task<string> _Autorizar(Guid IdRequisa, string Usuario)
        {

            string json = string.Empty;

            try
            {
                Cls_AutorizaRequisa d;

                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    Usuarios U = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == Usuario);


                    d = new Cls_AutorizaRequisa();
                    d.IdRequisa = new List<Guid>() { IdRequisa };
                    d.IdUsuario = U.IdUsuario;
                }


                HttpClient aClient = new HttpClient();
                StringContent storeHttpContent;
                storeHttpContent = new StringContent(JsonConvert.SerializeObject(d), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await aClient.PostAsync($"{Cls_ConexionAPI_BD.Url}/INV/Requisa/Autoriza", storeHttpContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Cls_E_Response resp = JsonConvert.DeserializeObject<Cls_E_Response>(responseBody);

               

               


                json = Cls_Mensaje.Tojson(null, 0, string.Empty, "<p>" + resp.msj.Replace("\n", "<br>") + "</p>", (resp.esError? 1 : 0));




            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;

        }




        [Route("api/INV/Requisa/GetPermiso")]
        [HttpGet]
        public string GetPermiso()
        {
            return V_GetPermiso();
        }

        private string V_GetPermiso()
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {

    


                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                    var qUsuario = (from _q in _Conexion.Usuarios
                                    where _q.Activo
                                    select new
                                    {
                                        _q.Usuario,
                                        Nombre = string.Concat(_q.Usuario, " --> ", _q.Nombres, " ", _q.Apellidos)
                                    }).ToList();






                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "USUARIO";
                    datos.d = qUsuario;
                    lstDatos.Add(datos);



                    var qBodega = (from _q in _Conexion.Bodegas
                                    select new
                                    {
                                        IdPermisoRequisaAuto = -1,
                                        CodBodega = _q.Codigo,
                                        Nombre = string.Concat( _q.Codigo, " --> ", _q.Bodega),
                                        Usuario = string.Empty,
                                        Activo = true
                                    }).ToList();






                    datos = new Cls_Datos();
                    datos.Nombre = "BODEGA";
                    datos.d = qBodega;
                    lstDatos.Add(datos);




                    var qPermisos = (from _q in _Conexion.RequisaAutorisaPermiso
                                     select new
                                     {
                                         _q.IdPermisoRequisaAuto,
                                         _q.CodBodega,
                                         _q.Usuarios.Usuario,
                                         UsuarioRegistra = string.Empty
                                     }).ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "PERMISO REQUISA";
                    datos.d = qPermisos;
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


        [Route("api/INV/Requisa/AutorizarPermiso")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AutorizarPermiso(Cls_RequisaAutorisaPermiso[] d)
        {

            if (ModelState.IsValid)
            {

                return Ok(V_AutorizarPermiso(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string V_AutorizarPermiso(Cls_RequisaAutorisaPermiso[] d)
        {

            string json = string.Empty;

            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {

                        int IdUsuario = 0;


                        d.ForEach(f =>
                        {
                            bool esNuevo = false;
                            RequisaAutorisaPermiso a = _Conexion.RequisaAutorisaPermiso.Find(f.IdPermisoRequisaAuto);


                            if (a == null)
                            {
                                a = new RequisaAutorisaPermiso();
                                esNuevo = true;
                            }

                            if (IdUsuario == 0) IdUsuario = _Conexion.Usuarios.First(w => w.Usuario == f.Usuario).IdUsuario;

                            a.IdUsuario = IdUsuario;
                            a.CodBodega = f.CodBodega;
                            a.IdUsuarioRegistra = f.IdUsuarioRegistra;




                            if (esNuevo && f.Activo) _Conexion.RequisaAutorisaPermiso.Add(a);
                            if(!f.Activo && !esNuevo) _Conexion.RequisaAutorisaPermiso.Remove(a);


                            _Conexion.SaveChanges();
                        });




                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "PERMISO REQUISA";
                        datos.d = "Registro Guardado";





                        scope.Complete();

                        json = Cls_Mensaje.Tojson(datos, 1, string.Empty, string.Empty, 0);

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