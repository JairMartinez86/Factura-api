using DevExpress.DirectX.Common.Direct2D;
using FAC_api.Class;
using FAC_api.Class.FACT;
using FAC_api.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using Microsoft.Ajax.Utilities;

namespace FAC_api.Class.SIS
{
    public class ServerController : ApiController
    {

        [Route("api/SIS/Login")]
        [HttpGet]
        public string Login(string user, string pass)
        {
            return v_Login(user, pass);
        }

        private string v_Login(string user, string pass)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

              
                    var qUsuario = (from _q in _Conexion.Usuarios.ToList()
                                    join _e in _Conexion.Estaciones on _q.IdUsuario equals _e.IdUsuario
                                    join _b in _Conexion.Bodegas on _e.IdBodega equals _b.IdBodega
                                    where _q.Usuario.TrimStart().TrimEnd() == user && _q.AccesoWeb
                                    select new
                                    {
                                        User = _q.Usuario,
                                        Nombre = _q.Nombres,
                                        Pwd = _Conexion.Database.SqlQuery<string>($"SELECT [SIS].[Desencriptar]({"0x" + BitConverter.ToString(_q.Pass).Replace("-", "")})").Single(),
                                        Rol = string.Empty,
                                        Bodega = _b.Codigo,
                                        _q.Lotificar,
                                        FechaLogin = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now),
                                        Desconectar = (!_q.AccesoWeb ? true : _q.Desconectar ),
                                        _q.ColaImpresionWeb
                                    }).ToList();



                    if (qUsuario.Count == 0)
                    {
                        json = Cls_Mensaje.Tojson(null, 0, "1", "Usuario no encontrado.", 1);
                        return json;
                    }




                    string Pwd = qUsuario[0].Pwd;

                    if (!Pwd.Equals(pass))
                    {
                        json = Cls_Mensaje.Tojson(null, 0, string.Empty, "Contraseña Incorrecta.", 1);
                        return json;
                    }

                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "USUARIO";
                    datos.d = qUsuario;
                    lstDatos.Add(datos);


                    lstDatos.AddRange(v_FechaServidor(user, qUsuario[0].Desconectar, _Conexion));



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



 

        [Route("api/SIS/FechaServidor")]
        [HttpGet]
        public string FechaServidor(string user)
        {
            return v_FechaServidor(user);
        }

        private string v_FechaServidor(string user)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    Usuarios u = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == user);
          
                    lstDatos.AddRange(v_FechaServidor(user, (u  == null ? true : (!u.AccesoWeb ? true : u.Desconectar)), _Conexion));



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        private Cls_Datos[] v_FechaServidor(string user, bool Desconectar,  BalancesEntities _Conexion)
        {
            Usuarios u = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == user);

            Cls_Datos datos = new Cls_Datos();
            datos.Nombre = "FECHA SERVIDOR";
            datos.d = string.Format("{0:yyy-MM-dd hh:mm:ss}", DateTime.Now);

          
            Cls_Datos datos2 = new Cls_Datos();
            datos2.Nombre = "DESCONECCION";
            datos2.d = Desconectar ? "-1" : "7200";

            Cls_Datos datos3 = new Cls_Datos();
            datos3.Nombre = "LOTIFICAR";
            datos3.d = u.Lotificar;


            var Perfil = _Conexion.AccesoWeb.Where(w => w.Usuario == user && w.Activo).ToList();

            Cls_Datos datos4 = new Cls_Datos();
            datos4.Nombre = "PERFIL";
            datos4.d = Perfil;


            return new Cls_Datos[] { datos, datos2, datos3, datos4 };
        }


        [Route("api/SIS/AccesoWeb")]
        [HttpGet]
        public string AccesoWeb(string user)
        {
            return V_AccesoWeb(user);
        }

        private string V_AccesoWeb(string user)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {

                    Usuarios Usuario = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == user && f.IdRol == 1 & f.AccesoWeb);

                    if (Usuario == null)
                    {
                        json = Cls_Mensaje.Tojson(null, 0, "1", "No tiene permiso para acceder a la inforacion solicitada.", 1);
                        return json;
                    }



                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();


                    var qUsuario = (from _q in _Conexion.Usuarios
                                    where _q.AccesoWeb
                                    select new
                                    {
                                        _q.Usuario,
                                        Nombre = string.Concat(_q.Usuario, " --> " ,_q.Nombres, " ", _q.Apellidos)
                                    }).ToList();






                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "USUARIO";
                    datos.d = qUsuario;
                    lstDatos.Add(datos);



                    var qAccesoWeb = _Conexion.AccesoWeb.ToList();


                    datos = new Cls_Datos();
                    datos.Nombre = "ACCESO WEB";
                    datos.d = qAccesoWeb;
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



        [Route("api/SIS/GuardarAcceso")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult GuardarAcceso(AccesoWeb[] d)
        {

            if (ModelState.IsValid)
            {

                return Ok(V_GuardarAcceso(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string V_GuardarAcceso(AccesoWeb[] d)
        {

            string json = string.Empty;

            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {

                        d.ForEach(f => {

                            bool esNuevo = false;
                            AccesoWeb a = _Conexion.AccesoWeb.Find(f.IdAcceso);

                            if (a == null)
                            {
                                a = new AccesoWeb();
                                esNuevo = true;
                            }

                            a.EsMenu = f.EsMenu;
                            a.Id = f.Id;
                            a.Caption = f.Caption;
                            a.MenuPadre = f.MenuPadre;
                            a.Clase = f.Clase;
                            a.Usuario = f.Usuario;
                            a.Modulo = "FACT";
                            a.Activo = f.Activo;

                            if (esNuevo) _Conexion.AccesoWeb.Add(a);
                            _Conexion.SaveChanges();



                        });


                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "ACCESO WEB";
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