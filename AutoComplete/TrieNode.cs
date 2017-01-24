using System.Collections.Generic;

namespace AutoComplete
{
    class TrieNode
    {
        public char Value { set; get; }
        public List<int> Offset { set; get; }
        public Dictionary<int, TrieNode> Nodes { set; get; }
        public bool FinalNode { set; get; }
    }
}