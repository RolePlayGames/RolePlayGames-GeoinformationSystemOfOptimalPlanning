namespace GSOP.Domain.Algorithms.Contracts.Genetic.Models;

public struct Parents<TGene> where TGene : IGene
{
    public IIndividual<TGene> FirstParent { get; set; }

    public IIndividual<TGene> SecondParent { get; set; }
}
