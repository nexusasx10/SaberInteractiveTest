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
            int index = 0;
            while (currentNode != null)
            {
                indexes[currentNode] = index;
                var strBytes = Encoding.UTF8.GetBytes(currentNode.Data); 
                s.Write(BitConverter.GetBytes(strBytes.Length));
                s.Write(strBytes);

                currentNode = currentNode.Next;
                index++;
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
            using (var br = new BinaryReader(s))
            {
                Count = br.ReadInt32();
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
                        currentNode.Next = new ListNode();
                        currentNode = currentNode.Next;
                    }

                    nodes[i] = currentNode;

                    var dataLength = br.ReadInt32();
                    currentNode.Data = Encoding.UTF8.GetString(br.ReadBytes(dataLength));
                }

                Tail = currentNode;

                for (int i = 0; i < Count; i++)
                {
                    int randIndex = br.ReadInt32();
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

            Dictionary<ListNode, int> indexes = new Dictionary<ListNode, int>();
            ListNode currentNode = Head;
            int index = 0;
            while (currentNode != null)
            {
                indexes[currentNode] = index;
                currentNode = currentNode.Next;
                index++;
            }

            Dictionary<ListNode, int> otherIndexes = new Dictionary<ListNode, int>();
            ListNode otherCurrentNode = other.Head;
            int otherIndex = 0;
            while (otherCurrentNode != null)
            {
                otherIndexes[otherCurrentNode] = otherIndex;
                otherCurrentNode = otherCurrentNode.Next;
                otherIndex++;
            }

            currentNode = Head;
            otherCurrentNode = other.Head;

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
                        if (currentNode.Rand == null && otherCurrentNode.Rand != null)
                            return false;
                        else if (currentNode.Rand != null && otherCurrentNode.Rand == null)
                            return false;
                        else if (currentNode.Rand != null && otherCurrentNode.Rand != null && indexes[currentNode.Rand] != otherIndexes[otherCurrentNode.Rand])
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
