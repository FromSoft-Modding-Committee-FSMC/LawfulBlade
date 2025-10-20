namespace CommandLineSleuth
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Write out each argument, then pause
            for (int i = 0; i < args.Length; ++i)
                Console.WriteLine($"Argument #{1 + i}: {args[i]}");

            Console.ReadKey();
        }
    }
}
