
public static class DebugUtils
{
    public static string ToString<T>(T[] array, string seperator = " ")
    {
        string s = "";
        for(int i = 0; i < array.Length - 1; i++)
        {
            s += array[i] + seperator;
        }

        s += array[array.Length - 1];

        return s;
    }
}
