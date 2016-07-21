using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new ManualResetEventSlim(false);
            var pingEvent = new AutoResetEvent(false);
            var pongEvent = new AutoResetEvent(false);

            // Done: Create a new cancellation token source.
            CancellationTokenSource cts = new CancellationTokenSource();
            // Done: Assign an appropriate value to token variable.
            CancellationToken token = cts.Token;

            Action ping = () =>
            {
                Console.WriteLine("ping: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {

                    Console.WriteLine("ping!");
                    pingEvent.Set();                                              
                    Thread.Sleep(500);
                    pongEvent.WaitOne();

                    // Done: Use cancellation token "token" internals here to set appropriate value.
                    if (token.IsCancellationRequested)
                    {
                        continueRunning = false;
                    }
                     
                }

                // Done: Fix issue with blocked pong task.
                pingEvent.Set();

                Console.WriteLine("ping: done");
            };

            Action pong = () =>
            {
                Console.WriteLine("pong: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {

                    Console.WriteLine("pong!");
                    pingEvent.WaitOne();
                    Thread.Sleep(500);                  
                    pongEvent.Set();


                    // Done: Use cancellation token "token" internals here to set appropriate value.
                    if (token.IsCancellationRequested)
                    {
                        continueRunning = false;
                    }
                    
                }

                // Done: Fix issue with blocked ping task.
                pongEvent.Set();

                Console.WriteLine("pong: done");
            };


            var pingTask = Task.Run(ping);
            var pongTask = Task.Run(pong);

            Console.WriteLine("Press any key to start.");
            Console.WriteLine("After ping-pong game started, press any key to exit.");
            Console.ReadKey();
            start.Set();

            Console.ReadKey();
            // Done: cancel both tasks using cancellation token.
            cts.Cancel();
            

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
