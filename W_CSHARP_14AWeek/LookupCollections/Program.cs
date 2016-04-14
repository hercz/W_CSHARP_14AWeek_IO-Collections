using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            ListDictionary list = new ListDictionary();
            list["estados Unidos"] = "United States Of America";
            list["Canadá"] = "Canada";
            list["España"] = "Spain";

            Console.WriteLine(list["España"]);
            Console.WriteLine(list["Canadá"]);
            Console.ReadKey();
        }
    }
}
