using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALO.Entidades
{

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_RETURN_STATUS
    {
        public Decimal RETURN_VALUE { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class TIPO_DATO
    {
        public int COLUMNA { get; set; }
        public Type TIPO { get; set; }

    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class RUTA_DOWNLOAD
    {
        public string URL { get; set; }

    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class RUTA_LOTE
    {
        public string LOTE { get; set; }

    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class XML_CONFIGURACIONES
    {
        public String MENSAJE { get; set; }
        public String SERVICIO { get; set; }
        public String DESCARGA { get; set; }
        public String RUTA { get; set; }
        public String RUTA_TOKEN { get; set; }
        public String PS { get; set; }
        public String intervalTime_1 { get; set; }
        

    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class XML_CONFIGURACIONES_EMAIL
    {

        public String HOST { get; set; }
        public String PUERTO { get; set; }
        public String CORREO_FROM { get; set; }
        public String USA_CREDENCIALES { get; set; }
        public String USA_SSL { get; set; }
        public String USUARIO { get; set; }
        public String PASSWORD { get; set; }

    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class XML_CONFIG_SERVER
    {

        public String SERVIDOR { get; set; }
        public String USUARIO { get; set; }
        public String PASSWORD { get; set; }


    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_FIFO_INTERFAZ_DIS
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public Int32 ID_INTERFAZ { get; set; }
        public Int32 ID_USUARIO { get; set; }
        public Int32 ID_CAMPANA { get; set; }
        public Int32 NRO_LOTE { get; set; }
        public String TABLA { get; set; }
        public Int32 ID_PAIS { get; set; }
        public String LOGIN { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_CREATE_FIFO_INTERFAZ_DIS_X_PROCESO
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public String ARCHIVO { get; set; }
        public Boolean EJECUTADO { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_DELETE_FIFO_INTERFAZ_DIS_X_PROCESO
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_UPDATE_FIFO_INTERFAZ_DIS_ESTADO
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public Int32 OPCION { get; set; }
        public String MENSAJE { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_FIFO_INTERFAZ_DIS_EJEC
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public Int32 ID_INTERFAZ { get; set; }
        public string LOGIN { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_FIFO_INTERFAZ_DIS_DB
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public Int32 ID_INTERFAZ { get; set; }
        public String LOGIN { get; set; }
        public String CORREO { get; set; }
        public Int32 NRO_LOTE { get; set; }
        public Int32 ID_USUARIO { get; set; }
    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_DELETE_FIFO_INTERFAZ_DIS
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_INTERFAZ_DETALLE
    {
        public Int32 ID_INTERFAZ { get; set; }
    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_INTERFAZ_DETALLE
    {
        public Int32 ID_INTERFAZ { get; set; }
        public Int32 ORDEN { get; set; }
        public String CAMPO { get; set; }
    }


    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_READ_TABLA_INTERFAZ
    {
        public String TABLA { get; set; }
        public Int32 ID_USUARIO { get; set; }
        public Int32 ID_INTERFAZ { get; set; }
        public Int32 ID_CAMPANA { get; set; }
    }

    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class oSP_READ_TABLA_INTERFAZ
    {
        public Boolean EXISTE { get; set; }
        public Int32 REGISTROS { get; set; }
    }




    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class iSP_UPDATE_FIFO_INTERFAZ_DIS_NUMERO
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public Int32 NUMERO { get; set; }
    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/

    public class oSP_READ_FIFO_INTERFAZ_DIS_X_PROCESO_EJE
    {
        public Int32 ID_FIFO_INTERFAZ_DIS { get; set; }
        public String ARCHIVO { get; set; }
        public Boolean EJECUTADO { get; set; }
        public Int32 PID { get; set; }
    }
    /*----------------------------------------------------------------------------*/
    /*----------------------------------------------------------------------------*/
    public class PROCESS_PID
    {
        public Int32 PID { get; set; }
    }
}
