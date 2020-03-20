using SAT_CL.FacturacionElectronica;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using TSDK.Datos;

namespace SAT.FacturacionElectronica
{
    public partial class DescargaCFDI : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento disparado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Verifica si se produce un Postback
            if (!Page.IsPostBack)
                //
                inicializaPagina();
            else if (Page.IsPostBack)
                //
                inicializaPagina();
        }
        /// <summary>
        /// Evento disparado al Descargar el Link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void link_descarga_Click(object sender, EventArgs e)
        {
            //Creamos lista de rutas y errrores
            List<KeyValuePair<string, byte[]>> rutas = new List<KeyValuePair<string, byte[]>>();
            List<string> errores = new List<string>();

            //Declarando Archivo Final
            byte[] final_file = null;
            //Validando que este disponible la Descarga
            if (validaDescarga())
            {   //Instanciando Link de Descarga
                using (LinkDescarga link = new LinkDescarga(10))
                {
                    //Obtenemos Id de Comprobantes
                    using (DataTable mit = DetalleLinkDescarga.RecuperaIdComprobantes(link.idLinkDescarga))
                    {
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(mit))
                        {
                            //Rrecorremos los Comprobantes
                            foreach (DataRow r in mit.Rows)
                            {
                                //Instanciando Comprobante
                                using (SAT_CL.FacturacionElectronica.Comprobante cfdi = new SAT_CL.FacturacionElectronica.Comprobante(r.Field<int>("IdComprobante")))
                                {   //validando si Existe la Ruta
                                    if (File.Exists(cfdi.ruta_xml))
                                    {   //Añadiendo Archivo al Arreglo de Bytes
                                        rutas.Add(new KeyValuePair<string, byte[]>(cfdi.serie + cfdi.folio.ToString() , File.ReadAllBytes(cfdi.ruta_xml)));
                                    }
                                    //Asignando la Ruta al primer nivel del Arreglo
                                    rutas.Add(new KeyValuePair<string, byte[]>(cfdi.serie + cfdi.folio.ToString() + ".pdf", cfdi.GeneraPDFComprobante()));
                     
                                }
                            }

                            //Añadimos archivos al Zip
                            final_file = TSDK.Base.Archivo.ConvirteArchivoZIP(rutas, out errores);

                            if (final_file != null)
                            {
                                //Guardando Archivo en Session
                                Session["ZIP"] = final_file;
                                //Declarando variable a Enviar
                                String url;
                                url = String.Format("../../UserControls/Prueba.aspx?id={0}", link.idLinkDescarga);
                                //Abre Nueva Ventana
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "AbreVentana", 200, 200, false, false, false, true, true, Page);
                                //Recarga la Forma
                                inicializaPagina();
                            }

                        }

                    }
                }
            }
        }


        #endregion

        #region Métodos
        /// <summary>
        /// Método que se encarga de Inicializar la pagina
        /// </summary>
        private void inicializaPagina()
        {   //Valida la Descarga
            if (validaDescarga())
                //Mostrando Visble el Panel Correcto
                pnlVinculoCorrecto.Visible = true;
            else//Mostrando Visble el Panel Incorrecto
                pnlVinculoIncorrecto.Visible = true;
        }
        /// <summary>
        /// Método que verifica 
        /// </summary>
        private bool validaDescarga()
        {   //Variable de Resultado
            bool result = false;
            //Validando que exista un ID
            if (Request["id"] != null)
            {   //Convirtiendo a entero
                if (Convert.ToInt32(Request["id"]) != 0)
                {   //Instanciando Link de Descarga
                    using (LinkDescarga link = new LinkDescarga
                        (Convert.ToInt32(Request["id"])))
                    {   //Verificando que exista un Contacto (puede ser '-1' o 'n')
                        if (link.idContacto != 0)
                        {   //Verficando que la fecha de Caducidad sea mayor al dia actual
                            if (link.fechaCaducidad >= TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro())
                            {   //Verificando que Existan descargas disponibles
                                if (link.descargasRestantes != 0)
                                {   //Muestra Descargas Restantes
                                    lblDescRest.Text = link.descargasRestantes.ToString();
                                    //Resultado Positivo
                                    result = true;
                                }
                                else//Mensaje de Fecha Caducada
                                    lblAviso.Text = "Ha Llegado al Limite de las Descargas Permitidas";
                            }
                            else//Mensaje de Fecha Caducada
                                lblAviso.Text = "La Fecha de Caducidad a Expirado";
                        }
                        else//Mensaje de Link Invalido
                            lblAviso.Text = "El Link de Descarga es Invalido";
                    }
                }
            }
            return result;
        }
        #endregion
    }
}