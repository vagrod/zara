using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public class Pair<T>
    {

        public T First { get; set; }
        public T Second { get; set; }


        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }
    }
}
