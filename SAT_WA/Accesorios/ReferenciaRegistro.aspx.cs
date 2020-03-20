using System;
using System.Web.UI;

namespace SAT.Accesorios
{
    public partial class ReferenciaRegistro : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Si no es unarecraga de página
            if (!Page.IsPostBack)
                //Inicializnado los controles de la página
                inicializaForma();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la página
        /// </summary>
        private void inicializaForma()
        {   //Declarando variables requeridas para inicialización de control incrustado
            int idRegistro, idTabla, idCompania;
            //Obteniendo valores requeridos
            if (validaDatosReferencia(out idRegistro, out idTabla, out idCompania))
                //Inicializando control
                ucReferencias.InicializaControl(idRegistro, idTabla, idCompania);
        }

        /// <summary>
        /// Determina si los valores proporcionados son suficientes para inicilizar el contenido de la ventana
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <returns></returns>
        private bool validaDatosReferencia(out int idRegistro, out int idTabla, out int idCompania)
        {   //Inicializando parametros de salida
            idRegistro = idTabla = idCompania = 0;
            //Obteniendo los valores requeridos
            //Id de Registro
            if (Request.QueryString.Get("idR") != "")
                idRegistro = Convert.ToInt32(Request.QueryString.Get("idR"));
            //Id de tabla
            if (Request.QueryString.Get("idT") != "")
                idTabla = Convert.ToInt32(Request.QueryString.Get("idT"));
            //Id de compania
            if (Request.QueryString.Get("idC") != "")
                idCompania = Convert.ToInt32(Request.QueryString.Get("idC"));
            //Si se tienen los datos suficientes
            if (idRegistro > 0 && idTabla > 0 && idCompania > 0)
                return true;
            //Devolviendo error
            return false;
        }

        #endregion
    }
}