using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.Accesorios
{
    public partial class BitacoraRegistro : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Inicialización
                inicializaForma();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los controles de la forma
        /// </summary>
        private void inicializaForma()
        {   //Declarando variables necesarias
            int idRegistro, idTabla;
            string titulo;
            //Validando la existencia de los datos requeridos para la carga
            if (validaDatosBitacora(out idRegistro, out idTabla, out titulo))
                //Inicialziando el control de usuario 
                ucBitacora.InicializaControlUsuario(idRegistro, idTabla, titulo);
        }
        /// <summary>
        /// Valida la exitencia de los datos necesarios para la carga de la bitácora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="titulo">Leyenda a mostrar en el encabezado del control de usuario</param>
        /// <returns></returns>
        private bool validaDatosBitacora(out int idRegistro, out int idTabla, out string titulo)
        {   //Inicializnado valores de salida
            idRegistro = idTabla = 0;
            titulo = "";
            //Id de Registro a consultar
            if (Request.QueryString.Get("idR") != "")
                //Asignando valores de salida
                idRegistro = Convert.ToInt32(Request.QueryString.Get("idR"));
            //Id de Tabla
            if (Request.QueryString.Get("idT") != "")
                //Asignando valores de salida
                idTabla = Convert.ToInt32(Request.QueryString.Get("idT"));
            //Leyenda
            if (Request.QueryString.Get("tB") != "")
                //Asignando valores de salida
                titulo = Request.QueryString.Get("tB");
            //Si Todos los parámetros fueron definidos correctamente
            if (idRegistro > 0 && idTabla > 0 && titulo != "")
                return true;
            //Devolviendo valor de error
            return false;
        }

        #endregion
    }
}