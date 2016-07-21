using System.Threading;
using static System.Threading.Monitor;
namespace Monitor
{
    // DONE: Use Monitor (not lock) to protect this structure.
    public class MyClass
    {
        private volatile int _value;
        private volatile object _syncObj = new object();

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
            try
            {
                Enter(_syncObj);
                _value++;
            }
            finally
            {
                Exit(_syncObj);
            }        
        }

        public void Decrease()
        {
            try
            {
                Enter(_syncObj);
                _value--;
            }
            finally
            {
                Exit(_syncObj);
            }            
        }
    }
}
