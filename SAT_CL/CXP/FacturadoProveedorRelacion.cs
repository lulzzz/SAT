using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.CXP
{
    /// <summary>
    /// Clase que permite realizar acciones de Consulta, Inserción, Actualizacion sobre registros de la tabla Facturado Proveedor Relación
    /// </summary>
    public class FacturadoProveedorRelacion:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string _nom_sp = "cxp.sp_facturado_proveedor_relacion_tfp";
        private int _id_factura_proveedor_relacion;
        /// <summary>
        /// Almacena el identificador de una relacion de factura proveedor
        /// </summary>
        public int id_factura_proveedor_relacion
        {
            get { return _id_factura_proveedor_relacion; }
        }
        private int _id_factura_proveedor;
        /// <summary>
        /// Almacena el identificador de una factura de proveedor
        /// </summary>
        public int id_factura_proveedor
        {
            get { return _id_factura_proveedor; }
        }
        private int _id_tabla;
        /// <summary>
        /// Almacena el identificador de la tabla de procedencia de una relacion de factura proveedor
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Almacena el identificador de registro de procedencia de una realacion de factura proveedor
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el tipo de acceso de visibilidad a un registro(Habilitado = true = Disponible, Deshabilitado = false = No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }            
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor default que inicializa los atributos de la calse a 0
        /// </summary>
        public FacturadoProveedorRelacion()
        {
            this._id_factura_proveedor_relacion = 0;
            this._id_factura_proveedor = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicialza a los atributos a partir de un registro
        /// </summary>
        /// <param name="id_factura_proveedor_relacion">Identificador de un registro de Factura proveedor relacion </param>
        public FacturadoProveedorRelacion(int id_factura_proveedor_relacion)
        {
            //Invoca al método que realiza la busqueda de registro
            cargaAtributos(id_factura_proveedor_relacion);
        }
        /// <summary>
        /// Constructor que inicialza a los atributos a partir de un registro
        /// </summary>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro</param>
        public FacturadoProveedorRelacion(int id_factura_proveedor, int id_tabla, int id_registro)
        {
            //Invoca al método que realiza la busqueda de registro
            cargaAtributos(id_factura_proveedor, id_tabla, id_registro);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~FacturadoProveedorRelacion()
        {
            Dispose(false);
        }

        #endregion
    
        #region Métodos Privados

        /// <summary>
        /// Método que realiza la busqueda de un registro Factura Proveedor relacion a partir de un identificador
        /// </summary>
        /// <param name="id_factura_proveedor_relacion">Id que sirve como referencia para la busqueda de registros en BD </param>
        /// <returns></returns>
        private bool cargaAtributos(int id_factura_proveedor_relacion)
        {
            //Creación de objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 3, id_factura_proveedor_relacion, 0, 0, 0, 0, false, "", "" };
            //Invoca al metodo que realiza la busqueda del registro
            using(DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {
                //Valida los datos de datase (Que exista el registro)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre registro de la filas del dataset y el resultado los almacena en los atributos de la clase
                    foreach(DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_factura_proveedor_relacion = id_factura_proveedor_relacion;
                        this._id_factura_proveedor = Convert.ToInt32(r["IdFacturaProveedor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor de objeto retorno hasta que el foreach termine de recorrer las filas del dataset
                    retorno = true;
                }
            }
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de 
        /// </summary>
        /// <param name="id_factura_proveedor">Factura</param>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_factura_proveedor, int id_tabla, int id_registro)
        {
            //Creación de objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 9, 0, id_factura_proveedor, id_tabla, id_registro, 0, false, "", "" };
            //Invoca al metodo que realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos de datase (Que exista el registro)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre registro de la filas del dataset y el resultado los almacena en los atributos de la clase
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_factura_proveedor_relacion = Convert.ToInt32(r["Id"]);
                        this._id_factura_proveedor = Convert.ToInt32(r["IdFacturaProveedor"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor de objeto retorno hasta que el foreach termine de recorrer las filas del dataset
                    retorno = true;
                }
            }
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Factura Proveedor Relación
        /// </summary>
        /// <param name="id_factura_proveedor">Actualiza el identificador de una factura proveedor</param>
        /// <param name="id_tabla">Actualiza el identificador de la tabla de procedencia de una factura proveedor</param>
        /// <param name="id_registro">Actualiza el identificadoror del registro de procedencia de una factura proveedor</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado disponible y no disponible el registro</param>
        /// <returns></returns>
        private RetornoOperacion editarFacturadoProveedorRelacion(int id_factura_proveedor, int id_tabla, int id_registro, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar los campos de un registro.
            object[] param = { 2, this._id_factura_proveedor_relacion, id_factura_proveedor, id_tabla, id_registro, id_usuario, habilitar, "", "" };
            //Invoca al método encargado de actualizar los campos de un registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }

        #endregion

        #region Método Públicos

            /// <summary>
        /// Método que inserta un registro de Factura Proveedor Relación
        /// </summary>
        /// <param name="id_factura_proveedor">Inserta el identificador de una factura proveedor</param>
        /// <param name="id_tabla">Inserta el identificador de la tabla de procedencia de una factura proveedor</param>
        /// <param name="id_registro">Inserta el identificadoror del registro de procedencia de una factura proveedor</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>
        public static RetornoOperacion InsertarFacturadoProveedorRelacion(int id_factura_proveedor, int id_tabla, int id_registro, int id_usuario)
        {
            //CReación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la insercion de un registro a base de datos
            object[] param = { 1, 0, id_factura_proveedor, id_tabla, id_registro, id_usuario, true, "", "" };
            //Asigna al objeto retorno el resultado de invocar al método encargado de realizar la insercion del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de Factura Proveedor Relación
        /// </summary>
        /// <param name="id_factura_proveedor">Actualiza el identificador de una factura proveedor</param>
        /// <param name="id_tabla">Actualiza el identificador de la tabla de procedencia de una factura proveedor</param>
        /// <param name="id_registro">Actualiza el identificadoror del registro de procedencia de una factura proveedor</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditarFacturadoProveedorRelacion(int id_factura_proveedor, int id_tabla, int id_registro, int id_usuario)
        {
            //Invoca y retorna el método que edita los campos de un registro 
            return this.editarFacturadoProveedorRelacion(id_factura_proveedor, id_tabla, id_registro, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que actualiza los campos de un registro de Factura Proveedor Relación
        /// </summary>
        /// <param name="id_factura_proveedor">Actualiza el identificador de una factura proveedor</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        public RetornoOperacion EditarFacturadoProveedorRelacion(int id_factura_proveedor, int id_usuario)
        {
            //Invoca y retorna el método que edita los campos de un registro 
            return this.editarFacturadoProveedorRelacion(id_factura_proveedor, this._id_tabla, this._id_registro, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de un registro (de Disponible a No Disponible)
        /// </summary>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo el cambio de estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarFacturaPoveedorRelacion(int id_usuario)
        {
            //Invoca y retorna el método que edita los campos de un registro 
            return this.editarFacturadoProveedorRelacion(this._id_factura_proveedor, this._id_tabla, this._id_registro, id_usuario, false);
        }
        /// <summary>
        /// Método que actualiza los valores de los atributos de la clase.
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturadoProveedorRelacion()
        {
            //Invoca y retorna el método que realiza la busqueda de registros.
            return this.cargaAtributos(this._id_factura_proveedor_relacion);
        }
        /// <summary>
        /// Método encargado de Obtener las Relaciones con las Facturas de Proveedor
        /// </summary>
        /// <param name="id_factura_proveedor">Factura de Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneRelacionesFactura(int id_factura_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtRelaciones = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la insercion de un registro a base de datos
            object[] param = { 7, 0, id_factura_proveedor, 0, 0, 0, false, "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tabla
                    dtRelaciones = ds.Tables["Table"];
            }

            //Devolviendo Resultado de Retorno
            return dtRelaciones;
        }
        /// <summary>
        /// Método encargado de Obtener Facturas por Entidad
        /// </summary>
        /// <param name="id_entidad">Entidad de Liga</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="suma_entidad_solicitada">Integra el monto por Aplicar</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasEntidad(int id_entidad, int id_registro, bool suma_entidad_solicitada)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la insercion de un registro a base de datos
            object[] param = { 8, 0, 0, 0, 0, 0, suma_entidad_solicitada, id_entidad.ToString(), id_registro.ToString() };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tabla
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturas;
        }

        #endregion

        #region Métodos Facturas Comprobaciones

        /// <summary>
        /// Método Público encargado de Obtener las Facturas Ligadas a una Comprobación dada.
        /// </summary>
        /// <param name="id_comprobacion">Comprobación</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasComprobacion(int id_comprobacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, id_comprobacion, 0, true, "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tabla
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturas;
        }
        /// <summary>
        /// Método Público encargado de Obtener el Total de las Facturas Ligadas a una Comprobación dada.
        /// </summary>
        /// <param name="id_comprobacion">Comprobación</param>
        /// <returns></returns>
        public static decimal ObtieneTotalFacturasComprobacion(int id_comprobacion)
        {
            //Declarando Objeto de Retorno
            decimal totalFacturas = 0.00M;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, 0, id_comprobacion, 0, true, "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Resultado Obtenido
                        totalFacturas = Convert.ToDecimal(dr["TotalFacturas"]);

                }
            }

            //Devolviendo Resultado Obtenido
            return totalFacturas;
        }

        #endregion

        #region Métodos Anticipos

        /// <summary>
        /// Método encargado de Obtener las Relaciones de Facturas de Proveedor con los Anticipos
        /// </summary>
        /// <param name="id_factura_proveedor">Factura de Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasRelacionAnticipos(int id_factura_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Armando Arreglo de Parametros
            object[] param = { 6, 0, id_factura_proveedor, 0, 0, 0, true, "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tabla
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtAnticipos;
        }

        #endregion
    }
}
