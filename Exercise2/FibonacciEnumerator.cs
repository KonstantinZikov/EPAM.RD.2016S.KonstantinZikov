using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public class FibonacciEnumerator : IEnumerator<int>
    {
        private int current = 1;
        private int last = 0;
        public int Current
        {
            get
            {
                return last;            
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose(){}

        public bool MoveNext()
        {
            current = last + current;
            last = current - last;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
