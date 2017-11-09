using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Similarity
{
    public static class StringSimilarity
    {
        #region Public-Members

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        #endregion

        #region Public-Methods

        public static decimal Calculate(string str1, string str2)
        {
            if (String.IsNullOrEmpty(str1) && String.IsNullOrEmpty(str2)) return 1;
            if (String.IsNullOrEmpty(str1) && !String.IsNullOrEmpty(str2)) return 0;
            if (!String.IsNullOrEmpty(str1) && String.IsNullOrEmpty(str2)) return 0;
            if (str1.Equals(str2)) return 1;

            string baseStr1 = BaseString(str1);
            string baseStr2 = BaseString(str2);

            decimal scoreLength = 1;
            if (str1.Length > str2.Length) scoreLength = decimal.Divide(str2.Length, str1.Length);
            if (str2.Length > str1.Length) scoreLength = decimal.Divide(str1.Length, str2.Length); 

            char[] uniqueCharsStr1 = UniqueChars(str1);
            char[] uniqueCharsStr2 = UniqueChars(str2);
            int uniqueCharsLenStr1 = uniqueCharsStr1.Length;
            int uniqueCharsLenStr2 = uniqueCharsStr2.Length;
            int charMatch = 0;
            int totalChars = 0;
            decimal scoreCharMatch = 0;

            if (uniqueCharsLenStr1 > uniqueCharsLenStr2)
            {
                totalChars = uniqueCharsLenStr1;
                for (int i = 0; i < uniqueCharsStr1.Length; i++)
                {
                    if (uniqueCharsStr2.Contains(uniqueCharsStr1[i])) charMatch++;
                }
            }
            else
            {
                totalChars = uniqueCharsLenStr2;
                for (int i = 0; i < uniqueCharsStr2.Length; i++)
                {
                    if (uniqueCharsStr1.Contains(uniqueCharsStr2[i])) charMatch++;
                }
            }

            if (totalChars > 0) scoreCharMatch = decimal.Divide(charMatch, totalChars);
            return (decimal)(scoreLength * scoreCharMatch);
        }

        #endregion

        #region Private-Methods

        private static string BaseString(string str)
        {
            if (String.IsNullOrEmpty(str)) return "";

            string ret = "";

            for (int i = 0; i < str.Length; i++)
            {
                int j = (int)(str[i]);

                if (
                    (j > 47 && j < 58)
                    || (j > 64 && j < 91)
                    || (j > 96 && j < 123)
                    )
                {
                    ret += Convert.ToString(str[i]).ToLower();
                }
            }

            return ret;
        }

        private static int CharacterCount(string str)
        {
            if (String.IsNullOrEmpty(str)) return 0;

            int ret = 0;

            for (int i = 0; i < str.Length; i++)
            {
                int j = (int)(str[i]);

                if (
                    (j > 47 && j < 58)
                    || (j > 64 && j < 91)
                    || (j > 96 && j < 123)
                    )
                {
                    ret++;
                }
            }

            return ret;
        }

        private static char[] UniqueChars(string str)
        {
            if (String.IsNullOrEmpty(str)) return new List<char>().ToArray();
            return str.Distinct().ToArray();
        }

        #endregion
    }
}
