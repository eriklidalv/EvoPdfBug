using System;

namespace EvoPdfBug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var c = new Class1();
            var bytes = c.GetByteArrayAsync();
            var doc = c.CreateDocument(bytes);

            Console.WriteLine(doc != null ? "Success" : "Fail");
        }
    }
}
