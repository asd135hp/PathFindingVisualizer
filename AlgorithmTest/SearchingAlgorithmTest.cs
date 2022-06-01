using Algorithms.IO;
using Algorithms.Types;
using Algorithms.Informed;
using Algorithms.Uninformed;
using Algorithms.DataCollection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AlgorithmTest
{
    public class SearchingAlgorithmTest
    {
        readonly Input input = new Input();
        SearchingAlgorithm[] Algorithms;

        public void Setup(int rows, int cols)
        {
            Algorithms = new SearchingAlgorithm[]
            {
                new DFS(rows, cols),
                new BFS(rows, cols),
                new BidirectionalSearch(rows,cols),
                new AStar(rows, cols),
                new GreedyBestFirst(rows, cols),
                new FringeSearch(rows, cols)
            };
        }

        [Test]
        public void TestNoExceptionWhenNoSolution()
        {
            input.ParseSquares(Cases.NoSolution);
            Setup(input.Rows, input.Columns);

            Assert.DoesNotThrow(() =>
            {
                foreach (var algo in Algorithms)
                {
                    var constTime = DateTime.Now;
                    double duration = 0.0;

                    var time = Task.Run(() => { duration = (DateTime.Now - constTime).TotalSeconds; });
                    var task = Task.Run(() =>
                    {
                        algo.Search(input.Graph, input.StartingState, input.GoalStates.ToArray());
                        time.Dispose();
                    });

                    while (task.Status == TaskStatus.Running)
                    {
                        // normally, it should not take 10 seconds for a simple search
                        if (duration > 10) throw new Exception("");
                        if (task.Exception != null) throw task.Exception;
                    }
                }
            });
        }

        [Test]
        public void TestThereIsAPath()
        {
            input.ParseSquares(Cases.TwoGoalsWithObs);
            Setup(input.Rows, input.Columns);

            Assert.DoesNotThrow(() =>
            {
                int count = 0;
                foreach (var algo in Algorithms)
                {
                    var constTime = DateTime.Now;
                    double duration = 0.0;

                    var time = Task.Run(() => { duration = (DateTime.Now - constTime).TotalSeconds; });
                    var task = Task.Run(() =>
                    {
                        algo.Search(input.Graph, input.StartingState, input.GoalStates.ToArray());
                        time.Dispose();

                        foreach (var goal in input.GoalStates)
                        {
                            var path = algo.GetPath(goal);
                            if (path != null && path.Count > 0)
                            {
                                count++;
                                return;
                            }
                        }
                    });

                    while (!task.IsCompletedSuccessfully)
                    {
                        // normally, it should not take 10 seconds for a simple search
                        if (duration > 10) throw new Exception("");
                        if (task.Exception != null) throw task.Exception;
                    }
                }

                if (count != Algorithms.Length)
                    throw new Exception("");
            });
        }
    }
}