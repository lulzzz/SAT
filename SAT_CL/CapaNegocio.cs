using System.Data;
using System.Web.UI.WebControls;

namespace SAT_CL
{
    /// <summary>
    /// Decripcion breve de a Capa de Negocio
    /// </summary>
    public class CapaNegocio
    {
        #region Atributos

        /// <summary>
        /// Objeto Capa de negocio instancido por la propia clase
        /// </summary>	
        private static CapaNegocio capaNegocio = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad para acceder a la unica instancia de la clase
        /// </summary>
        public static CapaNegocio m_capaNegocio
        {
            get
            {
                if (capaNegocio == null)
                {
                    lock (new object())
                    {
                        if (capaNegocio == null)
                            capaNegocio = new CapaNegocio();

                    }
                }
                return capaNegocio;
            }
        }

        #endregion

        #region Constrcutores

        /// <summary>
        /// Constrcutor de la clase
        /// </summary>
        private CapaNegocio()
        {

        }

        #endregion

        #region Catalogos en el esquema general

        #region DropDownList

        /// <summary>
        /// Método Privado encargado de Cargar los valores en el DropDownList
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="idTipoStoredProcedure">DropDownList a cargar</param>
        /// <param name="opcionInicial">Opción Inicial</param>
        /// <param name="idTipoCatalogo">Tipo del Catalogo</param>
        /// <param name="idTipoCatalogoSuperior">Tipo del Catalogo Superior</param>
        /// <param name="valorCatalogoSuperior">valor del Catalogo Superior</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        private void cargaCatalogoGeneral(DropDownList ddl, int idTipoStoredProcedure, string opcionInicial, int idTipoCatalogo, int idTipoCatalogoSuperior, int valorCatalogoSuperior, int param1, string param2, int param3, string param4)
        {   //parametros del store procedure
            object[] valor = { idTipoStoredProcedure, opcionInicial, idTipoCatalogo, idTipoCatalogoSuperior, valorCatalogoSuperior, param1, param2, param3, param4 };
            //Ejecuta el store procedure indicado
            using (DataTable mit = CargaCatalogoGeneral(idTipoStoredProcedure, opcionInicial, idTipoCatalogo, idTipoCatalogoSuperior, valorCatalogoSuperior, param1, param2, param3, param4))
                //Carga el dropdown con los resultados obtenido en la consulta
                TSDK.ASP.Controles.CargaDropDownList(ddl, mit, "id", "descripcion");
        }
        /// <summary>
        /// Carga cualquier tipo de catálogo que este ubicado dentro del esquema general.
        /// </summary>
        /// <param name="ddl">DropDownList a cargar</param>
        /// <param name="opcionInicial">OciónInicial que será colocada como primer opción de catálogo</param>
        /// <param name="idTipoCatalogo">Id de Tipo de Catálogo a cargar</param>
        public void CargaCatalogoGeneral(DropDownList ddl, string opcionInicial, int idTipoCatalogo)
        {   //Invocamos al metodo generico
            cargaCatalogoGeneral(ddl, 1, opcionInicial, idTipoCatalogo, 0, 0, 0, "", 0, "");
        }
        /// <summary>
        /// Carga cualquier tipo de catálogo que este ubicado dentro del esquema general, que tenga dependencia de otro catálogo.
        /// </summary>
        /// <param name="ddl">DropDownList a cargar</param>
        /// <param name="opcionInicial">OciónInicial que será colocada como primer opción de catálogo</param>
        /// <param name="idTipoCatalogo">Id de Tipo de Catálogo a cargar</param>
        /// <param name="valorCatalogoSuperior">Valor del catálogo superior del que depende el catálogo a cargar.</param>
        public void CargaCatalogoGeneral(DropDownList ddl, string opcionInicial, int idTipoCatalogo, int valorCatalogoSuperior)
        {   //Invocamos al metodo generico
            cargaCatalogoGeneral(ddl, 2, opcionInicial, idTipoCatalogo, 0, valorCatalogoSuperior, 0, "", 0, "");
        }
        /// <summary>
        /// Carga cualquier tipo de catálogo que este ubicado dentro del esquema general. Realizando la carga dentro de un DropDownList de fila de GridView.
        /// </summary>
        /// <param name="filaGridView">Fila donde se encuentra el control DropDownList</param>
        /// <param name="nombreDropDownList">Nombre del controlDropDownList a buscar</param>
        /// <param name="opcionInicial">OciónInicial que será colocada como primer opción de catálogo</param>
        /// <param name="idTipoCatalogo">Id de Tipo de Catálogo a cargar</param>
        public void CargaCatalogoGeneral(GridViewRow filaGridView, string nombreDropDownList, string opcionInicial, int idTipoCatalogo)
        {
            //Verificando que los elementos fila y nombre de control sean válidos
            if (filaGridView != null && !string.IsNullOrEmpty(nombreDropDownList))
            {
                //Referenciando a la fila del GridView que contiene el control
                using (DropDownList ddl = (DropDownList)filaGridView.FindControl(nombreDropDownList))
                {
                    //Validando que el DropDownList exista
                    if (ddl != null)
                        //Invocamos al metodo generico
                        cargaCatalogoGeneral(ddl, 1, opcionInicial, idTipoCatalogo, 0, 0, 0, "", 0, "");
                }
            }
        }

