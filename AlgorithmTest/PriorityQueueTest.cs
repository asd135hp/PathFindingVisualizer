using NUnit.Framework;
using Algorithms.Collections;

namespace AlgorithmTest
{
    public class PriorityQueueTest
    {
        private PriorityQueue<int> list;
        private int MaxVal;

        [SetUp]
        public void Setup()
        {
            Assert.DoesNotThrow(() =>
            {
                list = new PriorityQueue<int>();
                list.Insert(4, 2);
                list.Insert(15, 8);
                list.Insert(29, 14);
                list.Insert(35, 15);
                list.Insert(7, 4);
                list.Insert(7, 5);
                list.Insert(18, 11);
                list.Insert(94, 17);
                list.Insert(15, 9);
                list.Insert(20, 12);
                list.Insert(15, 10);
                list.Insert(63, 16);
                list.Insert(20, 13);
                list.Insert(6, 3);
                list.Insert(1, 1);
                list.Insert(9, 6);
                list.Insert(13, 7);
                MaxVal = 17;
            });
        }

        [Test]
        public void Test() { }

        /// <summary>
        /// Is also a type of insertion test
        /// </summary>
        [Test]
        public void TestPop()
        {
            while (!list.IsEmpty)
            {
                Assert.AreEqual(MaxVal--, list.Pop());
            }
        }

        /// <summary>
        /// Is also a type of insertion test
        /// </summary>
        [Test]
        public void TestDequeue()
        {
            int count = 1;
            while (!list.IsEmpty)
            {
                Assert.AreEqual(count++, list.Dequeue());
            }
        }
    }
}
