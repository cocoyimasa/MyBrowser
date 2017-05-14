using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///
/// <author>Hesai Wang</author>
/// <date>2016.9.1</date>
/// <description>Node Document Element Attribute</description>
///
namespace HtmlParser
{
    public class HtmlNode
    {
        public HtmlToken name;
        public HtmlNode parentNode;
        public HtmlDocument ownerDocument;

        public List<HtmlNode> nodeList = new List<HtmlNode>();

        public HtmlNode()
        {

        }
        public HtmlNode(HtmlToken _token)
        {
            this.name = _token;
        }
        public virtual void Accept()
        {

        }
        public override string ToString()
        {
            return "Node:" + name.value;
        }
    }
    public class HtmlDocument : HtmlNode
    {
        public HtmlDocument()
        {

        }
        public HtmlDocument(HtmlToken _token)
            :base(_token)
        {

        }
        public virtual void Accept()
        {

        }

        public override string ToString()
        {
            return "Document:" + name.value;
        }
    }
    public class HtmlElement : HtmlNode
    {
        public HtmlToken innerText;
        public string innerHTML;
        public Dictionary<HtmlToken, HtmlToken> attrs = new Dictionary<HtmlToken,HtmlToken>();
        public HtmlElement()
        {

        }
        public HtmlElement(HtmlToken _token)
            :base(_token)
        {

        }

        public override void Accept()
        {
            base.Accept();
        }
        public override string ToString()
        {
            return "Element:" + name.value;
        }
    }
    public class HtmlAttribute : HtmlNode
    {
        public HtmlElement elem;
        public HtmlToken attrName;
        public HtmlToken attrValue;

        public HtmlAttribute()
        {

        }
        public HtmlAttribute(HtmlElement _elem, HtmlToken _attrName, HtmlToken _attrValue)
            : base(_attrName)
        {
            this.elem = _elem;
            this.attrName = _attrName;
            this.attrValue = _attrValue;
        }
        public virtual void Accept()
        {

        }
    }
    public class HtmlText : HtmlNode
    {
        public HtmlToken text;
        public HtmlElement elem;
        public HtmlText()
        {

        }
        public HtmlText(HtmlElement _elem,HtmlToken text)
            :base(text)
        {
            this.elem = _elem;
            this.text = text;
        }
        public virtual void Accept()
        {

        }
    }
}
