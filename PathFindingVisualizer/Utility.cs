using Algorithms;
using Algorithms.Types;
using Algorithms.Informed;
using Algorithms.Uninformed;
using Assignment1.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Assignment1
{
    internal static class Utility
    {
        /// <summary>
        /// Milliseconds between tasks
        /// </summary>
        private const int MILLISECONDS = 25;

        /// <summary>
        /// Random color, which is useful for multiple goal states (not in use)
        /// </summary>
        public static System.Drawing.Color RandomColor
        {
            get
            {
                var random = new System.Random();
                var color = System.Drawing.Color.Transparent;

                // make the color so that it is not the same as all other possible colors,
                // which are pre-defined in SquareType struct
                //
                // even though the chance for a random color to be overlapped
                // with pre-defined color is quite slim,
                // I would rather to not risk it
                while (color == System.Drawing.Color.Transparent
                || color == SquareType.Goal
                || color == SquareType.Normal
                || color == SquareType.Optimal
                || color == SquareType.Searched
                || color == SquareType.Start
                || color == SquareType.Wall)
                {
                    color = System.Drawing.Color.FromArgb(
                        random.Next(0, 255),
                        random.Next(0, 255),
                        random.Next(0, 255)
                    );
                }

                return color;
            }
        }

        /// <summary>
        /// Finish the tasks in task list with some milliseconds (1000) between each task
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="millisecBetweenTasks"></param>
        public static void FinishTasks(List<Task> tasks, int millisecBetweenTasks = MILLISECONDS)
            => Task.Run(() =>
            {
                foreach (var task in tasks)
                {
                    if (task.Status != TaskStatus.Created) break;
                    task.Start();
                    Thread.Sleep(millisecBetweenTasks);
                }
            }).Wait();


        /// <summary>
        /// Parse Parents array from SearchingAlgorithm for final path.
        /// Then, using Task to correctly display the path obtained from Parents array.
        /// (Thread.Sleep on main thread will cause the whole application freeze!)
        /// </summary>
        /// <param name="drawingSquares"></param>
        /// <param name="startingState"></param>
        /// <param name="goalStates"></param>
        /// <param name="width">
        /// Is compulsory because Parents, DrawingSquares arrays are 1-dimensional array,
        /// flattened from supposedly 2-dimensional array
        /// </param>
        /// <param name="getPath">A function that get the resultant path</param>
        public static void ShowPaths(
            List<Square> drawingSquares,
            State startingState,
            List<State> goalStates,
            int width,
            System.Func<State, List<State>> getPath
        )
        {
            var taskList = new List<Task<bool>>();

            foreach (var goalState in goalStates)
            {
                var path = getPath(goalState);
                
                // ignore any non-existent paths
                if (path == null) continue;

                // add new async task running concurrently with main thread
                // displaying the path on the screen
                Task.Run(() =>
                {
                    // color the path from the algorithm
                    // var color = goalStates.Count > 1 ? RandomColor : SquareType.Optimal;
                    foreach (var state in path)
                    {
                        if (state.Type == StateType.Goal) continue;
                        drawingSquares[state.Y * width + state.X].ChangeSquareColor(SquareType.Optimal);

                        // pause current thread a number of millisecs for a better visualization
                        Thread.Sleep(MILLISECONDS);
                    }

                    // async code, only continue if user clicks "Ok" of the message box
                    return MessageBox.Show(
                        "Finished displaying path from " +
                        $"({startingState.X}, {startingState.Y}) to " +
                        $"({goalState.X}, {goalState.Y})\n" +
                        "(Press \"Ok\" to continue)"
                    ) == DialogResult.OK;
                });

                // only the first reachable goal will be displayed
                return;
            }

            // there is not a path
            MessageBox.Show("There is no path between source state to any of goal states");
        }

        /// <summary>
        /// Correctly return specified searching algorithm
        /// </summary>
        /// <param name="searchMethod">
        /// Search method, which is the first argument to be passed through this program
        /// </param>
        /// <param name="rows">Number of rows of the map</param>
        /// <param name="cols">Number of columns in the map</param>
        /// <returns>
        /// Null if the algorithm is not supported; otherwise,
        /// the method will return an SearchingAlgorithm object
        /// </returns>
        public static SearchingAlgorithm GetAlgorithm(string searchMethod, int rows, int cols)
        {
            switch (searchMethod.ToLower())
            {
                case "bfs":
                    return new BFS(rows, cols);
                case "dfs":
                    return new DFS(rows, cols);
                case "bds":
                    return new BidirectionalSearch(rows, cols);
                case "gbfs":
                    return new GreedyBestFirst(rows, cols);
                case "a*":
                    return new AStar(rows, cols);
                case "fringe":
                case "fringe-search":
                    return new FringeSearch(rows, cols);
                default:
                    MessageBox.Show("Unsupported type of search method! Terminate the program...");
                    return null;
            }
        }
    }
}
