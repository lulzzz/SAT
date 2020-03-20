using TSDK.Datos;
using TSDK.Base;
using System;
using System.Data;
using System.Transactions;
using System.Linq;

namespace SAT_CL.Facturacion
{
    public class FacturadoEgresoRelacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// 
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Enumeración que expresa que la Nota de Credito este Registrada
            /// </summary>
            Registrado = 0,
            /// <summary>
            /// Enumeración que expresa que la Nota de Credito este Vigente
            /// </summary>
            Vigente = 1,
            /// <summary>
            /// Enumeración que expresa que la Nota de Credito este Cancelada
            /// </summary>
            Cancelado,
            /// <summary>
            /// Enumeración que expresa que la Nota de Credito este Sustituida
            /// </summary>
            Sustituido
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_facturado_egreso_relacion_tfer";
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_facturado_egreso_relacion { get { return this._id_facturado_egreso_relacion; } }
        private int _id_facturado_egreso_relacion;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_facturado_egreso { get { return this._id_facturado_egreso; } }
        private int _id_facturado_egreso;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_cfdi_ingreso { get { return this._id_cfdi_ingreso; } }
        private int _id_cfdi_ingreso;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public int id_aplicacion { get { return this._id_aplicacion; } }
        private int _id_aplicacion;
        /// <summary>
        /// Atributo encargado de almacenar
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private bool _habilitar;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Cargar los Atributos por Defecto
        /// </summary>
        public FacturadoEgresoRelacion()
        {
            //Asignando Valores
            this._id_facturado_egreso_relacion =
            this._id_facturado_egreso =
            this._id_cfdi_ingreso =
            this._id_aplicacion = 0;
            this._id_estatus = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_facturado_egreso_relacion"></param>
        public FacturadoEgresoRelacion(int id_facturado_egreso_relacion)
        {
            //Cargando Atributos
            cargaAtributosInstancia(id_facturado_egreso_relacion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturadoEgresoRelacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Encargado de Cargar los Atributos de la Clase
        /// </summary>
        /// <param name="id_facturado_egreso_relacion"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_facturado_egreso_relacion)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando arreglo de Parametros
            object[] param = { 3, id_facturado_egreso_relacion, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Datos
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_facturado_egreso_relacion = id_facturado_egreso_relacion;
                        this._id_facturado_egreso = Convert.ToInt32(dr["IdFacturadoEgreso"]);
                        this._id_cfdi_ingreso = Convert.ToInt32(dr["IdCfdiEgreso"]);
                        this._id_aplicacion = Convert.ToInt32(dr["IdAplicacion"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado Positivo
                    retorno = true;
                }
            }

            //Devolviendo resultado Obtenido
            return retorno;
        }

        private RetornoOperacion actualizaAtributosBD(int id_facturado_egreso, int id_cfdi_ingreso, int id_aplicacion, Estatus estatus, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Armando arreglo de Parametros
            object[] param = { 2, this._id_facturado_egreso_relacion, id_facturado_egreso, id_cfdi_ingreso, id_aplicacion, (Estatus)estatus, id_usuario, habilitar, "", "" };

            //Actualizando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaEstatus(Estatus estatus, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaAtributosBD(this._id_facturado_egreso, this._id_cfdi_ingreso, this._id_aplicacion, estatus, id_usuario, this._habilitar);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Relaciones que ampara la Nota de Credito
        /// </summary>
        /// <param name="id_facturado_egreso"></param>
        /// <param name="id_cfdi_ingreso"></param>
        /// <param name="id_aplicacion"></param>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturadoEgresoRelacion(int id_facturado_egreso, int id_cfdi_ingreso, int id_aplicacion, int id_usuario)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Asignando Estatus
            Estatus estatus = id_aplicacion > 0 ? Estatus.Vigente : Estatus.Registrado;

            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_facturado_egreso, id_cfdi_ingreso, id_aplicacion, estatus, id_usuario, true, "", "" };

            //Actualizando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_facturado_egreso"></param>
        /// <param name="id_cfdi_ingreso"></param>
        /// <param name="id_aplicacion"></param>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturadoEgresoRelacion(int id_facturado_egreso, int id_cfdi_ingreso, int id_aplicacion, Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Vigente:
                    {
                        //Ejecutando SP
                        retorno = this.actualizaAtributosBD(id_facturado_egreso, id_cfdi_ingreso, id_aplicacion, estatus, id_usuario, this._habilitar);
                        break;
                    }
                case Estatus.Cancelado:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI fue cancelado previamente");
                        break;
                    }
                case Estatus.Sustituido:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI fue Sustituido previamente");
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Relación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturadoEgresoRelacion(int id_usuario)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Registrado:
                    {
                        //Ejecutando SP
                        retorno = this.actualizaAtributosBD(this._id_facturado_egreso, this._id_cfdi_ingreso, this._id_aplicacion, (Estatus)this._id_estatus, id_usuario, false);
                        break;
                    }
                case Estatus.Vigente:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI esta Vigente");
                        break;
                    }
                case Estatus.Cancelado:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI fue cancelado previamente");
                        break;
                    }
                case Estatus.Sustituido:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI fue Sustituido previamente");
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Obtener las relaciones de la Nota de Credito
        /// </summary>
        /// <param name="id_facturado_egreso">Nota de Credito</param>
        /// <returns></returns>
        public static DataTable ObtieneRelacionesNotaCredito(int id_facturado_egreso)
        {
            //Declarando Objeto de Retorno
            DataTable dtRelaciones = null;

            //Armando arreglo de Parametros
            object[] param = { 4, 0, id_facturado_egreso, 0, 0, 0, 0, true, "", "" };

            //Instanciando Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Datos
                    dtRelaciones = ds.Tables["Table"];
            }

