using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
namespace SAT_CL.Bancos
{
    /// <summary>
    /// Clase de la tabla cuenta_bancos que permite crear operaciones sobre la tabla.
    /// </summary>
    public class CuentaBancos : Disposable
    {
        #region Enumeración

        /// <summary>
        /// Enumera el usos que se le pueden dar a una cuenta
        /// </summary>
        public enum TipoCuenta
        {
            /// <summary>
            /// Permite saber si la cuenta bancaria se utiliza para determinadas Operaciones.
            /// </summary>
            General = 0,
            /// <summary>
            /// Permite saber si la cuenta bancaria se utiliza para todas las Operaciones.
            /// </summary>
            Default = 1,
        };

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo privado que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string nom_sp = "bancos.sp_cuenta_bancos_tcb";

        private int _id_cuenta_bancos;
        /// <summary>
        /// Corresponde al identificador de cuenta_banco
        /// </summary>
        public int id_cuenta_bancos
        {
            get { return _id_cuenta_bancos; }
        }

        private int _id_banco;
        /// <summary>
        /// ID que corresponde al identificador de un banco
        /// </summary>
        public int id_banco
        {
            get { return _id_banco; }
        }

        private int _id_tabla;
        /// <summary>
        /// ID que corresponde a una entidad relacionada a una Cuenta Banco
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }

