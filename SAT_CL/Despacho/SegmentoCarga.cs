using System;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
 
namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de los segmentos de Carga
    /// </summary>
   public  class SegmentoCarga: Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus del Movimiento Asignación
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Iniciado
            /// </summary>
            Iniciado,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado,

        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_segmento_carga_tsc";


        private int _id_segmento_carga;
        /// <summary>
        /// Describe el Id segmento Carga
        /// </summary>
        public int id_segmento_carga
        {
            get { return _id_segmento_carga; }
        }
        private int _id_servicio;
        /// <summary>
        /// Describe el Id de Servicio
        /// </summary>
        public int id_servicio
        {
            get { return _id_servicio; }
        }
        private decimal _secuencia;
        /// <summary>
        /// Describe la secuencia
        /// </summary>
        public decimal secuencia
        {
            get { return _secuencia; }
        }
        private byte _id_estatus_segmento;
        /// <summary>
        /// Describe el estatus segmento
        /// </summary>
        public byte id_estatus_segmento
        {
            get { return _id_estatus_segmento; }
        }
        private int _id_parada_inicio;
        /// <summary>
        /// Describe la parada inicio
        /// </summary>
        public int id_parada_inicio
        {
            get { return _id_parada_inicio; }
        }
        private int _id_parada_fin;
        /// <summary>
        /// Describe la parada fin
        /// </summary>
        public int id_parada_fin
        {
            get { return _id_parada_fin; }
        }
        private int _id_ruta;
        /// <summary>
        /// Describe la Ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        /// <summary>
        /// Enumera el Estatus Segmento de Carga
        /// </summary>
        public Estatus EstatusSegmento
        {
            get { return (Estatus)_id_estatus_segmento; }
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
        ~SegmentoCarga()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public SegmentoCarga()
        {

        }


        /// <summary>
        ///  Genera una Instancia de Tipo Segmento Carga
        /// </summary>
        /// <param name="id_segmento_carga">Id Segmento de Carga</param>
        public SegmentoCarga(int id_segmento_carga)
        {
            cargaAtributosInstancia(id_segmento_carga);
        }

        /// <summary>
        ///  Genera una Instancia de Tipo Segmento Carga
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        public SegmentoCarga(int id_servicio, decimal secuencia)
        {
            cargaAtributosInstancia(id_servicio, secuencia);
        }

        /// <summary>
        /// Genera una Instancia de Tipo Segmento Carga
        /// </summary>
        /// <param name="id_segmento_carga">Id Segmento de Carga</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_segmento_carga)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_segmento_carga, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_segmento_carga = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _secuencia = Convert.ToDecimal(r["Secuencia"]);
                        _id_estatus_segmento = Convert.ToByte(r["IdEstatusSegmento"]);
                        _id_parada_inicio = Convert.ToInt32(r["IdPardainicio"]);
                        _id_parada_fin = Convert.ToInt32(r["IdParadaFin"]);
                        _id_ruta = Convert.ToInt32(r["IdRuta"]);
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

        /// <summary>
        /// Genera una Instancia de Tipo Segmento Carga ligando una secuencia
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">secuencia</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio, decimal secuencia)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, id_servicio, secuencia, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_segmento_carga = Convert.ToInt32(r["Id"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _secuencia = Convert.ToDecimal(r["Secuencia"]);
                        _id_estatus_segmento = Convert.ToByte(r["IdEstatusSegmento"]);
                        _id_parada_inicio = Convert.ToInt32(r["IdPardainicio"]);
                        _id_parada_fin = Convert.ToInt32(r["IdParadaFin"]);
                        _id_ruta = Convert.ToInt32(r["IdRuta"]);
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
        ///  Método encargado de Editar un Segmento Carga
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <param name="secuencia">Secuencia del Segmento</param>
        /// <param name="estatus_segmento">Estatus del segmento</param>
        /// <param name="id_parada_inicio">Id Parada Inicio</param>
        /// <param name="id_parada_fin">Id Parada Fin</param>
        /// <param name="id_ruta">Id Ruta al que pertece el segmento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaSegmentoCarga(int id_servicio, decimal secuencia, Estatus estatus_segmento, int id_parada_inicio,
                                                    int id_parada_fin, int id_ruta, int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = {2, this._id_segmento_carga, id_servicio, secuencia, estatus_segmento, id_parada_inicio, id_parada_fin, id_ruta,
                                     id_usuario, habilitar,this._row_version, "", ""};

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
            object[] param = { 4, this._id_segmento_carga, 0, 0, 0, 0, 0, 0, 0, false, this._row_version, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Establecemos Resultado correcto
                    resultado = true;
            }

            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene Ultima Secuencia del Segmento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <returns></returns>
        private static decimal obtieneSecuenciaSegmneto(int id_servicio)
        {
            //Declaramos Resultados
            decimal secuencia = 1;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, id_servicio, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  InicializaDataGridView
                    decimal secuenciaBD = (from DataRow r in ds.Tables[0].Rows
                                           select Convert.ToDecimal(r["Secuencia"])).FirstOrDefault();

                    //Asignamos Secuencia Correspondiente
                    secuencia = secuenciaBD + 1;
                }

            }
            //Obtenemos Resultado
            return secuencia;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar un Segmento de Carga
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <param name="id_parada_inicio">Id Parada Inicio</param>
        /// <param name="id_parada_fin">Id Parada Fin</param>
        /// <param name="id_ruta">Id Ruta al qu epertenece el segmento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSegmentoCarga(int id_servicio, int id_parada_inicio,
                                                    int id_parada_fin, int id_ruta, int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, obtieneSecuenciaSegmneto(id_servicio), Estatus.Registrado, id_parada_inicio, id_parada_fin,id_ruta,
                                     id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Insertar un Segmento de Carga 
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_parada_inicio">Id Parada Inicio</param>
        /// <param name="id_parada_fin">Id Parada Fin</param>
        /// <param name="id_ruta">Id Ruta al qu epertenece el segmento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSegmentoCarga(int id_servicio, decimal secuencia, int id_parada_inicio,
                                                     int id_parada_fin, int id_ruta, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, secuencia, Estatus.Registrado, id_parada_inicio, id_parada_fin,id_ruta,
                                     id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        /// <summary>
        /// Método encargado de Insertar un Segmento de Carga 
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_parada_inicio">Id Parada Inicio</param>
        /// <param name="id_parada_fin">Id Parada Fin</param>
        /// <param name="id_ruta">Id Ruta al qu epertenece el segmento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaSegmentoCarga(int id_servicio, Estatus estatus, decimal secuencia, int id_parada_inicio,
                                                     int id_parada_fin, int id_ruta, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_servicio, secuencia, estatus, id_parada_inicio, id_parada_fin,id_ruta,
                                     id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Deshabilita un Segmento de Carga
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSegmentoCarga(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            // Validamos que el estatus este Registrado
            if ((Estatus)this._id_estatus_segmento == Estatus.Registrado)
            {
                resultado = this.editaSegmentoCarga(this._id_servicio, this._secuencia, (Estatus)this._id_estatus_segmento, this._id_parada_inicio, this._id_parada_fin,
                                                  this._id_ruta, id_usuario, false);

            }
            else
            {
                resultado = new RetornoOperacion("El estatus del segmento no permite su edición ");
            }
            return resultado;
        }

        /// <summary>
        /// Deshabilita un Segmentos de Carga
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaSegmentosDeCarga(int id_servicio, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Segmentos  ligando un Id Servicio
            using (DataTable mitSegmentos = CargaSegmentos(id_servicio))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitSegmentos))
                {
                    //Recorremos cada uno de los segmentos
                    foreach (DataRow r in mitSegmentos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos
                            using (SegmentoCarga objSegmetoCarga = new SegmentoCarga(r.Field<int>("Id")))
                            {
                                //Deshabilitamos Segmento
                                resultado = objSegmetoCarga.DeshabilitaSegmentoCarga(id_usuario);
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
            //Devolvemo resultado
            return resultado;
        }

       /// <summary>
       /// Iniciamos Segmento
       /// </summary>
       /// <param name="id_usuario">Id Usuario</param>
       /// <returns></returns>
       public RetornoOperacion IniciaSegmento(int id_usuario)
        {
           //Declaramos objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
           //Validamos Estatus del Segmento
            if (SegmentoCarga.Estatus.Iniciado != (Estatus)this._id_estatus_segmento)
            {
                //Actualiamos Estatus
                resultado = editaSegmentoCarga(this._id_servicio, this._secuencia, Estatus.Iniciado, this._id_parada_inicio, this._id_parada_fin,
                                          this._id_ruta, id_usuario, this._habilitar);
            }
           else
            {
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("El segmento ya se encuentrá Iniciado.");
            }
           //Devolcemos Resultado
            return resultado;
        }

       /// <summary>
       /// Actualizamos el estatus del Segmento
       /// </summary>
       /// <param name="estatus">Estatus</param>
       /// <param name="id_usuario">Id Usuario</param>
       /// <returns></returns>
       public RetornoOperacion ActualizaEstatusSegmento(Estatus estatus, int id_usuario)
       {
           //Actualiamos Estatus
           return editaSegmentoCarga(this._id_servicio, this._secuencia, estatus, this._id_parada_inicio, this._id_parada_fin,
                                     this._id_ruta, id_usuario, this._habilitar);
       }

       /// <summary>
       /// Terminamos Segmento
       /// </summary>
       /// <param name="id_usuario"></param>
       /// <returns></returns>
       public RetornoOperacion TerminaSegmento(int id_usuario)
       {
           //Deeclaramos objeto Resultado
           RetornoOperacion resultado = new RetornoOperacion();

           //Validamos Estatus
           if (Estatus.Iniciado == (Estatus)this._id_estatus_segmento)
           {
               //Terminamos Segmento
               resultado = editaSegmentoCarga(this._id_servicio, this._secuencia, Estatus.Terminado, this._id_parada_inicio, this._id_parada_fin,
                                        this._id_ruta, id_usuario, this._habilitar);
           }
           else
               resultado = new RetornoOperacion("El estatus del Segmento sebe estar Iniciado para su Terminación.");
               //Devolvemos Resultado
               return resultado;
       }

       /// <summary>
       /// Método encargado de editar un Segmento Carga
       /// </summary>
       /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
       /// <param name="secuencia">Secuencia del Segmento</param>
       /// <param name="estatus_segmento">Estatus del segmento</param>
       /// <param name="id_parada_inicio">Id Parada Inicio</param>
       /// <param name="id_parada_fin">Id Parada Fin</param>
       /// <param name="id_ruta">Id Ruta al qu epertenece el segmento</param>
       /// <param name="id_usuario">Id Usuario</param>
       /// <returns></returns>
       public RetornoOperacion EditaSegmentoCarga(int id_servicio, decimal secuencia, Estatus estatus_segmento, int id_parada_inicio,
                                                   int id_parada_fin, int id_ruta, int id_usuario)
       {
           return this.editaSegmentoCarga(id_servicio, secuencia, estatus_segmento, id_parada_inicio, id_parada_fin, id_ruta, id_usuario,
                                         this._habilitar);
       }

        /// <summary>
        /// Carga segmentos ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio que pertenece al Segmento</param>
        /// <returns></returns>
        public static DataTable CargaSegmentos(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, id_servicio, 0, 0, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Búsca la secuencia del segmento dado un Id de parada inicio y una Id parada fin
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_inicio">Id Parada Inicio</param>
        /// <param name="id_parada_fin">Id Parada Fin</param>
        /// <returns></returns>
        public static decimal BuscaSecuenciaSegmento(int id_servicio, int id_parada_inicio, int id_parada_fin)
        {
            //Declaramos Resultados
            decimal IdSegmento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 7, 0, id_servicio, 0, 0, id_parada_inicio, id_parada_fin, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Parada Posterior
                    IdSegmento = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToDecimal(r["secuencia"])).FirstOrDefault();

                }

            }

            //Obtenemos Resultado
            return IdSegmento;
        }



        /// <summary>
        /// Actualiza la secuencia de los Segmentos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaSecuenciaSegmentos(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, id_servicio, secuencia, 0, 0, 0, 0, id_usuario, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        /// Actualiza la secuencia de los Segmentos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DecrementaSecuenciaSegmentos(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando arreglo de parámetros
            object[] param = { 10, 0, id_servicio, secuencia, 0, 0, 0, 0, id_usuario, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        /// Carga segmentos obteniendo el HI ligando un Id Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static DataTable CargaSegmentosObteniendoHI(int id_servicio, int id_usuario)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 11, 0, id_servicio, 0, 0, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
       /// Buscamos segmento  en estatus iniciado ligando una parada destino
       /// </summary>
       /// <param name="id_servicio">Id Servicio</param>
       /// <param name="id_parada_destino">Id parada Destino</param>
       /// <returns></returns>
        public static int BuscamosSegmentoParadaDestino(int id_servicio, int id_parada_destino)
        {
            //Declaramos variable segmento
            int id_segmento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 12, 0, id_servicio, 0, 0, 0, id_parada_destino, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    id_segmento = (from DataRow r in ds.Tables[0].Rows
                                   select Convert.ToInt32(r["IdSegmento"])).FirstOrDefault();

                }
            }
            //Devolvemos Valor
            return id_segmento;
        }

        /// <summary>
        /// Buscamos segmento  en estatus iniciado ligando una parada origen
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_origen">Id parada Destino</param>
        /// <returns></returns>
        public static int BuscamosSegmentoParadaOrigen(int id_servicio, int id_parada_origen)
        {
            //Declaramos variable segmento
            int id_segmento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 13, 0, id_servicio, 0, 0, id_parada_origen, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    id_segmento = (from DataRow r in ds.Tables[0].Rows
                                   select Convert.ToInt32(r["IdSegmento"])).FirstOrDefault();

                }
            }
            //Devolvemos Valor
            return id_segmento;
        }

        /// <summary>
        /// Método encargado de Obtener los Segmentos con su Hoja de Instrucción Calculada
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable CargaSegmentosHI(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 16, 0, id_servicio, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
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
        /// Método  encargado de Actualizar los atributos Segmento de Carga
        /// </summary>
        /// <returns></returns>
        public bool ActualizaSegmento()
        {
            return this.cargaAtributosInstancia(this._id_segmento_carga);
        }

        /// <summary>
        /// Buscamos HI correspondiente al Segmento
        /// </summary>
        /// <param name="id_segmento">Id Segmento</param>
        /// <returns></returns>
        public static RetornoOperacion ObteniendoHISegmento(int id_segmento)
        {
            //Declaramos variable HI
            RetornoOperacion res = new RetornoOperacion();
            
            //Establecemos Mensaje Error

            //Instanciamos Segmento
            using (SegmentoCarga objSegmento = new SegmentoCarga(id_segmento))
            {
                //Parada Origen y Destino
                using (Parada objParadaOrigen = new Parada(objSegmento.id_parada_inicio), objParadaDestino = new Parada(objSegmento._id_parada_fin))
                {
                    //Mostramos Resultado Error
                    res = new RetornoOperacion("No es posible obtener la HI para el Segmento: " + objParadaOrigen.descripcion + " - " + objParadaDestino.descripcion);
                }
            }
           
            //Inicializando arreglo de parámetros
            object[] param = { 17, id_segmento, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    int hi = 0;
                    //Obtenemos el movimiento
                    hi = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["HI"])).FirstOrDefault();
                    //Validamos Hi encontrada
                    if (hi > 0)
                    {
                        //Establecemos HI correspondiente
                        res = new RetornoOperacion(hi, "",true);
                    }
                }
            }
            //Devolvemos Valor
            return res;
        }

        /// <summary>
        /// Obtenemos la Configuración en base a las Asignaciones del Servicio para el calculo de Diesel en Ruta
        /// </summary>
        /// <param name="id_segmento">Id Segmento</param>
        /// <returns></returns>
        public static string ObtieneConfiguracionSegmento(int id_segmento)
        {
            //Definiendo objeto de retorno
            string  configuracion = "";

            //Inicializando arreglo de parámetros
            object[] param = { 18, id_segmento, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo Clave de la Unidad Motriz
                    string claveUnidadMotriz = (from DataRow r in ds.Tables["Table"].Rows
                                                where r.Field<string>("Motriz") == "Si"
                                                select r.Field<string>("TipoUnidad")).DefaultIfEmpty().FirstOrDefault();
                    //Asignamos Valor
                    configuracion = claveUnidadMotriz;

                    
                    //Recorremos Tabla
                    foreach(DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Validamos que no Sea Motriz
                        if(r.Field<string>("Motriz") == "No")
                        {
                            //Asignamos Dimensines de la Unidad
                            configuracion += "-" + Cadena.RegresaCadenaSeparada(r.Field<string>("Dimensiones"),"pies",0);
                        }
                    }
                }

                //Devolviendo resultado
                return configuracion;
            }
        }

        #endregion
    }
   
}
