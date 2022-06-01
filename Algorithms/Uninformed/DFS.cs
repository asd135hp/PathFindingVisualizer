namespace Algorithms.Uninformed
{
    public class DFS : Types.UninformedAlgorithm
    {
        public DFS(int mapRows, int mapCols) : base(mapRows, mapCols)
        {
        }

        public override string Name => "Depth First Search";

        protected override bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            System.Action<State, StateType> UIAction = null
        )
        {
            if (source.Equals(goal)) return true;

            // mark actual source state as visited
            int sourceIndex = GetIndex(source);
            if (Levels[sourceIndex] == int.MaxValue)
            {
                Levels[sourceIndex] = 0;
                NumberOfNodes++;
            }

            // invoke ui action, which is necessary for visualizer
            // (confirm the next state to be searched)
            UIAction?.Invoke(
                source,
                source.Type != StateType.Start && source.Type != StateType.Goal ?
                    StateType.Next : StateType.NoChange
            );

            foreach (var direction in graph.GetAdjacencyList(source))
            {
                if (direction == null) continue;

                // invoke ui action, only for visualizer (confirming previously searched state)
                UIAction?.Invoke(
                    direction,
                    direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                        StateType.Searched : StateType.NoChange
                );

                int childIndex = GetIndex(direction);
                // pass on visited nodes or null nodes
                if (Levels[childIndex] != int.MaxValue) continue;

                // invoke ui action, only for visualizer (branching for the next state)
                UIAction?.Invoke(
                    direction,
                    direction.Type != StateType.Start && direction.Type != StateType.Goal ?
                        StateType.Planning : StateType.NoChange
                );

                Parents[childIndex] = source;
                Levels[childIndex] = Levels[sourceIndex] + 1;
                NumberOfNodes++;

                if (SingleGoalStateSearch(graph, direction, goal, UIAction))
                    return true;
            }

            return false;
        }
    }
}
