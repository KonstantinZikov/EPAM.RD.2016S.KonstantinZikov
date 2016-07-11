using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public static class FibonacciEnumeratorCreator
    {
        public static FibonacciEnumerator GetEnumerator()
        {
            return new FibonacciEnumerator();
        }     
    }
}
