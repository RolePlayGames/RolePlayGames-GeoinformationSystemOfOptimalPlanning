using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Optimization.Genetic.FinalConditionCheckers;

public class PopulationFitnessDescendingFinalConditionChecker<TGene> : IFinalConditionChecker<IPopulation<TGene>> where TGene : IGene
{
    protected int GenerationCount { get; }

    private int _currentGeneration;
    private double _bestFitness;

    public PopulationFitnessDescendingFinalConditionChecker(int generationCount)
    {
        if (generationCount < 1)
            throw new ArgumentOutOfRangeException(nameof(generationCount), "Generation coune should be greater than 1");

        GenerationCount = generationCount;
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

        return _currentGeneration >= GenerationCount;
    }
}
