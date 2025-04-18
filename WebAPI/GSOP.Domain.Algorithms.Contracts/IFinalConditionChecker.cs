﻿namespace GSOP.Domain.Algorithms.Contracts;

public interface IFinalConditionChecker<T>
{
    /// <summary>
    /// Set start settings to checker
    /// </summary>
    void Begin();

    /// <summary>
    /// Check algorithm state is final
    /// </summary>
    /// <param name="state">Current state</param>
    /// <returns>Is that final state</returns>
    bool IsStateFinal(T state);
}
