using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    class Query
    {
        public string query;
        public Dictionary<byte, bool> index;
        public ulong weight;
        public Query()
        {
            query = string.Empty;
            weight = 0;
            index = new Dictionary<byte, bool>();
        }
        public Query(string q, ulong weight)
        {
            query = q;
            this.weight = weight;
            index = new Dictionary<byte, bool>();
        }
    }
}
