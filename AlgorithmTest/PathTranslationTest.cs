using Algorithms;
using NUnit.Framework;

namespace AlgorithmTest
{
    public class PathTranslationTest
    {
        [Test]
        public void TestCorrectlyTranslatedPath()
        {
            var path = new State[]
            {
                new State(0,0),
                new State(0,1),
                new State(0,2),
                new State(0,3),
                new State(1,3),
                new State(2,3),
                new State(2,2),
                new State(3,2),
                new State(4,2),
                new State(4,3),
                new State(3,3)
            };
            Assert.AreEqual(
                "down; down; down; right; right; up; right; right; down; left;",
                PathTranslation.Translate(path).Trim()
            );
        }
    }
}