using Algorithms.Utility;
using System.Collections.Generic;

namespace Algorithms.Types
{
    public enum AlgorithmType
    {
        Informed,
        Uninformed
    }

    public abstract class SearchingAlgorithm
    {
        public SearchingAlgorithm(int mapRows, int mapCols)
        {
            Width = mapCols;
            Height = mapRows;

            int maxSize = Width * Height;
            Levels = new int[maxSize];
            Parents = new State[maxSize];
            NumberOfNodes = 0;

            Levels.Fill(int.MaxValue);
        }

        protected int Width { get; }
        protected int Height { get; }

        /// <summary>
        /// number_of_nodes requirement
        /// </summary>
        public long NumberOfNodes { get; protected set; }

        /// <summary>
        /// Output from running searching algorithm
        /// </summary>
        public int[] Levels { get; protected set; }

        /// <summary>
        /// Output from running searching algorithm
        /// </summary>
        public State[] Parents { get; protected set; }

        public abstract AlgorithmType Type { get; }
        public abstract string Name { get; }

        /// <summary>
        /// Get index in any one-dimensional arrays
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected int GetIndex(State state) => state.Y * Width + state.X;

        /// <summary>
        /// Perform graph search on a single goal state
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="source"></param>
        /// <param name="goals"></param>
        /// <param name="UIAction"></param>
        /// <returns>
        /// Must inherit this method to search,
        /// else the algorithm will always fail (returning false)
        /// </returns>
        protected virtual bool SingleGoalStateSearch(
            Graph graph,
            State source,
            State goal,
            System.Action<State, StateType> UIAction = null
        ) => false;

        /// <summary>
        /// Performing a graph search through searching algorithm
        /// (TODO: Rechecking the constrains of the assignment!!!!!!!!!!!!!!!!!!!!!!
        /// Would the closest goal be blocked and further goal is the answer instead?)
        /// </summary>
        /// <param name="graph">Overall graph object</param>
        /// <param name="source">Source state</param>
        /// <param name="goals">Goal states</param>
        /// <returns>
        /// True if there is a path to goal state,
        /// false when no path found or depth limit reached
        /// </returns>
        public bool Search(
            Graph graph,
            State source,
            State[] goals,
            System.Action<State, StateType> UIAction = null
        )
        {
            if (graph == null || source == null || goals == null) return false;

            // no need sorting here because it has already been sorted
            // (or not sorted if somehow the goals are manually declared
            // but it does not affect this method much)
            foreach(var goal in goals)
            {
                if (SingleGoalStateSearch(graph, source, goal, UIAction))
                    return true;

                // reset after each search until a solution is found
                Levels?.Fill(int.MaxValue);
                if (Parents != null)
                    Parents = new State[Width * Height];
            }

            return false;
        }

        /// <summary>
        /// Get the path in the graph by doing search algorithm
        /// </summary>
        /// <param name="goalState"></param>
        /// <returns></returns>
        public List<State> GetPath(State goalState)
        {
            var path = new List<State>();
            var parentState = Parents.GetValue(GetIndex(goalState)) as State;

            // from goal state:
            // while parent state is not null
            // (null => it is the starting state because that state does not have a parent),
            // add the state to the path
            while (parentState != null)
            {
                path.Add(parentState);
                parentState = Parents[GetIndex(parentState)];
            }

            if (path.Count == 0) return null;

            // reverse the order of the path to correctly display of the path
            path.Reverse();

            // remove the goal state from the list of path
            path.RemoveAt(0);

            return path;
        }
    }
}
