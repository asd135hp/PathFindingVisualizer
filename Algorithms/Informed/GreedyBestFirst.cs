using System;

namespace Algorithms.Informed
{
    public class GreedyBestFirst : Types.BestFirstSearchAlgorithm
    {
        public GreedyBestFirst(int mapRows, int mapCols) : base(mapRows, mapCols)
        {
        }

        public override string Name => "Greedy Best First Search";

        protected override Func<State, int> GetTotalCost(State goal)
            => (nextState) => Utility.Heuristic.ManhattanDistance(nextState, goal);
    }
}
