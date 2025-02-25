namespace GSOP.Domain.Algorithms.Genetic.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Swap element on pointed indexes
    /// </summary>
    /// <typeparam name="T">Array type</typeparam>
    /// <param name="array">Extension parameter</param>
    /// <param name="firstIndex">First index</param>
    /// <param name="secondIndex">Second index</param>
    public static void Swap<T>(this T[] array, int firstIndex, int secondIndex)
    {
        (array[secondIndex], array[firstIndex]) = (array[firstIndex], array[secondIndex]);
    }
}
