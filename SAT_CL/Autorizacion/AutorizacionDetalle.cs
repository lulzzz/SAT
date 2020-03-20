using System;
using System.Linq;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proprrciona los metodos para Administrar los Detalles de las Autorizaciones.
    /// </summary>
    public class AutorizacionDetalle:Disposable
    {
      
        #region Propiedades
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "autorizacion.sp_autorizacion_detalle_tad";


        private int _id_autorizacion_detalle;
        /// <summary>
        /// Id Autorizacion Detalle
        /// </summary>
        public  int id_autorizacion_detalle
            {
               get{return _id_autorizacion_detalle;}
            }
        private int _id_autorizacion;
        /// <summary>
        /// Id Autorizacion
        /// </summary>
        public int id_autorizacion
            {
            get {return _id_autorizacion;}

            }
        private string _valor_inicial;
        /// <summary>
        /// Valor inicial de una Autorizacion
        /// </summary>
         public string valor_inicial
         {
             get {return _valor_inicial;}
         }
         private string _valor_final;
        /// <summary>
        /// Valor Final de un Detalle Autorizacion
        /// </summary>
         public string valor_final
         {
             get {return _valor_final;}
         }
         private bool _habilitar;
        /// <summary>
        /// Estado de una Autorizacion Detalle
        /// </summary>
         public bool habilitar
         {
             get { return _habilitar; }
         }
        
        #endregion
        
        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AutorizacionDetalle()
        {
            Dispose(false);
        }
        #endregion
       
        #region Constructor
        /// <summary>
        /// Genera una instancia de tipo Autorizacion Detalle
        /// </summary>
        public AutorizacionDetalle()
        {
            _id_autorizacion_detalle = 0;
            _id_autorizacion = 0;
            _valor_inicial = "";
            _valor_final = "";
            _habilitar = false;
        }
         /// <summary>
         /// Genera una nueva instancia de tipo Autorización Detalle dado un id 
         /// </summary>
         /// <param name="id_autorizacion_detalle"></param>
        public AutorizacionDetalle(int id_autorizacion_detalle)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_autorizacion_detalle, 0, "", "", 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_autorizacion_detalle= Convert.ToInt32(r["Id"]);
                        _id_autorizacion = Convert.ToInt32(r["IdAutorizacion"]);
                        _valor_inicial = r["ValorInicial"].ToString();
                        _valor_final = r["ValorFinal"].ToString();
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }


        #endregion
       
        #region Método Privados
        /// <summary>
        /// Metodo encargado de Editar un Detalle Autoriazacion
        /// </summary>
        /// <param name="id_autorizacion"></param>
        /// <param name="valor_inicial"></param>
        /// <param name="valor_final"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAutorizacionDetalle(int id_autorizacion, string valor_inicial, string valor_final, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_autorizacion_detalle, id_autorizacion, valor_inicial, valor_final, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        #endregion
       
        #region Metodos Publicos
        /// <summary>
        /// Metodo encargado de insertar una Autorizacion Detalle
        /// </summary>
        /// <param name="id_autorizacion"></param>
        /// <param name="valor_inicial"></param>
        /// <param name="valor_final"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAutorizacionDetalle(int id_autorizacion, string valor_inicial, string valor_final, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0,id_autorizacion, valor_inicial, valor_final, id_usuario, true, "", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Metodo encargado de editar una Autorizacion Detalle
        /// </summary>
        /// <param name="id_autorizacion"></param>
        /// <param name="valor_inicial"></param>
        /// <param name="valor_final"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAutorizacionDetalle(int id_autorizacion, string valor_inicial, string valor_final, int id_usuario)
        {
            return this.editaAutorizacionDetalle(id_autorizacion, valor_inicial, valor_final,  id_usuario, this._habilitar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacionDetalle(int id_usuario)
        {
            return this.editaAutorizacionDetalle(this._id_autorizacion, this._valor_inicial, this._valor_final, id_usuario, false);

        }

        /// <summary>
        /// Realiza la inserción de los registros autorización realizada requeridos en el detalle de autorización de un registro y tabla específicos.
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_usuario">Id de usuario que solicita</param>
        /// <returns></returns>
        public RetornoOperacion InsertaAutorizacionesRequeridas(int id_tabla, int id_registro, int id_usuario)
        {
            //Definiendo objeto de retorno, sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_autorizacion_detalle);
         

            //Realizando la carga de los bloques del detalle de autorización
            DataTable bloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle);

            //Si existen registros
            if (Validacion.ValidaOrigenDatos(bloques))
            {
                //Para cada uno de los bloques
                foreach (DataRow b in bloques.Rows)
                {
                    //Cargando los responsables del bloque
                    DataTable responsables = AutorizacionDetalleBloqueResponsable.CargaResponsablesAutorizacionDetalleBloque(b.Field<int>("IdAutorizacionDetalleBloque"));

                    //Si existen responsables
                    if (Validacion.ValidaOrigenDatos(responsables))
                    {
                        //Para cada uno de los responsables
                        foreach (DataRow r in responsables.Rows)
                        {
                            //Insertando petición de autorización
                            resultado = AutorizacionRealizada.InsertaAutorizacionRealizada(id_tabla, id_registro, r.Field<int>("IdAutorizacionDetalleBloqueResponsable"), id_usuario);

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando autorización registrada
                                using (AutorizacionRealizada ar = new AutorizacionRealizada(resultado.IdRegistro))
                                {
                                    //Realziando el envío según corresponda
                                    //resultado = ar.EnviaNotificacionAutorizacion();    
                                }
                            }

                            //En caso de errores
                            if (!resultado.OperacionExitosa)
                                //Saliendo de ciclo de inserción
                                break;
                        }
                    }

                    //En caso de error
                    if (!resultado.OperacionExitosa)
                        //Saliendo de ciclo
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la deshabilitación de los registros autorización realizada requeridos en el detalle de autorización de un registro y tabla específicos.
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_usuario">Id de usuario que solicita</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacionesSinConfirmar(int id_tabla, int id_registro, int id_usuario)
        {
            //Definiendo objeto de retorno, sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_autorizacion_detalle);


            //Realizando la carga de los bloques del detalle de autorización
            DataTable bloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle);

            //Si existen registros
            if (Validacion.ValidaOrigenDatos(bloques))
            {
                //Para cada uno de los bloques
                foreach (DataRow b in bloques.Rows)
                {
                    //Cargando los responsables del bloque
                    DataTable responsables = AutorizacionDetalleBloqueResponsable.CargaAutorizacionResponsablesBloque(b.Field<int>("IdAutorizacionDetalleBloque"), id_tabla, id_registro);

                    //Si existen responsables
                    if (Validacion.ValidaOrigenDatos(responsables))
                    {
                        //Para cada uno de los responsables
                        foreach (DataRow r in responsables.Rows)
                        {
                            //Instanciando la autorización realizada
                            using (AutorizacionRealizada ar = new AutorizacionRealizada(r.Field<int>("IdAutorizacionRealizada")))
                            {
                                //Validando que exista
                                if (ar.id_autorizacion_realizada > 0)
                                {
                                    if (ar.bit_confirmacion == null)
                                    {
                                        //Deshabilitando petición de autorización
                                        resultado = ar.DeshabilitaAutorizacionRealizada(id_usuario);
                                    }

                                    //En caso de errores
                                    if (!resultado.OperacionExitosa)
                                        //Saliendo de ciclo de inserción
                                        break;
                                }
                            }
                        }
                    }

                    //En caso de error
                    if (!resultado.OperacionExitosa)
                        //Saliendo de ciclo
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
   
        /// <summary>
        /// Verifica si las autorizaciones requeridas del detalle de autorización están completas para un registro de una tabla específica.
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public RetornoOperacion ValidaAutorizacionCompleta(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno sin error
            RetornoOperacion resultado = new RetornoOperacion(id_registro);

            //Obteniendo los bloques de nivel inicial del detalle de autorización
            DataTable bloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle, 0);

            //Si existen bloques
            if (Validacion.ValidaOrigenDatos(bloques))
            { 
                //Recuperando el bloque raíz
                AutorizacionDetalleBloque bRaiz = new AutorizacionDetalleBloque((from DataRow r in bloques.Rows
                                                                                     select r.Field<int>("IdAutorizacionDetalleBloque")).FirstOrDefault());

                //Si la raíz fue encontrada
                if (bRaiz.id_autorizacion_detalle_bloque > 0)
                    //Realizando validación
                    resultado = bRaiz.ValidaAutorizacionResponsables(id_tabla, id_registro);
                else
                    //Indicando error
                    resultado = new RetornoOperacion("No pudo ser encontrado el bloque inicial de autorizaciones.");
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Verifica si las autorizaciones requeridas del detalle de autorización están negadas para un registro de una tabla específica.
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public RetornoOperacion ValidaAutorizacionNegada(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno sin error
            RetornoOperacion resultado = new RetornoOperacion(id_registro);

            //Obteniendo los bloques de nivel inicial del detalle de autorización
            DataTable bloques = AutorizacionDetalleBloque.CargaBloquesAutorizacionDetalle(this._id_autorizacion_detalle, 0);

            //Si existen bloques
            if (Validacion.ValidaOrigenDatos(bloques))
            {
                //Recuperando el bloque raíz
                AutorizacionDetalleBloque bRaiz = new AutorizacionDetalleBloque((from DataRow r in bloques.Rows
                                                                                 select r.Field<int>("IdAutorizacionDetalleBloque")).FirstOrDefault());

                //Si la raíz fue encontrada
                if (bRaiz.id_autorizacion_detalle_bloque > 0)
                    //Realizando validación
                    resultado = bRaiz.ValidaAutorizacionNegadasResponsables(id_tabla, id_registro);
                else
                    //Indicando error
                    resultado = new RetornoOperacion("No pudo ser encontrado el bloque inicial de autorizaciones.");
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
