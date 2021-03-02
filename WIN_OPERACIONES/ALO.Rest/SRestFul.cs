using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALO.Entidades;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Data;
using System.Xml;

namespace ALO.Rest
{
    public class SRestFul : IDisposable
    {



        public object ObjetoRest;
        string C_URL;

        /// <summary>
        /// DESTRUCTOR
        /// </summary>
        public void Dispose()
        {
            ObjetoRest = null;
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SRestFul(string URL)
        {
            C_URL = URL;
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
            WebRequest.DefaultWebProxy = null;
        }




        /// <summary>
        /// SOLICITAR INFORMACION
        /// </summary>
        /// <returns></returns>
        public int SolicitarWCFPost<T>(string SP
                                    , string Sistema
                                    , object Parametros
                                    , object Filtros)
        {



            OUTPUT_JSON_ALO Retorno = new OUTPUT_JSON_ALO();
            HttpWebRequest myRequest = null;

            try
            {

                //=============================================================
                // CONTRUCCION DE OBJETO INPUT                               ==
                //=============================================================
                INPUT_JSON_ALO ObjetoInput = new INPUT_JSON_ALO();
                ObjetoInput.R_METODO = new R_METODO { SP = SP, SISTEMA = Sistema };
                ObjetoInput.R_PARAM.PARAMETROS = Parametros;
                ObjetoInput.R_FILTRO.PARAMETROS = Filtros;

                //=============================================================
                // SE DEBE CONVERTIR EL OBJETO EN JSON PARA SER ENVIADO      ==
                //=============================================================
                string Json = "";
                using (SObjetoJson ObjetoParametros = new SObjetoJson())
                {
                    Json = ObjetoParametros.Serialize(ObjetoInput);
                    ObjetoParametros.Dispose();
                }


                //=============================================================
                // URL DE SERVICIO                                           ==
                //=============================================================
                Uri UrlDestino = new Uri(C_URL);


                //=============================================================
                // DATOS JSON EN URL                                         ==
                //=============================================================
                StringBuilder StrWebservices = new StringBuilder();
                string econding = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                string EncabezadoWS = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:alo=""alo.ibrlatam.com"">";
                string CabezeraWS = @"<soapenv:Header/><soapenv:Body><alo:RF_DB_POST>";
                string FooterWS = @"</alo:RF_DB_POST></soapenv:Body> </soapenv:Envelope>";



                StrWebservices.AppendLine(econding);
                StrWebservices.AppendLine(EncabezadoWS);
                StrWebservices.AppendLine(CabezeraWS);
                StrWebservices.AppendLine("<alo:json>" + Json + "</alo:json>");
                StrWebservices.AppendLine(FooterWS);


                //=============================================================
                // DATOS A BINARIOS
                //=============================================================
                byte[] data = Encoding.ASCII.GetBytes(StrWebservices.ToString());



                //=============================================================
                // SERVICIO RESTFUL                                          ==
                //=============================================================
                myRequest = (HttpWebRequest)WebRequest.Create(UrlDestino.ToString());
                myRequest.Method = "POST";
                myRequest.ContentLength = data.Length;
                myRequest.ContentType = "text/xml; charset=utf-8";
                myRequest.Headers.Add(@"SOAPAction: ""alo.ibrlatam.com/IServicioRestAlo/RF_DB_POST""");
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                myRequest.UserAgent = SP;


                myRequest.AllowAutoRedirect = false;
                myRequest.KeepAlive = false;
                myRequest.ProtocolVersion = HttpVersion.Version11;

                myRequest.Timeout = 10000;
                myRequest.ReadWriteTimeout = 10000;
                myRequest.Proxy = null;
                myRequest.ServicePoint.ConnectionLimit = 1000000;


                //=============================================================
                // PASAR POST
                //=============================================================
                using (var stream = myRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);

                    stream.Flush();
                    stream.Close();
                    stream.Dispose();


                }


                //=============================================================
                // ANULA OBJETO                                              ==
                //=============================================================
                StrWebservices = null;


                //=============================================================
                // RESCATO VALORES                                           ==
                //=============================================================
                string JsonReturn = "";


                using (var resp = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (Stream Str = resp.GetResponseStream())
                    {





                        if (resp.ContentEncoding.ToLower().Contains("gzip"))
                        {


                            using (Stream Compresion = new GZipStream(Str, CompressionMode.Decompress))
                            {
                                using (StreamReader rd = new StreamReader(Compresion))
                                {
                                    JsonReturn = rd.ReadToEnd();
                                    rd.Close();
                                    rd.Dispose();
                                }

                                Compresion.Close();
                                Compresion.Dispose();

                            }


                        }
                        else
                        {
                            if (resp.ContentEncoding.ToLower().Contains("deflate"))
                            {

                                using (Stream Compresion = new DeflateStream(Str, CompressionMode.Decompress))
                                {
                                    using (StreamReader rd = new StreamReader(Compresion))
                                    {
                                        JsonReturn = rd.ReadToEnd();
                                        rd.Close();
                                        rd.Dispose();
                                    }
                                    Compresion.Close();
                                    Compresion.Dispose();
                                }


                            }

                            else
                            {
                                using (StreamReader rd = new StreamReader(Str))
                                {
                                    JsonReturn = rd.ReadToEnd();
                                    rd.Close();
                                    rd.Dispose();
                                }

                            }

                        }



                        Str.Flush();
                        Str.Close();
                        Str.Dispose();
                    }


                    resp.Close();



                }


                myRequest = null;


                //=============================================================
                // PARSING XML                                               ==
                //=============================================================
                XmlDocument doc = new XmlDocument();

                try
                {

                    doc.LoadXml(JsonReturn);

                }
                catch
                {

                    if (JsonReturn.Length > 0)
                    {

                        throw new Exception("XML DEVUELTO NO CORRESPONDE POR PARTE DE WCF(1)");
                    }


                }

                //=============================================================
                // REEMPLAZO DE CODIGO                                       ==
                //=============================================================
                try
                {


                    var soapBody = doc.GetElementsByTagName("s:Body")[0];
                    string innerObject = soapBody.InnerXml;
                    innerObject = innerObject.Replace(@"xmlns=""alo.ibrlatam.com""", "");


                    doc.LoadXml(innerObject);



                }
                catch
                {
                    throw new Exception("XML DEVUELTO NO CORRESPONDE POR PARTE DE WCF(2)");

                }


                //=============================================================
                // LECTURA DE NODOS
                //=============================================================
                XmlNodeList DireccionNodo = null;
                string NewJson = "";

                try
                {
                    /*----------------------------------------------------*/
                    /* LECTURA DE NODOS                                   */
                    /*----------------------------------------------------*/
                    DireccionNodo = doc.SelectNodes(@"//RF_DB_POSTResponse");
                    foreach (XmlNode Nodo in DireccionNodo)
                    {

                        if (Nodo != null)
                        {
                            NewJson = Nodo["RF_DB_POSTResult"].InnerText;

                        }
                    }
                }
                catch
                {
                    throw new Exception("ESTRUCTURA WCF NO TRAJO DATOS JSON DE CONFIGURACIÓN");

                }

                //=============================================================
                // VER SI ESTE CONTIENE DATOS
                //=============================================================
                if (NewJson.Length <= 0)
                {
                    throw new Exception("JSON RETURN WCF VIENE VACIO");
                }

                doc = null;


                //=============================================================
                // SE DEBE DESERIALIZAR RESULTADOS                           ==
                //=============================================================
                using (SObjetoJson ObjetoRetorno = new SObjetoJson())
                {
                    Retorno = ObjetoRetorno.Deserialize<OUTPUT_JSON_ALO>(NewJson);
                    ObjetoRetorno.Dispose();
                }

                //=============================================================
                // COMPROBAR LA EJECUCION                                    ==
                //=============================================================
                int ESTADO = Retorno.HEADER.ESTADO;
                int ID_TIPO_RETORNO = Retorno.HEADER.ID_TIPO_RETORNO;

                if (ESTADO == 1)
                {



                    //=========================================================
                    // RETURN STATUS                                         ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 1)
                    {
                        T Objeto;

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();


                        using (SObjetoJson ObjetoSer = new SObjetoJson())
                        {
                            Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                            ObjetoSer.Dispose();
                        }



                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS                                                 ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 2)
                    {
                        List<T> Lista = new List<T>();

                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();


                        using (SObjetoJson ObjetoSer = new SObjetoJson())
                        {
                            Lista = ObjetoSer.Deserialize<List<T>>(JsonDetalle);
                            ObjetoSer.Dispose();
                        }


                        ObjetoRest = Lista;
                    }

                    //=========================================================
                    // OUTPUT                                                ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 3)
                    {
                        T Objeto;


                        string JsonDetalle = Retorno.RESULT.DETALLES.ToString();

                        using (SObjetoJson ObjetoSer = new SObjetoJson())
                        {
                            Objeto = ObjetoSer.Deserialize<T>(JsonDetalle);
                            ObjetoSer.Dispose();
                        }



                        ObjetoRest = Objeto;
                    }

                    //=========================================================
                    // DATOS DINAMICOS                                       ==
                    //=========================================================
                    if (ID_TIPO_RETORNO == 4 || ID_TIPO_RETORNO == 5)
                    {
                        throw new Exception("ESTE METODO NO SOPORTA LOS TIPOS DE RETORNOS DINAMICOS");
                    }




                }
                else
                {
                    ObjetoRest = Retorno.ERRORES;

                }

                return ESTADO;

            }
            catch
            {

                throw;

            }
            finally
            {
                if (myRequest != null)
                {
                    myRequest = null;
                }
            }


        }


    }

}
