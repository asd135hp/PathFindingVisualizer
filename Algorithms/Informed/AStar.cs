using System;
using Algorithms.Utility;

namespace Algorithms.Informed
{
    public class AStar : Types.BestFirstSearchAlgorithm
    {
        public AStar(int mapRows, int mapCols) : base(mapRows, mapCols)
        {

        }

        public override string Name => "A*";

        protected override Func<State, int> GetTotalCost(State goal)
            => (nextState) => Heuristic.ManhattanDistance(nextState, goal) + Levels[GetIndex(nextState)];
    }
}
