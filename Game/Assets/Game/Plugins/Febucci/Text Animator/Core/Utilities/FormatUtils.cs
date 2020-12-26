using System.Collections.Generic;

namespace Febucci.UI.Core
{
    public static class FormatUtils
    {
        /// <summary>
        /// Tries to parsing an attribute from a string list. If unsuccessful, sets the result it to the default value.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="index"></param>
        /// <param name="defValue"></param>
        /// <param name="result"></param>
        /// <returns>Returns true if successful</returns>
        public static bool TryGetFloat(List<string> attributes, int index, float defValue, out float result)
        {
            if (index < attributes.Count)
            {
                if (ParseFloat(attributes[index], out result))
                {
                    //Float parsed correctly, returns
                    return true;
                }
                else
                {
                    //Float couldn't parse, sets default value
                    result = defValue;
                    return false;
                }
            }

            result = defValue;
            return false;
        }

        /// <summary>
        /// Tries parsing a float given a string, independently of the system's culture
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ParseFloat(string value, out float result)
        {
            return float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
        }
    }

}