        #endregion

        #region ListBox

        /// <summary>
        /// Método encargado de Cargar los valores en el ListBox
        /// </summary>
        /// <param name="lbx">ListBox a cargar</param>
        /// <param name="idTipoStoredProcedure">Id de Tipo del SP</param>
        /// <param name="opcionInicial">Opción Inicial</param>
        /// <param name="idTipoCatalogo">Tipo del Catalogo</param>
        /// <param name="idTipoCatalogoSuperior">Tipo del Catalogo Superior</param>
        /// <param name="valorCatalogoSuperior">valor del Catalogo Superior</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        private void cargaCatalogoGeneral(ListBox lbx, int idTipoStoredProcedure, string opcionInicial, int idTipoCatalogo, int idTipoCatalogoSuperior, int valorCatalogoSuperior, int param1, string param2, int param3, string param4)
        {   //parametros del store procedure
            object[] valor = { idTipoStoredProcedure, opcionInicial, idTipoCatalogo, idTipoCatalogoSuperior, valorCatalogoSuperior, param1, param2, param3, param4 };
            //Ejecuta el store procedure indicado
            using (DataTable mit = CargaCatalogoGeneral(idTipoStoredProcedure, opcionInicial, idTipoCatalogo, idTipoCatalogoSuperior, valorCatalogoSuperior, param1, param2, param3, param4))
                //Carga el dropdown con los resultados obtenido en la consulta
                TSDK.ASP.Controles.CargaListBox(lbx, mit, "id", "descripcion");
        }
        /// <summary>
        /// Carga cualquier tipo de catálogo que este ubicado dentro del esquema general.
        /// </summary>
        /// <param name="lbx">ListBox a cargar</param>
        /// <param name="opcionInicial">OciónInicial que será colocada como primer opción de catálogo</param>
        /// <param name="idTipoCatalogo">Id de Tipo de Catálogo a cargar</param>
        public void CargaCatalogoGeneral(ListBox lbx, string opcionInicial, int idTipoCatalogo)
        {   //Invocamos al metodo generico
            cargaCatalogoGeneral(lbx, 1, opcionInicial, idTipoCatalogo, 0, 0, 0, "", 0, "");
        }
        /// <summary>
        /// Carga cualquier tipo de catálogo que este ubicado dentro del esquema general, que tenga dependencia de otro catálogo.
        /// </summary>
        /// <param name="lbx">ListBox a cargar</param>
        /// <param name="opcionInicial">OciónInicial que será colocada como primer opción de catálogo</param>
        /// <param name="idTipoCatalogo">Id de Tipo de Catálogo a cargar</param>
        /// <param name="valorCatalogoSuperior">Valor del catálogo superior del que depende el catálogo a cargar.</param>
        public void CargaCatalogoGeneral(ListBox lbx, string opcionInicial, int idTipoCatalogo, int valorCatalogoSuperior)
        {   //Invocamos al metodo generico
            cargaCatalogoGeneral(lbx, 2, opcionInicial, idTipoCatalogo, 0, valorCatalogoSuperior, 0, "", 0, "");
        }

        #endregion

        #region Origen de Datos

