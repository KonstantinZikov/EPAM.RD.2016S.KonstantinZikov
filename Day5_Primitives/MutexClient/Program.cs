﻿using System;
using System.Threading;

namespace MutexClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdNew = false;


            // TODO: mutex = new Mutex(.., "MyMutex", out createdNew);
            Mutex mutex = new Mutex(false,"MyMutex", out createdNew);
         
            Console.WriteLine("MutexClient. Mutex is new? " + createdNew + ". Waiting until mutex will be released.");

            // TODO: wait unit the mutex will be released
            mutex.WaitOne();
            Console.WriteLine("Press any key to release mutex.");
            Console.ReadKey();

            mutex.ReleaseMutex();
            Console.WriteLine("Mutex is released. Press any key to exit.");
            Console.ReadKey();
        }
    }
}