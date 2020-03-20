using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de almacenar los descuentos de comprobantes
    /// </summary>
    public class Descuento : Disposable
    {
        #region Atributos

        /// <summary>
        /// atributo que almacena el nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_descuento_tdo";

        private int _id_descuento;
        /// <summary>
        /// Atributo encargado de obtener identificador de descuento 
        /// </summary>
        public int id_descuento { get { return this._id_descuento; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de obtener el identificador de comprobante
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private decimal _porcentaje;
        /// <summary>
        /// Atributo encargado de obtener el porcentaje del descuento 
        /// </summary>
        public decimal porcentaje { get { return this._porcentaje; } }
        private decimal _cantidad_moneda_captura;
        /// <summary>
        /// Atributo encargado de obtener la cantidad capturada de la moneda  del comprobante 
        /// </summary>
        public decimal cantidad_moneda_captura { get { return this._cantidad_moneda_captura; } }
        private decimal _cantidad_moneda_nacional;
        /// <summary>
        /// Atributo encargado de obtener la cantidad del comprobante en moneda nacional 
        /// </summary>
        public decimal cantidad_moneda_nacional { get { return this._cantidad_moneda_nacional; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargadode definir si el registro se encuentra activo o inactivo 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Contructores

        /// <summary>
        /// Contrucctor que unicializa los valores por defecto 
        /// </summary>
        public Descuento()
        {
            //asignando valores 
            this._id_descuento =
            this._id_comprobante = 0;
            this._porcentaje =
            this._cantidad_moneda_captura =
            this._cantidad_moneda_nacional = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que Inicializa los Valores dado un descuento
        /// </summary>
        /// <param name="id_descuento">Identificador Descuento</param>
        public Descuento(int id_descuento)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_descuento);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Descuento()
        {
            Dispose(false);
        }

        #endregion

        #region Métododos Privados
        /// <summary>
        ///  Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_descuento">Identificador Descuento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_descuento)
        {
            //Declarando objeto de retorno 
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_descuento, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_descuento = id_descuento;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]); 
                        this._porcentaje = Convert.ToDecimal(dr["Porcentaje"]);
                        this._cantidad_moneda_captura = Convert.ToDecimal(dr["CantidadMonedaCaptura"]);
                        this._cantidad_moneda_nacional = Convert.ToDecimal(dr["CantidadMonedaNacional"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

                        //Terminando Ciclo
                        break;
                    }

                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        ///  Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_descuento">Identificador de descuento</param>
        /// <param name="id_comprobante">Identificador del comprobante </param>
        /// <param name="porcentaje">Porcentaje del descuento</param>
        /// <param name="cantidad_moneda_capturada">Cantidad capturada de la moneda del comprobante</param>
        /// <param name="cantidad_moneda_nacional">Cantidad en moneda nacional del comprobante</param>
        /// <param name="id_usuario">Identificador del usuario que actualiza registro</param>
        /// <param name="habilitar">Identifica regsitro activo inactivo</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_comprobante, decimal porcentaje, 
                                                      decimal cantidad_moneda_capturada, decimal cantidad_moneda_nacional, 
                                                      int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_descuento, id_comprobante, porcentaje, cantidad_moneda_captura, cantidad_moneda_nacional, 
                               id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Formas de Pago
        /// </summary>
        /// <param name="id_comprobante">IdComprobante</param>
        /// <param name="porcentaje">Porcentaje de descuento</param>
        /// <param name="cantidad_moneda_captura">Cantidad capturada de moneda del comprobante</param>
        /// <param name="cantidad_moneda_nacional">Cantidad de comprobante en moneda nacional</param>
        /// <param name="id_usuario">Identificador del usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDescuento(int id_comprobante, decimal porcentaje, decimal cantidad_moneda_captura, decimal cantidad_moneda_nacional, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobante, porcentaje, cantidad_moneda_captura, cantidad_moneda_nacional, id_usuario, true, "", "" };

             //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        
        }

        /// <summary>
        /// Método encargado de Editar las Formas de Pago
        /// </summary>
        /// <param name="id_comprobante">Identificador de comprobante</param>
        /// <param name="porcentaje">Porcentaje del descuento</param>
        /// <param name="cantidad_moneda_captura">Cantidad capturada con moneda del comrpobante</param>
        /// <param name="cantidad_moneda_nacional">Cantidad de del comprobante expresada en moneda nacional</param>
        /// <param name="id_usuario">Usuario que actualiza registro</param>
        /// <returns></returns>
        public  RetornoOperacion EditaDescuento(int id_comprobante,decimal porcentaje,decimal cantidad_moneda_captura,
                                               decimal cantidad_moneda_nacional,int id_usuario)
        {

            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_comprobante, porcentaje, cantidad_moneda_captura, cantidad_moneda_nacional, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Deshabilitar los descuentos sde comprobantes
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDescuento(int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_comprobante, this._porcentaje, this._cantidad_moneda_captura, this._cantidad_moneda_nacional, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de Actualizar los descuentos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDescuento()
        {
            //Devolviendo resultado Obtenido
            return this.cargaAtributosInstancia(this._id_descuento);
        }
        
        #endregion
    
    }

}
