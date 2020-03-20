using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;

namespace SAT.WebHandlers
{
    /// <summary>
    /// Descripción breve de AutoCompleta
    /// </summary>
    public class AutoCompleta : IHttpHandler
    {
        /// <summary>
        /// Método encargado de Recibir la Petición Web
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //Obteniendo Termino a Filtrar
            string term = context.Request["term"] ?? "";

            //Obteniendo Tipo de Completacion
            int id = Convert.ToInt32(context.Request.QueryString["id"]);

            //Obteniendo Tipo de Completacion
            string complement1 = context.Request.QueryString["param"];
            string complement2 = context.Request.QueryString["param2"];
            string complement3 = context.Request.QueryString["param3"];
            string complement4 = context.Request.QueryString["param4"];
            string complement5 = context.Request.QueryString["param5"];
            string complement6 = context.Request.QueryString["param6"];
            string complement7 = context.Request.QueryString["param7"];
            string complement8 = context.Request.QueryString["param8"];
            string complement9 = context.Request.QueryString["param9"];

            //Declarando Serializador de JavaScript
            JavaScriptSerializer js = new JavaScriptSerializer();

            //Devolviendo Resultado
            context.Response.Write(js.Serialize(autoCompleta(term, id, complement1, complement2, complement3, complement4, complement5, complement6, complement7, complement8, complement9)));
        }
        /// <summary>
        /// Método encargado de Obtener las Coincidencias del Catalogo
        /// </summary>
        /// <param name="prefix">Prefijo de la Lista</param>
        /// <param name="contextKey">Llave que Indica el Tipo Deseado</param>
        /// <param name="complement1">Complemento de la Busqueda 1</param>
        /// <param name="complement2">Complemento de la Busqueda 2</param>
        /// <param name="complement3">Complemento de la Busqueda 3</param>
        /// <param name="complement4">Complemento de la Busqueda 4</param>
        /// <param name="complement5">Complemento de la Busqueda 5</param>
        /// <param name="complement6">Complemento de la Busqueda 6</param>
        /// <param name="complement7">Complemento de la Busqueda 7</param>
        /// <param name="complement8">Complemento de la Busqueda 8</param>
        /// <param name="complement9">Complemento de la Busqueda 9</param>
        /// <returns></returns>
        public static List<string> autoCompleta(string prefix, int contextKey, string complement1, string complement2, string complement3,
                                                 string complement4, string complement5, string complement6, string complement7,
                                                 string complement8, string complement9)
        {
            //Declarando Objeto de Retorno
            List<string> optionList = new List<string>();

            //Declarando Objeto de Parametro
            object[] param = { contextKey, prefix, complement1, complement2, complement3, complement4, complement5, complement6, complement7, complement8, complement9 };

            //Ejecutando 
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaConsultaSugerencia", param))
            {
                //Validando que exista una tabla
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        //Añadiendo Filas a la lista
                        optionList.Add(dr["descripcion"].ToString());
                }
            }

            //Devolviendo Lista
            return optionList;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}