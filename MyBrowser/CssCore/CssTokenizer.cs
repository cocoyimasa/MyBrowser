using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssCore
{
    //5 delimeters
    // , : { } ;

    //插入$法！
    public class CssTokenizer
    {
        protected string code;
        public CssTokenizer(string code)
        {
            code = code.Replace("{", "${").Replace("}", "}$");
            this.code = code;
        }
        public List<string> Tokenizer()
        {
            string[] sep = {"$"};
            List<string> tokens = code.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToList();
            return tokens;
        }
    }
}
