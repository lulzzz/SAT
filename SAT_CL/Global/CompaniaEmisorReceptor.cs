using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{ 
    /// <summary>
    /// Clase encargada de Todas la Operaciones relacionadas con las Companias
    /// </summary>
    public class CompaniaEmisorReceptor : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_compania_emisor_receptor_tcer";

        private int _id_compania_emisor_receptor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania Emisora
        /// </summary>
        public int id_compania_emisor_receptor { get { return this._id_compania_emisor_receptor; } }
        private string _id_alterno;
        /// <summary>
        /// Atributo encargado de almacenar el Id Alterno
        /// </summary>
        public string id_alterno { get { return this._id_alterno; } }
        private string _rfc;
        /// <summary>
        /// Atributo encargado de almacenar el RFC
        /// </summary>
        public string rfc { get { return this._rfc; } }
        private string _nombre;
        /// <summary>
        /// Atributo encargado de almacenar el Nombre
        /// </summary>
        public string nombre { get { return this._nombre; } }
        private string _nombre_corto;
        /// <summary>
        /// Atributo encargado de almacenar el Nombre Corto
        /// </summary>
        public string nombre_corto { get { return this._nombre_corto; } }
        private int _id_direccion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Dirección
        /// </summary>
        public int id_direccion { get { return this._id_direccion; } }
        private bool _bit_emisor;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Emisor
        /// </summary>
        public bool bit_emisor { get { return this._bit_emisor; } }
        private bool _bit_receptor;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Receptor
        /// </summary>
        public bool bit_receptor { get { return this._bit_receptor; } }
        private bool _bit_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Proveedor
        /// </summary>
        public bool bit_proveedor { get { return this._bit_proveedor; } }
        private int _id_tipo_servicio;
        /// <summary>
        /// Atributo encargado de almacenar los Tipos de Servicio
        /// </summary>
        public int id_tipo_servicio { get { return this._id_tipo_servicio; } }
        private string _contacto;
        /// <summary>
        /// Atributo encargado de almacenar el Contacto
        /// </summary>
        public string contacto { get { return this._contacto; } }
        private string _correo;
        /// <summary>
        /// Atributo encargado de almacenar el Correo
        /// </summary>
        public string correo { get { return this._correo; } }
        private string _telefono;
        /// <summary>
        /// Atributo encargado de almacenar el Telefono
        /// </summary>
        public string telefono { get { return this._telefono; } }
        private decimal _limite_credito;
        /// <summary>
        /// Atributo encargado de almacenar el Limite de Credito
        /// </summary>
        public decimal limite_credito { get { return this._limite_credito; } }
        private int _dias_credito;
        /// <summary>
        /// Atributo encargado de almacenar los Dias de Credito
        /// </summary>
        public int dias_credito { get { return this._dias_credito; } }
        private int _id_compania_uso;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania de Uso
        /// </summary>
        public int id_compania_uso { get { return this._id_compania_uso; } }
        private int _id_compania_agrupador;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania Agrupadora
        /// </summary>
        public int id_compania_agrupador { get { return this._id_compania_agrupador; } }
        private string _informacion_adicional1;
        /// <summary>
        /// Atributo encargado de almacenar la Informacion Adicional 1
        /// </summary>
        public string informacion_adicional1 { get { return this._informacion_adicional1; } }
        private string _informacion_adicional2;
        /// <summary>
        /// Atributo encargado de almacenar la Informacion Adicional 1
        /// </summary>
        public string informacion_adicional2 { get { return this._informacion_adicional2; } }
        private int _id_regimen_fiscal;
        /// <summary>
        /// Atributo encargado de almacenar el Regimen Fiscal
        /// </summary>
        public int id_regimen_fiscal { get { return this._id_regimen_fiscal; } }
        private int _id_uso_cfdi;
        /// <summary>
        /// Atributo encargado de almacenar el Uso del CFDI
        /// </summary>
        public int id_uso_cfdi { get { return this._id_uso_cfdi; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private decimal _saldo_actual;
        /// <summary>
        /// Atributo encargado de almacenar el Saldo Actual
        /// </summary>
        public decimal saldo_actual { get { return this._saldo_actual; } }
        /// <summary>
        /// Obtiene la configuración  de Facturación Electronica para la compañia
        /// </summary>
        public Dictionary<string, string> FacturacionElectronica;
        /// <summary>
        /// Obtiene la configuración  de Facturación Electronica para la compañia v3.3
        /// </summary>
        public Dictionary<string, string> FacturacionElectronica33;
        /// <summary>
        /// Obtiene el directorio de trabajo del emidor
        /// </summary>
        public string DirectorioAlmacenamiento { get { return Cadena.SustituyePatronCadena(this._nombre, "[,|.|\\|/|:|*|\"|<|>|'|_||]", "") + @"\"; } }
        /// <summary>
        /// Obtiene la ruta física de almacenamiento del logotipo de la Compania
        /// </summary>
        public string ruta_logotipo { get { return this._ruta_logotipo; } }
        private string _ruta_logotipo;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public CompaniaEmisorReceptor()
        {   //Invocando Método de Cargado
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public CompaniaEmisorReceptor(int id_registro)
        {   //Invocando Método de Cargado
            cargaAtributosInstancia(id_registro);
        }

        public CompaniaEmisorReceptor(int id_compania, int id_cliente, byte indicador_tipo_saldo)
        {   //Invocando Método de Cargado
            cargaAtributosInstancia(id_compania, id_cliente, indicador_tipo_saldo);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un RFC
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="RFC"></param>
        public CompaniaEmisorReceptor(int id_compania, string  RFC)
        {   //Invocando Método de Cargado
            cargaAtributosInstancia(id_compania, RFC);
        }
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~CompaniaEmisorReceptor()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_compania_emisor_receptor = 0;
	        this._id_alterno = "";
	        this._rfc = "";
	        this._nombre = "";
	        this._nombre_corto = "";
	        this._id_direccion = 0;
	        this._bit_emisor = false;
	        this._bit_receptor = false;
	        this._bit_proveedor = false;
            this._id_tipo_servicio = 0;
	        this._contacto = "";
	        this._correo = "";
	        this._telefono = "";
	        this._limite_credito = 0;
	        this._dias_credito = 0;
	        this._id_compania_uso = 0;
            this._id_compania_agrupador = 0;
	        this._informacion_adicional1 = "";
            this._informacion_adicional2 = "";
            this._id_regimen_fiscal = 0;
            this._id_uso_cfdi = 0;
            this._saldo_actual = 0;
            this._habilitar = false;
            this.FacturacionElectronica = new Dictionary<string, string>();
            this._ruta_logotipo = "";
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 3, id_registro, "", "", "", "", 0, false, false, false, 0, "", "", "", 0, 0, 0, 0, "", "", 0, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada uno de los Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_compania_emisor_receptor = id_registro;
                        this._id_alterno = dr["IdAlterno"].ToString();
                        this._rfc = dr["RFC"].ToString();
                        this._nombre = dr["Nombre"].ToString();
                        this._nombre_corto = dr["NombreCorto"].ToString();
                        this._id_direccion = Convert.ToInt32(dr["IdDireccion"]);
                        this._bit_emisor = Convert.ToBoolean(dr["BitEmisor"]);
                        this._bit_receptor = Convert.ToBoolean(dr["BitReceptor"]);
                        this._bit_proveedor = Convert.ToBoolean(dr["BitProveedor"]);
                        this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
                        this._contacto = dr["Contacto"].ToString();
                        this._correo = dr["Correo"].ToString();
                        this._telefono = dr["Telefono"].ToString();
                        this._limite_credito = Convert.ToDecimal(dr["LimiteCredito"]);
                        this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
                        this._id_compania_uso = Convert.ToInt32(dr["IdCompaniaUso"]);
                        this._id_compania_agrupador = Convert.ToInt32(dr["IdCompaniaAgrupador"]);
                        this._informacion_adicional1 = dr["InformacionAdicional1"].ToString();
                        this._informacion_adicional2 = dr["InformacionAdicional2"].ToString();
                        this._saldo_actual = 0;
                        this._id_regimen_fiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        this._id_uso_cfdi = Convert.ToInt32(dr["IdUsoCFDI"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._ruta_logotipo = dr["RutaLogotipo"].ToString();

                        //Cargamos las Referencias de Facturación Electronica
                        cargaReferenciasFacturacionElectronica();
                        cargaReferenciasFacturacionElectronicav33();
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Cargar los Atributos dada la Compania, el Cliente y el Indicador del Tipo de Saldo
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="indicador_tipo_saldo">Indicador del Tipo de Saldo (1.- Acreedor y 2.- Deudor)</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_compania, int id_cliente, byte indicador_tipo_saldo)
        {   
            //Declarando Objeto de Retorno
            bool result = false;
            
            //Armando arreglo de Parametros
            object[] param = { 9, id_cliente, "", "", "", "", 0, false, false, false, 0, "", "", "", 0, 0, id_compania, 0, "", "", 0, 0, 0, false, indicador_tipo_saldo.ToString(), "" };
            
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Valores
                        this._id_compania_emisor_receptor = Convert.ToInt32(dr["Id"]);
                        this._id_alterno = dr["IdAlterno"].ToString();
                        this._rfc = dr["RFC"].ToString();
                        this._nombre = dr["Nombre"].ToString();
                        this._nombre_corto = dr["NombreCorto"].ToString();
                        this._id_direccion = Convert.ToInt32(dr["IdDireccion"]);
                        this._bit_emisor = Convert.ToBoolean(dr["BitEmisor"]);
                        this._bit_receptor = Convert.ToBoolean(dr["BitReceptor"]);
                        this._bit_proveedor = Convert.ToBoolean(dr["BitProveedor"]);
                        this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
                        this._contacto = dr["Contacto"].ToString();
                        this._correo = dr["Correo"].ToString();
                        this._telefono = dr["Telefono"].ToString();
                        this._limite_credito = Convert.ToDecimal(dr["LimiteCredito"]);
                        this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
                        this._id_compania_uso = Convert.ToInt32(dr["IdCompaniaUso"]);
                        this._id_compania_agrupador = Convert.ToInt32(dr["IdCompaniaAgrupador"]);
                        this._informacion_adicional1 = dr["InformacionAdicional1"].ToString();
                        this._informacion_adicional2 = dr["InformacionAdicional2"].ToString();
                        this._saldo_actual = Convert.ToDecimal(dr["Saldo"]);
                        this._id_regimen_fiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        this._id_uso_cfdi = Convert.ToInt32(dr["IdUsoCFDI"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._ruta_logotipo = dr["RutaLogotipo"].ToString();

                        //Cargamos las Referencias de Facturación Electronica
                        cargaReferenciasFacturacionElectronica();
                        cargaReferenciasFacturacionElectronicav33();
                    }
                    
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de cargar la Instancia ligando un RFC
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="RFC">RFC</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_compania, string RFC)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 7, 0, "", RFC, "", "", 0, false, false, false, 0, "", "", "", 0, 0, id_compania, 0, "", "", 0, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_compania_emisor_receptor = Convert.ToInt32(dr["Id"]); ;
                        this._id_alterno = dr["IdAlterno"].ToString();
                        this._rfc = dr["RFC"].ToString();
                        this._nombre = dr["Nombre"].ToString();
                        this._nombre_corto = dr["NombreCorto"].ToString();
                        this._id_direccion = Convert.ToInt32(dr["IdDireccion"]);
                        this._bit_emisor = Convert.ToBoolean(dr["BitEmisor"]);
                        this._bit_receptor = Convert.ToBoolean(dr["BitReceptor"]);
                        this._bit_proveedor = Convert.ToBoolean(dr["BitProveedor"]);
                        this._id_tipo_servicio = Convert.ToInt32(dr["IdTipoServicio"]);
                        this._contacto = dr["Contacto"].ToString();
                        this._correo = dr["Correo"].ToString();
                        this._telefono = dr["Telefono"].ToString();
                        this._limite_credito = Convert.ToDecimal(dr["LimiteCredito"]);
                        this._dias_credito = Convert.ToInt32(dr["DiasCredito"]);
                        this._id_compania_uso = Convert.ToInt32(dr["IdCompaniaUso"]);
                        this._id_compania_agrupador = Convert.ToInt32(dr["IdCompaniaAgrupador"]);
                        this._informacion_adicional1 = dr["InformacionAdicional1"].ToString();
                        this._informacion_adicional2 = dr["InformacionAdicional2"].ToString();
                        this._saldo_actual = 0;
                        this._id_regimen_fiscal = Convert.ToInt32(dr["IdRegimenFiscal"]);
                        this._id_uso_cfdi = Convert.ToInt32(dr["IdUsoCFDI"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        this._ruta_logotipo = dr["RutaLogotipo"].ToString();

                        //Cargamos las Referencias de Facturación Electronica
                        cargaReferenciasFacturacionElectronica();
                        cargaReferenciasFacturacionElectronicav33();
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_alterno">Id Alterno</param>
        /// <param name="rfc">RFC</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="nombre_corto">Nombre Corto</param>
        /// <param name="id_direccion">Dirección de la Compania</param>
        /// <param name="bit_emisor">Indicador de Compania Emisor</param>
        /// <param name="bit_receptor">Indicador de Compania Receptor</param>
        /// <param name="bit_proveedor">Indicador de Compania Proveedor</param>
        /// <param name="id_tipo_servicio">Tio Servicio</param>
        /// <param name="contacto">Contacto de la Compania</param>
        /// <param name="correo">Correo de la Compania</param>
        /// <param name="telefono">Telefono de la Compania</param>
        /// <param name="limite_credito">Limite de Credito</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_compania_uso">Compania Uso</param>
        /// <param name="id_compania_agrupador">Compania Agrupadora</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_uso_cfdi"></param>
        /// <param name="id_regimen_fiscal"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string id_alterno, string rfc, string nombre,	string nombre_corto,
	                            int id_direccion, bool bit_emisor,	bool bit_receptor,	bool bit_proveedor, int id_tipo_servicio, string contacto, string correo,
                                string telefono, decimal limite_credito, int dias_credito, int id_compania_uso, int id_compania_agrupador, string informacion_adicional1,
                                string informacion_adicional2, int id_regimen_fiscal, int id_uso_cfdi, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 2, this._id_compania_emisor_receptor, id_alterno, rfc, nombre, nombre_corto,
	                            id_direccion, bit_emisor, bit_receptor, bit_proveedor, id_tipo_servicio, contacto, correo,
	                            telefono, limite_credito, dias_credito, id_compania_uso, id_compania_agrupador, informacion_adicional1,
	                            informacion_adicional2, id_regimen_fiscal, id_uso_cfdi, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la carga de las referencias usadas en la Facturación Electronuca
        /// </summary>
        /// <returns></returns>
        private void cargaReferenciasFacturacionElectronica()
        {
            //Armando arreglo de Parametros
            object[] param = { 8, this._id_compania_emisor_receptor, "", "", "", "", 0, false, false, false, 0, "", "", "", 0, 0, 0, 0, "", "", 0, 0, 0, false, "", "" };

            //Realziando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existe el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando conjunto de elementos al diccionario de salida
                    this.FacturacionElectronica = (from DataRow r in ds.Tables["Table"].Rows
                                          select r).ToDictionary(c => c.Field<string>("Descripcion"), c => c.Field<string>("Valor"));
                }
            }
        }
        /// <summary>
        /// Realiza la carga de las referencias usadas en la Facturación Electronica v3.3
        /// </summary>
        /// <returns></returns>
        private void cargaReferenciasFacturacionElectronicav33()
        {
            //Armando arreglo de Parametros
            object[] param = { 11, this._id_compania_emisor_receptor, "", "", "", "", 0, false, false, false, 0, "", "", "", 0, 0, 0, 0, "", "", 0, 0, 0, false, "", "" };

            //Realziando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si existe el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando conjunto de elementos al diccionario de salida
                    this.FacturacionElectronica33 = (from DataRow r in ds.Tables["Table"].Rows
                                                   select r).ToDictionary(c => c.Field<string>("Descripcion"), c => c.Field<string>("Valor"));
                }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Companias
        /// </summary>
        /// <param name="id_alterno">Id Alterno</param>
        /// <param name="rfc">RFC</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="nombre_corto">Nombre Corto</param>
        /// <param name="id_direccion">Dirección de la Compania</param>
        /// <param name="bit_emisor">Indicador de Compania Emisor</param>
        /// <param name="bit_receptor">Indicador de Compania Receptor</param>
        /// <param name="bit_proveedor">Indicador de Compania Proveedor</param>
        /// <param name="id_tipo_servicio">Tipo Servicio</param>
        /// <param name="contacto">Contacto de la Compania</param>
        /// <param name="correo">Correo de la Compania</param>
        /// <param name="telefono">Telefono de la Compania</param>
        /// <param name="limite_credito">Limite de Credito</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_compania_uso">Compania Uso</param>
        /// <param name="id_compania_agrupador">Compania Agrupadora</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_uso_cfdi"></param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCompaniaEmisorRecepto(string id_alterno, string rfc, string nombre, string nombre_corto,
                                int id_direccion, bool bit_emisor, bool bit_receptor, bool bit_proveedor, int id_tipo_servicio, string contacto, string correo,
                                string telefono, decimal limite_credito, int dias_credito, int id_compania_uso, int id_compania_agrupador, string informacion_adicional1,
                                string informacion_adicional2, int id_regimen_fiscal, int id_uso_cfdi, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_alterno, rfc, nombre, nombre_corto,
	                            id_direccion, bit_emisor, bit_receptor, bit_proveedor, id_tipo_servicio, contacto, correo,
	                            telefono, limite_credito, dias_credito, id_compania_uso, id_compania_agrupador, informacion_adicional1,
	                            informacion_adicional2, id_regimen_fiscal, id_uso_cfdi, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Companias
        /// </summary>
        /// <param name="id_alterno">Id Alterno</param>
        /// <param name="rfc">RFC</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="nombre_corto">Nombre Corto</param>
        /// <param name="id_direccion">Dirección de la Compania</param>
        /// <param name="bit_emisor">Indicador de Compania Emisor</param>
        /// <param name="bit_receptor">Indicador de Compania Receptor</param>
        /// <param name="bit_proveedor">Indicador de Compania Proveedor</param>
        /// <param name="id_tipo_servicio">Tipo Sericio</param>
        /// <param name="contacto">Contacto de la Compania</param>
        /// <param name="correo">Correo de la Compania</param>
        /// <param name="telefono">Telefono de la Compania</param>
        /// <param name="limite_credito">Limite de Credito</param>
        /// <param name="dias_credito">Dias de Credito</param>
        /// <param name="id_compania_uso">Compania Uso</param>
        /// <param name="id_compania_agrupador">Compania Agrupadora</param>
        /// <param name="informacion_adicional1">Información Adicional 1</param>
        /// <param name="informacion_adicional2">Información Adicional 2</param>
        /// <param name="id_uso_cfdi"></param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaCompaniaEmisorRecepto(string id_alterno, string rfc, string nombre, string nombre_corto,
                                int id_direccion, bool bit_emisor, bool bit_receptor, bool bit_proveedor, int id_tipo_servicio, string contacto, string correo,
                                string telefono, decimal limite_credito, int dias_credito, int id_compania_uso, int id_compania_agrupador, string informacion_adicional1,
                                string informacion_adicional2, int id_regimen_fiscal, int id_uso_cfdi, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que no existan Comprobantes
            if (!(SAT_CL.FacturacionElectronica.Comprobante.ValidaFacturacionElectronica(this._id_compania_emisor_receptor)))
            {

                //Invocando Método de Actualización y permitiendo edición total
                result = this.actualizaRegistros(id_alterno, rfc, nombre, nombre_corto,
                                    id_direccion, bit_emisor, bit_receptor, bit_proveedor, id_tipo_servicio, contacto, correo,
                                    telefono, limite_credito, dias_credito, id_compania_uso, id_compania_agrupador, informacion_adicional1,
                                    informacion_adicional2, id_regimen_fiscal, id_uso_cfdi, id_usuario, this._habilitar);
            }
            //Solo se actualizará información comercial, la información fiscal se mantiene intacta
            else
            {
                //Invocando Método de Actualización y permitiendo edición parcial
                result = this.actualizaRegistros(id_alterno, this._rfc, nombre, nombre_corto,
                                    id_direccion, bit_emisor, bit_receptor, bit_proveedor, id_tipo_servicio, contacto, correo,
                                    telefono, limite_credito, dias_credito, id_compania_uso, id_compania_agrupador, informacion_adicional1,
                                    informacion_adicional2, id_regimen_fiscal, id_uso_cfdi, id_usuario, this._habilitar);

                //Instanciando Excepción
                result = new RetornoOperacion(result.IdRegistro, result.OperacionExitosa ? "La Compañía ya tiene CFDI Timbrados, RFC y Razón Social se mantienen sin cambios." : result.Mensaje, result.OperacionExitosa);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Companias
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCompaniaEmisorRecepto(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que no existan Comprobantes
            if (!(SAT_CL.FacturacionElectronica.Comprobante.ValidaFacturacionElectronica(this._id_compania_emisor_receptor)))

                //Invocando Método de Actualización
                result = this.actualizaRegistros(this._id_alterno, rfc, this._nombre, this._nombre_corto,
                                    this._id_direccion, this._bit_emisor, this._bit_receptor, this._bit_proveedor, this._id_tipo_servicio, this._contacto, this._correo,
                                    this._telefono, this._limite_credito, this._dias_credito, this._id_compania_uso, this._id_compania_agrupador, this._informacion_adicional1,
                                    this._informacion_adicional2, this._id_regimen_fiscal, this._id_uso_cfdi, id_usuario, false);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("La Compania ya tiene Comprobantes Timbrados");


            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Carga los Terceros para Asignación de Recurso
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="nombre_tercero">Nombre Tercero</param>
        /// <returns></returns>
        public static DataTable CargaTercerosParaAsignacionRecurso(int id_compania_emisor,  string nombre_tercero)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 4, 0, "", "", nombre_tercero, "", 0, false, false, false, 0, "", "", "", 0, 0, id_compania_emisor, 0, "", "", 0, 0, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método Público encargado de Obtener una Instancia de la Clase dado un RFC
        /// </summary>
        /// <param name="rfc">RFC</param>
        /// <param name="id_compania_uso">Compania Uso</param>
        /// <returns></returns>
        public static CompaniaEmisorReceptor ObtieneInstanciaCompaniaRFC(string rfc, int id_compania_uso)
        {   
            //Declarando Objeto de Retorno
            CompaniaEmisorReceptor cer = new CompaniaEmisorReceptor();
            
            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 5, 0, "", rfc, "", "", 0, false, false, false, 0, "", "", "", 0, 0, id_compania_uso, 0, "", "", 0, 0, 0, false, "", "" };
            
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando si existe el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Instanciando Compania
                        cer = new CompaniaEmisorReceptor(Convert.ToInt32(dr["Id"]));
                    }
                }
            }
            
            //Devolviendo Resultado Obtenido
            return cer;
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Valor de los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCompaniaEmisorRecepto()
        {   //Invocando Método de Carga de Atributos
            return this.cargaAtributosInstancia(this._id_compania_emisor_receptor);
        }

        /// <summary>
        /// Carga los Terceros para Asignación de Recurso en Despacho
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="nombre">Nombre del Tercero</param>
        /// <returns></returns>
        public static DataTable CargaTercerosParaAsignacionEnDespacho(int id_compania_emisor, string nombre)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 6, 0, "", "", nombre, "", 0, false, false, false, 0, "", "", "", 0, 0, id_compania_emisor, 0, "", "", 0, 0, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene las referencias del tipo leyenda al pie de impresión del emisor
        /// </summary>
        /// <returns></returns>
        public string[] LeyendasImpresionCFD()
        {
            //Declarando variable de retorno
            List<string> leyendas = new List<string>();

            //Cargando las referencias requeridas
            DataTable referencias = Referencia.CargaReferenciasRegistro(0, this._id_compania_emisor_receptor, 25, 1020, 1043, "", "", 0);

            //Añadiendo el regimen fiscal
            DataTable regimen = Referencia.CargaReferenciasRegistro(0, this._id_compania_emisor_receptor, 25, 1020, 1044, "", "", 0);

            //Validando el origen de datos
            if (Validacion.ValidaOrigenDatos(referencias))
            {
                //Recorriendo los registros devuletos
                foreach (DataRow f in referencias.Rows)
                    //Añadiendo leyenda encontrada
                    leyendas.Add(f.Field<string>("Valor"));
            }
            //Validando el origen de datos (regimen fiscal)
            if (Validacion.ValidaOrigenDatos(regimen))
            {
                //Recorriendo los registros devuletos
                foreach (DataRow f in regimen.Rows)
                    //Añadiendo leyenda encontrada
                    leyendas.Add(f.Field<string>("Valor"));
            }

            //Devolvinedo leyendas requeridas
            return leyendas.ToArray();
        }
        /// <summary>
        /// Método que obtiene el encabezado de una compañia para la impresión de informes
        /// </summary>
        /// <param name="id_compania_emisor_receptor">Identificador de la empresa</param>
        /// <returns></returns>
        public static DataTable EncabezadoImpresión(int id_compania_emisor_receptor)
        {
            //Creación de la variable datatables
            DataTable dtEncabezado = null;
            //Creación del arreglo param
            object[] param = { 10, id_compania_emisor_receptor, "", "", "", "", 0, false, false, false, 0, "", "", "", 0, 0, 0, 0, "", "", 0, 0, 0, false, "", "" };
            //Invoca al método EjecutaProcAlmacenadoDataSet
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla encabezado los valores obtenidos del dataset
                    dtEncabezado = DS.Tables["Table"];
            }
            //Devuelve al método la tabla dtEncabezado.
            return dtEncabezado;
        }

        /// <summary>
        /// Determina si la compañía especificada tiene habilitado el uso de aplicación móvil
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaConfiguracionUsoAplicacionMovil(int id_compania)
        {
            //Declarando variable de retorno
            RetornoOperacion resultado = new RetornoOperacion("La compañía no tiene habilitado el uso de aplicación móvil.");
            //Cargando referencia de configuración
            using (DataTable mit = Referencia.CargaReferencias(id_compania, 25, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 25, "Uso Aplicación Móvil", 0, "Configuración")))
            {
                //Si hay resultados
                if (mit != null)
                    if (Convert.ToBoolean(mit.Rows[0]["Valor"]))
                        resultado = new RetornoOperacion("Configuración válida.", true);
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
