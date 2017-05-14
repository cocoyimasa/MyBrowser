using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlParser;

namespace MyBroswer20160901
{
    class Program
    {
        static void Main(string[] args)
        {
            string htmlCode =
                "<!DOCTYPE html>\r\n"
                + "<html lang=\"en\">\r\n"
                + "<body>\r\n    "
                + "<div width=\"100\" height=\"200\">sssssssssss</div>\r\n"
                + "</body>\r\n</html>\r\n";
            //string htmlCode = "<html>";
            var tokenList = new HtmlParser.HtmlTokenizer(htmlCode).Tokenizer();
            foreach (var item in tokenList)
            {
                Console.WriteLine(item.value);
            }
            HtmlNode root = new HtmlParser.HtmlParser().Parse(tokenList, 0);

            Console.ReadKey();
        }
    }
}
