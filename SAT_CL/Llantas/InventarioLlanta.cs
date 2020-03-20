using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Llantas
{
    /// <summary>
    /// Clase que Consulta, Actualiza e Inserta un registros de Inventario Llanta
    /// </summary>
    public class InventarioLlanta : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure de inventario llanta
        /// </summary>
        private static string nom_sp = "llantas.sp_inventario_llanta_till";
        private int _id_inventario_llanta;
        /// <summary>
        /// Identifica el registro de inventario llanta.
        /// </summary>
        public int id_inventario_llanta
        {
            get { return _id_inventario_llanta; }
        }
        private int _consecutivo;
        /// <summary>
        /// Número consecutivo de inventario llanta por cada registro de inventario
        /// </summary>
        public int consecutivo
        {
            get { return _consecutivo; }
        }
        private int _id_inventario;
        /// <summary>
        /// Identifica a un inventario
        /// </summary>
        public int id_inventario
        {
            get { return _id_inventario; }
        }
        private int _id_actividad;
        /// <summary>
        /// Identifica la actividad de montaje o desmontaje de una llanta
        /// </summary>
        public int id_actividad
        {
            get { return _id_actividad; }
        }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Fecha de inicio de renovación de llanta
        /// </summary>
        public DateTime fecha_inicio
        {
            get { return _fecha_inicio; }
        }
        private DateTime _fecha_fin;
        /// <summary>
        /// Fecha de fin de una renovación de llanta o cuando la llanta se llevara a desecho
        /// </summary>
        public DateTime fecha_fin
        {
            get { return _fecha_fin; }
        }
        private DateTime _fecha_compra;
        /// <summary>
        /// Fecha de compra de la llanta
        /// </summary>
        public DateTime fecha_compra
        {
            get { return _fecha_compra; }
        }
        private int _mm_inicio;
        /// <summary>
        /// Milimetros  de inicio de medida de la llanta 
        /// </summary>
        public int mm_inicio
        {
            get { return _mm_inicio; }
        }
        private int _id_proveedor;
        /// <summary>
        /// Identifica al proveedor de la venta de llanta 
        /// </summary>
        public int id_proveedor
        {
            get { return _id_proveedor; }
        }
        private int _id_factura;
        /// <summary>
        /// Identifca una factura ligada a la compra de la llanta
        /// </summary>
        public int id_factura
        {
            get { return _id_factura; }
        }
        private decimal _costo;
        /// <summary>
        /// Precio de la llanta
        /// </summary>
        public decimal costo
        {
            get { return _costo; }
        }
        private decimal _kms;
        /// <summary>
        /// Kilometros recorridos de la llanta
        /// </summary>
        public decimal kms
        {
            get { return _kms; }
        }
        private DateTime _fecha_garantia;
        /// <summary>
        /// Fecha de fin de garantia de una llanta
        /// </summary>
        public DateTime fecha_garantia
        {
            get { return _fecha_garantia; }
        }
        private decimal _kms_garantia;
        /// <summary>
        /// Kilometros promedio de recorrido de una llanta
        /// </summary>
        public decimal kms_garantia
        {
            get { return _kms_garantia; }
        }
        private bool _habilitar;
        /// <summary>
        /// Estado de uso de un registro (Habilitado-Disponible / Deshabilitado-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Contructores
        /// <summary>
        /// Inicializa los valores de los atributos a cero
        /// </summary>
        public InventarioLlanta()
        {
            this._consecutivo = 0;
            this._id_inventario = 0;
            this._id_actividad = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._fecha_compra = DateTime.MinValue;
            this._mm_inicio = 0;
            this._id_proveedor = 0;
            this._id_factura = 0;
            this._costo = 0;
            this._kms = 0;
            this._fecha_garantia = DateTime.MinValue;
            this._kms_garantia = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Cosntructor que inicializa los atributos a partir de un registro de llanta inventario
        /// </summary>
        /// <param name="id_inventario_llanta"></param>
        public InventarioLlanta(int id_inventario_llanta)
        {
            //Invoca al método que realiza la busqueda de un registro de llanta inventario
            cargaAtributos(id_inventario_llanta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~InventarioLlanta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda y asignación de inventario Llanta
        /// </summary>
        /// <param name="id_inventario_llanta">Identifica el registro de InventarioLlanta</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_inventario_llanta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de inventario llanta
            object[] param = { 3, id_inventario_llanta, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, null, 0, 0, false, "", "" };
            //Invoca y asigna al dataset el método que realiza la busqueda de un registro de Inventario Llanta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos asignados al dataset(que no sean nulos)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Si existen datos en el dataset, recorre la fila y cada campo del registro lo alamcena en los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_inventario_llanta = id_inventario_llanta;
                        this._consecutivo = Convert.ToInt32(r["Consecutivo"]);
                        this._id_inventario = Convert.ToInt32(r["IdInventario"]);
                        this._id_actividad = Convert.ToInt32(r["IdActividad"]);
                        this._fecha_inicio = Convert.ToDateTime(r["FechaInicio"]);
                        this._fecha_fin = Convert.ToDateTime(r["FechaFin"]);
                        this._fecha_compra = Convert.ToDateTime(r["FechaCompra"]);
                        this._mm_inicio = Convert.ToInt32(r["MMInicio"]);
                        this._id_proveedor = Convert.ToInt32(r["IdProveedor"]);
                        this._id_factura = Convert.ToInt32(r["IdFactura"]);
                        this._costo = Convert.ToDecimal(r["Costo"]);
                        this._kms = Convert.ToDecimal(r["Kms"]);
                        this._fecha_garantia = Convert.ToDateTime(r["FechaGarantia"]);
                        this._kms_garantia = Convert.ToInt32(r["KmsGarantia"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actaliza un registro de inventario llanta
        /// </summary>
        /// <param name="id_inventario">Actualiza el identificador de un inventario</param>
        /// <param name="id_actividad">Actualiza una actividad de llanta</param>
        /// <param name="fecha_inicio">Actualiza la fecha de unicio cuando es una renovación</param>
        /// <param name="fecha_fin">Actualiza la fecha de fin de la renovación o cuando una llanta va a desecho</param>
        /// <param name="fecha_compra">Actualiza la fecha de compra de la llanta</param>
        /// <param name="mm_inicio">Actualiza los milimetros de inicio de medidad de la llanta</param>
        /// <param name="id_proveedor">Actualiza el proveedor de la llanta</param>
        /// <param name="id_factura">Actualiza la factura de compra de la llanta</param>
        /// <param name="costo">Actualiza el costo de la llanta</param>
        /// <param name="kms">Actualiza los kilometros recorridos de una llanta </param>
        /// <param name="fecha_garantia">Actualiza la fecha de garantia de la llanta</param>
        /// <param name="kms_garantia">Actualiza los kilometros de recorrido de garantia de la llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro de Inventario Llanta</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro de inventario de llanta (Habilitado-Disponible / Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarInventarioLlanta(int id_inventario, int id_actividad, DateTime fecha_inicio, DateTime fecha_fin, DateTime fecha_compra, int mm_inicio, int id_proveedor, int id_factura,
                                                        decimal costo, decimal kms, DateTime fecha_garantia, decimal kms_garantia, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar un registro de inventario llanta
            object[] param = { 2, this._id_inventario_llanta, this._consecutivo, id_inventario, id_actividad, fecha_inicio, fecha_fin, fecha_compra, mm_inicio, id_proveedor, id_factura, costo, kms, fecha_garantia, kms_garantia, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el método que realiza la actualización de un registro de inventario Llanta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro de inventario llanta
        /// </summary>
        /// <param name="id_inventario">Inserta el identificador de un inventario</param>
        /// <param name="id_actividad">Inserta una actividad de llanta</param>
        /// <param name="fecha_inicio">Inserta la fecha de unicio cuando es una renovación</param>
        /// <param name="fecha_fin">Inserta la fecha de fin de la renovación o cuando una llanta va a desecho</param>
        /// <param name="fecha_compra">Inserta la fecha de compra de la llanta</param>
        /// <param name="mm_inicio">Inserta los milimetros de inicio de medidad de la llanta</param>
        /// <param name="id_proveedor">Inserta el proveedor de la llanta</param>
        /// <param name="id_factura">Inserta la factura de compra de la llanta</param>
        /// <param name="costo">Inserta el costo de la llanta</param>
        /// <param name="kms">Inserta los kilometros recorridos de una llanta </param>
        /// <param name="fecha_garantia">Inserta la fecha de garantia de la llanta</param>
        /// <param name="kms_garantia">Inserta los kilometros de recorrido de garantia de la llanta</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la actualización del registro de Inventario Llanta</param>        
        /// <returns></returns>
        public static RetornoOperacion InsertarInventarioLlanta(int id_inventario, int id_actividad, DateTime fecha_inicio, DateTime fecha_fin, DateTime fecha_compra, int mm_inicio, int id_proveedor, int id_factura,
                                                        decimal costo, decimal kms, DateTime fecha_garantia, decimal kms_garantia, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar un registro de inventario llanta
            object[] param = { 1, 0, 0, id_inventario, id_actividad, fecha_inicio, fecha_fin, fecha_compra, mm_inicio, id_proveedor, id_factura, costo, kms, fecha_garantia, kms_garantia, id_usuario, true, "", "" };
            //Asigna al objeto retorno el método que realiza la insercion de un registro de inventario Llanta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actaliza un registro de inventario llanta
        /// </summary>
        /// <param name="id_inventario">Actualiza el identificador de un inventario</param>
        /// <param name="id_actividad">Actualiza una actividad de llanta</param>
        /// <param name="fecha_inicio">Actualiza la fecha de unicio cuando es una renovación</param>
        /// <param name="fecha_fin">Actualiza la fecha de fin de la renovación o cuando una llanta va a desecho</param>
        /// <param name="fecha_compra">Actualiza la fecha de compra de la llanta</param>
        /// <param name="mm_inicio">Actualiza los milimetros de inicio de medidad de la llanta</param>
        /// <param name="id_proveedor">Actualiza el proveedor de la llanta</param>
        /// <param name="id_factura">Actualiza la factura de compra de la llanta</param>
        /// <param name="costo">Actualiza el costo de la llanta</param>
        /// <param name="kms">Actualiza los kilometros recorridos de una llanta </param>
        /// <param name="fecha_garantia">Actualiza la fecha de garantia de la llanta</param>
        /// <param name="kms_garantia">Actualiza los kilometros de recorrido de garantia de la llanta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo la actualización del registro de Inventario Llanta</param>
        /// <returns></returns>
        public RetornoOperacion EditarInventarioLlanta(int id_inventario, int id_actividad, DateTime fecha_inicio, DateTime fecha_fin, DateTime fecha_compra, int mm_inicio, int id_proveedor, int id_factura,
                                                        decimal costo, decimal kms, DateTime fecha_garantia, decimal kms_garantia, int id_usuario)
        {
            //Retorna el método que realiz la actualizacion de un registro
            return this.editarInventarioLlanta(id_inventario, id_actividad, fecha_inicio, fecha_fin, fecha_compra, mm_inicio, id_proveedor, id_factura, costo, kms, fecha_garantia, kms_garantia, id_usuario, this._habilitar);
        }

        public RetornoOperacion DeshabilitarInventarioLlanta(int id_usuario)
        {
            //Retorna el método que realiz la actualizacion de un registro
            return this.editarInventarioLlanta(this._id_inventario, this._id_actividad, this._fecha_inicio, this._fecha_fin, this._fecha_compra, this._mm_inicio, this._id_proveedor, this._id_factura, this._costo, this._kms, this._fecha_garantia, this._kms_garantia, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaInventarioLlanta()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_inventario_llanta);
        }
        #endregion
    }
}
