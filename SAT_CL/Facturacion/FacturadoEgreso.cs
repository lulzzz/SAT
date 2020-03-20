using TSDK.Datos;
using TSDK.Base;
using System;
using System.Data;
using System.Transactions;

namespace SAT_CL.Facturacion
{
    public class FacturadoEgreso : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacena el Nombre del Store Procedure
        /// </summary>
        private static string _nom_sp = "facturacion.sp_facturado_egreso_tfe";

        /// <summary>
        /// Atributo encargado de almacenar el Identificador Principal del Registro
        /// </summary>
        public int id_facturado_egreso { get { return this._id_facturado_egreso; } }
        private int _id_facturado_egreso;
        /// <summary>
        /// Atributo encargado de almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo encargado de almacenar el Identificador de Facturado
        /// </summary>
        public int id_facturado { get { return this._id_facturado; } }
        private int _id_facturado;
        /// <summary>
        /// Atributo encargado de almacenar el Egreso de la Nota de Credito
        /// </summary>
        public int id_egreso_ingreso { get { return this._id_egreso_ingreso; } }
        private int _id_egreso_ingreso;
        /// <summary>
        /// Atributo encargado de almacenar el CFDI de Egreso
        /// </summary>
        public int id_cfdi_egreso { get { return this._id_cfdi_egreso; } }
        private int _id_cfdi_egreso;
        /// <summary>
        /// Atributo encargado de almacenar la Versión del CFDI (3.3 Actual)
        /// </summary>
        public string version_cfdi { get { return this._version_cfdi; } }
        private string _version_cfdi;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar el Estatus (Enumeración)
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public byte secuencia { get { return this._secuencia; } }
        private byte _secuencia;
        /// <summary>
        /// Atributo encargado de almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private bool _habilitar;

        #endregion

        #region Enumeraciones

        /// <summary>
        /// 
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// 
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

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public FacturadoEgreso()
        {
            //Asignando Atributos
            this._id_facturado_egreso =
            this._id_compania_emisora =
            this._id_facturado =
            this._id_egreso_ingreso = 
            this._id_cfdi_egreso = 0;
            this._version_cfdi = "";
            this._id_estatus =
            this._secuencia = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_facturado_egreso"></param>
        public FacturadoEgreso(int id_facturado_egreso)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_facturado_egreso);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// 
        /// </summary>
        ~FacturadoEgreso()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_facturado_egreso"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_facturado_egreso)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_facturado_egreso, 0, 0, 0, 0, "", 0, 0, 0, false, "", "" };

