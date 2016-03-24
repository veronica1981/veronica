using System;
using System.Globalization;

namespace CopSite.Old_App_Code
{
    public static class Utils
    {
        public static string NomalizeForStorage(string input)
        {
            if (input == null) return null;
            var str = input.Trim();
            if (str.Equals(""))
            {
                return "0";
            }
            if (str.Equals("0"))
            {
                return "0.00001";
            }
            else
            {
                double value = 0;
                if (double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.CurrentCulture, out value) ||
                    double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    return value.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                }
                return null;
            }
        }

        public static string NormalizeForDisplay(object input, string displayCharacter)
        {

            if (input == null || input.Equals("0"))
            {
                return displayCharacter;
            }

            double value = 0;
            if (double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.CurrentCulture, out value) ||
                double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return Math.Abs(value - 0.00001) < 0.0001 ? "0" : Convert.ToString(input); // if value == 0.00001
            }

            return displayCharacter;
        }

        public static double ConvertToDouble(string input)
        {
            double value = 0;
            if (double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.CurrentCulture, out value) ||
                double.TryParse(Convert.ToString(input), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            return 0;
        }

        public static bool IsDouble(string strValue)
        {
            return IsDouble(strValue, NumberStyles.Any);
        }

        public static bool IsDouble(string strValue, NumberStyles numberStyles)
        {
            bool isNum = true;
            double value;
            if (!String.IsNullOrEmpty(strValue))
                isNum = (double.TryParse(Convert.ToString(strValue), numberStyles, CultureInfo.CurrentCulture, out value) ||
                         double.TryParse(Convert.ToString(strValue), numberStyles, CultureInfo.InvariantCulture, out value));

            return isNum;
        }

    }
}