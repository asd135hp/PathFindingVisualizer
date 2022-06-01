using System;

namespace Algorithms.Utility
{
    internal static class Heuristic
    {
        /// <summary>
        /// Mahattan distance, which is more general than Pythagorean distance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static int ManhattanDistance<T>(T source, T goal) where T : State
            => Math.Abs(goal.X - source.X) + Math.Abs(goal.Y - source.Y);
    }
}
