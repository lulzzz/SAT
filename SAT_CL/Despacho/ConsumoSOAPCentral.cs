using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.FacturacionElectronica;

namespace SAT_CL.Despacho
{
   public static class  ConsumoSOAPCentral
    {
         

       /// <summary>
       /// 
       /// </summary>
       /// <param name="id_origen_datos"></param>
       /// <param name="id_compania"></param>
       /// <param name="nombre_compania"></param>
       /// <param name="id_unidad"></param>
       /// <param name="id_operador"></param>
       /// <param name="ciudad_disponible"></param>
       /// <param name="estado_disponible"></param>
       /// <param name="fecha_inicio"></param>
       /// <param name="limite_disponibilidad"></param>
       /// <param name="observacion"></param>
       /// <param name="ciudadesDeseadas"></param>
       /// <param name="id_orige_datos_registro"></param>
       /// <param name="id_usuario"></param>
       /// <param name="usuario"></param>
       /// <returns></returns>
        public static XDocument CreateSoapEnvelopePublicaUnidad(int id_origen_datos, int id_compania, string nombre_compania, int id_unidad,  int id_operador,
                                                                  string ciudad_disponible, string estado_disponible,DateTime fecha_inicio, decimal limite_disponibilidad,
                                                                  string observacion , DataTable mitciudadesDeseadas, int id_orige_datos_registro, int id_usuario, string usuario)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:tec='http://www.tectos.com.mx/'
                                    xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'><soapenv:Header/>
                                   <soapenv:Body>
                                  <tec:PublicacionUnidad>
                                    <tec:id_origen_datos></tec:id_origen_datos>
                                    <tec:id_compania></tec:id_compania>
                                    <tec:nombre_compania></tec:nombre_compania>
                                    <tec:id_unidad_origen></tec:id_unidad_origen>
                                    <tec:id_operador></tec:id_operador>
                                     <tec:nombre_compania></tec:nombre_compania>
                                    <tec:id_unidad_origen></tec:id_unidad_origen>
                                    <tec:ciudad_disponibilidad></tec:ciudad_disponibilidad>
                                    <tec:estado_disponibilidad></tec:estado_disponibilidad>
                                    <tec:inicio_disponibilidad></tec:inicio_disponibilidad>
                                    <tec:limite_disponibilidad></tec:limite_disponibilidad>
                                    <tec:observacion></tec:observacion>
                                    <tec:ciudades_deseadas_xml></tec:ciudades_deseadas_xml>
                                    <tec:id_origen_datos_registro></tec:id_origen_datos_registro>
                                    <tec:id_usuario></tec:id_usuario>
                                    <tec:usuario></tec:usuario>
                                    </tec:PublicacionUnidad>
                                    </soapenv:Body>
                                    </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Origen de Datos
            sb.Insert(sb.ToString().IndexOf("</tec:id_origen_datos>"), id_origen_datos);
            //Insertamos Id Compañia
            sb.Insert(sb.ToString().IndexOf("</tec:id_compania>"), id_compania);
            //Insertamos Nombtre de la Compañia
            sb.Insert(sb.ToString().IndexOf("</tec:nombre_compania"), nombre_compania);
            //Insertamos Id Unidad
            sb.Insert(sb.ToString().IndexOf("</tec:id_unidad_origen>"), id_unidad);
            //Insertamos Ciudad Disponible
            sb.Insert(sb.ToString().IndexOf("</tec:ciudad_disponibilidad>"), ciudad_disponible);
            //Insertamos Estado Disponible
            sb.Insert(sb.ToString().IndexOf("</tec:estado_disponibilidad>"), estado_disponible);
            //Insertamos Fexcha Inicio
            sb.Insert(sb.ToString().IndexOf("</tec:inicio_disponibilidad>"), fecha_inicio);
            //Insertamos Limite Dispñonible
            sb.Insert(sb.ToString().IndexOf("</tec:limite_disponibilidad>"), limite_disponibilidad);
            //Insertamos Observación
            sb.Insert(sb.ToString().IndexOf("</tec:observacion>"), observacion);
            //Insertamos Ciudades Deseadas
            sb.Insert(sb.ToString().IndexOf("</tec:ciudades_deseadas_xml"), "<![CDATA[" + ObtieneCiudadesDeseadas(mitciudadesDeseadas) + "]]>");
            //Insertamos Id Origen Datos Registro
            sb.Insert(sb.ToString().IndexOf("</tec:id_origen_datos_registro>"), id_orige_datos_registro);
            //Insertamos Id Usuario
            sb.Insert(sb.ToString().IndexOf("</tec:id_usuario>"), id_usuario);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</tec:usuario>"), usuario);         
            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="mitCiudadesDeseadas"></param>
       /// <returns></returns>
        public static string ObtieneCiudadesDeseadas(DataTable mitCiudadesDeseadas)
        {
            //Definiendo XDocumento
            XDocument xDoc = new XDocument();
            XElement ciudad_deseada = null;
            //Definiendo objeto de retorno
            XElement e_ciudades_deseadas = new XElement("CiudadesDeseadas");
            //Recorremos Ciudades Deseadas
            foreach (DataRow c in mitCiudadesDeseadas.Rows)
            {

                ciudad_deseada = new XElement("CiudadDeseada");
                //lIMPIAMOS Atributos
                ciudad_deseada.RemoveAttributes();
                 
                //Ingresamos Atributos
                ciudad_deseada.Add(new XAttribute("tarifaDeseada", c.Field<decimal>("Tarifa")));
                ciudad_deseada.Add(new XAttribute("ciudadDeseada", c.Field<string>("Ciudad")));
                ciudad_deseada.Add(new XAttribute("anticipoRequerido", c.Field<decimal>("Anticipo")));

                e_ciudades_deseadas.Add(ciudad_deseada);

            }
           
            //Establecemos Elemento
            xDoc.Add(e_ciudades_deseadas);
            //Devolvemos Resultado
            return xDoc.ToString();
        }
    

