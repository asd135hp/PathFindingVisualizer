using System;
using Algorithms.Collections;

namespace Algorithms.Types
{
    public abstract class BestFirstSearchAlgorithm: InformedAlgorithm
    {
        public BestFirstSearchAlgorithm(int mapRows, int mapCols) : base(mapRows, mapCols) { }

        public override string Name => "Best First Search Algorithm";

        /// <summary>
        /// An abstract method to return a function that, in turn, return f(N) upon invocation,
        /// depending on implementation
        /// </summary>
        /// <param name="goal">Stored goal to be used for future calculation</param>
        /// <returns>A function that returns f(N) upon invocation</returns>
        protected abstract Func<State, int> GetTotalCost(State goal);

        protected override bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            Action<State, StateType> UIAction = null
        )
        {
            // setting up necessary components
            var pendingStates = new PriorityQueue<State>();
            var totalCost = GetTotalCost(goal);

            State currentState = source;
            int sourceIndex = GetIndex(source);
            if (Levels[sourceIndex] == int.MaxValue)
            {
                Levels[sourceIndex] = 0;
                NumberOfNodes++;
            }

            // the same as checking an empty list
            while (currentState != null)
            {
                if (currentState.Equals(goal)) return true;

                // invoke ui action, which is necessary for visualizer
                // (confirm the next state to be searched)
                UIAction?.Invoke(
                    currentState,
                    currentState.Type != StateType.Start && currentState.Type != StateType.Goal ?
                        StateType.Next : StateType.NoChange
                );

                foreach (var direction in graph.GetAdjacencyList(currentState))
                {
                    if (direction == null) continue;

                    // invoke ui action, only for visualizer (confirming previously searched state)
                    UIAction?.Invoke(
                        direction,
                        direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                            StateType.Searched : StateType.NoChange
                    );

                    // set visited to current state
                    int directionIndex = GetIndex(direction);
                    if (Levels[directionIndex] != int.MaxValue) continue;

                    // invoke ui action, only for visualizer (branching for the next state)
                    UIAction?.Invoke(
                        direction,
                        direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                            StateType.Planning : StateType.NoChange
                    );

                    // preventing overlapping pushes of states due to only performing below expressions
                    // AFTER adding adjacent states, for which this method will not be updated correctly
                    // for pushes:
                    // Levels[directionIndex] != int.MaxValue would return false instead of true
                    // while it must be true for visited nodes
                    Parents[directionIndex] = currentState;
                    Levels[directionIndex] = Levels[GetIndex(currentState)] + 1;
                    NumberOfNodes++;

                    int currentTotalCost = totalCost.Invoke(direction);
                    pendingStates.Insert(currentTotalCost, direction);
                }

                currentState = pendingStates.Dequeue();
            }

            return false;
        }
    }
}
