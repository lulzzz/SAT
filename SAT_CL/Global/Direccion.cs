using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de Todas la Operaciones relacionadas con las Direcciones
    /// </summary>
    public class Direccion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_direccion_td";
        
        private int _id_direccion;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de la Dirección
        /// </summary>
        public int id_direccion { get { return this._id_direccion; } }
        private byte _id_tipo_direccion;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Dirección
        /// </summary>
        public byte id_tipo_direccion { get { return this._id_tipo_direccion; } }
        private string _calle;
        /// <summary>
        /// Atributo encargado de Almacenar la Calle
        /// </summary>
        public string calle { get { return this._calle; } }
        private string _no_exterior;
        /// <summary>
        /// Atributo encargado de Almacenar el Número Exterior
        /// </summary>
        public string no_exterior { get { return this._no_exterior; } }        
        private string _no_interior;
        /// <summary>
        /// Atributo encargado de Almacenar el Número Interior
        /// </summary>
        public string no_interior { get { return this._no_interior; } }        
        private string _colonia;
        /// <summary>
        /// Atributo encargado de Almacenar la Colonia
        /// </summary>
        public string colonia { get { return this._colonia; } }        
        private string _localidad;
        /// <summary>
        /// Atributo encargado de Almacenar la Localidad
        /// </summary>
        public string localidad { get { return this._localidad; } }        
        private string _municipio;
        /// <summary>
        /// Atributo encargado de Almacenar el Municipio
        /// </summary>
        public string municipio { get { return this._municipio; } }        
        private string _referencia;
        /// <summary>
        /// Atributo encargado de Almacenar la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private int _id_estado;
        /// <summary>
        /// Describe el Id estado
        /// </summary>
        public int id_estado
        {
            get { return _id_estado; }
        }
        private string _nombre_estado;
        /// <summary>
        /// Obtiene el Nombre del estado al que pertenece la dirección
        /// </summary>
        public string nombre_estado { get { return this._nombre_estado; } }
        private int _id_pais;
        /// <summary>
        /// Describe el Id Pais
        /// </summary>
        public int id_pais
        {
            get { return _id_pais; }
        }
        private string _nombre_pais;
        /// <summary>
        /// Obtiene el nombre del país al que pertenece la dirección
        /// </summary>
        public string nombre_pais { get { return this._nombre_pais; } }
        private string _codigo_postal;
        /// <summary>
        /// Atributo encargado de Almacenar el Código Postal
        /// </summary>
        public string codigo_postal { get { return this._codigo_postal; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Direccion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public Direccion(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Direccion()
        {
            Dispose();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Atributos
            this._id_direccion = 0;
            this._id_tipo_direccion = 0;
            this._calle = "";
            this._no_exterior = "";
            this._no_interior = "";
            this._colonia = "";
            this._localidad = "";
            this._municipio = "";
            this._referencia = "";
            this._id_estado = 0;
            this._nombre_estado = "";
            this._id_pais = 0;
            this._nombre_pais = "";
            this._codigo_postal = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, "", "", "", "", "", "", "", 0, 0, "", 0, false, "", "" };
            //Instanciando Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando que existan registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Atributos
                        this._id_direccion = id_registro;
                        this._id_tipo_direccion = Convert.ToByte(dr["IdTipoDireccion"]);
                        this._calle = dr["Calle"].ToString();
                        this._no_exterior = dr["NoExterior"].ToString();
                        this._no_interior = dr["NoInterior"].ToString();
                        this._colonia = dr["Colonia"].ToString();
                        this._localidad = dr["Localidad"].ToString();
                        this._municipio = dr["Municipio"].ToString(); ;
                        this._referencia = dr["Referencia"].ToString();
                        this._id_estado = Convert.ToInt32(dr["IdEstado"]);
                        this._nombre_estado = dr["Estado"].ToString();
                        this._id_pais = Convert.ToInt32(dr["IdPais"]);
                        this._nombre_pais = dr["Pais"].ToString();
                        this._codigo_postal = dr["CodigoPostal"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_tipo_direccion">Tipo de Dirección</param>
        /// <param name="calle">Calle</param>
        /// <param name="no_exterior">Número Exterior</param>
        /// <param name="no_interior">Número Interior</param>
        /// <param name="colonia">Colonia</param>
        /// <param name="localidad">Localidad</param>
        /// <param name="municipio">Municipio</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_estado">Id de Estado</param>
        /// <param name="id_pais">Id de Pais</param>
        /// <param name="codigo_postal">Código Postal</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(byte id_tipo_direccion, string calle, string no_exterior, string no_interior, string colonia, 
                                                    string localidad, string municipio, string referencia, int id_estado, int id_pais, 
                                                    string codigo_postal, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_direccion, id_tipo_direccion, calle, no_exterior, no_interior, colonia, localidad, 
	                            municipio, referencia, id_estado, id_pais, codigo_postal, id_usuario, habilitar, "", "" };
            
            //Validando que no tenga CFDI Timbrados
            if (!validaDireccionCFDI())

                //Obteniendo Resultado del SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Existen CFDI's Timbrados con esta Dirección, Imposible su Actualización");
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar que la Dirección no contenga un CFDI Timbrado
        /// </summary>
        /// <returns></returns>
        private bool validaDireccionCFDI()
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Objeto de Parametros
            object[] param = { 5, this._id_direccion, 0, "", "", "", "", "", "", "", 0, 0, "", 0, false, "", "" };

            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Obteniendo Resultado
                        result = Convert.ToInt32(dr["Validacion"]) == 1 ? true : false;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Direcciones
        /// </summary>
        /// <param name="id_tipo_direccion">Tipo de Dirección</param>
        /// <param name="calle">Calle</param>
        /// <param name="no_exterior">Número Exterior</param>
        /// <param name="no_interior">Número Interior</param>
        /// <param name="colonia">Colonia</param>
        /// <param name="localidad">Localidad</param>
        /// <param name="municipio">Municipio</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_estado">Id de Estado</param>
        /// <param name="id_pais">Id de Pais</param>
        /// <param name="codigo_postal">Código Postal</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDireccion(byte id_tipo_direccion, string calle, string no_exterior, string no_interior, string colonia, 
                                                    string localidad, string municipio, string referencia, int id_estado, int id_pais, 
                                                    string codigo_postal, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, id_tipo_direccion, calle, no_exterior, no_interior, colonia, localidad, 
	                            municipio, referencia, id_estado, id_pais, codigo_postal, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Direcciones
        /// </summary>
        /// <param name="id_tipo_direccion">Tipo de Dirección</param>
        /// <param name="calle">Calle</param>
        /// <param name="no_exterior">Número Exterior</param>
        /// <param name="no_interior">Número Interior</param>
        /// <param name="colonia">Colonia</param>
        /// <param name="localidad">Localidad</param>
        /// <param name="municipio">Municipio</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_estado">Id de Estado</param>
        /// <param name="id_pais">Id de Pais</param>
        /// <param name="codigo_postal">Código Postal</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDireccion(byte id_tipo_direccion, string calle, string no_exterior, string no_interior, string colonia,
                                                string localidad, string municipio, string referencia, int id_estado, int id_pais,
                                                string codigo_postal, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tipo_direccion, calle, no_exterior, no_interior, colonia, localidad,
                                municipio, referencia, id_estado, id_pais, codigo_postal, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar las Direcciones
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDireccion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_direccion, this._calle, this._no_exterior, this._no_interior, this._colonia, this._localidad,
                                this._municipio, this._referencia, this._id_estado, this._id_pais, this._codigo_postal, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDireccion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_direccion);
        }
        /// <summary>
        /// Devuelve la dirección de la dirección en un formato legíble
        /// </summary>
        /// <returns></returns>
        public string ObtieneDireccionCompleta()
        {
            //Declaramos Variable
            string direccion = "";
            //Validamos que exista objeto
            if (this._id_direccion > 0)
            {
                //Armando y devolviendo la dirección con todos los elementos de la ubicación
                direccion = string.Format("{0}{1}{2}{3} Del./Mpio. {4}, {5} [{6}] {7}{8}", this._calle,
                                    Cadena.ConcatenaCadenas(this._no_exterior.ToString(), " Ext.", " ", true),
                                    Cadena.ConcatenaCadenas(this._no_interior.ToString(), " Int.", " ", true),
                                    Cadena.ConcatenaCadenas(this._colonia, ", Col. ", " ", true),
                                    Cadena.ConcatenaCadenas(this._localidad, " ", "", true),
                                    this._municipio, this._nombre_estado, this._nombre_pais,
                                    Cadena.ConcatenaCadenas(this._codigo_postal.ToString()," C.P.:"," ",true),
                                    this._referencia != "" ? " (" + this._referencia + ")" : "").ToUpper();
            }
            //Devolvemos valor
            return direccion;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Direcciones en base a los filtros de busqueda
        /// </summary>
        /// <param name="id_tipo_direccion">Tipo de Dirección</param>
        /// <param name="calle">Calle</param>
        /// <param name="no_exterior">Número Exterior</param>
        /// <param name="no_interior">Número Interior</param>
        /// <param name="colonia">Colonia</param>
        /// <param name="localidad">Localidad</param>
        /// <param name="municipio">Municipio</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_estado">Id de Estado</param>
        /// <param name="id_pais">Id de Pais</param>
        /// <param name="codigo_postal">Código Postal</param>
        /// <returns></returns>
        public static DataTable ObtieneDirecciones(byte id_tipo_direccion, string calle, string no_exterior, string no_interior, string colonia,
                                                string localidad, string municipio, string referencia, int id_estado, int id_pais,
                                                string codigo_postal)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando Objeto de Parametros
            object[] param = { 4, 0, id_tipo_direccion, calle, no_exterior, no_interior, colonia, localidad, 
	                            municipio, referencia, id_estado, id_pais, codigo_postal, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Objeto de Retorno
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método encargado de Obtener los Valores en Catalogo del Pais y el Estado
        /// </summary>
        /// <param name="opcion_pais"></param>
        /// <param name="opcion_edo"></param>
        /// <param name="id_pais"></param>
        /// <param name="id_estado"></param>
        public static void ObtienePaisEstado(string opcion_pais, string opcion_edo, out int id_pais, out int id_estado)
        {
            //Asignando Valores
            id_pais = id_estado = 0;

            //Normalizando Pais y Estado
            string pais = Cadena.RegresaCadenaNormalizada(opcion_pais).ToUpper();
            string estado = Cadena.RegresaCadenaNormalizada(opcion_edo).ToUpper();

            //Validando la Opción Inicial del Pais
            switch (pais.ToUpper())
            {
                //México
                case "MEX":
                case "MÉX":
                case "MÉXICO":
                case "MEXICO":
                case "Mexico":
                    {
                        //Asignando Pais
                        id_pais = 1;
                        break;
                    }

                //Estados Unidos
                case "USA":
                case "EUA":
                case "ESTADOS UNIDOS":
                    {
                        //Asignando Pais
                        id_pais = 2;
                        break;
                    }

                default:
                    {
                        //Asignando Pais
                        id_pais = 0;
                        break;
                    }
            }

            //Validando la Opción Inicial del Estado
            switch (estado.ToUpper())
            {
                case "Aguascalientes":
                case "Ags.":
                case "AGUASCALIENTES":
                case "AGS.":
                case "AG":
                case "AGU":
                    {
                        //Asignando Estado
                        id_estado = 1;
                        break;
                    }
                case "Baja California":
                case "BAJA CALIFORNIA":
                case "B.C.":
                case "BC":
                case "BCN":
                    {
                        //Asignando Estado
                        id_estado = 2;
                        break;
                    }
                case "Baja California Sur":
                case "BAJA CALIFORNIA SUR":
                case "B.C.S.":
                case "BS":
                case "BCS":
                    {
                        //Asignando Estado
                        id_estado = 3;
                        break;
                    }
                case "Campeche":
                case "Camp.":
                case "CAMPECHE":
                case "CAMP.":
                case "CM":
                case "CAM":
                    {
                        //Asignando Estado
                        id_estado = 4;
                        break;
                    }
                case "Chiapas":
                case "Chis.":
                case "CHIAPAS":
                case "CHIS.":
                case "CS":
                case "CHP":
                    {
                        //Asignando Estado
                        id_estado = 5;
                        break;
                    }
                case "Chihuahua":
                case "Chih.":
                case "CHIHUAHUA":
                case "CHIH.":
                case "CH":
                case "CHH":
                    {
                        //Asignando Estado
                        id_estado = 6;
                        break;
                    }
                case "Coahuila":
                case "Coah.":
                case "COAHUILA":
                case "COAH.":
                case "CO":
                case "COA":
                    {
                        //Asignando Estado
                        id_estado = 7;
                        break;
                    }
                case "Colima":
                case "Col.":
                case "COLIMA":
                case "COL.":
                case "CL":
                case "COL":
                    {
                        //Asignando Estado
                        id_estado = 8;
                        break;
                    }
                case "Distrito Federal":
                case "DISTRITO FEDERAL":
                case "D.F.":
                case "DF":
                case "DIF":
                    {
                        //Asignando Estado
                        id_estado = 9;
                        break;
                    }
                case "Durango":
                case "Dgo.":
                case "DURANGO":
                case "DGO.":
                case "DG":
                case "DUR":
                    {
                        //Asignando Estado
                        id_estado = 10;
                        break;
                    }
                case "EDO MEX":
                case "Méx.":
                case "ESTADO DE MÉXICO":
                case "MEX.":
                case "ME":
                case "Estado de mexico":
                case "Edo. de Mex.":
                    {
                        //Asignando Estado
                        id_estado = 11;
                        break;
                    }
                case "Guanajuato":
                case "Gto.":
                case "GUANAJUATO":
                case "GTO.":
                case "GT":
                case "GUA":
                    {
                        //Asignando Estado
                        id_estado = 12;
                        break;
                    }
                case "Guerrero":
                case "Gro.":
                case "GUERRERO":
                case "GRO.":
                case "GR":
                case "GRO":
                    {
                        //Asignando Estado
                        id_estado = 13;
                        break;
                    }
                case "Hidalgo":
                case "Hgo.":
                case "HIDALGO":
                case "HGO.":
                case "HG":
                case "HID":
                    {
                        //Asignando Estado
                        id_estado = 14;
                        break;
                    }
                case "Jalisco":
                case "Jal.":
                case "JALISCO":
                case "JAL.":
                case "JC":
                case "JAL":
                    {
                        //Asignando Estado
                        id_estado = 15;
                        break;
                    }
                
                case "Michoacán":
                case "Mich.":
                case "MICHOACÁN":
                case "MICH.":
                case "MN":
                case "MIC":
                    {
                        //Asignando Estado
                        id_estado = 16;
                        break;
                    }
                case "Morelos":
                case "Mor.":
                case "MORELOS":
                case "MOR.":
                case "MS":
                case "MOR":
                    {
                        //Asignando Estado
                        id_estado = 17;
                        break;
                    }
                case "Nayarit":
                case "Nay.":
                case "NAYARIT":
                case "NAY.":
                case "NT":
                case "NAY":
                    {
                        //Asignando Estado
                        id_estado = 18;
                        break;
                    }
                case "Nuevo León":
                case "NUEVO LEÓN":
                case "N.L.":
                case "NL":
                case "NLE":
                    {
                        //Asignando Estado
                        id_estado = 19;
                        break;
                    }
                case "Oaxaca":
                case "Oax.":
                case "OAXACA":
                case "OAX.":
                case "OC":
                case "OAX":
                    {
                        //Asignando Estado
                        id_estado = 20;
                        break;
                    }
                case "Puebla":
                case "Pue.":
                case "PUEBLA":
                case "PUE.":
                case "PL":
                case "PUE":
                    {
                        //Asignando Estado
                        id_estado = 21;
                        break;
                    }
                case "Querétaro":
                case "Qro.":
                case "QUERÉTARO":
                case "QRO.":
                case "QO":
                case "QTO":
                    {
                        //Asignando Estado
                        id_estado = 22;
                        break;
                    }
                case "Quintana Roo":
                case "Q. Roo.":
                case "QUINTANA ROO":
                case "Q. ROO.":
                case "QR":
                case "ROO":
                    {
                        //Asignando Estado
                        id_estado = 23;
                        break;
                    }
                case "San Luis Potosí":
                case "SAN LUIS POTOSÍ":
                case "S.L.P.":
                case "SP":
                case "SLP":
                    {
                        //Asignando Estado
                        id_estado = 24;
                        break;
                    }
                case "Sinaloa":
                case "Sin.":
                case "SINALOA":
                case "SIN.":
                case "SL":
                case "SIN":
                    {
                        //Asignando Estado
                        id_estado = 25;
                        break;
                    }
                case "Sonora":
                case "Son.":
                case "SONORA":
                case "SON.":
                case "SR":
                case "SON":
                    {
                        //Asignando Estado
                        id_estado = 26;
                        break;
                    }
                case "Tabasco":
                case "Tab.":
                case "TABASCO":
                case "TAB.":
                case "TC":
                case "TAB":
                    {
                        //Asignando Estado
                        id_estado = 27;
                        break;
                    }
                case "Tamaulipas":
                case "Tamps.":
                case "TAMAULIPAS":
                case "TAMPS.":
                case "TS":
                case "TAM":
                    {
                        //Asignando Estado
                        id_estado = 28;
                        break;
                    }
                case "Tlaxcala":
                case "Tlax.":
                case "TLAXCALA":
                case "TLAX.":
                case "TL":
                case "TLA":
                    {
                        //Asignando Estado
                        id_estado = 29;
                        break;
                    }
                case "Veracruz":
                case "Ver.":
                case "VERACRUZ":
                case "VER.":
                case "VZ":
                case "VER":
                    {
                        //Asignando Estado
                        id_estado = 30;
                        break;
                    }
                case "Yucatán":
                case "Yuc.":
                case "YUCATÁN":
                case "YUC.":
                case "YN":
                case "YUC":
                    {
                        //Asignando Estado
                        id_estado = 31;
                        break;
                    }
                case "Zacatecas":
                case "Zac.":
                case "ZACATECAS":
                case "ZAC.":
                case "ZS":
                case "ZAC":
                    {
                        //Asignando Estado
                        id_estado = 32;
                        break;
                    }
                default:
                    {
                        //Asignando Estado
                        id_estado = 0;
                        break;
                    }
            }
        }


        #endregion
    }
}
