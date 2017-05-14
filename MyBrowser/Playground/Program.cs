using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        public class JsNumber
        {
            public Int64 value { get; set; }
            public JsNumber(Int64 v)
            {
                value = v;
            }
            public override string ToString()
            {
                return value.ToString();
            }
            public static bool operator ==(JsNumber num1,JsNumber num2)
            {
                return Object.Equals(num1.value, num2.value);
            }
            public static bool operator !=(JsNumber num1, JsNumber num2)
            {
                return !Object.Equals(num1.value, num2.value);
            }
            public static implicit operator Int64(JsNumber num)
            {
                return num.value;
            }
            public static implicit operator JsNumber(Int64 num)
            {
                return new JsNumber(num);
            }
        }
        enum State
        {
            Test_Hex = 0x0002
        }
        static void Main(string[] args)
        {
            //int[] strs = { 1111, 2222, 3333 };
            //var result = strs.Select(str => str.ToString()).Aggregate("", (res, str) => res + str);
            //Console.WriteLine(result);
            //Func<JsNumber, JsNumber, bool> fun = (s1, s2) => s1 == s2;
            //if(fun(1,1))
            //{
            //    Console.WriteLine(fun(1, 1).ToString());
            //}
            //else
            //{
            //    Console.WriteLine("bug");
            //}
            //Console.WriteLine((int)State.Test_Hex | 0x0004);
            //string code = "aaaa\naaaaa\naaaaa\n";
            //int line = 0;
            //foreach(var c in code )
            //{
            //    if( c=='\r')
            //    {
            //        line++;
            //    }
            //}
            //Console.WriteLine(line);

            //Console.WriteLine(Char.IsWhiteSpace('\r').ToString());

            string template = File.ReadAllText("../../Dom.txt");

            List<string> classNames = new List<string>() 
            {
                "Attr"
                ,"CharacterData"
                ,"ChildNode"
                ,"Comment"
                ,"CustomEvent"
                ,"Document"
                ,"DocumentFragment"
                ,"DocumentType"
                ,"DOMError"
                ,"DOMException"
                ,"DOMImplementation"
                ,"DOMString"
                ,"DOMTimeStamp"
                ,"DOMSettableTokenList"
                ,"DOMStringList"
                ,"DOMTokenList"
                ,"Element"
                ,"Event"
                ,"EventTarget"
                ,"HTMLCollection"
                ,"MutationObserver"
                ,"MutationRecord"
                ,"Node"
                ,"NodeFilter"
                ,"NodeIterator"
                ,"NodeList"
                ,"ParentNode"
                ,"ProcessingInstruction"
                ,"Range"
                ,"Text"
                ,"TreeWalker"
                ,"URL"
                ,"Window"
                ,"Worker"
                ,"XMLDocument"
            };
            //{
            //    "HTMLAnchorElement"
            //    ,"HTMLAppletElement"
            //    ,"HTMLAreaElement"
            //    ,"HTMLAudioElement"
            //    ,"HTMLBaseElement"
            //    ,"HTMLBodyElement"
            //    ,"HTMLBRElement"
            //    ,"HTMLButtonElement"
            //    ,"HTMLCanvasElement"
            //    ,"HTMLDataElement"
            //    ,"HTMLDataListElement"
            //    ,"HTMLDialogElement"
            //    ,"HTMLDirectoryElement"
            //    ,"HTMLDivElement"
            //    ,"HTMLDListElement"
            //    ,"HTMLElement"
            //    ,"HTMLEmbedElement"
            //    ,"HTMLFieldSetElement"
            //    ,"HTMLFontElement"
            //    ,"HTMLFormElement"
            //    ,"HTMLFrameElement"
            //    ,"HTMLFrameSetElement"
            //    ,"HTMLHeadElement"
            //    ,"HTMLHeadingElement"
            //    ,"HTMLHtmlElement"
            //    ,"HTMLHRElement"
            //    ,"HTMLIFrameElement"
            //    ,"HTMLImageElement"
            //    ,"HTMLInputElement"
            //    ,"HTMLKeygenElement"
            //    ,"HTMLLabelElement"
            //    ,"HTMLLegendElement"
            //    ,"HTMLLIElement"
            //    ,"HTMLLinkElement"
            //    ,"HTMLMapElement"
            //    ,"HTMLMediaElement"
            //    ,"HTMLMenuElement"
            //    ,"HTMLMetaElement"
            //    ,"HTMLMeterElement"
            //    ,"HTMLModElement"
            //    ,"HTMLObjectElement"
            //    ,"HTMLOListElement"
            //    ,"HTMLOptGroupElement"
            //    ,"HTMLOptionElement"
            //    ,"HTMLOutputElement"
            //    ,"HTMLParagraphElement"
            //    ,"HTMLParamElement"
            //    ,"HTMLPreElement"
            //    ,"HTMLProgressElement"
            //    ,"HTMLQuoteElement"
            //    ,"HTMLScriptElement"
            //    ,"HTMLSelectElement"
            //    ,"HTMLSourceElement"
            //    ,"HTMLSpanElement"
            //    ,"HTMLStyleElement"
            //    ,"HTMLTableElement"
            //    ,"HTMLTableCaptionElement"
            //    ,"HTMLTableCellElement"
            //    ,"HTMLTableDataCellElement"
            //    ,"HTMLTableHeaderCellElement"
            //    ,"HTMLTableColElement"
            //    ,"HTMLTableRowElement"
            //    ,"HTMLTableSectionElement"
            //    ,"HTMLTextAreaElement"
            //    ,"HTMLTimeElement"
            //    ,"HTMLTitleElement"
            //    ,"HTMLTrackElement"
            //    ,"HTMLUListElement"
            //    ,"HTMLUnknownElement"
            //    ,"HTMLVideoElement"
            //};
            new ClassGenerator(classNames, template).Generate();
            Console.WriteLine("Finish!");
            Console.ReadKey();
        }
    }
}
/*
"using System;\r\n" +
"using System.Collections.Generic;\r\n" +
"using System.IO;\r\n" +
"using System.Linq;\r\n" +
"using System.Text;\r\n" +
"using System.Threading.Tasks;\r\n" +
"namespace Html\r\n"+
"{\r\n"+
"    public class $className$\r\n"+
"    {\r\n"+
"        public $className$()\r\n"+
"        {\r\n"+
"             \r\n"+
"        }\r\n"+
"    }\r\n"+
"}\r\n"
 */