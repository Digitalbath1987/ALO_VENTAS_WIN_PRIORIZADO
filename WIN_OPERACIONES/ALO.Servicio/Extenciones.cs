using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ALO.Servicio
{
    public static class Extenciones
    {

        /// <summary>
        /// LISTA TO DATASET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ToDataSet<T>(this IList<T> list)
        {

            try
            {

                Type elementType = typeof(T);
                DataSet ds = new DataSet();
                DataTable t = new DataTable();
                ds.Tables.Add(t);


                //add a column to table for each public property on T
                foreach (var propInfo in elementType.GetProperties())
                {
                    Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                    t.Columns.Add(propInfo.Name, ColType);
                }

                //go through each property on T and add each value to the table
                foreach (T item in list)
                {
                    DataRow row = t.NewRow();

                    foreach (var propInfo in elementType.GetProperties())
                    {
                        row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                    }

                    t.Rows.Add(row);
                }

                return ds;
            }
            catch
            {
                throw;
            }
        }

    }
}
