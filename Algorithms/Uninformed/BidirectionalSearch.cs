using System.Collections.Generic;

namespace Algorithms.Uninformed
{
    /// <summary>
    /// Coding reference: https://www.geeksforgeeks.org/bidirectional-search/
    /// </summary>
    public class BidirectionalSearch : Types.UninformedAlgorithm
    {
        public BidirectionalSearch(int mapRows, int mapCols) : base(mapRows, mapCols)
        {
            // releases unwanted memory
            Levels = null;
        }

        public override string Name => "Bidirectional Search";

        /// <summary>
        /// Modularization for initializing queues for BFS
        /// </summary>
        /// <param name="initialState">Initial state to be initialized from</param>
        /// <returns></returns>
        private Queue<State> GetNewQueue(State initialState)
        {
            var queue = new Queue<State>();
            queue.Enqueue(initialState);
            return queue;
        }

        /// <summary>
        /// Find intersection in visited nodes on both directions 
        /// </summary>
        /// <param name="sourceVisited"></param>
        /// <param name="goalVisited"></param>
        /// <returns>An index on intersection between arrays, -1 on no intersection</returns>
        private int GetIntersectIndex(bool[] sourceVisited, bool[] goalVisited)
        {
            for(int i = 0; i < sourceVisited.Length; ++i)
                if (sourceVisited[i] && goalVisited[i])
                    return i;
            return -1;
        }

        /// <summary>
        /// The main body of BFS where it dequeue the first state and queue in unvisited states from itself
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="queue"></param>
        /// <param name="UIAction"></param>
        /// <param name="fromGoal">
        /// Indicates that this search is called from goal queue if param is set to true
        /// </param>
        /// <returns></returns>
        private void BFS(
            Graph graph,
            Queue<State> queue,
            bool[] visited,
            State[] parents,
            System.Action<State, StateType> UIAction = null
        )
        {
            var currentState = queue.Dequeue();
            int currentIndex = GetIndex(currentState);

            // prevent overlapping already visited nodes
            if (visited[currentIndex]) return;
            visited[currentIndex] = true;

            // invoke ui action, which is necessary for visualizer
            // (confirm the next state to be searched)
            UIAction?.Invoke(
                currentState,
                currentState.Type != StateType.Start && currentState.Type != StateType.Goal ?
                    StateType.Next : StateType.NoChange
            );

            // enqueue the whole adjacency list (pass on nulls)
            // as well as marking currentState as a parent to each of adjacent states (if any)
            // as a bactracker
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
                if (visited[index]) continue;

                // invoke ui action, only for visualizer (branching for the next state)
                UIAction?.Invoke(
                    direction,
                    direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                        StateType.Planning : StateType.NoChange
                );

                parents[index] = currentState;
                queue.Enqueue(direction);
            }
        }

        protected override bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            System.Action<State, StateType> UIAction = null
        )
        {
            int maxSize = Width * Height;
            bool[] sourceVisited = new bool[maxSize],
                goalVisited = new bool[maxSize];
            State[] sourceParents = new State[maxSize],
                goalParents = new State[maxSize];
            Queue<State> sourceQueue = GetNewQueue(source),
                goalQueue = GetNewQueue(goal);

            // search until there are no states left to visit for both sides
            while (sourceQueue.Count != 0 && goalQueue.Count != 0)
            {
                BFS(graph, sourceQueue, sourceVisited, sourceParents, UIAction);
                BFS(graph, goalQueue, goalVisited, goalParents, UIAction);

                // check if two sub-graphs are intersected at any points
                int intersectionIndex = GetIntersectIndex(sourceVisited, goalVisited);
                if(intersectionIndex != -1)
                {
                    // connect the path from both directions
                    State parentState = goalParents[intersectionIndex];
                    int lastIndex = intersectionIndex;
                    while(parentState != null)
                    {
                        int parentIndex = GetIndex(parentState),
                            lastX = lastIndex % Width,
                            lastY = (lastIndex - lastX) / Width;

                        // if goalParents[parentIndex] is null, parentIndex is the index of goal state
                        // therefore, these assignments are valid
                        sourceParents[parentIndex] = graph.Vertices[lastY][lastX];
                        parentState = goalParents[parentIndex];
                        lastIndex = parentIndex;
                    }
                    Parents = sourceParents;

                    for(int i = 0; i < maxSize; ++i)
                        if (goalVisited[i] || sourceVisited[i]) NumberOfNodes++;

                    return true;
                }
            }

            return false;
        }
    }
}