namespace Project.Utils;

public static class Util
{
    public static IEnumerable<string> SelectionIntToString(IList<string> allValues, IEnumerable<int> selectedValues)
    {
        foreach (var value in selectedValues)
        {
            yield return allValues[value];
        }
    }
    public static IEnumerable<int> SelectionStringToInt(IList<string> allValues, IEnumerable<string> selectedValues)
    {
        foreach (var value in selectedValues)
        {
            yield return FindIndex(allValues, value);
        }
    }
    public static bool SelectionIsInvalid(IEnumerable<int> selectedValues, int AllValuesCount) => (from idx in selectedValues where idx < 0 || idx >= AllValuesCount select idx).Any();
    public static int FindIndex(IList<string> allValues, string selectedValue)
    {
        for (var i = 0; i < allValues.Count; i++)
        {
            if (allValues[i] == selectedValue) { return i; }
        }
        return -1;
    }
}
