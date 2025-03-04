using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using System.Collections.Concurrent;

namespace GSOP.Domain.Algorithms.Bruteforce;

public class ParallelBruteforceStackDistributor : IBruteforceDistributor
{
    private readonly ICombinationsGenerator _combinationsGenerator;

    public ParallelBruteforceStackDistributor(ICombinationsGenerator combinationsGenerator)
    {
        _combinationsGenerator = combinationsGenerator;
    }

    public IEnumerable<List<List<TItem>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items)
    {
        var numBuckets = buckets.Count;
        var numItems = items.Count;

        // Генерируем все возможные распределения индексов
        foreach (var assignment in GenerateAllPossibleAssignments(numItems, numBuckets))
        {
            // Создаем список корзин для текущего распределения
            var distribution = Enumerable.Range(0, numBuckets).Select(_ => new List<TItem>()).ToList();

            // Заполняем корзины элементами на основе индексов
            for (var i = 0; i < numItems; i++)
            {
                distribution[assignment[i]].Add(items.ElementAt(i));
            }

            // Генерируем все перестановки элементов в каждой корзине
            foreach (var permutedDistribution in PermuteBuckets(distribution))
            {
                yield return permutedDistribution;
            }
        }
    }

    private ConcurrentQueue<List<List<TItem>>> PermuteBuckets<TItem>(List<List<TItem>> distribution)
    {
        var resultsQueue = new ConcurrentQueue<List<List<TItem>>>();
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };

        // Распараллеливаем перестановку корзин
        Parallel.ForEach(GenerateBucketPermutations(distribution), parallelOptions, resultsQueue.Enqueue);

        return resultsQueue;
    }

    // Перемещена логика из PermuteBuckets в GenerateBucketPermutations
    private IEnumerable<List<List<TItem>>> GenerateBucketPermutations<TItem>(List<List<TItem>> distribution)
    {
        if (distribution.Count == 0)
        {
            yield return new List<List<TItem>>();
            yield break;
        }

        // Берем первую корзину
        var firstBucket = distribution[0];
        var remainingBuckets = distribution.Skip(1).ToList();

        // Генерируем все перестановки элементов в первой корзине
        foreach (var permutation in GenerateBucketPermutations(firstBucket))
        {
            // Создаем новое распределение, в котором первая корзина содержит перестановку элементов
            var newDistribution = new List<List<TItem>> { permutation };

            // Рекурсивно генерируем все перестановки элементов в остальных корзинах
            foreach (var permutedRemainingBuckets in GenerateBucketPermutations(remainingBuckets))
            {
                yield return newDistribution.Concat(permutedRemainingBuckets).ToList();
            }
        }
    }

    private IEnumerable<List<TItem>> GenerateBucketPermutations<TItem>(List<TItem> bucket)
    {
        // Получаем все перестановки индексов элементов в корзине
        foreach (var permutation in _combinationsGenerator.GenerateCombinations(Enumerable.Range(0, bucket.Count).ToList()))
        {
            // Создаем новую корзину, в которой элементы переставлены в соответствии с перестановкой
            var permutedBucket = new List<TItem>();

            foreach (var index in permutation)
            {
                permutedBucket.Add(bucket[index]);
            }

            yield return permutedBucket;
        }
    }

    // Вспомогательная функция для генерации всех возможных назначений элементов корзинам
    private static IEnumerable<List<int>> GenerateAllPossibleAssignments(int numItems, int numBuckets)
    {
        if (numItems == 0)
        {
            yield return new List<int>();
            yield break;
        }

        // Создаем очередь для хранения частичных назначений
        var queue = new Queue<List<int>>();
        queue.Enqueue([]);

        while (queue.Count > 0)
        {
            var partialAssignment = queue.Dequeue();

            // Если длина частичного назначения равна numItems, то это полное назначение
            if (partialAssignment.Count == numItems)
            {
                yield return partialAssignment;
            }
            else
            {
                // В противном случае, добавляем все возможные варианты назначения для следующего элемента
                for (var i = 0; i < numBuckets; i++)
                {
                    var newAssignment = new List<int>(partialAssignment) { i };
                    queue.Enqueue(newAssignment);
                }
            }
        }
    }
}
