using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using ALO.Entidades;

namespace ALO.Utilidades
{
    public class NInfoDB
    {

        DATOS_DB Parameros;


        /// <summary>
        /// CONSTRUCTOR DE LA CLASE
        /// </summary>
        /// <param name="ParametrosConeccion"></param>
        public NInfoDB(DATOS_DB ParametrosConeccion)
        {
            Parameros = ParametrosConeccion;
        }



        /// <summary>
        /// PARAMETROS INPUT SP CON SUS DATOS
        /// </summary>
        /// <param name="SP"></param>
        /// <returns></returns>
        public List<oInputParameterSP_P> ParametrosInput(string ScriptFile, string SP)
        {


            //================================================================
            // VARIABLES DE CONECCION AL SISTEMA                            --
            //================================================================
            IDataReader Reader = null;
            SqlConnection SPConexion;
            string CadenaStringConexion = new UCadenaConexion().CadenaConexion(Parameros.USUARIO, Parameros.PASS, Parameros.DB, Parameros.SERVIDOR);
            SPConexion = new SqlConnection(CadenaStringConexion);
            List<oInputParameterSP_P> Lista = new List<oInputParameterSP_P>();


            try
            {




                //===========================================================
                // CONEXION PARA LA EJECUCIO DE BASES DE DATOS DE SISTEMA  ==
                //===========================================================
                SPConexion.Open();



                //===========================================================
                // INICIALIZA CADENA SCRIPT BASE DE DATOS                  ==
                //===========================================================
                System.IO.StreamReader sr;
                String str;
                sr = System.IO.File.OpenText(ScriptFile);
                str = sr.ReadToEnd();
                sr.Close();


                str = str.Replace("#SP", "'" + SP + "'");





                //===========================================================
                // EJECUCION                                               ==
                //===========================================================
                using (SqlCommand Comando = new SqlCommand())
                {
                    Comando.Connection = SPConexion;
                    Comando.CommandType = CommandType.Text;
                    Comando.CommandText = str;



                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = Comando;
                    adapter.Fill(ds);

                    //========================================================
                    // PARAMETROS DE PARA CONVERTIR EN LISTA
                    //========================================================
                    Lista = DataTableToList<oInputParameterSP_P>(ds.Tables[0]);





                }

                return Lista;

            }
            catch
            {

                throw;
            }
            finally
            {

                try
                {
                    if (SPConexion.State == ConnectionState.Open)
                    {
                        SPConexion.Close();
                    }
                    SPConexion = null;

                    Reader.Close();
                }
                catch { }
            }

        }


        /// <summary>
        /// PARAMETROS OUTPUT
        /// </summary>
        /// <param name="Script"></param>
        /// <param name="SP"></param>
        /// <returns></returns>
        public List<oOutputParameterSP_P> ParametrosOutputObj(string Script)
        {

            //================================================================
            // DECLARACION DE VARIABLES DE RETORNO                          --
            //================================================================
            List<oOutputParameterSP_P> Retorno = new List<oOutputParameterSP_P>();



            //================================================================
            // VARIABLES DE CONECCION AL SISTEMA                            --
            //================================================================
            IDataReader Reader = null;
            SqlConnection SPConexion;
            string CadenaStringConexion = new UCadenaConexion().CadenaConexion(Parameros.USUARIO, Parameros.PASS, Parameros.DB, Parameros.SERVIDOR);
            SPConexion = new SqlConnection(CadenaStringConexion);


            try
            {



                //===========================================================
                // CONEXION PARA LA EJECUCIO DE BASES DE DATOS DE SISTEMA  ==
                //===========================================================
                SPConexion.Open();






                //===========================================================
                // EJECUCION                                               ==
                //===========================================================
                using (SqlCommand Comando = new SqlCommand())
                {
                    Comando.Connection = SPConexion;
                    Comando.CommandType = CommandType.Text;
                    Comando.CommandText = Script;

                    Reader = Comando.ExecuteReader();

                    int Contador = 0;
                    while (Contador <= Reader.FieldCount - 1)
                    {

                        oOutputParameterSP_P Entidad = new oOutputParameterSP_P();
                        Entidad.TIPO = Reader.GetFieldType(Contador).ToString();
                        Entidad.NOMBRE = Reader.GetName(Contador);
                        Entidad.ORDEN = Contador + 1;


                        Retorno.Add(Entidad);
                        Contador++;

                    }



                }

                return Retorno;

            }
            catch
            {

                throw;
            }
            finally
            {

                try
                {
                    if (SPConexion.State == ConnectionState.Open)
                    {
                        SPConexion.Close();
                    }
                    SPConexion = null;

                    Reader.Close();
                }
                catch { }
            }

        }







        /// <summary>
        /// CREACIÓN DE ENTIDADES A LISTAS POR MEDIO DE UN DATATABLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<T> DataTableToList<T>(DataTable table)
        {
            try
            {
                List<T> list = new List<T>();
                T obj = default(T);

                foreach (var row in table.AsEnumerable())
                {

                    obj = Activator.CreateInstance<T>();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

    }
}
