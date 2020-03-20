using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    public class ReferenciaTipo : Disposable
    {
         #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure de la clase
        /// </summary>
        protected static string _nombre_stored_procedure = "global.sp_referencia_tipo_trt";

        private int _id_referencia_tipo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Referencia Tipo
        /// </summary>
        public int  id_referencia_tipo{ get { return this._id_referencia_tipo; } }
        private string _id_alterno;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Alterno
        /// </summary>
        public string id_alterno { get { return this._id_alterno; } }
          private int _id_referencia_grupo;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Refrencia Grupo
        /// </summary>
        public int  id_referencia_grupo{ get { return this._id_referencia_grupo; } }
        private int _id_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Cliente
        /// </summary>
        public int  id_cliente{ get { return this._id_cliente; } }
         private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar la descripción
        /// </summary>
        public string  descripcion{ get { return this._descripcion; } }
         private int _max_permitido;
        /// <summary>
        /// Atributo encargado de Almacenar el Max Permitido
        /// </summary>
        public int max_permitido { get { return this._max_permitido; } }
         private bool _bit_editable;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit Editable
        /// </summary>
        public bool  bit_editable{ get { return this._bit_editable; } }
         private bool _bit_valor_unico;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unico
        /// </summary>
        public bool  bit_valor_unico{ get { return this._bit_valor_unico; } }
         private bool _bit_impresion_factura;
        /// <summary>
        /// Atributo encargado de Almacenar el Bit de Impresion
        /// </summary>
        public bool  bit_impresion_factura{ get { return this._bit_impresion_factura; } }
         private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ReferenciaTipo()
        {   //Invocando Metodo de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro"></param>
        public ReferenciaTipo(int id_registro)
        {   //Invocando Metodo de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ReferenciaTipo()
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
            this._id_referencia_tipo = 0;
            this._id_alterno = "";
            this._id_referencia_grupo = 0;
            this._id_cliente = 0;
            this._descripcion = "";
            this._max_permitido = 0;
            this._bit_editable = false;
            this._bit_valor_unico = false;
            this._bit_impresion_factura = false;
            this._habilitar = false;

        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando objeto de retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, "", 0, 0, "", 0, false, false, 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada una de las Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_referencia_tipo = id_referencia_tipo;
                        this._id_alterno = dr["IdAlterno"].ToString();
                        this._id_referencia_grupo = Convert.ToInt32(dr["IdReferenciaGrupo"]);
                        this._id_cliente = Convert.ToInt32(dr["IdCliente"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._max_permitido = Convert.ToInt32(dr["MaxPermitido"]);
                        this._bit_editable = Convert.ToBoolean(dr["BitEditable"]);
                        this._bit_valor_unico = Convert.ToBoolean(dr["BitValorUnico"]);
                        this._bit_impresion_factura = Convert.ToBoolean(dr["BitImpresionFactura"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }//Asignando Positivo el Objeto de Retorno
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Devuelve el Id de Tipo de Referencia que coincida con las caracteristicas solicitadas
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="descripcion_tipo"></param>
        /// <param name="id_cliente">Id de Cliente</param>
        /// <param name="descripcion_grupo_referencia"></param>
        /// <returns></returns>
        public static int ObtieneIdReferenciaTipo(int id_compania, int id_tabla, string descripcion_tipo, int id_cliente, string descripcion_grupo_referencia)
        { 
            //Declarando arreglo de parámetros a usar en la consulta
            object[] param = { 4, 0, "", 0, id_cliente, descripcion_tipo, 0, false, false, id_tabla, false, id_compania, descripcion_grupo_referencia};

            //Realizando consulta
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param).IdRegistro;
        }

        #endregion
    }
}
