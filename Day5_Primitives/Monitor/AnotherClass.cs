using System.Threading;

namespace Monitor
{
    // DONE: Use SpinLock to protect this structure.
    public class AnotherClass
    {
        private volatile int _value;
        private SpinLock _spin = new SpinLock();

        public int Counter
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public void Increase()
        {
            bool lockTaken = false;
            try
            {             
                _spin.Enter(ref lockTaken);
                {
                    _value++;
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _spin.Exit();
                }
            }                         
        }

        public void Decrease()
        {
            bool lockTaken = false;
            try
            {
                _spin.Enter(ref lockTaken);
                {
                    _value--;
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _spin.Exit();
                }
            }          
        }
    }
}
