using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ALO.Entidades
{
    public class DATOS_DB
    {
        public String SERVIDOR { get; set; }
        public String USUARIO { get; set; }
        public String PASS { get; set; }
        public String DB { get; set; }
    }

    /// <summary>
    /// ESTRUCTURA PARAMETROS
    /// </summary>
    public class oInputParameterSP_P
    {

        public String SP { get; set; }
        public decimal ID { get; set; }
        public String PARAMETRO { get; set; }
        public String TIPO { get; set; }
        public int LARGO { get; set; }
        public int OUTP { get; set; }
        public SqlDbType TIPO_SQLTYPE { get; set; }
        public Type TIPO_C { get; set; }
        public String VALOR_DEFECTO { get; set; }
    }


    /// <summary>
    /// ESTRUCTURA PARAMETROS
    /// </summary>
    public class oOutputParameterSP_P
    {

        public int ID_METODO { get; set; }
        public int ORDEN { get; set; }
        public string NOMBRE { get; set; }
        public string TIPO { get; set; }


    }
}
