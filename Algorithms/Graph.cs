using System.Collections.Generic;

namespace Algorithms
{
    public class Graph
    {
        public readonly List<List<State>> Vertices;

        public Graph(int rows, int cols)
        {
            Vertices = new List<List<State>>();

            // empty initialization for vertices list of the graph
            // manual vertex type change through calling a method is compulsory
            for (int y = 0; y < rows; y++)
            {
                var tempList = new List<State>();
                for (int x = 0; x < cols; x++)
                {
                    tempList.Add(new State(x, y)
                    {
                        Type = StateType.Normal
                    });
                }
                Vertices.Add(tempList);
            }
        }

        /// <summary>
        /// Change state type at specified x and y coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type">Normal type will be ignored</param>
        public void ChangeStateTypeAt(int x, int y, StateType type)
        {
            if (type != StateType.Normal)
                Vertices[y][x].Type = type;
        }

        /// <summary>
        /// Get an adjacency list from a given vertex.
        /// Returning array will contain these direction IN ORDER: up, left, down, right.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public State[] GetAdjacencyList(State vertex)
        {
            var result = new State[4];
            int x = vertex.X,
                y = vertex.Y,
                MAX_X = Vertices[0].Count,
                MAX_Y = Vertices.Count;

            // up
            result[0] = y - 1 >= 0 ? Vertices[y - 1][x] : null;

            // left
            result[1] = x - 1 >= 0 ? Vertices[y][x - 1] : null;

            // down
            result[2] = y + 1 < MAX_Y ? Vertices[y + 1][x] : null;

            // right
            result[3] = x + 1 < MAX_X ? Vertices[y][x + 1] : null;

            // filter out wall states
            // it is the same thing if this for loop is hard-coded on those 4 assignments above
            for (int i = 0; i < 4; i++)
            {
                if (result[i] == null) continue;
                if (result[i].Type == StateType.Wall) result[i] = null;
            }

            return result;
        }
    }
}
