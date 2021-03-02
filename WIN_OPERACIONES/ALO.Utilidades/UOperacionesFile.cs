using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ALO.Utilidades
{
    public static class UOperacionesFile
    {

        /// <summary>
        /// RUTA PADRE
        /// </summary>
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public static string PathPadre(string FullPath)
        {
            try
            {

                FileInfo FILE_EXE = new FileInfo(FullPath);
                string Path = "";


                char Separador = Convert.ToChar(@"\");
                string[] PathSplit = FILE_EXE.FullName.Split(Separador);

                if (PathSplit.Length > 1)
                {


                    if (PathSplit[PathSplit.Length - 1].Length > 0)
                    {


                        for (int i = 0; i < PathSplit.Length - 1; i++)
                        {
                            Path = Path + PathSplit[i].ToString() + @"\";
                        }

                        Path = Path.Substring(0, Path.Length - 1);

                    }

                    else
                    {
                        Path = "";

                    }
                }




                return Path;

            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// NOMBRE DE ARCHIVO
        /// <param name="Ruta"></param>
        /// <returns></returns>
        public static string NameFile(string FullPath)
        {
            try
            {

                FileInfo FILE_EXE = new FileInfo(FullPath);


                char Separador = Convert.ToChar(".");
                string[] PathSplit = FILE_EXE.Name.Split(Separador);



                if (PathSplit.Length > 1)
                {

                    return PathSplit[0].ToString();

                }
                else
                {

                    throw new Exception("NO SE PUDO DETERMINAR NOMBRE DE MSI");
                }


            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// VERIFICA ARCHIVO COPIADO
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CheckFileHasCopied(string FilePath)
        {

            try
            {

                if (File.Exists(FilePath))
                {

                    using (File.OpenRead(FilePath))
                    {

                        return true;

                    }
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {

                System.Threading.Thread.Sleep(100);

                return CheckFileHasCopied(FilePath);

            }

        }

        /// <summary>
        /// SEPARACION DE ARCHIVOS
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="File"></param>
        /// <param name="NombreBaseSplit"></param>
        /// <param name="CarpetaDestino"></param>
        public static void SplitFile(int Size, string File, string NombreBaseSplit, string CarpetaDestino)
        {

            try
            {

                int fileNumber = 0;
                string newFileName = CarpetaDestino + @"\" + NombreBaseSplit;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(File))
                {
                    while (!sr.EndOfStream)
                    {
                        int count = 0;
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(newFileName + ++fileNumber + ".txt"))
                        {
                            sw.AutoFlush = true;
                            while (!sr.EndOfStream && ++count < Size)
                            {
                                sw.WriteLine(sr.ReadLine());
                            }
                        }
                    }
                }

            }
            catch 
            {
               
                throw;
            }

        }
    }
}
