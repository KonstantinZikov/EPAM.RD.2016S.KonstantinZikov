using System;
using System.Collections;
using System.Collections.Generic;

namespace IdGenerators
{
    public class FibonacciEnumerator : IEnumerator<int>
    {
        private int current = 1;
        private int last = 1;

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

        public void Dispose()
        {
        }

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
