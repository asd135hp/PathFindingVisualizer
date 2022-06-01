using System.Drawing;
using Algorithms;

namespace Assignment1.Drawing
{
    /// <summary>
    /// A little bit misleading name but essentially, it contains the color for each type of squares
    /// displayed on the screen
    /// </summary>
    public struct SquareType
    {
        public static Color Wall        => Color.Gray;
        public static Color Goal        => Color.Green;
        public static Color Start       => Color.Red;
        public static Color Normal      => Color.White;
        public static Color Optimal     => Color.Cyan;
        public static Color Searched    => Color.Blue;
        public static Color Planning    => Color.Yellow;
        public static Color Next        => Color.Orange;
        
        /// <summary>
        /// Conversion from StateType to SquareType's color
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Color From(StateType type)
        {
            return type switch
            {
                StateType.Wall      => Wall,
                StateType.Goal      => Goal,
                StateType.Start     => Start,
                StateType.Normal    => Normal,
                StateType.Optimal   => Optimal,
                StateType.Searched  => Searched,
                StateType.Next      => Next,
                StateType.Planning  => Planning,
                _                   => Color.Transparent,
            };
        }
    }
}
