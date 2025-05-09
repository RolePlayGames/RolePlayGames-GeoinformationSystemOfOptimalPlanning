﻿namespace GSOP.Domain.Algorithms.Genetic.Extensions;

public static class IEnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static T GetMinElement<T, M>(this IEnumerable<T> collection, Func<T, M> getMin) where M : IComparable
    {
        var enumerator = collection.GetEnumerator();
        enumerator.MoveNext();

        var minElement = enumerator.Current;
        var minParameter = getMin(minElement);

        while (enumerator.MoveNext())
        {
            var parameter = getMin(enumerator.Current);

            if (minParameter.CompareTo(parameter) > 0)
            {
                minElement = enumerator.Current;
                minParameter = parameter;
            }
        }

        return minElement;
    }
}
