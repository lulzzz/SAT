using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Nomina
{
    /// <summary>
    /// Clase encargada de Todas las Operaciones de 
    /// </summary>
    public class NominaOtros:Disposable
    {
        # region Enumeración
        /// <summary>
        /// Enumeración de tipo concepto de nomina otros
        /// </summary>
        public enum TipoNominaOtros
        {
            /// <summary>
            /// Define si el tipo de concepto de nomina otros es una incapacidad
            /// </summary>
            Incapacidad = 1,
            /// <summary>
            /// Define si el tipo de concepto de nomina otros es por horas extra
            /// </summary>
            HrsExtra = 2
        };
        /// <summary>
        /// Enumeración de sub tipo de concepto a corde al tipo de concepto de nomina
        /// </summary>
        public enum SubTipo
        {   
            /// <summary>
            /// Incapacidad por riesgo de trabajo
            /// </summary>
            RiesgoTrabajo = 1,
            /// <summary>
            /// Incapacidad por enfermedad en General
            /// </summary>
            EnfermedadGeneral = 2,
            /// <summary>
            /// Incapacidad por Maternidad
            /// </summary>
            Maternidad = 3,
            /// <summary>
            /// Horas extras dobles
            /// </summary>
            Dobles = 4,
            /// <summary>
            /// Horas extras triples
            /// </summary>
            Triples = 5
        };
        #endregion
        # region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla nomina otros
        /// </summary>
        private static string nom_sp = "nomina.sp_nomina_otros_tno";
        private int _id_nomina_otros;
        /// <summary>
        /// Atributo que almacena el identificador de un registro de nomina otros
        /// </summary>
        public int id_nomina_otros
        {
            get { return _id_nomina_otros; }
        }
        private int _id_nomina_empleado;
        /// <summary>
        /// Atributo que almacena el identificador nomina empleado.
        /// </summary>
        public int id_nomina_empleado
        {
            get { return _id_nomina_empleado; }
        }

        private byte _id_tipo;
        /// <summary>
        /// Atributo que almacena el Tipo de Concepto de la Nomina (Incapacidad, Hrs. Extra).
        /// </summary>
        public byte id_tipo
        {
            get { return _id_tipo; }
        }
        /// <summary>
        /// Permite acceder a los valores de la enumeracion TipoNominaOtros
        /// </summary>
        public TipoNominaOtros tipo_nomina
        {
            get { return (TipoNominaOtros)this._id_tipo; }
        }
        private int _dias;
        /// <summary>
        /// Atributo que almacena el numero de días del concepto de nomina
        /// </summary>
        public int dias
        {
            get { return _dias; }
        }
        /// <summary>
        /// Permite acceder a los valores de la enumeracion SubTipo (Acorde al tipo de concepto de nomina Incapacidad,hrs. extra:)
        /// </summary>
        public SubTipo sub_tipo
        {
            get { return (SubTipo)this._id_subtipo; }
        }
        private byte _id_subtipo;
        /// <summary>
        /// Atributo que almacena el subtipo de nomina y va ligado acorde al tipo horas extra(dobles,triples,etc.)
        /// </summary>
        public byte id_subtipo
        {
            get { return _id_subtipo; }
        }
        private decimal _importe_gravado;
        /// <summary>
        /// Atributo que almacena la cantidad monetaria por el concepto de nomina (Gravado)
        /// </summary>
        public decimal importe_gravado
        {
            get { return _importe_gravado; }
        }
        private decimal _importe_exento;
        /// <summary>
        /// Atributo que almacena la cantidad monetaria por el concepto de nomina (Exento)
        /// </summary>
        public decimal importe_exento
        {
            get { return _importe_exento; }
        }
        private decimal _cantidad;
        /// <summary>
        /// Atributo que almacena el numero de horas especificas en la nomina empleado.
        /// </summary>
        public decimal cantidad
        {
            get { return _cantidad; }
        }
        private bool _habilitar;
        /// <summary>
        /// Atributo que alamcena el estado de habilitación de un registro (habilitado/deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor default que inicializa los atributos privados en 0;
        /// </summary>
        public NominaOtros()
        {
            this._id_nomina_otros = 0;
            this._id_nomina_empleado = 0;
            this._id_tipo = 0;
            this._dias = 0;
            this._id_subtipo = 0;
            this._importe_gravado = 0.0m;
            this._importe_exento = 0.0m;
            this._cantidad = 0.0m;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de la busqueda de un registro.
        /// </summary>
        /// <param name="id_nomina_otros">Identificador que permite buscar un registro</param>
        public NominaOtros(int id_nomina_otros)
        {
            //Invoca al método cargaAtributos();
            cargaAtributos(id_nomina_otros);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase NominaOtros
        /// </summary>
        ~NominaOtros()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros e inicializa los atributos con el resultado de la busqueda.
        /// </summary>
        /// <param name="id_nomina_otros">Identificador del registro que se consultara.</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_nomina_otros)
        {
            //Creación de objeto retorno
            bool retorno = false;
            //Creación del arreglo necesario para alamcenar los parametros necesario para la consulta de registros a la base d e datos.
            object[] param = { 3, id_nomina_otros, 0, 0, 0, 0, 0.0m, 0.0m, 0.0m, 0, false, "", "" };
            //Creación de la variable tipo tabla DS que alamcena el resultado de la invocación del método EjecutaProcAlmacenadoDataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida si existen datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas y almacena los datos en la variable r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_nomina_otros = id_nomina_otros;
                        this._id_nomina_empleado = Convert.ToInt32(r["IdNominaEmpleado"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._dias = Convert.ToInt32(r["Dias"]);
                        this._id_subtipo = Convert.ToByte(r["IdSubTipo"]);
                        this._importe_gravado = Convert.ToDecimal(r["ImporteGravado"]);
                        this._importe_exento = Convert.ToDecimal(r["ImporteExento"]);
                        this._cantidad = Convert.ToDecimal(r["Cantidad"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de los datos.
                    retorno = true;
                }
            }
            //Retornal el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los registros de nomina otros
        /// </summary>
        /// <param name="id_nomina_empleado">Permite actualizar el identificador de nomina empleado</param>
        /// <param name="id_tipo">Permite actualizar el identificador de tipo de concepto de nomina (hrs. extra, incapacidad)</param>
        /// <param name="dias">Permite actualizar los dias del tipo de concepto de nomina </param>
        /// <param name="id_subtipo">Permite actualizar el subtipo de concepto de nomina</param>
        /// <param name="importe_gravado">Pemite actualizar la cantidad monetaria por el tipo de concepto de nomina (Gravado)</param>
        /// <param name="importe_exento">Pemite actualizar la cantidad monetaria por el tipo de concepto de nomina (Exento)</param>
        /// <param name="cantidad">Permite actualizar la cantidad </param>
        /// <param name="id_usuario">Permite actualizar al usuario que realiza acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar el estado de un registro (Habilitado/Deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editarNominaOtros(int id_nomina_empleado, TipoNominaOtros tipo_nomina, int dias, SubTipo sub_tipo, decimal importe_gravado, decimal importe_exento, decimal cantidad, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 2, this._id_nomina_otros, id_nomina_empleado, (byte)tipo_nomina, dias, (byte)sub_tipo, importe_gravado, importe_exento, cantidad, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que permite realizar la inserción de registros de Nomina Otros
        /// </summary>
        /// <param name="id_nomina_empleado">Pemite insertar un identificador de nomina empleado</param>
        /// <param name="id_tipo">Permite inserta el identificador de tipo de concepto de nomina (hrs. extra, incapacidad)</param>
        /// <param name="dias">Permite insertar los dias del tipo de concepto de nomina </param>
        /// <param name="id_subtipo">Permite insertar el subtipo de concepto de nomina</param>
        /// <param name="importe_gravado">Pemite insertar la cantidad monetaria por el tipo de concepto de nomina (Gravado)</param>
        /// <param name="importe_exento">Pemite insertar la cantidad monetaria por el tipo de concepto de nomina (Exento)</param>
        /// <param name="cantidad">Permite insertar la cantidad </param>
        /// <param name="id_usuario">Permite insertar al usuario que realizo la inserción del registro</param
        /// <returns></returns>
        public static RetornoOperacion InsertarNominaOtros(int id_nomina_empleado, TipoNominaOtros tipo_nomina, int dias, SubTipo sub_tipo, decimal importe_gravado, decimal importe_exento, decimal cantidad, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idNominaOtros = 0;
            
            //Declarando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Validando que no Exista el Comprobante
                        if (ne.id_comprobante == 0)
                        {
                            //Creación del arreglo que almacena los parametros de actualización del registro
                            object[] param = { 1, 0, id_nomina_empleado,(byte)tipo_nomina, dias, (byte)sub_tipo, importe_gravado, importe_exento, cantidad, id_usuario, true, "", "" };

                            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
                            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

                            //Validando Operación Exitosa?
                            if (retorno.OperacionExitosa)
                            {
                                //Asignando Valor
                                idNominaOtros = retorno.IdRegistro;

                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(ne.id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Detalle en Retorno
                                            retorno = new RetornoOperacion(idNominaOtros);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar registros de Nomina Otros
        /// </summary>
        /// <param name="id_nomina_empleado">Permite actualizar el identificador de nomina empleado</param>
        /// <param name="id_tipo">Permite actualizar el identificador de tipo de concepto de nomina (hrs. extra, incapacidad)</param>
        /// <param name="dias">Permite actualizar los dias del tipo de concepto de nomina </param>
        /// <param name="id_subtipo">Permite actualizar el subtipo de concepto de nomina</param>
        /// <param name="importe_gravado">Pemite insertar la cantidad monetaria por el tipo de concepto de nomina (Gravado)</param>
        /// <param name="importe_exento">Pemite insertar la cantidad monetaria por el tipo de concepto de nomina (Exento)</param>
        /// <param name="cantidad">Permite actualizar la cantidad </param>
        /// <param name="id_usuario">Permite actualizar al usuario que realiza acciones sobre el registro</param>
        /// <param name="habilitar">Permite cambiar el estado de un registro (Habilitado/Deshabilitado)</param>
        /// <returns></returns>
        public RetornoOperacion EditarNominaOtros(int id_nomina_empleado, TipoNominaOtros tipo_nomina, int dias, SubTipo sub_tipo, decimal importe_gravado, decimal importe_exento, decimal cantidad, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idNominaOtros = 0;

            //Declarando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Invoca y retorna al método editarNominaOtros
                        retorno = this.editarNominaOtros(id_nomina_empleado,(TipoNominaOtros)tipo_nomina, dias,(SubTipo)sub_tipo, importe_gravado, importe_exento, cantidad, id_usuario, this._habilitar);

                        //Validando que la Operación fue Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Asignando Nomina Otros
                            idNominaOtros = retorno.IdRegistro;
                            
                            //Validando que no Exista el Comprobante
                            if (ne.id_comprobante == 0)
                            {
                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(this._id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Nomina Otros
                                            retorno = new RetornoOperacion(idNominaOtros);
                                            
                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                        }
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }

            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que permite El cambio de estado de habilitación de un registro(Habilitado/Deshabilitado)
        /// </summary>
        /// <param name="id_usuario">Permite Identificar al ultimo usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarNominaOtros(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idNominaOtros = 0;

            //Declarando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Nómina Empleado
                using (NominaEmpleado ne = new NominaEmpleado(id_nomina_empleado))
                {
                    //Validando que exista la Nomina
                    if (ne.habilitar)
                    {
                        //Invoca y retorna al método editaNominaOtros
                        retorno = this.editarNominaOtros(this.id_nomina_empleado,(TipoNominaOtros)this.id_tipo, this.dias, (SubTipo)this.id_subtipo, this.importe_gravado, this.importe_exento, this.cantidad, id_usuario, false);

                        //Validando que la Operación fue Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Asignando Nomina Otros
                            idNominaOtros = retorno.IdRegistro;

                            //Validando que no Exista el Comprobante
                            if (ne.id_comprobante == 0)
                            {
                                //Obteniendo Totales
                                using (DataTable dtTotalesEmpleado = NominaEmpleado.ObtieneTotalesEmpleado(this._id_nomina_empleado))
                                {
                                    //Validando que existen los Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesEmpleado))
                                    {
                                        //Inicializando Ciclo
                                        foreach (DataRow dr in dtTotalesEmpleado.Rows)
                                        {
                                            //Actualizando Totales de Nomina de Empleado
                                            retorno = ne.ActualizaTotalesNominaEmpleado(Convert.ToDecimal(dr["TotalGravadoPercepcion"]), Convert.ToDecimal(dr["TotalGravadoDeduccion"]),
                                                            Convert.ToDecimal(dr["TotalExentoPercepcion"]), Convert.ToDecimal(dr["TotalExentoDeduccion"]), id_usuario);
                                            break;
                                        }

                                        //Validando que la Operación fue Exitosa
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Instanciando Nomina Otros
                                            retorno = new RetornoOperacion(idNominaOtros);
                                            
                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("La Nómina del Empleado ya ha sido Timbrada, Imposible su Edición");
                        }
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede Acceder a la Nómina del Empleado");
                }
            }

            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método encargado de Obtener la Nomina de Otros del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado"></param>
        /// <param name="id_tipo_nomina_otros"></param>
        /// <returns></returns>
        public static DataTable ObtieneNominaOtros(int id_nomina_empleado, int id_tipo_nomina_otros)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominaOtros = null;

            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 4, 0, id_nomina_empleado, id_tipo_nomina_otros, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominaOtros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtNominaOtros;
        }

        /// <summary>
        /// Método encargado de Obtener la Nomina de Otros del Empleado
        /// </summary>
        /// <param name="id_nomina">Encabezado de Nomina</param>
        /// <param name="id_tipo_nomina_otros">Concepto de Nomina Otros</param>
        /// <returns></returns>
        public static DataTable ObtieneNominaEncabezadoOtros(int id_nomina, int id_tipo_nomina_otros)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominaOtros = null;

            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 5, 0, id_nomina, id_tipo_nomina_otros, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominaOtros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtNominaOtros;
        }
        /// <summary>
        /// Método encargado de Obtener la Nomina de Otros del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina de Empleado</param>
        /// <returns></returns>
        public static DataTable ObtieneNominaOtros(int id_nomina_empleado)
        {
            //Declarando Objeto de Retorno
            DataTable dtNominaOtros = null;

            //Creación del arreglo que almacena los parametros de actualización del registro
            object[] param = { 6, 0, id_nomina_empleado, 0, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtNominaOtros = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtNominaOtros;
        }

        #endregion
    }
}
