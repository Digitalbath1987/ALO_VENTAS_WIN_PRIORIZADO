using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALO.Entidades
{
    //===================================================================
    /// <summary>
    /// ESTRUCTURA PARAMETROS
    /// </summary>
    public class R_METODO
    {
        public String SP { get; set; }
        public String SISTEMA { get; set; }
    }

    //===================================================================
    /// <summary>
    /// ESTRUCTURA PARAMETROS
    /// </summary>
    public class R_PARAM
    {

        public object PARAMETROS { get; set; }
    }


    //===================================================================
    /// <summary>
    /// ESTRUCTURA PARAMETROS
    /// </summary>
    public class R_FILTRO
    {

        public object PARAMETROS { get; set; }

    }

    //===================================================================
    /// <summary>
    /// INPUT JSON DEL APLICATIVO
    /// </summary>
    public class INPUT_JSON_ALO
    {



        public R_METODO R_METODO;
        public R_PARAM R_PARAM;
        public R_FILTRO R_FILTRO;

        //===========================================================
        // CONTRUCTOR                                              ==
        //===========================================================
        public INPUT_JSON_ALO()
        {
            R_METODO = new R_METODO();
            R_PARAM = new R_PARAM();
            R_FILTRO = new R_FILTRO();
        }



    }


    //===================================================================
    /// <summary>
    /// ENTIDAD DE ERRORES PROVOCADOS POR EL APLICATIVO
    /// </summary>
    public class ErroresException
    {

        public string NombreMetodo { get; set; }
        public string Clase { get; set; }
        public string NameSpace { get; set; }
        public string Mensaje { get; set; }
        public List<Secuencia> Eventos { get; set; }


        public ErroresException()
        {
            NombreMetodo = "";
            Clase = "";
            NameSpace = "";
            Mensaje = "";
            Eventos = new List<Secuencia>();
        }

    }
    //===================================================================
    /// <summary>
    /// SECUENCIA DE METODOS QUE PROVOCARON LA CAIDA
    /// </summary>
    public class Secuencia
    {

        public string Item { get; set; }

        public Secuencia()
        {
            Item = "";
        }
    }
    //===================================================================
    /// <summary>
    /// ESTADO BINARIO DEL APLICATIVO AL CONTESTAR
    /// </summary>
    public class Header
    {

        public int ESTADO { get; set; }
        public int ID_TIPO_RETORNO { get; set; }

    }
    //===================================================================
    /// <summary>
    /// RESULTADOS GENERICOS DEL SISTEMA
    /// </summary>
    public class Resultados
    {

        public object DETALLES { get; set; }

        public Resultados()
        {
            DETALLES = new object();
        }

    }
    //===================================================================
    /// <summary>
    /// RESULTADOS JSON DEL APLICATIVO
    /// </summary>
    public class OUTPUT_JSON_ALO
    {



        public Header HEADER;
        public ErroresException ERRORES;
        public Resultados RESULT;

        //===========================================================
        // CONTRUCTOR                                              ==
        //===========================================================
        public OUTPUT_JSON_ALO()
        {
            HEADER = new Header();
            ERRORES = new ErroresException();
            RESULT = new Resultados();
        }



    }
}