        /// <summary>
        /// Método Privado encargado de Cargar los valores en el DropDownList
        /// </summary>
        /// <param name="idTipoStoredProcedure">DropDownList a cargar</param>
        /// <param name="opcionInicial">Opción Inicial</param>
        /// <param name="idTipoCatalogo">Tipo del Catalogo</param>
        /// <param name="idTipoCatalogoSuperior">Tipo del Catalogo Superior</param>
        /// <param name="valorCatalogoSuperior">valor del Catalogo Superior</param>
        /// <param name="param1">Parámetro 1</param>
        /// <param name="param2">Parámetro 2</param>
        /// <param name="param3">Parámetro 3</param>
        /// <param name="param4">Parámetro 4</param>
        public DataTable CargaCatalogoGeneral(int idTipoStoredProcedure, string opcionInicial, int idTipoCatalogo, int idTipoCatalogoSuperior, int valorCatalogoSuperior, int param1, string param2, int param3, string param4)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //parametros del store procedure
            object[] valor = { idTipoStoredProcedure, opcionInicial, idTipoCatalogo, idTipoCatalogoSuperior, valorCatalogoSuperior, param1, param2, param3, param4 };
            //Ejecuta el store procedure indicado
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaCatalogoGeneral", valor))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "table"))
                    //Asignando a objeto de retorno
                    mit = DS.Tables["Table"];

