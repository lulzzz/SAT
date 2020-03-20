using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de  Servicio Despacho
    /// </summary>
    public  class ServicioDespacho: Disposable
    {

        /// <summary>
        /// Enumera el Tipo de Carga a realizar
        /// </summary>
        public enum TipoCarga
        {
            /// <summary>
            /// Id Servicio Despacho
            /// </summary>
            IdSericioDespacho = 1,
            /// <summary>
            /// Id Servicio
            /// </summary>
            IdServicio,
        }
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_servicio_despacho_tsd";


        private int _id_servicio_despacho;
        /// <summary>
        /// Describe el Id Servicio Despacho
        /// </summary>
        public int id_servicio_despacho
        {
            get { return _id_servicio_despacho; }
        }
        private int _id_servicio;
        /// <summary>
        /// Describe el Id Servicio
        /// </summary>
        public int id_servicio
        {
            get { return _id_servicio; }
        }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Describe la fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio
        {
            get { return _fecha_inicio; }
        }
        private DateTime _fecha_fin;
        /// <summary>
        /// Describe la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin
        {
            get { return _fecha_fin; }
        }
        private int _id_parada_origen;
        /// <summary>
        /// Describe la Parada Origen
        /// </summary>
        public int id_parada_origen
        {
            get { return _id_parada_origen; }
        }
        private int _id_parada_destino;
        /// <summary>
        /// Describe la Parada Destino
        /// </summary>
        public int id_parada_destino
        {
            get { return _id_parada_destino; }
        }
        private int _id_parada_carga_inicio;
        /// <summary>
        /// Describe la parada carga Inicio
        /// </summary>
        public int id_parada_carga_inicio
        {
            get { return _id_parada_carga_inicio; }
        }
        private int _id_parada_carga_fin;
        /// <summary>
        /// Describe la parada carga fin
        /// </summary>
        public int id_parada_carga_fin
        {
            get { return _id_parada_carga_fin; }
        }
        private decimal _kms_asignados;
        /// <summary>
        /// Describe los kms asignados
        /// </summary>
        public decimal kms_asignados
        {
            get { return _kms_asignados; }
        }
        private decimal _kms_recorridos;
        /// <summary>
        /// Describe los kms recorridos
        /// </summary>
        public decimal kms_recorridos
        {
            get { return _kms_recorridos; }
        }
        private decimal _kms_cargado_recorridos;
        /// <summary>
        /// Describe los kms cargado recorridos
        /// </summary>
        public decimal kms_cargado_recorridos
        {
            get { return _kms_cargado_recorridos; }
        }
        private decimal _kms_vacio_recorridos;
        /// <summary>
        /// Describe los kms vacios recorridos
        /// </summary>
        public decimal kms_vacio_recorridos
        {
            get { return _kms_vacio_recorridos; }
        }
        private decimal _kms_tronco_recorridos;
        /// <summary>
        /// Describe los kms troncos recorridos
        /// </summary>
        public decimal kms_tronco_recorridos
        {
            get { return _kms_tronco_recorridos; }
        }
        private int _id_unidad_motriz_principal;
        /// <summary>
        /// Describe la unidad motriz principal
        /// </summary>
        public int id_unidad_motriz_principal
        {
            get { return _id_unidad_motriz_principal; }
        }
        private int _id_unidad_arrastre1;
        /// <summary>
        /// Describe la unidad de arrastre 1
        /// </summary>
        public int id_unidad_arrastre1
        {
            get { return _id_unidad_arrastre1; }
        }
        private int _id_unidad_arrastre2;
        /// <summary>
        /// Describe la unidad de arrastre 2
        /// </summary>
        public int id_unidad_arrastre2
        {
            get { return _id_unidad_arrastre2; }
        }
        private int _id_tercero;
        /// <summary>
        /// Describe el Id tercero
        /// </summary>
        public int id_tercero
        {
            get { return _id_tercero; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        private byte[] _row_version;
        /// <summary>
        /// Describe la version del Segmento
        /// </summary>
        public byte[] row_version
        {
            get { return _row_version; }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ServicioDespacho()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public ServicioDespacho()
        {

        }


        /// <summary>
        /// Genera una Instancia de Tipo Servicio Despacho
        /// </summary>
        /// <param name="tipo_carga">Tipo de Carga</param>
        /// <param name="id">Id referente al Tipo de Carga</param>
        public ServicioDespacho(TipoCarga tipo_carga, int id)
        {
            cargaAtributosInstancia(tipo_carga, id);
        }

        /// <summary>
        /// Genera una Instancia de Tipo Servicio Despacho
        /// </summary>
        /// <param name="tipo_carga">Tipo de Carga</param>
        /// <param name="id">Id referente al Tipo de Carga</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(TipoCarga tipo_carga, int id)
        {
            //Declaramos Variables 
            int tipo = 0; //Tipo de Store a ejecutar
            int id_servicio_despacho = 0;
            int id_servicio = 0;
            bool resultado = false;

            //Evaluamos el Tipo de Carga
            switch (tipo_carga)
            {
                case TipoCarga.IdSericioDespacho:
                    //Asignamos valor a la variable
                    id_servicio_despacho = id;
                    tipo = 3;
                    break;
                case TipoCarga.IdServicio:
                    //Asignamos valor a la variable
                    id_servicio = id;
                    tipo = 5;
                    break;
            }


            //Inicializando arreglo de parámetros
            object[] param = { tipo, id_servicio_despacho, id_servicio, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_servicio_despacho = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        DateTime.TryParse(r["FechaInicio"].ToString(), out _fecha_inicio);
                        DateTime.TryParse(r["FechaFin"].ToString(), out _fecha_fin);
                        _id_parada_origen = Convert.ToInt32(r["IdParadaOrigen"]);
                        _id_parada_destino = Convert.ToInt32(r["IdParadaDestino"]);
                        _id_parada_carga_inicio = Convert.ToInt32(r["IdParadaCargoInicio"]);
                        _id_parada_carga_fin = Convert.ToInt32(r["IdParadaCargoFin"]);
                        _kms_asignados = Convert.ToDecimal(r["KmsAsignados"]);
                        _kms_recorridos = Convert.ToDecimal(r["KmsRecorridos"]);
                        _kms_cargado_recorridos = Convert.ToDecimal(r["KmsCargadoRecorridos"]);
                        _kms_vacio_recorridos = Convert.ToDecimal(r["KmsVacioRecorridos"]);
                        _kms_tronco_recorridos = Convert.ToDecimal(r["KmsTroncoRecorridos"]);
                        _id_unidad_motriz_principal = Convert.ToInt32(r["IdUnidadMotrizPrincipal"]);
                        _id_unidad_arrastre1 = Convert.ToInt32(r["IdUnidadArrastre1"]);
                        _id_unidad_arrastre2 = Convert.ToInt32(r["IdUnidadArrastre2"]);
                        _id_tercero = Convert.ToInt32(r["IdTercero"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];
                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }



        #endregion

        #region Metodos privados

        /// <summary>
        /// Método encargado de Editar un Servicio Despacho
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece</param>
        /// <param name="fecha_inicio">Fecha Inicio del Viaje</param>
        /// <param name="fecha_fin">Fecha Fin del Viaje</param>
        /// <param name="id_parada_origen">Id Parada Origen donde nace el viaje</param>
        /// <param name="id_parada_destino">Id Parada Destino donde termina el viaje</param>
        /// <param name="id_parada_carga_inicio">Id parada de carga inicial del viaje</param>
        /// <param name="id_parada_carga_fin">Id Parada Final del Viaje</param>
        /// <param name="kms_asignados">kms asignados al viaje</param>
        /// <param name="kms_recorridos">Total de kms recorridos del viaje</param>
        /// <param name="kms_cargado_recorridos">Total de kms cargados recorridos del viaje</param>
        /// <param name="kms_vacio_recorridos">Total de kms vacios recorridos del viaje</param>
        /// <param name="kms_tronco_recorridos">Total de kms troncos recorridos del viaj</param>
        /// <param name="id_unidad_motriz_principal">Id Unidad de Motriz Principal, que recorrio mas kilometros dentro del viaje</param>
        /// <param name="id_unidad_arrastre1">Id Unidad de Arratre 1</param>
        /// <param name="id_unidad_arrastre2">Id Unidad de Arratre 2</param>
        /// <param name="id_tercero">Id tercero que realizó el viaje, en caso de existir</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaServicioDespacho(int id_servicio, DateTime fecha_inicio, DateTime fecha_fin, int id_parada_origen, int id_parada_destino,
                                                       int id_parada_carga_inicio, int id_parada_carga_fin, decimal kms_asignados, decimal kms_recorridos,
                                                       decimal kms_cargado_recorridos, decimal kms_vacio_recorridos, decimal kms_tronco_recorridos, int id_unidad_motriz_principal,
                                                       int id_unidad_arrastre1, int id_unidad_arrastre2, int id_tercero, int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = {2, this._id_servicio_despacho, id_servicio, Fecha.ConvierteDateTimeObjeto(fecha_inicio), Fecha.ConvierteDateTimeObjeto(fecha_fin), id_parada_origen, id_parada_destino, id_parada_carga_inicio,
                                 id_parada_carga_fin, kms_asignados, kms_recorridos, kms_cargado_recorridos, kms_vacio_recorridos, kms_tronco_recorridos, 
                                 id_unidad_motriz_principal, id_unidad_arrastre1, id_unidad_arrastre2, id_tercero, id_usuario, habilitar,this._row_version, "", ""};

                //Establecemos Resultado
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
            else
            {
                //Establecmeos Error
                resultado = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            }
            return resultado;
        }

        /// <summary>
        /// Validamos versión de Registro desde la Base de Datos y Instancia creada
        /// </summary>
        /// <returns></returns>
        private bool validaVersionRegistro()
        {
            //Declaramos Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 4, this._id_servicio_despacho, 0, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, this._row_version, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Establecemos Resultado correcto
                    resultado = true;
            }
            return resultado;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        ///  Método encargado de Insertar un Servcio Despacho
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece</param>
        /// <param name="fecha_inicio">Fecha Inicio del Viaje</param>
        /// <param name="fecha_fin">Fecha Fin del Viaje</param>
        /// <param name="id_parada_origen">Id Parada Origen donde nace el viaje</param>
        /// <param name="id_parada_destino">Id Parada Destino donde termina el viaje</param>
        /// <param name="id_parada_carga_inicio">Id parada de carga inicial del viaje</param>
        /// <param name="id_parada_carga_fin">Id Parada Final del Viaje</param>
        /// <param name="kms_asignados">kms asignados al viaje</param>
        /// <param name="kms_recorridos">Total de kms recorridos del viaje</param>
        /// <param name="kms_cargado_recorridos">Total de kms cargados recorridos del viaje</param>
        /// <param name="kms_vacio_recorridos">Total de kms vacios recorridos del viaje</param>
        /// <param name="kms_tronco_recorridos">Total de kms troncos recorridos del viaj</param>
        /// <param name="id_unidad_motriz_principal">Id Unidad de Motriz Principal, que recorrio mas kilometros dentro del viaje</param>
        /// <param name="id_unidad_arrastre1">Id Unidad de Arratre 1</param>
        /// <param name="id_unidad_arrastre2">Id Unidad de Arratre 2</param>
        /// <param name="id_tercero">Id tercero que realizó el viaje, en caso de existir</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioDespacho(int id_servicio, DateTime fecha_inicio, DateTime fecha_fin, int id_parada_origen, int id_parada_destino,
                                                       int id_parada_carga_inicio, int id_parada_carga_fin, decimal kms_asignados, decimal kms_recorridos,
                                                       decimal kms_cargado_recorridos, decimal kms_vacio_recorridos, decimal kms_tronco_recorridos, int id_unidad_motriz_principal,
                                                       int id_unidad_arrastre1, int id_unidad_arrastre2, int id_tercero, int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1,0, id_servicio,Fecha.ConvierteDateTimeObjeto(fecha_inicio), Fecha.ConvierteDateTimeObjeto(fecha_fin), id_parada_origen, id_parada_destino, id_parada_carga_inicio,
                                 id_parada_carga_fin, kms_asignados, kms_recorridos, kms_cargado_recorridos, kms_vacio_recorridos, kms_tronco_recorridos, 
                                 id_unidad_motriz_principal, id_unidad_arrastre1, id_unidad_arrastre2, id_tercero, id_usuario, true,null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        ///  Método encargado de Insertar un Servcio Despacho
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece</param>
        /// <param name="fecha_inicio">Fecha Inicio del Viaje</param>
        /// <param name="fecha_fin">Fecha Fin del Viaje</param>
        /// <param name="id_parada_origen">Id Parada Origen donde nace el viaje</param>
        /// <param name="id_parada_destino">Id Parada Destino donde termina el viaje</param>
        /// <param name="id_parada_carga_inicio">Id parada de carga inicial del viaje</param>
        /// <param name="id_parada_carga_fin">Id Parada Final del Viaje</param>
        /// <param name="kms_asignados">kms asignados al viaje</param>
        /// <param name="kms_recorridos">Total de kms recorridos del viaje</param>
        /// <param name="kms_cargado_recorridos">Total de kms cargados recorridos del viaje</param>
        /// <param name="kms_vacio_recorridos">Total de kms vacios recorridos del viaje</param>
        /// <param name="kms_tronco_recorridos">Total de kms troncos recorridos del viaj</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioDespacho(int id_servicio, DateTime fecha_inicio, DateTime fecha_fin, int id_parada_origen, int id_parada_destino,
                                                       int id_parada_carga_inicio, int id_parada_carga_fin, decimal kms_asignados, decimal kms_recorridos,
                                                       decimal kms_cargado_recorridos, decimal kms_vacio_recorridos, decimal kms_tronco_recorridos,int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1,0, id_servicio,Fecha.ConvierteDateTimeObjeto(fecha_inicio), Fecha.ConvierteDateTimeObjeto(fecha_fin), id_parada_origen, id_parada_destino, id_parada_carga_inicio,
                                 id_parada_carga_fin, kms_asignados, kms_recorridos, kms_cargado_recorridos, kms_vacio_recorridos, kms_tronco_recorridos, 
                                 0, 0, 0, 0, id_usuario, true,null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de editar un Servicio Despacho
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece</param>
        /// <param name="fecha_inicio">Fecha Inicio del Viaje</param>
        /// <param name="fecha_fin">Fecha Fin del Viaje</param>
        /// <param name="id_parada_origen">Id Parada Origen donde nace el viaje</param>
        /// <param name="id_parada_destino">Id Parada Destino donde termina el viaje</param>
        /// <param name="id_parada_carga_inicio">Id parada de carga inicial del viaje</param>
        /// <param name="id_parada_carga_fin">Id Parada Final del Viaje</param>
        /// <param name="kms_asignados">kms asignados al viaje</param>
        /// <param name="kms_recorridos">Total de kms recorridos del viaje</param>
        /// <param name="kms_cargado_recorridos">Total de kms cargados recorridos del viaje</param>
        /// <param name="kms_vacio_recorridos">Total de kms vacios recorridos del viaje</param>
        /// <param name="kms_tronco_recorridos">Total de kms troncos recorridos del viaj</param>
        /// <param name="id_unidad_motriz_principal">Id Unidad de Motriz Principal, que recorrio mas kilometros dentro del viaje</param>
        /// <param name="id_unidad_arrastre1">Id Unidad de Arratre 1</param>
        /// <param name="id_unidad_arrastre2">Id Unidad de Arratre 2</param>
        /// <param name="id_tercero">Id tercero que realizó el viaje, en caso de existir</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioDespacho(int id_servicio, DateTime fecha_inicio, DateTime fecha_fin, int id_parada_origen, int id_parada_destino,
                                                       int id_parada_carga_inicio, int id_parada_carga_fin, decimal kms_asignados, decimal kms_recorridos,
                                                       decimal kms_cargado_recorridos, decimal kms_vacio_recorridos, decimal kms_tronco_recorridos, int id_unidad_motriz_principal,
                                                       int id_unidad_arrastre1, int id_unidad_arrastre2, int id_tercero, int id_usuario)
        {
            return this.editaServicioDespacho(id_servicio, fecha_inicio, fecha_fin, id_parada_origen, id_parada_destino, id_parada_carga_inicio, id_parada_carga_fin, kms_asignados,
                                     kms_recorridos, kms_cargado_recorridos, kms_vacio_recorridos, kms_tronco_recorridos, id_unidad_motriz_principal,
                                      id_unidad_arrastre1, id_unidad_arrastre2, id_tercero, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la actualización de la parada y fecha de inicio del servicio
        /// </summary>
        /// <param name="id_parada_origen">Id de parada de origen</param>
        /// <param name="fecha_inicio">Fecha de Inicio del servicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaParadaOrigenServicio(int id_parada_origen, DateTime fecha_inicio, int id_usuario)
        {
            //Actualizando y devolviendo resultado
            return editaServicioDespacho(this._id_servicio, fecha_inicio, this._fecha_fin, id_parada_origen,
                                        this._id_parada_destino, id_parada_origen, this._id_parada_carga_fin, this._kms_asignados,
                                        this._kms_recorridos, this._kms_cargado_recorridos, this._kms_vacio_recorridos,
                                        this._kms_tronco_recorridos, this._id_unidad_motriz_principal, this._id_unidad_arrastre1, 
                                        this._id_unidad_arrastre2, this._id_tercero, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la actualización de la parada, fecha y recursos finales del servicio
        /// </summary>
        /// <param name="id_parada_destino">Id de parada de destino</param>
        /// <param name="fecha_fin">Fecha de Fin de la parada</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaParadaDestinoServicio(int id_parada_destino, DateTime fecha_fin, int id_usuario)
        {
            //Declarando variables auxiliareas para obtencion de recursos principales del servicio
            int id_motriz_principal = 0, id_arrastre_1 = 0, id_arrastre_2 = 0, id_tercero = 0;

            //Obtenemos Unidades de Arratre Principales
            MovimientoAsignacionRecurso.ObtieneRecursoArrastrePrincipalServicio(this._id_servicio, out id_arrastre_1, out id_arrastre_2);
            //Motriz
            id_motriz_principal = MovimientoAsignacionRecurso.ObtieneRecursoPrincipalServicio(this._id_servicio, MovimientoAsignacionRecurso.Tipo.Unidad, 1);
            //Tercero
            id_tercero = MovimientoAsignacionRecurso.ObtieneRecursoPrincipalServicio(this._id_servicio, MovimientoAsignacionRecurso.Tipo.Tercero, 0);

            //Actualizando y devolviendo resultado
            return editaServicioDespacho(this._id_servicio, this._fecha_inicio, fecha_fin, this._id_parada_origen,
                                        id_parada_destino, this._id_parada_carga_inicio, id_parada_destino, this._kms_asignados,
                                        this._kms_recorridos, this._kms_cargado_recorridos, this._kms_vacio_recorridos,
                                        this._kms_tronco_recorridos, id_motriz_principal, id_arrastre_1, id_arrastre_2, 
                                        id_tercero, id_usuario, this._habilitar);                                        
        }


        /// <summary>
        /// Deshabilita un Servicio Despacho
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioDespacho(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            return this.editaServicioDespacho(this._id_servicio, this._fecha_inicio, this._fecha_fin, this._id_parada_origen, this._id_parada_destino, this._id_parada_carga_inicio, this._id_parada_carga_fin, this._kms_asignados,
                                     this._kms_recorridos, this._kms_cargado_recorridos, this._kms_vacio_recorridos, this._kms_tronco_recorridos, this._id_unidad_motriz_principal,
                                      this._id_unidad_arrastre1, this._id_unidad_arrastre2, this._id_tercero, id_usuario, false);

        }

        /// <summary>
        /// Actualizamos el kilometraje recorrido del servicio
        /// </summary>
        /// <param name="kms_recorridos_movimiento">kms reccoridos del Movimiento terminado</param>
        /// <param name="kms_cargado_recorridos_movimiento">kms cargados recorridos del movimiento terminado</param>
        /// <param name="kms_vacio_recorridos_movimiento">kms vacios recorridos del movimiento terminado</param>
        /// <param name="kms_tronco_recorridos_movimiento">kms tronco recorridos del movimiento terminado</param>
        /// <param name="id_usuario">id usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaKimometrajeRecorridosServicioDespacho(decimal kms_recorridos_movimiento, decimal kms_cargado_recorridos_movimiento, decimal kms_vacio_recorridos_movimiento,
                                                                             decimal kms_tronco_recorridos_movimiento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            return this.editaServicioDespacho(this._id_servicio, this._fecha_inicio, this._fecha_fin, this._id_parada_origen, this._id_parada_destino, this._id_parada_carga_inicio, this._id_parada_carga_fin, this._kms_asignados,
                                   this._kms_recorridos +  kms_recorridos_movimiento, this._kms_cargado_recorridos + kms_cargado_recorridos_movimiento,
                                   this._kms_vacio_recorridos + kms_vacio_recorridos_movimiento, this._kms_tronco_recorridos + kms_tronco_recorridos_movimiento,
                                   this._id_unidad_motriz_principal,
                                   this._id_unidad_arrastre1, this._id_unidad_arrastre2, this._id_tercero, id_usuario, this._habilitar);

        }
        /// <summary>
        /// Actualizamos el kilometraje recorrido del servicio
        /// </summary>
        /// <param name="kms_recorridos_movimiento">kms reccoridos del Movimiento terminado</param>
        /// <param name="kms_cargado_recorridos_movimiento">kms cargados recorridos del movimiento terminado</param>
        /// <param name="kms_vacio_recorridos_movimiento">kms vacios recorridos del movimiento terminado</param>
        /// <param name="kms_tronco_recorridos_movimiento">kms tronco recorridos del movimiento terminado</param>
        /// <param name="id_usuario">id usuario</param>
        /// <returns></returns>
        public RetornoOperacion RestaKimometrajeRecorridosServicioDespacho(decimal kms_recorridos_movimiento, decimal kms_cargado_recorridos_movimiento, decimal kms_vacio_recorridos_movimiento,
                                                                             decimal kms_tronco_recorridos_movimiento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            return this.editaServicioDespacho(this._id_servicio, this._fecha_inicio, this._fecha_fin, this._id_parada_origen, this._id_parada_destino, this._id_parada_carga_inicio, this._id_parada_carga_fin, this._kms_asignados,
                                   this._kms_recorridos - kms_recorridos_movimiento, this._kms_cargado_recorridos - kms_cargado_recorridos_movimiento,
                                   this._kms_vacio_recorridos - kms_vacio_recorridos_movimiento, this._kms_tronco_recorridos - kms_tronco_recorridos_movimiento,
                                   this._id_unidad_motriz_principal,
                                   this._id_unidad_arrastre1, this._id_unidad_arrastre2, this._id_tercero, id_usuario, this._habilitar);

        }

        /// <summary>
        /// Método  encargado de Actualizar los atributos Servicio Despacho
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioDespacho(ServicioDespacho.TipoCarga Tipo)
        {
            //Declaramos variable para almacenar el Id
            int id = this._id_servicio_despacho;
            //Validamos Tipo de Carga
            if (Tipo == TipoCarga.IdServicio)
            {
                //Establecemos Variable
                id = this._id_servicio;
            }
            return this.cargaAtributosInstancia(Tipo, id);
        }


        /// <summary>
        /// Método encarga de Obtener el Kilometraje de Cobro
        /// </summary>
        /// <returns></returns>
        public decimal ObtieneKilometrajeCobro()
        {
            //Declaramos Resultados
            decimal kmsCobro = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 7, 0, this._id_servicio, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    kmsCobro = (from DataRow r in ds.Tables[0].Rows
                                select Convert.ToInt32(r["KmsCobro"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return kmsCobro;
        }

        /// <summary>
        /// Método encargado de Obtener el Kilometraje de Pago
        /// </summary>
        /// <returns></returns>
        public decimal ObtieneKilometrajePago()
        {
            //Declaramos Resultados
            decimal kmsPago = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, this._id_servicio, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    kmsPago = (from DataRow r in ds.Tables[0].Rows
                               select Convert.ToInt32(r["KmsPago"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return kmsPago;
        }
        /// <summary>
        /// Método encargado de Obtener la Información del Viaje
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneInformacionViaje(int id_servicio, int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtInformacionServicio = null;

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, id_servicio, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, id_compania, "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtInformacionServicio = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtInformacionServicio;
        }

        #endregion
        
    }
}
