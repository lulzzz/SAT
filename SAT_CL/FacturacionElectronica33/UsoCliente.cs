using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// 
    /// </summary>
    public class UsoCliente : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_uso_cliente_tuc";

        private int _id_uso_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar el Uso del Cliente
        /// </summary>
        public int id_uso_cliente { get { return this._id_uso_cliente; } }
        private int _id_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar el Cliente
        /// </summary>
        public int id_cliente { get { return this._id_cliente; } }
        private int _id_uso;
        /// <summary>
        /// Atributo encargado de Almacenar el Uso del Comprobante v3.3
        /// </summary>
        public int id_uso { get { return this._id_uso; } }
        private int _secuencia;
        /// <summary>
        /// Atributo encargado de Almacenar la Secuencia
        /// </summary>
        public int secuencia { get { return this._secuencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public UsoCliente()
        {
            //Asignando Valores
            this._id_uso_cliente =
            this._id_cliente =
            this._id_uso =
            this._secuencia = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_uso_cliente">Uso de Cliente</param>
        public UsoCliente(int id_uso_cliente)
        {
            //Invocando Metodo de Carga
            cargaAtributosInstancia(id_uso_cliente);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UsoCliente()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_uso_cliente">Uso de Cliente</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_uso_cliente)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_uso_cliente, 0, 0, 0, 0, false, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_uso_cliente = id_uso_cliente;
                        this._id_cliente = Convert.ToInt32(dr["IdCliente"]);
                        this._id_uso = Convert.ToInt32(dr["IdUso"]);
                        this._secuencia = Convert.ToInt32(dr["Secuencia"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Valor Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_uso">Uso del Comprobante v3.3</param>
        /// <param name="secuencia">Secuencia de los Usos del Cliente</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_cliente, int id_uso, int secuencia, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_uso_cliente, id_cliente, id_uso, secuencia, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar el Uso del Cliente
        /// </summary>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_uso">Uso del Comprobante v3.3</param>
        /// <param name="secuencia">Secuencia de los Usos del Cliente</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUsoCliente(int id_cliente, int id_uso, int secuencia, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_cliente, id_uso, secuencia, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar el Uso del Cliente
        /// </summary>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_uso">Uso del Comprobante v3.3</param>
        /// <param name="secuencia">Secuencia de los Usos del Cliente</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaUsoCliente(int id_cliente, int id_uso, int secuencia, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_cliente, id_uso, secuencia, id_usuario, this.habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar el Uso del Cliente
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUsoCliente(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this.id_cliente, this.id_uso, this.secuencia, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Uso del Cliente
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUsoCliente()
        {
            //Devolviendo resultado Obtenido
            return this.cargaAtributosInstancia(this._id_uso_cliente);
        }
        /// <summary>
        /// Método encargado de Obtener los Usos de CFDI dado un Cliente
        /// </summary>
        /// <param name="id_cliente">Cliente</param>
        /// <returns></returns>
        public static DataTable ObtieneUsosClienteCFDI(int id_cliente)
        {
            //Declarando Objeto de Retorno
            DataTable dtUsos = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_cliente, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtUsos = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtUsos;
        }

        #endregion
    }
}
