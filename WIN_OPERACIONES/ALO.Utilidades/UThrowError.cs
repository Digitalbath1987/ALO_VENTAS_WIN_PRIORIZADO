using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using ALO.Entidades;

namespace ALO.Utilidades
{
    public static class UThrowError
    {






        /// <summary>
        /// CONFIGURACION DE XML ERROR
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static XmlDocument ConfiguraXML(Exception ex)
        {

            XmlDocument Documento = new XmlDocument();


            try
            {

                //=============================================================
                // DECLARACION DE VARIABLES                                  ==
                //=============================================================
                ErroresException Entidad = new ErroresException();



                MethodBase MetodoGatillante = ex.TargetSite;
                Type Class = MetodoGatillante.ReflectedType;

                //=============================================================
                // OPCIONES DE METODO GATILLANTE                             ==
                //=============================================================
                Entidad.NombreMetodo = MetodoGatillante.Name;
                Entidad.Clase = Class.FullName;
                Entidad.NameSpace = Class.Namespace;

                string Mensaje = ex.Message;
                Mensaje = Mensaje.Replace("\"", " ");
                Mensaje = Mensaje.Replace("'", " ");


                Entidad.Mensaje = Mensaje;


                //=============================================================
                // METODO EN CUAL FUE LA CAIDA                               ==
                //=============================================================
                List<Secuencia> Lista = new List<Secuencia>();
                StackTrace st = new StackTrace(ex, true);
                for (int i = st.FrameCount - 1; i >= 0; i--)
                {

                    StackFrame sf = st.GetFrame(i);
                    System.Reflection.MethodBase method = sf.GetMethod();
                    Lista.Add(new Secuencia { Item = method.Name });
                }
                Entidad.Eventos = Lista;


                //=============================================================
                // CREACION DE XML                                           ==
                //=============================================================
                Documento = GetEntityXml(Entidad, "Detalles");


                return Documento;


            }
            catch
            {
                return Documento;

            }

        }




        /// <summary>
        /// CONVERTIR LISTA EN OBJETO XML
        /// </summary>
        /// <param name="Entidad"></param>
        /// <param name="Root"></param>
        /// <returns></returns>
        public static XmlDocument GetEntityXml(ErroresException Entidad, String Root)
        {


            XmlDocument xmlDoc = new XmlDocument();

            try
            {


                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attr = new XmlAttributes();
                attr.XmlRoot = new XmlRootAttribute(Root);
                overrides.Add(typeof(ErroresException), attr);



                XPathNavigator nav = xmlDoc.CreateNavigator();
                using (XmlWriter writer = nav.AppendChild())
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ErroresException), overrides);
                    ser.Serialize(writer, Entidad, ns);
                }




                return xmlDoc;
            }
            catch
            {

                return xmlDoc;
            }
        }


        /// <summary>
        /// MENSAJES THROW 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string MensajeThrow(ErroresException Err, string C_RUTA_XSLT)
        {

            string Mensaje = "";

            try
            {


                //=============================================================
                // DECLARACION DE VARIABLES                                  ==
                //=============================================================
                XmlDocument Documento = new XmlDocument();



                //=============================================================
                // CONFIGURA XML                                             ==
                //=============================================================
                Documento = GetEntityXml(Err, "Detalles");


                //=============================================================
                // RUTA XSLT                                                 ==
                //=============================================================
                string RUTA_XSLT = C_RUTA_XSLT;


                //=============================================================
                // TRANSFORMA XML                                            ==
                //=============================================================
                XPathDocument doc = new XPathDocument(new StringReader(Documento.InnerXml.ToString()));
                XslTransform xslt = new XslTransform();
                xslt.Load(RUTA_XSLT);
                StringWriter sw = new StringWriter();
                xslt.Transform(doc, null, sw);


                return sw.ToString();


            }
            catch
            {
                return Mensaje;
            }

        }

        /// <summary>
        /// MENSAJES THROW 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string MensajeThrow(Exception ex, string C_RUTA_XSLT)
        {

            string Mensaje = "";

            try
            {


                //=============================================================
                // DECLARACION DE VARIABLES                                  ==
                //=============================================================
                XmlDocument Documento = new XmlDocument();



                //=============================================================
                // CONFIGURA XML                                             ==
                //=============================================================
                Documento = ConfiguraXML(ex);


                //=============================================================
                // RUTA XSLT                                                 ==
                //=============================================================
                string RUTA_XSLT = C_RUTA_XSLT;


                //=============================================================
                // TRANSFORMA XML                                            ==
                //=============================================================
                XPathDocument doc = new XPathDocument(new StringReader(Documento.InnerXml.ToString()));
                XslTransform xslt = new XslTransform();
                xslt.Load(RUTA_XSLT);
                StringWriter sw = new StringWriter();
                xslt.Transform(doc, null, sw);


                return sw.ToString();


            }
            catch
            {
                return Mensaje;
            }

        }

    }

}