            //Devovliendo Resultado Obtenido
            return dtRelaciones;
        }
        /// <summary>
        /// Método encargado de Actualizar la Cancelación en la Nota de Credito e Insertar la Nueva relación a los CFDI's de Ingreso
        /// </summary>
        /// <param name="id_facturado_egreso">Nueva Nota de Credito</param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCancelacionRelacion(int id_facturado_egreso, int id_usuario)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Vigente:
                    {
                        //Actualizando a Cancelado el Registro Anterior
                        retorno = this.actualizaEstatus(Estatus.Cancelado, id_usuario);
                        break;
                    }
                case Estatus.Cancelado:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI de la Nota de Credito fue cancelado previamente");
                        break;
                    }
                case Estatus.Sustituido:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI de la Nota de Credito fue Sustituido previamente");
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar la Cancelación en la Nota de Credito e Insertar la Nueva relación a los CFDI's de Ingreso
        /// </summary>
        /// <param name="id_facturado_egreso"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSustitucionRelacion(int id_facturado_egreso, int id_usuario)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Vigente:
                case Estatus.Cancelado:
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Actualizando a Cancelado el Registro Anterior
                            retorno = this.actualizaEstatus(Estatus.Sustituido, id_usuario);

                            //Validando Operación
                            if (retorno.OperacionExitosa)
                            {
                                //Insertando Registro Nuevo
                                retorno = InsertaFacturadoEgresoRelacion(id_facturado_egreso, this._id_cfdi_ingreso, this._id_aplicacion, id_usuario);

                                //Validando Operación
                                if (retorno.OperacionExitosa)

                                    //Completando Operación
                                    scope.Complete();
                            }
                        }
                        break;
                    }
                case Estatus.Sustituido:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("El CFDI de la Nota de Credito fue Sustituido previamente");
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Validar si el CFDI de Ingreso se encuentra amparado por una Nota de Credito
        /// </summary>
        /// <param name="id_comprobante_ingreso">Comprobante de Ingreso</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaNotaCreditoCFDI(int id_comprobante_ingreso)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion("No existe una Nota de Credito que ampare este Comprobante");

            //Armando arreglo de Parametros
            object[] param = { 5, 0, 0, id_comprobante_ingreso, 0, 0, 0, true, "", "" };

            //Instanciando Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Validando Existencia
                    if ((from DataRow dr in ds.Tables["Table"].Rows
                         select Convert.ToInt32(dr["Id"])).FirstOrDefault() > 0)

                        //Instanciando Excepción
                        retorno = new RetornoOperacion(1, "El Comprobante esta amparado por una Nota de Credito", true);
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion
    }
}
