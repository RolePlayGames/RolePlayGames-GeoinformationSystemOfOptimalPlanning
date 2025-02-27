using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Optimization.Genetic.FinalConditionCheckers;

public class PopulationFitnessDescendingFinalConditionChecker<TGene> : IFinalConditionChecker<IPopulation<TGene>> where TGene : IGene
{
    private readonly int _generationsCount;

    private int _currentGeneration;
    private double _bestFitness;

    public PopulationFitnessDescendingFinalConditionChecker(int generationsCount)
    {
        if (generationsCount < 1)
            throw new ArgumentOutOfRangeException(nameof(generationsCount), "Generation coune should be greater than 1");

        _generationsCount = generationsCount;
    }

    public void Begin()
    {
        _currentGeneration = 0;
        _bestFitness = double.MinValue;
    }

    public bool IsStateFinal(IPopulation<TGene> currentPopulation)
    {
        var bestFitness = currentPopulation.Best?.FitnessFunctionValue;

        if (_bestFitness < bestFitness)
        {
            _currentGeneration = 0;
            _bestFitness = bestFitness.Value;
        }
        else
        {
            _currentGeneration++;
        }

        return _currentGeneration >= _generationsCount;
    }
}
