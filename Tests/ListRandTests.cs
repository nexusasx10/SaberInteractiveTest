using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaberInteractiveTest;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ListRandTests
    {
        private void TestBase(ListRand list)
        {
            using (FileStream file = File.Create("temp"))
            {
                list.Serialize(file);
            }

            ListRand copy = new ListRand();
            using (FileStream file = File.OpenRead("temp"))
            {
                copy.Deserialize(file);
            }

            Assert.IsTrue(list.Equals(copy));
        }

        [TestMethod]
        public void TestOne()
        {
            ListRand original = CreateList(("1", 0));
            TestBase(original);
        }

        [TestMethod]
        public void TestMultiple()
        {
            ListRand original = CreateList(("1", 1), ("2", 2), ("3", 0));
            TestBase(original);
        }

        [TestMethod]
        public void TestNullRand()
        {
            ListRand original = CreateList(("abc", 1), ("def", 2), ("ghi", 0), ("gkl", -1));
            TestBase(original);
        }

        private ListRand CreateList(params (string, int)[] values)
        {
            ListRand list = new ListRand()
            {
                Count = values.Length,
            };

            ListNode[] items = new ListNode[values.Length];

            ListNode curNode = null;
            for (int i = 0; i < values.Length; i++)
            {
                ListNode newNode = new ListNode()
                {
                    Data = values[i].Item1,
                    Prev = curNode
                };

                if (curNode == null)
                {
                    curNode = newNode;
                }
                else
                {
                    curNode.Next = newNode;
                    curNode = newNode;
                }

                items[i] = curNode;

                if (list.Head == null)
                    list.Head = curNode;
            }

            list.Tail = curNode;

            for (int i = 0; i < values.Length; i++)
            {
                int index = values[i].Item2;

                items[i].Rand = index > -1
                    ? items[index]
                    : null;
            }

            return list;
        }
    }
}
