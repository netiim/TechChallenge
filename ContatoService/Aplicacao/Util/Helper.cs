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
        // Use uma expressão regular para encontrar palavras
        string pattern = @"\b\w+\b";
        return Regex.Replace(input, pattern, match =>
        {
            string word = match.Value;
            // Converta a palavra para minúsculas
            string lowerCaseWord = word.ToLower();
            // Converta a primeira letra para maiúscula
            string capitalizedWord = textInfo.ToTitleCase(lowerCaseWord);
            // Remova todos os caracteres não alfanuméricos e converta a palavra para camelCase
            return char.ToLower(capitalizedWord[0]) + capitalizedWord.Substring(1);
        });
    }
}
