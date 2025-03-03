using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;

namespace GSOP.Domain.Algorithms.Bruteforce;

public class BruteforeStackDistributor : IBruteforceDistributor
{
    private readonly ICombinationsGenerator _combinationsGenerator;

    public BruteforeStackDistributor(ICombinationsGenerator combinationsGenerator)
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

    private IEnumerable<List<List<TItem>>> PermuteBuckets<TItem>(List<List<TItem>> distribution)
    {
        // Создаем стек для хранения состояния обработки
        Stack<(List<List<TItem>> currentDistribution, int bucketIndex)> stack = new();

        // Начинаем с исходного распределения и первой корзины (индекс 0)
        stack.Push((distribution, 0));

        // Пока стек не пуст
        while (stack.Count > 0)
        {
            // Извлекаем текущее состояние из стека
            (var currentDistribution, var bucketIndex) = stack.Pop();

            // Если мы обработали все корзины, то возвращаем текущее распределение
            if (bucketIndex == currentDistribution.Count)
            {
                yield return currentDistribution;
                continue;
            }

            // Получаем текущую корзину
            var currentBucket = currentDistribution[bucketIndex];

            // Генерируем все перестановки элементов в текущей корзине
            foreach (var permutation in GenerateBucketPermutations(currentBucket))
            {
                // Создаем новое распределение, в котором текущая корзина содержит перестановку элементов
                var newDistribution = currentDistribution.Select(x => x.ToList()).ToList(); // Create a copy
                newDistribution[bucketIndex] = permutation;

                // Добавляем новое состояние в стек для обработки следующей корзины
                stack.Push((newDistribution, bucketIndex + 1));
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
            yield return new();
            yield break;
        }

        // Создаем очередь для хранения частичных назначений
        var queue = new Queue<List<int>>();
        queue.Enqueue([]); // Начинаем с пустого назначения

        // Пока очередь не пуста
        while (queue.Count > 0)
        {
            // Извлекаем частичное назначение из очереди
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