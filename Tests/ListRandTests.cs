using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaberInteractiveTest;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ListRandTests
    {
        [TestMethod]
        public void TestDefault()
        {
            ListRand original = CreateList(("1", 1), ("2", 2), ("3", 0));
            using (FileStream file = File.Create("temp"))
            {
                original.Serialize(file);
            }

            ListRand copy = new ListRand();
            using (FileStream file = File.OpenRead("temp"))
            {
                copy.Deserialize(file);
            }

            Assert.IsTrue(original.Equals(copy));
        }


        [TestMethod]
        public void TestNullRand()
        {
            ListRand original = CreateList(("abc", 1), ("def", 2), ("ghi", 0), ("gkl", -1));
            using (FileStream file = File.Create("temp"))
            {
                original.Serialize(file);
            }

            ListRand copy = new ListRand();
            using (FileStream file = File.OpenRead("temp"))
            {
                copy.Deserialize(file);
            }

            Assert.IsTrue(original.Equals(copy));
        }

        private ListRand CreateList(params (string, int)[] values)
        {
            var list = new ListRand()
            {
                Count = values.Length,
            };

            var items = new ListNode[values.Length];

            ListNode node = null;
            for (int i = 0; i < values.Length; i++)
            {
                ListNode newNode = new ListNode()
                {
                    Data = values[i].Item1
                };

                if (node == null)
                {
                    node = newNode;
                }
                else
                {
                    node.Next = newNode;
                    node = newNode;
                }

                items[i] = node;

                if (list.Head == null)
                    list.Head = node;
            }

            list.Tail = node;

            for (int i = 0; i < values.Length; i++)
            {
                var index = values[i].Item2;
                items[i].Rand = index > -1
                    ? items[index]
                    : null;
            }

            return list;
        }
    }
}
