using System;

namespace SM.Utilities
{
    /// <summary>
    /// Added By Truong Xuan Khanh - 5/1/2012
    /// </summary>
    public class GenericConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Parse<T>(string sourceValue) where T : IConvertible
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceValue"></param>
        /// <param name="provider"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Parse<T>(string sourceValue, IFormatProvider provider) where T : IConvertible
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T), provider);
        }  
    }
}
