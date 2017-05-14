using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///
/// <author>Hesai Wang</author>
/// <date>2016.09.02</date>
/// <description>Css Parser</description>
///
namespace CssCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string cssCode = "p,div,#ab,.Aclass,.text-style{background:red;font-size:10px;}";
            List<string> tokens = new CssTokenizer(cssCode).Tokenizer();
            foreach (var item in tokens)
            {
                Console.WriteLine(item);
            }
            CssNode node = new CssParser().Parse(tokens, 0);
            Console.ReadKey();
        }
    }
}
