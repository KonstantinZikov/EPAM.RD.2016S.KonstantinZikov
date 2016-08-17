using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// Generates id's by using fibonacci sequence. Sequence: {1,2,3,5,8...}
    /// </summary>
    public class FibonacciEnumerator : MarshalByRefObject, IIdGenerator
    {
        private int current = 1;
        private int last = 1;

        public int Current
        {
            get
            {
                return this.last;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            this.current = this.last + this.current;
            this.last = this.current - this.last;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
