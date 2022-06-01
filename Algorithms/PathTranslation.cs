
namespace Algorithms
{
    public static class PathTranslation
    {
        private static string GetDirection(State current, State next)
        {
            int cx = current.X, cy = current.Y,
                nx = next.X, ny = next.Y;

            if (cx == nx && cy == ny)
                throw new System.ArgumentException("A path should not contain repetion of a same state");

            if (cx + 1 == nx) return "right; ";
            if (cx - 1 == nx) return "left; ";
            if (cy + 1 == ny) return "down; ";
            if (cy - 1 == ny) return "up; ";

            return "";
        }

        /// <summary>
        /// With a certain path (array of states), translate it into a string of direction.
        /// Eg: right; left; up; down; ...
        /// </summary>
        /// <param name="path">A path, comes from output of the algorithm</param>
        /// <returns>A direction string</returns>
        public static string Translate(State[] path)
        {
            string result = "";
            for (int i = 0; i < path.Length; i++)
            {
                if (i + 1 == path.Length) break;
                result += GetDirection(path[i], path[i + 1]);
            }
            return result;
        }
    }
}
