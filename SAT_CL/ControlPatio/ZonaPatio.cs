using Microsoft.SqlServer.Types;
using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{
    /// <summary>
    /// Clase encargada de las Operaciones correspondientes las Zonas de los Patios
    /// </summary>
    public class ZonaPatio : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración encargada de Especificar los Tipos de Zona
        /// </summary>
        public enum TipoZona
        {   /// <summary>
            /// Expresa que el Tipo de Zona se refiere una Zona de Andenes del Patio
            /// </summary>
            Andenes = 1,
            /// <summary>
            /// Expresa que el Tipo de Zona se refiere una Zona de Cajas del Patio
            /// </summary>
            Cajas
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "control_patio.sp_zona_patio_tzp";

        private int _id_zona_patio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Zona del Patio
        /// </summary>
        public int id_zona_patio { get { return this._id_zona_patio; } }
        private int _id_ubicacion_patio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Ubicación del Patio
        /// </summary>
        public int id_ubicacion_patio { get { return this._id_ubicacion_patio; } }
        private int _id_zona_superior;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Zona a la que Pertenece el Patio
        /// </summary>
        public int id_zona_superior { get { return this._id_zona_superior; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción de la Zona del Patio
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private byte _id_tipo_zona;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de la Zona del Patio
        /// </summary>
        public byte id_tipo_zona { get { return this._id_tipo_zona; } }
        /// <summary>
        /// Atributo encargado de almacenar la Enumeracion del Tipo de la Zona del Patio
        /// </summary>
        public TipoZona tipo_zona { get { return (TipoZona)this._id_tipo_zona; } }
        private SqlGeography _geoubicacion;
        /// <summary>
        /// Atributo encargado de almacenar la Geoubicación de la Zona del Patio
        /// </summary>
        public SqlGeography geoubicacion { get { return this._geoubicacion; } }
        private string _color_hxd;
        /// <summary>
        /// Atributo encargado de almacenar el Color Asignado a la Zona del Patio
        /// </summary>
        public string color_hxd { get { return this._color_hxd; } }
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
        public ZonaPatio()
        {   //Asignando Valores
            this._id_zona_patio = 0;
            this._id_ubicacion_patio = 0;
            this._id_zona_superior = 0;
            this._descripcion = "";
            this._id_tipo_zona = 0;
            this._geoubicacion = null;
            this._color_hxd = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_zona_patio">Zona de Patio</param>
        public ZonaPatio(int id_zona_patio)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_zona_patio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ZonaPatio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atritbutos dado un Registro
        /// </summary>
        /// <param name="id_zona_patio">Zona del Patio</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_zona_patio)
        {   //Declarando Objeto de retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_zona_patio, 0, 0, "", 0, null, "", 0, false, "", "" };
            //Obteniendo Resultado del Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_zona_patio = id_zona_patio;
                        this._id_ubicacion_patio = Convert.ToInt32(dr["IdUbicacionPatio"]);
                        this._id_zona_superior = Convert.ToInt32(dr["IdZonaSuperior"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_tipo_zona = Convert.ToByte(dr["IdTipoZona"]);
                        this._geoubicacion = (SqlGeography)dr["Geoubicacion"];
                        this._color_hxd = dr["ColorHXD"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Retorno a Positivo
                    result = true;
                }
                //Devolviendo Resultado Obtenido
                return result;
            }
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_zona_superior">Zona Superior</param>
        /// <param name="descripcion">Descripción de la Zona</param>
        /// <param name="tipo_zona">Tipo de Zona (Andenes, Cajones)</param>
        /// <param name="geoubicacion">Geoubicación de la Zona</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_ubicacion_patio, int id_zona_superior, string descripcion, TipoZona tipo_zona, 
                                                SqlGeography geoubicacion, string color_hxd, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_zona_patio, id_ubicacion_patio, id_zona_superior, descripcion, (byte)tipo_zona, geoubicacion, 
                               color_hxd, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Zonas de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_zona_superior">Zona Superior</param>
        /// <param name="descripcion">Descripción de la Zona</param>
        /// <param name="tipo_zona">Tipo de Zona (Andenes, Cajones)</param>
        /// <param name="geoubicacion">Geoubicación de la Zona</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaZonaPatio(int id_ubicacion_patio, int id_zona_superior, string descripcion, TipoZona tipo_zona,
                                                SqlGeography geoubicacion, string color_hxd, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_ubicacion_patio, id_zona_superior, descripcion, (byte)tipo_zona, geoubicacion, 
                               color_hxd, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Zonas de Patio
        /// </summary>
        /// <param name="id_ubicacion_patio">Ubicación del Patio</param>
        /// <param name="id_zona_superior">Zona Superior</param>
        /// <param name="descripcion">Descripción de la Zona</param>
        /// <param name="tipo_zona">Tipo de Zona (Andenes, Cajones)</param>
        /// <param name="geoubicacion">Geoubicación de la Zona</param>
        /// <param name="color_hxd">Color en Formato Hexadecimal</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaZonaPatio(int id_ubicacion_patio, int id_zona_superior, string descripcion, TipoZona tipo_zona,
                                                SqlGeography geoubicacion, string color_hxd, int id_usuario)
        {   //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(id_ubicacion_patio, id_zona_superior, descripcion, tipo_zona, geoubicacion,
                               color_hxd, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Zona de Patio
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaZonaPatio(int id_usuario)
        {   //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(this._id_ubicacion_patio, this._id_zona_superior, this._descripcion, (TipoZona)this._id_tipo_zona, this._geoubicacion,
                               this._color_hxd, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Obtener el Arreglo de Bytes de la Imagen de la Zona
        /// </summary>
        /// <param name="id_zona_patio"></param>
        /// <returns></returns>
        public static byte[] ObtieneImagenLayOut(int id_zona_patio)
        {   //Declarando Objeto de Retorno
            byte[] layout = null;
            //Declarando Variable que contendra la Ruta
            string ruta = "";
            //Armando Arreglo de Parametros
            object[] param = { 4, id_zona_patio, 0, 0, "", 0, null, "", 0, false, "", "" };
            //Obteniendo Resultado del Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        //Asignando Valores
                        ruta = dr["URL"].ToString();
                }
            }
            //Obteniendo LayOut
            layout = System.IO.File.ReadAllBytes(ruta);
            //Devolviendo Resultado Obtenido
            return layout;
        }
        /// <summary>
        /// Método Público encargado de Actualizar las Zonas de Patio
        /// </summary>
        /// <returns></returns>
        public bool ActualizaZonaPatio()
        {   //Invocando Método de Carga de Atributos
            return this.cargaAtributosInstancia(this._id_zona_patio);
        }
        /// <summary>
        /// Método Público encargado de Pintar las Entidades en la Zona del Patio
        /// </summary>
        /// <param name="id_zona_patio">Zona de Patio</param>
        /// <param name="ruta_entidad_carga_ok">Entidad de Carga en Tiempo</param>
        /// <param name="ruta_entidad_descarga_ok">Entidad de Descarga en Tiempo</param>
        /// <param name="ruta_entidad_ok">Entidad en Tiempo</param>
        /// <param name="ruta_entidad_carga_ex">Entidad de Carga en Tiempo Excedido</param>
        /// <param name="ruta_entidad_descarga_ex">Entidad de Descarga en Tiempo Excedido</param>
        /// <param name="ruta_entidad_ex">Entidad en Tiempo Excedido</param>
        /// <param name="alto">Alto de la Entidad</param>
        /// <param name="ancho">Ancho de la Entidad</param>
        /// <returns></returns>
        public static byte[] PintaLayOutZona(int id_zona_patio, string ruta_entidad_carga_ok, string ruta_entidad_descarga_ok, string ruta_entidad_ok,
                                    string ruta_entidad_carga_ex, string ruta_entidad_descarga_ex, string ruta_entidad_ex, int alto, int ancho)
        {   //Obteniendo Imagenes
            byte[] ent_car_ok = System.IO.File.ReadAllBytes(ruta_entidad_carga_ok),
                    ent_des_ok = System.IO.File.ReadAllBytes(ruta_entidad_descarga_ok),
                        ent_ok = System.IO.File.ReadAllBytes(ruta_entidad_ok),
                            ent_car_ex = System.IO.File.ReadAllBytes(ruta_entidad_carga_ex),
                                ent_des_ex = System.IO.File.ReadAllBytes(ruta_entidad_descarga_ex),
                                    ent_ex = System.IO.File.ReadAllBytes(ruta_entidad_ex);
            //Declarando Objeto de Retorno
            byte[] layout = ObtieneImagenLayOut(id_zona_patio);
            //Obteniendo Unidades en Patio
            using(DataTable dtEntidades = EntidadPatio.CargaEntidadesZona(id_zona_patio))
            {   //Validando que existan Entidades en Patio
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtEntidades))
                {   //Recorriendo cada Entidad
                    foreach(DataRow dr in dtEntidades.Rows)
                    {   //Obteniendo Propiedades de la Entidad
                        int x = Convert.ToInt32(dr["CoordenadaX"]),
                            y = Convert.ToInt32(dr["CoordenadaY"]),
                            icon = Convert.ToInt32(dr["TamanoIcono"]);
                        //Validando el Tipo de Evento
                        switch(Convert.ToInt32(dr["TipoEvt"]))
                        {   //Carga
                            case 1:
                                {   //Validando Estatus de Tiempo
                                    if(dr["EstatusTiempo"].ToString() == "EX")
                                        //Dibuja Entidad en LayOut
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_car_ex, x + (icon/2), y + (icon/2), ancho, alto);
                                    else
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_car_ok, x + (icon / 2), y + (icon / 2), ancho, alto);
                                    break;
                                }
                            //Descarga
                            case 2:
                                {   //Validando Estatus de Tiempo
                                    if (dr["EstatusTiempo"].ToString() == "EX")
                                        //Dibuja Entidad en LayOut
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_des_ex, x + (icon / 2), y + (icon / 2), ancho, alto);
                                    else
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_des_ok, x + (icon / 2), y + (icon / 2), ancho, alto);
                                    break;
                                }
                            //Estaciona
                            case 3:
                                {   //Validando Estatus de Tiempo
                                    if (dr["EstatusTiempo"].ToString() == "EX")
                                        //Dibuja Entidad en LayOut
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_ex, x + (icon / 2), y + (icon / 2), ancho, alto);
                                    else
                                        layout = TSDK.Base.Dibujo.DibujaImagen(layout, ent_ok, x + (icon / 2), y + (icon / 2), ancho, alto);
                                    break;
                                }
                        }
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return layout;
        }
        
        #endregion
    }
}
