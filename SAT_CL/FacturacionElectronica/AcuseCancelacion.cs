using System;
using System.Data;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Proporciona los medios para la administración de Acuses de cancelación de Comprobantes
    /// </summary>
    public class AcuseCancelacion : Disposable
    {
        #region Enumeraciones

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del 
        /// </summary>
        private static string _nombre_stored_procedure = "fe.sp_acuse_cancelacion_tac";

        private int _id_acuse_cancelacion;
        /// <summary>
        /// Id de Acuse de Cancelación
        /// </summary>
        public int id_acuse_cancelacion { get { return this._id_acuse_cancelacion; } }
        private string _estatus;
        /// <summary>
        /// Estatus del Acuse
        /// </summary>
        public string estatus { get { return this._estatus; } }
        private DateTime _fecha;
        /// <summary>
        /// Fecha de Acuse
        /// </summary>
        public DateTime fecha { get { return this._fecha; } }
        private XDocument _acuse_xml;
        /// <summary>
        /// Acuse de cancelación en formato XML
        /// </summary>
        public XDocument acuse_xml { get { return this._acuse_xml; } }
        private bool _habilitar;
        /// <summary>
        /// Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva instancia del tipo AcuseCancelacion
        /// </summary>
        private AcuseCancelacion()
        {

        }
        /// <summary>
        /// Crea una nueva instancia del tipo AcuseCancelacion
        /// </summary>
        /// <param name="id_acuse_cancelacion">Id de Acuse de Cancelación</param>
        private AcuseCancelacion(int id_acuse_cancelacion)
        {
            cargaAtributosInstancia(id_acuse_cancelacion);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~AcuseCancelacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Refresca los valores de cada atributo de la instancia
        /// </summary>
        /// <param name="id_acuse_cancelacion">Id de acuse de cancelación</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_acuse_cancelacion)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicialzaindo parámetros de consulta
            object[] param = { 3, id_acuse_cancelacion, "", null, "", 0, false, "", "" };

            //Realizando carga de datos del registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {
                    //Para cada unos de los registros devueltos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores de atributos
                        this._id_acuse_cancelacion = Convert.ToInt32(r["IdAcuseCancelacion"]);
                        this._estatus = r["Estatus"].ToString();
                        this._fecha = Convert.ToDateTime(r["Fecha"]);
                        this._acuse_xml = XDocument.Parse(r["AcuseXML"].ToString());
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Indicando retorno correcto
                    resultado = true;
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la edición del registro
        /// </summary>
        /// <param name="estatus">Estatus de Acuse</param>
        /// <param name="fecha">Fecha de Acuse</param>
        /// <param name="acuse_xml">Acuse XML</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaAcuseCancelacion(string estatus, DateTime fecha, XDocument acuse_xml, int id_usuario, bool habilitar)
        {
            //Inicializando parámetros de actualización
            object[] param = { 2, this._id_acuse_cancelacion, estatus, fecha, acuse_xml.ToString(), id_usuario, habilitar, "", "" };

            //Realziando actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inserta un nuevo registro Acuse de Cancelación en BD
        /// </summary>
        /// <param name="estatus">Estatus de Acuse</param>
        /// <param name="fecha">Fecha de Acuse</param>
        /// <param name="acuse_xml">Acuse XML</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAcuseCancelacion(string estatus, DateTime fecha, XDocument acuse_xml, int id_usuario)
        {
            //Inicializando parámetros para insertar nuevo registro
            object[] param = { 1, 0, estatus, fecha, acuse_xml == null ? "" : acuse_xml.ToString(), id_usuario, true, "", "" };

            //insertando registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Realiza la edición de los campos de un registro Acuse de Cancelación
        /// </summary>
        /// <param name="estatus">Estatus del Acuse</param>
        /// <param name="fecha">Fecha del Acuse</param>
        /// <param name="acuse_xml">Acuse XML</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaAcuseCancelacion(string estatus, DateTime fecha, XDocument acuse_xml, int id_usuario)
        {
            return editaAcuseCancelacion(estatus, fecha, acuse_xml, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Realiza la deshabilitación de un registro Acuse de Cancelación
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAcuseCancelacion(int id_usuario)
        {
            return editaAcuseCancelacion(this._estatus, this._fecha, this._acuse_xml, id_usuario, false);
        }


        #endregion

    }
}
