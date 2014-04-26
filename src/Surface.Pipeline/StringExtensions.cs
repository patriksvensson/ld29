using System.ComponentModel;

namespace Surface.Pipeline
{
    internal static class StringExtensions
    {
        public static T Convert<T>(this string value, T defaultValue)
        {
            if (value != null)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
                if (converter.CanConvertFrom(typeof (string)))
                {
                    return (T) converter.ConvertFromInvariantString(value);
                }
            }
            return defaultValue;
        }
    }
}