           /// <summary>
       /// 
       /// </summary>
       /// <param name="mitCiudadesDeseadas"></param>
       /// <returns></returns>
        public static string ObtieneEventosDeseadas(DataTable mitEventos)
        {
            //Definiendo XDocumento
            XDocument xDoc = new XDocument();
            XElement evento_deseado = null;
            //Definiendo objeto de retorno
            XElement e_ciudades_deseadas = new XElement("CiudadesEventoRespuestas");
            //Recorremos Ciudades Deseadas
            foreach (DataRow c in mitEventos.Rows)
            {

                evento_deseado = new XElement("CiudadEventoRespuesta");
                //lIMPIAMOS Atributos
                evento_deseado.RemoveAttributes();
                 
                //Ingresamos Atributos
                evento_deseado.Add(new XAttribute("noSecuencia", c.Field<decimal>("Secuencia")));
                evento_deseado.Add(new XAttribute("ciudad", c.Field<string>("Ciudad")));
                evento_deseado.Add(new XAttribute("cita", c.Field<DateTime>("Cita")));
                evento_deseado.Add(new XAttribute("actividad", c.Field<string>("Actividad")));

                e_ciudades_deseadas.Add(evento_deseado);

            }
           
            //Establecemos Elemento
            xDoc.Add(e_ciudades_deseadas);
            //Devolvemos Resultado
            return xDoc.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mitCiudadesDeseadas"></param>
        /// <returns></returns>
        public static string ObtieneParadasPublicacionUnidad(DataTable mitParadas)
        {
            //Definiendo XDocumento
            XDocument xDoc = new XDocument();
            XElement parada = null;
            //Definiendo objeto de retorno
            XElement paradas = new XElement("Paradas");
            //Recorremos Ciudades Deseadas
            foreach (DataRow c in mitParadas.Rows)
            {

                parada = new XElement("Parada");
                //lIMPIAMOS Atributos
                parada.RemoveAttributes();

                //Ingresamos Atributos
                parada.Add(new XAttribute("noSecuencia", c.Field<decimal>("noSecuencia")));
                parada.Add(new XAttribute("idUbicacionOrigen", c.Field<Int32>("idUbicacionOrigen")));
                parada.Add(new XAttribute("descripcionUbicacionOrigen", c.Field<string>("descripcionUbicacionO")));
                parada.Add(new XAttribute("direccion", c.Field<string>("direccion")));
                parada.Add(new XAttribute("ciudad", c.Field<string>("ciudad")));
                parada.Add(new XAttribute("cp", c.Field<string>("cp")));
                parada.Add(new XAttribute("idTipoEvento", c.Field<Int32>("idTipoEvento")));
                parada.Add(new XAttribute("cita", c.Field<DateTime>("Cita")));
                paradas.Add(parada);

            }

            //Establecemos Elemento
            xDoc.Add(paradas);
            //Devolvemos Resultado
            return xDoc.ToString();
        }

    }
}
