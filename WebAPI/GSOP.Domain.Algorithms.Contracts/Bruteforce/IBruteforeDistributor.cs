namespace GSOP.Domain.Algorithms.Contracts.Bruteforce;

public interface IBruteforeDistributor
{
    IEnumerable<List<List<int>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items);
}
