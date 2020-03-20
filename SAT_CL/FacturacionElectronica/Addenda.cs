using System;
using System.Data;
using System.Transactions;
using System.Xml;
using System.Xml.Schema;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Proporciona los Comportamientos y Estados de la Clase Addenda
    /// </summary>
    public class Addenda : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración encargada de Identificar el Elemento del Comprobante
        /// </summary>
        public enum ElementoComprobante
        {
            /// <summary>
            /// Elemento de Tipo Addenda
            /// </summary>
            Addenda = 1,
            /// <summary>
            /// Elemento de Tipo Complemento
            /// </summary>
            Complemento
        }

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo encargado de almacenar el nombre del SP
        /// </summary>
        private static string nombre_store_procedure = "fe.sp_addenda_ta";

        private int _id_addenda;
        /// <summary>
        /// Atributo para almacenar el Id de la Addenda
        /// </summary>
        public int id_addenda
        {
            get { return this._id_addenda; }
        }
        private int _id_elemento_comprobante;
        /// <summary>
        /// Atributo para almacenar el Id del Elemento Comprobante
        /// </summary>
        public int id_elemento_comprobante
        {
            get { return this._id_elemento_comprobante; }
        }
        private string _descripcion;
        /// <summary>
        /// Atributo para almacenar la Descripción de la Addenda
        /// </summary>
        public string descripcion
        {
            get { return this._descripcion; }
        }
        private XmlDocument _xsd_validation;
        /// <summary>
        /// Atributo para almacenar el XSD de Validacion
        /// </summary>
        public XmlDocument xsd_validation
        {
            get { return this._xsd_validation; }
        }
        private bool _habilitar;
        /// <summary>
        /// Atributo para almacenar el Estatus Habilitado
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Contructor que carga los atributos por default
        /// </summary>
        public Addenda()
        {   //Invoca metodo de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Contructor que carga los atributos 
        /// </summary>
        /// <param name="id_registro">Id de Addenda</param>
        public Addenda(int id_registro)
        {   //Invoca metodo de carga con registro
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Addenda()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado encargado de Cargar los Atributos vacios
        /// </summary>
        /// <returns></returns>
        private void cargaAtributosInstancia()
        {   //Cargando atributos vacios
            _id_addenda = 0;
            _id_elemento_comprobante = 0;
            _descripcion = "";
            _xsd_validation = null;
            _habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Variable para declarar resultado
            bool result = false;
            //Se cargan los parametros
            object[] parametros = { 3, id_registro, 
                                    0, "", null, 0, true, "", ""};
            //Se ejecuta el Store Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, parametros))
            {   //Se valida el Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {   //Por cada Fila de la tabla "Table" del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Cargando los Registros en los Atributos
                        this._id_addenda = id_registro;
                        this._id_elemento_comprobante = Convert.ToInt32(dr["IdElementoComprobante"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        XmlDocument xmldoc = new XmlDocument();
                        XmlSchema schema = new XmlSchema();
                        //schema.Write(dr["XSDValidacion"].ToString());
                        //(new StringReader(dr["XSDValidacion"].ToString()));
                        xmldoc.LoadXml(dr["XSDValidacion"].ToString());


                        this._xsd_validation = xmldoc;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Método Privado encargado de la Actualizacion de Registros
        /// </summary>
        /// <param name="id_elemento_comprobante">Id de Elemento Comprobante</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="xsd_validation">XSD de Validación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaAddenda(int id_elemento_comprobante, 
                                    string descripcion, XmlDocument xsd_validation,
                                    int id_usuario, bool habilitar)
        {   //Se cargan los parametros
            object[] parametros = { 2, this._id_addenda,
                                    id_elemento_comprobante, descripcion, 
                                    xsd_validation.InnerXml, id_usuario, habilitar, 
                                    "", ""};
            //Regresa el Resultado del SP 
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto
                                    (nombre_store_procedure, parametros);
        }

        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método Público encargado de Actualizar Registros
        /// </summary>
        /// <param name="id_addenda"></param>
        /// <returns></returns>
        public bool ActualizaAddenda(int id_addenda)
        {   //Regresa invocacion del método cargaAtributoInstancia
            return this.cargaAtributosInstancia(id_addenda);
        }
        /// <summary>
        /// Método Público encargado de Ingresar Registros
        /// </summary>
        /// <param name="id_elemento_comprobante"></param>
        /// <param name="descripcion"></param>
        /// <param name="xsd_validation"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAddenda
                                    (int id_elemento_comprobante, string descripcion,
                                    XmlDocument xsd_validation, int id_usuario
                                    )
        {   //Se declara la variable de retorno
            RetornoOperacion result = new RetornoOperacion();
            //Se cargan los parametros
            object[] parametros = { 1, null, 
                                    id_elemento_comprobante, descripcion, 
                                    xsd_validation.InnerXml, id_usuario, true, 
                                    "", ""};
            return result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto
                                    (nombre_store_procedure, parametros);
        }
        /// <summary>
        /// Método Público encargado de Editar los Registros
        /// </summary>
        /// <param name="id_elemento_comprobante"></param>
        /// <param name="descripcion"></param>
        /// <param name="xsd_validation"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditarAddenda(int id_elemento_comprobante,
                                    string descripcion,
                                    XmlDocument xsd_validation, int id_usuario)
        {
            return this.actualizaAddenda(id_elemento_comprobante, descripcion,
                                    xsd_validation, id_usuario,
                                    this._habilitar);
        }

   
        /// <summary>
        /// Metodo encargado de Deshabilitar una Addenda ligado a una transaccion
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAddenda(int id_usuario)
        {
            //Inicializamos objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos Addenda Emisores
                using (DataTable mit = AddendaEmisor.CargaAddendaEmisor(this._id_addenda))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Obtenemos cada uno de los Registros
                        foreach (DataRow r in mit.Rows)
                        {
                            //Si se actualizo correctamente
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos cada uno de las Addendas Emisores
                                using (AddendaEmisor objAddendaEmisor = new AddendaEmisor(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Registros
                                    resultado = objAddendaEmisor.DeshabilitaAddendaEmisor(id_usuario);

                                }
                            }
                            else
                            {
                                //Salimos del ciclo
                                break;
                            }
                        }
                    }
                }
                //Si las Actualizaciones fueron Correctas
                if (resultado.OperacionExitosa)
                {
                    //Deshabilitamos Addenda
                    resultado = this.actualizaAddenda(this._id_elemento_comprobante, this._descripcion, this._xsd_validation, id_usuario,
                                                     false);
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
                    
            return resultado;
        }

        #endregion
    }

}