using System;

namespace SaberInteractiveTest
{
    public class ListNode : IEquatable<ListNode>
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand;
        public string Data;

        public bool Equals(ListNode other)
        {
            if (other == null)
                return false;

            return Data.Equals(other.Data);
        }
    }
}
