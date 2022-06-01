using System;
using System.Collections.Generic;
using Algorithms.Utility;
using Algorithms.Collections;

namespace Algorithms.Informed
{
    /// <summary>
    /// Source for theory: https://en.wikipedia.org/wiki/Fringe_search
    /// Source for correction: http://webdocs.cs.ualberta.ca/~games/pathfind/publications/cig2005.pdf
    /// </summary>
    public class FringeSearch: Types.InformedAlgorithm
    {
        public FringeSearch(int mapRows, int mapCols): base(mapRows, mapCols)
        {
            Levels = null;
        }

        public override string Name => "Fringe Search";

        protected override bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            Action<State, StateType> UIAction = null
        )
        {
            var fringe = new List<State>() { source };
            var cache = new Cache<State>(source);
            bool found = false;
            int fLimit, fStart;
            fLimit = fStart = Heuristic.ManhattanDistance(source, goal);

            while (!found && fringe.Count != 0)
            {
                int fMin = int.MaxValue;

                for(int i = 0; i < fringe.Count; ++i)
                {
                    var currentState = fringe[i];
                    var cacheBlock = cache.GetItem(currentState);
                    int f = cacheBlock.Cost + Heuristic.ManhattanDistance(currentState, goal);

                    if (currentState.Equals(goal))
                    {
                        found = true;
                        break;
                    }

                    // invoke ui action, which is necessary for visualizer
                    // (confirm the next state to be searched)
                    UIAction?.Invoke(
                        currentState,
                        currentState.Type != StateType.Start && currentState.Type != StateType.Goal ?
                            StateType.Next : StateType.NoChange
                    );

                    // reconsider min total cost if it goes beyond the allowed limit
                    // as well as posibly assigning new limit after this
                    // typical implementation of IDA* but using loop
                    if (f > fLimit)
                    {
                        fMin = Math.Min(f, fMin);
                        continue;
                    }

                    foreach(var direction in graph.GetAdjacencyList(currentState))
                    {
                        if (direction == null) continue;

                        // each direction's cost is the same (1)
                        int direction_cost = cacheBlock.Cost + 1;
                        var child_cache = cache.GetItem(direction);

                        // invoke ui action, only for visualizer (confirming previously searched state)
                        UIAction?.Invoke(
                            direction,
                            direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                                StateType.Searched : StateType.NoChange
                        );

                        // ignore visited child with better cost than current cost
                        if (child_cache != null && direction_cost >= child_cache.Cost)
                            continue;

                        // invoke ui action, only for visualizer (branching for the next state)
                        UIAction?.Invoke(
                            direction,
                            direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                                StateType.Planning : StateType.NoChange
                        );

                        // repeated states prevention
                        if (fringe.Contains(direction))
                            fringe.Remove(direction);

                        // basic information injection
                        fringe.Add(direction);
                        cache.Add(direction, direction_cost, currentState);
                    }

                    // remove current state from pending states list, preventing repeated checks
                    fringe.Remove(currentState);
                    --i;
                }

                fLimit = fMin;
            }

            // number of info stored in cache is the number of nodes in the search tree!
            NumberOfNodes = cache.Count;

            if (found)
            {
                var currentItem = goal;
                Block<State> currentBlock;
                while((currentBlock = cache.GetItem(currentItem)).Parent != null)
                {
                    Parents[GetIndex(currentItem)] = currentBlock.Parent;
                    currentItem = currentBlock.Parent;
                }
                return true;
            }
            return false;
        }
    }
}
