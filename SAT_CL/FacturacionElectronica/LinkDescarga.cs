using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{   /// <summary>
    /// Clase que muestra los Estados y Comportamientos de la misma
    /// </summary>
    public class LinkDescarga : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_link_descarga_tld";

        private int _idLinkDescarga;
        /// <summary>
        /// Obtiene el Id del Link de descarga de la instancia
        /// </summary>
        public int idLinkDescarga
        {
            get { return this._idLinkDescarga; }
        }

        private int _idContacto;
        /// <summary>
        /// Obtiene el Id del Contacto de la instancia
        /// </summary>
        public int idContacto
        {
            get { return this._idContacto; }
        }

        private DateTime _fechaGeneracion;
        /// <summary>
        /// Obtiene la fecha de generación del link representado en la instancia
        /// </summary>
        public DateTime fechaGeneracion
        {
            get { return this._fechaGeneracion; }
        }

        private DateTime _fechaCaducidad;
        /// <summary>
        /// Obtiene la fecha de caducidad del link representado en la instancia
        /// </summary>
        public DateTime fechaCaducidad
        {
            get { return this._fechaCaducidad; }
        }

        private int _descargasRestantes;
        /// <summary>
        /// Obtiene la cantidad de usos restantes del link de descarga
        /// </summary>
        public int descargasRestantes
        {
            get { return this._descargasRestantes; }
        }

        private bool _requierePDF;
        /// <summary>
        /// Determina si el link requiere también la descarga del archivo PDF
        /// </summary>
        public bool requierePDF
        {
            get { return this._requierePDF; }
        }

        private int _id_usuario;
        /// <summary>
        /// Obtiene el Id de Usuario
        /// </summary>
        public int id_usuario
        {
            get { return this._id_usuario; }
        }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el Id de usuario
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Contructores
        /// <summary>
        /// Constructor encargado de Inicializar los Valores por Default
        /// </summary>
        public LinkDescarga()
        {   //Invoca Método de carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Valores en base a un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public LinkDescarga(int id_registro)
        {   //Invoca Método de carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor de la Clase LinkDescarga
        /// </summary>
        ~LinkDescarga()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        private void cargaAtributosInstancia()
        {   //Asignando Valores por Default
            this._idLinkDescarga = 0;
            this._idContacto = 0;
            this._fechaGeneracion = DateTime.MinValue;
            this._fechaCaducidad = DateTime.MinValue;
            this._descargasRestantes = 0;
            this._requierePDF = false;
            this._id_usuario = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Atributos de un Id dado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Variable de Retorno
            bool result = false;
            //Parametros del SP
            object[] param = { 3, id_registro, 0, null, null, 0, false, 0, false, "", "" };
            //Obteniendo datos del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando que existan datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {   //Recorriendo todos los registros devueltos
                    foreach (DataRow fila in DS.Tables["Table"].Rows)
                    {   //Asignando atributos de la instancia
                        this._idLinkDescarga = id_registro;
                        this._idContacto = Convert.ToInt32(fila["IdContacto"]);
                        DateTime.TryParse(fila["FechaGeneracion"].ToString(), out this._fechaGeneracion);
                        DateTime.TryParse(fila["FechaCaducidad"].ToString(), out this._fechaCaducidad);
                        this._descargasRestantes = Convert.ToInt32(fila["DescargasRestantes"]);
                        this._requierePDF = Convert.ToBoolean(fila["PFD"]);
                        this._id_usuario = Convert.ToInt32(fila["IdUsuario"]);
                        this._habilitar = Convert.ToBoolean(fila["Habilitar"]);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Todas las Actualizaciones
        /// </summary>
        /// <param name="idContacto">Id de Contacto</param>
        /// <param name="fechaGeneracion">Fecha de Generación</param>
        /// <param name="fechaCaducidad">Fecha de Caducidad</param>
        /// <param name="descargasRestantes">Descargas Restantes</param>
        /// <param name="requierePDF">Requiere PDF</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaLinkDescarga(int idContacto, DateTime fechaGeneracion,
                                        DateTime fechaCaducidad, int descargasRestantes,
                                        bool requierePDF, int idUsuario, bool habilitar)
        {   //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Parametros
            object[] param = { 2, this._idLinkDescarga, idContacto, fechaGeneracion, fechaCaducidad,
                               descargasRestantes, requierePDF, idUsuario, habilitar, "", "" };
            //Regresando Variable de Retorno
            return result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
        }


        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método Público encargado de la Insercción de Registros
        /// </summary>
        /// <param name="idContacto">Id Contacto</param>
        /// <param name="fechaGeneracion">Fecha de Generación</param>
        /// <param name="fechaCaducidad">Fecha de Caducidad</param>
        /// <param name="descargasRestantes">Descargas Restantes</param>
        /// <param name="requierePDF">Requiere PDF</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaLinkDescarga(int idContacto, DateTime fechaGeneracion,
                                        DateTime fechaCaducidad, int descargasRestantes,
                                        bool requierePDF, int idUsuario)
        {   //Inicialziando parrámetros
            object[] param = { 1, 0, idContacto, fechaGeneracion, fechaCaducidad, 
                                 descargasRestantes, requierePDF, idUsuario, true, "", "" };
            //Declarando Variable de Retorno
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Regresando Resultado de la Operación
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Atributos
        /// </summary>
        /// <param name="idContacto">Id Contacto</param>
        /// <param name="fechaGeneracion">Fecha de Generación</param>
        /// <param name="fechaCaducidad">Fecha de Caducidad</param>
        /// <param name="descargasRestantes">Descargas Restantes</param>
        /// <param name="requierePDF">Requiere PDF</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaLinkDescarga(int idContacto, DateTime fechaGeneracion,
                                        DateTime fechaCaducidad, int descargasRestantes,
                                        bool requierePDF, int idUsuario//, SqlTransaction transaccion
                                        )
        {   //Regresando Resultado del SP
            return editaLinkDescarga(idContacto, fechaGeneracion, fechaCaducidad, descargasRestantes,
                                        requierePDF, idUsuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Actualizar los Registros
        /// </summary>
        /// <param name="idUsuario">Id de Link de descarga</param>
        /// <returns></returns>
        public bool ActualizaLinkDescarga(int idLinkDescarga)
        {
            return this.cargaAtributosInstancia(idLinkDescarga);
        }


        /// <summary>
        /// Método Público encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaLinkDescarga(int idUsuario)
        {
            return this.editaLinkDescarga(this._idContacto, this._fechaGeneracion,
                                        this._fechaCaducidad, this._descargasRestantes,
                                        this._requierePDF, idUsuario, false);
        }

        /*
        /// <summary>
        /// Método Público encargado de la Insercción de Registros
        /// </summary>
        /// <param name="idContacto">Id Contacto</param>
        /// <param name="fechaGeneracion">Fecha de Generación</param>
        /// <param name="fechaCaducidad">Fecha de Caducidad</param>
        /// <param name="descargasRestantes">Descargas Restantes</param>
        /// <param name="requierePDF">Requiere PDF</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <returns></returns>
        public static int InsertaLinkDescarga(int idContacto,
                                DateTime fechaGeneracion, DateTime fechaCaducidad,
                                int descargasRestantes, bool requierePDF,
                                int idUsuario)
        {   //Inicialziando parrámetros
            object[] param = { 1, 0, idContacto, fechaGeneracion, fechaCaducidad, 
                                 descargasRestantes, requierePDF, idUsuario, true, "", "" };
            //Declarando e inicializando variable de retorno
            int idLink = 0;
            //Insertando registro en BD
            try
            {   //Insertando el nuevo registro
                idLink = Convert.ToInt32(CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param));
            }
            //En caso de error
            catch (Exception ex)
            {   //Registrando error
                //clExcepciones.RegistraExcepcionXML("~/Errores/ReporteErrores.xml", ex);
            }
            //Devolviendo id de registro insertado
            return idLink;
        }
        /// <summary>
        /// Método Público encargado de la Insercción de Registros
        /// </summary>
        /// <param name="idContacto">Id Contacto</param>
        /// <param name="fechaGeneracion">Fecha de Generación</param>
        /// <param name="fechaCaducidad">Fecha de Caducidad</param>
        /// <param name="descargasRestantes">Descargas Restantes</param>
        /// <param name="requierePDF">Requiere PDF</param>
        /// <param name="idUsuario">Id de Usuario</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public static int InsertaLinkDescarga(int idContacto, 
                                DateTime fechaGeneracion, DateTime fechaCaducidad, 
                                int descargasRestantes, bool requierePDF, 
                                int idUsuario, SqlTransaction transaccion)
        {   //Inicialziando parrámetros
            object[] param = { 1, 0, idContacto, fechaGeneracion, fechaCaducidad, 
                                 descargasRestantes, requierePDF, idUsuario, true, "", "" };
            //Declarando e inicializando variable de retorno
            int idLink = 0;
            //Insertando registro en BD
            try
            {   //Insertando el nuevo registro
                idLink = Convert.ToInt32(CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param, transaccion));
            }
            //En caso de error
            catch (Exception ex)
            {   //Registrando error
                //clExcepciones.RegistraExcepcionXML("~/Errores/ReporteErrores.xml", ex);
            }
            //Devolviendo id de registro insertado
            return idLink;
        }
        /// <summary>
        /// Obtiene un arreglo con los Id de comprobantes involucrados en la descarga de archivos de la instancia
        /// </summary>
        /// <returns></returns>
        public int[] ObtieneRutasDescarga()
        {   //Inicialziando parámetros para consulta a la BD
            object[] param = {4, this._idLinkDescarga, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, 0, false, "", ""};
            //Obteniendo los datos desde la BD
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {   //Inicializnado variable de retorno
                    int[] comprobantesDescarga = new int[DS.Tables["Table"].Rows.Count];
                    //Declarando variable indexadora de rutas
                    int indice = 0;
                    //Recorriendo todos los elementos devueltos
                    foreach (DataRow fila in DS.Tables["Table"].Rows)
                    {   //Añadiendo la ruta de descarga
                        comprobantesDescarga[indice] = Convert.ToInt32(fila["IdComprobante"]);
                        //Incrementando el indice
                        indice++;
                    }//Devolvinedo el arreglo de rutas resultante
                    return comprobantesDescarga;
                }
            }

            //Devolviendo arreglo vacio
            return null;
        }*/
        /// <summary>
        /// Genera un link automaticamente con los parametros almacenados en la base de datos
        /// </summary>
        /// <param name="idContacto">Id de contacto</param>
        /// <param name="idComprobantes">Comprobantes que tendra el link</param>
        /// <param name="fechaCaducidad">Parámetro de salida para almacenar la fecha de caducidad del link de descarga</param>
        /// <param name="descargasRestantes">Parámetro de salida para almacenar el máximo de descargas configurado para cada link</param>
        ///  /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion GeneraLinkDescarga(int idContacto, string[] idComprobantes, out DateTime fechaCaducidad, out int descargasRestantes, int id_compania,int id_usuario)
        {
            //Declaramos Objeto Resultado 
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variable para Almacenear el Encabezado del Link Descarga
            int id_descarga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Definiendo los dias de caducidad (Obtenidos desde catálogo de variables en BD)
                int diasCaducidad = Convert.ToInt32(Cadena.VerificaCadenaVacia(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Dias caducidad por Link", id_compania), "1"));

                //Calculando la fecha de caducidad
                fechaCaducidad = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(diasCaducidad);

                //Definiendo las descargas restantes (Obtenidos desde catálogo de variables en BD)
                descargasRestantes = Convert.ToInt32(Cadena.VerificaCadenaVacia(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Máximo descargas por Link", id_compania), "1"));

                //Insertando el registro padre (link descarga)
                resultado = LinkDescarga.InsertaLinkDescarga(idContacto, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), fechaCaducidad, descargasRestantes, true, id_usuario);

                //Asignamos Variable de Encabezado de Descarga
                id_descarga = resultado.IdRegistro;

                //Si se Registro Corectamente
                if (resultado.OperacionExitosa)
                {

                    //Insertando los detalles de descarga
                    foreach (string comprobante in idComprobantes)
                    {
                        //Validamos Insercción del Link Detalle Descarga
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos  qu exista id_comprobante
                            if (comprobante != "")
                            {
                                resultado = DetalleLinkDescarga.InsertarDetalleLinkDescarga(id_descarga, Convert.ToInt32(comprobante), id_usuario);

                            }
                        }
                        else
                        {
                            //Salimos del Ciclo
                            break;
                        }
                    }
                    //Validamo Resultados Exitosos
                    if (resultado.OperacionExitosa)
                    {
                        //Asignamos Id Descarga
                        resultado = new RetornoOperacion(id_descarga);
                        //Completamos transacción
                        scope.Complete();
                    }
                }

            }
            //Devolvinedo resultado de transacción 
            return resultado;
        }


        #endregion
    }
}
