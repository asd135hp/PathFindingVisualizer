using System.Collections.Generic;
using Algorithms.Utility;

namespace Algorithms.IO
{
    public class Input
    {
        /// <summary>
        /// Overall graph for algorithms
        /// </summary>
        public Graph Graph { get; private set; }

        // map's rows and columns
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Input() { }

        /// <summary>
        /// Gate-keeping property
        /// </summary>
        private bool IsParsed = false;

        /// <summary>
        /// WARNING: Must initialize graph through ParseSquares method first.
        /// Returns 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Flattened index if called ParseSquares, else -1</returns>
        public int Get1DIndex(int x, int y) => IsParsed ? y * Columns + x : -1;

        /// <summary>
        /// WARNING: Must initialize graph through ParseSquares method first.
        /// Nullable if there is no starting state inside the graph or not calling ParseSquares first
        /// </summary>
        public State StartingState
        {
            get
            {
                if (IsParsed)
                    foreach (List<State> vertexRow in Graph.Vertices)
                        foreach (State vertex in vertexRow)
                            if (vertex.Type == StateType.Start) return vertex;

                return null;
            }
        }

        /// <summary>
        /// WARNING: Must initialize graph through ParseSquares method first.
        /// A list of goal states if called ParseSquares first, else an empty list is returned instead!
        /// </summary>
        public List<State> GoalStates
        {
            get
            {
                var result = new List<State>();

                if (IsParsed)
                {
                    foreach (List<State> vertexRow in Graph.Vertices)
                        foreach (State vertex in vertexRow)
                            if (vertex.Type == StateType.Goal) result.Add(vertex);

                    // pre-sort the goal before reading
                    result.Sort(delegate(State s1, State s2){
                        int dist1 = Heuristic.ManhattanDistance(StartingState, s1),
                            dist2 = Heuristic.ManhattanDistance(StartingState, s2);

                        if (dist1 < dist2) return -1;
                        if (dist1 > dist2) return 1;
                        return 0;
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// With a line that describes a wall,
        /// parse it and add the state to wall collection
        /// </summary>
        /// <param name="line">An input string read from given text file</param>
        private void ParseWall(string line)
        {
            string[] rawNumbers = line.Trim('(', ')').Split(',');
            int x = int.Parse(rawNumbers[0].Trim()),
                y = int.Parse(rawNumbers[1].Trim()),
                width = int.Parse(rawNumbers[2].Trim()),
                height = int.Parse(rawNumbers[3].Trim());

            // description of the walls
            for (int offX = 0; offX < width; offX++)
                for (int offY = 0; offY < height; offY++)
                    Graph.ChangeStateTypeAt(
                        x + offX,
                        y + offY,
                        StateType.Wall
                    );
        }

        /// <summary>
        /// Parse squares from given input strings for storing data
        /// </summary>
        /// <param name="inputLines"></param>
        /// <exception cref="System.FormatException">For when inputs are faulty</exception>
        public virtual void ParseSquares(string[] inputLines)
        {
            IsParsed = true;

            // first line defines height and width
            string[] firstLine = inputLines[0].Trim().Trim('[', ']').Split(',');
            Rows = int.Parse(firstLine[0].Trim());
            Columns = int.Parse(firstLine[1].Trim());
            Graph = new Graph(Rows, Columns);

            // second line
            // parsing for starting state
            string[] secondLine = inputLines[1].Trim().Trim('(', ')').Split(',');
            Graph.ChangeStateTypeAt(
                int.Parse(secondLine[0].Trim()),
                int.Parse(secondLine[1].Trim()),
                StateType.Start
            );

            // third line
            // parsing for all goal states
            foreach (string rawGoalState in inputLines[2].Split('|'))
            {
                // do not let any leftover bytes onto int.Parse method!
                string[] rawNumbers = rawGoalState.Trim().Trim('(', ')').Trim().Split(',');
                Graph.ChangeStateTypeAt(
                    int.Parse(rawNumbers[0].Trim()),
                    int.Parse(rawNumbers[1].Trim()),
                    StateType.Goal
                );
            }

            // fourth line+?
            // parsing for all wall states
            if (inputLines.Length > 3)
                for (int i = 3; i < inputLines.Length; i++)
                    ParseWall(inputLines[i].Trim());
        }
    }
}
