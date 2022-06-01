namespace Algorithms.Collections
{
    internal class Node<T>
    {
        public Node(int cost, T item)
        {
            Cost = cost;
            Item = item;
        }

        public readonly int Cost;
        public readonly T Item;
        public Node<T> NextNode = null;
    }

    /// <summary>
    /// A* is faster using this than inserting items into a C# built-in list.
    /// This is my best attempt in making A* faster
    /// because to make A* better than BFS in all cases then the priority queue must implement
    /// a data structure so that its performance is comparable or even better than a queue
    /// (Queue data structure is most likely a linked list with a record of both ends of the list
    /// -> both Enqueue and Dequeue are all O(1) and I can't best that performance)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class LinkedList<T>
    {
        public LinkedList()
        {

        }

        private Node<T> FirstNode = null;

        public bool IsEmpty => FirstNode == null;

        /// <summary>
        /// Insertion for doubly linked list
        /// if assignments of objects' address are negligible,
        /// the worst time complexity will be O(3n + 2) ~ O(3n) for comparisons only (debatable)
        /// </summary>
        /// <param name="newCost"></param>
        /// <param name="newItem"></param>
        public void Insert(int newCost, T newItem)
        {
            var newNode = new Node<T>(newCost, newItem);

            if (FirstNode == null)
            {
                FirstNode = newNode;
                return;
            }

            // found min cost and insert it into the list
            if (newCost < FirstNode.Cost)
            {
                newNode.NextNode = FirstNode;
                FirstNode = newNode;
                return;
            }

            // start traversing down the list
            var currentNode = FirstNode;
            while(currentNode.NextNode != null)
            {
                // if the cost is as following: currentCost <= totalCost < nextCost, return current index
                // because the cost must be the last one of the same cost in the list,
                // as well as standing right before its higher cost
                if (currentNode.Cost <= newCost && newCost < currentNode.NextNode.Cost)
                {
                    newNode.NextNode = currentNode.NextNode;
                    currentNode.NextNode = newNode;
                    return;
                }

                currentNode = currentNode.NextNode;
            }

            // currentNode is the last here
            // new node has the highest cost
            currentNode.NextNode = newNode;
        }

        /// <summary>
        /// The same as Dequeue where the method will remove the first item in the list
        /// </summary>
        /// <returns>The first item in the list. Null on an empty list</returns>
        public T RemoveFirst()
        {
            if (IsEmpty) return default;

            T item = FirstNode.Item;
            var newFirst = FirstNode.NextNode;

            FirstNode.NextNode = null;
            FirstNode = newFirst;
            return item;
        }

        /// <summary>
        /// The same as Pop where the method will remove the last item the the list.
        /// Trade off for not recording both ends of the list.
        /// Could say that O(n) is the best overall performance.
        /// </summary>
        /// <returns>The last item in the list. Null on an empty list</returns>
        public T RemoveLast()
        {
            if (IsEmpty) return default;

            var currentNode = FirstNode;
            while(currentNode.NextNode?.NextNode != null)
            {
                currentNode = currentNode.NextNode;
            }

            // if next node is null, the list is only containing 1 item right now
            T item = (currentNode.NextNode ?? currentNode).Item;

            if (currentNode.NextNode == null) FirstNode = null;
            currentNode.NextNode = null;
            return item;
        }
    }
}