            //Instanciando Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Datos
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_facturado_egreso = id_facturado_egreso;
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._id_facturado = Convert.ToInt32(dr["IdFacturado"]);
                        this._id_egreso_ingreso = Convert.ToInt32(dr["IdEgresoIngreso"]);
                        this._id_cfdi_egreso = Convert.ToInt32(dr["IdCfdiEgreso"]);
                        this._version_cfdi = dr["VersionCfdi"].ToString();
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._secuencia = Convert.ToByte(dr["Secuencia"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado Positivo
                    retorno = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_facturado">Facturado</param>
        /// <param name="id_egreso_ingreso">Nota de Credito (Ingreso)</param>
        /// <param name="id_cfdi_egreso">CFDI de la Nota de Credito</param>
        /// <param name="version_cfdi">Versión del Comprobante</param>
        /// <param name="estatus">Estatus de Facturación</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_compania_emisora, int id_facturado, int id_egreso_ingreso, 
                                                int id_cfdi_egreso, string version_cfdi, Estatus estatus, byte secuencia, 
                                                int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_facturado_egreso, id_compania_emisora, id_facturado, id_egreso_ingreso,
                        id_cfdi_egreso, version_cfdi, (byte)estatus, secuencia, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar el Estatus del Registro
        /// </summary>
        /// <param name="estatus"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaEstatus(Estatus estatus, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(this._id_compania_emisora, this._id_facturado, this._id_egreso_ingreso, this._id_cfdi_egreso, 
                                       this._version_cfdi, estatus, this._secuencia, id_usuario, this._habilitar);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar la Relación de las Notas de Credito
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_facturado">Facturado</param>
        /// <param name="id_egreso_ingreso">Nota de Credito (Ingreso)</param>
        /// <param name="id_cfdi_egreso">CFDI de la Nota de Credito</param>
        /// <param name="version_cfdi">Versión del Comprobante</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturadoEgreso(int id_compania_emisora, int id_facturado, int id_egreso_ingreso,
                                                int id_cfdi_egreso, string version_cfdi, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Determinando Estatus
            Estatus estatus = id_cfdi_egreso > 0 ? Estatus.Vigente : Estatus.Registrado;

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_emisora, id_facturado, id_egreso_ingreso, id_cfdi_egreso,
                               version_cfdi, (byte)estatus, 0, id_usuario, true, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar la Relación de las Notas de Credito
        /// </summary>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_facturado">Facturado</param>
        /// <param name="id_egreso_ingreso">Nota de Credito (Ingreso)</param>
        /// <param name="id_cfdi_egreso">CFDI de la Nota de Credito</param>
        /// <param name="version_cfdi">Versión del Comprobante</param>
        /// <param name="estatus">Estatus de la Relación</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturadoEgreso(int id_compania_emisora, int id_facturado, int id_egreso_ingreso,
                                                int id_cfdi_egreso, string version_cfdi, Estatus estatus, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Registrado:
                case Estatus.Vigente:
                    {
                        //Ejecutando SP
                        retorno = actualizaRegistroBD(id_compania_emisora, id_facturado, id_egreso_ingreso, id_cfdi_egreso,
                                                      version_cfdi, estatus, this._secuencia, id_usuario, this._habilitar);
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
        /// Método encargado de 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturadoEgreso(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Estatus
            switch ((Estatus)this._id_estatus)
            {
                case Estatus.Registrado:
                    {
                        //Ejecutando SP
                        retorno = actualizaRegistroBD(this._id_compania_emisora, this._id_facturado, this._id_egreso_ingreso, this._id_cfdi_egreso,
                                                      this._version_cfdi, (Estatus)this._id_estatus, this._secuencia, id_usuario, false);
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
        /// Método encargado de Actualizar los Atributos del Facturado Egreso
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturadoEgreso()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_facturado_egreso);
        }
        /// <summary>
        /// Método encargado de Actualizar el CFDI Timbrado de la Nota de Credito
        /// </summary>
        /// <param name="id_cfdi_egreso">CFDI de Egreso (Timbrada)</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaNotaCreditoTimbrada(int id_cfdi_egreso, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Actualizando Campo
            retorno = this.EditaFacturadoEgreso(this._id_compania_emisora, this._id_facturado, this._id_egreso_ingreso, id_cfdi_egreso,
                                    this._version_cfdi, id_cfdi_egreso > 0 ? Estatus.Vigente : Estatus.Registrado, id_usuario);

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public static RetornoOperacion InsertaNuevaNotaCreditoTimbrada(int id_facturado_egreso, int id_cfdi_nc_nva, string version, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            int idFacEgreso = 0;

            //Instanciando Registro a Reemplazar
            using (FacturadoEgreso nc_anterior = new FacturadoEgreso(id_facturado_egreso))
            {
                //Validando Existencia
                if (nc_anterior.habilitar)
                {
                    //Obteniendo Detalles
                    using (DataTable dtDetalles = FacturadoEgresoRelacion.ObtieneRelacionesNotaCredito(nc_anterior.id_facturado_egreso))
                    {
                        //Validando detalles
                        if (Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Actualizando a Estatus Sustitución
                                retorno = nc_anterior.actualizaEstatus(Estatus.Sustituido, id_usuario);

                                //Validando Operación
                                if (retorno.OperacionExitosa)
                                {
                                    //Insertando Duplica de Registro
                                    retorno = FacturadoEgreso.InsertaFacturadoEgreso(nc_anterior.id_compania_emisora, nc_anterior.id_facturado, nc_anterior.id_egreso_ingreso,
                                                                                 id_cfdi_nc_nva, version, id_usuario);

                                    //Validando Operación
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Asignando Facturado Egreso
                                        idFacEgreso = retorno.IdRegistro;

                                        //Recorriendo Detalles
                                        foreach (DataRow dr in dtDetalles.Rows)
                                        {
                                            //Instanciando Detalle
                                            using (FacturadoEgresoRelacion detalle = new FacturadoEgresoRelacion(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Validando detalle
                                                if (detalle.habilitar)
                                                {

                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("No se puede recuperar el Detalle");
                                            }

                                            //Validando Resultado
                                            if (!retorno.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }

                                    }
                                }
                            }
                        }
                        else
                            //instanciando Excepción
                            retorno = new RetornoOperacion("No se pudo recuperar los Detalles que ampara la Nota de Credito");
                    }
                }
                else
                    //instanciando Excepción
                    retorno = new RetornoOperacion("No se pudo recuperar la Nota de Credito");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion
    }
}
