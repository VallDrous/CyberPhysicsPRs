using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2CP
{
    public struct dataForCounter
    {
        public string name;
        public double dCount;
        public double nCount;

        public dataForCounter(string name, double dCount, double nCount)
        {
            this.name = name;
            this.dCount = dCount;
            this.nCount = nCount;
        }
    }
}
