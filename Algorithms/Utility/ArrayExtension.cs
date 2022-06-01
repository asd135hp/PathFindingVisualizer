using System.Linq;
using System.Collections.Generic;

namespace Algorithms.Utility
{
    public static class ArrayExtension
    {
        /// <summary>
        /// Fill whole array with the same value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }

        /// <summary>
        /// Pop the last item in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Pop<T>(this List<T> list)
        {
            var lastValue = list.Last();
            list.RemoveAt(list.Count - 1);
            return lastValue;
        }

        /// <summary>
        /// Remove an element from the list at certain index and return that element.
        /// If out of bounds, null or default is expected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns>
        /// Default value(mostly null) on failure.
        /// Removed element is expected on success
        /// </returns>
        public static T Remove<T>(this List<T> list, int index)
        {
            if (index < 0 || index > list.Count - 1) return default;

            T value = list[index];
            list.RemoveAt(index);
            return value;
        }
    }
}
