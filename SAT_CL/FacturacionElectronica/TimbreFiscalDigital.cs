using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase que define Comportamientos y Estados
    /// </summary>
    public class TimbreFiscalDigital : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_timbre_fiscal_digital_ttf";

        private int _id_timbre_fiscal_digital;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Id de Timber Fiscal
        /// </summary>
        public int id_timbre_fiscal_digital
        {
            get { return this._id_timbre_fiscal_digital; }
        }
        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Id del Comprobante
        /// </summary>
        public int id_comprobante
        {
            get { return this._id_comprobante; }
        }

        private int _id_compania_proveedor;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Id del Comprobante
        /// </summary>
        public int id_compania_proveedor
        {
            get { return this._id_compania_proveedor; }
        }
        private string _version;
        /// <summary>
        /// Atributo encargado de Obtener el Valor de la Versión
        /// </summary>
        public string version
        {
            get { return this._version; }
        }

        private string _UUID;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Folio
        /// </summary>
        public string UUID
        {
            get { return this._UUID; }
        }

        private DateTime _fecha_timbrado;
        /// <summary>
        /// Atributo encargado de Obtener el Valor de la Fecha de Timbrado
        /// </summary>
        public DateTime fecha_timbrado
        {
            get { return this._fecha_timbrado; }
        }

        private string _sello_CFD;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Sello CFD
        /// </summary>
        public string sello_CFD
        {
            get { return this._sello_CFD; }
        }

        private string _no_certificado;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Número de Certificado
        /// </summary>
        public string no_certificado
        {
            get { return this._no_certificado; }
        }

        private string _sello_SAT;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Sello SAT
        /// </summary>
        public string sello_SAT
        {
            get { return this._sello_SAT; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Obtener el Valor del Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }



        #endregion

        #region Constructores
        /// <summary>
        /// Contructor que Inicializa los Atributos por Default
        /// </summary>
        public TimbreFiscalDigital()
        {   //Invoca Metodo de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Contructor que Inicializa los Atributos con un Id
        /// </summary>
        /// <param name="id">Id de Registro</param>
        public TimbreFiscalDigital(int id)
        {   //Invoca Metodo de Carga
            cargaAtributosInstancia(id);
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TimbreFiscalDigital()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado que se encarga de Inicializar los Valores Por Default
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Cargando Atributos por Default
            this._id_timbre_fiscal_digital = 0;
            this._id_comprobante = 0;
            this._id_compania_proveedor = 0;
            this._version = "";
            this._UUID = "";
            this._fecha_timbrado = DateTime.MinValue;
            this._sello_CFD = "";
            this._no_certificado = "";
            this._sello_SAT = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado que se encarga de Inicializar los Valores
        /// </summary>
        /// <param name="id">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id)
        {   //Declarando el parametro de Retorno
            bool result = false;
            //Declarando parametros
            object[] param = { 3, id, 0, 0,"", "", null, "", "", "", 0, false, "", "" };
            //Obteniendo Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {   //Por cada Fila Encontrada
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores a los Atributos
                        this._id_timbre_fiscal_digital = id;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._version = dr["Version"].ToString();
                        this._UUID = dr["UUID"].ToString();
                        DateTime.TryParse(dr["FechaTimbrado"].ToString(), out this._fecha_timbrado);
                        this._sello_CFD = dr["SelloCFD"].ToString();
                        this._no_certificado = dr["NoCertificado"].ToString();
                        this._sello_SAT = dr["SelloSAT"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Método Privado Encargado de Editar Atributos
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_compania_proveedor">Id Compañia que realizó el timbrado</param>
        /// <param name="version">Version</param>
        /// <param name="UUID">Folio Unico</param>
        /// <param name="fecha_timbrado">Fecha de Timbrado</param>
        /// <param name="sello_CFD">Sello CFD</param>
        /// <param name="no_certificado">No de Certificado</param>
        /// <param name="sello_SAT">Sello SAT</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaAtributos(int id_comprobante,int id_compania_proveedor, string version, string UUID,
                                    DateTime fecha_timbrado, string sello_CFD, string no_certificado,
                                    string sello_SAT, int id_usuario, bool habilitar)
        {   //Declarando parametros
            object[] param = { 2, this._id_timbre_fiscal_digital, id_comprobante,  id_compania_proveedor, version, UUID, 
                                        fecha_timbrado, sello_CFD, no_certificado, 
                                        sello_SAT, id_usuario, habilitar, "", "" };
            //Declarando Variable de Retorno
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retornando Variable con Resultado
            return result;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <param name="id">Id de Registro</param>
        /// <returns></returns>
        public bool ActualizaTimbreFiscal(int id)
        {
            return this.cargaAtributosInstancia(id);
        }
        /// <summary>
        /// Método Público encargado de Insertar Registros
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_compania_proveedor">Id Compañia que realizó el timbrado (PAC)</param>
        /// <param name="version">Version</param>
        /// <param name="UUID">Folio Unico</param>
        /// <param name="fecha_timbrado">Fecha de Timbrado</param>
        /// <param name="sello_CFD">Sello CFD</param>
        /// <param name="no_certificado">No de Certificado</param>
        /// <param name="sello_SAT">Sello SAT</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarTimbreFiscal(int id_comprobante, int id_compania_proveedor, string version, string UUID,
                                    DateTime fecha_timbrado, string sello_CFD, string no_certificado,
                                    string sello_SAT, int id_usuario)
        {   //Declarando parametros
            object[] param = { 1, 0, id_comprobante, id_compania_proveedor, version, UUID, 
                                        fecha_timbrado, sello_CFD, no_certificado, 
                                        sello_SAT, id_usuario, true, "", "" };
            //Declarando Variable de Retorno
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retornando Variable con Resultado
            return result;
        }

        /// <summary>
        /// Método Público encargado de la Edición de Atributos
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="id_compania_proveedor">Id Compañia que realizó el timbrado (PAC)</param>
        /// <param name="version">Version</param>
        /// <param name="UUID">Folio Unico</param>
        /// <param name="fecha_timbrado">Fecha de Timbrado</param>
        /// <param name="sello_CFD">Sello CFD</param>
        /// <param name="no_certificado">No de Certificado</param>
        /// <param name="sello_SAT">Sello SAT</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTimbreFiscal(int id_comprobante, int id_compania_proveedor, string version, string UUID,
                                    DateTime fecha_timbrado, string sello_CFD, string no_certificado,
                                    string sello_SAT, int id_usuario)
        {   //Retorno de Resultado
            return this.editaAtributos(id_comprobante, id_compania_proveedor, version, UUID,
                                        fecha_timbrado, sello_CFD, no_certificado,
                                        sello_SAT, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTimbreFiscal(int id_usuario)
        {   //Retorno de Resultado
            return this.editaAtributos(this._id_comprobante, this._id_compania_proveedor, this._version, this._UUID,
                                        this._fecha_timbrado, this._sello_CFD, this._no_certificado,
                                        this._sello_SAT, id_usuario, false);
        }

        /// <summary>
        /// Realiza la carga de los registros ligados a un Id de comprobante
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static DataTable CargaTimbresFiscalesComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parámetros de consulta de registros
            object[] param = { 4, 0, id_comprobante, 0, "", "", null, "", "", "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Busca e instancía el timbre fiscal digital del comprobante solicitado
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static TimbreFiscalDigital RecuperaTimbreFiscalComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            TimbreFiscalDigital timbre = new TimbreFiscalDigital();
            //Realizando la carga de los timbres del comprobante
            using (DataTable mit = CargaTimbresFiscalesComprobante(id_comprobante))
            {

                //Si el origen es válido
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Para cada uno de los registros
                    foreach (DataRow r in mit.Rows)
                    {
                        timbre = new TimbreFiscalDigital(Convert.ToInt32(r["IdTimbreFiscalDigital"]));
                    }
                }
            }

            //Devolviendo resultado
            return timbre;
        }

        #endregion
    }
}
