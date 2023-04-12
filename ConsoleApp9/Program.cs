namespace ConsoleApp9
{
    internal class Program
    {
        static Queue<int> buffer = new Queue<int>();
        private readonly static object lockObj = new object();
        private static Random rand = new Random();   
            
        static void Produce()
        {
            int num1 = rand.Next(0, 100);
            while (true)
            {

                lock (lockObj)
                {

                    if (buffer.Count > 10)
                    {
                        Monitor.Pulse(lockObj);
                    }
                    if (buffer.Count > 100)
                    {
                        Monitor.Pulse(lockObj);
                        Monitor.Wait(lockObj);
                    }
                }

                buffer.Enqueue(num1);
                Console.WriteLine($"Produce:{buffer.Count}");


                Thread.Sleep(rand.Next(0, 200));
            }
        }

        static void Consume()
        {
            while (true)
            {

                lock (lockObj)
                {


                    if (buffer.Count < 10)
                    {
                        Monitor.Pulse(lockObj);
                        Monitor.Wait(lockObj);
                    }

                    buffer.Dequeue();
                    Console.WriteLine($"Consume:{buffer.Count}");
                }

                Thread.Sleep(rand.Next(0, 200));
            }


        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(()=>Produce());
            t1.Name = "Producer";
            Thread t2 = new Thread(()=> Consume());
            t2.Name = "Consumer";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();


        }
    }
}