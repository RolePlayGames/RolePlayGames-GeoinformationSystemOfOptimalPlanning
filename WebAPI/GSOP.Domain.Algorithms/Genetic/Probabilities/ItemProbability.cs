namespace GSOP.Domain.Algorithms.Genetic.Probabilities;

public readonly struct ItemProbability<T>
{
    public T Item { get; }

    public double Probability { get; }

    public ItemProbability(T item, double probability)
    {
        if (probability < 0 || probability > 1)
            throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be between 0 and 1");

        Item = item;
        Probability = probability;
    }
}
