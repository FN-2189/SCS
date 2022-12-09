
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
}
