using GV_api.Class;
using GV_api.Class.FACT;
using GV_api.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace GV_api.Class.SIS
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
                using (AUDESCASANGVEntities _Conexion = new AUDESCASANGVEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();

              
                    var qUsuario = (from _q in _Conexion.Perfiles1.ToList()
                                    where _q.Usuario.TrimStart().TrimEnd() == user
                                    select new
                                    {
                                        User = _q.Usuario,
                                        Nombre = _q.Nombre,
                                        Pwd = _q.Clave,
                                        Rol = string.Empty,
                                        FechaLogin = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now),
                                        Desconectar = false
                                    }).ToList();



                    if (qUsuario.Count == 0)
                    {
                        json = Cls_Mensaje.Tojson(null, 0, "1", "Usuario no encontrado.", 1);
                        return json;
                    }




                    string Pwd = Desencriptar(qUsuario[0].Pwd, user);

                    if (!Pwd.Equals(pass))
                    {
                        json = Cls_Mensaje.Tojson(null, 0, string.Empty, "Contraseña Incorrecta.", 1);
                        return json;
                    }

                    Cls_Datos datos = new Cls_Datos();
                    datos.Nombre = "USUARIO";
                    datos.d = qUsuario;
                    lstDatos.Add(datos);


                    lstDatos.AddRange(v_FechaServidor(user, qUsuario[0].Desconectar));



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



        private string Desencriptar(string S, string P)
        {

            int C1 = 0;
            int C2 = 0;
            string r = string.Empty;

            if (P.Length > 0)
            {
                for (int i = 1; i <= S.Length; i++)
                {
              
                    C1 = charToDigit(Convert.ToChar(Mid(S, i, 1)));

                    if (i > P.Length)
                    {
                        C2 = charToDigit(Convert.ToChar(Mid(P, i % P.Length + 1, 1)));
                    }
                    else
                    {
                        C2 = charToDigit(Convert.ToChar(Mid(P, i , 1)));
                    }

                    C1 = C1 - C2 - 64;

                    if (Math.Sign(C1) == -1) C1 = 256 + C1;

                    r = r + Convert.ToChar(C1);
                }
            }
            else
            {
                r = S;
            }

            return r;
        }

        int charToDigit(char character)
        {
            return character - 64; //or character-0x40 if you prefer hex
        }

        public string Mid(string s, int a, int b)

        {

            string temp = s.Substring(a - 1, b);

            return temp;

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
                using (AUDESCASANGVEntities _Conexion = new AUDESCASANGVEntities())
                {
                    List<Cls_Datos> lstDatos = new List<Cls_Datos>();
                    // Perfiles1 u = _Conexion.Perfiles1.FirstOrDefault(f => f.Usuario.TrimStart().TrimEnd().Equals(user));


                    // lstDatos.AddRange(v_FechaServidor(user, (u == null ? true : u.Desconectar)));
                    lstDatos.AddRange(v_FechaServidor(user, false));



                    json = Cls_Mensaje.Tojson(lstDatos, lstDatos.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }


        private Cls_Datos[] v_FechaServidor(string user, bool Desconectar)
        {

            Cls_Datos datos = new Cls_Datos();
            datos.Nombre = "FECHA SERVIDOR";
            datos.d = string.Format("{0:yyy-MM-dd hh:mm:ss}", DateTime.Now);


            Cls_Datos datos2 = new Cls_Datos();
            datos2.Nombre = "DESCONECCION";
            datos2.d = Desconectar ? "-1" : "120";


            return new Cls_Datos[] { datos, datos2 };
        }
    }
}