using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLInitializer.Library;

namespace SQLInitializer.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Adding rows in table. Please wait...");

                int x = Initializer.Initialize(100,
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=true;",
                "dbo.test2");

                Console.WriteLine($"{x} rows added successfuly.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                throw;
            }
        }
    }
}
