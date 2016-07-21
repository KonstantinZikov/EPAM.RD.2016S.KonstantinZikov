using System;
using System.Collections.Generic;
using System.Threading;

namespace ReadWrite
{
    // Done: Use ReaderWriterLockSlim to protect this class.
    public class MyList
    {
        private List<int> _list = new List<int>()
        {
            1,
            2,
            3,
            4
        };

        private ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

        public void Add(int value)
        {
            _readerWriterLock.EnterWriteLock();
            Console.WriteLine("Write");
            _list.Add(value);
            _readerWriterLock.ExitWriteLock();
        }

        public void Remove()
        {
            _readerWriterLock.EnterWriteLock();
            if (_list.Count > 0)
            {
                Console.WriteLine("Write");
                _list.RemoveAt(0);
            }
            _readerWriterLock.ExitWriteLock();
        }

        public int Get()
        {
            _readerWriterLock.EnterReadLock();
            int value = 0;
            if (_list.Count > 0)
            {
                Console.WriteLine("Read");
                value = _list[0];
            }
            _readerWriterLock.ExitReadLock();
            return value;
        }
    }
}
