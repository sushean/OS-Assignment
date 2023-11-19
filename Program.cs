using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static object lockObject = new object();
    static Queue<int> buffer = new Queue<int>();
    static int bufferSize = 5;
    static int itemCount = 0;

    static void Main()
    {
        Thread producerThread = new Thread(Producer);
        Thread consumerThread = new Thread(Consumer);

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }

    static void Producer()
    {
        while (true)
        {
            lock (lockObject)
            {
                while (itemCount == bufferSize)
                {
                    Console.WriteLine("Buffer full. Producer waiting.");
                    Monitor.Wait(lockObject);
                }

                int item = new Random().Next(100);
                Console.WriteLine($"Producing item: {item}");
                buffer.Enqueue(item);
                itemCount++;

                Monitor.Pulse(lockObject); // Notify the waiting consumer.
            }

            Thread.Sleep(1000); // Simulate some work by the producer.
        }
    }

    static void Consumer()
    {
        while (true)
        {
            lock (lockObject)
            {
                while (itemCount == 0)
                {
                    Console.WriteLine("Buffer empty. Consumer waiting.");
                    Monitor.Wait(lockObject);
                }

                int item = buffer.Dequeue();
                itemCount--;

                Console.WriteLine($"Consuming item: {item}");

                Monitor.Pulse(lockObject); // Notify the waiting producer.
            }

            Thread.Sleep(1500); // Simulate some work by the consumer.
        }
    }
}
