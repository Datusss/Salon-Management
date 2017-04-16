using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SM.Utilities
{
    public class EnumHelper
    {
        public static string ToDisplayText<TEnum>(string type)
        {
            return (from TEnum e in Enum.GetValues(typeof(TEnum)) where e.ToString().Equals(type) select ToDescription((Enum)Enum.Parse(typeof(TEnum), e.ToString()))).FirstOrDefault();

        }

        public static string ToDescription(Enum value)
        {
            try
            {
                var attributes =
          (DescriptionAttribute[])
          value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static T EnumTypeTo<T>(object value)
        {
            var conversionType = typeof(T);

            if (!conversionType.IsGenericType && conversionType.IsEnum)
            {
                return (T)Enum.ToObject(conversionType, value);
            }

            try
            {
                return (T)Convert.ChangeType(value, conversionType);
            }
            catch
            {
                return default(T);
            }
        }

        public static List<T> ToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        public static Dictionary<int, string> ToDictionary<T>(bool useDescription = true) where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            return values.Cast<object>().ToDictionary(value => (int)value, value => useDescription ? ToDescription((Enum)Enum.Parse(typeof(T), value.ToString())) : value.ToString());
        }

        public static Dictionary<string, int> ToValueMap<T>() where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            return values.Cast<object>().ToDictionary(value => value.ToString(), value => (int)value);
        }
    }
}
