using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors;

public class TournamentIndividualsSelector<TGene> : IndividualsSelector<TGene> where TGene : IGene
{
    public const int _minTeamCapacity = 2;

    protected IBestSelector<TGene> BestSelector { get; }

    protected int TeamCapacity { get; }

    public TournamentIndividualsSelector(IBestSelector<TGene> bestSelector, int teamCapacity)
    {
        if (teamCapacity < _minTeamCapacity)
            throw new ArgumentOutOfRangeException(nameof(teamCapacity), "Team capacity should be greater than or equal to 2");

        BestSelector = bestSelector ?? throw new ArgumentNullException(nameof(bestSelector), "Best selector shouldn't be null");
        TeamCapacity = teamCapacity;
    }

    protected override IEnumerable<IIndividual<TGene>> SelectIndividualsInternal(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        foreach (var team in SelectTeams(individuals))
        {
            var bestIndividual = BestSelector.SelectBestIndividual(team);

            yield return bestIndividual;
        }
    }

    private IEnumerable<IEnumerable<IIndividual<TGene>>> SelectTeams(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var enumerator = individuals.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var team = new List<IIndividual<TGene>>();

            for (var i = 0; i < TeamCapacity; i++)
            {
                team.Add(enumerator.Current);

                if (!enumerator.MoveNext())
                    break;
            }

            yield return team;
        }
    }
}
