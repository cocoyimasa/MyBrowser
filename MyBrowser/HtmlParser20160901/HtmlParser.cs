using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///
/// <author>Hesai Wang</author>
/// <date>2016.9.1</date>
/// <description>Parser</description>
///
namespace HtmlParser
{
    public static class Utils
    {
        public static void OrThrow(this bool expr, string message)
        {
            if (!expr)
            {
                throw new Exception(message);
            }
        }
    }
    public class HtmlParser
    {
        public HtmlNode current;
        public HtmlParser()
        {

        }
        public bool Move(List<HtmlToken> tokens, ref int index)
        {
            if (index >= tokens.Count)
            {
                return false;
            }
            else
            {
                index++;
                return true;
            }
        }
        public bool ReadType(List<HtmlToken> tokens, int index, TokenType type)
        {
            if (index >= tokens.Count)
            {
                return false;
            }
            else if (tokens[index].type == type)
            {
                return true;
            }
            return false;
        }
        public HtmlNode Parse(List<HtmlToken> tokens, int index)
        {
            HtmlDocument doc = new HtmlDocument();
            current = doc;
            while (ReadType(tokens, index, TokenType.HtmlElementBegin))
            {
                Move(tokens, ref index);
                HtmlElement elem = (HtmlElement)ParseElement(tokens, ref index);
                elem.ownerDocument = doc;
                doc.nodeList.Add(elem);
                Move(tokens, ref index);
            }
            //ReadType(tokens, index, TokenType.HtmlElement).OrThrow("Out of index | Read Element Type Error");

            return doc;
        }
        public HtmlNode ParseElement(List<HtmlToken> tokens, ref int index)
        {
            HtmlElement elem = new HtmlElement();
            elem.parentNode = current;
            current = elem;

            if(current is HtmlDocument)
            {
                elem.ownerDocument = current as HtmlDocument;
            }
            else
            {
                elem.ownerDocument = elem.parentNode.ownerDocument;
            }

            ReadType(tokens, index, TokenType.HtmlElement).OrThrow("Expect Element");
            HtmlToken elemName = tokens[index];
            elem.name = elemName;

            Move(tokens, ref index);
            while (ReadType(tokens, index, TokenType.HtmlAttributeName))
            {
                HtmlAttribute attr = (HtmlAttribute)ParseAttribute(tokens, ref index);
                elem.attrs.Add(attr.attrName, attr.attrValue);
                Move(tokens, ref index);
            }
            if (ReadType(tokens, index, TokenType.HtmlSlash))
            {
                Move(tokens, ref index);
                ReadType(tokens, index, TokenType.HtmlElementEnd).OrThrow("Expect >");
            }
            else if (ReadType(tokens, index, TokenType.HtmlElementEnd))
            {
                Move(tokens, ref index);
                ParseNodeList(tokens, ref index);
                ReadType(tokens, index, TokenType.HtmlElementBegin).OrThrow("Expect <");
                Move(tokens, ref index);
                ReadType(tokens, index, TokenType.HtmlSlash).OrThrow("Expect /");
                Move(tokens, ref index);
                ReadType(tokens, index, TokenType.HtmlElementClosed).OrThrow("Expect Element Close");
                (elemName.value == tokens[index].value).OrThrow("Element Name needs match");
            }

            current = current.parentNode;
            return elem;
        }
        public void ParseNodeList(List<HtmlToken> tokens, ref int index)
        {
            HtmlElement elem = current as HtmlElement;
            if(ReadType(tokens, index, TokenType.HtmlElementBegin) && 
                ReadType(tokens, index+1, TokenType.HtmlSlash))
            {
                return;
            }
            while (true) 
            { 
                if (ReadType(tokens, index, TokenType.HtmlInnerText))
                {
                    elem.nodeList.Add(new HtmlText(elem, tokens[index]));
                }
                else if (ReadType(tokens, index, TokenType.HtmlElementBegin) &&
                    ReadType(tokens, index + 1, TokenType.HtmlSlash))
                {
                    return;
                }
                else if (ReadType(tokens, index, TokenType.HtmlElementBegin))
                {
                    Move(tokens, ref index);
                    elem.nodeList.Add(ParseElement(tokens, ref index));
                }
                Move(tokens, ref index);
            }

        }
        public HtmlNode ParseAttribute(List<HtmlToken> tokens, ref int index)
        {
            HtmlAttribute attr = new HtmlAttribute();
            attr.elem = (HtmlElement)current;
            attr.parentNode = current;
            current = attr;

            attr.attrName = tokens[index];
            Move(tokens, ref index);

            ReadType(tokens, index, TokenType.Equal).OrThrow("expect =");
            Move(tokens, ref index);
            ReadType(tokens, index, TokenType.HtmlAttributeValue).OrThrow("expect AttributeValue");
            attr.attrValue = tokens[index];

            current = current.parentNode;
            return attr;
        }
    }
}
