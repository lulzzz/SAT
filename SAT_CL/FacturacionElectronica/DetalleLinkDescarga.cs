using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Realiza las acciones posibles sobre registros Detalle Link de Descarga
    /// </summary>
    public class DetalleLinkDescarga : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_link_descarga_detalle_tdl";

        private int _idDetalleLinkDescarga;
        /// <summary>
        /// Obtiene el Id de detalle de descarga
        /// </summary>
        public int idDetalleLinkDescarga
        {
            get { return this._idDetalleLinkDescarga; }
        }

        private int _idLinkDescarga;
        /// <summary>
        /// Obtiene el Id de Link de descarga
        /// </summary>
        public int idLinkDescarga
        {
            get { return this._idLinkDescarga; }
        }

        private int _idComprobante;
        /// <summary>
        /// Obtiene el Id de CFD
        /// </summary>
        public int idComprobante
        {
            get { return this._idComprobante; }
        }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el Estatus de Habilitado
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Genera una nueva instancia "DetalleLinkDescarga" con los datos Vacios
        /// </summary>
        public DetalleLinkDescarga()
        {   //Inicialziando valor de Id de registro
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Genera una nueva instancia "DetalleLinkDescarga" a partir de su Id
        /// </summary>
        /// <param name="idDetalle">Id Detalle</param>
        public DetalleLinkDescarga(int idDetalle)
        {   //Inicialziando valor de Id de registro
            cargaAtributosInstancia(idDetalle);
        }


        #endregion

        #region Destructor

        ~DetalleLinkDescarga()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Inicializa los valores de atributo de instancia
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando atributos de la instancia
            this._idDetalleLinkDescarga = 0;
            this._idLinkDescarga = 0;
            this._idComprobante = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Inicializa los valores de atributo de instancia a partir de un Id
        /// </summary>
        /// <param name="idDetalleLinkDescarga">Id Detalle descarga</param>
        private bool cargaAtributosInstancia(int idDetalleLinkDescarga)
        {   //Declarando Variable de Retorno
            bool result = false;
            //Inicializando arreglo de parámetros para carga de registro desde BD
            object[] parametros = { 3, idDetalleLinkDescarga, 0, 0, 0, false, "", "" };
            //Obteniendo datos del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, parametros))
            {   //Validando que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {   //Recorriendo todos los registros devueltos
                    foreach (DataRow fila in DS.Tables["Table"].Rows)
                    {   //Asignando atributos de la instancia
                        this._idDetalleLinkDescarga = idDetalleLinkDescarga;
                        this._idLinkDescarga = Convert.ToInt32(fila["IdLink"]);
                        this._idComprobante = Convert.ToInt32(fila["IdComprobante"]);
                        this._habilitar = Convert.ToBoolean(fila["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar todos los Atributos
        /// </summary>
        /// <param name="idLinkDescarga">Id de Link de Descarga</param>
        /// <param name="idComprobante">Id de Comprobante</param>
        /// <param name="idUsuario">Id de usuario</param>
        /// <returns></returns>
        private RetornoOperacion editaAtributos(int idLinkDescarga, int idComprobante, int idUsuario, bool habilitar)
        {   //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Inicializando arreglo de parámetros para carga de registro desde BD
            object[] parametros = { 2, this._idDetalleLinkDescarga, idLinkDescarga, idComprobante, idUsuario, habilitar, "", "" };
            //Regresando Resultado Obtenido
            return result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, parametros);
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <param name="idDetalleLinkDescarga">Id del Detalle del Link de Descarga</param>
        /// <returns></returns>
        public bool ActualizaDetalleLinkDescarga(int idDetalleLinkDescarga)
        {
            return this.cargaAtributosInstancia(idDetalleLinkDescarga);
        }

        /// <summary>
        /// Método Público encargado de Ingresar los Valores de los Atributos
        /// </summary>
        /// <param name="idLinkDescarga">Id del Link de Descarga</param>
        /// <param name="idComprobante">Id del Comprobante</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarDetalleLinkDescarga(int idLinkDescarga, int idComprobante,
                                        int idUsuario)
        {   //Declarando Parametros del SP
            object[] param = { 1, 0, idLinkDescarga, idComprobante, idUsuario, true, "", "" };
            //Declarando Variable de Retorno
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar los Valores de los Atributos
        /// </summary>
        /// <param name="idLinkDescarga">Id del Link de Descarga</param>
        /// <param name="idComprobante">Id del Comprobante</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        public RetornoOperacion EditarDetalleLinkDescarga(int idLinkDescarga, int idComprobante,
                                        int idUsuario)
        {
            return editaAtributos(idLinkDescarga, idComprobante, idUsuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDetalleLinkDescarga(int idUsuario)
        {
            return editaAtributos(this._idLinkDescarga, this._idComprobante, idUsuario, false);
        }

        /// <summary>
        /// Método encargado de Recuperar el Id del Comprobante dado un Id de Link de Descarga
        /// </summary>
        /// <param name="idLinkDescarga"></param>
        /// <returns></returns>
        public static int RecuperaIdComprobante(int idLinkDescarga)
        {   //Variable de Resultado
            int idComprobante = 0;
            //Parametros de SP
            object[] param = { 4, 0, idLinkDescarga, 0, 0, false, "", "" };
            //Obeniendo Resultado del SP
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando Valores
                    foreach (DataRow dr in DS.Tables["Table"].Rows)
                    {   //Asignando Id de Comprobante
                        idComprobante = Convert.ToInt32(dr["IdComprobante"]);
                    }
            }
            //Regresando Resultado
            return idComprobante;
        }

        /// <summary>
        /// Método encargado de Recuperar el Id del Comprobante dado un Id de Link de Descarga
        /// </summary>
        /// <param name="idLinkDescarga"></param>
        /// <returns></returns>
        public static DataTable RecuperaIdComprobantes(int idLinkDescarga)
        {   //Variable de Resultado
            DataTable mit = null;
            //Parametros de SP
            object[] param = { 4, 0, idLinkDescarga, 0, 0, false, "", "" };
            //Obeniendo Resultado del SP
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando Valores
                    mit = DS.Tables["Table"];
            }
            //Regresando Resultado
            return mit;
        }

        #endregion

    }
}
