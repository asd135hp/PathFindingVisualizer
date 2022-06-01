using System.Collections.Generic;

namespace Algorithms.Uninformed
{
    public class BFS : Types.UninformedAlgorithm
    {
        public BFS(int mapRows, int mapCols) : base(mapRows, mapCols)
        {
        }

        public override string Name => "Breadth First Search";

        protected override bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            System.Action<State, StateType> UIAction = null
        )
        {
            Levels[GetIndex(source)] = 0;
            NumberOfNodes++;

            // BFS could not use recursion
            var queue = new Queue<State>();
            queue.Enqueue(source);

            // search until there are no states left to visit
            while (queue.Count != 0)
            {
                var currentState = queue.Dequeue();
                if (currentState.Equals(goal)) return true;

                // invoke ui action, which is necessary for visualizer
                // (confirm the next state to be searched)
                UIAction?.Invoke(
                    currentState,
                    currentState.Type != StateType.Start && currentState.Type != StateType.Goal ?
                        StateType.Next : StateType.NoChange
                );

                // enqueue the whole adjacency list (pass on nulls)
                // as well as marking currentState as a parent to each of adjacent states (if any)
                foreach (var direction in graph.GetAdjacencyList(currentState))
                {
                    if (direction == null) continue;

                    // invoke ui action, only for visualizer (confirming previously searched state)
                    UIAction?.Invoke(
                        direction,
                        direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                            StateType.Searched : StateType.NoChange
                    );

                    int index = GetIndex(direction);
                    if (Levels[index] != int.MaxValue) continue;

                    // invoke ui action, only for visualizer (branching for the next state)
                    UIAction?.Invoke(
                        direction,
                        direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                            StateType.Planning : StateType.NoChange
                    );

                    // set new level value to parent's level + 1 here to prevent overlapping states enqueue
                    Levels[index] = Levels[GetIndex(currentState)] + 1;
                    Parents[index] = currentState;
                    
                    // this node's level is not inf so it will be in the search tree
                    NumberOfNodes++;
                    queue.Enqueue(direction);
                }
            }

            return false;
        }
    }
}
