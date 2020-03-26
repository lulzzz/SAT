using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.ControlPatio;
using SAT_CL.Global;
using System.Transactions;
using System.IO;


namespace SAT_WCF
{
    /// <summary>
    /// Servicio que porporciona los medios para interacción de aplicaciones móviles con la plataforma principal SAT.
    /// </summary>
    public class MobileService : IMobileService
    {
        /// <summary>
        /// Realiza las validaciones necesarias sobre la cuenta de usuario indicada y permite el acceso remoto a la plataforma.
        /// </summary>
        /// <param name="email">Email registrado en cuenta de usuario activa</param>
        /// <param name="contrasena">Contraseña asignada por el usuario para su inicio de sesión</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        public string AutenticaUsuario(string email, string contrasena)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado;

            //Validando conjunto de datos requeridos
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contrasena))
                //Instanciando usuario
                using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(email))
                {
                    //Realizando autenticación de usuario solicitado
                    resultado = u.AutenticaUsuario(contrasena);
                }
            else
                resultado = new RetornoOperacion(string.Format("{0} {1}", string.IsNullOrEmpty(email) ? "Falta email." : "", string.IsNullOrEmpty(contrasena) ? "Falta contraseña." : ""));

            //Devolvemos Resultado
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Realiza el firmado del usuario sobre una compañía en particular
        /// </summary>
        /// <param name="id_usuario">Id de Usuario Autenticado</param>
        /// <param name="id_compania">Id de Compañía donde se firmará el usuario</param>
        /// <param name="tipo_dispositivo">Tipo de dispositivo desde donde se accesa (Consultar TipoDispositivo en contrato de servicio)</param>
        /// <param name="nombre_dispositivo">Nombre o alias del dispositivo</param>
        /// <param name="direccion_ip_mac">Dirección ipV6 o MAC del dispositivo</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        public string IniciaSesion(int id_usuario, int id_compania, string tipo_dispositivo, string nombre_dispositivo, string direccion_ip_mac)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion("No fueron proporcionados todos los valores de parámetros requeridos.");

            //Validando conjunto de datos requeridos
            if (id_usuario > 0 && id_compania > 0 && !string.IsNullOrEmpty(tipo_dispositivo) && !string.IsNullOrEmpty(nombre_dispositivo) && !string.IsNullOrEmpty(direccion_ip_mac))
                //Insertamos Sesión del Usuario
                resultado = SAT_CL.Seguridad.UsuarioSesion.IniciaSesion(id_usuario, id_compania, (SAT_CL.Seguridad.UsuarioSesion.TipoDispositivo)((byte)convierteCadenaTipoDispositivo(tipo_dispositivo)), direccion_ip_mac, nombre_dispositivo, id_usuario);
            else
                resultado = new RetornoOperacion(string.Format("{0} {1} {2} {3} {4}.", id_usuario < 1 ? "Falta id_usuario." : "", id_compania < 1 ? "Falta id_compania." : "",
                                    string.IsNullOrEmpty(tipo_dispositivo) ? "Falta tipo_dispositivo." : "", string.IsNullOrEmpty(nombre_dispositivo) ? "Falta nombre_dispositivo." : "",
                                    string.IsNullOrEmpty(direccion_ip_mac) ? "Falta direccion_ip_mac." : ""));

            //Devolviendo resultado
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Realiza la conversión de un tipo de dispositivo en cadena a valor de enumeración
        /// </summary>
        /// <param name="tipo_dispositivo"></param>
        /// <returns></returns>
        private TipoDispositivo convierteCadenaTipoDispositivo(string tipo_dispositivo)
        {
            //Declarando objeto de retorno
            TipoDispositivo t = TipoDispositivo.Desconocido;

            //Determinando el valor de entrada
            switch (tipo_dispositivo)
            {
                case "Escritorio":
                    t = TipoDispositivo.Escritorio;
                    break;
                case "Portatil":
                    t = TipoDispositivo.Portatil;
                    break;
                case "Android":
                    t = TipoDispositivo.Android;
                    break;
            }

            //Devolviendo resultado
            return t;
        }
        /// <summary>
        /// Obtiene las compañías a las que está adscrito el usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string ObtieneCompaniasUsuario(int id_usuario)
        {
            //Declarando objeto de resultado
            string resultado = "";

            //Si el usuario fue proporcionado
            if (id_usuario > 0)
            {
                //Creando flujo de memoria
                using (System.IO.Stream s = new System.IO.MemoryStream())
                {
                    //Obteniendo conjunto de compañía
                    using (DataTable mit = SAT_CL.Seguridad.UsuarioCompania.ObtieneCompaniasUsuario(id_usuario))
                    {
                        //Realizando filtrado de columnas
                        using (DataTable mitCopia = OrigenDatos.CopiaDataTableFiltrandoColumnas(mit, "Table", false, "IdCompaniaEmisorReceptor", "NombreCorto"))
                        {
                            //Validando que existan registros
                            if (Validacion.ValidaOrigenDatos(mitCopia))
                            {
                                //Leyendo flujo de datos XML
                                mitCopia.WriteXml(s);
                                //Convirtiendo el flujo a una cadena de caracteres xml
                                resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogo(int id_catalogo, string opcion_inicial)
        {
            //Invocando método base
            return CargaCatalogoParametros(id_catalogo, opcion_inicial, 0, "", 0, "");
        }
        /// <summary>
        /// Obtiene el catálogo solicitado a partir de su id y los criterios deseados
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogoParametros(int id_catalogo, string opcion_inicial, int param1, string param2, int param3, string param4)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(id_catalogo, opcion_inicial, param1, param2, param3, param4))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el catálogo clasificado como general, solicitado a partir de su id
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string CargaCatalogoGeneral(int id_catalogo, string opcion_inicial)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(1, opcion_inicial, id_catalogo, 0, 0, 0, "", 0, ""))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el catálogo clasificado como general, solicitado a partir de su id y un valor de un catálogo superior
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo a consultar</param>
        /// <param name="opcion_inicial">Primer elemento del listado devuelto (no incluido en el catálogo original). Si es una cadena vacía, sólo se obtendrá el catálogo</param>
        /// <param name="valor_superior">Valor del catálogo superior</param>
        /// <returns></returns>
        public string CargaCatalogoGeneralParametros(int id_catalogo, string opcion_inicial, int valor_superior)
        {
            //Declarando objeto de retorno
            string resultado = "";

            //Validando que el parámetro id_catalogo se haya especificado, para evitar consultas no necesarias
            if (id_catalogo > 0)
            {
                //Consultando catálogo solicitado
                using (DataTable mit = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(2, opcion_inicial, id_catalogo, 0, valor_superior, 0, "", 0, ""))
                {
                    //Validando que existan registros
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            mit.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Cargar las Posibles Sugerencias
        /// </summary>
        /// <param name="contextKey">Indica el Catalogo a Obtener</param>
        /// <param name="prefix">Contiene el Indicio de las Opciones a Elegir</param>
        /// <param name="complement1">Indica el Primer complemento en caso de requerirse</param>
        /// <param name="complement2">Indica el Segundo complemento en caso de requerirse</param>
        /// <param name="complement3">Indica el Tercer complemento en caso de requerirse</param>
        /// <returns></returns>
        public string CargaConsultaSugerencia(int contextKey, string prefix, string complement1, string complement2, string complement3)
        {
            //Declarando Objeto de Retorno
            string resultado = "";

            //Declarando Tabla de Sugerencias
            using (DataTable dtSugerencias = new DataTable("Table"))
            {
                //Creando Columnas
                dtSugerencias.Columns.Add("id", typeof(int));
                dtSugerencias.Columns.Add("descripcion", typeof(string));

                //Declarando Arreglo de Parametros
                object[] param = { contextKey, prefix, complement1, complement2, complement3 };

                //Ejecutando Consulta
                using (DataSet ds = SAT_CL.CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaConsultaSugerencia", param))
                {
                    //Validando que exista una tabla
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo cada fila
                        foreach (DataRow dr in ds.Tables["Table"].Rows)

                            //Añadiendo el Registro a la Tabla
                            dtSugerencias.Rows.Add(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(dr["descripcion"].ToString(), "ID:", 1)),
                                                    TSDK.Base.Cadena.RegresaCadenaSeparada(dr["descripcion"].ToString(), "ID:", 0));

                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            dtSugerencias.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el registro de un encabezado de acceso y sus detalles (entidades que accesan)
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <param name="id_acceso">Id de Acceso del Patio (elemento de patio)</param>
        /// <param name="fecha_hora">Fecha y hora de acceso en formato yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="entidades_xml">Conjunto de entidades que accesan al patio. Validar contra esquema correspondiente</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public string RegistraEntradaPatio(int id_patio, int id_acceso, string fecha_hora, string entidades_xml, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(fecha_hora, out fecha);

            //Validando que el Patio exista
            if (id_patio > 0)
            {
                //Validando que el Acceso exista
                if (id_acceso > 0)
                {
                    //Validando que exista la Fecha
                    if (fecha != DateTime.MinValue)
                    {
                        try
                        {   //Declarando Objeto de Entidades
                            XDocument documento = XDocument.Parse(entidades_xml);

                            //Validando si tiene Entidades
                            if (documento.Root.HasElements)
                            {
                                //Inicializando Transaccion
                                using (TransactionScope transaccion = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Obteniendo Entidades
                                    IEnumerable<XElement> entidades = documento.Root.Elements();

                                    //Insertando Acceso
                                    resultado = AccesoPatio.InsertaAccesoPatio(id_patio, id_acceso, AccesoPatio.TipoActualizacion.Mobil,
                                                                AccesoPatio.TipoAcceso.Entrada, fecha, "", id_usuario);

                                    //Validando que el Acceso se Insertara correctamente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Obteniendo Acceso de Entrada
                                        int idAccesoEntrada = resultado.IdRegistro;

                                        //Recorriendo Ciclo
                                        foreach (XElement entidad in entidades)
                                        {
                                            //Insertando Detalle de Acceso
                                            resultado = DetalleAccesoPatio.InsertaDetalleAccesoPatio(idAccesoEntrada, 0, Convert.ToInt32(entidad.Attribute("idTransportista").Value),
                                                0, 0, Convert.ToBoolean(entidad.Attribute("cargado").Value), fecha, Convert.ToByte(entidad.Attribute("idTipo").Value),
                                                entidad.Attribute("descripcion").Value, entidad.Attribute("identificacion").Value, 0, id_usuario);

                                            //Validando si se Inserto el Detalle
                                            if (!resultado.OperacionExitosa)
                                                break;
                                        }

                                        //Validando que la Operación fuese Exitosa
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciando Acceso de Patio
                                            resultado = new RetornoOperacion(idAccesoEntrada);

                                            //Completando Transacción
                                            transaccion.Complete();
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            //Devolviendo Excepcion
                            resultado = new RetornoOperacion(e.Message);
                        }
                    }
                    else//Instanciando Excepción
                        resultado = new RetornoOperacion("La Fecha es invalida");
                }
                else//Instanciando Excepción
                    resultado = new RetornoOperacion("El Acceso es invalido");
            }
            else//Instanciando Excepción
                resultado = new RetornoOperacion("El Patio es invalido");

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Obtiene el Reporte de las Unidades que se encuentran en Patio
        /// </summary>
        /// <param name="descripcion">Descripción de la Unidad</param>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public string ObtieneUnidadesDentro(string descripcion, int id_patio)
        {
            //Declarando Objeto de Retorno
            string resultado = "";

            //Declarando Tabla de Sugerencias
            using (DataTable dtUnidades = new DataTable("Table"))
            {
                //Creando Columnas
                dtUnidades.Columns.Add("id", typeof(int));
                dtUnidades.Columns.Add("detalleUnidad", typeof(string));
                dtUnidades.Columns.Add("identificador", typeof(string));
                dtUnidades.Columns.Add("transportista", typeof(string));
                dtUnidades.Columns.Add("fechaEntrada", typeof(string));
                dtUnidades.Columns.Add("estado", typeof(string));

                //Obteniendo Reporte
                using (DataTable dt = SAT_CL.ControlPatio.Reporte.ObtieneUnidadesDentroMovil(descripcion, id_patio))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                    {
                        //Recorriendo Ciclo
                        foreach (DataRow dr in dt.Rows)
                        {
                            //Obteniendo Fecha
                            DateTime fec_ent;
                            DateTime.TryParse(dr["FechaEntrada"].ToString(), out fec_ent);

                            //Añadiendo Fila a la Tabla
                            dtUnidades.Rows.Add(Convert.ToInt32(dr["Id"]), dr["DetalleUnidad"].ToString() + " - " + dr["Tipo"].ToString(), dr["Identificador"].ToString(), dr["Transportista"].ToString(),
                                                fec_ent.ToString(fec_ent.Date.CompareTo(DateTime.Today) == 0 ? "HH:mm" : "dd MMM HH:mm", System.Globalization.CultureInfo.CreateSpecificCulture("es-MX")),
                                                dr["Estado"].ToString());
                        }

                        //Creando flujo de memoria
                        using (System.IO.Stream s = new System.IO.MemoryStream())
                        {
                            //Leyendo flujo de datos XML
                            dtUnidades.WriteXml(s);
                            //Convirtiendo el flujo a una cadena de caracteres xml
                            resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                        }
                    }
                }
            }

            //Devolviendo Objeto de Retorno
            return resultado;
        }
        /// <summary>
        /// Realiza el registro de un encabezado de acceso y actualizar sus detalles (entidades que salen)
        /// </summary>
        /// <param name="id_patio">Id de Patio</param>
        /// <param name="id_acceso">Id de Acceso del Patio (elemento de patio)</param>
        /// <param name="fecha_hora">Fecha y hora de salida en formato yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="entidades_xml">Conjunto de entidades que accesan al patio. Validar contra esquema correspondiente</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public string RegistraSalidaPatio(int id_patio, int id_acceso, string fecha_hora, string entidades_xml, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Validando que exista el Patio
            if (id_patio > 0)
            {
                //Validando que exista el Acceso
                if (id_acceso > 0)
                {
                    //Obteniendo Fecha y Hora
                    DateTime fec_sal;
                    DateTime.TryParse(fecha_hora, out fec_sal);

                    //Validando que exista la Fecha
                    if (fec_sal != DateTime.MinValue)
                    {
                        try
                        {   //Declarando Objeto de Entidades
                            XDocument documento = XDocument.Parse(entidades_xml);

                            //Validando si tiene Entidades
                            if (documento.Root.HasElements)
                            {
                                //Inicializando Transaccion
                                using (TransactionScope transaccion = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Obteniendo Entidades
                                    IEnumerable<XElement> entidades = documento.Root.Elements();

                                    //Insertando Acceso
                                    resultado = AccesoPatio.InsertaAccesoPatio(id_patio, id_acceso, AccesoPatio.TipoActualizacion.Mobil,
                                                                AccesoPatio.TipoAcceso.Salida, fec_sal, "", id_usuario);

                                    //Validando que el Acceso se Insertara correctamente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Obteniendo Acceso de Entrada
                                        int idAccesoSalida = resultado.IdRegistro;

                                        //Recorriendo Ciclo
                                        foreach (XElement entidad in entidades)
                                        {
                                            //Instanciando Detalle de Acceso
                                            using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(entidad.Value)))
                                            {
                                                //Validando que exista el Detalle
                                                if (dap.id_detalle_acceso_patio > 0)
                                                {
                                                    //Actualizando Salida del Detalle 
                                                    resultado = dap.ActualizaSalidaDetalleMobil(idAccesoSalida, id_usuario);

                                                    //Validando si se Inserto el Detalle
                                                    if (!resultado.OperacionExitosa)
                                                        break;
                                                }
                                                else
                                                    break;
                                            }
                                        }

                                        //Validando que el Acceso se Insertara correctamente
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Completando Transacción
                                            transaccion.Complete();

                                            //Instanciando Resultado de Acceso de Salida
                                            resultado = new RetornoOperacion(idAccesoSalida);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //Instanciando Excepción
                            resultado = new RetornoOperacion(e.Message);
                        }
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("La Fecha es Invalida");
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("El Acceso es Invalido");
            }
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion("El Patio es Invalido");

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método Público encargado de Obtener la Instancia por Defecto del Usuario Y Compania
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public string ObtienePatioDefaultUsuario(int id_usuario, int id_compania)
        {
            //Obteniendo Instancia de Usuario/Patio
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(id_usuario, id_compania))
            {
                //Declarando documento xml
                System.Xml.Linq.XDocument d = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "true"));

                //Creando elemento raíz
                System.Xml.Linq.XElement e = new System.Xml.Linq.XElement("UsuarioPatio");

                //Añadiendo atributos
                e.Add(new System.Xml.Linq.XElement("idPatio", up.id_patio));
                e.Add(new System.Xml.Linq.XElement("idAccesoDefault", up.id_acceso_default));

                //Añadiendo elemento raíz a documento
                d.Add(e);

                //Realizando Conversión de Objeto a XML
                return d.ToString();
            }
        }
        /// <summary>
        /// Metodo que regresa los indicadores relacionados con las unidades en patio
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public string ObtieneIndicadoresAccesoPatio(int id_patio)
        {
            //Declarando Objeto de Retorno
            string resultado = "";

            //Obteniendo Indicadores
            using (DataTable dtIndicadores = DetalleAccesoPatio.retornaIndicadoresUnidades(id_patio))
            {
                //Creando flujo de memoria
                using (System.IO.Stream s = new System.IO.MemoryStream())
                {
                    //Leyendo flujo de datos XML
                    dtIndicadores.WriteXml(s);

                    //Convirtiendo el flujo a una cadena de caracteres xml
                    resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                }
            }

            //Devolviendo Resultado Obtenido
            return resultado;

        }
        /// <summary>
        /// Método Público encargado de Obtener las Unidades con Evento Pendientes de Confirmación
        /// </summary>
        /// <param name="id_patio">Ubicación de Patio</param>
        /// <returns></returns>
        public string ObtieneUnidadesEventosPendientes(int id_patio)
        {
            //Declarando Objeto de Retorno
            string resultado = "";

            //Obteniendo Unidades con Eventos Pendientes
            using (DataTable dtUnidades = DetalleAccesoPatio.ObtieneUnidadesEventoPendientes(id_patio))
            {
                //Creando flujo de memoria
                using (System.IO.Stream s = new System.IO.MemoryStream())
                {
                    //Leyendo flujo de datos XML
                    dtUnidades.WriteXml(s);

                    //Convirtiendo el flujo a una cadena de caracteres xml
                    resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                }
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Eventos dado un Detalle de Acceso
        /// </summary>
        /// <param name="id_detalle_acceso">Detalle de Acceso</param>
        /// <returns></returns>
        public string ObtieneEventosDetalleAcceso(int id_detalle_acceso)
        {
            //Declarando Objeto de Retorno
            string resultado = "";

            //Obteniendo Eventos por Detalle de Acceso
            using (DataTable dtEventos = EventoDetalleAcceso.ObtieneEventosPorDetalleInstruccion(id_detalle_acceso))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventos))
                {
                    //Creando flujo de memoria
                    using (System.IO.Stream s = new System.IO.MemoryStream())
                    {
                        //Leyendo flujo de datos XML
                        dtEventos.WriteXml(s);

                        //Convirtiendo el flujo a una cadena de caracteres xml
                        resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Confirmar el Evento
        /// </summary>
        /// <param name="id_evento">Id de Evento</param>
        /// <param name="tipo">Tipo de Confirmacion</param>
        /// <param name="fecha">Fecha de Actualización</param>
        /// <param name="id_usuario">Usuario que Actualiza el Evento</param>
        /// <returns></returns>
        public string ActualizaConfirmacionEvento(int id_evento, byte tipo, string fecha, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Obteniendo Fecha
            DateTime fec = DateTime.MinValue;
            DateTime.TryParse(fecha, out fec);

            //Instanciando Evento
            using (EventoDetalleAcceso eda = new EventoDetalleAcceso(id_evento))
            {
                //Validando que exista el Evento
                if (eda.id_evento_detalle_acceso > 0)

                    //Invocando Método de Actualización
                    resultado = eda.ActualizaConfirmacionEvento((EventoDetalleAcceso.TipoConfirmacion)tipo, fec, id_usuario);
            }

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método Público encaragdo de Guardar los Archivos de cada Registro
        /// </summary>
        /// <param name="id_registro">Parametro que hace Referencia al Registro</param>
        /// <param name="id_tabla">Parametro que hace Referencia a la Tabla</param>
        /// <param name="id_compania">Compañia Emisora</param>
        /// <param name="id_archivo_tipo">Tipo de Archivo</param>
        /// <param name="referencia">Referencia del Archivo (Nombre)</param>
        /// <param name="archivo">Archivo en Formato Base 64</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string GuardaArchivoRegistro(int id_registro, int id_tabla, int id_compania, int id_archivo_tipo, string referencia, string archivo, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = null;

            //Instanciando el Tipo de Configuración del Archivo
            using (ArchivoTipoConfiguracion atc = new ArchivoTipoConfiguracion(id_archivo_tipo, id_compania, id_tabla))
            {
                //Validando si existe la Configuración
                if (atc.id_archivo_tipo_configuracion > 0)
                {
                    //Declarando Arreglo de Bytes
                    byte[] fileArray = Convert.FromBase64String(archivo);

                    //Validando que existan
                    if (fileArray != null)
                    {
                        //Instanciando Tipo del Archivo
                        using (ArchivoTipo at = new ArchivoTipo(atc.id_archivo_tipo))
                        {
                            //Validando que exista el Tipo
                            if (at.id_archivo_tipo > 0)
                            {
                                //Declaramos variable para almacenar ruta
                                string ruta = "";
                                //Armando ruta de guardado físico de archivo
                                //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                ruta = string.Format(@"{0}{1}\{2}-{3}{4}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), atc.id_tabla.ToString("0000"), id_registro.ToString("0000000"), DateTime.Now.ToString("yyMMddHHmmss"), at.extension);

                                //Insertamos Registro
                                resultado = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(atc.id_tabla, id_registro, atc.id_archivo_tipo_configuracion, referencia.ToUpper(), id_usuario, fileArray, ruta);
                            }
                            else
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No se encontro el tipo del Archivo");
                        }
                    }
                    else
                        //Instanciando Excepcion
                        resultado = new RetornoOperacion("No existe el Archivo");
                }
                else
                    //Instanciando Excepcion
                    resultado = new RetornoOperacion("No se encontraron datos complementarios Detalle Archivo");
            }

            //Devolviendo resultado Obtenido
            return resultado.ToXMLString();
        }


    }
}
