namespace GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;

public class CombinationsGenerator : ICombinationsGenerator
{
    public IEnumerable<List<int>> GenerateCombinations(List<int> variations)
    {
        var controlled = new int[variations.Count];

        yield return variations.ToList();

        var i = 0;

        while (i < variations.Count)
        {
            if (controlled[i] < i)
            {
                if (i % 2 == 0)
                {
                    (variations[0], variations[i]) = (variations[i], variations[0]);
                }
                else
                {
                    (variations[controlled[i]], variations[i]) = (variations[i], variations[controlled[i]]);
                }

                yield return variations.ToList();

                controlled[i]++;
                i = 0;
            }
            else
            {
                controlled[i] = 0;
                i++;
            }
        }
    }
}
