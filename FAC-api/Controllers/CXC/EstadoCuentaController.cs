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
using ReporteBalance;
using System.Data;
using FastMember;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace FAC_api.Controllers.FACT
{
    public class EstadoCuentaController : ApiController
    {


        [Route("api/CXC/EstadoCuenta/Datos")]
        [HttpGet]
        public string Datos(string Tipo, string Param, bool Permiso)
        {
            return v_Datos(Tipo, Param, Permiso);
        }

        private string v_Datos(string Tipo, string Param, bool Permiso)
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
                                             _q.IdCliente,
                                             Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                             Cliente = _q.Nombre.TrimStart().TrimEnd(),
                                             Ruc = _q.NoCedula.TrimStart().TrimEnd(),
                                             Cedula = _q.NoCedula.TrimStart().TrimEnd(),
                                             Correo = (_q.Correo.TrimStart().TrimEnd() != string.Empty ? _q.Correo.TrimStart().TrimEnd() + ";" : string.Empty) + (_q.Correo2.TrimStart().TrimEnd() != string.Empty ? _q.Correo2.TrimStart().TrimEnd() : string.Empty),
                                             Contacto = string.Concat(_q.Celular.TrimStart().TrimEnd(), "/", _q.Telefono1.TrimStart().TrimEnd(), "/", _q.Telefono2.TrimStart().TrimEnd(), "/", _q.Telefono3.TrimStart().TrimEnd()),
                                             _q.Limite,
                                             Moneda = _q.IdMoneda,
                                             CodVendedor = (_q.Vendedor == null ? string.Empty : _q.Vendedor.TrimStart().TrimEnd()),
                                             EsClave = _q.ClienteClave,
                                             Filtro = string.Concat(_q.Codigo, _q.Nombre.TrimStart().TrimEnd(), _q.NoCedula.TrimStart().TrimEnd(), _q.Telefono1.TrimStart().TrimEnd(), _q.Telefono2.TrimStart().TrimEnd(), _q.Telefono3.TrimStart().TrimEnd(), _q.Celular.TrimStart().TrimEnd()),
                                             Key = string.Concat(_q.Codigo, " ", _q.Nombre.TrimStart().TrimEnd())
                                         }).ToList();

   

                        var qVendedores = (from _q in _Conexion.Vendedor
                                           select new
                                           {
                                               Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                               Vendedor = _q.Nombre.TrimStart().TrimEnd(),
                                               Key = string.Concat(_q.Codigo, " ", _q.Nombre.TrimStart().TrimEnd())
                                           }).ToList();


              



                        var qBodegas = (from _q in _Conexion.Bodegas
                                        orderby _q.Codigo
                                        select new 
                                        {
                                            _q.IdBodega,
                                            Codigo = _q.Codigo.TrimStart().TrimEnd(),
                                            Bodega = _q.Bodega.TrimStart().TrimEnd(),
                                            Key = string.Concat(_q.Codigo.TrimStart().TrimEnd(), " ", _q.Bodega.TrimStart().TrimEnd())
                                        }).ToList();





                        var qListaPrecio = (from _q in _Conexion.ConceptoPrecio
                                        orderby _q.Descripcion
                                        select new
                                        {
                                            _q.IdConceptoPrecio,
                                            Descripcion = _q.Descripcion.TrimStart().TrimEnd()
                                        }).ToList();



                        datos.d = new object[] { qClientes, qVendedores, qBodegas, qListaPrecio };

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


                            cl.IdConceptoPrecio = Cliente.IdConceptoPrecio;
                            cl.CodVendedor = Cliente.Vendedor;
                            cl.CuentaClave = Cliente.ClienteClave;
                            cl.SuspendidoMoroso = Cliente.EsMoroso;
                            cl.ConfianzaFactVencido = Cliente.FacturarVencido;
                            cl.ConfianzaFactSiempre = Cliente.ClienteConfFacSiempre;
                            cl.ConfianzaFactUnaVez = Cliente.FactSiempre;
                            cl.Estado = Cliente.Estado;
                            cl.Observaciones = string.Empty;
                           

                            cl.Bodegas  = (from q in _Conexion.ClienteTienda
                                     join b in _Conexion.Bodegas on q.IdBodega equals b.IdBodega
                                     where q.CodigoCliente == Param
                                     select b.Codigo).ToArray();

                             if(cl.Bodegas.Length > 0)cl.CuentaClave = true;


                            List < ClienteTienda > BOC = _Conexion.ClienteTienda.Where(w => w.CodigoCliente == Param).ToList();


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

                            DataTable table = new DataTable("SP_ReporteEstadoCuentaCliente");
                            DataSet ds = new DataSet();
                            using (var reader = ObjectReader.Create(EstadoCuentaCordoba))
                            {
                                table.Load(reader);
                            }
                            ds.Tables.Add(table);


                            cReporteCartera cReporte = new cReporteCartera();
                            object[] obj = cReporte.EstadoCuenta(ds, cl.Codigo, cl.Cliente, cl.Direccion, cl.Limite, "Cordoba", cl.Ruc, cl.UltimoRoc, cl.UltimoRocFecha, cl.UltimoRocMonto, cl.Telefono, Permiso);


                            if (obj[0].ToString() != string.Empty)
                            {
                                return json = Cls_Mensaje.Tojson(null, 0, "1", obj[0].ToString(), 1);
                            }

                            table = new DataTable("SP_ReporteEstadoCuentaCliente");
                            ds = new DataSet();
                            using (var reader = ObjectReader.Create(EstadoCuentaDolar))
                            {
                                table.Load(reader);
                            }
                            ds.Tables.Add(table);
                            object[] obj2 = cReporte.EstadoCuenta(ds, cl.Codigo, cl.Cliente, cl.Direccion, cl.Limite, "Dolares", cl.Ruc, cl.UltimoRoc, cl.UltimoRocFecha, cl.UltimoRocMonto, cl.Telefono, Permiso);



                            if (obj2[0].ToString() != string.Empty)
                            {
                                return json = Cls_Mensaje.Tojson(null, 0, "1", obj2[0].ToString(), 1);
                            }




                            datos.d = new object [] {cl, EstadoCuenta, obj[1], obj2[1] };



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



        [Route("api/CXC/EstadoCuenta/GuardarPermiso")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult GuardarPermiso(Cls_ClienteCartera d)
        {
            if (ModelState.IsValid)
            {

                return Ok(V_GuardarPermiso(d));

            }
            else
            {
                return BadRequest();
            }

        }

        private string V_GuardarPermiso(Cls_ClienteCartera d)
        {

            string json = string.Empty;

            try
            {

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    using (BalancesEntities _Conexion = new BalancesEntities())
                    {



                        Cliente cl = _Conexion.Cliente.FirstOrDefault(f => f.Codigo == d.Codigo);
                        Usuarios u = _Conexion.Usuarios.FirstOrDefault(f => f.Usuario == d.Usuario);

                        cl.Limite = d.Limite;
                        cl.Plazo = d.Plazo;
                        cl.IdMoneda = d.Moneda;
                        cl.Vendedor = d.CodVendedor;
                        cl.IdConceptoPrecio = (int)d.IdConceptoPrecio;
                        cl.ClienteClave = d.CuentaClave;
                        cl.EsMoroso = (bool)d.SuspendidoMoroso;
                        cl.FacturarVencido = d.ConfianzaFactVencido;
                        cl.ClienteConfFacSiempre = (bool)d.ConfianzaFactSiempre;
                        cl.FactSiempre = (bool)d.ConfianzaFactUnaVez;
                        cl.Estado = d.Estado;
                        cl.IdUsuarioModifica = u.IdUsuario;
                        cl.FechaModificacion = DateTime.Now;

                        _Conexion.SaveChanges();


                        d.Bodegas.ForEach(f =>
                        {
                            Bodegas b = _Conexion.Bodegas.FirstOrDefault(w => w.Codigo == f);

                            if(b != null)
                            {
                                ClienteTienda ct = _Conexion.ClienteTienda.FirstOrDefault(w => w.IdBodega == b.IdBodega);

                                if(ct == null)
                                {
                                    ct = new ClienteTienda();
                                    ct.CodigoCliente = d.Codigo;
                                    ct.IdBodega = b.IdBodega;
                                    ct.FechaAsignacion = DateTime.Now;
                                    ct.CodigoCliente = d.Codigo;
                                    ct.IdUsuarioCrea = u.IdUsuario;
                                    ct.Activo = true;
                                    _Conexion.SaveChanges();
                                }
                            }

                            
                        });

                     

                        foreach(ClienteTienda t in _Conexion.ClienteTienda.Where(w => w.CodigoCliente == cl.Codigo))
                        {
                            Bodegas b = _Conexion.Bodegas.FirstOrDefault(w => w.IdBodega == t.IdBodega);

                            if(!d.Bodegas.Contains(b.Codigo))
                            {
                                t.Activo = false;
                                t.IdUsuarioInactiva = u.IdUsuario;
                                t.FechaCierre = DateTime.Now;
                                _Conexion.SaveChanges();
                            }

                        }



                        Anotaciones an = new Anotaciones();
                        an.CodCliente = cl.Codigo;
                        an.Nota = d.Observaciones;
                        an.Fecha = (DateTime) cl.FechaModificacion;
                        an.IdUsuario = u.IdUsuario;
                        an.Hora = string.Format("{0:hh:mm: tt}", cl.FechaModificacion);
                        _Conexion.Anotaciones.Add(an);
                        _Conexion.SaveChanges();


                        _Conexion.Database.ExecuteSqlCommand($"\tDECLARE @P_IdConceptoPrecio INT = {cl.IdConceptoPrecio},\r\n\t@Distribuidor BIT,\r\n\t@PrecioLista NVARCHAR(2),\r\n\t@P_IdMoneda NVARCHAR(10),\r\n\t@ModenaInv NVARCHAR(2)\r\n\r\n\r\n\r\nIF @P_IdConceptoPrecio = 8 \r\nBEGIN\r\n\t\tset @Distribuidor = 1\r\n\t\tset @PrecioLista = 1\r\n\t\t\t\t\t\r\nEND\r\nELSE\r\n\tBEGIN\r\n\t\tset @PrecioLista = (select Codigo from FAC.ConceptoPrecio where IdConceptoPrecio = @P_IdConceptoPrecio)\r\n\t\tset @Distribuidor = 0\r\n\tEND\r\nIF @P_IdMoneda = 'DOL'\r\n\tBEGIN\r\n\t\tSET @ModenaInv = 'D'\r\n\tEND\r\nELSE\r\n\tBEGIN \r\n\t\tSET @ModenaInv = 'C'\r\n\tEND\r\n\r\n\r\n\t\t\t\r\nUPDATE INVESCASAN..Clientes\r\nSET   \t\t\t\t\t\r\n[Plazo] = {d.Plazo}\r\n,[vendedor] = ''\r\n,[techo] = {d.Limite}\r\n,[facturarvencido] = {(d.ConfianzaFactVencido == true ? 1 : 0)}\r\n,[clave] = {(d.CuentaClave == true ? 1 : 0)}\r\n,[Moneda] = @ModenaInv\r\n,[Lista] = @PrecioLista\t\t\r\nWHERE  [codcta] = '{d.Codigo}'");

                        _Conexion.SaveChanges();

                        Cls_Datos datos = new Cls_Datos();
                        datos.Nombre = "GUARDAR";
                        datos.d = "<b>Registro Guardado<b/>";
              



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