using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Proporciona  los método para la administración de la Serie y Folio
    /// </summary>
    public class SerieFolio : Disposable
    {
        public enum tipo
        {
            /// <summary>
            /// Facturacion Electronica
            /// </summary>
            FacturacionElectronica = 1,
            /// <summary>
            /// Recibo dde Nomina
            /// </summary>
            ReciboNomina

        }

        #region Atributos

        /// <summary>
        /// Define el nombre del Stored Procedure utilizado en interacciones de  la clase
        /// </summary>
        public static string nombre_stored_procedure = "fe.sp_serie_folio_tsf";
        private int _id_folio_serie;
        private int _id_emisor;
        private string _serie;
        private bool _activa;
        private int _folio_inicial;
        private int _folio_final;
        private byte _tipo_folio_serie;
        private bool _habilitar;

        /// <summary>
        /// id Folio Serie
        /// </summary>
        public int id_folio_serie { get { return _id_folio_serie; } }
        /// <summary>
        /// Emisor
        /// </summary>
        public int id_emisor { get { return _id_emisor; } }
        /// <summary>
        /// Serie
        /// </summary>
        public string serie { get { return _serie; } }
        /// <summary>
        /// Activa
        /// </summary>
        public bool activa { get { return _activa; } }
        /// <summary>
        /// Folio Inicial
        /// </summary>
        public int folio_inicial { get { return _folio_inicial; } }
        /// <summary>
        /// Folio Final
        /// </summary>
        public int folio_final { get { return _folio_final; } }
        /// <summary>
        /// Tipo Folio Serie
        /// </summary>
        public int tipo_folio_serie { get { return tipo_folio_serie; } }
        /// <summary>
        /// bit habilitar del registro
        /// </summary>
        public bool habilitar { get { return _habilitar; } }


        /// <summary>
        /// Enumera el Tipo de Serie Folio
        /// </summary>
        public tipo estatusTipo
        {
            get { return (tipo)_tipo_folio_serie; }
        }



        #endregion

        #region Constructores

        /// <summary>
        /// Genera una instancia en blanco del tipo SerieFolio
        /// </summary>
        public SerieFolio()
        {
            _id_folio_serie = 0;
            _id_emisor = 0;
            _serie = "";
            _activa = false;
            _folio_inicial = 0;
            _folio_final = 0;
            _tipo_folio_serie = 0;
            _habilitar = false;

        }

        /// <summary>
        ///  Genera una instancia en blanco del tipo SerieFolio
        /// </summary>
        /// <param name="id_folio_serie"></param>
        public SerieFolio(int id_folio_serie)
        {
            cargaAtributosInstancia(id_folio_serie);
        }


        /// <summary>
        ///  Genera una instancia en blanco del tipo SerieFolio ligado una Serie y un Emisor
        /// </summary>
        /// <param name="_serie"></param>
        /// <param name="id_emisor"></param>
        public SerieFolio(string serie, int id_emisor)
        {
            cargaAtributosInstancia(serie, id_emisor);
        }



        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~SerieFolio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instancia, consultando a BD
        /// </summary>
        /// <param name="id_folio_serie"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_folio_serie)
        {
            //Declarando variable de resultado
            bool resultado = false;

            //Declarando arreglo de objetos para la consulta
            object[] parametros = { 3, id_folio_serie, 0, "", false, 0, 0, 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Verificamos que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {

                        this._id_folio_serie = Convert.ToInt32(r["Id"]);
                        this._id_emisor = Convert.ToInt32(r["IdEmisor"]);
                        this._serie = r["Serie"].ToString();
                        this._activa = Convert.ToBoolean(r["Activa"]);
                        this._folio_inicial = Convert.ToInt32(r["FolioInicial"]);
                        this._folio_final = Convert.ToInt32(r["FolioFinal"]);
                        this._tipo_folio_serie = Convert.ToByte(r["TipoFolio"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Validando asignación de atributos
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado de la carga
            return resultado;
        }



        /// <summary>
        /// Realiza la carga de los atributos de la instancia Serie Folio
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="id_emisor"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string serie, int id_emisor)
        {
            //Declarando variable de resultado
            bool resultado = false;

            //Declarando arreglo de objetos para la consulta
            object[] parametros = { 4, 0, id_emisor, serie, false, 0, 0, 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Verificamos que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {

                        this._id_folio_serie = Convert.ToInt32(r["Id"]);
                        this._id_emisor = Convert.ToInt32(r["IdEmisor"]);
                        this._serie = r["Serie"].ToString();
                        this._activa = Convert.ToBoolean(r["Activa"]);
                        this._folio_inicial = Convert.ToInt32(r["FolioInicial"]);
                        this._folio_final = Convert.ToInt32(r["FolioFinal"]);
                        this._tipo_folio_serie = Convert.ToByte(r["TipoFolio"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Validando asignación de atributos
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado de la carga
            return resultado;
        }

        /// <summary>
        /// Edita un registro de Tipo Serie Y folio
        /// </summary>
        /// <param name="id_emisor"></param>
        /// <param name="serie"></param>
        /// <param name="activa"></param>
        /// <param name="folio_inicial"></param>
        /// <param name="folio_final"></param>
        /// <param name="tipo_folio_serie"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaSerieFolio(int id_emisor, string serie, bool activa, int folio_inicial,
                                                    int folio_final, byte tipo_folio_serie, int id_usuario, bool habilitar)
        {
            //Declarando arreglo de objetos para la consulta
            object[] parametros = { 2, this._id_folio_serie, id_emisor, serie, activa, folio_inicial, folio_final, tipo_folio_serie, id_usuario, habilitar, "", "" };
            //Realizando la modificacion del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, parametros);
        }



        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Metodo encargado de la insercion de un registro
        /// </summary>
        /// <param name="id_emisor"></param>
        /// <param name="serie"></param>
        /// <param name="activa"></param>
        /// <param name="folio_inicial"></param>
        /// <param name="folio_final"></param>
        /// <param name="tipo_folio_serie"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSerieFolio(int id_emisor, string serie, bool activa, int folio_inicial,
                                                    int folio_final, tipo tipo_folio_serie, int id_usuario)
        {
            //Declarando arreglo de objetos para la consulta
            object[] parametros = { 1, 0, id_emisor, serie, activa, folio_inicial, folio_final, tipo_folio_serie, id_usuario, true, "", "" };
            //Realizando la modificacion del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, parametros);
        }

        /// <summary>
        /// Metodo encargado de editar un Registro
        /// </summary>
        /// <param name="id_emisor"></param>
        /// <param name="serie"></param>
        /// <param name="activa"></param>
        /// <param name="folio_inicial"></param>
        /// <param name="folio_final"></param>
        /// <param name="tipo_folio_serie"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaSerieFolio(int id_emisor, string serie, bool activa, int folio_inicial,
                                                int folio_final, tipo tipo_folio_serie, int id_usuario)
        {
            //Realizando la modificacion del registro
            return editaSerieFolio(id_emisor, serie, activa, folio_inicial, folio_final, (byte)tipo_folio_serie,
                                   id_usuario, true);
        }


        /// <summary>
        /// Deshabilita una Serie y Folio
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSerieFolio(int id_usuario)
        {
            //Realizando la modificacion del registro
            return editaSerieFolio(this._id_emisor, this._serie, this._activa, this._folio_inicial, this._folio_final, this._tipo_folio_serie,
                                   id_usuario, false);
        }

        #endregion


    }
}
