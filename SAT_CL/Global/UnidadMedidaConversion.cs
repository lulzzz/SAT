using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas las COnversiones de las Unidades de Medida
    /// </summary>
    public class UnidadMedidaConversion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_unidad_medida_conversion_tuc";

        private int _id_unidad_medida_conversion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Unidad de Medida de Conversión
        /// </summary>
        public int id_unidad_medida_conversion { get { return this._id_unidad_medida_conversion; } }
        private int _id_unidad_original;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad Original
        /// </summary>
        public int id_unidad_original { get { return this._id_unidad_original; } }
        private int _id_unidad_conversion;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad a Convertir
        /// </summary>
        public int id_unidad_conversion { get { return this._id_unidad_conversion; } }
        private string _operacion_conversion;
        /// <summary>
        /// Atributo encargado de almacenar la Operación de Conversión
        /// </summary>
        public string operacion_conversion { get { return this._operacion_conversion; } }
        private decimal _valor_conversion;
        /// <summary>
        /// Atributo encargado de almacenar el Valor de Conversión
        /// </summary>
        public decimal valor_conversion { get { return this._valor_conversion; } }
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
        public UnidadMedidaConversion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public UnidadMedidaConversion(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UnidadMedidaConversion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_unidad_medida_conversion = 0;
            this._id_unidad_original = 0;
            this._id_unidad_conversion = 0;
            this._operacion_conversion = "";
            this._valor_conversion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, "", 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_unidad_medida_conversion = id_registro;
                        this._id_unidad_original = Convert.ToInt32(dr["IdUnidadOriginal"]);
                        this._id_unidad_conversion = Convert.ToInt32(dr["IdUnidadConversion"]);
                        this._operacion_conversion = dr["OperacionConversion"].ToString();
                        this._valor_conversion = Convert.ToDecimal(dr["ValorConversion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encaragdo de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_unidad_original">Unidad Original</param>
        /// <param name="id_unidad_conversion">Unidad de Conversión</param>
        /// <param name="operacion_conversion">Operación de Conversión</param>
        /// <param name="valor_conversion">Valor de Conversión</param>
        /// <param name="id_usuario">id de Usuario</param>
        /// <param name="habilitar">estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_unidad_original, int id_unidad_conversion, string operacion_conversion, 
                                                    decimal valor_conversion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_unidad_medida_conversion, id_unidad_original, id_unidad_conversion, 
                               operacion_conversion, valor_conversion, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Conversiones de Unidades de Medida
        /// </summary>
        /// <param name="id_unidad_original">Unidad Original</param>
        /// <param name="id_unidad_conversion">Unidad de Conversión</param>
        /// <param name="operacion_conversion">Operación de Conversión</param>
        /// <param name="valor_conversion">Valor de Conversión</param>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUnidadMedidaConversion(int id_unidad_original, int id_unidad_conversion, 
                                                string operacion_conversion, decimal valor_conversion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_unidad_original, id_unidad_conversion, 
                               operacion_conversion, valor_conversion, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Conversiones de Unidades de Medida
        /// </summary>
        /// <param name="id_unidad_original">Unidad Original</param>
        /// <param name="id_unidad_conversion">Unidad de Conversión</param>
        /// <param name="operacion_conversion">Operación de Conversión</param>
        /// <param name="valor_conversion">Valor de Conversión</param>
        /// <param name="id_usuario">id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUnidadMedidaConversion(int id_unidad_original, int id_unidad_conversion, 
                                                string operacion_conversion, decimal valor_conversion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_unidad_original, id_unidad_conversion,
                               operacion_conversion, valor_conversion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Conversiones de Unidades de Medida
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DesahbilitaUnidadMedidaConversion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_unidad_original, this._id_unidad_conversion,
                               this._operacion_conversion, this._valor_conversion, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar las Conversiones de Unidades de Medida
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUnidadMedidaConversion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_unidad_medida_conversion);
        }
        /// <summary>
        /// Realiza la conversión de la cantidad indicada, desde la unidad de medida de origen hacia la unidad de medida solicitada.
        /// </summary>        
        /// <param name="id_unidad_original">Id de Unidad de Medida Origen</param>
        /// <param name="id_unidad_conversion">Id de Unidad de Medida Destino</param>
        /// <param name="cantidad">Cantidad a convertir</param>
        /// <returns></returns>
        public static decimal ConvierteCantidadUnidadMedida(int id_unidad_original, int id_unidad_conversion, decimal cantidad)
        { 
            //Declarando objeto de resultado
            decimal resultado = 0;

            //Declarando conjunto de parámetros requeridos
            object[] param = { 4, 0, id_unidad_original, id_unidad_conversion, "", 0, 0, false, "", "" };

            //Realizando carga de datos de conversion
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            { 
                //Si el origen de datos existe
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Recuperando valor de conversion y operador usado
                        switch (r["OperacionConversion"].ToString())
                        {
                            case "*":
                                resultado = cantidad * (Convert.ToDecimal(r["ValorConversion"]) > 0 ? Convert.ToDecimal(r["ValorConversion"]) : 1);
                                break;
                            case "/":
                                resultado = cantidad / (Convert.ToDecimal(r["ValorConversion"]) > 0 ? Convert.ToDecimal(r["ValorConversion"]) : 1);
                                break;
                        }

                        //Terminando ciclo lectura de parámetros conversión
                        break;
                    }
                }
                // Si no hay valor de conversión
                else
                    resultado = cantidad;
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
