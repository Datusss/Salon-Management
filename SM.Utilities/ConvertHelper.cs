using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace SM.Utilities
{
    public class ConvertHelper
    {
        /// <summary>
        /// Convert List<T> to Datatable
        /// h.dinhhoan@gmail.com
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>DataTable</returns>
        public static DataSet ToDataSet<T>(IList<T> list)
        {
            var elementType = typeof(T);
            var ds = new DataSet();
            var t = new DataTable();
            t.TableName = elementType.Name;
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            var dtc = new DataColumn("GUID", typeof(string));
            t.Columns.Add(dtc);
            t.PrimaryKey = new DataColumn[] { dtc };
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
                row["GUID"] = Guid.NewGuid().ToString();
                t.Rows.Add(row);
            }
            ds.AcceptChanges();
            return ds;
        }
        public static Guid ToGuid(object val, Guid defValue)
        {
            var ret = defValue;
            try
            {
                ret = new Guid(val.ToString());
            }
            catch { }

            return ret;
        }

        public static Guid ToGuid(object val)
        {
            return ToGuid(val.ToString(), Guid.Empty);
        }

        public static Guid ToGuid(SqlGuid val)
        {
            return val.IsNull ? Guid.Empty : val.Value;
        }

        public static byte ToByte(object obj)
        {
            byte retVal = 0;

            try
            {
                retVal = Convert.ToByte(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static int ToInt32(object obj)
        {
            int retVal = 0;

            try
            {
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static long ToInt64(object obj)
        {
            long retVal = 0;

            try
            {
                retVal = Convert.ToInt64(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            int retVal;
            try
            {
                if (obj == null)
                    return defaultValue;
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static string ToString(object obj)
        {
            string retVal;
            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = String.Empty;
            }

            return retVal;
        }

        public static string ToString(object obj, string def)
        {
            string retVal;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = def;
            }

            return retVal;
        }
        public static DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, DateTime.Now);
        }

        public static DateTime ToDateTime(object obj, string format)
        {
            return DateTime.ParseExact(obj.ToString(), format, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            DateTime retVal;
            try
            {
                retVal = Convert.ToDateTime(obj);
            }
            catch
            {
                retVal = defaultValue;
            }
            if (retVal >= (DateTime)SqlDateTime.MaxValue) return (DateTime)SqlDateTime.MaxValue;
            return retVal <= (DateTime)SqlDateTime.MinValue ? ((DateTime)SqlDateTime.MinValue).AddYears(5) : retVal;
        }

        public static bool ToBoolean(object obj)
        {
            bool retVal;

            try
            {
                retVal = Convert.ToBoolean(obj);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        public static double ToDouble(object obj)
        {
            double retVal;

            try
            {
                retVal = Convert.ToDouble(obj, CultureInfo.InvariantCulture);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static float ToSingle(object obj)
        {
            float retVal;

            try
            {
                retVal = Convert.ToSingle(obj, CultureInfo.InvariantCulture);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }
        public static double ToDouble(object obj, double defaultValue)
        {
            double retVal;

            try
            {
                retVal = Convert.ToDouble(obj.ToString().Trim(), CultureInfo.InvariantCulture);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static Decimal ToDecimal(object val, Decimal defValue)
        {
            try
            {
                return Convert.ToDecimal(val, CultureInfo.InvariantCulture);

            }
            catch { return defValue; }
        }

        /*
        public static float ToFloat(object val, float defValue = 0)
        {
            try
            {
                return Convert.ToDou
            }
            catch
            {
                return defValue; 
            }
        }
        */
        public static Decimal ToDecimal(object val)
        {
            return ToDecimal(val, 0);
        }

        public static Decimal ToDecimal(SqlDecimal val)
        {
            return val.IsNull ? Decimal.Zero : val.Value;
        }

        public static long ToJavaScriptMilliseconds(DateTime dt)
        {
            var datetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            return ((dt.ToUniversalTime().Ticks - datetimeMinTimeTicks) / 10000);
        }
        public static Decimal CurrencyToDecimal(object val)
        {
            var strval = ToString(val);
            strval = strval.Replace("$", string.Empty);
            strval = strval.Replace(",", string.Empty);
            return ToDecimal(strval.Trim());
        }

        /// <summary>
        /// Format to $0.00
        /// </summary>
        /// <param name="val">Value to format</param>
        /// <returns></returns>
        public static string ToCurrency(object val)
        {
            return string.Format("{0:C}", val);
        }
        /// <summary>
        /// Format to 0.00
        /// </summary>
        /// <param name="val">Value to format</param>
        /// <returns></returns>
        public static string ToNumberic(object val)
        {
            var result= string.Format(new CultureInfo("en-US"),"{0:#,##0.##}", val);
            if (string.IsNullOrEmpty(result))
            {
                return "0";
            }
            else
            {
                return result;
            }
        }
    }
}
