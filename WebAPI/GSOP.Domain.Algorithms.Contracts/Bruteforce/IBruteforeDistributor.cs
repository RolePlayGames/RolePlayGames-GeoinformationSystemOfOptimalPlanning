namespace GSOP.Domain.Algorithms.Contracts.Bruteforce;

public interface IBruteforceDistributor
{
    IEnumerable<List<List<TItem>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items);
}
