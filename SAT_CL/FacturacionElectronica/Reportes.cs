using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase encargada de Gestionar Todos los Reportes del Módulo de Facturación Electronica
    /// </summary>
    public class Reporte : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        private static string _nom_sp = "fe.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de cargar los compribante
        /// </summary>
        /// <param name="id_compania_emisor">Compania Emisor</param>
        /// <param name="id_compania_receptor">Compania Receptor</param>
        /// <param name="id_tipo">Tipo de Comprobantes</param>
        /// <param name="id_estatus"> Estatus del Comprobante</param>
        /// <param name="inicioFechaExpedicion">Inicio Fecha de expedición</param>
        /// <param name="finFechaExpedicion">Fin fecha de expedición</param>
        /// <param name="generado">Generado</param>
        /// <param name="serie">Serie del Comprobante</param>
        /// <param name="folio">Folio del Comprobante</param>
        /// <param name="inicioFechaCaptura">Inicio Fecha de Captura</param>
        /// <param name="finFechaCaptura">Fin Fecha de Captura</param>
        /// <param name="inicioFechaCancelacion">Inicio fecha de cancelación</param>
        /// <param name="finFechaCancelacion">Fin fecha de Cancelación</param>
        /// <param name="id_usuario_timbra">Usuario que Timbra el CFDI</param>
        /// <returns></returns>
        public static DataTable CargaComprobantes(int id_compania_emisor, int id_compania_receptor, byte id_tipo, byte id_estatus, DateTime inicioFechaExpedicion,
                                              DateTime  finFechaExpedicion, int generado, string serie, int folio, DateTime  inicioFechaCaptura, 
                                              DateTime  finFechaCaptura, DateTime inicioFechaCancelacion,  DateTime  finFechaCancelacion, int id_usuario_timbra)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 1, id_compania_emisor, id_compania_receptor, id_tipo, id_estatus, inicioFechaExpedicion == DateTime.MinValue ? "" : inicioFechaExpedicion.ToString("dd/MM/yyyy HH:mm"),
                                  finFechaExpedicion == DateTime.MinValue ? "" : finFechaExpedicion.ToString("dd/MM/yyyy HH:mm"), generado, serie, folio, inicioFechaCaptura == DateTime.MinValue ? "" : inicioFechaCaptura.ToString("dd/MM/yyyy HH:mm"), 
                                   finFechaCaptura == DateTime.MinValue ? "" : finFechaCaptura.ToString("dd/MM/yyyy HH:mm"),  inicioFechaCancelacion == DateTime.MinValue ? "" : inicioFechaCancelacion.ToString("dd/MM/yyyy HH:mm"),
                                   finFechaCancelacion == DateTime.MinValue ? "" : finFechaCancelacion.ToString("dd/MM/yyyy HH:mm"), id_usuario_timbra, "", "", "", "", "", "", };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtFacturas;
        }

        /// <summary>
        /// Carga Reporte de la Cancelación para Timbre Fiscal
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_tipo">Id Tipo</param>
        /// <param name="id_compania_receptor">Id Compania Receptor</param>
        /// <param name="folio">Folio</param>
        /// <returns></returns>
         public static DataTable CargaCancelacionTimbreFiscal(int id_compania_emisor, byte id_tipo, int id_compania_receptor, int folio)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 2, id_compania_emisor, id_tipo, id_compania_receptor, folio, "",
                                 "", "", "", "", "", 
                                   "",  "","", "", "", "", "", "", "", "", };

            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtFacturas;

        }
         /// <summary>
         /// Método encargado de realizar la consulta de las facturas a cada cliente.
         /// </summary>
         /// <param name="id_compania_emisor_receptor">Identificador de cliente.</param>
         /// <returns></returns>
         public static DataTable UltimasFacturasCliente(int id_compania_receptor)
         {
             //Creación de la variable datatable
             DataTable dtFacturas = null;
             //Creación del arreglo param
             object[] param = { 3, id_compania_receptor, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
             //Obtiene las facturas y el resultado lo alamcena en un dataset
             using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
             {
                 //Valida los datos del DS
                 if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                     //Asigna los valores del dataset a la tabla dtFacturas
                     dtFacturas = DS.Tables["Table"];
             }
             //Devuelve al método la tabla facturas
             return dtFacturas;

         }

         /// <summary>
         /// Método encargado de realizar la consulta de las facturas a cada cliente por facturacion otros.
         /// </summary>
         /// <param name="id_compania_emisor_receptor">Identificador de cliente.</param>
         /// <returns></returns>
         public static DataTable FacturaOtrosCliente(int id_compania_receptor)
         {
             //Creación de la variable datatable
             DataTable dtFacturas = null;
             //Creación del arreglo param
             object[] param = { 4, id_compania_receptor, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
             //Obtiene las facturas y el resultado lo alamcena en un dataset
             using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
             {
                 //Valida los datos del DS
                 if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                     //Asigna los valores del dataset a la tabla dtFacturas
                     dtFacturas = DS.Tables["Table"];
             }
             //Devuelve al método la tabla facturas
             return dtFacturas;
         }
        #endregion
    }
}
