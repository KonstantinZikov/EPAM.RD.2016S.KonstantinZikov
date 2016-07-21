using System;
using System.Collections.Generic;
using System.Threading;

namespace ReadWrite
{
    class Program
    {
        // Done: replace Object type with appropriate type for slim version of manual reset event.
        private static IList<Thread> CreateWorkers(ManualResetEventSlim mres, Action action, int threadsNum, int cycles)
        {
            var threads = new Thread[threadsNum];

            for (int i = 0; i < threadsNum; i++)
            {
                Action d = () =>
                {
                    mres.Wait();

                    for (int j = 0; j < cycles; j++)
                    {
                        action();
                    }
                };

                // Done: Create a new thread that will run the delegate above here.
                Thread thread = new Thread(new ThreadStart(d)); 
                threads[i] = thread;
            }

            return threads;
        }

        static void Main(string[] args)
        {
            var list = new MyList();

            // Done: Replace Object type with slim version of manual reset event here.
            ManualResetEventSlim mres = new ManualResetEventSlim(false);

            var threads = new List<Thread>();

            threads.AddRange(CreateWorkers(mres, () => { list.Add(1); }, 10, 100));
            threads.AddRange(CreateWorkers(mres, () => { list.Get(); }, 10, 100));
            threads.AddRange(CreateWorkers(mres, () => { list.Remove(); }, 10, 100));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.WriteLine("Press any key to run unblock working threads.");
            Console.ReadKey();

            // NOTE: When an user presses the key all waiting worker threads should begin their work.
            mres.Set();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }
    }
}
