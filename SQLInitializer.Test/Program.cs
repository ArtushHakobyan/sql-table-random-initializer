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
            Initializer.Initialize(2,
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=true;", "dbo.text");
        }
    }
}
