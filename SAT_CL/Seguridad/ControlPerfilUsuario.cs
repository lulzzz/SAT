using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Clase encargade de asignar permisos para usuarios con referencia a sus perfiles
    /// </summary>
    public class ControlPerfilUsuario:Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Tipo 
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Perfil
            /// </summary>
            Perfil = 1,
            /// <summary>
            ///Usuario
            /// </summary>
            Usuario,
        }

        #endregion

        #region Propiedades y atributos

        /// <summary>
        ///  Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "seguridad.sp_control_perfil_usuario_tcpu";
     

        //Propiedades y atributos
        private int _id_control_perfil_usuario;
        /// <summary>
        /// Obtiene la Id de control Perfil de Usuario
        /// </summary>
        public int id_control_perfil_usuario { get { return this._id_control_perfil_usuario; } }
        private byte _id_tipo;
        /// <summary>
        /// Obtiene la Id de Tipo
        /// </summary>
        public byte id_tipo { get { return this._id_tipo; } }
        private int _id_control;
        /// <summary>
        /// Obtiene la Id de Control
        /// </summary>
        public int id_control { get { return this._id_control; } }
        private int _id_perfil;
        /// <summary>
        /// Obtiene la Id de Perfil
        /// </summary>
        public int id_perfil { get { return this._id_perfil; } }
        private int _id_usuario;
        /// <summary>
        /// Obtiene la Id de Usuario
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }
        private decimal _valor;
        /// <summary>
        /// Obtiene el Valor
        /// </summary>
        public decimal valor { get { return this._valor; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación de registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        /// <summary>
        /// Enumera el Tipo 
        /// </summary>
        public Tipo TipoControlPerfilUsuario { get { return (Tipo)_id_tipo; } }

        
        #endregion
       
        #region Constructores


          /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ControlPerfilUsuario()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public ControlPerfilUsuario(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Tipo, Control, Perfil, Uusraio
        /// </summary>
        /// <param name="tipo">Tipo</param>
        /// <param name="id_control">Id Control</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="id_usuario">Id Usuario</param>
        public ControlPerfilUsuario(Tipo tipo, int id_control, int id_perfil, int id_usuario)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(tipo, id_control, id_perfil, id_usuario);
        } 
             
        #endregion
        
        #region Destructores
        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~ControlPerfilUsuario()
        {
            Dispose(false);
        }
        #endregion
        
        #region Metodos privados

            /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {  
            this._id_control_perfil_usuario = 0;
            this._id_tipo = 0;
            this._id_control = 0;
            this._id_perfil = 0;
            this._id_usuario = 0;
            this._valor = 0;
            this._habilitar = false;
        }
        
        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Inicializamos el arreglo de parametros                               
            object[] param = {3, id_registro, 0, 0, 0, 0, 0, 0, false, "", ""};
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {   
                        this._id_control_perfil_usuario = Convert.ToInt32(r["Id"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._id_control = Convert.ToInt32(r["IdControl"]);
                        this._id_perfil = Convert.ToInt32(r["IdPerfil"]);
                        this._id_usuario = Convert.ToInt32(r["IdUsuario"]);
                        this._valor = Convert.ToDecimal(r["Valor"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        /// Método encargado de Inicializar una ControlPefilUsuario
        /// </summary>
        /// <param name="tipo">Tipo</param>
        /// <param name="id_control">Id Control</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(Tipo tipo, int id_control, int id_perfil, int id_usuario)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Inicializamos el arreglo de parametros                               
            object[] param = { 6, 0, tipo, id_control, id_perfil, id_usuario, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        this._id_control_perfil_usuario = Convert.ToInt32(r["Id"]);
                        this._id_tipo = Convert.ToByte(r["IdTipo"]);
                        this._id_control = Convert.ToInt32(r["IdControl"]);
                        this._id_perfil = Convert.ToInt32(r["IdPerfil"]);
                        this._id_usuario = Convert.ToInt32(r["IdUsuario"]);
                        this._valor = Convert.ToDecimal(r["Valor"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        ///  Carga los valores de registro sobre atributos de instancia
        /// </summary>
        /// <param name="tipo">Tipo (Perfil/Usuario)</param>
        /// <param name="id_control">Id Control</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="valor">Valor</param>
        /// <param name="id_usuario_actualiza">Id Usuario Actualiza</param>
        /// <returns></returns>
        private RetornoOperacion editaControlPerfilUsuario(Tipo tipo, int id_control, 
                                                  int id_perfil, int id_usuario,  decimal valor, int id_usuario_actualiza, bool habilitar
                                                  )
        {
            //Incializando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = {2, this._id_control_perfil_usuario, tipo, id_control, id_perfil, id_usuario, valor, id_usuario_actualiza, habilitar, "", ""};
            //Realizando la actualización solicitada
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado
            return resultado;
        }

        
        #endregion
        
        #region Metodos Publicos

        /// <summary>
        ///  Inserta un nuevo registro de Forma
        /// </summary>
        /// <param name="tipo"> Tipo (Perfil/Usuario)</param>
        /// <param name="id_control">Id Control</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="valor">Valor</param>
        /// <param name="id_usuario_actualiza">Id Usuario Actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaControlPerfilUsuario(Tipo tipo, int id_control, 
                                                  int id_perfil, int id_usuario,  decimal valor, int id_usuario_actualiza
                                                  )
         {
            //Incializando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = {1, 0, tipo, id_control, id_perfil, id_usuario, valor, id_usuario_actualiza, true, "","" };
            //Realizando la actualización solicitada
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        ///  Carga los valores de registro sobre atributos de instancia
        /// </summary>
        /// <param name="tipo">Tipo (Perfil/Usuario)</param>
        /// <param name="id_control">Id Control</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="valor">Valor</param>
        /// <param name="id_usuario_actualiza">Id Usuario Actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaControlPerfilUsuario(Tipo tipo, int id_control,
                                                  int id_perfil, int id_usuario, decimal valor, int id_usuario_actualiza
                                                  )
        {
            //Incializando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = { 2, this._id_control_perfil_usuario, tipo, id_control, id_perfil, id_usuario, valor, id_usuario_actualiza, true, "", "" };
            //Realizando la actualización solicitada
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado
            return resultado;
        }
        
        /// <summary>
        /// Realiza la deshabilitación de cualquier configuración existente asociada al control solicitado
        /// </summary>
        /// <param name="id_usuario_actualiza">Id Usuario Actualiza</param>
        /// <returns></returns>
        public  RetornoOperacion DeshabilitaControlesPerfilUsuario(int id_usuario_actualiza)
        { 
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualización
            resultado = this.editaControlPerfilUsuario((Tipo)this._id_tipo, this._id_control, this._id_perfil, this._id_usuario,
                                                  this._valor, id_usuario_actualiza, false);

            //Devolviendo resultado
            return resultado;
        }

    
        /// <summary>
        /// Autorización sobre el control indicado para el perfil
        /// </summary>
        /// <param name="id_accion">Id Acción</param>
        /// <param name="id_perfil">Id Perfil</param>
        /// <param name="valor">-1 Niega Control / 1 Autoriza Control</param>
        /// <param name="id_usuario_actualiza">Usuario que realiza las actualizaciones del registro</param>
        /// <returns></returns>
        public static RetornoOperacion AutorizaControlPerfil(byte id_accion, int id_perfil, decimal valor, int id_usuario_actualiza)
        {
            //DFeclaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Cargamos Controles ligados a la Acción
                using (DataTable mit = SAT_CL.Seguridad.Control.CargaControles(id_accion))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos cada uno de los Controles
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciamos Control Perfil Usuario
                            using (ControlPerfilUsuario objControlPerfilUsuario = new ControlPerfilUsuario(Tipo.Perfil, r.Field<int>("Id"), id_perfil, 0))
                            {
                                //Validamos Existencia del Objeto
                                if (objControlPerfilUsuario.id_control_perfil_usuario > 0)
                                {
                                    //Negamos Control Perfil Usuario
                                    resultado = objControlPerfilUsuario.editaControlPerfilUsuario((Tipo)objControlPerfilUsuario.id_tipo, objControlPerfilUsuario.id_control,
                                                                                                 objControlPerfilUsuario.id_perfil, objControlPerfilUsuario.id_usuario, valor, id_usuario_actualiza, objControlPerfilUsuario.habilitar);
                                }
                                else
                                {
                                    //Insertamos Nuevo Registro
                                    resultado = InsertaControlPerfilUsuario(Tipo.Perfil, r.Field<int>("Id"), id_perfil, 0, valor, id_usuario_actualiza);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del Ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }

            }
            //Devolvemos Resultado
            return resultado;
        }


        /// <summary>
        /// Autorización sobre el control indicado para el perfil
        /// </summary>
        /// <param name="id_accion">Id Acción</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="valor">-1 Niega Control / 1 Autoriza Control</param>
        /// <param name="id_usuario_actualiza">Usuario que realiza las actualizaciones del registro</param>
        /// <returns></returns>
        public static RetornoOperacion AutorizaControlUsuario(byte id_accion, int id_usuario, decimal valor, int id_usuario_actualiza)
        {
            //DFeclaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
              //Cargamos Controles ligados a la Acción
                using (DataTable mit = SAT_CL.Seguridad.Control.CargaControles(id_accion))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recorremos cada uno de los Controles
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciamos Control Perfil Usuario
                            using (ControlPerfilUsuario objControlPerfilUsuario = new ControlPerfilUsuario(Tipo.Usuario, r.Field<int>("Id"), 0, id_usuario))
                            {
                                //Validamos Existencia del Objeto
                                if (objControlPerfilUsuario.id_control_perfil_usuario > 0)
                                {
                                    //Negamos Control Perfil Usuario
                                    resultado = objControlPerfilUsuario.editaControlPerfilUsuario((Tipo)objControlPerfilUsuario.id_tipo, objControlPerfilUsuario.id_control,
                                                                                                 objControlPerfilUsuario.id_perfil, objControlPerfilUsuario.id_usuario, valor, id_usuario_actualiza, objControlPerfilUsuario.habilitar);
                                }
                                else
                                {
                                    //Insertamos Nuevo Registro
                                    resultado = InsertaControlPerfilUsuario(Tipo.Usuario, r.Field<int>("Id"), 0, id_usuario, valor, id_usuario_actualiza);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del Ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Carga Controles negados del Usuario
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="id_forma"></param>
        /// <returns></returns>
        public static DataTable CargaControlesNegadosUsuario(int id_usuario, int id_forma)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicialziando los parámetros de consulta
            object[] param = { 4, 0, 0, 0, 0, id_usuario, 0, 0, false, id_forma, "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
        /// Carga los controles negados al perfil
        /// </summary>
        public static DataTable CargaControlesNegadosPerfil(int idPerfil)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicialziando los parámetros de consulta
            object[] param = { 5, 0, 0, 0, idPerfil, 0, 0, 0, false, "", "" };

           //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}
