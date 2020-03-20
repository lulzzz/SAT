using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.EgresoServicio;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que permite realizar acciones sobre los registros de una ruta(Insertar, Editar, Consultar)
    /// </summary>
    public class Ruta : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla ruta
        /// </summary>
        private static string nom_sp = "ruta.sp_ruta_tr";
        private int _id_ruta;
        /// <summary>
        /// Identificador de una ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Identificador de una compañia a la cual pertenece el uso de una ruta en especificó
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _id_cliente;
        /// <summary>
        /// Identificador del cliente que hace uso de esa ruta
        /// </summary>
        public int id_cliente
        {
            get { return _id_cliente; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas que permitan el reconocimiento de una ruta
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private string _alias_ruta;
        /// <summary>
        /// Nombre corto de la ruta
        /// </summary>
        public string alias_ruta
        {
            get { return _alias_ruta; }
        }
        private int _tipo_aplicacion;
        /// <summary>
        /// Identificador de tipo_aplicacion (Planeado o Iniciado)
        /// </summary>
        public int tipo_aplicacion
        {
            get { return _tipo_aplicacion; }
        }
        private int _id_tabla_origen;
        /// <summary>
        /// Identificador de la tabla de procedencia de un origen (Ciudad o Ubicación)
        /// </summary>
        public int id_tabla_origen
        {
            get { return _id_tabla_origen; }
        }
        private int _id_registro_origen;
        /// <summary>
        ///identificador del registro que identifica la procedencia del origen 
        /// </summary>
        public int id_registro_origen
        {
            get { return _id_registro_origen; }
        }
        private int _id_tabla_destino;
        /// <summary>
        /// identificador de la tabla de procedencia de un destino (Ciudad o Ubicación)
        /// </summary>
        public int id_tabla_destino
        {
            get { return _id_tabla_destino; }
        }
        private int _id_registro_destino;
        /// <summary>
        /// Identificador del registro de un destino
        /// </summary>
        public int id_registro_destino
        {
            get { return _id_registro_destino; }
        }
        private decimal _kilometraje;
        /// <summary>
        /// Kilometraje de un orige a un destino
        /// </summary>
        public decimal kilometraje
        {
            get { return _kilometraje; }
        }
        private bool _bit_permisionario;
        /// <summary>
        /// Define si el uso de una ruta esta disponible para un permisionario o no
        /// </summary>
        public bool bit_permisionario
        {
            get { return _bit_permisionario; }
        }
        private bool _bit_comprobacion;
        /// <summary>
        /// Define si el uso de una ruta esta disponible para un permisionario o no
        /// </summary>
        public bool bit_comprobacion
        {
            get { return _bit_comprobacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define el esstado de uso del registro (Habilitado-Disponible, Deshabilitado-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public Ruta()
        {
            this._id_ruta = 0;
            this._id_compania_emisor = 0;
            this._id_cliente = 0;
            this._descripcion = "";
            this._alias_ruta = "";
            this._tipo_aplicacion = 0;
            this._id_tabla_origen = 0;
            this._id_registro_origen = 0;
            this._id_tabla_destino = 0;
            this._id_registro_destino = 0;
            this._kilometraje = 0;
            this._bit_permisionario = false;
            this._bit_comprobacion = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Cosntructor que inicializa los atributos a aprtir de un registro
        /// </summary>
        /// <param name="id_ruta">Identificador de una ruta</param>
        public Ruta(int id_ruta)
        {
            //Invoca al método que realiza la busqueda y asignación de datos a los atributos
            cargaAtributos(id_ruta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Ruta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método encargado de realizar la busqueda de un registro y el resultado almacenarlo en los atributos de la clase
        /// </summary>
        /// <param name="id_ruta">Id que sirve como referencia para realizar la busqueda del registro</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ruta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 3, id_ruta, 0, 0, "", "", 0, 0, 0, 0, 0, 0, false, false, 0, false, "", "" };
            //Realiza la busqueda del registro ruta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los valores del dataset
                if (Validacion.ValidaOrigenDatos(DS))
                {
                    //Recorre las filas del dataset y asigna el resultado a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ruta = id_ruta;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompania"]);
                        this._id_cliente = Convert.ToInt32(r["IdCliente"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._alias_ruta = Convert.ToString(r["Alias"]);
                        this._tipo_aplicacion = Convert.ToInt32(r["TipoAplicacion"]);
                        this._id_tabla_origen = Convert.ToInt32(r["IdTablaOrigen"]);
                        this._id_registro_origen = Convert.ToInt32(r["IdRegistroOrigen"]);
                        this._id_tabla_destino = Convert.ToInt32(r["IdTablaDestino"]);
                        this._id_registro_destino = Convert.ToInt32(r["IdRegistroDestino"]);
                        this._kilometraje = Convert.ToDecimal(r["Kilometraje"]);
                        this._bit_permisionario = Convert.ToBoolean(r["BitPermisionario"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de ruta
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de una compañia a la que pertenece una ruta en específico</param>
        /// <param name="id_cliente">Actualiza el identificador del cliente</param>
        /// <param name="descripcion">Actualiza el nombre o las caracteristicas de una ruta</param>
        /// <param name="alias_ruta">Actualiza el nombre corto de una ruta</param>
        /// <param name="tipo_aplicacion">Actualiza el tipo aplicacion</param>
        /// <param name="id_tabla_origen">Actualiza el identificador de la tabla origen (Ciudad o Ubicación)</param>
        /// <param name="id_registro_origen">Actualiza el identificador de registro origen</param>
        /// <param name="id_tabla_destino">Actualiza el identificador de la tabla destino (Ciudad o Ubicación)</param>
        /// <param name="id_registro_destino">Actualiza el identificador del registro destino</param>
        /// <param name="kilometraje">Actualiza el kilometraje</param>
        /// <param name="bit_permisionario">Actualiza si la ruta es utilizada por permisionario o no</param>
        /// <param name="bit_comprobacion">Actualiza el bit comprobacion</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo las actulizaciones al registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible, Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarRuta(int id_compania_emisor, int id_cliente, string descripcion, string alias_ruta, int tipo_aplicacion ,int id_tabla_origen, int id_registro_origen,
                                            int id_tabla_destino, int id_registro_destino, decimal kilometraje, bool bit_permisionario, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para la edición de un registro
            object[] param = { 2, this._id_ruta, id_compania_emisor, id_cliente, descripcion, alias_ruta, tipo_aplicacion, id_tabla_origen, id_registro_origen, id_tabla_destino, id_registro_destino, kilometraje, bit_permisionario, 1, id_usuario, habilitar, "", "" };
            //Realiza la edición del registro de ruta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Método Publica
        /// <summary>
        /// Método que inserta un registro de ruta
        /// </summary>
        /// <param name="id_compania_emisor">Inserta el identificador de una compañia a la que pertenece una ruta en especifico</param>
        /// <param name="id_cliente">Inserta el identificador del cliente</param>
        /// <param name="descripcion">Inserta el nombre o las caracteristicas de una ruta</param>
        /// <param name="alias_ruta">Inserta el nombre corto de una ruta</param>
        /// <param name="tipo_aplicacion">Actualiza el tipo aplicacion</param>
        /// <param name="id_tabla_origen">Inserta el identificador de la tabla origen (Ciudad o Ubicación)</param>
        /// <param name="id_registro_origen">Inserta el identificador de registro origen</param>
        /// <param name="id_tabla_destino">Inserta el identificador de la tabla destino (Ciudad o Ubicación)</param>
        /// <param name="id_registro_destino">Inserta el identificador del registro destino</param>
        /// <param name="kilometraje">Inserta el kilometraje</param>
        /// <param name="bit_permisionario">Inserta si la ruta es utilizada por permisionario o no</param>
        /// <param name="bit_comprobacion"></param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo las actulizaciones al registro</param>
        /// <param name="habilitar">Inserta el estado de uso del registro (Habilitado-Disponible, Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarRuta(int id_compania_emisor, int id_cliente, string descripcion, string alias_ruta, int tipo_aplicacion, int id_tabla_origen, int id_registro_origen,
                                                    int id_tabla_destino, int id_registro_destino, decimal kilometraje, bool bit_permisionario, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos para la inserción de un registro
            object[] param = { 1, 0, id_compania_emisor, id_cliente, descripcion, alias_ruta, tipo_aplicacion, id_tabla_origen, id_registro_origen, id_tabla_destino, id_registro_destino, kilometraje, bit_permisionario, 1, id_usuario, true, "", "" };
            //Realiza la inserción del registro de ruta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de ruta
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el identificador de una compañia a la que pertenece una ruta en especifico</param>
        /// <param name="id_cliente">Actualiza el identificador del cliente</param>
        /// <param name="descripcion">Actualiza el nombre o las caracteristicas de una ruta</param>
        /// <param name="alias_ruta">Actualiza el nombre corto de una ruta</param>
        /// <param name="tipo_aplicacion">Actualiza el tipo aplicacion</param>
        /// <param name="id_tabla_origen">Actualiza el identificador de la tabla origen (Ciudad o Ubicación)</param>
        /// <param name="id_registro_origen">Actualiza el identificador de registro origen</param>
        /// <param name="id_tabla_destino">Actualiza el identificador de la tabla destino (Ciudad o Ubicación)</param>
        /// <param name="id_registro_destino">Actualiza el identificador del registro destino</param>
        /// <param name="kilometraje">Actualiza el kilometraje</param>
        /// <param name="bit_permisionario">Actualiza si la ruta es utilizada por permisionario o no</param>
        /// <param name="bit_comprobacion"></param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo las actulizaciones al registro</param>        
        /// <returns></returns>
        public RetornoOperacion EditarRuta(int id_compania_emisor, int id_cliente, string descripcion, string alias_ruta, int tipo_aplicacion, int id_tabla_origen, int id_registro_origen,
                                           int id_tabla_destino, int id_registro_destino, decimal kilometraje, bool bit_permisionario, int id_usuario)
        {
            //Retorna al método el método que actualiza los campos del registro
            return this.editarRuta(id_compania_emisor, id_cliente, descripcion, alias_ruta, tipo_aplicacion, id_tabla_origen, id_registro_origen, id_tabla_destino,
                                   id_registro_destino, kilometraje, bit_permisionario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Cambia el estado de uso del registro de ruta
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo al cambio de estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion Deshabilitar(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                /*Deshabilitamos Casetas*/
                //Cargamos Casetas
                using (DataTable mit = RutaCaseta.CargaCasetas(this._id_ruta))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos Casetas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos casetas
                                using (RutaCaseta objRutaCaseta = new RutaCaseta(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Caseta
                                    resultado = objRutaCaseta.DeshabilitarRutaCaseta(id_usuario);
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
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    /*Deshabilitamos Tipos de Unidad*/
                    //Cargamos Tipo de Unidades
                    using (DataTable mitTipoUnidad = RutaTipoUnidad.CargaTipoUnidad(this._id_ruta))
                    {
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(mitTipoUnidad))
                        {
                            //Recorremos Tipo de Unidad
                            foreach (DataRow r in mitTipoUnidad.Rows)
                            {
                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilitamos Tipo de Unidad
                                    using (RutaTipoUnidad objTipoUnidad = new RutaTipoUnidad(r.Field<int>("Id")))
                                    {
                                        //Deshabilitamos Tuipo de Unidad
                                        resultado = objTipoUnidad.DeshabilitaRutaTipoUnidad(id_usuario);
                                    }
                                }
                                else
                                {
                                    //Salimos dell ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
                //Validamos rResultado
                if(resultado.OperacionExitosa)
                {
                    //Obtenemos Depositos
                    using(DataTable mitDepositos = RutaDeposito.ObtieneConceptoDeposito(this._id_ruta))
                    {
                        //Validamos Origen de Datos
                        if(Validacion.ValidaOrigenDatos(mitDepositos))
                        {
                            //Recorremos los Depositos
                            foreach(DataRow r in mitDepositos.Rows)
                            {
                                //Validamos resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Instancioamos DSeposito
                                    using(RutaDeposito objDeposito = new RutaDeposito(r.Field<int>("Id")))
                                    {
                                        //Deshabilitamos Deposito
                                        resultado = objDeposito.DeshabilitaRutaDeposito(id_usuario);
                                    }
                                }
                                else
                                    //Salimos del ciclo
                                    break;
                            }
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Retorna al método el método que actualiza los campos del registro
                    resultado = editarRuta(this._id_compania_emisor, this._id_cliente, this._descripcion, this._alias_ruta, this._tipo_aplicacion, this._id_tabla_origen, this._id_registro_origen,
                                           this._id_tabla_destino, this._id_registro_destino, this._kilometraje, this._bit_permisionario, id_usuario, false);
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Cerramos Transacción
                   scope.Complete();
                }
            }
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        ///  Metodo encargado de Copiar una Ruta
        /// </summary>
        /// <param name="id_ruta"></param>
        /// <param name="mit_ruta_caseta"></param>
        /// <param name="mit_ruta_tipo_unidad"></param>
        /// <param name="mitDepositos"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion CopiaRuta(int id_ruta, DataTable mit_ruta_caseta,DataTable mit_ruta_tipo_unidad, DataTable mit_depositos, int id_usuario)
        {
            //Declramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
             //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {               
                //Validamos Id Casetas
                if (Validacion.ValidaOrigenDatos(mit_ruta_caseta))
                {
                    //Rrecorremos cada uno de los Id de Casetas
                    foreach (DataRow  r in mit_ruta_caseta.Rows)
                    {
                        //Si el Resultado es exitoso
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Ruta Caseta
                            using (RutaCaseta objRutaCaseta = new RutaCaseta(r.Field<int>("Id")))
                            {
                                //Insertamos Ruta
                                resultado = RutaCaseta.InsertarRutaCaseta(id_ruta, objRutaCaseta.id_caseta, id_usuario, objRutaCaseta.id_tipo_deposito);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //Si el Resultado fue exitoso
                if (resultado.OperacionExitosa)
                {
                    //Validamos Tipo de Unidad
                    if (Validacion.ValidaOrigenDatos(mit_ruta_tipo_unidad))
                    {
                        //Recorremos cada uno de los Tipos de Unidad
                        foreach (DataRow  r in mit_ruta_tipo_unidad.Rows)
                        {
                            //Si el Resultado es exitoso
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Tipos de Unidad
                                using (RutaTipoUnidad objRutaTipoUnidad = new RutaTipoUnidad(r.Field<int>("Id")))
                                {
                                    //Insertamos Tipos de Unidad
                                    resultado = RutaTipoUnidad.InsertarRutaTipoUnidad(id_ruta, objRutaTipoUnidad.id_tipo_unidad, objRutaTipoUnidad.id_configuracion,
                                                objRutaTipoUnidad.rendimiento, id_usuario);
                                    //Asignamos Tipo de Unidad
                                    int IdRutaTipoUnidad = resultado.IdRegistro; ;
                                    //Validamos Resultados
                                    if(resultado.OperacionExitosa)
                                    {
                                        //Cargamos Vales de Acuerdo al Id Tipo de Unidad
                                        using(DataTable  mitVales = RutaUnidadDiesel.CargaVales(objRutaTipoUnidad.id_ruta_tipo_unidad))
                                        {

                                            //Vaidamos Origen de Datos
                                            if(Validacion.ValidaOrigenDatos(mitVales))
                                            {
                                                //Recorremos Vales
                                                foreach(DataRow v in mitVales.Rows)
                                                {
                                                    //Validamos resultado
                                                    if(resultado.OperacionExitosa)
                                                    {
                                                        //Instanciamos Vale
                                                        using(RutaUnidadDiesel objDiesel = new RutaUnidadDiesel(v.Field<int>("Id")))
                                                        {
                                                            //Insertamos Vale de Diesel
                                                            resultado = RutaUnidadDiesel.InsertarRutaUnidadDiesel(IdRutaTipoUnidad, (RutaUnidadDiesel.TipoOperacion)objDiesel.id_tipo_operacion, objDiesel.id_tabla ,objDiesel.id_registro, id_usuario);
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
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                //Si el resultado es Exitoso
                if(resultado.OperacionExitosa)
                {
                    //Validamos Depositos
                    if(Validacion.ValidaOrigenDatos(mit_depositos))
                    {
                        //Recorremos cada uno de los Depósitos
                        foreach(DataRow r in mit_depositos.Rows)
                        {
                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Deposito
                                using(RutaDeposito objDeposito = new RutaDeposito(r.Field<int>("Id")))
                                {
                                    //Insertamos Depositpo con Nueva ruta
                                    resultado = RutaDeposito.InsertarRutaDeposito(id_ruta, objDeposito.id_concepto_restriccion,
                                            (RutaDeposito.TipoMonto)objDeposito.id_tipo_monto, objDeposito.Monto, objDeposito.bit_comprobacion, id_usuario);
                                }
                            }
                            else
                                //Salimos del ciclo
                                break;
                        }
                    }
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }
          
            return resultado;
        }

        /// <summary>
        /// Carga Rutas Coincidentes ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaRutasServicio(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 4, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, false, false, 0, false, id_servicio, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
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
        /// Carga Rutas Coincidentes ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaRutasSecuencias(int id_servicio, int id_ruta, int id_segmento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 5, id_ruta, 0, 0, "", "", 0, 0, 0, 0, 0, 0, false, false, 0, false, id_servicio, id_segmento };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
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
        /// [OBSOLETE]
        ///  Método que se encarga de Calcular la Ruta
        /// </summary>
        /// <param name="mitCasetas"></param>
        /// <param name="mitVales"></param>
        /// <param name="mitDepositos"></param>
        /// <param name="id_ruta"></param>
        /// <param name="efectivo_casetas"></param>
        /// <param name="id_segmento"></param>
        /// <param name="id_movimiento"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion CalculaRuta(DataTable mitCasetas, DataTable mitVales, DataTable mitDepositos, int id_ruta, bool efectivo_casetas, int id_segmento, int id_movimiento, int id_compania_emisor, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("No se actualizo el segmento de la Ruta");
            RetornoOperacion resultadoCasetas = new RetornoOperacion();
            RetornoOperacion resultadoVales = new RetornoOperacion();
            RetornoOperacion resultadoDepositos = new RetornoOperacion();
            //Instanciamos Ruta
                using (Ruta objRuta = new Ruta(id_ruta))
                {
                    //Instanciamos Segmento
                    using (SAT_CL.Despacho.SegmentoCarga objSegmento = new Despacho.SegmentoCarga(id_segmento))
                    {
                        //Validamos que no exista el Calculo
                        if (objSegmento.id_ruta == 0)
                        {
                            //Instanciamos Movimimiento
                            using (SAT_CL.Despacho.Movimiento objMovimiento = new Despacho.Movimiento(id_movimiento))
                            {
                                //Instanciamos Servicio
                                using (SAT_CL.Documentacion.Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                                {
                                    //Obtenemos Movimiento Asignación de la Unidad
                                    using (SAT_CL.Despacho.MovimientoAsignacionRecurso objMovimientoAsignacionRecursoUnidad = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objMovimiento.id_movimiento,
                                         Despacho.MovimientoAsignacionRecurso.Tipo.Unidad, SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimiento.id_movimiento))),
                                        //Obtenemos Movimiento Asignación del Operador
                                        objMovimientoAsignacionRecursoOperador = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objMovimiento.id_movimiento,
                                           Despacho.MovimientoAsignacionRecurso.Tipo.Operador)),
                                        //Obtenemos Movimiento Asignación del Terceroi
                                        objMovimientoAsignacionRecursoTercero = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objMovimiento.id_movimiento,
                                           Despacho.MovimientoAsignacionRecurso.Tipo.Tercero)))
                                    {
                                        //Instanciamos Unidad 
                                        using (Unidad objUnidad = new Unidad(objMovimientoAsignacionRecursoUnidad.id_recurso_asignado))
                                        {
                                            //Instanciamos Operador
                                            using (Operador objOperador = new Operador(objMovimientoAsignacionRecursoOperador.id_recurso_asignado))
                                            {
                                                int id_operador = objUnidad.bit_no_propia == true ? 0 : objMovimientoAsignacionRecursoOperador.id_recurso_asignado;
                                                int id_unidad = objUnidad.bit_no_propia == true ? objMovimientoAsignacionRecursoUnidad.id_recurso_asignado : 0;
                                                int id_tercero = objMovimientoAsignacionRecursoTercero.id_recurso_asignado;
                                                string identificador = objUnidad.bit_no_propia == true ? objOperador.nombre : objUnidad.numero_unidad;
                                                int id_asignacion = id_unidad != 0 ? objMovimientoAsignacionRecursoUnidad.id_movimiento_asignacion_recurso : objMovimientoAsignacionRecursoOperador.id_movimiento_asignacion_recurso;
                                                //Validamos Ruta Valida para Permisionario en Caso de ser necesario
                                                if ((id_unidad != 0 && objRuta._bit_permisionario == true) || (id_unidad == 0 && objRuta._bit_permisionario == false))
                                                {
                                                    //Intsanciamos Asignaciín
                                                    using (Despacho.MovimientoAsignacionRecurso objMovimientoAsignacion = new Despacho.MovimientoAsignacionRecurso(id_asignacion))
                                                    {
                                                        //Validamos Estatus de la Asignación
                                                        if ((MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion == MovimientoAsignacionRecurso.Estatus.Iniciado || (MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion == MovimientoAsignacionRecurso.Estatus.Terminado)
                                                        {
                                                            //Validamos Existencia de Asignación de Unidad
                                                            if (objMovimientoAsignacionRecursoUnidad.id_movimiento_asignacion_recurso > 0)
                                                            {
                                                                //Validamos Origen de Datos Casetas
                                                                if (Validacion.ValidaOrigenDatos(mitCasetas))
                                                                {
                                                                    //Obteniendo el Monto a registrar
                                                                    decimal montoDeposito = (from DataRow r in mitCasetas.Rows
                                                                                             select r.Field<decimal>("Monto")).DefaultIfEmpty().Sum();
                                                                    //Validamos que el Monto de las Casetas sea Mayor a 0
                                                                    if (montoDeposito > 0)
                                                                    {
                                                                        //Instanciamos Concepto para Deposito de Caseta
                                                                        using (ConceptoDeposito objConcepto = new ConceptoDeposito(ConceptoDeposito.ObtieneConcepto("Casetas Ruta", 0)))
                                                                        {
                                                                            //Validamos Concepto
                                                                            if (objConcepto.id_concepto_deposito > 0)
                                                                            {
                                                                                //Establecemos Concepto Restriccion
                                                                                int ConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(objConcepto.id_concepto_deposito);
                                                                                //Insertamos la Casetas de la ruta
                                                                                resultadoCasetas = InsertaCasetasRuta(id_ruta, efectivo_casetas, id_usuario, objMovimiento, objServicio, id_operador, id_unidad,
                                                                                    id_tercero, identificador, montoDeposito, objConcepto, ConceptoRestriccion);
                                                                            }
                                                                            else
                                                                            {
                                                                                //Establecemos error
                                                                                resultadoCasetas = new RetornoOperacion("El Concepto de Casetas no Existe.");
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                                /*Insetamos Vales*/
                                                                //Insertamos Vales de la Ruta
                                                                resultadoVales = InsertaValesRuta(mitVales, id_ruta, id_compania_emisor, id_usuario, objServicio, id_operador, id_unidad, id_tercero, identificador, id_asignacion, objMovimientoAsignacion);

                                                                /*Insertamos Depositos*/
                                                                resultadoDepositos = insertarDepositosRuta(mitDepositos, id_ruta, id_usuario, objServicio, id_operador, id_unidad, id_tercero, identificador, id_asignacion);

                                                                //Validamos resultado
                                                                if (resultadoVales.OperacionExitosa || resultadoDepositos.OperacionExitosa || resultadoCasetas.OperacionExitosa)
                                                                {
                                                                    //Editamos Segmento
                                                                    resultado = objSegmento.EditaSegmentoCarga(objSegmento.id_servicio, objSegmento.secuencia, (Despacho.SegmentoCarga.Estatus)objSegmento.id_estatus_segmento,
                                                                                                    objSegmento.id_parada_inicio, objSegmento.id_parada_fin, id_ruta, id_usuario);
                                                                 

                                                                }
                                                            }
                                                            else
                                                                resultado = new RetornoOperacion("Es necesario las Asignación de Unidad para el Cálculo.");
                                                        }
                                                        else
                                                            resultado = new RetornoOperacion("El estatus de la Asignación debe ser Iniciada, Terminada y sin Liquidar.");
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion("Ruta sólo para permisionario");

                                            }
                                        }
                                    }

                                }

                            }
                        }
                        else
                            resultado = new RetornoOperacion("Ya se realizó el cáculo de la Ruta.");
                    }
                }           
            //devolvemos Resultado
            return resultado = new RetornoOperacion(string.Format("Resultado Caseta:{0}, Resultado Vales:{1}, Resultado Depositos:{2}, Resultado Segmento", resultadoCasetas.Mensaje, resultadoVales.Mensaje,
                                                   resultadoDepositos.Mensaje, resultado.Mensaje), true);
        }
        /// <summary>
        ///  Método que se encarga de Calcular la Ruta
        /// </summary>
        /// <param name="mitCasetas"></param>
        /// <param name="mitVales"></param>
        /// <param name="mitDepositos"></param>
        /// <param name="id_ruta"></param>
        /// <param name="efectivo_casetas"></param>
        /// <param name="id_segmento"></param>
        /// <param name="id_movimiento"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion CalculaRuta(DataTable mitCasetas, DataTable mitVales, DataTable mitDepositos, int id_ruta, int id_segmento, int id_movimiento, int id_compania_emisor, int id_asignacion_recurso, int id_operador_r, int id_proveedor_compania_r, List<int> rutas, List<int> segmento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("El tipo de aplicación de la ruta no es aplicable a este segmento");
            RetornoOperacion resultadoCasetas = new RetornoOperacion(-1);
            RetornoOperacion resultadoCasetasDepositos = new RetornoOperacion();
            RetornoOperacion resultadoVales = new RetornoOperacion();
            RetornoOperacion resultadoDepositos = new RetornoOperacion();
            RetornoOperacion resultadoIaves = new RetornoOperacion();
            //Instanciamos Ruta
            using (Ruta objRuta = new Ruta(id_ruta))
            {
                //Instanciamos Segmento
                using (SAT_CL.Despacho.SegmentoCarga objSegmento = new Despacho.SegmentoCarga(id_segmento))
                {
                    //Validamos que no exista el Calculo
                    if (objSegmento.id_ruta == 0)
                    {
                        //Instanciamos Movimimiento
                        using (SAT_CL.Despacho.Movimiento objMovimiento = new Despacho.Movimiento(id_movimiento))
                        {
                            //Instanciamos Servicio
                            using (SAT_CL.Documentacion.Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                            {
                                //Obtenemos Movimiento Asignación de la Unidad
                                using (SAT_CL.Despacho.MovimientoAsignacionRecurso objMovimientoAsignacionRecursoUnidad = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionRecursoIniciadaTerminada(objMovimiento.id_movimiento,
                                     Despacho.MovimientoAsignacionRecurso.Tipo.Unidad, SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimiento.id_movimiento))),
                                    //Obtenemos Movimiento Asignación del Operador
                                    objMovimientoAsignacionRecursoOperador = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objMovimiento.id_movimiento,
                                       Despacho.MovimientoAsignacionRecurso.Tipo.Operador)),
                                    //Obtenemos Movimiento Asignación del Terceroi
                                    objMovimientoAsignacionRecursoTercero = new Despacho.MovimientoAsignacionRecurso(Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objMovimiento.id_movimiento,
                                       Despacho.MovimientoAsignacionRecurso.Tipo.Tercero)))
                                {
                                    //Instanciamos Unidad 
                                    using (Unidad objUnidad = new Unidad(objMovimientoAsignacionRecursoUnidad.id_recurso_asignado))
                                    {
                                        //Instanciamos Operador
                                        using (Operador objOperador = new Operador(objMovimientoAsignacionRecursoOperador.id_recurso_asignado))
                                        {
                                            int id_operador = objUnidad.bit_no_propia == true ? objMovimientoAsignacionRecursoOperador.id_recurso_asignado : id_operador_r;
                                            int id_unidad = objUnidad.bit_no_propia == true ? objMovimientoAsignacionRecursoUnidad.id_recurso_asignado : 0;
                                            int id_tercero = objMovimientoAsignacionRecursoTercero.id_recurso_asignado > 0 ? objMovimientoAsignacionRecursoTercero.id_recurso_asignado : id_proveedor_compania_r;
                                            string identificador = objUnidad.bit_no_propia == true ? objOperador.nombre : objUnidad.numero_unidad;
                                            //int id_asignacion = id_unidad != 0 ? objMovimientoAsignacionRecursoUnidad.id_movimiento_asignacion_recurso : objMovimientoAsignacionRecursoOperador.id_movimiento_asignacion_recurso;
                                            int id_asignacion = id_asignacion_recurso;
                                            //Validamos Ruta Valida para Permisionario en Caso de ser necesario
                                            if ((id_unidad != 0 && objRuta._bit_permisionario == true) || (id_unidad == 0 && objRuta._bit_permisionario == false))
                                            {
                                                //Intsanciamos Asignaciín
                                                using (Despacho.MovimientoAsignacionRecurso objMovimientoAsignacion = new Despacho.MovimientoAsignacionRecurso(id_asignacion))
                                                {
                                                    //Validamos Estatus de la Asignación
                                                    if ((MovimientoAsignacionRecurso.Estatus)objMovimientoAsignacion.id_estatus_asignacion != MovimientoAsignacionRecurso.Estatus.Cancelado)
                                                    {
                                                        //Validamos Existencia de Asignación de Unidad
                                                        if (objMovimientoAsignacionRecursoUnidad.id_movimiento_asignacion_recurso > 0)
                                                        {
                                                            //Validamos Origen de Datos Casetas
                                                            if (Validacion.ValidaOrigenDatos(mitCasetas))
                                                            {
                                                                //Obteniendo el Monto a registrar
                                                                decimal montoDeposito = (from DataRow r in mitCasetas.Rows
                                                                                         select r.Field<decimal>("Deposito")).DefaultIfEmpty().Sum();
                                                                //Obteniendo el Monto a registrar
                                                                decimal monto = (from DataRow r in mitCasetas.Rows
                                                                                 select r.Field<decimal>("Monto")).DefaultIfEmpty().Sum();
                                                                //Validamos que el Monto de las Casetas Deposito sea Mayor a 0
                                                                if (montoDeposito > 0)
                                                                {
                                                                    //Instanciamos Concepto para Deposito de Caseta
                                                                    using (ConceptoDeposito objConcepto = new ConceptoDeposito(ConceptoDeposito.ObtieneConcepto("Casetas Ruta", 0)))
                                                                    {
                                                                        //Validamos Concepto
                                                                        if (objConcepto.id_concepto_deposito > 0)
                                                                        {
                                                                            //Establecemos Concepto Restriccion
                                                                            int ConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(objConcepto.id_concepto_deposito);
                                                                            //Insertamos la Casetas de la ruta
                                                                            resultadoCasetasDepositos = InsertaCasetasRuta(id_ruta, false, id_usuario, objMovimiento, objServicio, id_operador, id_unidad,
                                                                                id_tercero, identificador, montoDeposito, objConcepto, ConceptoRestriccion);
                                                                        }
                                                                        else
                                                                        {
                                                                            //Establecemos error
                                                                            resultadoCasetasDepositos = new RetornoOperacion("El Concepto de Casetas no Existe.");
                                                                        }

                                                                    }
                                                                }
                                                                //Validamos que el Monto de las Casetas Deposito sea Mayor a 0
                                                                if (monto > 0)
                                                                {
                                                                    //Instanciamos Concepto para Deposito de Caseta
                                                                    using (ConceptoDeposito objConcepto = new ConceptoDeposito(ConceptoDeposito.ObtieneConcepto("Casetas Ruta", 0)))
                                                                    {
                                                                        //Validamos Concepto
                                                                        if (objConcepto.id_concepto_deposito > 0)
                                                                        {
                                                                            //Establecemos Concepto Restriccion
                                                                            //Inserccion Tabla cruces 
                                                                            int ConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(objConcepto.id_concepto_deposito);
                                                                            //Insertamos la Casetas de la ruta
                                                                            resultadoCasetas = InsertaCasetasRuta(id_ruta, true, id_usuario, objMovimiento, objServicio, id_operador, id_unidad,
                                                                                id_tercero, identificador, monto, objConcepto, ConceptoRestriccion);
                                                                        }
                                                                        else
                                                                        {
                                                                            //Establecemos error
                                                                            resultadoCasetas = new RetornoOperacion("El Concepto de Casetas no Existe.");
                                                                        }

                                                                    }
                                                                }

                                                                List<DataRow> CruzadasIave = (from DataRow r in mitCasetas.AsEnumerable()
                                                                                              where r.Field<Decimal>("MontoIAVE") > 0
                                                                                              select r).ToList();
                                                                if (CruzadasIave.Count > 0)
                                                                {
                                                                    foreach (DataRow r in CruzadasIave)
                                                                    {
                                                                        resultadoIaves = SAT_CL.Ruta.CrucesAutorizadosIave.InsertaCrucesAutorizadosIAVE(Convert.ToInt32(r["IdCaseta"]), Convert.ToString(r["Descripcion"]), Convert.ToString(r["TAG"]), objSegmento.id_servicio, Convert.ToInt32(r["IdUnidad"]), id_segmento, 0, objServicio.cita_carga, objServicio.cita_descarga, Convert.ToDecimal(r["MontoIAVE"]), 1, id_usuario);
                                                                    }
                                                                }
                                                            }
                                                            if (Validacion.ValidaOrigenDatos(mitVales))
                                                            {
                                                                /*Insetamos Vales*/
                                                                //Insertamos Vales de la Ruta
                                                                resultadoVales = InsertaValesRutaprueba(mitVales, id_ruta, id_compania_emisor, id_usuario, objServicio, id_operador, id_unidad, id_tercero, identificador, id_asignacion, objMovimientoAsignacion);  
                                                            }
                                                            if (Validacion.ValidaOrigenDatos(mitDepositos))
                                                            {

                                                                /*Insertamos Depositos*/
                                                                resultadoDepositos = insertarDepositosRuta(mitDepositos, id_ruta, id_usuario, objServicio, id_operador, id_unidad, id_tercero, identificador, id_asignacion);

                                                            }

                                                            //Validamos resultado
                                                            //if (resultadoVales.OperacionExitosa || resultadoDepositos.OperacionExitosa || resultadoCasetas.OperacionExitosa)
                                                            if (resultadoIaves.OperacionExitosa || resultadoDepositos.OperacionExitosa || resultadoCasetas.OperacionExitosa || resultadoVales.OperacionExitosa)
                                                            {
                                                                for (int i = 0; i < segmento.Count; i++)
                                                                {
                                                                    //Instanciamos Segmento
                                                                    using (SAT_CL.Despacho.SegmentoCarga objSegmentoRutas = new Despacho.SegmentoCarga(segmento[i]))
                                                                    {
                                                                        if (objSegmentoRutas.habilitar)
                                                                        {
                                                                            //Editamos Segmento
                                                                            resultado = objSegmentoRutas.EditaSegmentoCarga(objSegmentoRutas.id_servicio, objSegmentoRutas.secuencia, (Despacho.SegmentoCarga.Estatus)objSegmentoRutas.id_estatus_segmento,
                                                                                                        objSegmentoRutas.id_parada_inicio, objSegmentoRutas.id_parada_fin, rutas[i], id_usuario);
                                                                        }
                                                                    }

                                                                }
                                                                //resultado = new RetornoOperacion(string.Format("Resultado Caseta:{0}, Resultado Vales:{1}, Resultado Depositos:{2}, Resultado Segmento", resultadoCasetas.Mensaje, resultadoVales.Mensaje,
                                                                  //        resultadoDepositos.Mensaje, resultado.Mensaje), true);
                                                                resultado = new RetornoOperacion(string.Format("Resultado Calculo:{0}", resultado.Mensaje), true);
                                                            }
                                                        }
                                                        else
                                                            resultado = new RetornoOperacion("Es necesario las Asignación de Unidad para el Cálculo.");
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion("El estatus de la Asignación debe ser Iniciada, Terminada y sin Liquidar.");
                                                }
                                            }
                                            else
                                                resultado = new RetornoOperacion("Ruta sólo para permisionario");

                                        }
                                    }
                                }

                            }

                        }
                    }
                    else
                        resultado = new RetornoOperacion("Ya se realizó el cáculo de la Ruta.");
                }
            }
            //devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Insertar Depósitos
        /// </summary>
        /// <param name="mitDepositos"></param>
        /// <param name="id_ruta"></param>
        /// <param name="id_usuario"></param>
        /// <param name="objServicio"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tercero"></param>
        /// <param name="identificador"></param>
        /// <param name="id_asignacion"></param>
        /// <returns></returns>
        private static RetornoOperacion insertarDepositosRuta(DataTable mitDepositos, int id_ruta, int id_usuario, SAT_CL.Documentacion.Servicio objServicio, int id_operador, int id_unidad, int id_tercero, string identificador, int id_asignacion)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Validamos Origen de datos
            if (Validacion.ValidaOrigenDatos(mitDepositos))
            {
                //Creamos la transacción 
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Recorremos cada uno de los Concepto
                    foreach (DataRow r in mitDepositos.Rows)
                    {
                        //Insertamos Deposito
                        resultado = Deposito.InsertaDeposito(objServicio.id_compania_emisor, identificador,
                        objServicio.id_cliente_receptor, r.Field<int>("IdConcepto"), id_ruta, r.Field<int>("Id"), "",
                        Deposito.TipoCargo.Depositante, false, r.Field<int>("IdRestriccion"),
                        id_unidad, id_operador, id_tercero, objServicio.id_servicio, id_asignacion, r.Field<decimal>("Monto"),Fecha.ObtieneFechaEstandarMexicoCentro(),
                        id_usuario);

                        //Instanciamos Depósito
                        using (Deposito objDeposito = new Deposito(resultado.IdRegistro))
                        {
                            //Obteniendo Deposito
                            int id_deposito = resultado.IdRegistro;

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Solicitamos Depósito
                                resultado = objDeposito.SolicitarDeposito(r.Field<int>("IdRestriccion"), id_usuario);

                            }
                        }
                        ////Validamos resultado
                        //if (!resultado.OperacionExitosa || resultado.IdRegistro != -2)
                        //{
                        //    //Salimos del Ciclo

                        //    break;
                        //}
                        //else
                        //    //Actualizamos Resultado a Existos
                        //    resultado = new RetornoOperacion("Depósitos registrados.", true);
                    }
                    //Validamos Resultado
                    if(resultado.OperacionExitosa)
                    {
                        //Finalizamos Transacción
                        scope.Complete();
                    }
                }

            }
            return resultado;
        }

        /// <summary>
        ///  Insertamos los Vales de la Ruta
        /// </summary>
        /// <param name="mitVales">Lista de Vales por Insertar</param>
        /// <param name="id_ruta"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="objServicio"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tercero"></param>
        /// <param name="identificador"></param>
        /// <param name="id_asignacion"></param>
        /// <param name="objMovimientoAsignacion"></param>
        /// <returns></returns>
        private static RetornoOperacion InsertaValesRuta(DataTable mitVales, int id_ruta, int id_compania_emisor, int id_usuario, SAT_CL.Documentacion.Servicio objServicio,
             int id_operador, int id_unidad, int id_tercero, string identificador, int id_asignacion, Despacho.MovimientoAsignacionRecurso objMovimientoAsignacion)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declaramos Variable Deposito
            int id_deposito = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Vales
                if (Validacion.ValidaOrigenDatos(mitVales))
                {
                    //Por Cada Vale de Diesel
                    foreach (DataRow diesel in mitVales.Rows)
                    {
                        //Validamos resultado
                        if (resultado.OperacionExitosa)
                        {

                            //Obtenemos Costo del Diesel
                            decimal costo_diesel = Convert.ToDecimal(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Precio Combustible"));
                            //Deacuerdo al Tipo de Vale
                            if (diesel.Field<string>("TipoOperacion") == "Efectivo (Tractor)" || diesel.Field<string>("TipoOperacion") == "Efectivo (Remolque)")
                            {
                                //Obtenomos Unidad Diesel
                                int id_concepto_disel = ConceptoDeposito.ObtieneConcepto("Diesel (Tractor)", 0);

                                //Validamos Unidad Motyriz o Arrastre
                                if (diesel.Field<string>("TipoOperacion") == "Efectivo (Remolque)")
                                {
                                    id_concepto_disel = ConceptoDeposito.ObtieneConcepto("Diesel (Remolque)", id_compania_emisor);

                                }

                                //Instanciamos Concepto para Diesel
                                using (ConceptoDeposito objConcepto = new ConceptoDeposito(id_concepto_disel))
                                {
                                    //Calculamos Monto
                                    decimal monto = diesel.Field<decimal>("litros") * costo_diesel;
                                    int ConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(objConcepto.id_concepto_deposito);
                                    //Registramos Depósito
                                    resultado = Deposito.InsertaDeposito(objServicio.id_compania_emisor, identificador,
                                                                objServicio.id_cliente_receptor, objConcepto.id_concepto_deposito, id_ruta, 0, "", Deposito.TipoCargo.Depositante,
                                                                true, ConceptoRestriccion,
                                                                id_unidad, id_operador, id_tercero, objServicio.id_servicio, id_asignacion, monto,
                                                                id_usuario);

                                    //Instanciamos Depósito
                                    using (Deposito objDeposito = new Deposito(resultado.IdRegistro))
                                    {
                                        //Obteniendo Deposito
                                        id_deposito = resultado.IdRegistro;

                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Solicitamos Depósito
                                            resultado = objDeposito.SolicitarDeposito(ConceptoRestriccion, id_usuario);
                                        }
                                    }
                                }
                            }
                            //Insertamos Vale
                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {

                                //Deacuerdo al Tipo de Vale
                                if (diesel.Field<string>("TipoOperacion") == "Vale (Tractor)" || diesel.Field<string>("TipoOperacion") == "Vale (Remolque)")
                                {
                                    //Obtenemos Costo
                                    int id_costo_vale = Convert.ToInt32(Cadena.RegresaCadenaSeparada(CostoCombustible.ObtieneCostoCombustibleEntidad(15, diesel.Field<int>("IdEstacion"), 1, Fecha.ConvierteDateTimeString(Fecha.ObtieneFechaEstandarMexicoCentro(), "yyyy-MM-dd HH:mm")).ToString(), ':', 1));

                                    //Instanciamos Costo
                                    using (CostoCombustible objCostoCombustible = new CostoCombustible(id_costo_vale))
                                    {
                                        //Validamos Existencia de Costo
                                        if (objCostoCombustible.id_costo_combustible > 0)
                                        {
                                            //Obtenomos Unidad Diesel
                                            int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);

                                            //Validamos Unidad Motyriz o Arrastre
                                            if (diesel.Field<string>("TipoOperacion") == "Vale (Remolque)")
                                            {
                                                id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadArrasteDiesel(objMovimientoAsignacion.id_movimiento);

                                            }

                                            //Validamos que Exista Unidad de Diesel
                                            if (id_unidad_diesel > 0)
                                            {
                                                //Insertando Diesel CORREGIR unidad dioesel
                                                resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaAsignacionDiesel(identificador, objServicio.id_compania_emisor, 0, diesel.Field<int>("IdEstacion"), 
                                                                                                    Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), 1, 0, 0, false, 
                                                                                                    "Diesel Ruta" + "[" + id_ruta + "]", 0, 0, (byte)AsignacionDiesel.TipoVale.Original, diesel.Field<decimal>("litros"), 
                                                                                                    objCostoCombustible.costo_combustible, id_unidad, id_unidad_diesel, id_operador, id_tercero, objServicio.id_servicio,
                                                                                                    id_asignacion, id_usuario);

                                                //Validando Operación Exitosa
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Obteniendo Asignación
                                                    int id_AsignacionDiesel = resultado.IdRegistro;

                                                    //Obteniendo Carga Autotanque Actva
                                                    using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(diesel.Field<int>("IdEstacion"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                                                    {
                                                        //Validando que exista una carga
                                                        if (carga.habilitar)
                                                        {
                                                            //Instanciando Detalle de Liquidación
                                                            using (DetalleLiquidacion dlDiesel = new DetalleLiquidacion(id_AsignacionDiesel, 69))
                                                            {
                                                                //Validando que exista
                                                                if (dlDiesel.habilitar)
                                                                {
                                                                    //Insertando Id de Carga como Referencia
                                                                    resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "IdCarga", 0, "Carga AutoTanque"),
                                                                                carga.id_carga_autotanque.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //Operación Exitosa
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Insertando Id de Carga como Referencia
                                                                        resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "SobranteDiesel", 0, "Carga AutoTanque"),
                                                                                    carga.sobrante_carga_actual.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                        //Validando Operación Exitosa
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Actualiza Sobrante de la Carga Actual (Carga actual - litros asignados)
                                                                            resultado = carga.ActualizaSobranteCargaActual(carga.sobrante_carga_actual - dlDiesel.cantidad, id_usuario);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    //Instanciando Excepción
                                                                    resultado = new RetornoOperacion("No existe el Vale de Diesel");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                //Establecemos Error
                                                resultado = new RetornoOperacion("No existe Diesel para Asignación de Unidad");
                                        }
                                        else
                                            //Establcemos Error
                                            resultado = new RetornoOperacion("No Existe el Costo para la Asignación de Vales de Diesel.");
                                    }
                                }
                                else
                                {
                                    //Insertando Vale de Diesel corregir Unidad Diesel
                                    resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaValeDieselPorDeposito(identificador,
                                                                objServicio.id_compania_emisor, 0,
                                                                TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                costo_diesel, false, "Diesel Ruta" + "[" + id_ruta + "]", id_deposito, (byte)AsignacionDiesel.TipoVale.Original,
                                                                diesel.Field<decimal>("litros"), id_unidad, id_operador, id_tercero, objServicio.id_servicio,
                                                                id_asignacion, id_usuario);


                                }
                            }

                        }

                        else
                            //Finalizamos ciclo
                            break;
                    }

                }
                //Validamos Operacion 
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }


            return resultado;
        }

        /// <summary>
        ///  Insertamos los Vales de la Ruta
        /// </summary>
        /// <param name="mitVales">Lista de Vales por Insertar</param>
        /// <param name="id_ruta"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="objServicio"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tercero"></param>
        /// <param name="identificador"></param>
        /// <param name="id_asignacion"></param>
        /// <param name="objMovimientoAsignacion"></param>
        /// <returns></returns>
        private static RetornoOperacion InsertaValesRutaprueba(DataTable mitVales, int id_ruta, int id_compania_emisor, int id_usuario, SAT_CL.Documentacion.Servicio objServicio,
          int id_operador, int id_unidad, int id_tercero, string identificador, int id_asignacion, Despacho.MovimientoAsignacionRecurso objMovimientoAsignacion)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            RetornoOperacion resultadotipo = new RetornoOperacion(0);
            //Declaramos Variable Deposito
            int id_deposito = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Vales
                if (Validacion.ValidaOrigenDatos(mitVales))
                {
                    //Por Cada Vale de Diesel
                    foreach (DataRow diesel in mitVales.Rows)
                    {
                        //Validamos resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Deacuerdo al Tipo de Vale
                            //if (diesel.Field<string>("TipoOperacion") == "Efectivo (Tractor)" || diesel.Field<string>("TipoOperacion") == "Efectivo (Remolque)")
                            if (diesel.Field<string>("TipoOperacion") == "Efectivo" )
                            {
                                //Validamos Existencia de Costo
                                if (diesel.Field<decimal>("CostoCombustible") > 0)
                                {
                                    //Obtenomos Unidad Diesel
                                    int id_concepto_disel = ConceptoDeposito.ObtieneConcepto(diesel.Field<string>("Concepto"), 0);

                                    //Instanciamos Concepto para Diesel
                                    using (ConceptoDeposito objConcepto = new ConceptoDeposito(id_concepto_disel))
                                    {
                                        //Calculamos Monto
                                        decimal monto = diesel.Field<decimal>("Total");
                                        int ConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(objConcepto.id_concepto_deposito);
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Registramos Depósito
                                        resultado = Deposito.InsertaDeposito(objServicio.id_compania_emisor, identificador,
                                                                    objServicio.id_cliente_receptor, objConcepto.id_concepto_deposito, id_ruta, 0, "Calculo Rutas", Deposito.TipoCargo.Depositante,
                                                                    true, ConceptoRestriccion,
                                                                    id_unidad, id_operador, id_tercero, objServicio.id_servicio, id_asignacion, monto, Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                    id_usuario);
                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
      
                                             //Instanciando Resultado con el Deposito
                                            //resultado = new RetornoOperacion(id_deposito);
                                            //Instanciamos Depósito
                                            using (Deposito objDeposito = new Deposito(resultado.IdRegistro))
                                            {
                                                //Obteniendo Deposito
                                                id_deposito = resultado.IdRegistro;
                                                //Registramos el depósito como programado
                                                resultado = SAT_CL.EgresoServicio.AnticipoProgramado.InsertaAnticipoProgramado(resultado.IdRegistro, objServicio.id_compania_emisor,
                                                                                                                    objServicio.id_servicio, ConceptoRestriccion, monto,
                                                                                                                    Fecha.ObtieneFechaEstandarMexicoCentro(), "Calculo Rutas", id_usuario);
                   

                                                //Validamos Resultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Solicitamos Depósito
                                                    resultado = objDeposito.SolicitarDeposito(ConceptoRestriccion, id_usuario);
                                                    //Actualizando Atributos
                                                    objDeposito.ActualizaDeposito();

                                                    //Instanciamos Concepto
                                                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConceptoDeposito = new ConceptoDeposito(id_concepto_disel))
                                                    {
                                                        //Validando que el Concepto sea de Diesel para Tractor o Remolque
                                                        if (objConceptoDeposito.descripcion == "Diesel (Tractor)" || objConceptoDeposito.descripcion == "Diesel (Remolque)" || objConceptoDeposito.descripcion == "G.Magna" || objConceptoDeposito.descripcion == "G.Premium")
                                                        {
                                                            //Validamos que Exista Unidad de Diesel
                                                            if (id_unidad_diesel > 0)
                                                            {
                                                                resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaAsignacionDieselProgramado(identificador, objServicio.id_compania_emisor, 0, diesel.Field<int>("IdEstacion"),
                                                                  Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), diesel.Field<int>("IdCosto"), (byte)(SAT_CL.EgresoServicio.CostoCombustible.Estatus)diesel.Field<byte>("IdTipo"), 0, false,
                                                                  "Diesel Monedero Ruta" + "[" + id_ruta + "]", 0, 0, (byte)AsignacionDiesel.TipoVale.Original, diesel.Field<decimal>("litros"),
                                                                  diesel.Field<decimal>("CostoCombustible"), id_unidad, id_unidad_diesel, id_operador, id_tercero, objServicio.id_servicio,
                                                                  id_asignacion, id_usuario);
                                                            }
                      
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                //Establcemos Error
                                resultado = new RetornoOperacion("No Existe el Costo para la Asignación de Vales de Diesel.");
                            }
                            //Insertamos Vale
                            //Validamos resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Deacuerdo al Tipo de Vale
                                //if (diesel.Field<string>("TipoOperacion") == "Vale (Tractor)" || diesel.Field<string>("TipoOperacion") == "Vale (Remolque)")
                                if (diesel.Field<string>("TipoOperacion") == "Vale" )
                                {

                                    //Validamos Existencia de Costo
                                    if (diesel.Field<decimal>("CostoCombustible") > 0)
                                    {
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Validamos que Exista Unidad de Diesel
                                        if (id_unidad_diesel > 0)
                                        {
                                            //(byte)(SAT_CL.EgresoServicio.CostoCombustible.Estatus)diesel.Field<byte>("IdTipo")
                                            //Insertando Diesel CORREGIR unidad dioesel
                                            resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaAsignacionDieselProgramado(identificador, objServicio.id_compania_emisor, 0, diesel.Field<int>("IdEstacion"),
                                                                                                Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), diesel.Field<int>("IdCosto"), (byte)(SAT_CL.EgresoServicio.CostoCombustible.Estatus)diesel.Field<byte>("IdTipo"), 0, false,
                                                                                                "Diesel Ruta" + "[" + id_ruta + "]", 0, 0, (byte)AsignacionDiesel.TipoVale.Original, diesel.Field<decimal>("litros"),
                                                                                                diesel.Field<decimal>("CostoCombustible"), id_unidad, id_unidad_diesel, id_operador, id_tercero, objServicio.id_servicio,
                                                                                                id_asignacion, id_usuario);

                                            //Validando Operación Exitosa
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obteniendo Asignación
                                                int id_AsignacionDiesel = resultado.IdRegistro;

                                                //Obteniendo Carga Autotanque Actva
                                                using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(diesel.Field<int>("IdEstacion"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                                                {
                                                    //Validando que exista una carga
                                                    if (carga.habilitar)
                                                    {
                                                        //Instanciando Detalle de Liquidación
                                                        using (DetalleLiquidacion dlDiesel = new DetalleLiquidacion(id_AsignacionDiesel, 69))
                                                        {
                                                            //Validando que exista
                                                            if (dlDiesel.habilitar)
                                                            {
                                                                //Insertando Id de Carga como Referencia
                                                                resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "IdCarga", 0, "Carga AutoTanque"),
                                                                            carga.id_carga_autotanque.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                //Operación Exitosa
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Insertando Id de Carga como Referencia
                                                                    resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "SobranteDiesel", 0, "Carga AutoTanque"),
                                                                                carga.sobrante_carga_actual.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //Validando Operación Exitosa
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualiza Sobrante de la Carga Actual (Carga actual - litros asignados)
                                                                        resultado = carga.ActualizaSobranteCargaActual(carga.sobrante_carga_actual - dlDiesel.cantidad, id_usuario);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                resultado = new RetornoOperacion("No existe el Vale de Diesel");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            //Establecemos Error
                                            resultado = new RetornoOperacion("No existe Diesel para Asignación de Unidad");
                                    }
                                    else
                                        //Establcemos Error
                                        resultado = new RetornoOperacion("No Existe el Costo para la Asignación de Vales de Diesel.");

                                }
                                else
                                    //Establcemos Error
                                    resultadotipo = new RetornoOperacion("No Existe el tipo de operación.");
                            }

                            if (resultado.OperacionExitosa)
                            {
                                //Deacuerdo al Tipo de Vale
                                //if (diesel.Field<string>("TipoOperacion") == "Vale (Tractor)" || diesel.Field<string>("TipoOperacion") == "Vale (Remolque)")
                                if (diesel.Field<string>("TipoOperacion") == "Monedero")
                                {

                                    //Validamos Existencia de Costo
                                    if (diesel.Field<decimal>("CostoCombustible") > 0)
                                    {
                                        //Obtenomos Unidad Diesel
                                        int id_unidad_diesel = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(objMovimientoAsignacion.id_movimiento);
                                        //Validamos que Exista Unidad de Diesel
                                        if (id_unidad_diesel > 0)
                                        {
                                            //Insertando Diesel CORREGIR unidad dioesel
                                            resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaAsignacionDieselProgramado(identificador, objServicio.id_compania_emisor, 0, diesel.Field<int>("IdEstacion"),
                                                                                                Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), diesel.Field<int>("IdCosto"), (byte)(SAT_CL.EgresoServicio.CostoCombustible.Estatus)diesel.Field<byte>("IdTipo"), 0, false,
                                                                                                "Diesel Monedero Ruta" + "[" + id_ruta + "]", 0, 0, (byte)AsignacionDiesel.TipoVale.Monedero, diesel.Field<decimal>("litros"),
                                                                                                diesel.Field<decimal>("CostoCombustible"), id_unidad, id_unidad_diesel, id_operador, id_tercero, objServicio.id_servicio,
                                                                                                id_asignacion, id_usuario);

                                            //Validando Operación Exitosa
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obteniendo Asignación
                                                int id_AsignacionDiesel = resultado.IdRegistro;

                                                //Obteniendo Carga Autotanque Actva
                                                using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(diesel.Field<int>("IdEstacion"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                                                {
                                                    //Validando que exista una carga
                                                    if (carga.habilitar)
                                                    {
                                                        //Instanciando Detalle de Liquidación
                                                        using (DetalleLiquidacion dlDiesel = new DetalleLiquidacion(id_AsignacionDiesel, 69))
                                                        {
                                                            //Validando que exista
                                                            if (dlDiesel.habilitar)
                                                            {
                                                                //Insertando Id de Carga como Referencia
                                                                resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "IdCarga", 0, "Carga AutoTanque"),
                                                                            carga.id_carga_autotanque.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                //Operación Exitosa
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Insertando Id de Carga como Referencia
                                                                    resultado = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "SobranteDiesel", 0, "Carga AutoTanque"),
                                                                                carga.sobrante_carga_actual.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                                                                    //Validando Operación Exitosa
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualiza Sobrante de la Carga Actual (Carga actual - litros asignados)
                                                                        resultado = carga.ActualizaSobranteCargaActual(carga.sobrante_carga_actual - dlDiesel.cantidad, id_usuario);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                resultado = new RetornoOperacion("No existe el Vale de Diesel");
                                                        }
                                                    }
                                                }
                                                using (SAT_CL.Despacho.Movimiento objMovimiento = new SAT_CL.Despacho.Movimiento (objMovimientoAsignacion.id_movimiento))
                                                {
                                                    if (objMovimiento.habilitar)
                                                    {
                                                        //INSERTAR METODO QUE INSERTE EN LA TABLA NUEVA 
                                                        resultado = SAT_CL.Ruta.CarteraProveedor.InsertaCarteraProveedor(diesel.Field<int>("IdProveedor"), 1, diesel.Field<int>("Id"), diesel.Field<decimal>("Litros"), diesel.Field<decimal>("CostoCombustible"), diesel.Field<decimal>("Total"),
                                                        resultado.IdRegistro, "Monedero Ruta", objMovimiento.id_servicio, objMovimiento.id_segmento_carga, id_ruta, Fecha.ObtieneFechaEstandarMexicoCentro(),1,0, id_usuario);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                            //Establecemos Error
                                            resultado = new RetornoOperacion("No existe Diesel para Asignación de Unidad");
                                    }
                                    else
                                        //Establcemos Error
                                        resultado = new RetornoOperacion("No Existe el Costo para la Asignación de Vales de Diesel.");

                                }
                                else
                                    //Establcemos Error
                                    resultadotipo = new RetornoOperacion("No Existe el tipo de operación.");
                            }

                        }
                        else
                            //Finalizamos ciclo
                            break;
                    }

                }
                //Validamos Operacion 
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }


            return resultado;
        }
        /// <summary>
        /// Insertamos Casetas ligadas a una Ruta
        /// </summary>
        /// <param name="id_ruta"></param>
        /// <param name="efectivo_casetas"></param>
        /// <param name="id_usuario"></param>
        /// <param name="objMovimiento"></param>
        /// <param name="objServicio"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tercero"></param>
        /// <param name="identificador"></param>
        /// <param name="montoDeposito"></param>
        /// <param name="objConcepto"></param>
        /// <param name="ConceptoRestriccion"></param>
        /// <returns></returns>
        private static RetornoOperacion InsertaCasetasRuta(int id_ruta, bool efectivo_casetas, int id_usuario, SAT_CL.Despacho.Movimiento objMovimiento,
          SAT_CL.Documentacion.Servicio objServicio, int id_operador, int id_unidad, int id_tercero, string identificador, decimal montoDeposito,
          ConceptoDeposito objConcepto, int ConceptoRestriccion)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Insertando depósito
                resultado = EgresoServicio.Deposito.InsertaDeposito(objServicio.id_compania_emisor, identificador, objServicio.id_cliente_receptor,
                objConcepto.id_concepto_deposito, id_ruta, ConceptoRestriccion, Fecha.ObtieneFechaEstandarMexicoCentro(), "", Deposito.TipoCargo.Depositante,
                efectivo_casetas, id_usuario);

                //Asignamos Valor
                int id_deposito = resultado.IdRegistro;

                //Validando que la Operación fue exitosa
                if (resultado.OperacionExitosa)
                {
                    //Insertando Detalle de Liquidación
                    resultado = DetalleLiquidacion.InsertaDetalleLiquidacion(51, resultado.IdRegistro, id_unidad, id_operador, id_tercero,
                                        objServicio.id_servicio, objMovimiento.id_movimiento, 0, 1, 32, montoDeposito, id_usuario);
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciamos Deposito
                        using (Deposito objDeposito = new Deposito(id_deposito))
                        {
                            //Solicitamos Depósito
                            resultado = objDeposito.SolicitarDeposito(ConceptoRestriccion, id_usuario);

                        }
                    }
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Terminamos Transaccion
                    scope.Complete();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRuta()
        {
            //Invoca al método que asigna valores a los atributos de la clase
            return this.cargaAtributos(this._id_ruta);
        }

        
        #endregion


    }
}