using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAC_api.Class
{

    public class Cls_Mensaje
    {
        public static string Tojson(object o, int Length, string CodError, string Mensaje, int esError)
        {
            string json = string.Empty;


            if (o != null)
            {
                json = JsonConvert.SerializeObject(o);
                json = string.Concat("{ \"d\": ", json, ",  \"msj\": ", "{\"Codigo\":\"", CodError, "\",\"Mensaje\":\"", Mensaje, "\"}", ", \"count\":", Length, ", \"esError\":", 0, "}");
            }
            else
                json = string.Concat("{ \"d\":  [{ }],  \"msj\": ", "{\"Codigo\":\"", CodError, "\",\"Mensaje\":\"", Mensaje.Replace("\r\n", " "), "\"}", ", \"count\":", Length, ", \"esError\":", esError, "}");





            return json;
        }



    }
}