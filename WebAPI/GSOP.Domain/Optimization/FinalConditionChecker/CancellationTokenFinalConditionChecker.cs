using GSOP.Domain.Algorithms.Contracts;

namespace GSOP.Domain.Optimization.FinalConditionChecker;

public class CancellationTokenFinalConditionChecker<T> : IFinalConditionChecker<T>
{
    private readonly CancellationToken _cancellationToken;

    public CancellationTokenFinalConditionChecker(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public void Begin()
    {

    }

    public bool IsStateFinal(T state)
    {
        return _cancellationToken.IsCancellationRequested;
    }
}
