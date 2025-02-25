using GSOP.Domain.Algorithms.Contracts;

namespace GSOP.Domain.Optimization.FinalConditionChecker;

public class TimeFinalConditionChecker<T> : IFinalConditionChecker<T>
{
    private const int _ticksInSecond = 1000;

    private readonly TimeSpan _maxTime;

    private DateTime _startTime;

    public TimeFinalConditionChecker(TimeSpan maxTime)
    {
        _maxTime = maxTime;
    }

    public TimeFinalConditionChecker(int seconds)
    {
        if (seconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds should be greater than 0");

        _maxTime = new TimeSpan(seconds * _ticksInSecond);
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
