using GSOP.Domain.Algorithms.Contracts;

namespace GSOP.Domain.Optimization.FinalConditionChecker;

public class TimeFinalConditionChecker<T> : IFinalConditionChecker<T>
{
    private readonly TimeSpan _maxTime;

    private DateTime _startTime;

    public TimeFinalConditionChecker(TimeSpan maxTime)
    {
        _maxTime = maxTime;
    }

    public void Begin()
    {
        _startTime = DateTime.Now;
    }

    public bool IsStateFinal(T state)
    {
        return DateTime.Now - _startTime >= _maxTime;
    }
}
