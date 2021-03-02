using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using ALO.Entidades;

namespace ALO.Utilidades
{
    public static class UEmail
    {


        /// <summary>
        /// ENVIO DE CORREO ELECTRONICO
        /// </summary>
        /// <param name="ASUNTO"></param>
        /// <param name="DISPLAYNAME"></param>
        /// <param name="LISTA_DISTRIBUCION"></param>
        /// <param name="HTML_VIEW"></param>
        /// <param name="CONFIGURACION"></param>
        /// <returns></returns>
        public static bool ENVIA_CORREO_ELECTRONICO(string ASUNTO
                                           , string DISPLAYNAME
                                           , string LISTA_DISTRIBUCION
                                           , AlternateView HTML_VIEW
                                           , XML_CONFIGURACIONES_EMAIL CONFIGURACION)
        {


            //===========================================================
            // CONFIGURACION DE CORREO ELECTRONICO 
            //===========================================================
            SmtpClient CLI_SMTP = CLIENTE_SMTP(CONFIGURACION);



            //===========================================================
            // CORREO FROM
            //===========================================================
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(CONFIGURACION.CORREO_FROM, DISPLAYNAME);


            //===========================================================
            // CONTENIDO XSLT 
            //===========================================================
            msg.AlternateViews.Add(HTML_VIEW);

            //===========================================================
            // LISTA DE DISTRIBUCION
            //===========================================================
            string[] Correos = LISTA_DISTRIBUCION.Split(';');

            foreach (string s in Correos)
            {

                if (s.Length > 6)
                {
                    msg.To.Add(new MailAddress(s.ToString().Trim()));
                }
            }




            msg.IsBodyHtml = true;
            msg.Subject = ASUNTO;




            try
            {


                CLI_SMTP.Send(msg);
                CLI_SMTP = null;
                msg.Dispose();
                msg = null;
                return true;

            }
            catch
            {
                CLI_SMTP = null;
                msg.Dispose();
                msg = null;
                throw;
            }

        }


        /// <summary>
        /// DATOS DE CORREO
        /// </summary>
        /// <param name="INFO_SERVICIO"></param>
        /// <returns></returns>
        private static SmtpClient CLIENTE_SMTP(XML_CONFIGURACIONES_EMAIL INFO_SERVICIO)
        {


            //===========================================================
            // DECLARACION DE VARIABLES
            //===========================================================
            SmtpClient CLIENTE = new SmtpClient();
            CLIENTE.Timeout = 10000;

            //===========================================================
            // CONFIGURACION DE CORREO 
            //===========================================================
            CLIENTE.Host = INFO_SERVICIO.HOST;
            CLIENTE.Port = Convert.ToInt32(INFO_SERVICIO.PUERTO);
            CLIENTE.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (INFO_SERVICIO.USA_CREDENCIALES == "1")
            {
                CLIENTE.UseDefaultCredentials = true;
                CLIENTE.Credentials = new NetworkCredential(INFO_SERVICIO.USUARIO, INFO_SERVICIO.PASSWORD);
            }
            else
            {
                CLIENTE.UseDefaultCredentials = false;
            }

            if (INFO_SERVICIO.USA_SSL == "1")
            {
                CLIENTE.EnableSsl = true;
            }
            else
            {
                CLIENTE.EnableSsl = false;
            }




            return CLIENTE;

        }

    }
}
