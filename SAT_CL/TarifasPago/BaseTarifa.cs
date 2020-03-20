using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.TarifasPago
{
    public class BaseTarifa : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define los tipos de base de tarifa existentes
        /// </summary>
        public enum Base
        {
            /// <summary>
            /// Tarifa por Kilometros Recorridos
            /// </summary>
            Distancia = 1,
            /// <summary>
            /// Tarifa por Kilogramos Transportados
            /// </summary>
            Peso,
            /// <summary>
            /// Tarifa por Litros Transportados
            /// </summary>
            Volumen,
            /// <summary>
            /// Tarifa fija o plana
            /// </summary>
            Fijo,
            /// <summary>
            /// Tarifa por Horas de Servicio
            /// </summary>
            Tiempo
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure de la clase
        /// </summary>
        private static string _nombre_stored_procedure = "tarifas_pago.sp_base_tarifa_tbt";

        private int _id_base_tarifa;
        /// <summary>
        /// Obtiene el Id de Base de Tarifa
        /// </summary>
        public int id_base_tarifa { get { return this._id_base_tarifa; } }
        /// <summary>
        /// Obtiene la descripción de la base de tarifa
        /// </summary>
        public Base base_tarifa { get { return (Base)(this._id_base_tarifa); } }
        private string _descripcion;
        /// <summary>
        /// Obtiene ela descripción de la base de tarifa
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private int _id_unidad_medida;
        /// <summary>
        /// Obtiene el Id de la Unidad de Medida asignada
        /// </summary>
        public int id_unidad_medida { get { return this._id_unidad_medida; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del registro en BD
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia nueva y sin configuración del tipo BaseTarifa
        /// </summary>
        public BaseTarifa()
        {
            //Asignando valores de atributos
            this._id_base_tarifa = 0;
            this._descripcion = "";
            this._id_unidad_medida = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Crea una instancia nueva BaseTarifa, con la información del registro solicitado
        /// </summary>
        /// <param name="id_base_tarifa"></param>
        public BaseTarifa(int id_base_tarifa)
        {
            cargarAtributosInstancia(id_base_tarifa);
        }
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~BaseTarifa()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instancia
        /// </summary>
        /// <param name="id_base_tarifa">Id de Base de tarifa</param>
        /// <returns></returns>
        private bool cargarAtributosInstancia(int id_base_tarifa)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Declarando arreglo de parámetros para consulta en BD
            object[] param = { 3, id_base_tarifa, "", 0, 0, false, "", "" };

            //Realizando consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando entre resultados devueltos
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores de atributos
                        this._id_base_tarifa = Convert.ToInt32(r["Id"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._id_unidad_medida = Convert.ToInt32(r["IdUnidadMedida"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Indicando asignación correcta de atributos
                        resultado = true;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Públicos



        #endregion
    }
}