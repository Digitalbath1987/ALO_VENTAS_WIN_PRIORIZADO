using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALO.Entidades;
using System.Xml.Linq;

namespace ALO.Utilidades
{
    public class UArchivoConfiguracion
    {

        /// <summary>
        /// LEER ARCHIVO DE CONFIGURACION
        /// </summary>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static XML_CONFIGURACIONES CONFIG_APP(string Archivo)
        {




            try
            {






                //=========================================================
                // ARCHIVO DE ORIGEN DE CONFIGURACIONES VERIFICAR 
                //=========================================================
                if (System.IO.File.Exists(Archivo) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO DE CONFIGURACION :" + Archivo);
                }
                //=========================================================
                // LECTURA DE ARCHIVOS
                //=========================================================
                XDocument DOCUMENTO_XML = XDocument.Load(Archivo);


                var definitions = DOCUMENTO_XML.Descendants("add")
                     .Select(x => new
                     {
                         key = x.Attribute("key").Value,
                         value = x.Attribute("value").Value
                     });

                XML_CONFIGURACIONES Configuraciones = new XML_CONFIGURACIONES();


                foreach (var def in definitions)
                {

                    switch (def.key)
                    {
                        case "SERVICIO":
                            Configuraciones.SERVICIO = def.value;
                            break;
                        case "MENSAJE":
                            Configuraciones.MENSAJE = def.value;
                            break;
                        case "DESCARGA":
                            Configuraciones.DESCARGA = def.value;
                            break;
                        case "RUTA":
                            Configuraciones.RUTA = def.value;
                            break;
                        case "RUTA_TOKEN":
                            Configuraciones.RUTA_TOKEN = def.value;
                            break;
                        case "PS":
                            Configuraciones.PS = def.value;
                            break;
                        case "intervalTime_1":
                            Configuraciones.intervalTime_1 = def.value;
                            break;
                        default:
                            break;
                    }

                }




                return Configuraciones;


            }
            catch
            {

                throw;
            }

        }


        /// <summary>
        /// CONFIGURACION DEL SERVER
        /// </summary>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static XML_CONFIG_SERVER CONFIG_APP_SERVER(string Archivo)
        {




            try
            {

                //============================================================
                // ARCHIVO DE ORIGEN DE CONFIGURACIONES VERIFICAR 
                //============================================================
                if (System.IO.File.Exists(Archivo) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO DE CONFIGURACION :" + Archivo);
                }

                //============================================================
                // LECTURA DE ARCHIVO
                //============================================================
                XDocument DOCUMENTO_XML = XDocument.Load(Archivo);


                var definitions = DOCUMENTO_XML.Descendants("add")
                     .Select(x => new
                     {
                         key = x.Attribute("key").Value,
                         value = x.Attribute("value").Value
                     });

                XML_CONFIG_SERVER Configuraciones = new XML_CONFIG_SERVER();


                foreach (var def in definitions)
                {

                    switch (def.key)
                    {
                        case "SERVIDOR":
                            Configuraciones.SERVIDOR = def.value;
                            break;
                        case "USUARIO":
                            Configuraciones.USUARIO = def.value;
                            break;
                        case "PASSWORD":
                            Configuraciones.PASSWORD = def.value;
                            break;
                        default:
                            break;
                    }

                }




                return Configuraciones;


            }
            catch
            {

                throw;
            }

        }
        /// <summary>
        /// CONFIGURACIONES DE CORREO
        /// </summary>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static XML_CONFIGURACIONES_EMAIL CONFIG_APP_EMAIL(string Archivo)
        {




            try
            {







                //============================================================
                // ARCHIVO DE ORIGEN DE CONFIGURACIONES VERIFICAR 
                //============================================================
                if (System.IO.File.Exists(Archivo) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO DE CONFIGURACION :" + Archivo);
                }

                //============================================================
                // LECTURA DE ARCHIVO
                //============================================================
                XDocument DOCUMENTO_XML = XDocument.Load(Archivo);


                var definitions = DOCUMENTO_XML.Descendants("add")
                     .Select(x => new
                     {
                         key = x.Attribute("key").Value,
                         value = x.Attribute("value").Value
                     });

                XML_CONFIGURACIONES_EMAIL Configuraciones = new XML_CONFIGURACIONES_EMAIL();


                foreach (var def in definitions)
                {

                    switch (def.key)
                    {
                        case "HOST":
                            Configuraciones.HOST = def.value;
                            break;
                        case "PUERTO":
                            Configuraciones.PUERTO = def.value;
                            break;
                        case "CORREO_FROM":
                            Configuraciones.CORREO_FROM = def.value;
                            break;
                        case "USA_CREDENCIALES":
                            Configuraciones.USA_CREDENCIALES = def.value;
                            break;
                        case "USA_SSL":
                            Configuraciones.USA_SSL = def.value;
                            break;
                        case "USUARIO":
                            Configuraciones.USUARIO = def.value;
                            break;
                        case "PASSWORD":
                            Configuraciones.PASSWORD = def.value;
                            break;
                        default:
                            break;
                    }

                }




                return Configuraciones;


            }
            catch
            {

                throw;
            }

        }

    }
}
