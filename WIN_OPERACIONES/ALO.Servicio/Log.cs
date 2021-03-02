using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ALO.Servicio
{
    public static class Log
    {


        /// <summary>
        /// ESCRITURA DE LOG
        /// </summary>
        /// <param name="Message"></param>
        public static void WriteMensajeLog(string Message)
        {

            //=========================================================
            // DECLARACION DE VARIABLES 
            //=========================================================
            string Archivo = AppDomain.CurrentDomain.BaseDirectory + "\\LogServices.txt";
            StreamWriter Stream = null;



            //=========================================================
            // OPERACION DE ARCHIVOS 
            //=========================================================
            try
            {

                if (!File.Exists(Archivo))
                {

                    Stream = new StreamWriter(Archivo);
                    Stream.WriteLine(DateTime.Now.ToString() + " : " + Message);
                    Stream.Close();

                }
                else
                {
                    Stream = new StreamWriter(Archivo, true);
                    Stream.WriteLine(DateTime.Now.ToString() + " : " + Message);
                    Stream.Flush();
                    Stream.Close();
                }


            }
            catch { }



        }

    }
}
