using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase encargada de 
    /// </summary>
    public class ProveedorWSCargaParametros
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "monitoreo.sp_proveedor_ws_cargaParametros";

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Obtener los Parametros del Web Service del Proveedor
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor del Servicio Web</param>
        /// <returns></returns>
        public static DataTable ObtieneParametrosProveedorWS(int id_proveedor_ws)
        {
            //Declarando Objeto de Retorno
            DataTable dtParams = null;

            //Armando Arreglo de Parametros
            object[] param = { id_proveedor_ws, "", "", "", "" };

            //Obteniendo Parametros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor
                    dtParams = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtParams;
        }
        /// <summary>
        /// Método encargado de Obtener los Parametros del Web Service del Proveedor en Formato XML
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor del Servicio Web</param>
        /// <returns></returns>
        public static XDocument ObtieneXMLParametrosProveedorWS(int id_proveedor_ws)
        {
            //Declarando Objeto de Retorno
            XDocument document;

            //Instanciando Proveedor de Tipo de Servicio
            using (ProveedorWS pro = new ProveedorWS(id_proveedor_ws))
            
            //Instanciando Parametros
            using (DataTable dt = ObtieneParametrosProveedorWS(id_proveedor_ws))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Gestionando Resultado
                    DataTable dtParametros = dt.Copy();
                    dtParametros.TableName = pro.accion;                    
                    
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtParametros.WriteXml(s);

                        //Obteniendo DataSet en XML
                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                        //Añadiendo Elemento al nodo de Publicaciones
                        document = new XDocument(dataTableElement.Element(pro.accion));
                    }
                }
                else
                    //Añadiendo Root
                    document = new XDocument(new XElement(pro.accion));
            }

            //Devolviendo Resultado Obtenido
            return document;
        }

        #endregion
    }
}
