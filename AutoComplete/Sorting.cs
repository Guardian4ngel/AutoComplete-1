using System.Collections.Generic;

namespace AutoComplete
{
    class Sorting
    {
        public static void BubbleSort(List<int> lq)
        {
            for (int i = 0; i < lq.Count - 1; i++)
            {
                for (int j = 0; j < lq.Count - 1; j++)
                {
                    if (AutoComplete.myQueries[lq[j]].weight < AutoComplete.myQueries[lq[j + 1]].weight)
                    {
                        int tmp = lq[j];
                        lq[j] = lq[j + 1];
                        lq[j + 1] = tmp;
                    }
                }
            }
        }
        public static List<int> MergeSort(List<int> q)
        {
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> result = new List<int>();
            if (q.Count <= 1)
                return q;
            int mid = q.Count / 2;
            for (int i = 0; i < mid; i++)
            {
                left.Add(q[i]);
            }
            for (int i = mid; i < q.Count; i++)
            {
                right.Add(q[i]);
            }
            left = MergeSort(left);
            right = MergeSort(right);
            result = Merge(left, right);
            return result;
        }
        private static List<int> Merge(List<int> l, List<int> r)
        {
            List<int> res = new List<int>();
            int li = 0;
            int ri = 0;
            while (l.Count > li && r.Count > ri)
            {
                if (AutoComplete.myQueries[l[li]].weight > AutoComplete.myQueries[r[ri]].weight)
                {
                    res.Add(l[li]);
                    li++;
                }
                else
                {
                    res.Add(r[ri]);
                    ri++;
                }
            }
            while (l.Count > li)
            {
                res.Add(l[li]);
                li++;
            }
            while (r.Count > ri)
            {
                res.Add(r[ri]);
                ri++;
            }
            return res;
        }
    }
}
