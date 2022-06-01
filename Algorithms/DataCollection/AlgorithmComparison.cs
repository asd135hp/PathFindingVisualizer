using Algorithms.IO;
using Algorithms.Types;
using Algorithms.Informed;
using Algorithms.Uninformed;
using System;

namespace Algorithms.DataCollection
{
    public static class AlgorithmComparison
    {
        /// <summary>
        /// Maximum loop count
        /// Straining computer's power (by specifying higher loop count)
        /// will affect greatly to the outcome of the comparison
        /// </summary>
        private const uint LOOP = 1;

        /// <summary>
        /// Feed an action through this to calculate a timespan for 10000x action's performance
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static TimeSpan GetElapsedTime(Action action)
        {
            var task = System.Threading.Tasks.Task.Run(() =>
            {
                var then = DateTime.Now;
                for (int i = 0; i < LOOP; i++) action.Invoke();
                return DateTime.Now - then;
            });
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Get algorithm depending on which argument
        /// </summary>
        /// <param name="which"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static SearchingAlgorithm GetAlgorithm(int which, int rows, int cols)
        {
            switch (which)
            {
                case 0: return new DFS(rows, cols);
                case 1: return new BFS(rows, cols);
                case 2: return new BidirectionalSearch(rows, cols);
                case 3: return new AStar(rows, cols);
                case 4: return new GreedyBestFirst(rows, cols);
                case 5: return new FringeSearch(rows, cols);
                default: return null;
            }
        }

        /// <summary>
        /// Get the closest path count,
        /// which might not be to the closest goal because that closest one is blocked
        /// </summary>
        /// <param name="algo"></param>
        /// <param name="input"></param>
        /// <returns>Count of the path to the closest and unblocked goal</returns>
        private static int GetClosestPathCount(SearchingAlgorithm algo, Input input)
        {
            foreach(var goalState in input.GoalStates)
            {
                var path = algo.GetPath(goalState);
                if (path != null) return path.Count;
            }

            return 0;
        }

        /// <summary>
        /// Comparison between all algorithms in one single test case
        /// TODO: Efficient algorithm contains both time and best possible path!
        /// </summary>
        /// <returns>A meaningful and output-able string about the result of the comparison</returns>
        public static string Compare(string[] researchCase = null)
        {
            // preparations
            Input input = new Input();
            input.ParseSquares(researchCase ?? Cases.TwoGoalsWithoutObs);
            string result = $"Result:\n";

            // start finding the best algorithm
            SearchingAlgorithm bestAlgo = null;
            double bestRating = -1;
            for(int i = 0; i < 6; i++)
            {
                var algo = GetAlgorithm(i, input.Rows, input.Columns);
                if (algo == null) continue;

                double time = GetElapsedTime(() =>
                     {
                         // no overlapping object searches
                         GetAlgorithm(i, input.Rows, input.Columns).Search(
                             input.Graph,
                             input.StartingState,
                             input.GoalStates.ToArray()
                         );
                     }
                ).TotalSeconds;

                // search on the main object first
                algo.Search(
                    input.Graph,
                    input.StartingState,
                    input.GoalStates.ToArray()
                );

                // map goal states into path lengths and then add them together
                int pathCount = GetClosestPathCount(algo, input);

                // push result
                result += $"- {algo.Name}: {time} seconds, ";
                result += $"resultant path contains {pathCount} directions\n";

                // get best rating
                // which is mainly path count that matters,
                // then amount of time it used second
                double rating = time + pathCount;
                if (bestRating == -1)
                {
                    bestRating = rating;
                    bestAlgo = algo;
                    continue;
                }

                bestRating = Math.Min(bestRating, rating);
                if (bestRating == rating) bestAlgo = algo;
            }

            return result +
                $"Therefore, {bestAlgo.Name} is the most efficient algorithm.\n" +
                $"Its rating from running {LOOP} loops of the same test case is: {bestRating}";
        }
    }
}
