using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using TSDK.Base;

namespace SAT_CL.CXP
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con la Recepción
    /// </summary>
    public class Recepcion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "cxp.sp_recepcion_tr";

        private int _id_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Recepción
        /// </summary>
        public int id_recepcion { get { return this._id_recepcion; } }
        private int _id_compania_proveedor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania Proveedora
        /// </summary>
        public int id_compania_proveedor { get { return this._id_compania_proveedor; } }
        private int _id_compania_receptor;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Compania Receptora
        /// </summary>
        public int id_compania_receptor { get { return this._id_compania_receptor; } }
        private int _secuencia;
        /// <summary>
        /// Atributo encargado de almacenar la secuencia por compania receptora
        /// </summary>
        public int secuencia { get { return this._secuencia; } }
        private DateTime _fecha_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar al Fecha de Recepción
        /// </summary>
        public DateTime fecha_recepcion { get { return this._fecha_recepcion; } }
        private string _entregado_por;
        /// <summary>
        /// Atributo encargado de almacenar la Referencia de quien Entrega
        /// </summary>
        public string entregado_por { get { return this._entregado_por; } }
        private byte _id_medio_recepcion;
        /// <summary>
        /// Atributo encargado de almacenar el Medio de Recepción
        /// </summary>
        public byte id_medio_recepcion { get { return this._id_medio_recepcion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion 

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Recepcion()
        {   //Asignando Valores
            this._id_recepcion = 0;
            this._id_compania_proveedor = 0;
            this._id_compania_receptor = 0;
            this._secuencia = 0;
            this._fecha_recepcion = DateTime.MinValue;
            this._entregado_por = "";
            this._id_medio_recepcion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_recepcion">Id de Recepción</param>
        public Recepcion(int id_recepcion)
        {
            cargaAtributosInstancia(id_recepcion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase encargado de Liberar los Recursos
        /// </summary>
        ~Recepcion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_recepcion">Id de Recepción</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_recepcion)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_recepcion, 0, 0, 0, null, "", 0, 0, false, "", "" };
            //Instanciando Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_recepcion = id_recepcion;
                        this._id_compania_proveedor = Convert.ToInt32(dr["IdCompaniaProveedor"]);
                        this._id_compania_receptor = Convert.ToInt32(dr["IdCompaniaReceptor"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        DateTime.TryParse(dr["FechaRecepcion"].ToString(), out this._fecha_recepcion);
                        this._entregado_por = dr["EntregadoPor"].ToString();
                        this._id_medio_recepcion = Convert.ToByte(dr["IdMedioRecepcion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Reusltado Positivo
                result = true;
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_compania_proveedor">Compania Proveedora</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="secuencia">Secuencia por compania receptora</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="entregado_por">Entregado Por</param>
        /// <param name="id_medio_recepcion">Medio de Recepción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_compania_proveedor, int id_compania_receptor, int secuencia, DateTime fecha_recepcion, 
                                            string entregado_por, byte id_medio_recepcion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_recepcion, id_compania_proveedor, id_compania_receptor, secuencia, fecha_recepcion, entregado_por, 
                               id_medio_recepcion, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Deolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Recepciones de las Facturas
        /// </summary>
        /// <param name="id_compania_proveedor">Compania Proveedora</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="secuencia">Secuencia por compania receptora</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="entregado_por">Entregado Por</param>
        /// <param name="id_medio_recepcion">Medio de Recepción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaRecepcion(int id_compania_proveedor, int id_compania_receptor, DateTime fecha_recepcion,
                                            string entregado_por, byte id_medio_recepcion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania_proveedor, id_compania_receptor, 0, fecha_recepcion, entregado_por, 
                               id_medio_recepcion, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Deolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Recepciones de las Facturas
        /// </summary>
        /// <param name="id_compania_proveedor">Compania Proveedora</param>
        /// <param name="id_compania_receptor">Compania Receptora</param>
        /// <param name="fecha_recepcion">Fecha de Recepción</param>
        /// <param name="entregado_por">Entregado Por</param>
        /// <param name="id_medio_recepcion">Medio de Recepción</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaRecepcion(int id_compania_proveedor, int id_compania_receptor, int secuencia, DateTime fecha_recepcion,
                                            string entregado_por, byte id_medio_recepcion, int id_usuario)
        {   //Invocando Método de Carga
            return this.actualizaRegistros(id_compania_proveedor, id_compania_receptor, secuencia, fecha_recepcion, entregado_por,
                               id_medio_recepcion, id_usuario, this._habilitar);
        }
        /// <summary>aa
        /// Método Público encargado de Deshabilitar la Recepción de la Factura
        /// </summary>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRecepcion(int id_usuario)
        {   //Invocando Método de Carga
            return this.actualizaRegistros(this._id_compania_proveedor, this._id_compania_receptor, this._secuencia, this._fecha_recepcion, this._entregado_por,
                               this._id_medio_recepcion, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar la Recepción de la Factura
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRecepcion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_recepcion);
        }

        #endregion

        #region Métodos Tabla Temporal

        /// <summary>
        /// Devuelve la tabla utilizada para almacenamiento temporal de registros equipo ligados a un servicio
        /// </summary>
        /// <param name="nombre_tabla">Nombre que se asignará a tabla temporal</param>
        /// <returns></returns>
        public static DataTable EsquemaTemporalRecepcionFactura(string nombre_tabla)
        {
            //Declarando objeto de retorno
            DataTable mit = new DataTable(nombre_tabla);

            //Definiendo los campos que debe contener
            mit.Columns.Add("*IdAutonumerico", typeof(int));
            mit.Columns["*IdAutonumerico"].AutoIncrement = true;
            mit.Columns["*IdAutonumerico"].AutoIncrementSeed = 1;
            mit.Columns["*IdAutonumerico"].AutoIncrementStep = 1;
            mit.Columns.Add("*IdFactura", typeof(int));
            mit.Columns.Add("Serie", typeof(string));
            mit.Columns.Add("Folio", typeof(int));
            mit.Columns.Add("MontoCaptura", typeof(decimal));
            mit.Columns.Add("FechaFacturacion", typeof(DateTime));

            //Devolviendo tabla generada
            return mit;
        }
        /// <summary>
        /// Inserta un nuevo registro dentro de la tabla temporal especificada, devolviendo el ID autonumérico asignado
        /// </summary>
        /// <param name="mit">Tabla</param>
        /// <param name="Serie">Serie</param>
        /// <param name="Folio">Numero de Folio</param>
        /// <param name="FechaFacturacion">Fecha de Facturacion</param>
        /// <param name="MontoCaptura">Monto de Captura</param>
        /// <param name="IdFactura">id de Referencia</param>
        /// <returns></returns>
        public static int InsertaRegistroTablaTemporal(DataTable mit, string Serie, int Folio, DateTime FechaFacturacion, Double MontoCaptura, int IdFactura)
        {
            //Declarando objeto de retorno
            int idRegistro = 0;

            //Validando el origen de datos(sólo existencia de un equema de tabla)
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit, false))
            {
                //Validando que no exista ningun registro la misma serie y folio
                if ((from DataRow r in mit.Rows
                     where ((r.Field<string>("Serie") == Serie)
                     && (r.Field<int>("Folio") == Folio))
                     select r).Count() == 0)
                {
                    //Insertando nuevo registro
                    try
                    {
                        //Instanciando nueva fila
                        DataRow nuevo = mit.NewRow();

                        //Configurando datos de registro
                        nuevo.SetField("*IdFactura", IdFactura);
                        nuevo.SetField("Serie", Serie);
                        nuevo.SetField("Folio", Folio);
                        nuevo.SetField("MontoCaptura", MontoCaptura);
                        nuevo.SetField("FechaFacturacion", FechaFacturacion);

                        //Añadiendo el registro a la tabla
                        mit.Rows.Add(nuevo);
                        //Asignando Id de registro insertado
                        idRegistro = (from DataRow r in mit.Rows
                                      select r.Field<int>("*IdAutonumerico")).Max();
                    }
                    catch (Exception) { }
                }
            }

            //Devolvinedo ID asignado
            return idRegistro;
        }
        /// <summary>
        /// Edita el monto en un registro existente indicado por el Id autonumérico asignado al mismo.
        /// </summary>
        /// <param name="mit">Tabla donde se encuentra el registro a editar</param>
        /// <param name="IdAutonumerico">Id autonumérico del registro a editar</param>
        /// <param name="serie">Serie</param>
        /// <param name="folio">Folio</param>
        /// <param name="fechaFacturacion">Fecha Facturación</param>
        /// <param name="montoCaptura">Monto Captura</param>
        /// <returns></returns>
        public static int EditaRegistroTablaTemporal(DataTable mit, int IdAutonumerico, string serie, int folio, DateTime fechaFacturacion, double montoCaptura)
        {   //Declarando objeto de retorno
            int idRegistro = -1;
            //Validando el origen de datos(sólo existencia de un equema de tabla)
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit, false))
            {   //Obteniendo el registro por editar
                DataRow concepto = (from DataRow r in mit.Rows
                                    where r.Field<int>("*IdAutonumerico") == IdAutonumerico
                                    select r).FirstOrDefault();
                //Validando que el registro exista
                if (concepto != null)
                {
                    try
                    {
                        concepto.SetField<string>("serie", serie);
                        concepto.SetField<int>("folio", folio);
                        concepto.SetField<DateTime>("fechaFacturacion", fechaFacturacion);
                        concepto.SetField<double>("montoCaptura", montoCaptura);
                        //Asignando Id de registro editado
                        idRegistro = IdAutonumerico;
                    }
                    catch (Exception) { }
                }
            }
            //Devolvinedo ID asignado
            return idRegistro;
        }
        /// <summary>
        /// Elimina un registro existente en la tabla temporal a partir de su Id Autonumérico asignado
        /// </summary>
        /// <param name="mit">Tabla a utilizar</param>
        /// <param name="idAutonumerico">Id Autonumérico del registro a eliminar</param>
        /// <returns></returns>
        public static int EliminaRegistroTablaTemporal(DataTable mit, int idAutonumerico)
        {   //Declarando objeto de retorno
            int idRegistro = 0;
            //Validando el origen de datos(sólo existencia de un equema de tabla)
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit, true))
            {   //Asignando Id de registro insertado
                DataRow registro = (from DataRow r in mit.Rows
                                    where r.Field<int>("*IdAutonumerico") == idAutonumerico
                                    select r).FirstOrDefault();
                //Si el registro fue encontrado
                if (registro != null)
                {
                    try
                    {   //Asiganndo Id de registro eliminado
                        idRegistro = registro.Field<int>("*IdAutonumerico");
                        //Eliminando de la tabla temporal
                        mit.Rows.Remove(registro);

                    }
                    catch (Exception) { idRegistro = 0; }
                }
            }
            //Devolvinedo ID asignado
            return idRegistro;
        }

        #endregion
    }
}
