using System;
using System.Collections.Generic;

namespace ProjektSI
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>> where T1 : IComparable
    {
        public void Add(T1 Key, T2 Value)
        {
            Add(new Tuple<T1, T2>(Key, Value));
        }

        public new void Sort()
        {
            Comparison<Tuple<T1, T2>> c = (a, b) => a.Item1.CompareTo(b.Item1);
            base.Sort(c);
        }

    }
}
