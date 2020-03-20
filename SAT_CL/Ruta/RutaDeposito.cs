using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que permite realizar acciones sobre los registros de ruta deposito
    /// </summary>
    public class RutaDeposito : Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumeración de los tipo de monto de un deposito
        /// </summary>
        public enum TipoMonto
        {
            /// <summary>
            /// Cuando el monto del depósito es Fijo
            /// </summary>
            Fijo = 1,
            /// <summary>
            /// Cuando el monto del depósito es Variable
            /// </summary>
            Variable
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla Ruta Depósito
        /// </summary>
        private static string nom_sp = "ruta.sp_ruta_deposito_trd";
        private int _id_ruta_deposito;
        /// <summary>
        /// Identificador de un depósito generado a una ruta
        /// </summary>
        public int id_ruta_deposito
        {
            get { return _id_ruta_deposito; }
        }
        private int _id_ruta;
        /// <summary>
        /// Identifica a una ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private int _id_concepto;
        /// <summary>
        /// Identifica el concepto de un depósito (Descripción por la cual se realizá el depósito)
        /// </summary>
        public int id_concepto
        {
            get { return _id_concepto; }
        }
        private int _id_concepto_restriccion;
        /// <summary>
        /// Identifica las restricciones que puede tener un concepto
        /// </summary>
        public int id_concepto_restriccion
        {
            get { return _id_concepto_restriccion; }
        }
        private byte _id_tipo_monto;
        /// <summary>
        /// Identifica el tipo de mondo de un depósito (Fijo o Variable)
        /// </summary>
        public byte id_tipo_monto
        {
            get { return _id_tipo_monto; }
        }
        /// <summary>
        /// Permite tener acceso a la enumeración de los tipo de monto de un depósito
        /// </summary>
        public TipoMonto tipoMonto
        {
            get { return (TipoMonto)this._id_tipo_monto; }
        }
        private decimal _monto;
        /// <summary>
        /// Valor monetario por el cual se realizó el depósito
        /// </summary>
        public decimal Monto
        {
            get { return _monto; }
        }
        private bool _bit_comprobacion;
        /// <summary>
        /// Define si podra realizar la comprobación
        /// </summary>
        public bool bit_comprobacion
        {
            get { return _bit_comprobacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define la disponiblidad de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos a cero
        /// </summary>
        public RutaDeposito()
        {
            this._id_ruta_deposito = 0;
            this._id_ruta = 0;
            this._id_concepto = 0;
            this._id_concepto_restriccion = 0;
            this._id_tipo_monto = 0;
            this._monto = 0;
            this._bit_comprobacion = false;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de ruta depósito
        /// </summary>
        /// <param name="id_ruta_deposito">Identificador que sirve como referencia para inicializar los atributos</param>
        public RutaDeposito(int id_ruta_deposito)
        {
            //Invoca al método que realiza la asignación de valores a los atributos
            cargaAtributos(id_ruta_deposito);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~RutaDeposito()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro y asigna a los atributos el resultado.
        /// </summary>
        /// <param name="id_ruta_deposito">Identificador de un regsitro de Ruta Depósito a buscar</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ruta_deposito)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos para realizar la consulta de una ruta depósito
            object[] param = { 3, id_ruta_deposito, 0, 0, 0, 0, 0, false, 0, false, "", "" };
            //Realiza la busqueda del registro y lo almacena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos los datos de las filas del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ruta_deposito = id_ruta_deposito;
                        this._id_ruta = Convert.ToInt32(r["IdRuta"]);
                        this._id_concepto = Convert.ToInt32(r["IdConcepto"]);
                        this._id_concepto_restriccion = Convert.ToInt32(r["IdConceptoRestriccion"]);
                        this._id_tipo_monto = Convert.ToByte(r["IdTipoMonto"]);
                        this._monto = Convert.ToDecimal(r["Monto"]);
                        this._bit_comprobacion = Convert.ToBoolean(r["BitComprobacion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Regresa al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que realiza actualizaciones sobre un registro de ruta depósito
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_concepto">Actualiza el identificador de un concepto por el cual se realizó el depósito</param>
        /// <param name="id_concepto_restriccion">Actualiza el identificador de las restricciones que pueda tener un concepto de depósito</param>
        /// <param name="tipoMonto">Actualiza el tipo de monto de un depósito (Fijo o Variable) </param>
        /// <param name="monto">Actualiza la cantidad monetaria por la cual se realizó el depósito</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso de un registro de ruta depósito</param>
        /// <returns></returns>
        private RetornoOperacion editarRutaDeposito(int id_ruta, int id_concepto_restriccion, TipoMonto tipoMonto, decimal monto, bool bit_comprobacion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Instancia a la clase Concepto Restricción
            using (SAT_CL.EgresoServicio.ConceptoRestriccion cr = new SAT_CL.EgresoServicio.ConceptoRestriccion(id_concepto_restriccion))
            {
                //Valida los montos 
                if (cr.minimo_monto < monto && cr.maximo_monto > monto)
                {
                    //Creación del arreglo que almacena los datos necesarios para realizar la acutilizacion de un registro
                    object[] param = { 2, this._id_ruta_deposito, id_ruta, cr.id_concepto, id_concepto_restriccion, (TipoMonto)tipoMonto, monto, bit_comprobacion, id_usuario, habilitar, "", "" };
                    //Realiza la actualización del registro ruta deposito
                    retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
                }
                // Si el monto minimo es mayor al monto
                else if (cr.minimo_monto > monto)
                {
                    //Envia un mensaje de error de actualización a incumplimiento de los montos
                    retorno = new RetornoOperacion("El monto: " + monto + " no cumple con el monto mínimo: " + cr.minimo_monto + " del concepto de depósito ");
                }
                //Si el monto maximo es menor al monto 
                else if (cr.maximo_monto < monto)
                {
                    //Envia un mensaje de error de actualización a incumplimiento de los montos
                    retorno = new RetornoOperacion("El monto: " + monto + " no cumple con el monto máximo : " + cr.maximo_monto + " del concepto de depósito ");
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que realiza inserción sobre un registro de ruta deposito
        /// </summary>
        /// <param name="id_ruta">Inserta el identificador de una ruta</param>
        /// <param name="id_concepto">Inserta el identificador de un concepto por el cual se realizo el depósito</param>
        /// <param name="id_concepto_restriccion">Inserta el identificador de las restricciones que pueda tener un concepto de depósito</param>
        /// <param name="tipoMonto">Inserta el tipo de monto de un deposito (Fijo o Variable) </param>
        /// <param name="monto">Inserta la cantidad monetario por la cual se realizó el depósito</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó acciones sobre el registro</param>
        /// <param name="habilitar">Inserta el estado de uso de un registro de ruta depósito</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarRutaDeposito(int id_ruta, int id_concepto_restriccion, TipoMonto tipoMonto, decimal monto, bool bit_comprobacion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //instancia a la clase concepto restricción
            using (SAT_CL.EgresoServicio.ConceptoRestriccion cr = new SAT_CL.EgresoServicio.ConceptoRestriccion(id_concepto_restriccion))
            {
                //Valida los montos (que el monto sea mayor al monto mínimo y menor al monto máximo)
                if (cr.minimo_monto < monto && cr.maximo_monto > monto)
                {
                    //Creación del arreglo que almacena los datos necesarios para realizar la inserción de un registro
                    object[] param = { 1, 0, id_ruta, cr.id_concepto, id_concepto_restriccion, (TipoMonto)tipoMonto, monto, bit_comprobacion, id_usuario, true, "", "" };
                    //Realiza la inserción del registro ruta depósito
                    retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
                }
                // Si el monto minimo es mayor al monto
                else if (cr.minimo_monto > monto)
                {
                    //Envia un mensaje de error de actualización a incumplimiento de los montos
                    retorno = new RetornoOperacion("El monto: " + monto + " no cumple con el monto mínimo: " + cr.minimo_monto + " del concepto de depósito ");
                }
                    //Si el monto maximo es menor al monto 
                else if (cr.maximo_monto < monto)
                {
                    //Envia un mensaje de error de actualización a incumplimiento de los montos
                    retorno = new RetornoOperacion("El monto: " + monto + " no cumple con el monto máximo : " + cr.maximo_monto + " del concepto de depósito ");
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que realiza actualizaciones sobre un registro de ruta deposito
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_concepto">Actualiza el identificador d eun concepto por el cual se realizo el depósito</param>
        /// <param name="id_concepto_restriccion">Actualiza el identificador de las restricciones que pueda tener un concepto de depósito</param>
        /// <param name="tipoMonto">Actualiza el tipo de monto de un depósito (Fijo o Variable) </param>
        /// <param name="monto">Actualiza la cantidad monetario por la cual se realizó el depósito</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarRutaDeposito(int id_ruta, int id_concepto_restriccion, TipoMonto tipoMonto, decimal monto, bool bit_comprobacion, int id_usuario)
        {
            //Retorna al método el resultado del método que reaiza la actualización del registro
            return this.editarRutaDeposito(id_ruta, id_concepto_restriccion, (TipoMonto)tipoMonto, monto, bit_comprobacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la acción de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRutaDeposito(int id_usuario)
        {
            //Retorna al método el resultado del método que realiza la actualización del registro
            return this.editarRutaDeposito(this._id_ruta, this._id_concepto_restriccion, (TipoMonto)this._id_tipo_monto, this._monto, this._bit_comprobacion, id_usuario, false);
        }
        /// <summary>
        /// Método que obtiene los conceptos de deposito de una ruta 
        /// </summary>
        /// <param name="id_ruta">Identifica la ruta a la cual pertenecen los conceptos de deposito</param>
        /// <returns></returns>
        public static DataTable ObtieneConceptoDeposito(int id_ruta)
        {
            //Creación del objeto retorno
            DataTable dtConcepto = new DataTable();
            //Creación del arreglo que almacena los datos necesarios para obtener los conceptos de depósito
            object[] param = { 4, 0, id_ruta, 0, 0, 0, 0, false, 0, false, "", "" };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asignando Resultado Obtenido
                    dtConcepto = DS.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtConcepto;
        }

        /// <summary>
        /// Método encargado de Cargar los depósitos validos para una Ruta de acuerdo a un Segmento
        /// [OBSOLETE]
        /// </summary>
        /// <param name="id_ruta">Id Ruta Coincidente</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaDepositosRutaSegmento(int id_ruta, int id_servicio)
        {
            //Creación del objeto retorno
            DataTable dtConcepto = new DataTable();
            //Creación del arreglo que almacena los datos necesarios para obtener los conceptos de depósito
            object[] param = { 5, 0, 0, 0, 0, 0, 0, false, 0, false, id_servicio, id_ruta };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asignando Resultado Obtenido
                    dtConcepto = DS.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtConcepto;
        }
        /// <summary>
        /// Método encargado de Cargar los depósitos validos para una Ruta de acuerdo a un Segmento
        /// </summary>
        /// <param name="id_ruta">Id Ruta Coincidente</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaDepositosRutaSegmento(string id_ruta, int id_servicio)
        {
            //Creación del objeto retorno
            DataTable dtConcepto = new DataTable();

            //Creación del arreglo que almacena los datos necesarios para obtener los conceptos de depósito
            object[] param = { 5, 0, 0, 0, 0, 0, 0, false, 0, false, id_servicio, id_ruta };
            //Almacena el el dataset DS el resultado de invocar el método EjecutaProcAlmacenadoDataSet().
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset contenga datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asignando Resultado Obtenido
                    dtConcepto = DS.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtConcepto;
        }
        #endregion

    }
}
