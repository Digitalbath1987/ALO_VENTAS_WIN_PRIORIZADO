using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ALO.Entidades;

namespace ALO.Utilidades
{
    public static class UMensaje
    {


        /// <summary>
        /// ENVIAR MENSAJE A APLICACION
        /// </summary>
        /// <param name="MENSAJE"></param>
        public static void EnviaMensaje(string LOGIN,string MENSAJE)
        {
            try
            {


                //============================================================
                // CONSOLA DONDE SE ENCUENTRA EL ENSAMBLADO
                //============================================================
                FileInfo FILE = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);

                //============================================================
                // BUSCAR ARCHIVO DE CONFIGURACION
                //============================================================
                string C_FILE_CONFIGURACION = FILE.DirectoryName + @"\XML\CONFIGURACION.XML";


                //=========================================================
                // CONFIGURACION
                //=========================================================
                if (File.Exists(C_FILE_CONFIGURACION) == false)
                {
                    throw new Exception("EL ARCHIVO CONFIGURACION NO EXISTE EN INSTALACIÓN DE SERVICIO");

                }

                //=========================================================
                // CONFIGURACIONES DEL SERVICIO
                //=========================================================
                XML_CONFIGURACIONES C_CON = new XML_CONFIGURACIONES();
                C_CON = UArchivoConfiguracion.CONFIG_APP(C_FILE_CONFIGURACION);


                if (C_CON == null)
                {
                    throw new Exception("ARCHIVO DE CONFIGURACION NO FUE LEIDO POR CODIGO DE MENSAJES");

                }




                //===========================================================
                // LLAMAR ASHX POST                                        
                //===========================================================
                string URL = C_CON.MENSAJE + "?LOGIN=" + LOGIN +"&MENSAJE=" + MENSAJE;
                string response = (new WebClient()).DownloadString(URL);



            }
            catch 
            {
                throw;
            }


        }


    }
}
