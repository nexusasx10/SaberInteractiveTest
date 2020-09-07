using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberInteractiveTest
{
    public class ListRand : IEquatable<ListRand>
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            s.Write(BitConverter.GetBytes(Count));
            Dictionary<ListNode, int> indexes = new Dictionary<ListNode, int>();
            ListNode currentNode = Head;
            int curNodeIndex = 0;

            while (currentNode != null)
            {
                indexes[currentNode] = curNodeIndex;
                byte[] strBytes = Encoding.UTF8.GetBytes(currentNode.Data);
                s.Write(BitConverter.GetBytes(strBytes.Length));
                s.Write(strBytes);
                currentNode = currentNode.Next;
                curNodeIndex++;
            }

            currentNode = Head;
            while (currentNode != null)
            {
                if (currentNode.Rand == null)
                    s.Write(BitConverter.GetBytes(-1));
                else
                    s.Write(BitConverter.GetBytes(indexes[currentNode.Rand]));

                currentNode = currentNode.Next;
            }
        }

        public void Deserialize(FileStream s)
        {
            using (BinaryReader reader = new BinaryReader(s))
            {
                Count = reader.ReadInt32();
                ListNode[] nodes = new ListNode[Count];

                ListNode currentNode = null;
                for (int i = 0; i < Count; i++)
                {
                    if (currentNode == null)
                    {
                        currentNode = new ListNode();
                        Head = currentNode;
                    }
                    else
                    {
                        currentNode.Next = new ListNode()
                        {
                            Prev = currentNode
                        };
                        currentNode = currentNode.Next;
                    }

                    nodes[i] = currentNode;

                    int dataLength = reader.ReadInt32();
                    currentNode.Data = Encoding.UTF8.GetString(reader.ReadBytes(dataLength));
                }

                Tail = currentNode;

                for (int i = 0; i < Count; i++)
                {
                    int randIndex = reader.ReadInt32();
                    if (randIndex > -1)
                        nodes[i].Rand = nodes[randIndex];
                }
            }
        }

        public bool Equals(ListRand other)
        {
            if (other == null)
                return false;

            if (Count != other.Count)
                return false;

            Dictionary<ListNode, int> CalculateIndexes(ListRand list)
            {
                Dictionary<ListNode, int> indexesInternal = new Dictionary<ListNode, int>();
                ListNode currentNodeInternal = list.Head;
                int indexInternal = 0;
                while (currentNodeInternal != null)
                {
                    indexesInternal[currentNodeInternal] = indexInternal;
                    currentNodeInternal = currentNodeInternal.Next;
                    indexInternal++;
                }

                return indexesInternal;
            }

            Dictionary<ListNode, int> indexes = CalculateIndexes(this);
            Dictionary<ListNode, int> otherIndexes = CalculateIndexes(other);

            ListNode currentNode = Head;
            ListNode otherCurrentNode = other.Head;

            while (true)
            {
                if (currentNode == null)
                {
                    if (otherCurrentNode != null)
                        return false;

                    return true;
                }
                else 
                {
                    if (currentNode.Equals(otherCurrentNode))
                    {
                        bool onlyFirstNull = currentNode.Rand == null &&
                            otherCurrentNode.Rand != null;
                        bool onlySecondNull = currentNode.Rand != null &&
                            otherCurrentNode.Rand == null;
                        bool notEqual = currentNode.Rand != null &&
                            otherCurrentNode.Rand != null &&
                            indexes[currentNode.Rand] != otherIndexes[otherCurrentNode.Rand];

                        if (onlyFirstNull || onlySecondNull || notEqual)
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }

                currentNode = currentNode.Next;
                otherCurrentNode = otherCurrentNode.Next;
            }
        }
    }
}
