using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ALO.Rest
{
    public class SObjetoJson : IDisposable
    {

        /// <summary>
        /// DESTRUCTOR
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public SObjetoJson()
        {


        }

        /// <summary>
        /// SERIALIZACION DE OBJETO
        /// </summary>
        /// <returns></returns>
        public string Serialize(Object Objeto)
        {


            //===============================================================
            // DECLARACION DE VARIABLES
            //===============================================================
            StringBuilder Sb = new StringBuilder();
            string JsonResult;
            JsonSerializer Serializer = new JsonSerializer();



            try
            {



                //===========================================================
                // SERIALIZAR
                //===========================================================
                using (StringWriter Sw = new StringWriter(Sb))
                {

                    using (JsonWriter Writer = new JsonTextWriter(Sw))
                    {


                        Serializer.Serialize(Writer, Objeto);
                        JsonResult = Sb.ToString();

                        Writer.Flush();
                        Writer.Close();

                    }



                    Sw.Flush();
                    Sw.Close();
                    Sw.Dispose();
                }

                //===========================================================
                // RETORNO
                //=========================================================== 
                return JsonResult;


            }
            catch
            {
                throw;
            }
            finally
            {

                Sb.Clear();
                Sb = null;
                Serializer = null;


            }

        }

        /// <summary>
        /// DESERIALIZAR OBJETO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string Json)
        {


            //===============================================================
            // DECLARACION DE VARIABLES
            //===============================================================
            T ResutObjeto;
            JsonSerializer SerializerJson = null;


            try
            {

                //===========================================================
                // DECLARACION DE VARIABLES
                //=========================================================== 
                SerializerJson = new JsonSerializer();


                //===========================================================
                // JSON A BYTE
                //=========================================================== 
                byte[] byteArray = Encoding.UTF8.GetBytes(Json);


                //===========================================================
                // MEMORIA
                //=========================================================== 
                using (MemoryStream StreamJson = new MemoryStream(byteArray))
                {

                    //=======================================================
                    // READER
                    //=======================================================
                    using (StreamReader ReaderJson = new StreamReader(StreamJson))
                    {

                        //===================================================
                        // JSON TEXT
                        //===================================================
                        using (JsonTextReader Reader = new JsonTextReader(ReaderJson))
                        {
                            ResutObjeto = SerializerJson.Deserialize<T>(Reader);
                            Reader.Close();
                        }


                        ReaderJson.Close();
                        ReaderJson.Dispose();

                    }

                    StreamJson.Flush();
                    StreamJson.Close();
                    StreamJson.Dispose();

                }



                return ResutObjeto;


            }
            catch
            {
                throw;
            }
            finally
            {



                SerializerJson = null;


            }

        }

    }
}
