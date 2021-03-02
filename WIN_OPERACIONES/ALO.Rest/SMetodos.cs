using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALO.Entidades;
using ALO.Utilidades;

namespace ALO.Rest
{
    public class SMetodos
    {


        string C_URL;
        string C_RUTA_XSLT;
        string SISTEMA = "VENTAS_PRI_PRE";


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SMetodos(string URL, string XSLT)
        {
            C_URL = URL;
            C_RUTA_XSLT = XSLT;
        }

        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <returns></returns>
        public List<oSP_READ_FIFO_INTERFAZ_DIS> SP_READ_FIFO_INTERFAZ_DIS()
        {
            List<oSP_READ_FIFO_INTERFAZ_DIS> ListaRest = new List<oSP_READ_FIFO_INTERFAZ_DIS>();

            try
            {


                //===========================================================
                // SERVICIO  
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_FIFO_INTERFAZ_DIS>("SP_READ_FIFO_INTERFAZ_DIS", SISTEMA, new object(), new object());


                    //===========================================================
                    // EVALUACIÓN DE CABEZERA 
                    //===========================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_FIFO_INTERFAZ_DIS>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO(iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO Input)
        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_RETURN_STATUS>("SP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO(iSP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO Input)
        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_RETURN_STATUS>("SP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }



       

        /// <summary>
        ///  LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO(iSP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO Input)
        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_RETURN_STATUS>("SP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }




        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <returns></returns>
        public List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC> SP_READ_FIFO_INTERFAZ_DIS_EJEC()
        {
            List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC> ListaRest = new List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC>();

            try
            {


                //===========================================================
                // SERVICIO  
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_FIFO_INTERFAZ_DIS_EJEC>("SP_READ_FIFO_INTERFAZ_DIS_EJEC", SISTEMA, new object(), new object());


                    //===========================================================
                    // EVALUACIÓN DE CABEZERA 
                    //===========================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }






        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <returns></returns>
        public List<oSP_READ_FIFO_INTERFAZ_DIS_DB> SP_READ_FIFO_INTERFAZ_DIS_DB()
        {
            List<oSP_READ_FIFO_INTERFAZ_DIS_DB> ListaRest = new List<oSP_READ_FIFO_INTERFAZ_DIS_DB>();

            try
            {


                //===========================================================
                // SERVICIO  
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_FIFO_INTERFAZ_DIS_DB>("SP_READ_FIFO_INTERFAZ_DIS_DB", SISTEMA, new object(), new object());


                    //===========================================================
                    // EVALUACIÓN DE CABEZERA 
                    //===========================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_FIFO_INTERFAZ_DIS_DB>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_DELETE_FIFO_INTERFAZ_DIS(iSP_DELETE_FIFO_INTERFAZ_DIS Input)
        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_RETURN_STATUS>("SP_DELETE_FIFO_INTERFAZ_DIS", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }




        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_INTERFAZ_DETALLE> SP_READ_INTERFAZ_DETALLE(iSP_READ_INTERFAZ_DETALLE Input)
        {
            List<oSP_READ_INTERFAZ_DETALLE> ListaRest = new List<oSP_READ_INTERFAZ_DETALLE>();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_INTERFAZ_DETALLE>("SP_READ_INTERFAZ_DETALLE", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_INTERFAZ_DETALLE>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }






        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<oSP_READ_TABLA_INTERFAZ> SP_READ_TABLA_INTERFAZ(iSP_READ_TABLA_INTERFAZ Input)
        {
            List<oSP_READ_TABLA_INTERFAZ> ListaRest = new List<oSP_READ_TABLA_INTERFAZ>();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_TABLA_INTERFAZ>("SP_READ_TABLA_INTERFAZ", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_TABLA_INTERFAZ>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }





        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public oSP_RETURN_STATUS SP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO(iSP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO Input)
        {
            oSP_RETURN_STATUS ObjetoRest = new oSP_RETURN_STATUS();

            try
            {
                //===========================================================
                // SERVICIO 
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_RETURN_STATUS>("SP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO", SISTEMA, Input, new object());


                    if (ESTADO == 1)
                    {
                        ObjetoRest = (oSP_RETURN_STATUS)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ObjetoRest;


            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// LECTURA DE METODO REST
        /// </summary>
        /// <returns></returns>
        public List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE> SP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE()
        {
            List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE> ListaRest = new List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE>();

            try
            {


                //===========================================================
                // SERVICIO  
                //===========================================================
                using (SRestFul Servicio = new SRestFul(C_URL))
                {


                    //===========================================================
                    // LLAMADA DEL SERVICIO  
                    //===========================================================
                    int ESTADO = Servicio.SolicitarWCFPost<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE>("SP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE", SISTEMA, new object(), new object());


                    //===========================================================
                    // EVALUACIÓN DE CABEZERA 
                    //===========================================================
                    if (ESTADO == 1)
                    {
                        ListaRest = (List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE>)Servicio.ObjetoRest;
                    }
                    else
                    {
                        ErroresException Error = (ErroresException)Servicio.ObjetoRest;
                        throw new EServiceRestFulException(UThrowError.MensajeThrow(Error, C_RUTA_XSLT));
                    }
                }


                return ListaRest;


            }
            catch
            {
                throw;
            }
        }




    }
}
