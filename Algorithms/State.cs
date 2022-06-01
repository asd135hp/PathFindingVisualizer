
namespace Algorithms
{
    public enum StateType
    {
        Start,
        Wall,
        Goal,
        Normal,
        Optimal,
        Searched,
        Planning,
        Next,
        NoChange
    }

    public class State
    {
        public State(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
        public StateType Type;

        /// <summary>
        /// Overriding implementation to replace comparing hash codes (default behaviour)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is State otherState && otherState.X == X && otherState.Y == Y;

        /// <summary>
        /// Obligatory implementation
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Transform State into (x,y) instead of Algorithm.State
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"({X}, {Y})";
    }
}
