using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aplicacao.Util;

public static class Helper
{
    public static string ToCamelCase(string input)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        string pattern = @"\b\w+\b";
        return Regex.Replace(input, pattern, match =>
        {
            string word = match.Value;
         
            string lowerCaseWord = word.ToLower();
            string capitalizedWord = textInfo.ToTitleCase(lowerCaseWord);
           
            return char.ToLower(capitalizedWord[0]) + capitalizedWord.Substring(1);
        });
    }
}
