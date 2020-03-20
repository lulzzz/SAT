using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using TSDK.Base;
using TSDK.Base.CertificadoDigital;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Permite la generación de los elementos involucrados en el sellado digital de un CFD.
    /// Esta clase no puede ser heredada.
    /// </summary>
    public abstract class SelloDigitalSAT
    {
        #region Enumeraciones

        /// <summary>
        /// Define los posibles métodos de digestión disponibles en la clase
        /// </summary>
        public enum MetodoDigestion { MD5, SHA1, SHA256 };

        #endregion

        #region Métodos Generales

        /// <summary>
        /// Realiza el proceso de transformar los caracteres unicode en una secuencia de bytes
        /// </summary>
        /// <param name="cadenaOriginal">Cadena que será transformada a UTF8</param>
        /// <returns>Arreglo de bytes que representan a la cadena de entrada en el formato UTF8.
        /// Si "cadenaOriginal" es nula o vacia, retorno valor null.</returns>
        public static byte[] CodificacionUTF8(string cadenaOriginal)
        {
            //Declarando variable de retorno
            byte[] cadenaUTF8 = null;

            //Instanciamos un codificado UTF8
            //UTF8Encoding UTF8 = new UTF8Encoding();

            //Si la cadena no está vacia
            if (!string.IsNullOrEmpty(cadenaOriginal))
                //Transformar los caracteres Unicode en una secuencia de bytes
                cadenaUTF8 = Encoding.UTF8.GetBytes(cadenaOriginal);

            //Retornamos la cadena codificada
            return cadenaUTF8;
        }
        /// <summary>
        /// Realiza el proceso de generar una encryptacion siempre de 16 bytes, la posibilidad de una 
        /// misma salida antes dos entradas diferente es de 1 en 2 at 35 (garantiza inalterabilidad)
        /// </summary>
        /// <param name="cadenaUTF8"></param>
        /// <returns></returns>
        public static byte[] Digestion(byte[] cadenaUTF8, MetodoDigestion tipoDigestion)
        {
            //Declaramos arreglo de bytes resultante
            byte[] cadenaDigerida = null;

            switch (tipoDigestion)
            {
                case MetodoDigestion.MD5:
                    {
                        //Creamos una instancia del metodo de digestion
                        var digestionMD5 = System.Security.Cryptography.MD5.Create();
                        //Realizamos la digestion
                        cadenaDigerida = digestionMD5.ComputeHash(cadenaUTF8);
                        break;
                    }
                case MetodoDigestion.SHA1:
                    {
                        //Creamos una instancia del metodo de digestion SHA1
                        var digestionSHA1 = System.Security.Cryptography.SHA1.Create();
                        //Realizamos la digestion
                        cadenaDigerida = digestionSHA1.ComputeHash(cadenaUTF8);
                        break;
                    }
                case MetodoDigestion.SHA256:
                    {
                        //Creamos una instancia del metodo de digestion SHA256
                        var digestionSHA256 = System.Security.Cryptography.SHA256.Create();
                        //Realizamos la digestion
                        cadenaDigerida = digestionSHA256.ComputeHash(cadenaUTF8);
                        break;
                    }
            }

            //Retornamos el arreglo de byte digerido
            return cadenaDigerida;
        }
        /// <summary>
        /// Metodo encargado de obtener la cadena original a partir de un archivo XML 
        /// </summary>
        /// <param name="archivo_xml">URI del archivo a convertir</param>
        /// <param name="archivo_xslt">URI del archivo de transformación</param>
        /// <param name="archivo_xslt_alternativo">URI del archivo de transformación a utilizar si el primero no es accesible</param>
        /// <param name="cadena_original">Resultado de transformación de archivo (Cadena Original)</param>
        /// <returns></returns>
        public static RetornoOperacion CadenaCFD(string archivo_xml, string archivo_xslt, string archivo_xslt_alternativo, out string cadena_original)
        {
            //Declarando variable de retorno e inicializnado parámetro de salida
            RetornoOperacion resultado = new RetornoOperacion("Transformación Exitosa.", true);
            cadena_original = "";
            bool enLinea = true;


            //Instanciamos un nuevo objecto de compilacion xslt
            var xslt = new XslCompiledTransform();

            //Intentando generar Cadena Original
            try
            {
                //Cargamos nuestro archivo xslt de la ruta indicada
                xslt.Load(archivo_xslt);
            }
            catch (Exception)
            {
                //Estableciendo error con la versión en linea
                enLinea = false;

                try
                {
                    //Cargamos nuestro archivo local .xslt de la ruta indicada
                    xslt.Load(archivo_xslt_alternativo);
                }
                catch (Exception e)
                {
                    //Registrando excepción
                    resultado = new RetornoOperacion(e.Message + "<br/>" + "Error con la transformación local, la cadena original no pudo ser generada.");
                }
            }

            //Si no hay errores hasta este punto
            if (resultado.OperacionExitosa)
            {
                try
                {
                    //Instanciamos de manera temporal un flujo de escritura de archivos
                    using (MemoryStream flujo = new MemoryStream())
                    {
                        //Aplicamos el estilo
                        xslt.Transform(new XPathDocument(archivo_xml), null, flujo);

                        //Creamos un flujo de lectura
                        var s = new StreamReader(flujo);

                        //Si el flujo de lectura existe
                        if (s != null)
                        {
                            //Posicionando al inicio del flujo
                            s.BaseStream.Position = 0;

                            //Estableciendo valor de retorno
                            cadena_original = s.ReadToEnd();
                        }

                        //Cerrando lector de flujo con el archivo temporal
                        s.Close();

                        //Si NO se generó conn la versión en linea
                        if (!enLinea)
                            //Actualizando mensaje de resultado
                            resultado = new RetornoOperacion("Error con la transformación en linea, se generó la cadena original con una versión local.", true);
                    }
                }
                //En caso de error
                catch (Exception ex)
                {
                    //Registrando error
                    resultado = new RetornoOperacion(ex.Message);
                }
            }

            //Devolviendo cadena resultante
            return resultado;
        }
        /// <summary>
        /// Metodo encargado de obtener la cadena original a partir de un flujo de documento XML 
        /// </summary>
        /// <param name="flujoCFD">Flujo que contiene el CFD</param>
        /// <param name="archivo_xslt">URI del archivo de transformación</param>
        /// <param name="archivo_xslt_alternativo">URI del archivo de transformación a utilizar si el primero no puede ser utilizado</param>
        /// <param name="cadena_original">Resultado de transformación de archivo (Cadena Original)</param>
        /// <returns></returns>
        public static RetornoOperacion CadenaCFD(Stream flujoCFD, string archivo_xslt, string archivo_xslt_alternativo, out string cadena_original)
        {
            //Declarando variable de retorno e inicializnado parámetro de salida
            RetornoOperacion resultado = new RetornoOperacion("Transformación exitosa.", true);
            cadena_original = "";
            bool enLinea = true;

            //Instanciamos un nuevo objecto de compilacion xslt
            var xslt = new XslCompiledTransform();

            //Intentando generar Cadena Original
            try
            {
                //Cargamos nuestro archivo xslt de la ruta indicada
                xslt.Load(archivo_xslt);
            }
            catch (Exception)
            {
                //Estableciendo error con la versión en linea
                enLinea = false;

                try
                {
                    //Cargamos nuestro archivo local .xslt de la ruta indicada
                    xslt.Load(archivo_xslt_alternativo);
                }
                catch (Exception e)
                {
                    resultado = new RetornoOperacion(e.Message + "<br/>" + "Error con la transformación local, la cadena original no pudo ser generada.");
                }
            }

            //Si no hay errores hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Intentando generar Cadena Original
                try
                {
                    //Instanciamos de manera temporal un flujo de escritura de archivos
                    using (MemoryStream flujo = new MemoryStream())
                    {
                        //Colocando posición sobre primer elemento para evitar lecturas vacias del flujo
                        flujoCFD.Position = 0;

                        //Instanciando Navegador 
                        XPathDocument d = new XPathDocument(flujoCFD);

                        //Aplicamos el estilo
                        xslt.Transform(d, null, flujo);

                        //Creamos un flujo de lectura
                        var s = new StreamReader(flujo);

                        //Si el flujo de lectura existe
                        if (s != null)
                        {
                            //Posicionando al inicio del flujo
                            s.BaseStream.Position = 0;

                            //Estableciendo valor de retorno
                            cadena_original = s.ReadToEnd();
                        }

                        //Cerrando lector de flujo con el archivo temporal
                        s.Close();

                        //Si NO se generó conn la versión en linea
                        if (!enLinea)
                            //Actualizando mensaje de resultado
                            resultado = new RetornoOperacion("Error con la transformación en linea, se generó la cadena original con una versión local.", true);
                    }
                }
                //En caso de error
                catch (Exception ex)
                {
                    //Registrando error
                    resultado = new RetornoOperacion(ex.Message);
                }
            }

            //Devolviendo cadena resultante
            return resultado;
        }

        public static RetornoOperacion SellaCadenaOriginal3_3(Stream flujoCFD, string archivo_xslt, string archivo_xslt_alternativo, out string cadena_original)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion("Transformación exitosa.", true);
            cadena_original = "";

            bool enLinea = true;

            //Instanciamos un nuevo objecto de compilacion xslt
            var xslt = new XslCompiledTransform();

            //Intentando generar Cadena Original
            try
            {
                //Cargamos nuestro archivo xslt de la ruta indicada
                xslt.Load(archivo_xslt);
            }
            catch (Exception ex)
            {
                //Estableciendo error con la versión en linea
                enLinea = false;

                try
                {
                    //Cargamos nuestro archivo local .xslt de la ruta indicada
                    xslt.Load(archivo_xslt_alternativo);
                }
                catch (Exception e)
                {
                    result = new RetornoOperacion(e.Message + "<br/>" + "Error con la transformación local, la cadena original no pudo ser generada.");
                }
            }

            //Validanco Operación Exitosa
            if (result.OperacionExitosa)
            {
                try
                {
                    //Instanciando Navegador 
                    XPathDocument d = new XPathDocument(flujoCFD);
                    StringWriter stringWrite = new System.IO.StringWriter();

                    //Transformando a Cadena Original
                    xslt.Transform(d, null, stringWrite);

                    //Estableciendo valor de retorno
                    cadena_original = stringWrite.ToString();

                    //Terminando Escritura
                    stringWrite.Close();
                }
                catch (Exception ex)
                {
                    //Registrando error
                    result = new RetornoOperacion(ex.Message);
                }

                //Si NO se generó conn la versión en linea
                if (!enLinea)
                    //Actualizando mensaje de resultado
                    result = new RetornoOperacion("Error con la transformación en linea, se generó la cadena original con una versión local.", true);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region CFDI 3.2

        /// <summary>
        /// Metodo encargado de firmar la cadena original en formato UTF8 con algoritmo de digestion MD5
        /// </summary>
        /// <param name="cadenaMD5">Cadena en formato UTF8 que será firmada</param>
        /// <param name="rutaArchivoKey">Ruta física de almacenamiento de la llave privada</param>
        /// <param name="contraseñaKey">Contraseña para abrir la llave privada</param>
        public static string FirmaCadenaMD5(byte[] cadenaUTF8, string rutaArchivoKey, string contraseñaKey)
        {
            //Declarando variable de retorno
            string sello = "";

            //Obtenemos la contraseña en tipo SecureString 
            SecureString contraseña = Cadena.CadenaSegura(contraseñaKey);

            //Convertimos el archivo contraseña a un arreglo de bytes
            byte[] archivoKeyBytes = System.IO.File.ReadAllBytes(rutaArchivoKey);

            //Obtenemos un proveedor de RSA se leen los valores del archivo certificado y se inicializa el proveedor RSA
            RSACryptoServiceProvider RSA = CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(archivoKeyBytes, contraseña);

            //Instanciamos un objeto hash MD5
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            //Firmamos los datos
            byte[] datosFirmados = RSA.SignData(cadenaUTF8, MD5);

            //Convertimos los datos a caracteres imprimibles 
            sello = Convert.ToBase64String(datosFirmados);

            //Devolvinedo resultado
            return sello;
        }

        /// <summary>
        /// Metodo encargado de firmar la cadena original en formato UTF8 con algoritmo de digestion SHA1
        /// </summary>
        /// <param name="cadenaUTF8"></param>
        /// <param name="rutaArchivoKey"></param>
        /// <param name="contraseñaKey"></param>
        /// <returns></returns>
        public static string FirmaCadenaSHA1(byte[] cadenaUTF8, string rutaArchivoKey, string contraseñaKey)
        {
            //Inicialziando variable de retorno
            string sello = "";

            //Convertimos el archivo contraseña a un arreglo de bytes
            byte[] bytesCertificado = System.IO.File.ReadAllBytes(rutaArchivoKey);

            //Obtenemos la contraseña en tipo SecureString 
            SecureString contraseña = Cadena.CadenaSegura(contraseñaKey);

            //Obtenemos un proveedor de RSA se leen los valores del archivo certificado y se inicializa el proveedor RSA
            RSACryptoServiceProvider RSA = CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(bytesCertificado, contraseña);

            //Si fue posible realizar apertura
            if (RSA != null)
            {
                //Instanciamos un objeto hash MD5
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();

                //Firmamos los datos
                byte[] datosFirmados = RSA.SignData(cadenaUTF8, SHA1);

                //Convertimos los datos a caracteres imprimibles 
                sello = Convert.ToBase64String(datosFirmados);
            }

            //Devolviendo el resultado de la firma
            return sello;
        }
        /// <summary>
        /// Metodo encargado de firmar la cadena original en formato UTF8 con algoritmo de digestion SHA1
        /// </summary>
        /// <param name="cadenaUTF8"></param>
        /// <param name="bytesCertificado"></param>
        /// <param name="contraseñaKey"></param>
        /// <returns></returns>
        public static string FirmaCadenaSHA1(byte[] cadenaUTF8, byte[] bytesCertificado, string contraseñaKey)
        {
            //Inicialziando variable de retorno
            string sello = "";

            //Obtenemos la contraseña en tipo SecureString 
            SecureString contraseña = Cadena.CadenaSegura(contraseñaKey);

            //Obtenemos un proveedor de RSA se leen los valores del archivo certificado y se inicializa el proveedor RSA
            RSACryptoServiceProvider RSA = CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(bytesCertificado, contraseña);

            //Si fue posible realizar apertura
            if (RSA != null)
            {
                //Instanciamos un objeto hash MD5
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();

                //Firmamos los datos
                byte[] datosFirmados = RSA.SignData(cadenaUTF8, SHA1);

                //Convertimos los datos a caracteres imprimibles 
                sello = Convert.ToBase64String(datosFirmados);
            }

            //Devolviendo el resultado de la firma
            return sello;
        }
        /// <summary>
        /// Reliza la validación de una firma digital, en razón a los datos originales y los datos firmados
        /// </summary>
        /// <param name="proveedorClavePublica">Proveedor de Servicios de encriptación</param>
        /// <param name="bytesOriginales">Bytes sin firmar</param>
        /// <param name="bytesFirmados">Bytes con la firma digital</param>
        /// <param name="proveedorEncriptacion">Algoritmo de encriptación de datos utilizado</param>
        /// <returns></returns>
        public static bool ValidaFirmaDigital(RSACryptoServiceProvider proveedorClavePublica, byte[] bytesOriginales, byte[] bytesFirmados, object proveedorEncriptacion)
        {
            //Verifica los datos usando el RSACryptoServiceProvider y el algoritmo de encriptación definido
            if (!proveedorClavePublica.VerifyData(bytesOriginales, proveedorEncriptacion, bytesFirmados))
                return false;
            //Si la validación fue correcta
            else
                return true;
        }

        #endregion

        #region CFDI 3.3

        /// <summary>
        /// Metodo encargado de firmar la cadena original en formato UTF8 con algoritmo de digestion SHA256
        /// </summary>
        /// <param name="cadenaUTF8"></param>
        /// <param name="rutaArchivoKey"></param>
        /// <param name="contraseñaKey"></param>
        /// <returns></returns>
        public static string FirmaCadenaSHA256(byte[] cadenaUTF8, string rutaArchivoKey, string contraseñaKey)
        {
            //Inicialziando variable de retorno
            string sello = "";

            //Convertimos el archivo contraseña a un arreglo de bytes
            byte[] bytesCertificado = System.IO.File.ReadAllBytes(rutaArchivoKey);

            //Obtenemos la contraseña en tipo SecureString 
            SecureString contraseña = Cadena.CadenaSegura(contraseñaKey);

            //Obtenemos un proveedor de RSA se leen los valores del archivo certificado y se inicializa el proveedor RSA
            RSACryptoServiceProvider RSA = CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(bytesCertificado, contraseña);

            //Si fue posible realizar apertura
            if (RSA != null)
            {
                //Instanciamos un objeto hash MD5
                SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();

                //Firmamos los datos
                byte[] datosFirmados = RSA.SignData(cadenaUTF8, SHA256);

                //Convertimos los datos a caracteres imprimibles 
                sello = Convert.ToBase64String(datosFirmados);
            }

            //Devolviendo el resultado de la firma
            return sello;
        }
        /// <summary>
        /// Metodo encargado de firmar la cadena original en formato UTF8 con algoritmo de digestion SHA256
        /// </summary>
        /// <param name="cadenaUTF8"></param>
        /// <param name="bytesCertificado"></param>
        /// <param name="contraseñaKey"></param>
        /// <returns></returns>
        public static string FirmaCadenaSHA256(byte[] cadenaUTF8, byte[] bytesCertificado, string contraseñaKey)
        {
            //Inicialziando variable de retorno
            string sello = "";
            byte[] datosFirmados = null;

            //Obtenemos la contraseña en tipo SecureString 
            SecureString contraseña = Cadena.CadenaSegura(contraseñaKey);
            SHA256Managed sham = new SHA256Managed();

            //Obtenemos un proveedor de RSA se leen los valores del archivo certificado y se inicializa el proveedor RSA
            RSACryptoServiceProvider RSA = CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(bytesCertificado, contraseña);

            //Si fue posible realizar apertura
            if (RSA != null)
            {
                try
                {
                    //Firmando Cadena
                    datosFirmados = RSA.SignData(cadenaUTF8, sham);
                    //Convirtiendo Valor en Cadena
                    sello = Convert.ToBase64String(datosFirmados);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ////Instanciamos un objeto hash MD5
                //SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
                ////Firmamos los datos
                //byte[] datosFirmados = RSA.SignData(cadenaUTF8, SHA256);
                ////Convertimos los datos a caracteres imprimibles
                //sello = Convert.ToBase64String(datosFirmados);

                /** Firmando con SHA256 **/
                //SHA256Managed sha = new SHA256Managed();
                //byte[] digest = sha.ComputeHash(cadenaUTF8);
                //RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                //RSAFormatter.SetHashAlgorithm("SHA256");
                //byte[] SignedHashValue = RSAFormatter.CreateSignature(digest);
                //sello = Convert.ToBase64String(SignedHashValue);
            }

            //Devolviendo el resultado de la firma
            return sello;
        }

        #endregion
    }
}