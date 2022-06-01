using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using Algorithms;
using Algorithms.Types;
using Algorithms.DataCollection;
using Assignment1.IO;
using Assignment1.Drawing;

namespace Assignment1
{
    public partial class Form1 : Form
    {
        private void Visualizer(SearchingAlgorithm algo, Input input)
        {
            var taskList = new List<Task>();
            void commonAction(State currentVertex, StateType type)
            {
                // add to task list for later display
                taskList.Add(new Task(() =>
                {
                    if(type != StateType.NoChange)
                    {
                        // change color of current vertex's position on drawing squares
                        int index = input.Get1DIndex(currentVertex.X, currentVertex.Y);
                        input.DrawingSquares[index]
                            .ChangeSquareColor(SquareType.From(type));
                    }
                }));
            };

            bool found = algo.Search(
                input.Graph,
                input.StartingState,
                input.GoalStates.ToArray(),
                commonAction
            );

            // run on separate thread so that the main thread does not pause
            Task.Run(() =>
            {
                // show the discoveries from searching algorithm
                Utility.FinishTasks(taskList);

                var result = MessageBox.Show(
                    "Finished displaying searched states from the algorithm!\n" +
                    "(Press \"Ok\" to continue)"
                );

                if (result == DialogResult.OK)
                {
                    if (found)
                    {
                        Utility.ShowPaths(
                            input.DrawingSquares,
                            input.StartingState,
                            input.GoalStates,
                            input.Columns,
                            algo.GetPath
                        );
                        return;
                    }
                    
                    MessageBox.Show("No solution found");
                }
            });
        }

        private void CommandLine(SearchingAlgorithm algo, Input input, string fileName, string method)
        {
            bool found = algo.Search(
                input.Graph,
                input.StartingState,
                input.GoalStates.ToArray()
            );

            // print output directly on GUI instead of CLI
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            // put double quotes on names that have more than 1 word
            string methodName = algo.Name.Split(' ').Length > 1 ? $"\"{algo.Name}\"" : algo.Name;

            var textBox = Controls.Find("output", true)[0];
            textBox.Text += $"{currentDir}> search {fileName} {method}";
            textBox.Text += $"{Environment.NewLine}{currentDir}> ";
            textBox.Text += $"{fileName} {methodName} {algo.NumberOfNodes}{Environment.NewLine}";

            if (found)
            {
                var startState = input.StartingState;
                foreach (var goalState in input.GoalStates)
                {
                    // because the path is trimmed off on both ends for UI display,
                    // so adding those states again is a must
                    var path = algo.GetPath(goalState);
                    if (path == null) continue;

                    path.Insert(0, startState);
                    path.Add(goalState);
                    textBox.Text += $"{PathTranslation.Translate(path.ToArray()).TrimEnd()}";

                    // output only one path, the rest is irrelavant
                    break;
                }
            }
            else textBox.Text += "No solution found";
        }

        private void Comparison(string whichCase)
        {
            string[] compCase = whichCase != null && whichCase.Length != 0 ?
                Cases.GetCase(
                    byte.Parse(whichCase[0].ToString()),
                    whichCase.Length == 2
                ) : null;

            string preText = compCase == null ? "" :
                $"Search case: {whichCase[0]} goal(s) with" +
                (whichCase.Length == 2 ? "" : "out") + " obstacles\n";
            
            var label = Controls.Find("output", true)[0];
            label.Text = preText + AlgorithmComparison.Compare(compCase);
            Text = "Comparison between algorithms";
        }

        public Form1(string inputFile, string searchMethod, string thirdArg)
        {
            try
            {
                bool isVisualizer = thirdArg != null;
                Input input = new Input(inputFile);
                InitializeComponent();

                var titleBarHeight = RectangleToScreen(ClientRectangle).Top - Top;
                if (isVisualizer && searchMethod != "comparison")
                {
                    Width = input.WindowMaxWidth;
                    Height = titleBarHeight + input.WindowMaxHeight;
                    Controls.AddRange(input.DrawingSquares.ToArray());
                }
                else
                {
                    Controls.Add(new TextBox()
                    {
                        Name = "output",
                        Width = Width - 20,
                        Height = Height - titleBarHeight,
                        Font = new System.Drawing.Font("Arial", 14),
                        BorderStyle = 0,
                        BackColor = BackColor,
                        Multiline = true,
                        ReadOnly = true,
                        TabStop = false
                    });
                }

                // enable comparison mode, where it compare the time it takes between algorithms
                if (searchMethod == "comparison")
                {
                    Text = "Comparison between algorithms (please wait...)";
                    Show();
                    Comparison(thirdArg);
                    return;
                }

                // initialize algorithm of choice
                SearchingAlgorithm algo = Utility.GetAlgorithm(searchMethod, input.Rows, input.Columns);
                if (algo == null)
                {
                    Thread.Sleep(1000);
                    Close();
                }

                // prefix caption of the window through setting Text property
                Text = (isVisualizer ? "(Visualiser) " : "(Command Line) ") + algo.Name;

                if (isVisualizer) Visualizer(algo, input);
                else CommandLine(algo, input, inputFile, searchMethod);
            }
            catch (Exception e)
            {
                // should not show but is just a preventative measure
                MessageBox.Show(e.ToString());
                Close();
            }
        }
    }
}
