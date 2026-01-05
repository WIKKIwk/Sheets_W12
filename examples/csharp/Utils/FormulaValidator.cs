using W12CSheets.Client.Models;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Formula validator utility
/// </summary>
public static class FormulaValidator
{
    private static readonly string[] SupportedFunctions = 
    {
        "SUM", "AVERAGE", "COUNT", "MAX", "MIN",
        "IF", "AND", "OR", "NOT",
        "VLOOKUP", "HLOOKUP", "INDEX", "MATCH",
        "LEN", "UPPER", "LOWER", "CONCATENATE",
        "TODAY", "NOW", "DATE", "TIME"
    };

    /// <summary>
    /// Check if formula syntax is valid
    /// </summary>
    public static bool IsValidFormula(string formula)
    {
        if (!formula.StartsWith("="))
            return false;

        if (formula.Length > 8192)
            return false;

        // Check for balanced parentheses
        int openParens = 0;
        foreach (char c in formula)
        {
            if (c == '(') openParens++;
            if (c == ')') openParens--;
            if (openParens < 0) return false;
        }

        return openParens == 0;
    }

    /// <summary>
    /// Extract function names from formula
    /// </summary>
    public static List<string> ExtractFunctions(string formula)
    {
        var functions = new List<string>();
        var current = "";

        for (int i = 0; i < formula.Length; i++)
        {
            if (char.IsLetter(formula[i]))
            {
                current += formula[i];
            }
            else if (formula[i] == '(' && !string.IsNullOrEmpty(current))
            {
                functions.Add(current.ToUpper());
                current = "";
            }
            else
            {
                current = "";
            }
        }

        return functions;
    }

    /// <summary>
    /// Check if all functions in formula are supported
    /// </summary>
    public static bool AreAllFunctionsSupported(string formula)
    {
        var functions = ExtractFunctions(formula);
        return functions.All(f => SupportedFunctions.Contains(f));
    }

    /// <summary>
    /// Get list of unsupported functions
    /// </summary>
    public static List<string> GetUnsupportedFunctions(string formula)
    {
        var functions = ExtractFunctions(formula);
        return functions.Where(f => !SupportedFunctions.Contains(f)).Distinct().ToList();
    }
}
