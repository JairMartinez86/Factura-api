using GV_api.Class;
using GV_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
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

          
                    var query = (from _q in _Conexion.Clientes
                                 select new
                                 {
                                     Codigo = _q.CODCTA.TrimStart().TrimEnd(),
                                     Cliente = _q.DESCTA.TrimStart().TrimEnd(),
                                     Filtro = string.Concat(_q.CODCTA.TrimStart().TrimEnd(), _q.DESCTA.TrimStart().TrimEnd(), _q.Cedula.TrimStart().TrimEnd(), _q.TELEFO.TrimStart().TrimEnd(), _q.TELEFONO1.TrimStart().TrimEnd(), _q.Celular.TrimStart().TrimEnd()),
                                     Key = string.Concat(_q.CODCTA.TrimStart().TrimEnd(), " ", _q.DESCTA.TrimStart().TrimEnd())
                                 }).ToList();

                    datos.Nombre = "CLIENTES";
                    datos.d = query;
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