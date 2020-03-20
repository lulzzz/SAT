using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
using System.Web.UI.WebControls;

namespace SAT_CL.Seguridad
{
    /// <summary>
    /// Proporciona los medios para la administración de elementos Forma
    /// </summary>
    public class Forma : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los distintos tipos de recurso Forma disponibles
        /// </summary>
        public enum TipoRecurso
        {
            /// <summary>
            /// Forma Web
            /// </summary>
            FormaWeb = 1,
            /// <summary>
            /// Página Maestra
            /// </summary>
            PaginaMaestra,
            //Control de Usuario Web
            ControlUsuarioWeb
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "seguridad.sp_forma_tf";

        private int _id_forma;
        /// <summary>
        /// Obtiene el Id de Registro de la instancia
        /// </summary>
        public int id_forma { get { return this._id_forma; } }
        private byte _id_tipo_recurso;
        /// <summary>
        /// Obtiene el Tipo de recurso de la instancia
        /// </summary>
        public TipoRecurso tipo_recurso { get { return (TipoRecurso)this._id_tipo_recurso; } }
        private string _nombre_forma;
        /// <summary>
        /// Obtiene el Nombre de la forma de la instancia
        /// </summary>
        public string nombre_forma { get { return this._nombre_forma; } }
        private string _descripcion_forma;
        /// <summary>
        /// Obtiene la descripción de la instancia
        /// </summary>
        public string descripcion_forma { get { return this._descripcion_forma; } }
        private string _ruta_relativa;
        /// <summary>
        /// Obtiene la ruta relativa de la forma representada por la instancia
        /// </summary>
        public string ruta_relativa { get { return this._ruta_relativa; } }
        private bool _habilitar;
        /// <summary>
        /// Obteiene el valor que determina si la entidad es considerada como activa
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia del tipo Forma con los datos del registro solicitado
        /// </summary>
        /// <param name="id_forma">Id de Forma a recuperar</param>
        public Forma(int id_forma)
        {
            cargaAtributosInstancia(id_forma);
        }
        /// <summary>
        /// Crea una instancia del tipo Forma a partir de la url de la misma
        /// </summary>
        /// </summary>
        /// <param name="ruta_relativa">Ruta Relativa a consultar</param>
        public Forma(string ruta_relativa)
        {
            cargaAtributosInstancia(ruta_relativa);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instancia en base al Id de registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de registro a consultar</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_registro, 0, "", "", "", 0, 0, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        this._id_forma = Convert.ToInt32(r["Id"]);
                        this._id_tipo_recurso = Convert.ToByte(r["IdTipoRecurso"]);
                        this._nombre_forma = r["NombreForma"].ToString();
                        this._descripcion_forma = r["DescripcionForma"].ToString();
                        this._ruta_relativa = r["RutaRelativa"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Indicando resultado correcto de signación
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga de los atributos de la instancia en base a la ruta relativa indicada
        /// </summary>
        /// <param name="ruta_relativa">Ruta Relativa a consultar</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(string ruta_relativa)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 4, 0, 0, "", "", ruta_relativa, 0, 0, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        this._id_forma = Convert.ToInt32(r["Id"]);
                        this._id_tipo_recurso = Convert.ToByte(r["IdTipoRecurso"]);
                        this._nombre_forma = r["NombreForma"].ToString();
                        this._descripcion_forma = r["DescripcionForma"].ToString();
                        this._ruta_relativa = r["RutaRelativa"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Indicando resultado correcto de signación
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Aplica la seguridad de manera recursiva sobre un control, para el nombre de control negado que se ha solicitado
        /// </summary>
        /// <param name="nombre_control_negado">Nombre del control a proteger</param>
        /// <param name="control">Control a evaluar</param>
        /// <param name="nivel_seguridad_principal">Indica que tipo de elemento desencadenó la petición inicial de seguridad</param>
        private static void aplicaSeguridadControl(string nombre_control_negado, System.Web.UI.Control control, TipoRecurso nivel_seguridad_principal)
        {
            //Declarando bandera para señalar si es valida la inspección del control actual en base a su tipo de recurso
            bool aplicar_seguridad = true;
            switch (nivel_seguridad_principal)
            {
                case TipoRecurso.FormaWeb:
                case TipoRecurso.ControlUsuarioWeb:
                    //Si es un control de usuario
                    if (control is System.Web.UI.UserControl)
                        aplicar_seguridad = false;
                    break;
                case TipoRecurso.PaginaMaestra:
                    //Si es un control de usuario o una forma web
                    if (control is System.Web.UI.UserControl || (control is ContentPlaceHolder))
                        aplicar_seguridad = false;
                    break;                
            }

            //Si se aplicará seguridad
            if (aplicar_seguridad)
            {
                //Si el control tiene el mismo nombre que el elemento a bloquear
                if (nombre_control_negado == control.ID)
                {
                    try
                    {
                        //Ocultando elemento
                        control.Visible = false;
                    }
                    catch (NotImplementedException) { }
                }
                //De lo contrario
                else
                {
                    //Si el control tiene subelementos
                    if (control.HasControls())
                    {
                        //Para cada elemento
                        foreach (System.Web.UI.Control c in control.Controls)
                        {
                            aplicaSeguridadControl(nombre_control_negado, c, nivel_seguridad_principal);
                        }
                    }
                }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_forma);
        }
        /// <summary>
        /// Busca los elementos negados al usuario (por perfil activo y usuario) para el recurso señalado
        /// </summary>
        /// <param name="forma">Referencia del recurso por proteger (Sin Página Maestra)</param>
        /// <param name="id_usuario">Id de usuario al que se configurará</param>
        public static void AplicaSeguridadForma(System.Web.UI.Page forma, int id_usuario)
        { 
            //Instanciando recurso (forma o control de usuario)
            using (Forma f = new Forma(forma.AppRelativeVirtualPath))
            { 
                //Si la forma existe
                if (f.habilitar)
                { 
                    //Cargando elementos negados al usuario señalado
                    using (DataTable mit = ControlPerfilUsuario.CargaControlesNegadosUsuario(id_usuario, f.id_forma))
                    { 
                        //Si hay elementos por proteger ante el usuario
                        if (mit != null)
                        { 
                            //Para cada uno de los controles que serán negados
                            foreach (string nombre_control in (from DataRow r in mit.Rows
                                                               select r["Nombre"].ToString()))
                            { 
                                //Para Cada control de interés
                                foreach (System.Web.UI.Control control in forma.Controls)
                                {
                                    //Aplicando seguridad
                                    aplicaSeguridadControl(nombre_control, control, f.tipo_recurso);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Busca los elementos negados al usuario (por perfil activo y usuario) para el recurso señalado
        /// </summary>
        /// <param name="forma">Referencia del recurso por proteger (Asociada a Página Maestra)</param>
        /// <param name="nombre_contenedor">Nombre del contenedor (ContentPlaceHolder) de la página maestra asociada, en la que se encuentra el contenido de la forma</param>
        /// <param name="id_usuario">Id de usuario al que se configurará</param>
        public static void AplicaSeguridadForma(System.Web.UI.Page forma, string nombre_contenedor, int id_usuario)
        {
            //Instanciando recurso (forma o control de usuario)
            using (Forma f = new Forma(forma.AppRelativeVirtualPath))
            {
                //Si la forma existe
                if (f.habilitar)
                {
                    //Cargando elementos negados al usuario señalado
                    using (DataTable mit = ControlPerfilUsuario.CargaControlesNegadosUsuario(id_usuario, f.id_forma))
                    {
                        //Si hay elementos por proteger ante el usuario
                        if (mit != null)
                        {
                            //Para cada uno de los controles que serán negados
                            foreach (string nombre_control in (from DataRow r in mit.Rows
                                                               select r["Nombre"].ToString()))
                            {
                                //Aplicando seguridad
                                aplicaSeguridadControl(nombre_control, forma.Master.FindControl(nombre_contenedor), f.tipo_recurso);

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Busca los elementos negados al usuario (por perfil activo y usuario) para el recurso señalado
        /// </summary>
        /// <param name="master">Referencia del recurso por proteger</param>
        /// <param name="id_usuario">Id de usuario al que se configurará</param>
        public static void AplicaSeguridadPaginaMaestra(System.Web.UI.MasterPage master, int id_usuario)
        {
            //Instanciando recurso (forma o control de usuario)
            using (Forma f = new Forma(master.AppRelativeVirtualPath))
            {
                //Si la forma existe
                if (f.habilitar)
                {
                    //Cargando elementos negados al usuario señalado
                    using (DataTable mit = ControlPerfilUsuario.CargaControlesNegadosUsuario(id_usuario, f.id_forma))
                    {
                        //Si hay elementos por proteger ante el usuario
                        if (mit != null)
                        {
                            //Para cada uno de los controles que serán negados
                            foreach (string nombre_control in (from DataRow r in mit.Rows
                                                               select r["Nombre"].ToString()))
                            {
                                //Para Cada control de interés
                                foreach (System.Web.UI.Control control in master.Controls)
                                {
                                    //Aplicando seguridad
                                    aplicaSeguridadControl(nombre_control, control, f.tipo_recurso);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Busca los elementos negados al usuario (por perfil activo y usuario) para el recurso señalado
        /// </summary>
        /// <param name="control_usuario">Referencia del recurso por proteger</param>
        /// <param name="id_usuario">Id de usuario al que se configurará</param>
        public static void AplicaSeguridadControlusuarioWeb(System.Web.UI.UserControl control_usuario, int id_usuario)
        {
            //Instanciando recurso (forma o control de usuario)
            using (Forma f = new Forma(control_usuario.AppRelativeVirtualPath))
            {
                //Si la forma existe
                if (f.habilitar)
                {
                    //Cargando elementos negados al usuario señalado
                    using (DataTable mit = ControlPerfilUsuario.CargaControlesNegadosUsuario(id_usuario, f.id_forma))
                    {
                        //Si hay elementos por proteger ante el usuario
                        if (mit != null)
                        {
                            //Para cada uno de los controles que serán negados
                            foreach (string nombre_control in (from DataRow r in mit.Rows
                                                               select r["Nombre"].ToString()))
                            {
                                //Para Cada control de interés
                                foreach (System.Web.UI.Control control in control_usuario.Controls)
                                {
                                    //Aplicando seguridad
                                    aplicaSeguridadControl(nombre_control, control, f.tipo_recurso);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
