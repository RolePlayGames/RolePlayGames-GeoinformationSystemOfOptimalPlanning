namespace GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;

public interface ICombinationsGenerator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="variations"></param>
    /// <returns></returns>
    IEnumerable<List<int>> GenerateCombinations(List<int> variations);
}
