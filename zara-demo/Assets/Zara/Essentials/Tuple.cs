using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public class Tuple<T1, T2>
    {

        public Tuple(T1 i1, T2 i2)
        {
            Item1 = i1;
            Item2 = i2;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

    }
}
