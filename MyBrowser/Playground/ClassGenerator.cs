using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    public class ClassGenerator
    {
        public string template;
        List<string> classNames = new List<string>();
        public ClassGenerator(List<string> classNames,string _template)
        {
            this.classNames = classNames;
            template = _template;
        }

        public void Generate()
        {
            foreach(var item in classNames)
            {
                string content = template.Replace("$className$", item);
                string path = "../../GeneratedCode/Dom/"+item + ".cs";
                if(!File.Exists(path))
                {
                    FileStream stream = File.Open(path,FileMode.Create);
                    stream.Close();
                    File.WriteAllText(path, content, Encoding.Unicode);
                }
                else
                {
                    File.WriteAllText(path, content, Encoding.Unicode);
                }
                
            }
        }
    }
}
