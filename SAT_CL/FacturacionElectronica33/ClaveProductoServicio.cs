using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las operaciones de la tabla "clave_producto_servicio_tcps"
    /// </summary>
    public class ClaveProductoServicio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_clave_producto_servicio_tcps";

        private int _id_clave_producto_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de clave producto servicio
        /// </summary>
        public int id_clave_producto_servicio { get { return this._id_clave_producto_servicio; } }

        private string _clave_sat;
        /// <summary>
        /// Atributo encargado de almacenar la clave producto servicio
        /// </summary>
        public string clave_sat { get { return this._clave_sat; } }

        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la descripcion de producto servicio
        /// </summary>
        public string descripcion { get { return this._descripcion; } }

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
        public ClaveProductoServicio()
        {   //Invocando Método de Cargado
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ClaveProductoServicio(int id_registro)
        {   //Invocando Método de Cargado
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ClaveProductoServicio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
            //Asignando Valores
            this._id_clave_producto_servicio = 0;
            this._clave_sat = "";
            this._descripcion = "";
            this._habilitar = false;
        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 3, id_registro, "", "", 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_clave_producto_servicio = id_registro;
                        this._clave_sat = dr["ClaveSat"].ToString();
                        this._descripcion = dr["Descripcion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="clave_sat">Permite actualizar el campo de clave de sat</param>
        /// <param name="descripcion">Permite actualizar el campo  de la descripcion del producto servicio</param>
        /// <param name="habilitar">Permite actualizar si  el campo se encuentra habilitado</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(string clave_sat, string descripcion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 2, this._id_clave_producto_servicio, clave_sat, descripcion,id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos


        /// <summary>
        /// Método encargado de Insertar registros de ClaveProductoSErvucio
        /// </summary>
        /// <param name="clave_sat">Permite insertar el campo de clave de sat</param>
        /// <param name="descripcion">Permite insertar el campo  de la descripcion del producto servicio</param>
        /// <param name="id_usuario">Permite insertar el usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaClaveProductoServicio(string clave_sat, string descripcion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, clave_sat, descripcion, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Editar los regitros de ClaveProductoServicio
        /// </summary>
        /// <param name="clave_sat">Permite Editar el campo de clave de sat</param>
        /// <param name="descripcion">Permite Editar el campo  de la descripcion del producto servicio</param>
        /// <param name="id_usuario">Permite Editar el usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaClaveProductoServicio(string clave_sat, string descripcion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(clave_sat, descripcion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Deshabilitar los registros de clavePOroductoServicio
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaClaveProductoServicio(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._clave_sat, this._descripcion, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de Actualizar el Tipo de Comprobante
        /// </summary>
        /// <returns></returns>
        public bool ActualizaClaveProductoServicio()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_clave_producto_servicio);
        }

        #endregion




    }
}
