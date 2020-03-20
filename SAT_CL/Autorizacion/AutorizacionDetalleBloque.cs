using System;
using System.Linq;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proporciona los metodos para Administrar los Detalles de las Autorizaciones.
    /// </summary>
    public class AutorizacionDetalleBloque : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera el Tipo de Operador
        /// </summary>
        public enum TipoOperador
        {
            /// <summary>
            /// AND
            /// </summary>
            AND = 1,
            /// <summary>
            /// OR
            /// </summary>
            OR = 2
        }
        #endregion

        #region propiedades
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "autorizacion.sp_autorizacion_detalle_bloque";


        private int _id_autorizacion_detalle_bloque;
        /// <summary>
        /// Id Autorizacion Detalle Bloque
        /// </summary>
        /// 
        public int id_autorizacion_detalle_bloque
        {
            get { return _id_autorizacion_detalle_bloque; }
        }

        private int _id_autorizacion_detalle;
        /// <summary>
        /// Id Autorizacion Detalle
        /// </summary>
        ///
        public int id_autorizacion_detalle
        {
            get { return _id_autorizacion_detalle; }

        }

        private int _bloque;
        /// <summary>
        /// Valor del bloque
        /// </summary>
        public int bloque
        {
            get { return _bloque; }
        }

        private int _bloque_superior;
        /// <summary>
        /// Valor del bloque superior
        /// </summary>
        public int bloque_superior
        {
            get { return _bloque_superior; }
        }

        private short _id_operador_logico;
        /// <summary>
        /// Id del operador
        /// </summary>
        public short id_operador_logico
        {
            get { return _id_operador_logico; }
        }

        private bool _habilitar;
        /// <summary>
        /// Habilitacion de una Autorizacion Detalle Bloque
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        /// <summary>
        /// Enumera el Tipo de Operador
        /// </summary>
        public TipoOperador operador
        {
            get { return (TipoOperador)_id_operador_logico; }
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AutorizacionDetalleBloque()
        {
            Dispose(false);
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Genera una instancia de tipo Autorizacion Detalle Bloque
        /// </summary>
        public AutorizacionDetalleBloque()
        {
            _id_autorizacion_detalle_bloque = 0;
            _id_autorizacion_detalle = 0;
            _bloque = 0;
            _bloque_superior = 0;
            _id_operador_logico = 0;
            _habilitar = false;
        }
        /// <summary>
        /// Genera una nueva instancia de tipo Autorización Detalle Bloque dado un id 
        /// </summary>
        /// <param name="id_autorizacion_detalle_bloque"></param>
        public AutorizacionDetalleBloque(int id_autorizacion_detalle_bloque)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_autorizacion_detalle_bloque, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_autorizacion_detalle_bloque = Convert.ToInt32(r["IdAutorizacionDetalleBloque"]);
                        _id_autorizacion_detalle = Convert.ToInt32(r["IdAutorizacionDetalle"]);
                        _bloque = Convert.ToInt32(r["Bloque"]);
                        _bloque_superior = Convert.ToInt32(r["BloqueSuperior"]);
                        _id_operador_logico = Convert.ToInt16(r["IdOperador"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }

        #endregion

        #region Metodos privado
        /// <summary>
        /// Metodo encargado de Editar un Detalle Autoriazacion Bloque
        /// </summary>
        /// <param name="id_autorizacion_detalle"></param>
        /// <param name="bloque"></param>
        /// <param name="bloque_superior"></param>
        /// <param name="id_operador_logico"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAutorizacionDetalleBloque(int id_autorizacion_detalle, int bloque, int bloque_superior, short id_operador_logico, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_autorizacion_detalle_bloque, id_autorizacion_detalle, bloque, bloque_superior, id_operador_logico, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        #endregion

        #region Metodos Publicos
        /// <summary>
        /// Metodo encargado de insertar una Autorizacion Detalle Bloque
        /// </summary>
        /// <param name="id_autorizacion_detalle"></param>
        /// <param name="bloque"></param>
        /// <param name="bloque_superior"></param>
        /// <param name="id_operador_logico"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAutorizacionDetalleBloque(int id_autorizacion_detalle, int bloque, int bloque_superior, TipoOperador id_operador_logico, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_autorizacion_detalle, bloque, bloque_superior, (short)id_operador_logico, id_usuario, true, "", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }


        /// <summary>
        /// Metodo encargado de editar una Autorizacion Detalle
        /// </summary>
        /// <param name="id_autorizacion_detalle"></param>
        /// <param name="bloque"></param>
        /// <param name="bloque_superior"></param>
        /// <param name="id_operador_logico"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAutorizacionDetalleBloque(int id_autorizacion_detalle, int bloque, int bloque_superior, TipoOperador id_operador_logico, int id_usuario)
        {
            return this.editaAutorizacionDetalleBloque(id_autorizacion_detalle, bloque, bloque_superior, (short)id_operador_logico, id_usuario, this._habilitar);
        }


        /// <summary>
        /// Deshabilita una Autorizacion Detalle Bloque
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacionDetalleBloque(int id_usuario)
        {
            return this.editaAutorizacionDetalleBloque(this._id_autorizacion_detalle, this._bloque, this._bloque_superior, this._id_operador_logico, id_usuario, false);
        }

        /// <summary>
        /// Realiza la carga de los bloques asignados a un detalle de autorización
        /// </summary>
        /// <param name="id_autorizacion_detalle">Id de Detalle de Autorización</param>
        /// <returns></returns>
        public static DataTable CargaBloquesAutorizacionDetalle(int id_autorizacion_detalle)
        { 
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo arreglo de objetos que contendrá al conjunto de criterios de consulta
            object[] param = { 4, 0, id_autorizacion_detalle, 0, 0, 0, 0, false, "", "" };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Realiza la carga de los bloques asignados a un detalle de autorización y bloque padre
        /// </summary>
        /// <param name="id_autorizacion_detalle">Id de Detalle de Autorización</param>
        /// <param name="id_bloque_superior">Id de Bloque superior</param>
        /// <returns></returns>
        public static DataTable CargaBloquesAutorizacionDetalle(int id_autorizacion_detalle, int id_bloque_superior)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Definiendo arreglo de objetos que contendrá al conjunto de criterios de consulta
            object[] param = { 5, 0, id_autorizacion_detalle, 0, id_bloque_superior, 0, 0, false, "", "" };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Realiza la validación de las autorizaciones de un bloque, dado un registro de tabla específica. Devuelve error si existe alguna autorización sin evaluar (acorde al operador AND / OR).
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public RetornoOperacion ValidaAutorizacionResponsables(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(id_registro);

            //Cargando los bloques pertenecientes a este bloque (sub bloques)
            DataTable subBloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle, this._id_autorizacion_detalle_bloque);

            //Validando la existencia de sub bloques
            if (Validacion.ValidaOrigenDatos(subBloques))
            {
                //Para cada uno de estos sub bloques
                foreach (DataRow sb in subBloques.Rows)
                { 
                    //Instanciando bloque
                    using (AutorizacionDetalleBloque sBloque = new AutorizacionDetalleBloque(sb.Field<int>("IdAutorizacionDetalleBloque")))
                    { 
                        //Si el bloque se instanció correctamente
                        if (sBloque.id_autorizacion_detalle_bloque > 0)
                            //Realizando validación del bloque
                            resultado = sBloque.ValidaAutorizacionResponsables(id_tabla, id_registro);
 
                        //Si existe error
                        if (!resultado.OperacionExitosa)
                            //Saliendo del ciclo
                            break;
                    }
                }
            }

            //Si no existen errores
            if (resultado.OperacionExitosa)
            {
                //Obteniendo los resultados de autorización de los responsables del bloque actual
                DataTable autorizaciones = AutorizacionDetalleBloqueResponsable.CargaAutorizacionResponsablesBloque(this._id_autorizacion_detalle_bloque, id_tabla, id_registro);

                //Validando que existan autorizaciones
                if (Validacion.ValidaOrigenDatos(autorizaciones))
                {
                    //Determinando el uso de resultados en base al operador lógico que rige al bloque
                    switch (this.operador)
                    {
                        case TipoOperador.AND:
                            //Si alguno de los elementos NO se ha autorizado aún
                            if ((from DataRow a in autorizaciones.Rows
                                 select a.Field<bool?>("Autorizado")).Any(au => au.Equals(null)))
                                //Señalando el error
                                resultado = new RetornoOperacion("Existen autorizaciones pendientes.");
                            break;
                        case TipoOperador.OR:
                            //Si todos los elementos NO se han autorizado aún
                            if ((from DataRow a in autorizaciones.Rows
                                 select a.Field<bool?>("Autorizado")).All(au => au.Equals(null)))
                                //Señalando el error
                                resultado = new RetornoOperacion("Existen autorizaciones pendientes.");
                            break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la validación de las autorizaciones de un bloque, dado un registro de tabla específica. Devuelve error en caso de existir autorizaciones negadas (acorde al operador AND / OR) 
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public RetornoOperacion ValidaAutorizacionNegadasResponsables(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(id_registro);

            //Cargando los bloques pertenecientes a este bloque (sub bloques)
            DataTable subBloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle, this._id_autorizacion_detalle_bloque);

            //Validando la existencia de sub bloques
            if (Validacion.ValidaOrigenDatos(subBloques))
            {
                //Para cada uno de estos sub bloques
                foreach (DataRow sb in subBloques.Rows)
                {
                    //Instanciando bloque
                    using (AutorizacionDetalleBloque sBloque = new AutorizacionDetalleBloque(sb.Field<int>("IdAutorizacionDetalleBloque")))
                    {
                        //Si el bloque se instanció correctamente
                        if (sBloque.id_autorizacion_detalle_bloque > 0)
                            //Realizando validación del bloque
                            resultado = sBloque.ValidaAutorizacionNegadasResponsables(id_tabla, id_registro);

                        //Si existe error
                        if (!resultado.OperacionExitosa)
                            //Saliendo del ciclo
                            break;
                    }
                }
            }

            //Si no existen errores
            if (resultado.OperacionExitosa)
            {
                //Obteniendo los resultados de autorización de los responsables del bloque actual
                DataTable autorizaciones = AutorizacionDetalleBloqueResponsable.CargaAutorizacionResponsablesBloque(this._id_autorizacion_detalle_bloque, id_tabla, id_registro);

                //Validando que existan autorizaciones
                if (Validacion.ValidaOrigenDatos(autorizaciones))
                {
                    //Determinando el uso de resultados en base al operador lógico que rige al bloque
                    switch (this.operador)
                    {
                        case TipoOperador.AND:
                            //Si alguno de los elementos NO se ha autorizado aún
                            if ((from DataRow a in autorizaciones.Rows
                                 select a.Field<bool?>("Autorizado")).Any(au => au.Equals(false)))
                                //Señalando el error
                                resultado = new RetornoOperacion("Existen autorizaciones rechazadas.");
                            break;
                        case TipoOperador.OR:
                            //Si todos los elementos NO se han autorizado aún
                            if ((from DataRow a in autorizaciones.Rows
                                 select a.Field<bool?>("Autorizado")).All(au => au.Equals(false)))
                                //Señalando el error
                                resultado = new RetornoOperacion("Existen autorizaciones rechazadas.");
                            break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}

