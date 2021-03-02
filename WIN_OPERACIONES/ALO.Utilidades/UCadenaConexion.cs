using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALO.Utilidades
{
    public class UCadenaConexion
    {


        /// <summary>
        /// CADENA DE CONECCIÓN
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Paswword"></param>
        /// <param name="BaseDatos"></param>
        /// <param name="Servidor"></param>
        /// <returns></returns>
        public string CadenaConexion(string Usuario
                                   , string Paswword
                                   , string BaseDatos
                                   , string Servidor)
        {

            string Retorno = "";
            try
            {

                //===========================================================
                // CADENA DE CONEXION                                      ==
                //===========================================================
                Retorno = @"User ID=" + Usuario + ";"
                        + "Password =" + Paswword + ";"
                        + "Initial Catalog =" + BaseDatos + ";"
                        + "Data Source=" + Servidor + ";"
                        + "Persist Security Info=True;"
                        + "Pooling=False;"
                        + "Connection Lifetime=1;"
                        + "Application Name=CONECCION_OLDB";



                return Retorno;

            }
            catch
            {
                return Retorno;
            }


        }



    }
}
