namespace SM.Utilities
{
    public class Globals
    {
        public static object GetPropValue(object src, string propName)
        {
            if (src == null) return null;
            var prop = src.GetType().GetProperty(propName);
            return prop == null ? null : prop.GetValue(src, null);
        }    
       
    }
}
