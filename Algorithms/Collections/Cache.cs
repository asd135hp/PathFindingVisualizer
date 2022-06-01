using System.Collections.Generic;

namespace Algorithms.Collections
{
    public class Block<T>
    {
        public int Cost;
        public T Parent;
        public Block(int cost = 0, T parent = default)
        {
            Cost = cost;
            Parent = parent;
        }
    }

    public class Cache<T>
    {
        private readonly Dictionary<T, Block<T>> Storage = new Dictionary<T, Block<T>>();

        public Cache(T initialItem)
        {
            Storage[initialItem] = new Block<T>();
        }

        /// <summary>
        /// All keys are unique so it is reasonable to return number of keys in storage
        /// </summary>
        public int Count => Storage.Keys.Count;

        public void Add(T item, int cost, T parent) => Storage[item] = new Block<T>(cost, parent);

        public Block<T> GetItem(T item) => Storage.ContainsKey(item) ? Storage[item] : default;

        public void Clear() => Storage.Clear();
    }
}
