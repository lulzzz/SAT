using SAT_CL.FacturacionElectronica;
using System;
using System.ComponentModel;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Descripción breve de CertificadoDigital
    /// </summary>
    public class CertificadoDigital : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles estaus en los que puede encontrarse un certificado digital
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Certificado actualmente activo
            /// </summary>
            Vigente = 1,
            /// <summary>
            /// Certificado cuya vigencia ya ha sido expirada
            /// </summary>
            Vencido,
            /// <summary>
            /// Certificado Cancelado por el propietario
            /// </summary>
            Revocado
        }
        /// <summary>
        /// Define los tipos de certificados utilizados
        /// </summary>
        public enum TipoCertificado
        {
            /// <summary>
            /// Firma Electrónica Avanzada
            /// </summary>
            [Description("FIEL")]
            FIEL = 1,
            /// <summary>
            /// Certificado de Sello Digital
            /// </summary>
            [Description("CSD")]
            CSD
        }

        #endregion

        #region Atributos (R)

        //Nombre procedimiento almacenado
        private static string nombre_procedimiento = "global.sp_certificado_digital_tcs";

        private int _idKey;
        /// <summary>
        /// Id Archivo registro
        /// </summary>
        public int idKey
        {
            get
            {
                return _idKey;
            }
        }
        private int _idCer;
        /// <summary>
        /// Id Archivo registro
        /// </summary>
        public int idCer
        {
            get
            {
                return _idCer;
            }
        }
        private int _id_certificado_digital;
        /// <summary>
        /// Obtiene el identificador de la instancia
        /// </summary>
        public int id_certificado_digital
        {
            get
            {
                return _id_certificado_digital;
            }
        }

        private int _id_emisor;
        /// <summary>
        /// Obiene el emisor al que pertenece el certificado
        /// </summary>
        public int id_emisor
        {
            get
            {
                return _id_emisor;
            }
        }

        private int _id_sucursal;
        /// <summary>
        /// Obtiene el Id de Sucursal
        /// </summary>
        public int id_sucursal
        {
            get
            {
                return this._id_sucursal;
            }
        }

        private byte _id_tipo_certificado;
        /// <summary>
        /// Obtiene el tipo de certificado
        /// </summary>
        public TipoCertificado tipo_certificado
        {
            get
            {
                return (TipoCertificado)_id_tipo_certificado;
            }
        }

        /// <summary>
        /// Obtien el Id tipo certificado
        /// </summary>
        public byte id_tipo_certificado
        {
            get
            {
                return _id_tipo_certificado;
            }
        }

        private byte _id_estatus_certificado;
        /// <summary>
        /// Obtiene el estatus del certificado
        /// </summary>
        public Estatus estatus_certificado
        {
            get
            {
                return (Estatus)_id_estatus_certificado;
            }
        }

        /// <summary>
        /// Obtiene el estatus del certificado
        /// </summary>
        public byte id_estatus_certificado
        {
            get
            {
                return _id_estatus_certificado;
            }
        }

        private string _contrasena;
        /// <summary>
        /// Obtiene la contraseña del certificado
        /// </summary>
        public string contrasena
        {
            get
            {
                return _contrasena;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string contrasena_desencriptada
        { 
            get 
            {
                return Encriptacion.DesencriptaBytesAES(Convert.FromBase64String(this._contrasena), this._key_contrasena);
            }
        }

        private string _iv_contrasena; 
        /// <summary>
        /// 
        /// </summary>
        public string iv_contrasena { get { return this._iv_contrasena; } }

        private string _key_contrasena;
        /// <summary>
        /// 
        /// </summary>
        public string key_contrasena { get { return this._key_contrasena; } }

        private string _contrasena_revocacion;
        /// <summary>
        /// Obtiene la contraseña del certificado
        /// </summary>
        public string contrasena_revocacion
        {
            get
            {
                return _contrasena_revocacion;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string contrasena_revocacion_desencriptada
        {
            get
            {
                return Encriptacion.DesencriptaBytesAES(Convert.FromBase64String(this._contrasena_revocacion), this._key_contrasena_revocacion);
            }
        }

        private string _iv_contrasena_revocacion;
        /// <summary>
        /// 
        /// </summary>
        public string iv_contrasena_revocacion { get { return this._iv_contrasena_revocacion; } }

        private string _key_contrasena_revocacion;
        /// <summary>
        /// 
        /// </summary>
        public string key_contrasena_revocacion { get { return this._key_contrasena_revocacion; } }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el campo habilitar de la instancia
        /// </summary>
        public bool Habilitar
        {
            get
            {
                return _habilitar;
            }
        }

        private string _ruta_llave_publica;
        /// <summary>
        /// Obtiene la ruta de almacenamiento físico del certificado
        /// </summary>
        public string ruta_llave_publica
        {
            get { return this._ruta_llave_publica; }
        }

        private string _ruta_llave_privada;
        /// <summary>
        /// Obtiene la ruta de almacenamiento físico del certificado
        /// </summary>
        public string ruta_llave_privada
        {
            get { return this._ruta_llave_privada; }
        }

        #endregion

        #region Constructores (R)

        /// <summary>
        /// Instancía un certificado digital con los valores por default
        /// </summary>
        private CertificadoDigital()
        {
            //Asignando valores
            _id_certificado_digital = 0;
            _id_emisor = 0;
            _id_sucursal = 0;
            _id_tipo_certificado = 0;
            _id_estatus_certificado = 0;
            _contrasena = "";
            _iv_contrasena = "";
            _key_contrasena = "";
            _contrasena_revocacion = "";
            _iv_contrasena_revocacion = "";
            _key_contrasena_revocacion = "";
            _habilitar = false;
            _idCer = 0;
            _idKey = 0;
            _ruta_llave_privada = "";
            _ruta_llave_publica = "";
        }

        /// <summary>
        /// Instancía un certificado digital en base a su id
        /// </summary>
        /// <param name="id_certificado">Id de Certificado</param>
        public CertificadoDigital(int id_certificado)
        {
            //Instanciando arreglo de objetos
            object[] param = { 3, id_certificado, 0, 0, 0, 0, "", "", "", "", "", "", 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo los renglones de la tabla
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando valores
                        _id_certificado_digital = Convert.ToInt32(r["Id"]);
                        _id_emisor = Convert.ToInt32(r["IdEmisor"]);
                        _id_sucursal = Convert.ToInt32(r["IdSucursal"]);
                        _id_tipo_certificado = Convert.ToByte(r["IdTipoCertificado"]);
                        _id_estatus_certificado = Convert.ToByte(r["IdEstatusCertificado"]);
                        _contrasena = r["Contraseña"].ToString();
                        _iv_contrasena = r["IVContraseña"].ToString();
                        _key_contrasena = r["KEYContraseña"].ToString();
                        _contrasena_revocacion = r["ContraseñaRevocacion"].ToString();
                        _iv_contrasena_revocacion = r["IVContraseñaRevocacion"].ToString();
                        _key_contrasena_revocacion = r["KEYContraseñaRevocacion"].ToString();
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _idCer = Convert.ToInt32(r["IdCer"]);
                        _idKey = Convert.ToInt32(r["IdKey"]);
                        _ruta_llave_privada = r["RutaArchivoKey"].ToString();
                        _ruta_llave_publica = r["RutaArchivoCer"].ToString();
                    }
                }
            }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~CertificadoDigital()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos privados (R)

        /// <summary>
        /// Realiza la edición de los datos de un certificado
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_sucursal">Id de Sucursar</param>
        /// <param name="tipo_certificado">Tipo de Certificado</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="contrasena">Contraseña de apertura del certificado</param>
        /// <param name="iv_contrasena"></param>
        /// <param name="key_contrasena"></param>
        /// <param name="contrasena_revocacion">Contraseña de revocación del certificado</param>
        /// <param name="iv_contrasena_revocacion"></param>
        /// <param name="key_contrasena_revocacion"></param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaCertificado(int id_emisor, int id_sucursal, TipoCertificado tipo_certificado,
                            Estatus estatus, string contrasena, string iv_contrasena, string key_contrasena,
                            string contrasena_revocacion, string iv_contrasena_revocacion, string key_contrasena_revocacion,
                            int id_usuario, bool habilitar)
        {
            //Inicializando parametros
            object[] param = { 2, this._id_certificado_digital, id_emisor, id_sucursal, (byte)tipo_certificado, (byte)estatus, 
                            contrasena, iv_contrasena, key_contrasena, contrasena_revocacion, iv_contrasena_revocacion, key_contrasena_revocacion,
                            id_usuario, habilitar, "", "" };

            //Ejecutando el SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento, param);
        }

        #endregion

        #region Métodos publicos (R)

        /// <summary>
        /// Inserta un nuevo registro Certificado
        /// </summary>
        /// <param name="id_emisor">Id de Emisor al que pertenece</param>
        /// <param name="id_sucursal">Id de sucursal del emisor</param>
        /// <param name="tipo_certificado"></param>
        /// <param name="contrasena"></param>
        /// <param name="contrasena_revocacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCertificado(int id_emisor, int id_sucursal, TipoCertificado tipo_certificado,
                                                              string contrasena, string contrasena_revocacion, int id_usuario)
        {
            //Declaramos llaves de encriptación aleatorias
            string key_contrasena = Cadena.CadenaAleatoria(10),
                    key_contrasena_revocacion = Cadena.CadenaAleatoria(10);

            //Encriptando contraseñas
            string contrasenaEnc = Convert.ToBase64String(Encriptacion.EncriptaCadenaAES(contrasena, key_contrasena));
            string contrasenaEncRev = Convert.ToBase64String(Encriptacion.EncriptaCadenaAES(contrasena_revocacion, key_contrasena_revocacion));

            //Inicializando parametros
            object[] param = { 1, 0, id_emisor, id_sucursal, (byte)tipo_certificado, (byte)Estatus.Vigente, contrasenaEnc, 
                            "", key_contrasena, contrasenaEncRev, "", key_contrasena_revocacion, 
                            id_usuario, true, "", "" };

            //Ejecutando el SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento, param);

        }

        /// <summary>
        /// Realiza la edición de los datos de un certificado
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_sucursal">Id de Sucursar</param>
        /// <param name="tipo_certificado">Tipo de Certificado</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="contrasena">Contraseña de apertura del certificado</param>
        /// <param name="iv_contrasena"></param>
        /// <param name="key_contrasena"></param>
        /// <param name="contrasena_revocacion">Contraseña de revocación del certificado</param>
        /// <param name="iv_contrasena_revocacion"></param>
        /// <param name="key_contrasena_revocacion"></param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaCertificado(int id_emisor, int id_sucursal, TipoCertificado tipo_certificado,
                            Estatus estatus, string contrasena, string iv_contrasena, string key_contrasena, string contrasena_revocacion, string iv_contrasena_revocacion, string key_contrasena_revocacion, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Ejecutando el SP
            resultado = editaCertificado(id_emisor, id_sucursal, tipo_certificado, estatus, contrasena, iv_contrasena, key_contrasena, contrasena_revocacion, iv_contrasena_revocacion, key_contrasena_revocacion, id_usuario, this._habilitar);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualiza el estatus del certificado a "Vencido"
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusVencido(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Ejecutando el SP
            resultado = editaCertificado(this._id_emisor, this._id_sucursal, this.tipo_certificado, Estatus.Vencido,
                                        this._contrasena, this._iv_contrasena, this._key_contrasena,
                                        this._contrasena_revocacion, this._iv_contrasena_revocacion, this._key_contrasena_revocacion,
                                        id_usuario, this._habilitar);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza el estatus del certificado a "Revocado"
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="contrasena_revocacion">Contraseña para revocar certificado</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusRevocado(int id_usuario, string contrasena_revocacion)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Si la contraseña es la misma a la contraseña definida para revocación
            if (Encriptacion.DesencriptaBytesAES(Convert.FromBase64String(this._contrasena_revocacion),
                this._key_contrasena_revocacion) == contrasena_revocacion)
            {
                //Ejecutando el SP
                resultado = editaCertificado(this._id_emisor, this._id_sucursal, this.tipo_certificado, Estatus.Revocado,
                                            this._contrasena, this._iv_contrasena, this._key_contrasena,
                                        this._contrasena_revocacion, this._iv_contrasena_revocacion, this._key_contrasena_revocacion, id_usuario, this._habilitar);
            }
            //De lo contrario
            else
                resultado = new RetornoOperacion("La contraseña de revocación es incorrecta.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la deshabilitación del certificado
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCertificado(int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que no existan comprobnates sellados con el certificado indicado
            if (!Validacion.ValidaOrigenDatos(Comprobante.RecuperaComprobantesSellados(this._id_certificado_digital)))
            {
                //Ejecutando el SP
                resultado = editaCertificado(this._id_emisor, this._id_sucursal, this.tipo_certificado, this.estatus_certificado,
                                            this._contrasena, this._iv_contrasena, this._key_contrasena,
                                        this._contrasena_revocacion, this._iv_contrasena_revocacion, this._key_contrasena_revocacion, id_usuario, false);
            }
            else
                resultado = new RetornoOperacion("No es posible deshabilitar el certificado, ya tiene comprobantes sellados.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene el Certificado de Sello Digital activo del Emisor y/o sucursal
        /// </summary>
        /// <param name="id_emisor">Id de Emisor</param>
        /// <param name="id_sucursal">Id de Sucursal</param>
        /// <param name="tipo_certificado">Tipo de Certificado a buscar</param>
        /// <returns></returns>
        public static CertificadoDigital RecuperaCertificadoEmisorSucursal(int id_emisor, int id_sucursal, TipoCertificado tipo_certificado)
        {
            //Declarando objeto de retorno
            CertificadoDigital cer = new CertificadoDigital();

            //Inicializando parametros
            object[] param = { 4, 0, id_emisor, id_sucursal, (byte)tipo_certificado, 0, "", "", "", "", "", "", 0, true, "", "" };

            //Ejecutando el SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento, param))
            {
                //Si existe el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada regitrso
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        cer = new CertificadoDigital(Convert.ToInt32(r["Id"]));
                }

                return cer;
            }
        }

        #endregion

    }
}
