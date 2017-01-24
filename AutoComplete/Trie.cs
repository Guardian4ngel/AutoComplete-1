using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutoComplete
{
    class Trie
    {
        private TrieNode Root;
        private List<string> WordSuggestions;
        private List<int> QuerySuggestions;

        public Trie()
        {
            Root = new TrieNode() { Value = ' ' };
        }
        public List<string> GetWordSuggestions()
        {
            return WordSuggestions;
        }
        public List<int> GetQuerySuggestions()
        {
            if (QuerySuggestions != null && QuerySuggestions.Count > 0)
                return QuerySuggestions;
            return null;
        }
        public void InsertWords(string s)
        {
            TrieNode tmp = null;
            TrieNode current = Root;
            foreach (char x in s)
            {
                if (current.Nodes == null)
                    current.Nodes = new Dictionary<int, TrieNode>();
                if (!current.Nodes.ContainsKey(x))
                {
                    tmp = new TrieNode() { Value = x };
                    current.Nodes.Add(x, tmp);
                }
                current = current.Nodes[x];
            }
            current.FinalNode = true;
        }
        public void InsertQueries(ulong weight, string s)
        {
            TrieNode current = Root;
            byte j = 0;
            foreach (char x in s)
            {
                if (x == ' ')
                {
                    current = Root;
                    j++;
                    continue;
                }
                if (current.Nodes != null)
                {
                    if (current.Nodes.ContainsKey(x))
                    {
                        current = current.Nodes[x];
                        if (current.Offset == null)
                            current.Offset = new List<int>();
                        int currOffset = AutoComplete.myQueries.Count - 1;
                        if (!AutoComplete.myQueries[currOffset].index.ContainsKey(j))
                            AutoComplete.myQueries[currOffset].index.Add(j, true);
                        current.Offset.Add(currOffset);
                    }
                }
            }
        }
        private TrieNode find(string s)
        {
            if (s == string.Empty || s == null)
                return null;
            TrieNode current = Root;
            string cur = "";
            string prev = "";
            byte i = 0;
            foreach (char x in s)
            {
                if (x == ' ')
                {
                    if (cur == "")
                        continue;
                    prev = cur;
                    cur = "";
                    i++;
                    continue;
                }
                cur += x;
            }
            if (cur.Length == 0)
            {
                cur = prev;
                i--;
            }
            foreach (char x in cur)
            {
                if (current.Nodes == null)
                    return null;
                if (current.Nodes.ContainsKey(x))
                    current = current.Nodes[x];
                else
                    return null;
            }
            return current;
        }
        public void suggestQueries(string s)
        {
            TrieNode current = new TrieNode();
            if (QuerySuggestions != null)
                if (QuerySuggestions.Count > 0)
                    QuerySuggestions.Clear();
            current = find(s);
            if (current == Root || current == null)
                return;
            if (current.Offset == null)
                return;
            QuerySuggestions = new List<int>(current.Offset);
        }
        public void suggestWords(TrieNode current, string s)
        {
            if (current == null)
                return;
            if (current.Nodes == null || current == Root)
                return;
            foreach (TrieNode node in current.Nodes.Values)
            {
                string tmp = s + node.Value;
                if (node.FinalNode)
                {
                    if (WordSuggestions.Count > 5)
                        return;
                    else
                        WordSuggestions.Add(tmp);
                }
                suggestWords(node, tmp);
            }
        }
    }
}