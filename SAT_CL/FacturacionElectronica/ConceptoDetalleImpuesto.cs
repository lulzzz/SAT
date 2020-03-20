using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    public class ConceptoDetalleImpuesto : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_concepto_detalle_impuesto_tcd";

        private int _id_concepto_detalle_impuesto;
        /// <summary>
        /// Atributo Encargado de almacenar el Id
        /// </summary>
        public int id_concepto_detalle_impuesto
        {
            get { return this._id_concepto_detalle_impuesto; }
        }

        private int _id_concepto;
        /// <summary>
        /// Atributo Encargado de almacenar el Id de Concepto
        /// </summary>
        public int id_concepto
        {
            get { return this._id_concepto; }
        }

        private int _id_detalle_impuesto;
        /// <summary>
        /// Atributo Encargado de almacenar el Id de Detalle de Impuesto
        /// </summary>
        public int id_detalle_impuesto
        {
            get { return this._id_detalle_impuesto; }
        }

        private decimal _importe_moneda_captura;
        /// <summary>
        /// Atributo Encargado de almacenar el Importe de Moneda Captura
        /// </summary>
        public decimal importe_moneda_captura
        {
            get { return this._importe_moneda_captura; }
        }

        private decimal _importe_moneda_nacional;
        /// <summary>
        /// Atributo Encargado de almacenar el Importe de Moneda Nacional
        /// </summary>
        public decimal importe_moneda_nacional
        {
            get { return this._importe_moneda_nacional; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo Encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la Clase que carga los valores por default
        /// </summary>
        public ConceptoDetalleImpuesto()
        {
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase que carga los valores en base a un ID
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ConceptoDetalleImpuesto(int id_registro)
        {
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        ~ConceptoDetalleImpuesto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado encargado de cargar lo Atributos por default
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Carga de Valores
            this._id_concepto_detalle_impuesto = 0;
            this._id_concepto = 0;
            this._id_detalle_impuesto = 0;
            this._importe_moneda_captura = 0;
            this._importe_moneda_nacional = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de cargar lo Atributos en base a un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Carga de Valores
            bool result = false;
            //Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, false, "", "" };
            //Cargando DataSet con datos en base a registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Valida origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {   //Registro por cada fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Cargar Valores
                        this._id_concepto_detalle_impuesto = id_registro;
                        this._id_concepto = Convert.ToInt32(dr["IdConcepto"]);
                        this._id_detalle_impuesto = Convert.ToInt32(dr["IdDetalleImpuesto"]);
                        this._importe_moneda_captura = Convert.ToDecimal(dr["ImporteMonedaCaptura"]);
                        this._importe_moneda_nacional = Convert.ToDecimal(dr["ImporteMonedaNacional"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }

            return result;
        }
        /// <summary>
        /// Método Privado encargado de Editar los Valores por Id
        /// </summary>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="id_detalle_impuesto">Id de Detalle de Impueso</param>
        /// <param name="importe_moneda_captura">Importe Moneda Captura</param>
        /// <param name="importe_moneda_nacional">Importe Moneda Nacional</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaConceptoDetalleImpuesto(int id_concepto, int id_detalle_impuesto,
            decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario, bool habilitar)
        {   //Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Parametros
            object[] param = { 2, this._id_concepto_detalle_impuesto, id_concepto, id_detalle_impuesto, 
                                 importe_moneda_captura, importe_moneda_nacional, id_usuario, habilitar, 
                                 "", "" };
            //Regresando el valor obtenido
            return result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método Público encargdo de Actualizar los Atributos por Id
        /// </summary>
        /// <param name="id_ConceptoDetalleImpuesto">Id regisro</param>
        /// <returns></returns>
        public bool ActualizaConceptoDetalleImpuesto(int id_ConceptoDetalleImpuesto)
        {
            return this.cargaAtributosInstancia(id_ConceptoDetalleImpuesto);
        }
        /// <summary>
        /// Método encargado de Insertar ConceptoDetalleImpuesto
        /// </summary>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="id_detalle_impuesto">Id de Detalle de Impuesto</param>
        /// <param name="importe_moneda_captura">Importe Moneda Captura</param>
        /// <param name="importe_moneda_nacional">Importe Moneda Nacional</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarConceptoDetalleImpuesto(int id_concepto,
            int id_detalle_impuesto, decimal importe_moneda_captura, decimal importe_moneda_nacional,
            int id_usuario)
        {   //Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Parametros
            object[] param = { 1, null, id_concepto, id_detalle_impuesto, 
                                 importe_moneda_captura, importe_moneda_nacional, id_usuario, true, 
                                 "", "" };
            //Regresando el valor obtenido
            return result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
        }

        /// <summary>
        /// Método Público encargado de Editar los Atributos en base a un Id
        /// </summary>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="id_detalle_impuesto">Id de Detalle de Impuesto</param>
        /// <param name="importe_moneda_captura">Importe Moneda Captura</param>
        /// <param name="importe_moneda_nacional">Importe Moneda Nacional</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoDetalleImpuesto(int id_concepto, int id_detalle_impuesto,
            decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario)
        {
            return this.editaConceptoDetalleImpuesto(id_concepto, id_detalle_impuesto,
                importe_moneda_captura, importe_moneda_nacional, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Encargado de Deshabilitar los ConceptoDetalleImpuesto
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConceptoDetalleImpuesto(int id_usuario)
        {
            return this.editaConceptoDetalleImpuesto(this._id_concepto, this._id_detalle_impuesto,
                this._importe_moneda_captura, this._importe_moneda_nacional, id_usuario, false);
        }


        /// <summary>
        /// Recupera Conceptos
        /// </summary>
        /// <param name="id_detalle_impuesto"></param>
        /// <returns></returns>
        public static DataTable RecuperaConceptosDetalles(int id_detalle_impuesto)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            object[] param = { 4, 0, 0, id_detalle_impuesto, 0, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }


        /// <summary>
        /// Recupera los Conceptos ligado a un detalle impuesto 
        /// </summary>
        /// <param name="id_detalle_impuesto"></param>
        /// <returns></returns>
        public static DataTable RecuperaConceptosDetallesImpuesto(int id_detalle_impuesto)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            object[] param = { 5, 0, 0, id_detalle_impuesto, 0, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }



        /// <summary>
        /// Recupera Detalles de Impuestos ligado a un concepto
        /// </summary>
        /// <param name="id_concepto"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public static DataTable RecuperaImpuestosConcepto(int id_concepto)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            object[] param = { 6, 0, id_concepto, 0, 0, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        #endregion
    }
}