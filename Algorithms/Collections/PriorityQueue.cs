namespace Algorithms.Collections
{
    /// <summary>
    /// The name is super ambiguous already because PriorityQueue and PriorityList basically do the same thing
    /// but Queue has a limited amount of methods to call from than List (only Enqueue and Dequeue)
    /// This priority queue combines all possible methods for manipulation (except RemoveAt)
    /// Reference: https://en.wikipedia.org/wiki/Priority_queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        private readonly LinkedList<T> Items = new LinkedList<T>();

        public PriorityQueue()
        {

        }

        public bool IsEmpty => Items.IsEmpty;

        /// <summary>
        /// Insert the item based on their cost value
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="item"></param>
        public void Insert(int totalCost, T item) => Items.Insert(totalCost, item);

        /// <summary>
        /// Removes the last item in the list, which is the highest cost item
        /// </summary>
        /// <returns>
        /// The last item if the list contains more than 0 elements,
        /// else T's default will be returned instead
        /// </returns>
        public T Pop() => Items.RemoveLast();

        /// <summary>
        /// Queue's verb to push item into the list but does the same thing as Insert
        /// </summary>
        /// <param name="totalCost"></param>
        /// <param name="item"></param>
        public void Enqueue(int totalCost, T item) => Insert(totalCost, item);

        /// <summary>
        /// Acts the same way as Queue.Dequeue()
        /// </summary>
        /// <returns>The first item in the queue, can return null if there isn't any</returns>
        public T Dequeue() => Items.RemoveFirst();
    }
}
