using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using ALO.Entidades;
using System.IO;
using ALO.Utilidades;
using System.Net.Mail;
using System.Net.Mime;
using ALO.Rest;
using System.Management;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data.SqlClient;


namespace ALO.Servicio
{
    partial class Main : ServiceBase
    {

        //==================================================================
        // DECLARACION DE VARIABLES SERVIVICIO
        //==================================================================
        private readonly Timer C_myTimer_1;
        private readonly Timer C_myTimer_2;
        private readonly Timer C_myTimer_3;
        private readonly Timer C_myTimer_4;
        private readonly Timer C_myTimer_5;
        private readonly Timer C_myTimer_6;


        //==================================================================
        // ARCHIVOS DE CONFIGURACION
        //==================================================================
        static XML_CONFIGURACIONES C_CON;
        static XML_CONFIGURACIONES_EMAIL C_CON_SMTP;


        //==================================================================
        // SEMAFORO
        //==================================================================
        static bool EN_PROCESO_TIMER_1;
        static bool EN_PROCESO_TIMER_2;
        static bool EN_PROCESO_TIMER_3;
        static bool EN_PROCESO_TIMER_4;
        static bool EN_PROCESO_TIMER_5;
        static bool EN_PROCESO_TIMER_6;


        //==================================================================
        // ARCHIVOS
        //==================================================================
        static string C_FILE_CONFIGURACION;
        static string C_FILE_XSLT_LOG;
        static string C_FILE_XSLT_CORREO;
        static string C_FILE_XSLT_CORREO_DIS;
        static string C_FILE_RUTA_EJECUCION;
        static string C_FILE_DB_INPUT;
        static string C_FILE_SMTP;
        static string C_FILE_IMAGEN;
        static string C_FILE_FMT;
        static string C_FILE_RUTA_TOKEN;


        const string Comillas = "\"";

        /// <summary>
        /// SERVICIO ALO INICIO
        /// </summary>
        public Main()
        {

            try
            {
                InitializeComponent();




                //=========================================================
                // ELIMINA LOG DE ARCHIVOS ANTERIORES
                //=========================================================
                DateTime FECHA = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                FileInfo FILE_LOG = new FileInfo(FILE_ENSAMBLADO.DirectoryName + @"\LogServices.txt");


                if (FILE_LOG.CreationTime < FECHA)
                {
                    File.Delete(FILE_LOG.FullName);
                }


                //=========================================================
                // SEMAFORO EN FALSE AL INICIAR
                //=========================================================
                EN_PROCESO_TIMER_1 = false;
                EN_PROCESO_TIMER_2 = false;
                EN_PROCESO_TIMER_3 = false;
                EN_PROCESO_TIMER_4 = false;
                EN_PROCESO_TIMER_5 = false;
                EN_PROCESO_TIMER_6 = false;


                //=========================================================
                // ARCHIVOS
                //=========================================================
                C_FILE_CONFIGURACION = FILE_ENSAMBLADO.DirectoryName + @"\XML\CONFIGURACION.XML";
                C_FILE_SMTP = FILE_ENSAMBLADO.DirectoryName + @"\XML\SMTP.XML";
                C_FILE_XSLT_LOG = FILE_ENSAMBLADO.DirectoryName + @"\XSLT\LOG.XSLT";
                C_FILE_XSLT_CORREO = FILE_ENSAMBLADO.DirectoryName + @"\XSLT\TOKEN.XSLT";
                C_FILE_XSLT_CORREO_DIS = FILE_ENSAMBLADO.DirectoryName + @"\XSLT\DISCADOR.XSLT";
                C_FILE_DB_INPUT = FILE_ENSAMBLADO.DirectoryName + @"\DB\InputParameterSP.sql";
                C_FILE_IMAGEN = FILE_ENSAMBLADO.DirectoryName + @"\IMAGEN\LOGO.png";
                C_FILE_FMT = FILE_ENSAMBLADO.DirectoryName + @"\fmt";

                //=========================================================
                // CONFIGURACION
                //=========================================================
                if (File.Exists(C_FILE_CONFIGURACION) == false)
                {
                    throw new Exception("EL ARCHIVO CONFIGURACION NO EXISTE EN INSTALACIÓN DE SERVICIO");

                }

                //=========================================================
                // CONFIGURACION
                //=========================================================
                if (File.Exists(C_FILE_SMTP) == false)
                {
                    throw new Exception("EL ARCHIVO CONFIGURACION SMTP NO EXISTE EN INSTALACIÓN DE SERVICIO");

                }

                //=========================================================
                // CONFIGURACION LOG
                //=========================================================
                if (File.Exists(C_FILE_XSLT_LOG) == false)
                {
                    throw new Exception("EL ARCHIVO XSLT DE ARCHIVOS LOG NO SE ENCUENTRA EN SISTEMA");

                }

                //=========================================================
                // CONFIGURACION CORREO
                //=========================================================
                if (File.Exists(C_FILE_XSLT_CORREO) == false)
                {
                    throw new Exception("EL ARCHIVO XSLT DE ARCHIVOS CORREO NO SE ENCUENTRA EN SISTEMA");

                }

                //=========================================================
                // CONFIGURACION CORREO
                //=========================================================
                if (File.Exists(C_FILE_XSLT_CORREO_DIS) == false)
                {
                    throw new Exception("EL ARCHIVO XSLT DE ARCHIVOS CORREO DISCADOR NO SE ENCUENTRA EN SISTEMA");

                }

                //=========================================================
                // IAMGEN CORREOS
                //=========================================================
                if (File.Exists(C_FILE_IMAGEN) == false)
                {
                    throw new Exception("EL ARCHIVO DE IMAGEN NO SE ENCUENTRA EN SISTEMA");

                }



                //=========================================================
                // CONFIGURACION LOG
                //=========================================================
                if (File.Exists(C_FILE_DB_INPUT) == false)
                {
                    throw new Exception("EL ARCHIVO DB INPUT NO SE ENCUENTRA EN SISTEMA");

                }

                //=========================================================
                // RUTA DE DESTINO FMT
                //=========================================================
                if (Directory.Exists(C_FILE_FMT) == false)
                {
                    throw new Exception("NO EXISTE RUTA FMT DE DESTINO EN SERVICIO");
                }

                //=========================================================
                // CONFIGURACIONES DEL SERVICIO
                //=========================================================
                C_CON = new XML_CONFIGURACIONES();
                C_CON = UArchivoConfiguracion.CONFIG_APP(C_FILE_CONFIGURACION);

                //=========================================================
                // DATOS DE CORREOS ELECTRONICOS
                //=========================================================
                C_CON_SMTP = new XML_CONFIGURACIONES_EMAIL();
                C_CON_SMTP = UArchivoConfiguracion.CONFIG_APP_EMAIL(C_FILE_SMTP);




                C_FILE_RUTA_EJECUCION = C_CON.RUTA;
                C_FILE_RUTA_TOKEN = C_CON.RUTA_TOKEN;


                //=========================================================
                // CONFIGURACION
                //=========================================================
                if (Directory.Exists(C_FILE_RUTA_EJECUCION) == false)
                {
                    throw new Exception("LA CARPETA DE EJECUCIONES NO EXISTE EN SERVIDOR");

                }

                if (Directory.Exists(C_FILE_RUTA_TOKEN) == false)
                {
                    throw new Exception("LA CARPETA DE TOKEN NO EXISTE EN SERVIDOR");

                }


                //=========================================================
                // INICIA TIMER
                //=========================================================       
                C_myTimer_1 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_1.Elapsed += (P_TIMER_1);
                C_myTimer_1.Start();

                C_myTimer_2 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_2.Elapsed += (P_TIMER_2);
                C_myTimer_2.Start();


                C_myTimer_3 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_3.Elapsed += (P_TIMER_3);
                C_myTimer_3.Start();

                C_myTimer_4 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_4.Elapsed += (P_TIMER_4);
                C_myTimer_4.Start();

                C_myTimer_5 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_5.Elapsed += (P_TIMER_5);
                C_myTimer_5.Start();

                C_myTimer_6 = new Timer(Convert.ToDouble(C_CON.intervalTime_1));
                C_myTimer_6.Elapsed += (P_TIMER_6);
                C_myTimer_6.Start();



                //P_TIMER_1(null, null);
                //P_TIMER_2(null, null);
                //P_TIMER_3(null, null);
                //P_TIMER_4(null, null);
                //P_TIMER_5(null, null);
                //P_TIMER_6(null, null);


            }
            catch (Exception ex)
            {

                Log.WriteMensajeLog("ERROR AL EJECUTAR SERVICIO DE CARGA " + ex.Message);
                Log.WriteMensajeLog(ex.Message);




            }




        }

