using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TSDK.Base;

namespace SAT_CL.ControlPatio
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones correspondientes con los Tipos de Evento
    /// </summary>
    public class TipoEvento : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa la Naturaleza del Evento
        /// </summary>
        public enum NaturalezaEvento
        {   /// <summary>
            /// Naturaleza de Tipo "Carga"
            /// </summary>
            Carga = 1,
            /// <summary>
            /// Naturaleza de Tipo "Descarga"
            /// </summary>
            Descarga,
            /// <summary>
            /// Naturaleza de Tipo "Estaciona"
            /// </summary>
            Estaciona
        }

        #endregion

        #region Atributos
        /// <summary>
        /// Atributo  nom_sp con el nombre del sp_ de la tabla
        /// </summary>
        private static string nom_sp = "control_patio.sp_tipo_evento_tte";

        /// <summary>
        /// Atributo privado del id_tipo_evento
        /// </summary>
        private int _id_tipo_evento;
        /// <summary>
        /// ID que corresponde al tipo de evento del objeto
        /// </summary>
        public int id_tipo_evento
        {
            get { return _id_tipo_evento; }
        }
        /// <summary>
        /// ID de ubicación patio del objeto actual
        /// </summary>
        private int _id_ubicacion_patio;
        /// <summary>
        /// Atributo de Tipo Publico de ID de ubicacion patio
        /// </summary>
        public int id_ubicacion_patio
        {
            get { return _id_ubicacion_patio; }
        }
        /// <summary>
        /// Descripcion del patio del objeto
        /// </summary>
        private string _descripcion;
        /// <summary>
        /// Atributo tipo publico de la Descripcion del patio
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }

        }
        /// <summary>
        /// ID de un tipo de Entidad
        /// </summary>
        private int _id_tipo_entidad;
        /// <summary>
        /// ID Tipo publico de ID_Tipo_Entidad
        /// </summary>
        public int id_tipo_entidad
        {
            get { return _id_tipo_entidad; }

        }
        /// <summary>
        /// Naturaleza del Evento
        /// </summary>
        private byte _id_naturaleza_evento;
        /// <summary>
        /// Atributo tipo público de la Naturaleza del Evento
        /// </summary>
        public byte id_naturaleza_evento
        {
            get { return _id_naturaleza_evento; }

        }
        /// <summary>
        /// Atributo tipo público de la Enumeración de la Naturaleza del Evento
        /// </summary>
        public NaturalezaEvento naturaleza_evento
        {
            get { return (NaturalezaEvento)_id_naturaleza_evento; }
        }
        /// <summary>
        /// Habilitar registro
        /// </summary>
        private bool _habilitar;
        /// <summary>
        ///Atributo de tipo publico de Habilitar registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Contructor que inicializa Atributos a valor Default
        /// </summary>
        public TipoEvento()
        {
            this._id_tipo_evento = 0;
            this._id_ubicacion_patio = 0;
            this._descripcion = "";
            this._id_tipo_entidad = 0;
            this._habilitar = false;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id_tipo_evento"></param>
        public TipoEvento(int id_tipo_evento)
        {
            //Declaracion e Inicializacion de un arreglo que contiene los parametros a utilizar en el store procedure
            object[] param = { 3, id_tipo_evento, 0, "", 0, 0, 0, false, "", "" };

            //Invocacion del Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos de la tabla del DataSet con los datos del StoreProcedure
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre cada una de las filas de la tabla Dataset y almacena sus valores en r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_tipo_evento = id_tipo_evento;
                        _id_ubicacion_patio = Convert.ToInt32(r["IdUbicacionPatio"]);
                        _descripcion = Convert.ToString(r["Descripcion"]);
                        _id_tipo_entidad = Convert.ToInt32(r["IdTipoEntidad"]);
                        _id_naturaleza_evento = Convert.ToByte(r["IdNaturalezaEvento"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                    }
                }
            }
        }

        #endregion
    }
}