                //Devolviendo objeto de resultado
                return mit;
            }
        }

        #endregion

        #endregion

        #region Catalogos fuera del esquema

        /// <summary>
        /// Método que obtiene el valor de una variable en BD
        /// </summary>
        /// <param name="descripcionVariable">Nombre de la Variable</param>
        /// <returns></returns>
        public string RegresaVariableCatalogoBD(string descripcionVariable)
        {  
            //Declarando e inicializando arreglo de parámetros requerido
            //Inicialziando párametros
            object[] parametros = { 2, 0, descripcionVariable, 0, null, 0, 0, true };
            //Declarando Variable de Retorno
            string retorno = "";
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cat_variable_tcv", parametros))
            {   //Validando que existan Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Cada Fila
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        retorno = r[0].ToString();
                }
            }
            //Devolvinedo variable
            return retorno;
        }
        /// <summary>
        /// Método que obtiene el valor de una variable en BD ligando la Compañia correspondiente y la descripción
        /// </summary>
        /// <param name="descripcionVariable">Nombre de la Variable</param>
        /// <param name="id_compania">Id de la Compañia por obtener</param>
        /// <returns></returns>
        public string RegresaVariableCatalogoBD(string descripcionVariable, int id_compania)
        {   //Declarando e inicializando arreglo de parámetros requerido
            //Inicialziando párametros
            object[] parametros = {6, 0, descripcionVariable, 0, null, id_compania, 0, true  };
            //Declarando Variable de Retorno
            string retorno = "";
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cat_variable_tcv", parametros))
            {   //Validando que existan Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Cada Fila
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        retorno = r[0].ToString();
                }
            }
            //Devolvinedo variable
            return retorno;
        }
        /// <summary>
        /// Método encargado de Devolver todo el registro de una variable de Catalogo
        /// </summary>
        /// <param name="descripcionVariable">Nombre de la Variable</param>
        /// <param name="id_compania">Compañia por obtener</param>
        /// <returns></returns>
        public DataTable RegresaRegistroVariableCatalogoBD(string descripcionVariable, int id_compania)
        {   
            //Declarando e inicializando arreglo de parámetros requerido
            DataTable dtVariableBD = null;
            
            //Inicialziando párametros
            object[] parametros = { 7, 0, descripcionVariable, 0, null, id_compania, 0, true };
            
            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cat_variable_tcv", parametros))
            {   
                //Validando que existan Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Datos Obtenidos
                    dtVariableBD = ds.Tables["Table"];
            }
            //Devolvinedo variable
            return dtVariableBD;
        }

        #region DataSet

        /// <summary>
        /// Obtiene el origen de datos con el resultado del catálogo solicitado
        /// </summary>
        /// <param name="id_catalogo">Id de Catálogo</param>
        /// <param name="opcionInicial">Opción Inicial</param>
        /// <param name="parametro1">Parámetro 1</param>
        /// <param name="parametro2">Parámetro 2</param>
        /// <param name="parametro3">Parámetro 3</param>
        /// <param name="parametro4">Parámetro 4</param>
        /// <returns></returns>
        public DataTable CargaCatalogo(int id_catalogo, string opcionInicial, int parametro1, string parametro2, int parametro3, string parametro4)
        {   
            //Declarando objetod de retorno
            DataTable mit = null;

            //parametros del store proc
            object[] valor = { id_catalogo, opcionInicial, parametro1, parametro2, parametro3, parametro4 };

            //Ejecuta el store procedure indicado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet("global.sp_cargaCatalogo", valor))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Retornamos el resultado de la consulta
                return mit;
            }
        }

        #endregion

        #region DropDownList

        /// <summary>
        /// Método que carga cualquier catalogo que no este ubicado dentro del esquema global de catalogos.
        /// </summary>
        /// <param name="ddl">Control DropDownList en que se cargará el catálogo</param>
        /// <param name="id_catalogo">catalogo a cargar</param>
        /// <param name="opcionInicial">Opcion Inical a mostrar en el Catálogo, con valor 0</param>
        /// <param name="parametro1">Parametro 1</param>
        /// <param name="parametro2">Parametro 2</param>  
        /// <param name="parametro3">Parametro 3</param>
        /// <param name="parametro4">Parametro 4</param>
        public void CargaCatalogo(DropDownList ddl, int id_catalogo, string opcionInicial, int parametro1, string parametro2, int parametro3, string parametro4)
        {   
            //parametros del store proc
            object[] valor = { id_catalogo, opcionInicial, parametro1, parametro2, parametro3, parametro4 };
            //Ejecuta el store procedure indicado
            using (DataTable mit = CargaCatalogo(id_catalogo, opcionInicial, parametro1, parametro2, parametro3, parametro4))
                //Carga el dropdown con los resultados obtenido en la consulta
                TSDK.ASP.Controles.CargaDropDownList(ddl, mit, "id", "descripcion");
        }
        /// <summary>
        /// Carga catalogos fuera del esquema global en base al id de catalogo y una opción inicial no obligatoria
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="id_catalogo"></param>
        /// <param name="opcionInicial"></param>
        public void CargaCatalogo(DropDownList ddl, int id_catalogo, string opcionInicial)
        {   
            //Invocamos al metodo general de carga de catalogos
            CargaCatalogo(ddl, id_catalogo, opcionInicial, 0, "", 0, "");
        }


        #endregion

        #region ListBox

        /// <summary>
        /// Método que carga cualquier catalogo que no este ubicado dentro del esquema global de catalogos.
        /// </summary>
        /// <param name="lbx">Control ListBox en que se cargará el catálogo</param>
        /// <param name="id_catalogo">catalogo a cargar</param>
        /// <param name="opcionInicial">Opcion Inical a mostrar en el Catálogo, con valor 0</param>
        /// <param name="parametro1">Parametro 1</param>
        /// <param name="parametro2">Parametro 2</param>  
        /// <param name="parametro3">Parametro 3</param>
        /// <param name="parametro4">Parametro 4</param>
        public void CargaCatalogo(ListBox lbx, int id_catalogo, string opcionInicial, int parametro1, string parametro2, int parametro3, string parametro4)
        {
            //parametros del store proc
            object[] valor = { id_catalogo, opcionInicial, parametro1, parametro2, parametro3, parametro4 };
            //Ejecuta el store procedure indicado
            using (DataTable mit = CargaCatalogo(id_catalogo, opcionInicial, parametro1, parametro2, parametro3, parametro4))
                //Carga el dropdown con los resultados obtenido en la consulta
                TSDK.ASP.Controles.CargaListBox(lbx, mit, "id", "descripcion");
        }
        /// <summary>
        /// Carga catalogos fuera del esquema global en base al id de catalogo y una opción inicial no obligatoria
        /// </summary>
        /// <param name="lbx"></param>
        /// <param name="id_catalogo"></param>
        /// <param name="opcionInicial"></param>
        public void CargaCatalogo(ListBox lbx, int id_catalogo, string opcionInicial)
        {
            //Invocamos al metodo general de carga de catalogos
            CargaCatalogo(lbx, id_catalogo, opcionInicial, 0, "", 0, "");
        }


        #endregion

        #endregion
    }
}
