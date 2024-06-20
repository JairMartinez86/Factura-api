using DevExpress.Utils.About;
using DevExpress.Utils.Extensions;
using DevExpress.XtraSpreadsheet.Model;
using FAC_api.Class;
using FAC_api.Class.CXC;
using FAC_api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace FAC_api.Controllers.FACT
{
    public class EstadoCuentaController : ApiController
    {


        [Route("api/CXC/EstadoCuenta/Datos")]
        [HttpGet]
        public string Datos(string Tipo, string Param)
        {
            return v_Datos(Tipo, Param);
        }

        private string v_Datos(string Tipo, string Param)
        {
            string json = string.Empty;
            if (Tipo == null) Tipo = string.Empty;
            if (Param == null) Param = string.Empty;

            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                

                    Cls_Datos datos = new Cls_Datos();

                    if (Tipo == "Clientes")
                    {

                        var qClientes = (from _q in _Conexion.Cliente
                                         select new
                                         {
                                             IdCliente = _q.IdCliente,
                                             Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                             Cliente = _q.Nombre.TrimStart().TrimEnd(),
                                             Ruc = _q.NoCedula.TrimStart().TrimEnd(),
                                             Cedula = _q.NoCedula.TrimStart().TrimEnd(),
                                             Correo = (_q.Correo.TrimStart().TrimEnd() != string.Empty ? _q.Correo.TrimStart().TrimEnd() + ";" : string.Empty) + (_q.Correo2.TrimStart().TrimEnd() != string.Empty ? _q.Correo2.TrimStart().TrimEnd() : string.Empty),
                                             Contacto = string.Concat(_q.Celular.TrimStart().TrimEnd(), "/", _q.Telefono1.TrimStart().TrimEnd(), "/", _q.Telefono2.TrimStart().TrimEnd(), "/", _q.Telefono3.TrimStart().TrimEnd()),
                                             Limite = _q.Limite,
                                             Moneda = _q.IdMoneda,
                                             CodVendedor = (_q.Vendedor == null ? string.Empty : _q.Vendedor.TrimStart().TrimEnd()),
                                             EsClave = _q.ClienteClave,
                                             Filtro = string.Concat(_q.Codigo, _q.Nombre.TrimStart().TrimEnd(), _q.NoCedula.TrimStart().TrimEnd(), _q.Telefono1.TrimStart().TrimEnd(), _q.Telefono2.TrimStart().TrimEnd(), _q.Telefono3.TrimStart().TrimEnd(), _q.Celular.TrimStart().TrimEnd()),
                                             Key = string.Concat(_q.Codigo, " ", _q.Nombre.TrimStart().TrimEnd())
                                         }).ToList();

                        datos.d = qClientes;

                    }
                    else
                    {
                        var Cliente = _Conexion.Cliente.FirstOrDefault(f => f.Codigo == Param);

                        if(Cliente != null)
                        {

                            List<decimal> lst = _Conexion.Database.SqlQuery<decimal>($"SELECT TasaCambio FROM CON.TasaCambio WHERE Fecha = CAST('{string.Format("{0:yyyy-MM-dd}", DateTime.Now)}' AS DATE)").ToList();

                            decimal tc = 1;
                            if (lst.Count > 0) tc = lst.First();



                            string Sql = $"DECLARE @P_CodCliente NVARCHAR(20) = '{Param}',\r\n@P_SaldoCordoba DECIMAL(18,2) = 0,\r\n@P_SaldoDolar DECIMAL(18,2) = 0,\r\n@P_MonedaLocal NVARCHAR(3),\r\n@P_MonedaSistema NVARCHAR(3),\r\n@P_UltimoRoc NVARCHAR(20),\r\n@P_UltimoRocFecha DATE,\r\n@P_UltimoRocMonto DECIMAL(18,2),\r\n@P_UltimoRocMoneda NVARCHAR(3),\r\n@P_Direccion NVARCHAR(MAX)\r\n\r\n\r\n\r\nSELECT TOP 1 @P_MonedaLocal = T.MonedaLocal, @P_MonedaSistema = T.MonedaFacturacion FROM SIS.Parametros AS T\r\n\r\n\r\nSELECT TOP 1 @P_Direccion = ISNULL(T.Direccion, '') FROM CXC.DireccionCliente AS T WHERE T.IdCliente = {Cliente.IdCliente} AND T.Activo = 1  ORDER BY T.EsDirPrincipal DESC\r\n\r\n\r\n\r\nSELECT @P_SaldoDolar = IIF( T.IdMoneda = @P_MonedaLocal, 0,  SUM(T.TotalDolar)), @P_SaldoCordoba =  IIF( T.IdMoneda = @P_MonedaLocal,  SUM(T.TotalCordoba), 0)\r\nFROM SIS.MovimientoDoc AS T\r\nWHERE T.CodigoCliente = @P_CodCliente AND T.Activo = 1 \r\nGROUP BY T.IdMoneda\r\n\r\n\r\nSELECT @P_UltimoRoc = MAX(T.NoDocOrigen), @P_UltimoRocFecha = T.FechaDocumento, @P_UltimoRocMonto = ABS(SUM(T.Total)), @P_UltimoRocMoneda = T.IdMoneda\r\nFROM SIS.MovimientoDoc AS T\r\nWHERE T.CodigoCliente = @P_CodCliente AND T.Activo = 1  AND T.TipoDocumentoOrigen = 'ROC.'\r\nGROUP BY T.TipoDocumentoOrigen, T.FechaDocumento, T.Total, T.IdMoneda\r\n\r\n\r\n\r\nSELECT ISNULL(@P_Direccion, '') AS Direccion,  ISNULL(@P_SaldoDolar, 0) AS SaldoDolar, ISNULL(@P_SaldoCordoba, 0) AS SaldoCordoba, ISNULL(@P_UltimoRoc, '') AS UltimoRoc, @P_UltimoRocFecha AS UltimoRocFecha, ISNULL(@P_UltimoRocMonto, 0) AS UltimoRocMonto, ISNULL(@P_UltimoRocMoneda, '') AS UltimoRocMoneda, @P_MonedaSistema AS MonedaSistema, @P_MonedaLocal AS MonedaLocal\r\n\r\n\r\n";
                            Cls_ClienteCartera cl = _Conexion.Database.SqlQuery<Cls_ClienteCartera>(Sql).Single();

                      
                            cl.Codigo = Cliente.Codigo.TrimStart().TrimEnd();
                            cl.Cliente = Cliente.Nombre.TrimStart().TrimEnd();
                            cl.Ruc = Cliente.NoCedula.TrimStart().TrimEnd();
                            cl.Plazo = Cliente.Plazo;
                            cl.Correo = Cliente.Correo.TrimStart().TrimEnd();
                            cl.Telefono = string.Concat(Cliente.Celular?.TrimStart().TrimEnd(), "/", Cliente.Telefono1?.TrimStart().TrimEnd(), "/", Cliente.Telefono2?.TrimStart().TrimEnd(), "/", Cliente.Telefono3?.TrimStart().TrimEnd());
                            cl.Limite = Cliente.Limite;
                            cl.Moneda = Cliente.IdMoneda;


                            if(cl.Moneda == cl.MonedaLocal)
                            {
                                cl.Disponible = cl.Limite - (cl.SaldoCordoba + Math.Round( (cl.SaldoDolar * tc), 2));
                            }
                            else
                            {
                                cl.Disponible = cl.Limite - (cl.SaldoDolar + Math.Round((cl.SaldoCordoba / tc), 2));
                            }


                            Sql = $"DECLARE @P_CodCliente NVARCHAR(20) = '{Param}',\r\n@P_Fecha DATETIME = GETDATE()\r\n\r\n\r\nEXEC [CXC].[SP_ReporteEstadoCuentaCliente] '{cl.MonedaLocal}', '{cl.MonedaLocal}', @P_Fecha, @P_CodCliente";

                            Cls_EstadoCuenta[] EstadoCuentaCordoba = _Conexion.Database.SqlQuery<Cls_EstadoCuenta>(Sql).ToArray();

                            Sql = $"DECLARE @P_CodCliente NVARCHAR(20) = '{Param}',\r\n@P_Fecha DATETIME = GETDATE()\r\n\r\n\r\nEXEC [CXC].[SP_ReporteEstadoCuentaCliente] '{cl.MonedaSistema}', '{cl.MonedaSistema}', @P_Fecha, @P_CodCliente";

                            Cls_EstadoCuenta[] EstadoCuentaDolar = _Conexion.Database.SqlQuery<Cls_EstadoCuenta>(Sql).ToArray();

      
                            EstadoCuentaCordoba.Select(c => { c.IdMoneda = cl.MonedaLocal; c.DiasV = (c.Corriente != 0 && c.Debe != 0 ? (DateTime.Now.Date - c.FechaDoc).Days +1 : c.DiasV); return c; }).ToList();
                            EstadoCuentaDolar.Select(c => { c.IdMoneda = cl.MonedaSistema; c.DiasV = (c.Corriente != 0  && c.Debe != 0? (DateTime.Now.Date - c.FechaDoc).Days +1 : c.DiasV) ; return c; }).ToList();


                            List<Cls_EstadoCuenta> EstadoCuenta = new List<Cls_EstadoCuenta>();
                            EstadoCuenta.AddRange(EstadoCuentaCordoba);
                            EstadoCuenta.AddRange(EstadoCuentaDolar);




                            datos.d = new object [] {cl, EstadoCuenta };



                        }



                    }

                   
                    datos.Nombre = Tipo;
                   
                


                    json = Cls_Mensaje.Tojson(datos, 1, string.Empty, string.Empty, 0);
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