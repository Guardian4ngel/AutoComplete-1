using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoComplete
{
    class AutoComplete
    {
        private static Trie t = new Trie();
        private static List<string> myDictionary = new List<string>();
        public static List<Query> myQueries = new List<Query>();
        private static List<int> Suggestions = new List<int>();
        private static string Corrected = "";
        public static void ImportWords()
        {
            StreamReader sr = new StreamReader("test.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                t.InsertWords(line.ToLower());
                myDictionary.Add(line.ToLower());
            }
        }
        public static void ImportQueries()
        {
            StreamReader sr = new StreamReader("Queries.txt");
            string line, s;
            string[] words;
            ulong w;
            while ((line = sr.ReadLine()) != null)
            {
                words = line.Split(',');
                w = ulong.Parse(words[0]);
                s = words[1].ToLower();
                Query q = new Query(s, w);
                myQueries.Add(q);
                t.InsertQueries(w, s);
            }
        }
        public static string GetCorrectedWords()
        {
            return Corrected;
        }
        public static List<int> GetSuggestions(string userInput)
        {
            if (userInput == null)
                return null;
            if (userInput != string.Empty)
            {
                t.suggestQueries(userInput);
                if (t.GetQuerySuggestions() != null && t.GetQuerySuggestions().Count >= 0)
                {
                    Suggestions = new List<int>(t.GetQuerySuggestions());
                    return Suggestions;
                }
                else
                {
                    string[] w = userInput.Split(' ');
                    if (w[w.Length - 1].Length >= 3)
                    {
                        Corrected = Rectify(userInput);
                        return GetSuggestions(Corrected);
                    }
                }

            }
            return null;
        }
        private static string Rectify(string s)
        {
            if (s == null)
                return null;
            if (s.Length < 3)
                return null;
            string[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length < 3)
                    break;
                string tmp = "";
                t.suggestQueries(words[i]);
                if (t.GetQuerySuggestions() != null)
                    continue;
                int dist;
                int min = 100;
                foreach (string x in myDictionary)
                {
                    if (Enumerable.Range(words[i].Length - 2, words[i].Length + 2).Contains(x.Length))
                    {
                        dist = editDistance(words[i], x);
                        if (dist <= (words[i].Length % 2 == 0 ? words[i].Length / 2 : words[i].Length / 2 + 1))
                        {
                            if (dist < min)
                            {
                                min = dist;
                                tmp = x;
                                t.suggestQueries(tmp);
                                if (t.GetQuerySuggestions() == null || t.GetQuerySuggestions().Count == 0)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (tmp != "")
                    words[i] = tmp;
            }
            s = "";
            for (int i = 0; i < words.Length; i++)
            {
                s += words[i];
                if (i < words.Count() - 1)
                    s += ' ';
            }
            if (t.GetQuerySuggestions() != null && t.GetQuerySuggestions().Count > 0)
                return s;
            else
                return null;
        }
        public static int editDistance(string s, string t)
        {
            if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(t)) return 0;

            int lengthS = s.Length;
            int lengthT = t.Length;
            var distances = new int[lengthS + 1, lengthT + 1];
            for (int i = 0; i <= lengthS; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthT; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthS; i++)
                for (int j = 1; j <= lengthT; j++)
                {
                    int cost = t[j - 1] == s[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
                }
            return distances[lengthS, lengthT];
        }
    }
}
