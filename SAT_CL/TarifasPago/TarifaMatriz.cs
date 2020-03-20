using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.TarifasPago
{
    public class TarifaMatriz:Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de alamcenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas_pago.sp_tarifa_matriz_ttm";

        private int _id_tarifa_matriz;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tarifa Matriz
        /// </summary>
        public int id_tarifa_matriz { get { return this._id_tarifa_matriz; } }
        private int _id_tarifa;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa
        /// </summary>
        public int id_tarifa { get { return this._id_tarifa; } }
        private string _valor_filtro_col;
        /// <summary>
        /// Atributo encargado de almacenar el Valor Filtro de la Columna
        /// </summary>
        public string valor_filtro_col { get { return this._valor_filtro_col; } }
        private string _valor_filtro_row;
        /// <summary>
        /// Atributo encargado de almacenar el Valor Filtro de la Celda
        /// </summary>
        public string valor_filtro_row { get { return this._valor_filtro_row; } }
        private string _valor_desc_col;
        /// <summary>
        /// Atributo encargado de almacenar el Valor de la Descripción de la Columna
        /// </summary>
        public string valor_desc_col { get { return this._valor_desc_col; } }
        private string _valor_desc_row;
        /// <summary>
        /// Atributo encargado de almacenar el Valor de la Descripción de la Celda
        /// </summary>
        public string valor_desc_row { get { return this._valor_desc_row; } }
        private int _posicion_x;
        /// <summary>
        /// Atributo encargado de almacenar la Posición X
        /// </summary>
        public int posicion_x { get { return this._posicion_x; } }
        private int _posicion_y;
        /// <summary>
        /// Atributo encargado de almacenar la Posición Y
        /// </summary>
        public int posicion_y { get { return this._posicion_y; } }
        private decimal _tarifa_cargado;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa Cargado
        /// </summary>
        public decimal tarifa_cargado { get { return this._tarifa_cargado; } }
        private decimal _tarifa_vacio;
        /// <summary>
        /// Atributo encargado de almacenar la Tarifa Cargado
        /// </summary>
        public decimal tarifa_vacio { get { return this._tarifa_vacio; } }
        private decimal _tarifa_tronco;
        /// <summary>
        /// Obtiene el valor de tarifa para traslado en tronco
        /// </summary>
        public decimal tarifa_tronco { get { return this._tarifa_tronco; } }
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
        public TarifaMatriz()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public TarifaMatriz(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TarifaMatriz()
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
            this._id_tarifa_matriz = 0;
            this._id_tarifa = 0;
            this._valor_filtro_col = "";
            this._valor_filtro_row = "";
            this._valor_desc_col = "";
            this._valor_desc_row = "";
            this._posicion_x = 0;
            this._posicion_y = 0;
            this._tarifa_cargado = 0;
            this._tarifa_tronco = 0;
            this._tarifa_vacio = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, "", "", "", "", 0, 0, 0, 0, 0, 0, true, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tarifa_matriz = id_registro;
                        this._id_tarifa = Convert.ToInt32(dr["IdTarifa"]);
                        this._valor_filtro_col = dr["ValorFiltroCol"].ToString();
                        this._valor_filtro_row = dr["ValorFiltroRow"].ToString();
                        this._valor_desc_col = dr["ValorDescCol"].ToString();
                        this._valor_desc_row = dr["ValorDescRow"].ToString();
                        this._posicion_x = Convert.ToInt32(dr["PosicionX"]);
                        this._posicion_y = Convert.ToInt32(dr["PosicionY"]);
                        this._tarifa_cargado = Convert.ToDecimal(dr["TarifaCargado"]);
                        this._tarifa_tronco = Convert.ToDecimal(dr["TarifaTronco"]);
                        this._tarifa_vacio = Convert.ToDecimal(dr["TarifaVacio"]);
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
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_tarifa">Tarifa a la que pertenece la Matriz</param>
        /// <param name="valor_filtro_col">Valor Filtro de la Columna</param>
        /// <param name="valor_filtro_row">Valor Filtro de la Celda</param>
        /// <param name="valor_desc_col">Valor Descripcion de la Columna</param>
        /// <param name="valor_desc_row">Valor Descripcion de la Celda</param>
        /// <param name="posicion_x">Posición X</param>
        /// <param name="posicion_y">Posición Y</param>
        /// <param name="tarifa_cargado">Tarifa Cargado</param>
        /// <param name="tarifa_vacio">Tarifa Vacio</param>
        /// <param name="tarifa_tronco">Tarifa en Tronco</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tarifa, string valor_filtro_col, string valor_filtro_row, string valor_desc_col, string valor_desc_row,
                                                    int posicion_x, int posicion_y, decimal tarifa_cargado, decimal tarifa_vacio, decimal tarifa_tronco, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tarifa_matriz, id_tarifa, valor_filtro_col, valor_filtro_row, valor_desc_col, valor_desc_row,
                                 posicion_x, posicion_y, tarifa_cargado, tarifa_vacio, tarifa_tronco, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Matrices de las Tarifas
        /// </summary>
        /// <param name="id_tarifa">Tarifa a la que pertenece la Matriz</param>
        /// <param name="valor_filtro_col">Valor Filtro de la Columna</param>
        /// <param name="valor_filtro_row">Valor Filtro de la Celda</param>
        /// <param name="valor_desc_col">Valor Descripcion de la Columna</param>
        /// <param name="valor_desc_row">Valor Descripcion de la Celda</param>
        /// <param name="posicion_x">Posición X</param>
        /// <param name="posicion_y">Posición Y</param>
        /// <param name="tarifa_cargado">Tarifa Cargado</param>
        /// <param name="tarifa_vacio">Tarifa Vacio</param>
        /// <param name="tarifa_tronco">Tarifa en tronco</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifaMatriz(int id_tarifa, string valor_filtro_col, string valor_filtro_row, string valor_desc_col, string valor_desc_row,
                                                    int posicion_x, int posicion_y, decimal tarifa_cargado, decimal tarifa_vacio, decimal tarifa_tronco, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tarifa, valor_filtro_col, valor_filtro_row, valor_desc_col, valor_desc_row,
                                 posicion_x, posicion_y, tarifa_cargado, tarifa_vacio, tarifa_tronco, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Matrices de las Tarifas
        /// </summary>
        /// <param name="id_tarifa">Tarifa a la que pertenece la Matriz</param>
        /// <param name="valor_filtro_col">Valor Filtro de la Columna</param>
        /// <param name="valor_filtro_row">Valor Filtro de la Celda</param>
        /// <param name="valor_desc_col">Valor Descripcion de la Columna</param>
        /// <param name="valor_desc_row">Valor Descripcion de la Celda</param>
        /// <param name="posicion_x">Posición X</param>
        /// <param name="posicion_y">Posición Y</param>
        /// <param name="tarifa_cargado">Tarifa Cargado</param>
        /// <param name="tarifa_vacio">Tarifa Vacio</param>
        /// <param name="tarifa_tronco">Tarifa en Tronco</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifaMatriz(int id_tarifa, string valor_filtro_col, string valor_filtro_row, string valor_desc_col, string valor_desc_row,
                                                    int posicion_x, int posicion_y, decimal tarifa_cargado, decimal tarifa_vacio, decimal tarifa_tronco, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tarifa, valor_filtro_col, valor_filtro_row, valor_desc_col, valor_desc_row,
                                 posicion_x, posicion_y, tarifa_cargado, tarifa_vacio, tarifa_tronco, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Matrices de las Tarifas
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifaMatriz(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tarifa, this._valor_filtro_col, this._valor_filtro_row, this._valor_desc_col, this._valor_desc_row,
                                 this._posicion_x, this._posicion_y, this._tarifa_cargado, this._tarifa_vacio, this._tarifa_tronco, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de las Matrices de las Tarifas
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTarifaMatriz()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tarifa_matriz);
        }
        
        /// <summary>
        /// Método Público encargado de Obtener los elementos de matriz ligadas a una Tarifa
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <returns></returns>
        public static DataTable ObtieneMatrizTarifa(int id_tarifa)
        {   //Declarando OBjeto de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_tarifa, "", "", "", "", 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado de la Consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valores
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Públic encargado de Obtener los Detalles Ligados a una Tarifa con un Valor Especifico de una Columna
        /// </summary>
        /// <param name="id_tarifa">Tarifa</param>
        /// <param name="valor_filtro_col">Valor de la Columna</param>
        /// <param name="valor_filtro_row">Valor de la Celda</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesTarifaValorColumna(int id_tarifa, string valor_filtro_col, string valor_filtro_row)
        {   //Declarando OBjeto de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_tarifa, valor_filtro_col, valor_filtro_row, "", "", 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado de la Consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valores
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        
        /// <summary>
        /// Método Público encargado de Obtener los Detalles de las Matrices ligadas a una Tarifa
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <returns></returns>
        public static DataSet ObtieneDetallesMatrizTarifa(int id_tarifa)
        {   //Armando Arreglo de Parametros
            object[] param = { 6, 0, id_tarifa, "", "", "", "", 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado de la Consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                //Devolviendo Resultado Obtenido
                return ds;
        }
        /// <summary>
        /// Obtiene el detalle de tarifa que coincida con los valores de filtros indicados
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="valor_filtro_col">Valor de búsqueda en columnas</param>
        /// <param name="valor_filtro_row">Valor de búsqueda en filas</param>
        /// <param name="operador_filtro_col">Operador de búsqueda en columnas</param>
        /// <param name="operador_filtro_row">Operador de búsqueda en filas (válidos: '=' y '&lt;=')</param>
        /// <returns></returns>
        public static TarifaMatriz ObtieneDetalleMatrizTarifa(int id_tarifa, string valor_filtro_col, string valor_filtro_row, string operador_filtro_col, string operador_filtro_row)
        {
            //Declarando objeto de retorno
            TarifaMatriz tarifa = new TarifaMatriz();

            //Armando Arreglo de Parametros para consulta de detalles
            object[] param = { 7, 0, id_tarifa, valor_filtro_col, valor_filtro_row, "", "", 0, 0, 0, 0, 0, 0, false, operador_filtro_col, operador_filtro_row };

            //Obteniendo Resultado de la Consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo orign de datos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Instanciando registro 
                        tarifa = new TarifaMatriz(r.Field<int>("Id"));
                        break;
                    }
                }
            }

            //Devolviendo detalle de tarifa
            return tarifa;
        }
         
        #endregion
    }
}
