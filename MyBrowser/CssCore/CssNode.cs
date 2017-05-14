using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssCore
{
    public enum CssSelectorType
    {
        Unkown,
        //Basic Selectors-------------------
        // #id
        IdSelector,
        //类 .class
        ClassSelector,
        //标签 p
        ElementSelector,
        //通配符选择器 *ns | * * | *
        UniversalSelector,
        //属性 [attr=value]
        //Combinator Selectors--------------
        AttributeSelector,
        //相邻兄弟 A + B
        AdjacentSiblingSelector,
        //普通兄弟 A ~ B
        GeneralSiblingSelector,
        //子选择器 A > B
        ChildSelector,
        //后代 A B
        DescendantSelector,
        //Pseudo Selector-------------------
        //伪元素 ::x
        PseudoElement,
        //伪类 :x
        PseudoClass

    }
    public class CssNode
    {
        public string name;
        public CssNode()
        {

        }
        public CssNode(string _name)
        {
            this.name = _name;
        }
    }
    public class StyleSheet : CssNode
    {
        public List<StyleRule> styleRules = new List<StyleRule>();
        public StyleSheet()
        {

        }
        public StyleSheet(string _name)
            : base(_name)
        {

        }
    }
    public class StyleRule : CssNode
    {
        public List<CssSelector> selectorList =new List<CssSelector>();
        public List<PropertyRule> propertyRuleList = new List<PropertyRule>();
        public StyleRule()
        {

        }
        public StyleRule(string _name)
            : base(_name)
        {

        }
    }
    public class CssSelector : CssNode
    {
        public CssSelectorType type;
        public string selector;

        public CssSelector()
        {

        }
        public CssSelector(string _name,CssSelectorType _type)
            :base(_name)
        {
            this.type = _type;
        }
    }
    public class BasicCssSelector : CssSelector
    {
        public string symbol; // "#" "." "[" "*" "" 
        public BasicCssSelector()
        {

        }
        public BasicCssSelector(string _name, CssSelectorType _type,string _symbol)
            :base(_name,_type)
        {
            this.symbol = _symbol;
        }
    }
    public class CombinatorCssSelector : CssSelector
    {
        public CssSelector left; // Combined by basic selectors
        public CssSelector right;
        public CombinatorCssSelector()
        {

        }
        public CombinatorCssSelector(string _name, CssSelectorType _type)
            :base(_name,_type)
        {
        }
    }
    public class PseudoCssSelector : CssSelector
    {
        public string symbol;//":" "::"
        public PseudoCssSelector()
        {

        }
        public PseudoCssSelector(string _name, CssSelectorType _type,string _symbol)
            :base(_name,_type)
        {
            this.symbol = _symbol;
        }
    }
    public class PropertyRule : CssNode
    {
        public string property;
        public string value;
        public PropertyRule()
        {

        }
        public PropertyRule(string _name,string _property,string _value)
            : base(_name)
        {
            this.property = _property;
            this.value = _value;
        }
    }
}