        private int _id_registro;
        /// <summary>
        /// Id que corresponde a un registro relacionado a una Cuenta Banco
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }

        private string _num_cuenta;
        /// <summary>
        /// Corresponde al número de Cuenta Bancaria
        /// </summary>
        public string num_cuenta
        {
            get { return _num_cuenta; }
        }

        private byte _id_tipo_cuenta;
        /// <summary>
        /// Corresponde al tipo de uso de una Cuenta de Banco
        /// </summary>
        public byte id_tipo_cuenta
        {
            get { return _id_tipo_cuenta; }
        }

        /// <summary>
        /// Permite acceder a la enumeración de TipoCuenta.
        /// </summary>
        public TipoCuenta tipo_cuenta
        {
            get { return (TipoCuenta)this._id_tipo_cuenta; }
        }


        private bool _habilitar;
        /// <summary>
        /// Corresponde al estado de habilitación de un registro de Cuenta Banco
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Cosntructores
        /// <summary>
        /// Constructor por default que inicializa los atributos privados
        /// </summary>
        public CuentaBancos()
        {
            this._id_cuenta_bancos = 0;
            this._id_banco = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._num_cuenta = "";
            this._id_tipo_cuenta = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Cosntructor que inicializa los atributos mediante un registros
        /// </summary>
        /// <param name="id_cuenta_bancos"> Id que permite asignar registros a los atributos</param>
        public CuentaBancos(int id_cuenta_bancos)
        {
            //Invoca al método privado carga Atributos
            cargaAtributoInstancia(id_cuenta_bancos);

        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~CuentaBancos()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite buscar y cargar a los atributos los registros de Cuenta Bancos
        /// </summary>
        /// <param name="id_cuenta_bancos">Id que permite realizar la busqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_cuenta_bancos)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y asignación  de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 3, id_cuenta_bancos, 0, 0, 0, "", 0, 0, false, "", "" };
            //Invoca al Store procedure de la tabla
            using (DataSet Ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos que existan y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(Ds.Tables[0]))
                {
                    //Recorre y almacena los registros encontrados en la variable r
                    foreach (DataRow r in Ds.Tables[0].Rows)
                    {
                        this._id_cuenta_bancos = id_cuenta_bancos;
                        this._id_banco = Convert.ToInt32(r["IdBanco"]);
                        this._id_tabla = Convert.ToByte(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._num_cuenta = Convert.ToString(r["NumeroCuenta"]);
                        this._id_tipo_cuenta = Convert.ToByte(r["IdTipoCuenta"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto retorno siempre y cuando cumpla con la validación de los datos
                    retorno = true;
                }
            }
            //Retorno del objeto al método
            return retorno;
        }
        /// <summary>
        /// Método que permite actualizar los registros de cuenta banco
        /// </summary>
        /// <param name="id_banco">Permite actualizar el campo id_banco</param>
        /// <param name="id_tabla">Permite actualizar el campo id_tabla</param>
        /// <param name="id_registro">Permite actualizar el campo id_registro</param>
        /// <param name="num_cuenta">Permite actualizar el campo num_cuenta</param>
        /// <param name="id_tipo_cuenta">Permite actualizar el campo id_tipo_cuenta</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarCuentaBancos(int id_banco, int id_tabla, int id_registro,string num_cuenta, TipoCuenta tipo_cuenta, 
                                                    int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el sp de la tbala
            object[] param = { 2, this.id_cuenta_bancos, id_banco, id_tabla, id_registro, num_cuenta, (byte)tipo_cuenta, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del objeto al método
            return retorno;
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que permite insertar registros a Cuenta Bancos
        /// </summary>
        /// <param name="id_banco">Permite insertar un id_banco en CuentaBancos</param>
        /// <param name="id_tabla">Permite insertar la entidad que hace uso de una cuenta bancaria</param>
        /// <param name="id_registro">Permite insertar un registro que hace uso de una cuenta bancaria</param>
        /// <param name="num_cuenta">Permite insertar un  numero de cuenta en CuentaBancos</param>
        /// <param name="id_tipo_cuenta">Permite insertar el  uso de una cuenta bancaria</param>
        /// <param name="id_usuario">Permite identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCuentaBancos(int id_banco, int id_tabla, int id_registro, string num_cuenta,TipoCuenta tipo_cuenta, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 1, 0, id_banco, id_tabla, id_registro, num_cuenta, (byte)tipo_cuenta,id_usuario, true, "", "" };
            //Asignación  de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del objeto al método
            return retorno;
        }

        /// <summary>
        /// Método que permite actualizar registros de cuenta banco
        /// </summary>
        /// <param name="id_banco">Permite Actualizar registros de id_banco</param>
        /// <param name="id_tabla">Permite Actualizar registros de id_tabla</param>
        /// <param name="id_registro">Permite Actualizar registros de id_regsitro</param>
        /// <param name="num_cuenta">Permite Actualizar registros de num_cuenta</param>
        /// <param name="id_tipo_cuenta">Permite Actualizar registros de id_tipo_cuenta</param>
        /// <param name="id_usuario">Permite Actualizar registros de id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarCuentaBancos(int id_banco, int id_tabla, int id_registro, string num_cuenta, TipoCuenta tipo_cuenta, int id_usuario)
        {
            //Retorna e Invoca al método privado editarCuentaBanco
            return this.editarCuentaBancos(id_banco, id_tabla, id_registro, num_cuenta, tipo_cuenta, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <param name="id_usuario">Id que permite identificar al usuario que realizo el cambio de estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCuentaBancos(int id_usuario)
        {
            //Retorna e Invoca al método privado editarCuentaBancos
            return this.editarCuentaBancos(this.id_banco, this.id_tabla, this.id_registro, this.num_cuenta, this.tipo_cuenta, id_usuario, false);
        }

        /// <summary>
        /// Carga las Cuentas de los Bancos
        /// </summary>
        /// <param name="id_tabla">Id de Tabla (25 Compañia, 76 Operador)</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_compania">Id Compañia</param> 
        /// <param name="activas">Activas</param>
        /// <returns></returns>
        public static DataTable CargaCuentasBanco(int id_tabla, int id_registro, int id_compania, byte activas)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación y asignación  de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 4, 0,0, id_tabla, id_registro, "", 0, 0, false, id_compania, activas};


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga las Cuentas de los Bancos
        /// </summary>
        /// <param name="id_tabla">Id de Tabla (25 Compañia, 76 Operador)</param>
        /// <param name="id_registro">Id Registro</param>
        ///<param name="id_tipo_cuenta">Tipo de Cuenta a Consultar</param>
        /// <returns></returns>
        public static CuentaBancos ObtieneCuentaBanco(int id_tabla, int id_registro, TipoCuenta id_tipo_cuenta)
        {
            //Definiendo objeto de retorno
            CuentaBancos objCuenBancos = null;

            //Creación y asignación  de valores al arreglo, necesarios para el sp de la tabla
            object[] param = { 5, 0, 0, id_tabla, id_registro, "", id_tipo_cuenta, 0, false, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Validacion
                    int id_cuenta = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
                    //Validamos Exista Cuenta
                    if(id_cuenta>0)
                    {
                        //Asignamos Objeto
                        objCuenBancos = new CuentaBancos(id_cuenta);
                        

                    }
                }

                //Devolviendo resultado
                return objCuenBancos;
            }
        }

        #endregion
    }
}
