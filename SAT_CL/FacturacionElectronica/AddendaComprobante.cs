using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase encargada de Definir Estados y Comportamiento
    /// </summary>
    public class AddendaComprobante : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string nom_sp = "fe.sp_addenda_comprobante_tac";

        private int _id_addenda_comprobante;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Comprobante de la Addenda
        /// </summary>
        public int id_addenda_comprobante
        {
            get { return this._id_addenda_comprobante; }
        }

        private int _id_emisor_addenda;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Addenda
        /// </summary>
        public int id_emisor_addenda
        {
            get { return this._id_emisor_addenda; }
        }

        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Comprobante
        /// </summary>
        public int id_comprobante
        {
            get { return this._id_comprobante; }
        }

        private int _id_comprobante33;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Comprobante (CFDI3.3)
        /// </summary>
        public int id_comprobante33
        {
            get { return this._id_comprobante33; }
        }

        private XmlDocument _xml_addenda;
        /// <summary>
        /// Atributo encargado de almacenar el XML de la Addenda
        /// </summary>
        public XmlDocument xml_addenda
        {
            get { return this._xml_addenda; }
        }

        private bool _bit_validacion;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Validación
        /// </summary>
        public bool bit_validacion
        {
            get { return this._bit_validacion; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor encargado de Iniciar los Atributos por default
        /// </summary>
        public AddendaComprobante()
        {
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Iniciar los Atributos en base a un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public AddendaComprobante(int id_registro)
        {
            cargaAtributosInstancia(id_registro);
        }
        /// <summary>
        /// Constructor encargado de Iniciar los Atributos en base a un Registro con Transacción SQL
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="trans">Transacción SQL</param>
        public AddendaComprobante(int id_registro, SqlTransaction trans)
        {
            cargaAtributosInstancia(id_registro, trans);
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor de La Clase
        /// </summary>
        ~AddendaComprobante()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado encargado de cargar los valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
            this._id_addenda_comprobante = 0;
            this._id_emisor_addenda = 0;
            this._id_comprobante = 0;
            this._id_comprobante33 = 0;
            this._xml_addenda = null;
            this._bit_validacion = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de cargar los valores en base a un Id de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando variable de retorno
            bool result = false;
            //Declarando parametros del SP
            object[] param = { 3, id_registro, 0, 0, 0, "", false, 0, false, "", "" };
            //Creando DataSet para almacenar el Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando que el DataSet contenga registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Por cada fila en la Tabla dentro del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores a los Atributos
                        this._id_addenda_comprobante = id_registro;
                        this._id_emisor_addenda = Convert.ToInt32(dr["IdAddendaEmisor"]);
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_comprobante33 = Convert.ToInt32(dr["IdComprobante33"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLAddenda"].ToString());
                        this._xml_addenda = xmldoc;
                        this._bit_validacion = Convert.ToBoolean(dr["Validacion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Método Privado encargado de cargar los valores en base a un Id de registro con Transaccion SQL
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="trans">Transacción</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro, SqlTransaction trans)
        {   //Declarando variable de retorno
            bool result = false;
            //Declarando parametros del SP
            object[] param = { 3, id_registro, 0, 0, 0, null, false, 0, false, "", "" };
            //Creando DataSet para almacenar el Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param, trans))
            {   //Validando que el DataSet contenga registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Por cada fila en la Tabla dentro del DataSet
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores a los Atributos
                        this._id_addenda_comprobante = id_registro;
                        this._id_emisor_addenda = Convert.ToInt32(dr["IdAddendaEmisor"]);
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_comprobante33 = Convert.ToInt32(dr["IdComprobante33"]);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(dr["XMLAddenda"].ToString());
                        this._xml_addenda = xmldoc;
                        this._bit_validacion = Convert.ToBoolean(dr["Validacion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Editar el Registro
        /// </summary>
        /// <param name="id_emisor_addenda">Id de Emisor Addenda</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        /// <param name="xml">XML de Validación</param>
        /// <param name="bit_validacion">Estatus de validacion</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarAddendaComprobante(int id_emisor_addenda, int id_comprobante, int id_comprobante33,
                                                          XmlDocument xml, bool bit_validacion, int id_usuario, bool habilitar)
        {   //Declarando variable de Retorno
            RetornoOperacion result;
            
            //Declarando parametros del SP
            object[] param = { 2, this._id_addenda_comprobante, id_emisor_addenda, id_comprobante, id_comprobante33,
                             xml.InnerXml, bit_validacion, id_usuario, habilitar, "", ""};

            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Regresando Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Editar el Registro sin Validación de Comprobante Generado
        /// </summary>
        /// <param name="xml">XML de Validación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        private RetornoOperacion editarAddendaComprobante(XmlDocument xml, int id_usuario)
        {   //Declarando variable de Retorno
            RetornoOperacion result;
            //Declarando parametros del SP
            object[] param = { 2, this._id_addenda_comprobante, this.id_emisor_addenda, this.id_comprobante,
                               this._id_comprobante33, xml.InnerXml, this.bit_validacion, id_usuario, this._habilitar, "", ""};
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Regresando Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método Público encargado de la Insercción de Registros
        /// </summary>
        /// <param name="id_emisor_addenda">Id de Emisor de Addenda</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        /// <param name="xml">XML de Validación</param>
        /// <param name="bit_validacion">Estatus de validacion</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion IngresarAddendaComprobante(int id_emisor_addenda, int id_comprobante, int id_comprobante33,
                                                                  XmlDocument xml, bool bit_validacion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result;
            //Declarando Parametros del SP
            object[] param = { 1, 0, id_emisor_addenda, id_comprobante, id_comprobante33, xml.InnerXml, bit_validacion, id_usuario, true, "", "" };
            
            //Obteniendo resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Regresando Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Publico Encargado de la Edición de campos dado un Id de Registro
        /// </summary>
        /// <param name="id_emisor_addenda">Id de Emisor de Addenda</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        /// <param name="xml">XML de Validación</param>
        /// <param name="bit_validacion">Estatus de validacion</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarAddendaComprobante(int id_emisor_addenda, int id_comprobante, int id_comprobante33,
                                                         XmlDocument xml, bool bit_validacion, int id_usuario)
        {
            return this.editarAddendaComprobante(id_emisor_addenda, id_comprobante, id_comprobante33, xml, bit_validacion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Publico Encargado de la Edición sin Validación de Comprobante Timbrado
        /// </summary>
        /// <param name="xml">XML de Validación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarAddendaComprobante(XmlDocument xml, int id_usuario)
        {
            return this.editarAddendaComprobante(xml, id_usuario);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAddendaComprobante(int id_usuario)
        {
            return this.editarAddendaComprobante(this._id_emisor_addenda, this._id_comprobante, this._id_comprobante33,
                                                 this._xml_addenda, this._bit_validacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public bool ActualizarAddendaComprobante(int id_registro)
        {
            return cargaAtributosInstancia(id_registro);
        }

        #region CFDI v3.2

        /// <summary>
        /// Recupera los registros addenda del tipo y comprobante solicitado
        /// </summary>
        /// <param name="id_addenda_emisor">Id de relación Addenda, Emisor y Receptor (0 para no considerar filtro)</param>
        /// <param name="id_comprobante">Id de Comprobante (obligatorio)</param>
        /// <returns></returns>
        public static DataTable CargaAddendaEmisorComprobante(int id_addenda_emisor, int id_comprobante)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializando parámetros de consulta
            object[] param = { 4, 0, id_addenda_emisor, id_comprobante, 0, "", false, 0, false, "", "" };
            //Realizando consulta a base de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Añadiendo tabla encontrada
                    mit = ds.Tables["Table"];
                }
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Recupera el objeto addenda del tipo y comprobante solicitado
        /// </summary>
        /// <param name="id_addenda_emisor">Id de relación Addenda, Emisor y Receptor</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static AddendaComprobante RecuperaAddendaComprobante(int id_addenda_emisor, int id_comprobante)
        {   //Definiendo objeto de resultado
            AddendaComprobante addenda = new AddendaComprobante();
            //Realizando consulta a base de datos
            using (DataTable mit = CargaAddendaEmisorComprobante(id_addenda_emisor, id_comprobante))
            {   //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(mit))
                {   //Para cada registro devuelto
                    foreach (DataRow r in mit.Rows)
                    {   //Asignando atributos de registo
                        addenda = new AddendaComprobante(Convert.ToInt32(r["IdAddendaComprobante"]));
                    }
                }
            }
            //Devolviendo resultado
            return addenda;
        }
        /// <summary>
        /// Realiza la deshabilitación de los registros addenda ligados al comprobante indicado
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public static RetornoOperacion EliminaAddendasComprobante(int id_comprobante, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_comprobante);
            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando Addendas del comprobante
                using (DataTable mit = CargaAddendaEmisorComprobante(0, id_comprobante))
                {
                    //Si existen registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las addendas capturadas
                        foreach (DataRow a in mit.Rows)
                        {
                            //Instanciando registros
                            using (AddendaComprobante ac = new AddendaComprobante(Convert.ToInt32(a["IdAddendaComprobante"])))
                            {
                                //Realziando deshabilitación
                                resultado = ac.DeshabilitaAddendaComprobante(id_usuario);

                                //Si existe error
                                if (!resultado.OperacionExitosa)
                                    //Saliendo de ciclo
                                    break;
                            }
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region CFDI v3.3

        /// <summary>
        /// Recupera los registros addenda del tipo y comprobante solicitado
        /// </summary>
        /// <param name="id_addenda_emisor">Id de relación Addenda, Emisor y Receptor (0 para no considerar filtro)</param>
        /// <param name="id_comprobante">Id de Comprobante (obligatorio)</param>
        /// <returns></returns>
        public static DataTable CargaAddendaEmisorComprobanteV33(int id_addenda_emisor, int id_comprobante33)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializando parámetros de consulta
            object[] param = { 4, 0, id_addenda_emisor, 0, id_comprobante33, "", false, 0, false, "", "" };
            //Realizando consulta a base de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Añadiendo tabla encontrada
                    mit = ds.Tables["Table"];
                }
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Recupera el objeto addenda del tipo y comprobante solicitado
        /// </summary>
        /// <param name="id_addenda_emisor">Id de relación Addenda, Emisor y Receptor</param>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static AddendaComprobante RecuperaAddendaComprobanteV33(int id_addenda_emisor, int id_comprobante)
        {   //Definiendo objeto de resultado
            AddendaComprobante addenda = new AddendaComprobante();
            //Realizando consulta a base de datos
            using (DataTable mit = CargaAddendaEmisorComprobanteV33(id_addenda_emisor, id_comprobante))
            {   //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(mit))
                {   //Para cada registro devuelto
                    foreach (DataRow r in mit.Rows)
                    {   //Asignando atributos de registo
                        addenda = new AddendaComprobante(Convert.ToInt32(r["IdAddendaComprobante"]));
                    }
                }
            }
            //Devolviendo resultado
            return addenda;
        }
        /// <summary>
        /// Realiza la deshabilitación de los registros addenda ligados al comprobante indicado
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public static RetornoOperacion EliminaAddendasComprobanteV33(int id_comprobante, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_comprobante);
            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando Addendas del comprobante
                using (DataTable mit = CargaAddendaEmisorComprobanteV33(0, id_comprobante))
                {
                    //Si existen registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las addendas capturadas
                        foreach (DataRow a in mit.Rows)
                        {
                            //Instanciando registros
                            using (AddendaComprobante ac = new AddendaComprobante(Convert.ToInt32(a["IdAddendaComprobante"])))
                            {
                                //Realziando deshabilitación
                                resultado = ac.DeshabilitaAddendaComprobante(id_usuario);

                                //Si existe error
                                if (!resultado.OperacionExitosa)
                                    //Saliendo de ciclo
                                    break;
                            }
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #endregion
    }
}