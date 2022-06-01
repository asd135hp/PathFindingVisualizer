using System.IO;
using System.Collections.Generic;
using Algorithms;
using Assignment1.Drawing;

namespace Assignment1.IO
{
    public class Input : Algorithms.IO.Input
    {
        /// <summary>
        /// Guarantee: This field will always be initialized alongside with Input object
        /// </summary>
        public List<Square> DrawingSquares { get; private set; }

        /// <summary>
        /// Guarantee: This field will always be initialized alongside with Input object
        /// </summary>
        public int WindowMaxWidth { get; private set; }

        /// <summary>
        /// Guarantee: This field will always be initialized alongside with Input object
        /// </summary>
        public int WindowMaxHeight { get; private set; }

        public Input(string filename) : base()
        {
            DrawingSquares = new List<Square>();

            if (File.Exists(filename)) ParseSquares(File.ReadAllLines(filename));
            else throw new IOException("Could not read the file in the same directory as this .exe");
        }

        /// <summary>
        /// Parse squares from given input strings in order for GUI to work properly
        /// </summary>
        /// <param name="inputLines"></param>
        /// <exception cref="System.FormatException">For when inputs are faulty</exception>
        public override void ParseSquares(string[] inputLines)
        {
            base.ParseSquares(inputLines);

            // set max window size for trimming unnecessary spaces
            // adding 2 * rows and 2 * columns pixels on both sides of the screen is a bit excessive
            // but I will prioritize the fact that it works and is viewable
            WindowMaxHeight = Rows * (Square.SQUARE_SIZE + 2);
            WindowMaxWidth = Columns * (Square.SQUARE_SIZE + 2);

            // initialize DrawingSquares
            for(int row = 0; row < Rows; row++)
                for(int column = 0; column < Columns; column++)
                {
                    State currentVertex = Graph.Vertices[row][column];
                    DrawingSquares.Add(
                        new Square(
                            currentVertex.X,
                            currentVertex.Y,
                            SquareType.From(currentVertex.Type)
                        )
                    );
                }
        }
    }
}
