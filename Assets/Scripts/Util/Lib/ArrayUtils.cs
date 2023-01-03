
public static class ArrayUtils
{
    public static T[] AdvanceQueue<T>(T[] array, T newValue)
    {
        T[] output = new T[array.Length];
        array.CopyTo(output, 0);

        for(int i = 0; i < array.Length - 1; i++)
        {
            output[i + 1] = output[i];
        }
        output[0] = newValue;

        return output;
    }

    public static string ToString<T>(T[] array, string seperator)
    {
        string s = "";
        // add all but last
        for(int i = 0; i < array.Length - 1; i++)
        {
            s += array[i].ToString() + seperator;
        }

        // add last
        s += array[array.Length - 1].ToString();

        return s;
    }
}