        /// <summary>
        /// TIMER 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_1(object sender, ElapsedEventArgs e)
        {


            try
            {
                

                


                //=========================================================
                // PROCESOS EN EJECUCION SI ESTE ESTA EN EJECUCION SALE
                //=========================================================
                if (EN_PROCESO_TIMER_1 == true) { return; }
                EN_PROCESO_TIMER_1 = true;



                //=========================================================
                // INSTALACION O ACTUALIZACION DE SOFTWARE
                //=========================================================
                List<oSP_READ_FIFO_INTERFAZ_DIS> LST_FIFO = new List<oSP_READ_FIFO_INTERFAZ_DIS>();
                LST_FIFO = LEER_FIFO_INTERFAZ();


                if (LST_FIFO == null)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (1)");
                    return;
                }

                if (LST_FIFO.Count <= 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (1)");
                    return;
                }


                //=========================================================
                // OBJETO
                //=========================================================
                oSP_READ_FIFO_INTERFAZ_DIS COLA = LST_FIFO.First();

                if (COLA.ID_FIFO_INTERFAZ_DIS == 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (1)");
                    return;
                }

                
                //=========================================================
                // ACTUALIZA FIFO
                //========================================================= 
                ACTUALIZAR_ESTADO_FIFO(COLA.ID_FIFO_INTERFAZ_DIS, 1,"OPERACION ENTRO A EJECUCIÓN");





                //=========================================================
                // SE DEBE CREAR LA RUTA DE EJECUCION
                //=========================================================
                string RUTA = C_FILE_RUTA_EJECUCION + @"\DISCADOR\" + COLA.ID_FIFO_INTERFAZ_DIS;


                //=========================================================
                // EXISTENCIA Y ELIMINACION
                //=========================================================
                if (Directory.Exists(RUTA) == false)
                {
                    Directory.CreateDirectory(RUTA);

                }
                else
                {

                    DirectoryInfo d = new DirectoryInfo(RUTA);

                    foreach (var file in d.GetFiles("*.*"))
                    {
                        File.Delete(file.FullName);
                    }


                }

                //=========================================================
                // NOMBRE DE ARCHIVO BCP BAJADA DE DATOS
                //=========================================================
                string FileTXTOut = RUTA + @"\INT.TXT";

                //=========================================================
                // SE DEBE BAJAR LA INFORMACION
                //=========================================================
                DATOS_BCP_FILE_OUT(COLA, FileTXTOut, "ZEUS");


                while (UOperacionesFile.CheckFileHasCopied(FileTXTOut) == false)
                {
                    Console.WriteLine("ARCHIVO BAJADO " + COLA.ID_INTERFAZ);

                }

                //=========================================================
                // NOMBRE DE ARCHIVO BCP BAJADA DE DATOS
                //=========================================================
                string FileTXTQuery = RUTA + @"\INT_QUERY.TXT";


                //=========================================================
                // SE DEBE CONSTRUIR QUERY PARA EJECUTAR
                //=========================================================
                TRANSFORMAR_QUERY_PRIORIZADOS_ALO(COLA, FileTXTOut, FileTXTQuery);


                while (UOperacionesFile.CheckFileHasCopied(FileTXTQuery) == false)
                {
                    Console.WriteLine("ARCHIVO QUERY SIN ACCESO " + COLA.ID_INTERFAZ);

                }

                //=========================================================
                // SE DEBE SEPARAR EJECUCIONES
                //=========================================================
                UOperacionesFile.SplitFile(1000, FileTXTQuery, "Q_", UOperacionesFile.PathPadre(FileTXTQuery));


                //=========================================================
                // NOMBRE DE ARCHIVO BCP BAJADA DE DATOS
                //=========================================================
                string FileOUTQuery = RUTA + @"\OUT_QUERY_PS.TXT";



                //=========================================================
                // ENVIO DE EJECUCIONES DE PROCEDIMIENTOS
                //========================================================= 
                ELIMINAR_PROCESO(COLA.ID_FIFO_INTERFAZ_DIS);
                INGRESAR_PROCESO_ALO(UOperacionesFile.PathPadre(FileTXTQuery), COLA.ID_FIFO_INTERFAZ_DIS, FileOUTQuery, C_CON.PS);


              



                //===========================================================
                // MENSAJE AL WEBSITE
                //===========================================================
                UMensaje.EnviaMensaje(COLA.LOGIN, "PROCESO GENERAL DE BASE DE DATOS DE TELEFONOS YA FUERON GENERADOS");




            }
            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_1 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_1 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_1 = false;

            }


        }

        /// <summary>
        /// TIMER 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_2(object sender, ElapsedEventArgs e)
        {


            try
            {





                //=========================================================
                // PROCESOS EN EJECUCION SI ESTE ESTA EN EJECUCION SALE
                //=========================================================
                if (EN_PROCESO_TIMER_2 == true) { return; }
                EN_PROCESO_TIMER_2 = true;
                

                //=========================================================
                // INSTALACION O ACTUALIZACION DE SOFTWARE
                //=========================================================
                List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC> LST_FIFO = new List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC>();
                LST_FIFO = LEER_FIFO_DISCADOR();


                if (LST_FIFO == null)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (2)");
                    return;
                }

                if (LST_FIFO.Count <= 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (2)");
                    return;
                }





                //=========================================================
                // OBJETO
                //=========================================================
                oSP_READ_FIFO_INTERFAZ_DIS_EJEC COLA = LST_FIFO.First();


                if (COLA.ID_FIFO_INTERFAZ_DIS <= 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (2)");
                    return;
                }

                //=========================================================
                // SE DEBE CREAR LA RUTA DE EJECUCION
                //=========================================================
                string RUTA = C_FILE_RUTA_EJECUCION + @"\DISCADOR\" + COLA.ID_FIFO_INTERFAZ_DIS;


                //=========================================================
                // RESCATO ARCHIVO DE SALIDA PARA SER PROCESADO
                //=========================================================
                string FileINQuery = RUTA + @"\OUT_QUERY_PS.TXT";
                string FileOutQuery = RUTA + @"\IN_QUERY_PS_DIS.TXT";



                //=========================================================
                // UNIR ARCHIVOS 
                //=========================================================
                DirectoryInfo d = new DirectoryInfo(RUTA);


                StringBuilder Sb = new StringBuilder();
                foreach (var file in d.GetFiles("E_*.TXT*"))
                {

                    using (StreamReader Reader = new StreamReader(file.FullName))
                    {

                        Sb.Append(Reader.ReadToEnd());
                        {
                        }

                    }
                }
                System.IO.File.WriteAllText(FileINQuery, Sb.ToString());

                //=========================================================
                // ACTUALIZAR NUMEROS
                //=========================================================
                int NumeroLinea = TotalLines(FileINQuery);
                ACTUALIZAR_ESTADO_FIFO_NUMERO(COLA.ID_FIFO_INTERFAZ_DIS, NumeroLinea);




                //=========================================================
                // TRANSFORMAR QUERY
                //=========================================================
                TRANSFORMAR_QUERY_PRIORIZADOS_DISCADOR(COLA, FileINQuery, FileOutQuery);


                while (UOperacionesFile.CheckFileHasCopied(FileOutQuery) == false)
                {
                    Console.WriteLine("ARCHIVO QUERY GENERADO " + COLA.ID_INTERFAZ);

                }

                //=========================================================
                // SE DEBE SEPARAR EJECUCIONES
                //=========================================================
                UOperacionesFile.SplitFile(1000, FileOutQuery, "D_", UOperacionesFile.PathPadre(FileOutQuery));



                //=========================================================
                // ACTUALIZARPROCESO
                //=========================================================
                ELIMINAR_PROCESO(COLA.ID_FIFO_INTERFAZ_DIS);
                INGRESAR_PROCESO_DIS(UOperacionesFile.PathPadre(FileOutQuery), COLA.ID_FIFO_INTERFAZ_DIS, FileOutQuery, C_CON.PS);

                

                ACTUALIZAR_ESTADO_FIFO(COLA.ID_FIFO_INTERFAZ_DIS,2 ,"PROCESO FIFO DISCADOR A INICIADO");


                //===========================================================
                // MENSAJE AL WEBSITE
                //===========================================================
                UMensaje.EnviaMensaje(COLA.LOGIN, "PROCESO ENVIADO A DISCADOR");



            }
            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_2 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_2 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_2 = false;

            }


        }

        /// <summary>
        /// TIMER 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_3(object sender, ElapsedEventArgs e)
        {


            try
            {





                //=========================================================
                // PROCESOS EN EJECUCION SI ESTE ESTA EN EJECUCION SALE
                //=========================================================
                if (EN_PROCESO_TIMER_3 == true) { return; }
                EN_PROCESO_TIMER_3 = true;


                //=========================================================
                // INSTALACION O ACTUALIZACION DE SOFTWARE
                //=========================================================
                List<oSP_READ_FIFO_INTERFAZ_DIS_DB> LST_FIFO = new List<oSP_READ_FIFO_INTERFAZ_DIS_DB>();
                LST_FIFO = LEER_FIFO_DB();


                if (LST_FIFO == null)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (3)");
                    return;
                }

                if (LST_FIFO.Count <= 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (3)");
                    return;
                }





                //=========================================================
                // OBJETO
                //=========================================================
                oSP_READ_FIFO_INTERFAZ_DIS_DB COLA = LST_FIFO.First();


                if (COLA.ID_FIFO_INTERFAZ_DIS <= 0)
                {
                    Console.WriteLine("NO EXISTEN FIFO EJECUCION EN SISTEMA (3)");
                    return;
                }


                //=========================================================
                // ELIMINAR INFO DE LOS DIRECTORIOS
                //=========================================================
                string RUTA = C_FILE_RUTA_EJECUCION + @"\DISCADOR\" + COLA.ID_FIFO_INTERFAZ_DIS;



                //=========================================================
                // RESCATO ARCHIVO DE SALIDA PARA SER PROCESADO
                //=========================================================
                string FileINQuery = RUTA + @"\OUT_QUERY_PS.TXT";
                string FileOutQuery = RUTA + @"\OUT_QUERY_BCP.TXT";

                //=========================================================
                // PROCESAR TOKEN
                //=========================================================
                string Token = PROCESAR_BCP_FILE_TOKEN(COLA.ID_FIFO_INTERFAZ_DIS, COLA.ID_USUARIO, COLA.NRO_LOTE, FileINQuery, FileOutQuery, "EROS");




                //===========================================================
                // MENSAJE AL WEBSITE
                //===========================================================
                UMensaje.EnviaMensaje(COLA.LOGIN,"PROCESO FINALIZADO Y LOTE INGRESADO EN SISTEMA");

                

                //=========================================================
                // URL
                //=========================================================
                string URL = C_CON.DESCARGA + "?TOKEN=" + Token;

                

                //=========================================================
                // PROCESAR ENVIO DE CORREO
                //=========================================================
                PROCESAR_ENVIO_CORREO_DISCADOR(COLA.NRO_LOTE.ToString(), C_FILE_XSLT_CORREO_DIS, C_FILE_IMAGEN, COLA.CORREO, URL);





                //=========================================================
                // ACTUALIZAR ULTIMOS ESTADOS
                //=========================================================
                ACTUALIZAR_ESTADO_FIFO(COLA.ID_FIFO_INTERFAZ_DIS,3, "PROCESO DISCADOR FINALIZADO");

                


                //===========================================================
                // BUSCAR ARCHIVOS EN CONFIGURACIONES
                //===========================================================
                DirectoryInfo d = new DirectoryInfo(RUTA);


                foreach (var file in d.GetFiles("*.*"))
                {
                    //=======================================================
                    // PARAMETROS DE ENTRADA 
                    //=======================================================
                    System.IO.File.Delete(file.FullName);


                }

                //===========================================================
                // ELIMINAR DIRECTORIO
                //===========================================================
                Directory.Delete(RUTA, true);


                //===========================================================
                // ELIMINAR FIFO
                //===========================================================
                ELIMINAR_PROCESO(COLA.ID_FIFO_INTERFAZ_DIS);
                ELIMINAR_FIFO(COLA.ID_FIFO_INTERFAZ_DIS);


            }
            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_3 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_3 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_3 = false;

            }


        }

 
        /// <summary>
        /// TIMER 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_4(object sender, ElapsedEventArgs e)
        {


            try
            {





                //=========================================================
                // PROCESOS EN EJECUCION SI ESTE ESTA EN EJECUCION SALE
                //=========================================================
                if (EN_PROCESO_TIMER_4 == true) { return; }
                EN_PROCESO_TIMER_4 = true;


             



                //=========================================================
                // ELIMINAR INFO DE LOS DIRECTORIOS
                //=========================================================
                string RUTA = C_FILE_RUTA_EJECUCION + @"\TOKEN\";

                //===========================================================
                // BUSCAR ARCHIVOS EN CONFIGURACIONES
                //===========================================================
                DirectoryInfo d = new DirectoryInfo(RUTA);
                DateTime FECHA = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                //===========================================================
                // ELIMINAR CARPETA ANTERIORES
                //===========================================================
                foreach (FileInfo item in d.GetFiles("*.*"))
                {
                    DateTime FECHA_FILE = Convert.ToDateTime(item.CreationTime.ToShortDateString());

                    int Dias = Math.Abs(((TimeSpan)(FECHA - FECHA_FILE)).Days);

                    if (Dias >= 2)
                    {
                        try
                        {
                            File.Delete(item.FullName);
                        }
                        catch { }
                    }

                }






            }
            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_4 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_4 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_4 = false;

            }


        }

        /// <summary>
        /// TIMER 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_5(object sender, ElapsedEventArgs e)
        {


            try
            {





                //=========================================================
                // PROCESOS EN EJECUCION SI ESTE ESTA EN EJECUCION SALE
                //=========================================================
                if (EN_PROCESO_TIMER_5 == true) { return; }
                EN_PROCESO_TIMER_5 = true;






                //=========================================================
                // ELIMINAR INFO DE LOS DIRECTORIOS
                //=========================================================
                string RUTA = C_FILE_RUTA_EJECUCION + @"\TOKEN";
                DateTime FECHA = Convert.ToDateTime(DateTime.Now.ToShortDateString());





                //===========================================================
                // ELIMINAR CARPETA ANTERIORES
                //===========================================================
                RUTA = C_FILE_RUTA_EJECUCION + @"\DISCADOR";
                DirectoryInfo DirectorioDiscador = new DirectoryInfo(RUTA);

                foreach (DirectoryInfo item in DirectorioDiscador.EnumerateDirectories())
                {
                    DateTime FECHA_DIR = Convert.ToDateTime(item.CreationTime.ToShortDateString());

                    int Dias = Math.Abs(((TimeSpan)(FECHA - FECHA_DIR)).Days);

                    if (Dias >= 2)
                    {
                        try
                        {
                            ELIMINAR_FIFO(Convert.ToInt32(item.Name));
                            ELIMINAR_PROCESO(Convert.ToInt32(item.Name));
                            Directory.Delete(item.FullName, true);
                        }
                        catch { }
                    }


                }





            }
            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_5 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_5 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_5 = false;

            }


        }



         /// <summary>
        /// TIMER 6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_TIMER_6(object sender, ElapsedEventArgs e)
        {


            try
            {
                //=========================================================
                // DECLARACIÓN DE VARIABLES
                //=========================================================
                List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE> LST_JECUCION = new List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE>();

                List<PROCESS_PID> PID = new List<PROCESS_PID>();


                //=========================================================
                // LLAMADA AL MÉTODO
                //=========================================================
                LST_JECUCION = LEER_FIFO_INTERFAZ_DIS_X_PROCESO_EJE();




                //=========================================================
                // SI LA LISTA ES MAYOR A CERO PROCEDE
                //=========================================================
                if (LST_JECUCION != null)
                {
                    if (LST_JECUCION.Count > 0)
                    {
                        //=========================================================
                        // LLENADO DE PID
                        //=========================================================
                        Process[] processes = Process.GetProcesses();
                        foreach (Process p in processes)
                        {
                            PID.Add(new PROCESS_PID { PID = p.Id });
                        }

                        //RECORRE EL ARREGLO DE PROCESOS
                        foreach (var ejec in LST_JECUCION)
                        {
                            bool EXISTE = true;
                            try
                            {
                                EXISTE = PID.Exists(x => x.PID == ejec.PID);
                            }
                            catch
                            {
                                EXISTE = true;
                            }

                            if (EXISTE == false)
                            {
                                string RUTA = C_FILE_RUTA_EJECUCION + @"\DISCADOR\" + ejec.ID_FIFO_INTERFAZ_DIS + @"\" + ejec.ARCHIVO.ToUpper().Replace(".TXT", ".PS1");
                                //=========================================================
                                // EJECUCION DE PS
                                //=========================================================
                                if (File.Exists(RUTA))
                                {
                                    ProcessStartInfo ProcStartInfoFMT = new ProcessStartInfo();
                                    ProcStartInfoFMT.UseShellExecute = false;
                                    ProcStartInfoFMT.CreateNoWindow = true;
                                    ProcStartInfoFMT.FileName = C_CON.PS;
                                    ProcStartInfoFMT.Arguments = RUTA;


                                    using (Process proc = new Process())
                                    {
                                        Console.WriteLine("EJECUTA FILE " + RUTA);
                                        proc.StartInfo = ProcStartInfoFMT;
                                        proc.Start();

                                    }
                                }
                            }
                        }
                    }
                }
            }//CIERRA TRY

            catch (Exception ex)
            {
                Console.WriteLine("P_TIMER_6 SERVICIO WIN : " + ex.Message);
                Log.WriteMensajeLog("P_TIMER_6 SERVICIO WIN : " + ex.Message);

                Log.WriteMensajeLog(ex.Message);

            }
            finally
            {

                EN_PROCESO_TIMER_6 = false;

            }
        }

        /// <summary>
        /// BCP DATOS
        /// </summary>
        /// <param name="e"></param>
        private static void DATOS_BCP_FILE_OUT(oSP_READ_FIFO_INTERFAZ_DIS COLA, string FILE_OUT, string SERVER)
        {



            try
            {


                //============================================================
                // ARCHIVOS DE CONFIGURACION
                //============================================================
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                string ArchivoServer = FILE_ENSAMBLADO.DirectoryName + @"\XML\" + SERVER + ".XML";

                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(ArchivoServer) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO ARCHIVO DE CONFIGURACION LLAMADO :" + SERVER);
                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION SERVER
                //============================================================
                XML_CONFIG_SERVER CONF_SERVER = new XML_CONFIG_SERVER();
                CONF_SERVER = UArchivoConfiguracion.CONFIG_APP_SERVER(ArchivoServer);



                //============================================================
                // LEER INTERFAZ DETALLE
                //============================================================
                List<oSP_READ_INTERFAZ_DETALLE> Lista = new List<oSP_READ_INTERFAZ_DETALLE>();
                Lista = LEER_INTERFAZ_DETALLE(COLA.ID_INTERFAZ);

                if (Lista == null)
                {
                    throw new Exception("NO EXISTE DETALLE DE INTERFAZ");
                }

                if (Lista.Count <= 0)
                {
                    throw new Exception("NO EXISTE DETALLE DE INTERFAZ");
                }

                string QUERY = "SELECT ";
                //============================================================
                // ITERACION DE DETALLE
                //============================================================
                foreach (oSP_READ_INTERFAZ_DETALLE item in Lista)
                {
                    QUERY = QUERY + item.CAMPO + ",";


                }

                //===========================================================
                // QUERY
                //===========================================================
                QUERY = QUERY.Substring(0, QUERY.Length - 1);
                QUERY = QUERY + " FROM IBR_VENTAS_EXPRESS.dbo."
                              + COLA.TABLA
                              + " WHERE ID_USUARIO = " + COLA.ID_USUARIO
                              + " AND ID_CAMPANA = " + COLA.ID_CAMPANA
                              + " AND ID_INTERFAZ = " + COLA.ID_INTERFAZ;


                //===========================================================
                // REGISTROS
                //===========================================================
                List<oSP_READ_TABLA_INTERFAZ> LST_REGISTROS = new List<oSP_READ_TABLA_INTERFAZ>();
                LST_REGISTROS = LEER_REGISTROS_INTERFAZ(COLA.TABLA, COLA.ID_USUARIO,COLA.ID_CAMPANA, COLA.ID_INTERFAZ);


                if (LST_REGISTROS == null)
                {
                    throw new Exception("NO EXISTEN REGISTROS EN LA INTERFAZ");
                }

                if (LST_REGISTROS.Count <= 0)
                {
                    throw new Exception("NO EXISTEN REGISTROS EN LA INTERFAZ");
                }

                int REGISTROS = LST_REGISTROS.First().REGISTROS;

                if (REGISTROS <= 0)
                {
                    throw new Exception("NO EXISTEN REGISTROS EN LA INTERFAZ");

                }



                
                //===========================================================
                // BCP
                //===========================================================
                string BCP = "bcp "
                                + " "
                                + Comillas
                                + QUERY
                                + Comillas
                                + " queryout  "
                                + Comillas
                                + FILE_OUT
                                + Comillas
                                + " -c -t" + Comillas + "|" + Comillas
                                + " -U "
                                + CONF_SERVER.USUARIO
                                + " -P "
                                + CONF_SERVER.PASSWORD
                                + " -S "
                                + CONF_SERVER.SERVIDOR;




                //=========================================================
                // EJECUCION
                //=========================================================
                ProcessStartInfo ProcStartInfoBCP = new ProcessStartInfo("cmd", "/c " + BCP);
                ProcStartInfoBCP.UseShellExecute = false;
                ProcStartInfoBCP.RedirectStandardOutput = true;
                ProcStartInfoBCP.RedirectStandardError = true;

                using (Process proc = new Process())
                {

                    proc.StartInfo = ProcStartInfoBCP;
                    proc.OutputDataReceived += P_OutputDataReceived;
                    proc.ErrorDataReceived += P_CaptureError;


                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();

                }



                


            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// EJECUCION DE CONSOLA
        /// </summary>
        /// <param name="COLA"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        /// <param name="SERVER"></param>
        private static void EJECUCION_PS_ALO(int ID_FIFO_INTERFAZ, string FILE_IN, string FILE_OUT, string SERVER, string FILE_OUT_NAME, string RUTA_PS)
        {



            try
            {


                //============================================================
                // ARCHIVOS DE CONFIGURACION
                //============================================================
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                string ArchivoServer = FILE_ENSAMBLADO.DirectoryName + @"\XML\" + SERVER + ".XML";
                string FilePowerShell = FILE_ENSAMBLADO.DirectoryName + @"\PS\EJECUCION_ALO.ps1";
                string FilePS = "";


                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(ArchivoServer) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO ARCHIVO DE CONFIGURACION LLLAMADO :" + SERVER);
                }

                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(FilePowerShell) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO PS PARA EJECUTAR ");
                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION SERVER
                //============================================================
                XML_CONFIG_SERVER CONF_SERVER = new XML_CONFIG_SERVER();
                CONF_SERVER = UArchivoConfiguracion.CONFIG_APP_SERVER(ArchivoServer);

                //============================================================
                // CREACION DE DICCIONARIO DE CLAVE VALOR
                //============================================================
                string Comillas = "'";
                Dictionary<string, string> Diccionario = new Dictionary<string, string>();
                Diccionario.Add("#X_P1", Comillas + CONF_SERVER.SERVIDOR.ToString() + Comillas);
                Diccionario.Add("#X_P2", Comillas + "IBR_VENTAS_EXPRESS" + Comillas);
                Diccionario.Add("#X_P3", Comillas + CONF_SERVER.USUARIO.ToString() + Comillas);
                Diccionario.Add("#X_P4", Comillas + CONF_SERVER.PASSWORD.ToString() + Comillas);
                Diccionario.Add("#X_P5", Comillas + FILE_IN + Comillas);
                Diccionario.Add("#X_P6", Comillas + FILE_OUT + Comillas);
                Diccionario.Add("#X_P7", Comillas + ID_FIFO_INTERFAZ.ToString() + Comillas);
                Diccionario.Add("#X_P8", Comillas + FILE_OUT_NAME + Comillas);



                //============================================================
                // SOBRE EL SCRIP DEBEMOS CREAR UNA COPIA Y REEMPLAZAR
                //============================================================
                using (StreamReader Reader = new StreamReader(FilePowerShell))
                {
                    StringBuilder Sb = new StringBuilder();
                    Sb.Append(Reader.ReadToEnd());
                    {


                        foreach (KeyValuePair<string, string> Llave in Diccionario)
                        {
                            Sb.Replace(Llave.Key, Llave.Value);
                        }


                    }

                    FilePS = UOperacionesFile.PathPadre(FILE_OUT) + @"\" + FILE_OUT_NAME.ToUpper().Replace(".TXT", "").Trim() + ".ps1";

                    System.IO.File.WriteAllText(FilePS, Sb.ToString());
                }

                //=========================================================
                // ESPERAR COPIA COMPLETA
                //=========================================================
                while (UOperacionesFile.CheckFileHasCopied(FilePS) == false)
                {
                    Console.WriteLine("FILE PS LIBERADO");
                }
                

                //=========================================================
                // EJECUCION
                //=========================================================
                ProcessStartInfo ProcStartInfoFMT = new ProcessStartInfo();
                ProcStartInfoFMT.UseShellExecute = false;
                ProcStartInfoFMT.CreateNoWindow = true;
                ProcStartInfoFMT.FileName = RUTA_PS;
                ProcStartInfoFMT.Arguments = FilePS;


                using (Process proc = new Process())
                {
                    Console.WriteLine("EJECUTA FILE " + FilePS);
                    proc.StartInfo = ProcStartInfoFMT;
                    proc.Start();
                
                }






            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// EJECUCION DE CONSOLA
        /// </summary>
        /// <param name="COLA"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        /// <param name="SERVER"></param>
        private static void EJECUCION_PS_CON(int ID_FIFO_INTERFAZ_CON, string FILE_IN, string FILE_OUT, string SERVER, string FILE_OUT_NAME, string RUTA_PS)
        {



            try
            {


                //============================================================
                // ARCHIVOS DE CONFIGURACION
                //============================================================
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                string ArchivoServer = FILE_ENSAMBLADO.DirectoryName + @"\XML\" + SERVER + ".XML";
                string FilePowerShell = FILE_ENSAMBLADO.DirectoryName + @"\PS\EJECUCION_CON.ps1";
                string FilePS = "";


                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(ArchivoServer) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO ARCHIVO DE CONFIGURACION LLLAMADO :" + SERVER);
                }

                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(FilePowerShell) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO PS PARA EJECUTAR ");
                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION SERVER
                //============================================================
                XML_CONFIG_SERVER CONF_SERVER = new XML_CONFIG_SERVER();
                CONF_SERVER = UArchivoConfiguracion.CONFIG_APP_SERVER(ArchivoServer);

                //============================================================
                // CREACION DE DICCIONARIO DE CLAVE VALOR
                //============================================================
                string Comillas = "'";
                Dictionary<string, string> Diccionario = new Dictionary<string, string>();
                Diccionario.Add("#X_P1", Comillas + CONF_SERVER.SERVIDOR.ToString() + Comillas);
                Diccionario.Add("#X_P2", Comillas + "IBR_REPORTE" + Comillas);
                Diccionario.Add("#X_P3", Comillas + CONF_SERVER.USUARIO.ToString() + Comillas);
                Diccionario.Add("#X_P4", Comillas + CONF_SERVER.PASSWORD.ToString() + Comillas);
                Diccionario.Add("#X_P5", Comillas + FILE_IN + Comillas);
                Diccionario.Add("#X_P6", Comillas + FILE_OUT + Comillas);
                Diccionario.Add("#X_P7", Comillas + ID_FIFO_INTERFAZ_CON.ToString() + Comillas);
                Diccionario.Add("#X_P8", Comillas + FILE_OUT_NAME + Comillas);



                //============================================================
                // SOBRE EL SCRIP DEBEMOS CREAR UNA COPIA Y REEMPLAZAR
                //============================================================
                using (StreamReader Reader = new StreamReader(FilePowerShell))
                {
                    StringBuilder Sb = new StringBuilder();
                    Sb.Append(Reader.ReadToEnd());
                    {


                        foreach (KeyValuePair<string, string> Llave in Diccionario)
                        {
                            Sb.Replace(Llave.Key, Llave.Value);
                        }


                    }

                    FilePS = UOperacionesFile.PathPadre(FILE_OUT) + @"\" + FILE_OUT_NAME.ToUpper().Replace(".TXT", "").Trim() + ".ps1";

                    System.IO.File.WriteAllText(FilePS, Sb.ToString());
                }


                //=========================================================
                // ESPERAR COPIA COMPLETA
                //=========================================================
                while (UOperacionesFile.CheckFileHasCopied(FilePS) == false)
                {
                    Console.WriteLine("FILE PS LIBERADO");
                }


                //=========================================================
                // EJECUCION
                //=========================================================
                ProcessStartInfo ProcStartInfoFMT = new ProcessStartInfo();
                ProcStartInfoFMT.UseShellExecute = false;
                ProcStartInfoFMT.CreateNoWindow = true;
                ProcStartInfoFMT.FileName = RUTA_PS;
                ProcStartInfoFMT.Arguments = FilePS;


                using (Process proc = new Process())
                {
                    Console.WriteLine("EJECUTA FILE " + FilePS);
                    proc.StartInfo = ProcStartInfoFMT;
                    proc.Start();

                }






            }
            catch
            {

                throw;

            }

        }
        /// <summary>
        /// EJECUCION DE CONSOLA
        /// </summary>
        /// <param name="COLA"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        /// <param name="SERVER"></param>
        private static void EJECUCION_PS_NONQUERY(int ID_FIFO_INTERFAZ, string FILE_IN, string SERVER, string RUTA_PS, string FILE_OUT, string FILE_OUT_NAME)
        {



            try
            {


                //============================================================
                // ARCHIVOS DE CONFIGURACION
                //============================================================
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                string ArchivoServer = FILE_ENSAMBLADO.DirectoryName + @"\XML\" + SERVER + ".XML";
                string FilePowerShell = FILE_ENSAMBLADO.DirectoryName + @"\PS\EJECUCION_DIS.ps1";
                string FilePS = "";


                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(ArchivoServer) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO ARCHIVO DE CONFIGURACION LLLAMADO :" + SERVER);
                }

                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(FilePowerShell) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO PS PARA EJECUTAR ");
                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION SERVER
                //============================================================
                XML_CONFIG_SERVER CONF_SERVER = new XML_CONFIG_SERVER();
                CONF_SERVER = UArchivoConfiguracion.CONFIG_APP_SERVER(ArchivoServer);

                //============================================================
                // CREACION DE DICCIONARIO DE CLAVE VALOR
                //============================================================
                string Comillas = "'";
                Dictionary<string, string> Diccionario = new Dictionary<string, string>();
                Diccionario.Add("#X_P1", Comillas + CONF_SERVER.SERVIDOR.ToString() + Comillas);
                Diccionario.Add("#X_P2", Comillas + "MitACD" + Comillas);
                Diccionario.Add("#X_P3", Comillas + CONF_SERVER.USUARIO.ToString() + Comillas);
                Diccionario.Add("#X_P4", Comillas + CONF_SERVER.PASSWORD.ToString() + Comillas);
                Diccionario.Add("#X_P5", Comillas + FILE_IN + Comillas);
                Diccionario.Add("#X_P6", Comillas + ID_FIFO_INTERFAZ.ToString() + Comillas);
                Diccionario.Add("#X_P7", Comillas + FILE_OUT_NAME + Comillas);
                


                //============================================================
                // SOBRE EL SCRIP DEBEMOS CREAR UNA COPIA Y REEMPLAZAR
                //============================================================
                using (StreamReader Reader = new StreamReader(FilePowerShell))
                {
                    StringBuilder Sb = new StringBuilder();
                    Sb.Append(Reader.ReadToEnd());
                    {


                        foreach (KeyValuePair<string, string> Llave in Diccionario)
                        {
                            Sb.Replace(Llave.Key, Llave.Value);
                        }


                    }

                    FilePS = UOperacionesFile.PathPadre(FILE_OUT) + @"\" + FILE_OUT_NAME.ToUpper().Replace(".TXT", "").Trim() + ".ps1";

                    System.IO.File.WriteAllText(FilePS, Sb.ToString());
                }


                //=========================================================
                // ESPERAR COPIA COMPLETA
                //=========================================================
                while (UOperacionesFile.CheckFileHasCopied(FilePS) == false)
                {
                    Console.WriteLine("FILE PS LIBERADO");
                }

                //=========================================================
                // EJECUCION
                //=========================================================
                ProcessStartInfo ProcStartInfoFMT = new ProcessStartInfo();
                ProcStartInfoFMT.UseShellExecute = false;
                ProcStartInfoFMT.CreateNoWindow = true;
                ProcStartInfoFMT.FileName = RUTA_PS;
                ProcStartInfoFMT.Arguments = FilePS;


                using (Process proc = new Process())
                {
                    Console.WriteLine("EJECUTA FILE " + FilePS);
                    proc.StartInfo = ProcStartInfoFMT;
                    proc.Start();

                }






            }
            catch
            {

                throw;

            }

        }
        /// <summary>
        /// QUERY DE PRIORIZADOS
        /// </summary>
        /// <param name="COLA"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        private static void TRANSFORMAR_QUERY_PRIORIZADOS_ALO(oSP_READ_FIFO_INTERFAZ_DIS COLA, string FILE_IN, string FILE_OUT)
        {



            try
            {


                //============================================================
                // LECTURA DE ARCHIVO
                //============================================================
                string Linea ="";
                string Query = "";


                //============================================================
                // ESCRIBE ARCHIVO
                //============================================================
                using (StreamWriter writer = new StreamWriter(FILE_OUT))
                {


                    //========================================================
                    // LEER ARCHIVO
                    //========================================================
                    System.IO.StreamReader file = new System.IO.StreamReader(FILE_IN);
                    while ((Linea = file.ReadLine()) != null)
                    {


                        string[] Contenido = Linea.Split('|');

                        if (Contenido.Length > 0)
                        {
                            Query = "";

                            if (COLA.ID_INTERFAZ == 1)
                            {

                                Query = String.Format("EXEC SP_READ_PRIORIZADOS_1 '{0}',{1},{2},{3}{4}", Contenido[0], COLA.ID_CAMPANA, COLA.NRO_LOTE, COLA.ID_FIFO_INTERFAZ_DIS, Environment.NewLine);
                                

                            }
                            if (COLA.ID_INTERFAZ == 2)
                            {

                                Query = String.Format("EXEC SP_READ_PRIORIZADOS_2 '{0}','{1}',{2},{3},{4}{5}", Contenido[0], Contenido[1], COLA.ID_CAMPANA, COLA.NRO_LOTE,COLA.ID_FIFO_INTERFAZ_DIS, Environment.NewLine);


                            }
                            if (Query.Length > 0)
                            {
                                writer.Write(Query);

                            }


                        }

                    }
                    //========================================================
                    // FIN LEER ARCHIVO
                    //========================================================
                    file.Close();

                }





            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// QUERY DE PRIORIZADOS
        /// </summary>
        /// <param name="COLA"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        private static void TRANSFORMAR_QUERY_PRIORIZADOS_DISCADOR(oSP_READ_FIFO_INTERFAZ_DIS_EJEC COLA, string FILE_IN, string FILE_OUT)
        {



            try
            {


                //============================================================
                // LECTURA DE ARCHIVO
                //============================================================
                string Linea = "";
                string Query = "";


                //============================================================
                // ESCRIBE ARCHIVO
                //============================================================
                using (StreamWriter writer = new StreamWriter(FILE_OUT))
                {


                    //========================================================
                    // LEER ARCHIVO
                    //========================================================
                    System.IO.StreamReader file = new System.IO.StreamReader(FILE_IN);
                    while ((Linea = file.ReadLine()) != null)
                    {


                        string[] Contenido = Linea.Split('|');

                        if (Contenido.Length == 4 )
                        {
                            Query = "";

                            if (Contenido.Length >= 4)
                            {
                                Query = String.Format("EXEC EXT_SP_CREATE_TAREAS {0},'{1}','{2}','{3}'{4}", Contenido[0], Contenido[1], Contenido[2], Contenido[3], Environment.NewLine);
                            }


                            if (Query.Length > 0)
                            {
                                writer.Write(Query);

                            }


                        }

                    }
                    //========================================================
                    // FIN LEER ARCHIVO
                    //========================================================
                    file.Close();

                }





            }
            catch
            {

                throw;

            }

        }
        /// <summary>
        /// ENVIAR LOG A SERVICIO
        /// </summary>
        /// <param name="e"></param>
        private static List<oSP_READ_FIFO_INTERFAZ_DIS> LEER_FIFO_INTERFAZ()
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_FIFO_INTERFAZ_DIS> Retorno = new List<oSP_READ_FIFO_INTERFAZ_DIS>();

                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_FIFO_INTERFAZ_DIS();




                return Retorno;


            }
            catch
            {

                throw;

            }

        }



        /// <summary>
        /// LEER PROCESOS EN EJECUCION 
        /// </summary>
        /// <param name="e"></param>
        private static List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE> LEER_FIFO_INTERFAZ_DIS_X_PROCESO_EJE()
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE> Retorno = new List<oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE>();


               //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE();




                return Retorno;


            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// ENVIAR LOG A SERVICIO
        /// </summary>
        /// <param name="e"></param>
        private static List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC> LEER_FIFO_DISCADOR()
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC> Retorno = new List<oSP_READ_FIFO_INTERFAZ_DIS_EJEC>();

                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_FIFO_INTERFAZ_DIS_EJEC();




                return Retorno;


            }
            catch
            {

                throw;

            }

        }



        /// <summary>
        /// LEER FIFO COLA
        /// </summary>
        /// <returns></returns>
        private static List<oSP_READ_FIFO_INTERFAZ_DIS_DB> LEER_FIFO_DB()
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_FIFO_INTERFAZ_DIS_DB> Retorno = new List<oSP_READ_FIFO_INTERFAZ_DIS_DB>();

                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_FIFO_INTERFAZ_DIS_DB();




                return Retorno;


            }
            catch
            {

                throw;

            }

        }




        /// <summary>
        /// LEER INTERFAZ DETALLE
        /// </summary>
        /// <param name="ID_INTERFAZ"></param>
        /// <returns></returns>
        private static List<oSP_READ_INTERFAZ_DETALLE> LEER_INTERFAZ_DETALLE(int ID_INTERFAZ)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_INTERFAZ_DETALLE> Retorno = new List<oSP_READ_INTERFAZ_DETALLE>();


                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_READ_INTERFAZ_DETALLE ParametrosInput = new iSP_READ_INTERFAZ_DETALLE();
                ParametrosInput.ID_INTERFAZ = ID_INTERFAZ;



                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_INTERFAZ_DETALLE(ParametrosInput);




                return Retorno;


            }
            catch
            {

                throw;

            }

        }



        /// <summary>
        /// LEER REGISTROS DE INTERFAZ
        /// </summary>
        /// <param name="TABLA"></param>
        /// <param name="ID_USUARIO"></param>
        /// <param name="ID_CAMPANA"></param>
        /// <param name="ID_INTERFAZ"></param>
        /// <returns></returns>
        private static List<oSP_READ_TABLA_INTERFAZ> LEER_REGISTROS_INTERFAZ(string TABLA, int ID_USUARIO,int ID_CAMPANA, int ID_INTERFAZ)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);
                List<oSP_READ_TABLA_INTERFAZ> Retorno = new List<oSP_READ_TABLA_INTERFAZ>();


  
                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_READ_TABLA_INTERFAZ ParametrosInput = new iSP_READ_TABLA_INTERFAZ();
                ParametrosInput.TABLA = TABLA;
                ParametrosInput.ID_USUARIO = ID_USUARIO;
                ParametrosInput.ID_INTERFAZ = ID_INTERFAZ;
                ParametrosInput.ID_CAMPANA = ID_CAMPANA;


                //===========================================================
                // ENVIAR OBJETO A NEGOCIO 
                //===========================================================
                Retorno = Servicio.SP_READ_TABLA_INTERFAZ(ParametrosInput);

                

                return Retorno;


            }
            catch
            {

                throw;

            }

        }

 


        /// <summary>
        /// ELIMINAR PROCESOS
        /// </summary>
        /// <param name="ID_FIFO_INTERFAZ_DIS"></param>
        private static void ELIMINAR_PROCESO(int ID_FIFO_INTERFAZ_DIS)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);


                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO ParametrosInput = new iSP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO();
                ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ_DIS;

                //===========================================================
                // ENVIAR OBJETO A SERVICIO 
                //===========================================================
                Servicio.SP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO(ParametrosInput);

               




            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// ELIMINAR FIFO
        /// </summary>
        /// <param name="ID_FIFO_INTERFAZ"></param>
        private static void ELIMINAR_FIFO(int ID_FIFO_INTERFAZ)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);



                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_DELETE_FIFO_INTERFAZ_DIS ParametrosInput = new iSP_DELETE_FIFO_INTERFAZ_DIS();
                ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ;

                //===========================================================
                // ENVIAR OBJETO A SERVICIO 
                //===========================================================
                Servicio.SP_DELETE_FIFO_INTERFAZ_DIS(ParametrosInput);






            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// ACTUALIZAR ESTADO FIFO
        /// </summary>
        /// <param name="ID_FIFO_INTERFAZ"></param>
        /// <param name="OPCION"></param>
        /// <param name="MENSAJE"></param>
        private static void ACTUALIZAR_ESTADO_FIFO(int ID_FIFO_INTERFAZ_DIS, int OPCION, string MENSAJE)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);


                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO ParametrosInput = new iSP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO();
                ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ_DIS;
                ParametrosInput.OPCION = OPCION;
                ParametrosInput.MENSAJE = MENSAJE;

                //===========================================================
                // ENVIAR OBJETO A SERVICIO 
                //===========================================================
                Servicio.SP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO(ParametrosInput);






            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// ACTUALIZAR FIFO NUMERO
        /// </summary>
        /// <param name="ID_FIFO_INTERFAZ"></param>
        /// <param name="NUMERO"></param>
        private static void ACTUALIZAR_ESTADO_FIFO_NUMERO(int ID_FIFO_INTERFAZ_DIS, int NUMERO)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);


                //===========================================================
                // PARAMETROS DE ENTRADA 
                //===========================================================
                iSP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO ParametrosInput = new iSP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO();
                ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ_DIS;
                ParametrosInput.NUMERO = NUMERO;

                //===========================================================
                // ENVIAR OBJETO A SERVICIO 
                //===========================================================
                Servicio.SP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO(ParametrosInput);






            }
            catch
            {

                throw;

            }

        }



        /// <summary>
        /// INGRESAR PROCESO
        /// </summary>
        /// <param name="Ruta"></param>
        /// <param name="ID_FIFO_INTERFAZ"></param>
        private static void INGRESAR_PROCESO_ALO(string Ruta, int ID_FIFO_INTERFAZ,string FileOUTQuery,string RUTA_PS)

        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);




                //===========================================================
                // BUSCAR ARCHIVOS EN CONFIGURACIONES
                //===========================================================
                DirectoryInfo d = new DirectoryInfo(Ruta);


                foreach (var file in d.GetFiles("Q_*.TXT*"))
                {
                    //=======================================================
                    // PARAMETROS DE ENTRADA 
                    //=======================================================
                    iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO ParametrosInput = new iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO();
                    ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ;
                    ParametrosInput.ARCHIVO = file.Name;
                    ParametrosInput.EJECUTADO = false;


                    //=======================================================
                    // CONFIGURAR ARCHIVOS DE SALIDA 
                    //=======================================================
                    string FileOut = UOperacionesFile.PathPadre(file.FullName) + @"\" + file.Name.Replace("Q_", "E_");

                    //=======================================================
                    // ENVIAR OBJETO A NEGOCIO 
                    //=======================================================
                    Servicio.SP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO(ParametrosInput);
                    EJECUCION_PS_ALO(ID_FIFO_INTERFAZ, file.FullName, FileOut, "ZEUS", file.Name, RUTA_PS);


                }

              


            }
            catch
            {

                throw;

            }

        }


        /// <summary>
        /// INGRESAR PROCESO DISCADOR
        /// </summary>
        /// <param name="Ruta"></param>
        /// <param name="ID_FIFO_INTERFAZ"></param>
        /// <param name="FileOUTQuery"></param>
        /// <param name="RUTA_PS"></param>
        private static void INGRESAR_PROCESO_DIS(string Ruta, int ID_FIFO_INTERFAZ, string FileOUTQuery, string RUTA_PS)
        {



            try
            {


                //===========================================================
                // DECLARACION DE VARIABLES
                //===========================================================
                SMetodos Servicio = new SMetodos(C_CON.SERVICIO, C_FILE_XSLT_LOG);




                //===========================================================
                // BUSCAR ARCHIVOS EN CONFIGURACIONES
                //===========================================================
                DirectoryInfo d = new DirectoryInfo(Ruta);


                foreach (var file in d.GetFiles("D_*.TXT*"))
                {
                    //=======================================================
                    // PARAMETROS DE ENTRADA 
                    //=======================================================
                    iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO ParametrosInput = new iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO();
                    ParametrosInput.ID_FIFO_INTERFAZ_DIS = ID_FIFO_INTERFAZ;
                    ParametrosInput.ARCHIVO = file.Name;
                    ParametrosInput.EJECUTADO = false;




                    //=======================================================
                    // ENVIAR OBJETO A NEGOCIO 
                    //=======================================================
                    Servicio.SP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO(ParametrosInput);
                    EJECUCION_PS_NONQUERY(ID_FIFO_INTERFAZ, file.FullName, "EROS", RUTA_PS, file.FullName, file.Name);

                }




            }
            catch
            {

                throw;

            }

        }




        /// <summary>
        /// PROCESAR FILE TOKEN
        /// </summary>
        /// <param name="ID_FIFO"></param>
        /// <param name="ID_USUARIO"></param>
        /// <param name="LOTE"></param>
        /// <param name="FILE_IN"></param>
        /// <param name="FILE_OUT"></param>
        /// <param name="SERVER"></param>
        /// <returns></returns>
        private static string PROCESAR_BCP_FILE_TOKEN(int ID_FIFO, int ID_USUARIO,int LOTE, string FILE_IN, string FILE_OUT, string SERVER)
        {



            try
            {

                //=========================================================
                // DECLARACION DE VARIABLES
                //=========================================================
                DateTime FECHA = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                string Tabla = "IbrACD.dbo.CAMPANA_DISCAR_VENTA";
                string Query = "";

                //=========================================================
                // DIRECTORIO FMT
                //=========================================================
                string[] FILE_FMT = Directory.GetFiles(C_FILE_FMT, "*.*");



                foreach (string FileDir in FILE_FMT)
                {

                    FileInfo FILE_DIR = new FileInfo(FileDir);


                    if (FILE_DIR.CreationTime < FECHA)
                    {
                        File.Delete(FILE_DIR.FullName);
                    }

                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION
                //============================================================
                FileInfo FILE_ENSAMBLADO = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                string ArchivoServer = FILE_ENSAMBLADO.DirectoryName + @"\XML\" + SERVER + ".XML";

                //============================================================
                // VALIDACION
                //============================================================
                if (System.IO.File.Exists(ArchivoServer) == false)
                {
                    throw new Exception("NO EXISTE ARCHIVO ARCHIVO DE CONFIGURACION LLLAMADO :" + SERVER);
                }

                //============================================================
                // ARCHIVOS DE CONFIGURACION SERVER
                //============================================================
                XML_CONFIG_SERVER CONF_SERVER = new XML_CONFIG_SERVER();
                CONF_SERVER = UArchivoConfiguracion.CONFIG_APP_SERVER(ArchivoServer);


                //============================================================
                // CADENA DE CONECCION
                //============================================================
                string CadenaConeccion = new UCadenaConexion().CadenaConexion(CONF_SERVER.USUARIO, CONF_SERVER.PASSWORD, "IBRACD", CONF_SERVER.SERVIDOR);
                


                //============================================================
                // RUTA FMT
                //============================================================
                string RUTA_FMT = C_FILE_FMT + @"\" + ID_FIFO.ToString() + ".FMT";


                //============================================================
                // LEER TABLA Y ORIGINAR ARCHIVO FMT
                //============================================================
                string FMT = "bcp "
                                + " "
                                + Tabla
                                + " format nul -c -t " + Comillas + "|" + Comillas
                                + " -f "
                                + Comillas
                                + RUTA_FMT
                                + Comillas
                                + " -U "
                                + CONF_SERVER.USUARIO
                                + " -P "
                                + CONF_SERVER.PASSWORD
                                + " -S "
                                + CONF_SERVER.SERVIDOR;


                //============================================================
                // EJECUCION
                //============================================================
                ProcessStartInfo ProcStartInfoFMT = new ProcessStartInfo("cmd", "/c " + FMT);
                ProcStartInfoFMT.UseShellExecute = false;
                ProcStartInfoFMT.RedirectStandardOutput = true;
                ProcStartInfoFMT.RedirectStandardError = true;

                using (Process proc = new Process())
                {

                    proc.StartInfo = ProcStartInfoFMT;
                    proc.OutputDataReceived += P_OutputDataReceived;
                    proc.ErrorDataReceived += P_CaptureError;


                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();

                }

                //============================================================
                // VERIFICAR QUE EXISTA FMT
                //============================================================
                while (UOperacionesFile.CheckFileHasCopied(RUTA_FMT) == false) { }
                if (File.Exists(RUTA_FMT) == false)
                {
                    Console.WriteLine("FILE FMT DE SOLICITUD : " + ID_FIFO + " NO EXISTE");
                    throw new Exception("ARCHIVO FMT : " + RUTA_FMT + " NO FUE GENERADO");
                }

                
                //============================================================
                // ESCRIBE ARCHIVO
                //============================================================
                using (StreamWriter writer = new StreamWriter(FILE_OUT))
                {


                    //========================================================
                    // LEER ARCHIVO
                    //========================================================
                    string Linea ="";
                    
                    System.IO.StreamReader file = new System.IO.StreamReader(FILE_IN);
                    while ((Linea = file.ReadLine()) != null)
                    {


                        string[] Contenido = Linea.Split('|');

                        if (Contenido.Length == 4)
                        {

                            string Escribir = ID_FIFO + "|" + ID_USUARIO;
                            foreach (string Valor in Contenido)
                            {
                                Escribir = Escribir + "|" + Valor.ToString();
                            }

                            if (Escribir.Length > 0)
                            {
                                writer.Write(Escribir + Environment.NewLine);

                            }


                        }

                    }
                    //========================================================
                    // FIN LEER ARCHIVO
                    //========================================================
                    file.Close();

                }

                //============================================================
                // ELIMINAR INFORMACION
                //============================================================
                Query = string.Format("EXEC SP_DELETE_CAMPANA_DISCAR_VENTA {0},{1},{2}", ID_FIFO, ID_USUARIO, LOTE);
                PROCESAR_OLDB_QUERY(CadenaConeccion, Query);


                //============================================================
                // EJECUTAR BCP
                //============================================================
                string BCP = "bcp "
                                + " "
                                + Tabla
                                + " in "
                                + Comillas
                                + FILE_OUT
                                + Comillas
                                + " -f "
                                + Comillas
                                + RUTA_FMT
                                + Comillas
                                + " -U "
                                + CONF_SERVER.USUARIO
                                + " -P "
                                + CONF_SERVER.PASSWORD
                                + " -S "
                                + CONF_SERVER.SERVIDOR;



                //=========================================================
                // EJECUCION
                //=========================================================
                ProcessStartInfo ProcStartInfoBCP = new ProcessStartInfo("cmd", "/c " + BCP);
                ProcStartInfoBCP.UseShellExecute = false;
                ProcStartInfoBCP.RedirectStandardOutput = true;
                ProcStartInfoBCP.RedirectStandardError = true;

                using (Process proc = new Process())
                {

                    proc.StartInfo = ProcStartInfoBCP;
                    proc.OutputDataReceived += P_OutputDataReceived;
                    proc.ErrorDataReceived += P_CaptureError;


                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();

                }


                //============================================================
                // EJECUCION DE QUERY PROCESO DE EXCEL
                //============================================================
                string Token = Guid.NewGuid().ToString();
                string RUTA = C_FILE_RUTA_EJECUCION + @"\TOKEN\" + Token + ".XML";
                Query = string.Format("EXEC SP_READ_CAMPANA_DISCAR_VENTA {0},{1},{2}", ID_FIFO, ID_USUARIO, LOTE);


                PROCESAR_EXCEL_XML_OLDB(CadenaConeccion, Query, RUTA);


                //=========================================================
                // MOVER ARCHIVO
                //=========================================================
                if (File.Exists(RUTA) == true)
                {

                    FileInfo FileInfo = new FileInfo(RUTA);
                    string RUTA_DESTINO = C_FILE_RUTA_TOKEN + @"\" + Token;
                    string FileDestino = RUTA_DESTINO + @"\" + FileInfo.Name;

                    if (Directory.Exists(RUTA_DESTINO) == false)
                    {
                        Directory.CreateDirectory(RUTA_DESTINO);
                    }

                    File.Copy(RUTA, FileDestino, true);

                }




                //============================================================
                // ELIMINAR INFORMACION
                //============================================================
                Query = string.Format("EXEC SP_DELETE_CAMPANA_DISCAR_VENTA {0},{1},{2}", ID_FIFO, ID_USUARIO, LOTE);
                PROCESAR_OLDB_QUERY(CadenaConeccion, Query);



                //============================================================
                // DEVOLVER TOKEN
                //============================================================
                return Token;



            }
            catch
            {

                throw;

            }

        }

        /// <summary>
        /// TRANFORMAR SALIDA XML EN EXCEL
        /// </summary>
        /// <param name="Coneccion"></param>
        /// <param name="query"></param>
        /// <param name="NameXml"></param>
        private static void PROCESAR_EXCEL_XML_OLDB(string CadenaConeccion, string query, string FileXml)
        {

            //https://social.msdn.microsoft.com/Forums/en-US/1e63f0d6-c679-4481-a059-7454bef5d245/problem-using-xmltextwriter-export-to-excel?forum=xmlandnetfx

            //===============================================================
            // DECLARACION DE VARIABLES 
            //===============================================================
            SqlConnection Conexion = null;

            try
            {


                //============================================================
                // ABRIR LA CONECCION
                //============================================================
                Conexion = new SqlConnection(CadenaConeccion);
                Conexion.Open();

                //============================================================
                // LISTA DE TIPOS
                //============================================================
                List<TIPO_DATO> LST_TIPO = new List<TIPO_DATO>();


                //============================================================
                // ESTABLECER EL COMAND 
                //============================================================
                using (SqlCommand Comando = new SqlCommand())
                {

                    Comando.CommandType = CommandType.Text;
                    Comando.CommandText = query;
                    Comando.Connection = Conexion;

                    //========================================================
                    // READER
                    //========================================================
                    using (SqlDataReader rdr = Comando.ExecuteReader())
                    {


                        string office = "urn:schemas-microsoft-com:office:office";
                        string excel = "urn:schemas-microsoft-com:office:excel";
                        string spreadsheet = "urn:schemas-microsoft-com:office:spreadsheet";

                        XmlTextWriter xmlTextWriter = new XmlTextWriter(FileXml, null);
                        xmlTextWriter.Formatting = Formatting.Indented;
                        xmlTextWriter.WriteStartDocument();
                        xmlTextWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                        xmlTextWriter.WriteStartElement("Workbook");
                        xmlTextWriter.WriteAttributeString("xmlns", string.Empty, "http://www.w3.org/2000/xmlns/", spreadsheet);
                        xmlTextWriter.WriteAttributeString("xmlns", "o", "http://www.w3.org/2000/xmlns/", office);
                        xmlTextWriter.WriteAttributeString("xmlns", "x", "http://www.w3.org/2000/xmlns/", excel);
                        xmlTextWriter.WriteAttributeString("xmlns", "ss", "http://www.w3.org/2000/xmlns/", spreadsheet);
                        xmlTextWriter.WriteAttributeString("xmlns", "html", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/TR/REC-html40");




                        //====================================================
                        // PROPIEDADES
                        //====================================================
                        xmlTextWriter.WriteStartElement(string.Empty, "DocumentProperties", office);
                        xmlTextWriter.WriteElementString("Author", "IBR");
                        xmlTextWriter.WriteElementString("LastAuthor", "IBR");
                        xmlTextWriter.WriteElementString("Version", "14.00");
                        xmlTextWriter.WriteEndElement();


                        //====================================================
                        // OFFICE
                        //====================================================
                        xmlTextWriter.WriteStartElement("OfficeDocumentSettings");
                        xmlTextWriter.WriteAttributeString("xmlns", string.Empty, "http://www.w3.org/2000/xmlns/", office);
                        xmlTextWriter.WriteStartElement("AllowPNG");
                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteEndElement();


                        //====================================================
                        // LIBRO
                        //====================================================
                        xmlTextWriter.WriteStartElement(string.Empty, "ExcelWorkbook", excel);
                        xmlTextWriter.WriteElementString("WindowHeight", "4680");
                        xmlTextWriter.WriteElementString("WindowWidth", "4515");
                        xmlTextWriter.WriteElementString("WindowTopX", "480");
                        xmlTextWriter.WriteElementString("WindowTopY", "120");
                        xmlTextWriter.WriteElementString("ProtectStructure", "False");
                        xmlTextWriter.WriteElementString("ProtectWindows", "False");
                        xmlTextWriter.WriteEndElement();


                        //====================================================
                        // ESTILOS
                        //====================================================
                        xmlTextWriter.WriteStartElement("Styles");
                        xmlTextWriter.WriteStartElement("Style");
                        xmlTextWriter.WriteAttributeString("ss:ID", "Default");
                        xmlTextWriter.WriteAttributeString("ss:Name", "Normal");


                        xmlTextWriter.WriteStartElement("Alignment");
                        xmlTextWriter.WriteAttributeString("ss:Vertical", "Bottom");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Borders");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Font");
                        xmlTextWriter.WriteAttributeString("ss:FontName", "Calibri");
                        xmlTextWriter.WriteAttributeString("x:Family", "Swiss");
                        xmlTextWriter.WriteAttributeString("ss:Size", "11");
                        xmlTextWriter.WriteAttributeString("ss:Color", "#000000");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Interior");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("NumberFormat");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Protection");
                        xmlTextWriter.WriteEndElement();


                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteEndElement();



                        xmlTextWriter.WriteStartElement("Worksheet");
                        xmlTextWriter.WriteAttributeString("ss:Name", "Hoja1");

                        xmlTextWriter.WriteStartElement("Table");
                        xmlTextWriter.WriteAttributeString("ss:ExpandedColumnCount", "255");
                        xmlTextWriter.WriteAttributeString("x:FullColumns", "1");
                        xmlTextWriter.WriteAttributeString("x:FullRows", "1");
                        xmlTextWriter.WriteAttributeString("ss:DefaultColumnWidth", "60");
                        xmlTextWriter.WriteAttributeString("ss:DefaultRowHeight", "15");





                        int Columnas = rdr.FieldCount;
                        int FILA = 1;
                        //====================================================
                        // ENCABEZADOS
                        //====================================================
                        while (rdr.Read())
                        {

                            //================================================
                            // ENCABEZADOS
                            //================================================
                            if (FILA == 1)
                            {
                                xmlTextWriter.WriteStartElement("Row");
                                for (int i = 0; i < Columnas; i++)
                                {
                                    LST_TIPO.Add(new TIPO_DATO { COLUMNA = i, TIPO = rdr.GetFieldType(i) });

                                    xmlTextWriter.WriteStartElement("Cell");
                                    xmlTextWriter.WriteRaw("<Data ss:Type=\"String\">" + rdr.GetName(i) + "</Data>");
                                    xmlTextWriter.WriteEndElement();
                                }
                                xmlTextWriter.WriteEndElement();
                            }

                            //================================================
                            // FILAS
                            //================================================
                            xmlTextWriter.WriteStartElement("Row");
                            for (int i = 0; i < Columnas; i++)
                            {

                                xmlTextWriter.WriteStartElement("Cell");
                                //xmlTextWriter.WriteRaw("<Data ss:Type=\"String\">" + rdr.GetValue(i).ToString() + "</Data>");


                                Type Tipo = LST_TIPO.Where(p => p.COLUMNA == i).First().TIPO;
                                xmlTextWriter.WriteRaw("<Data ss:Type=" + @"""" + FormatoTipo(Tipo) + @"""" + ">" + rdr.GetValue(i).ToString() + "</Data>");
                                xmlTextWriter.WriteEndElement();

                            }
                            xmlTextWriter.WriteEndElement();






                            FILA++;
                            Console.WriteLine(FILA);
                        }


                        //====================================================
                        // WORKSHEETOPTIONS
                        //====================================================
                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteStartElement(string.Empty, "WorksheetOptions", excel);

                        xmlTextWriter.WriteStartElement("PageSetup");

                        xmlTextWriter.WriteStartElement("Header");
                        xmlTextWriter.WriteAttributeString("x:Margin", "0.3");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Footer");
                        xmlTextWriter.WriteAttributeString("x:Margin", "0.3");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("PageMargins");
                        xmlTextWriter.WriteAttributeString("x:Bottom", "0.75");
                        xmlTextWriter.WriteAttributeString("x:Left", "0.7");
                        xmlTextWriter.WriteAttributeString("x:Right", "0.7");
                        xmlTextWriter.WriteAttributeString("x:Top", "0.75");
                        xmlTextWriter.WriteEndElement();


                        xmlTextWriter.WriteEndElement();


                        xmlTextWriter.WriteStartElement("Selected");
                        xmlTextWriter.WriteEndElement();

                        xmlTextWriter.WriteStartElement("Panes");
                        xmlTextWriter.WriteStartElement("Pane");
                        xmlTextWriter.WriteElementString("Number", "3");//To be modified
                        xmlTextWriter.WriteElementString("ActiveRow", "2");//To be modified
                        xmlTextWriter.WriteElementString("ActiveCol", "3");//To be modified
                        xmlTextWriter.WriteEndElement();//End Pane    
                        xmlTextWriter.WriteEndElement();//End Panes   


                        xmlTextWriter.WriteElementString("ProtectObjects", "False");//To be modified
                        xmlTextWriter.WriteElementString("ProtectScenarios", "False");//To be modified
                        xmlTextWriter.WriteEndElement();//End WorksheetOptions   




                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.Flush();
                        xmlTextWriter.Close();


                        //====================================================
                        // FILAS
                        //====================================================








                    }
                    //========================================================
                    // FIN READER
                    //========================================================



                }





            }
            catch
            {
                throw;

            }
            finally
            {

                //============================================================
                // CERRAR VARIABLES DE CONECCION
                //============================================================
                if (Conexion != null)
                {
                    if (Conexion.State == ConnectionState.Open)
                    {
                        Conexion.Close();
                    }
                    Conexion = null;
                }





            }


        }

        /// <summary>
        /// PROCESAR XML
        /// </summary>
        /// <param name="CadenaConeccion"></param>
        /// <param name="query"></param>
        private static void PROCESAR_OLDB_QUERY(string CadenaConeccion, string query)
        {

         
            //===============================================================
            // DECLARACION DE VARIABLES 
            //===============================================================
            SqlConnection Conexion = null;

            try
            {


                //============================================================
                // ABRIR LA CONECCION
                //============================================================
                Conexion = new SqlConnection(CadenaConeccion);
                Conexion.Open();




                //============================================================
                // ESTABLECER EL COMAND 
                //============================================================
                using (SqlCommand Comando = new SqlCommand())
                {

                    Comando.CommandType = CommandType.Text;
                    Comando.CommandText = query;
                    Comando.Connection = Conexion;
                    Comando.ExecuteNonQuery();


                }





            }
            catch
            {
                throw;

            }
            finally
            {

                //============================================================
                // CERRAR VARIABLES DE CONECCION
                //============================================================
                if (Conexion != null)
                {
                    if (Conexion.State == ConnectionState.Open)
                    {
                        Conexion.Close();
                    }
                    Conexion = null;
                }





            }


        }

        /// <summary>
        /// PROCESAR ENVIO DE CORREO ELECTRONICO
        /// </summary>
        /// <param name="TOKEN"></param>
        /// <param name="RUTA_XSLT"></param>
        /// <param name="RUTA_IMAGEN"></param>
        /// <param name="CORREO"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static bool PROCESAR_ENVIO_CORREO(string TOKEN, string RUTA_XSLT, string RUTA_IMAGEN, string CORREO ,string URL)
        {
            try
            {


                //================================================================
                // DATASET
                //================================================================
                DataSet DataXML = new DataSet();


                //================================================================
                // CONSTRUIR LISTA
                //================================================================
                List<RUTA_DOWNLOAD> LST_Download = new List<RUTA_DOWNLOAD>();
                LST_Download.Add(new RUTA_DOWNLOAD { URL = URL });


                DataXML = LST_Download.ToDataSet<RUTA_DOWNLOAD>();


                //================================================================
                // CONVERTIR DATASET
                //================================================================
                string StrXML = DataXML.GetXml();
                StrXML = StrXML.ToString().Replace("NewDataSet", "RESULT");

                XmlDocument DOC = new XmlDocument();
                DOC.LoadXml(StrXML);


                //================================================================
                // CONFIGURACION DEL BODY
                //================================================================
                XPathDocument DOC_XSLT = new XPathDocument(new StringReader(DOC.InnerXml.ToString()));
                XslTransform xslt = new XslTransform();
                xslt.Load(RUTA_XSLT);
                StringWriter SB = new StringWriter();
                xslt.Transform(DOC_XSLT, null, SB);


                string BODY = SB.ToString();

                //================================================================
                // CONFIGURACION DEL BODY IMAGEN
                //================================================================
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(BODY,
                             Encoding.UTF8,
                             MediaTypeNames.Text.Html);


                LinkedResource img = new LinkedResource(RUTA_IMAGEN, MediaTypeNames.Image.Jpeg);
                img.ContentId = "imagen";
                htmlView.LinkedResources.Add(img);


                //================================================================
                // ENVIA CORREO ELETRONICO
                //================================================================
                string ASUNTO = "OPERACIONES SOLICITADAS TOKEN " + TOKEN;



                bool ENVIO = UEmail.ENVIA_CORREO_ELECTRONICO(ASUNTO
                                                            , "OPERACIONES"
                                                            , CORREO
                                                            , htmlView
                                                            , C_CON_SMTP);




                return ENVIO;


            }
            catch
            {

                throw;

            }


        }

        /// <summary>
        /// ENVIO DE CORREO
        /// </summary>
        /// <param name="LOTE"></param>
        /// <param name="RUTA_XSLT"></param>
        /// <param name="RUTA_IMAGEN"></param>
        /// <param name="CORREO"></param>
        /// <param name="TOKEN"></param>
        /// <returns></returns>
        private static bool PROCESAR_ENVIO_CORREO_DISCADOR(string LOTE, string RUTA_XSLT, string RUTA_IMAGEN, string CORREO, string URL)
        {
            try
            {


                //================================================================
                // DATASET
                //================================================================
                DataSet DataXML = new DataSet();


                //================================================================
                // CONSTRUIR LISTA
                //================================================================
                List<RUTA_DOWNLOAD> LST_Download = new List<RUTA_DOWNLOAD>();
                LST_Download.Add(new RUTA_DOWNLOAD { URL = URL });


                DataXML = LST_Download.ToDataSet<RUTA_DOWNLOAD>();


                //================================================================
                // CONVERTIR DATASET
                //================================================================
                string StrXML = DataXML.GetXml();
                StrXML = StrXML.ToString().Replace("NewDataSet", "RESULT");

                XmlDocument DOC = new XmlDocument();
                DOC.LoadXml(StrXML);


                //================================================================
                // CONFIGURACION DEL BODY
                //================================================================
                XPathDocument DOC_XSLT = new XPathDocument(new StringReader(DOC.InnerXml.ToString()));
                XslTransform xslt = new XslTransform();
                xslt.Load(RUTA_XSLT);
                StringWriter SB = new StringWriter();
                xslt.Transform(DOC_XSLT, null, SB);


                string BODY = SB.ToString();

                //================================================================
                // CONFIGURACION DEL BODY IMAGEN
                //================================================================
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(BODY,
                             Encoding.UTF8,
                             MediaTypeNames.Text.Html);


                LinkedResource img = new LinkedResource(RUTA_IMAGEN, MediaTypeNames.Image.Jpeg);
                img.ContentId = "imagen";
                htmlView.LinkedResources.Add(img);


                //================================================================
                // ENVIA CORREO ELETRONICO
                //================================================================
                string ASUNTO = "OPERACIONES SOLICITADAS LOTE " + LOTE;


                CORREO = CORREO + ";" + "explotador@ibrchile.cl";
                bool ENVIO = UEmail.ENVIA_CORREO_ELECTRONICO(ASUNTO
                                                            , "DISCADOR"
                                                            , CORREO
                                                            , htmlView
                                                            , C_CON_SMTP);




                return ENVIO;


            }
            catch
            {

                throw;

            }


        }

        /// <summary>
        /// TOTAL DE LINEAS
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        int TotalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        } 

        /// <summary>
        /// FORMATO DE TIPO
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string FormatoTipo(System.Type item)
        {
            switch (Type.GetTypeCode(item))
            {
                case TypeCode.Boolean: return "String";
                case TypeCode.Byte: return "Number";
                case TypeCode.Char: return "String";
                case TypeCode.SByte: return "Number";
                case TypeCode.DateTime: return "String";
                case TypeCode.Decimal: return "Number";
                case TypeCode.Double: return "Number";
                case TypeCode.Single: return "Number";
                case TypeCode.Int16: return "Number";
                case TypeCode.Int32: return "Number";
                case TypeCode.Int64: return "Number";
                case TypeCode.UInt16: return "Number";
                case TypeCode.UInt32: return "Number";
                case TypeCode.UInt64: return "Number";
                case TypeCode.String: return "String";
                case TypeCode.DBNull: return "String";
                case TypeCode.Empty: return "String";
                case TypeCode.Object: return "String";
                default: return "String";
            }
        }
        /// <summary>
        /// CONSOLA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string output = e.Data;
            Console.WriteLine("EJECUCION CONSOLA " + DateTime.Now);
            Console.WriteLine("DATA : " + output);

        }

        /// <summary>
        /// CAPTURA ERRORES
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void P_CaptureError(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("ERROR : " + e.Data);
        }
      


        /// <summary>
        /// AL INICIAR
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {

                Log.WriteMensajeLog("IN ONSTART SERVICIO SE ENCUENTRA INICIADO");

            }
            catch (Exception ex)
            {
                Log.WriteMensajeLog("ONSTART : " + ex.Message);
            }
        }

        /// <summary>
        /// AL FINALIZAR
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStop()
        {
            Log.WriteMensajeLog("IN ONSTOP SERVICIO SE ENCUENTRA DETENIDO");

           
        }
    }
}
