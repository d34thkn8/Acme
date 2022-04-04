using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ACME.ENCUESTAS.API.Utils
{
    public class XmlHelper
    {
        public static string ConvertListObjectToXmlDetail<T>(List<T> items)
        {
            DataTable table = CreateDataTableFromObjects(items);

            string result = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<datos>");
            if (table != null)
                if (table.Rows.Count > 0)
                    foreach (DataRow row in table.Rows)
                    {
                        sb.Append("<detalle>");
                        foreach (DataColumn column in table.Columns)
                        {
                            sb.Append("<" + column.ColumnName + ">");
                            string res = "";
                            if (column.DataType == typeof(bool))
                                res = (bool)row[column.ColumnName] == true ? "1" : "0";
                            else if (column.DataType == typeof(DateTime))
                            {
                                DateTime fecha = ((DateTime)row[column.ColumnName]);

                                if (fecha.Year < 1900)
                                {
                                    fecha = new DateTime(1900, 1, 1);
                                    res = fecha.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                    res = ((DateTime)row[column.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                                res = row[column.ColumnName].ToString();
                            sb.Append(res);
                            sb.Append("</" + column.ColumnName + ">");
                        }
                        sb.Append("</detalle>");
                    }
            sb.Append("</datos>");
            result = sb.ToString();

            return result;
        }

        private static DataTable CreateDataTableFromObjects<T>(List<T> items)
        {
            var myType = typeof(T);
            string name = myType.Name;
            DataTable dt = new DataTable(name);

            foreach (PropertyInfo info in myType.GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name.ToLower(), info.PropertyType));
            }

            foreach (var item in items)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo info in myType.GetProperties())
                {
                    dr[info.Name.ToLower()] = info.GetValue(item);
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
