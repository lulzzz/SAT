using System;
using TSDK.Base;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase que permite realizar acciones sobre la tabla Facturado Facturación (Inserción, Actualización, Deshabilitar Registros)
    /// </summary>
    public class FacturadoFacturacion : Disposable
    {

        #region Enumeracion

        /// <summary>
        /// Enumera los estados de Facturado Facturación
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Estatus cuando la factura es registrada en el sistema.
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// Estatus cuando la factura tiene un proceso de facturación.
            /// </summary>
            Facturada = 2,
            /// <summary>
            /// Estatus cuando se cancela el proceso de facturación.
            /// </summary>
            Cancelada = 3,
            /// <summary>
            /// 
            /// </summary>
            PendienteCancelacion = 4,
            /// <summary>
            /// 
            /// </summary>
            NotaCredito = 5,
        };

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo privado que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string nom_sp = "facturacion.sp_facturado_facturacion_tff";

        private int _id_facturado_facturacion;
        /// <summary>
        /// Id que permite identificar la relacion entre una factura y una facturacion electrónica
        /// </summary>
        public int id_facturado_facturacion
        {
            get { return _id_facturado_facturacion; }
        }

        private int _id_factura;
        /// <summary>
        /// Id que identifica el registro de una factura
        /// </summary>
        public int id_factura
        {
            get { return _id_factura; }
        }

        private int _id_factura_concepto;
        /// <summary>
        /// Id que identifica el registro por concepto de una factura.
        /// </summary>
        public int id_factura_concepto
        {
            get { return _id_factura_concepto; }
        }

        private int _id_factura_global;
        /// <summary>
        /// Id que permite identificar si la factura pertenece a una agrupación (Factura Global) para facturar.
        /// </summary>
        public int id_factura_global
        {
            get { return _id_factura_global; }
        }

        private int _id_factura_electronica;
        /// <summary>
        /// Id que permite identificar si el registro de una factura contiene una factura electronica.
        /// </summary>
        public int id_factura_electronica
        {
            get { return _id_factura_electronica; }
        }

        private int _id_factura_electronica33;
        /// <summary>
        /// Id que permite identificar si el registro de una factura contiene una factura electronica. (CFDI3.3)
        /// </summary>
        public int id_factura_electronica33
        {
            get { return _id_factura_electronica33; }
        }

        private byte _id_estatus;
        /// <summary>
        /// Permite identificar el estatus de una factura (registrada, Facturada,Cancelada).
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de Estatus de una factura (Registrada, Facturada, Cancelada)
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }

        private bool _habilitar;
        /// <summary>
        /// Corresponde al estado de habilitación de un registro de FacturadoFacturación
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por defaul de la clase que inicializa los atributos
        /// </summary>
        public FacturadoFacturacion()
        {
            this._id_facturado_facturacion =
            this._id_factura =
            this._id_factura_concepto =
            this._id_factura_global =
            this._id_factura_electronica =
            this._id_factura_electronica33 = 0;
            this._id_estatus = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro asignado
        /// </summary>
        /// <param name="id_facturado_facturacion">ID que sirve como referencia para la asignación de un registro a los atributos</param>
        public FacturadoFacturacion(int id_facturado_facturacion)
        {
            //Incova al método cargaAtributoInstancia
            cargaAtributoInstancia(id_facturado_facturacion);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturadoFacturacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite buscar y cargar los registros a los atributos de FacturadoFacturación
        /// </summary>
        /// <param name="id_facturado_facturacion">Id que permite realizar la busqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_facturado_facturacion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y asignación de valores al arreglo, necesarios para el sp de la tabla FacturadoFacturación
            object[] param = { 3, id_facturado_facturacion, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Invoca al SP de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos, que existan y que no sean nulos.
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y almacena en la variable r, el recorrido de las filas.
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        //Asignando Valores
                        this._id_facturado_facturacion = id_facturado_facturacion;
                        this._id_factura = Convert.ToInt32(r["IdFactura"]);
                        this._id_factura_concepto = Convert.ToInt32(r["IdFacturaConcepto"]);
                        this._id_factura_global = Convert.ToInt32(r["IdFacturaGlobal"]);
                        this._id_factura_electronica = Convert.ToInt32(r["IdFacturaElectronica"]);
                        this._id_factura_electronica33 = Convert.ToInt32(r["IdFacturaElectronica33"]);
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor de retorno siempre y cuando se cumpla la sentencia de validacion de datos
                    retorno = true;
                }
            }
            //Retorna el resultado al método
            return retorno;
        }
        /// <summary>
        /// Método que permite Actualizar los campos de un registro que pertenecen a FacturadoFacturación
        /// </summary>
        /// <param name="id_factura">Permite actualizar el identificador de una factura</param>
        /// <param name="id_factura_concepto">Permite actualizar el identificador del concepto de una factura</param>
        /// <param name="id_factura_global">Permite actualizar el identificador que corresponde a una factura global</param>
        /// <param name="id_factura_electronica">Permite actualizar el identificador de una factura electronica</param>
        /// <param name="id_estatus">Permite actualizar el estatus como registrado, facturado o cancelada</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario con el ultimo usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Permite actualizar el campo como habilitado o deshabilitado</param>
        /// <returns></returns>
        private RetornoOperacion editarFacturadoFacturacion(int id_factura, int id_factura_concepto, int id_factura_global, int id_factura_electronica,
                                                            int id_factura_electronica33, Estatus estatus, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y asignación de valores al arerglo necesarios para el sp de la tabla
            object[] param = { 2, this.id_facturado_facturacion, id_factura, id_factura_concepto, id_factura_global,
                              id_factura_electronica, id_factura_electronica33, (byte)estatus, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite realizar inserciones a FacturadoFacturacion
        /// </summary>
        /// <param name="id_factura">Permite realizar la inserción de un identficador de una factura</param>
        /// <param name="id_factura_concepto">Permite realizar la inserción de un identificador del concepto de una factura</param>
        /// <param name="id_factura_global">Permite realizar la inserción de un identificador de la agrupacion de facturas</param>
        /// <param name="id_factura_electronica">Permite realizar la inserción de un identificador de la factura electronica</param>
        /// <param name="id_usuario">Permite identificar el usuario que realizo la inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarFacturadoFacturacion(int id_factura, int id_factura_concepto, int id_factura_global,
                                                            int id_factura_electronica, int id_factura_electronica33, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 1, 0, id_factura, id_factura_concepto, id_factura_global, id_factura_electronica, 
                               id_factura_electronica33, Estatus.Registrada, id_usuario, true, "", "" };

            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Retorno del resultado al método
            return retorno;

        }
        /// <summary>
        /// Método que permite realizar inserciones a FacturadoFacturacion
        /// </summary>
        /// <param name="id_factura">Permite realizar la inserción de un identficador de una factura</param>
        /// <param name="id_factura_concepto">Permite realizar la inserción de un identificador del concepto de una factura</param>
        /// <param name="id_factura_global">Permite realizar la inserción de un identificador de la agrupacion de facturas</param>
        /// <param name="id_factura_electronica">Permite realizar la inserción de un identificador de la factura electronica</param>
        /// <param name="estatus">Permite realizar la inserción de un estatus al registro (Registrada,Facturada,Cancelada)</param>
        /// <param name="id_usuario">Permite identificar el usuario que realizo la inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarFacturadoFacturacion(int id_factura, int id_factura_concepto, int id_factura_global,
                                                int id_factura_electronica, int id_factura_electronica33, Estatus estatus, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 1, 0, id_factura, id_factura_concepto, id_factura_global, id_factura_electronica,
                               id_factura_electronica33, (byte)estatus, id_usuario, true, "", "" };

            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Retorno del resultado al método
            return retorno;

        }
        /// <summary>
        /// Método que permite la actualización de registros de FacturadoFacturación
        /// </summary>
        /// <param name="id_factura">Permite la actualización del campo Id_factura</param>
        /// <param name="id_factura_concepto">Permite la actualiación del capo id_factura_concepto</param>
        /// <param name="id_factura_global">Permite la actualización del campo Id_factura_global</param>
        /// <param name="id_factura_electronica">Permite la actualización del campo id_factura_electronica</param>
        /// <param name="id_estatus">Permite la actualización del campo id_estatus</param>
        /// <param name="id_usuario">Permite la actualización del cmpo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarFacturadoFacturacion(int id_factura, int id_factura_concepto, int id_factura_global, int id_factura_electronica,
                                                           int id_factura_electronica33, Estatus estatus, int id_usuario)
        {
            //Retorna e Invoca el método editarFacturadoFacturación
            return this.editarFacturadoFacturacion(id_factura, id_factura_concepto, id_factura_global, id_factura_electronica,
                                                   id_factura_electronica33, estatus, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <param name="id_usuario">Id que permite identificar al usuario que realizo el cambio de estado (Habilitado/Deshabilitado) de un registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturadoFacturacion(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.editarFacturadoFacturacion(this._id_factura, this._id_factura_concepto, this._id_factura_global, this._id_factura_electronica,
                                                   this._id_factura_electronica33, (Estatus)this._id_estatus, id_usuario, false);
        }
        /// <summary>
        /// Actualizamos Estatus de Facturado Facturación
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusFacturadoFacturacion(Estatus estatus, int id_usuario)
        {
            //Retorna e Invoca el método editarFacturadoFacturación
            return this.editarFacturadoFacturacion(this.id_factura, this.id_factura_concepto, this.id_factura_global, this.id_factura_electronica,
                                                   this._id_factura_electronica33, estatus, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Obtener las relación de las Facturas dada una Factura y una Factura Global
        /// </summary>
        /// <param name="id_factura">Factura Actual</param>
        /// <param name="id_factura_global">Factura Global Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturacionFacturasGlobales(int id_factura, int id_factura_global)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 4, 0, id_factura, 0, id_factura_global, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Totales de la Factura Global
        /// </summary>
        /// <param name="id_factura_global"></param>
        /// <returns></returns>
        public static DataTable ObtieneTotalesFacturaGlobal(int id_factura_global)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 5, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Obtiene la relación de la factura Eectrónica ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneRelacionFactura(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 6, 0, id_factura, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_relacion")).FirstOrDefault();
                }
            }
            return id_relacion;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles Sin Fcatura Electrónica ligada a una Factura Global
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesSinFERegistradosFacturaGlobal(int id_factura_global)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 7, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método encargado de Obtener la moneda de la Factura Global
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaMonedaFacturaGlobal(int id_factura_global)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("Existe mas de una Moneda.");
            //Declarando Objeto de Retorno
            int total_monedas = 0;
            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 8, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    total_monedas = (from DataRow r in ds.Tables["Table"].Rows
                                     select r.Field<int>("Id_moneda")).DefaultIfEmpty().Count();
                }
                //Validamos Exista una Moneda
                if (total_monedas == 1)
                {
                    //Validamos Actualización Correcta
                    resultado = new RetornoOperacion(0);
                }

            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Obtiene la relación de la factura Eectrónica ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneRelacionFacturaGlobal(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 10, 0, id_factura, 0, 0, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_relacion")).FirstOrDefault();
                }
            }
            return id_relacion;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Facturas y/ó Conceptos Ligados a una Factura Global
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesFacturacionFacturaGlobal(int id_factura_global)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 12, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }

        #region Metodos CFDIv3.2

        /// <summary>
        /// Obtiene la relación de la factura Eectrónica ligando el Id factura
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static int ObtieneFacturaElectronicaFacturaGlobal(int id_factura_global)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 13, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "3.2", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();
                }
            }
            return id_relacion;
        }
        /// <summary>
        /// Obtiene Relación ligando una Factura Electrónica
        /// </summary>
        /// <param name="id_factura_electronica">Id Factura Electronica</param>
        /// <returns></returns>
        public static int ObtieneRelacionFacturaElectronica(int id_factura_electronica)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 9, 0, 0, 0, 0, id_factura_electronica, 0, 0, 0, true, "3.2", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_relacion")).FirstOrDefault();
                }
            }
            //Devolviendo Resultado Obtenido
            return id_relacion;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles  ligando una factura electronica
        /// </summary>
        /// <param name="id_factura_electronica">Factura Electrónica</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesFacturaGlobal(int id_factura_electronica)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 11, 0, 0, 0, 0, id_factura_electronica, 0, 0, 0, true, "3.2", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles  ligando una factura electronica
        /// </summary>
        /// <param name="id_factura_electronica">Factura Electrónica</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesFacturaGlobalV3_3(int id_factura_electronica33)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacFacturacion = null;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 11, 0, 0, 0, 0, 0, id_factura_electronica33, 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valor Obtenido
                    dtFacFacturacion = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacFacturacion;
        }
        /// <summary>
        /// Obtiene la factura Electrónica cancelada ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneFacturacionElectronicaCancelada(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 15, 0, id_factura, 0, 0, 0, 0, 0, 0, true, "3.2", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_factura_electronica")).FirstOrDefault();
                }
            }
            //Devolviendo Resultado
            return id_relacion;
        }
        /// <summary>
        /// Obtiene la factura Electrónica cancelada ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneFacturacionElectronicaActiva(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 16, 0, id_factura, 0, 0, 0, 0, 0, 0, true, "3.2", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_factura_electronica")).FirstOrDefault();
                }
            }
            return id_relacion;
        }

        #endregion

        #region Metodos CFDIv3.3

        /// <summary>
        /// Obtiene la relación de la factura Eectrónica ligando el Id factura
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static int ObtieneFacturaElectronicaFacturaGlobalV3_3(int id_factura_global)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 13, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();
                }
            }
            return id_relacion;
        }
        /// <summary>
        /// Obtiene la relación de la Factura Electrónica ligando el Id de la Factura Global
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <returns></returns>
        public static int ObtieneFacturaElectronicaFacturaGlobalCanceladaV3_3(int id_factura_global)
        {
            //Declarando variable
            int id_factura_electronica33 = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 17, 0, 0, 0, id_factura_global, 0, 0, 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_factura_electronica33 = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();
                }
            }
            return id_factura_electronica33;
        }
        /// <summary>
        /// Obtiene Relación ligando una Factura Electrónica
        /// </summary>
        /// <param name="id_factura_electronica">Id Factura Electronica</param>
        /// <returns></returns>
        public static int ObtieneRelacionFacturaElectronicav3_3(int id_factura_electronica)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 9, 0, 0, 0, 0, 0, id_factura_electronica, 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_relacion")).FirstOrDefault();
            }

            //Devolviendo Resultado Obtenido
            return id_relacion;
        }
        /// <summary>
        /// Obtiene la factura Electrónica cancelada ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneFacturacionElectronicaCanceladaV3_3(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 15, 0, id_factura, 0, 0, 0, 0, 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_factura_electronica")).FirstOrDefault();
                }
            }
            //Devolviendo Resultado
            return id_relacion;
        }
        /// <summary>
        /// Obtiene la factura Electrónica cancelada ligando el Id factura
        /// </summary>
        /// <param name="id_factura">Id Factura</param>
        /// <returns></returns>
        public static int ObtieneFacturacionElectronicaActivaV3_3(int id_factura)
        {
            //Declarando variable
            int id_relacion = 0;

            //Creación y asignación de valores al arreglo necesarios para el sp
            object[] param = { 16, 0, id_factura, 0, 0, 0, 0 , 0, 0, true, "3.3", "" };

            //Obteniendo Resultado de DB
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                {
                    //Obtenemos la relación
                    id_relacion = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id_factura_electronica")).FirstOrDefault();
                }
            }
            return id_relacion;
        }

        #endregion

        #endregion
    }
}


