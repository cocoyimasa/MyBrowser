using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssCore
{
    public static class CssUtils
    {
        public static void OrThrow(this bool expr,string message)
        {
            if(!(expr))
            {
                throw new Exception(message);
            }
        }
    }
    public class CssParser
    {
        public CssParser()
        {

        }
        public CssNode Parse(List<string> tokens,int index)
        {
            StyleSheet styleSheet = new StyleSheet();
            while(index < tokens.Count)
            {
                StyleRule rule = (StyleRule)ParseStyleRule(tokens, ref index);
                styleSheet.styleRules.Add(rule);
                index++;
            }
            return styleSheet;
        }
        public CssNode ParseStyleRule(List<string> tokens, ref int index)
        {
            List<CssSelector> selectorList = ParseCssSelectorList(tokens, ref index);
            index++;
            List<PropertyRule> propertyRuleList = ParsePropertyRuleList(tokens, ref index);
            StyleRule rule = new StyleRule("rule");
            rule.selectorList = selectorList;
            rule.propertyRuleList = propertyRuleList;
            return rule;
        }
        public CssSelector GetBasicSelector(string sel)
        {
            CssSelector selector = null;
            CssSelectorType type = CssSelectorType.Unkown;
            if (sel[0] == '#')
            {
                type = CssSelectorType.IdSelector;
                selector = new BasicCssSelector(sel.Substring(1), type, "#");
            }
            else if (sel[0] == '.')
            {
                type = CssSelectorType.ClassSelector;
                selector = new BasicCssSelector(sel.Substring(1), type, ".");
            }
            else if (sel[0] == '[')
            {
                type = CssSelectorType.AttributeSelector;
                selector = new BasicCssSelector(sel.Substring(1, sel.Length - 2), type, "[");
            }
            else if (sel[0] == ':')
            {
                if (sel[1] == ':')
                {
                    type = CssSelectorType.PseudoElement;
                    selector = new BasicCssSelector(sel.Substring(2), type, "::");
                }
                else
                {
                    type = CssSelectorType.PseudoClass;
                    selector = new BasicCssSelector(sel.Substring(1), type, ":");
                }
            }
            else if (Char.IsLetter(sel[0]))
            {
                type = CssSelectorType.ElementSelector;
                selector = new BasicCssSelector(sel.Substring(1), type, "");
            }
            return selector;
        }
        public CssSelector GetCurrentSelector(string symbol)
        {
            CssSelector current = null;
            if (symbol.Equals("+"))
            {
                current = new CombinatorCssSelector("+", CssSelectorType.AdjacentSiblingSelector);
            }
            else if (symbol.Equals("~"))
            {
                current = new CombinatorCssSelector("~", CssSelectorType.GeneralSiblingSelector);
            }
            else if (symbol.Equals(">"))
            {
                current = new CombinatorCssSelector(">", CssSelectorType.ChildSelector);
            }
            else
            {
                current = new CombinatorCssSelector("", CssSelectorType.DescendantSelector);
                (current as CombinatorCssSelector).left = GetBasicSelector(symbol);
            }
            return current;
        }
        public CssSelector GetCombinatorSelector(string sel)
        {
            string newSel = sel.Replace("+", " + ").Replace("~", " ~ ").Replace(">", " > ");
            string[] sep = new string[] { " " };
            List<string> basicSels = newSel.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToList();

            int j = basicSels.Count - 1;

            if (j == 0)
            {
                BasicCssSelector selector = (BasicCssSelector)GetBasicSelector(basicSels[j]);
                return selector;
            }
            else
            {
                Stack<CssSelector> stack = new Stack<CssSelector>();
                while (j >= 0)
                {
                    CombinatorCssSelector current = null;
                    CssSelector selectorRight = null;
                    if (stack.Count == 0)
                    {
                        selectorRight = (BasicCssSelector)GetBasicSelector(basicSels[j]);
                        j--;
                    }
                    else
                    {
                        selectorRight = stack.Pop();
                    }

                    string symbol = basicSels[j];
                    current = (CombinatorCssSelector)GetCurrentSelector(symbol);
                    j--;
                    if (j < 0)
                    {
                        current.right = selectorRight;
                        break;
                    }
                    BasicCssSelector selectorLeft = (BasicCssSelector)GetBasicSelector(basicSels[j]);
                    current.left = selectorLeft;
                    current.right = selectorRight;
                    stack.Push(current);
                    j--;
                }
                return stack.Pop();
            }
        }
        public List<CssSelector> ParseCssSelectorList(List<string> tokens, ref int index)
        {
            List<CssSelector> selectors = new List<CssSelector>();
            string[] sep = new string[]{","};
            List<string> selStrs = tokens[index].Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            //for exam : A > B > C, #id + * > .cls 
            foreach(var sel in selStrs)
            {
                (sel.Length > 0).OrThrow("Css Syntax Error:Selector parse error.Length should > 0");
                CssSelector selector = GetCombinatorSelector(sel);// #id + * > .cls 
                selectors.Add(selector);
            }
            return selectors;
        }

        public List<PropertyRule> ParsePropertyRuleList(List<string> tokens, ref int index)
        {
            List<PropertyRule> propertyRules = new List<PropertyRule>();
            string[] sep = new string[] { ";" };
            List<string> rules = tokens[index].Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            (rules.Count > 0).OrThrow("Css Syntax Error:Rules At least surrounded with { }");

            (rules[0].StartsWith("{")).OrThrow("Css Syntax Error:expect {");
            (rules[rules.Count - 1].Equals("}")).OrThrow("Css Syntax Error:expect }");
            rules[0] = rules[0].Substring(1);
            for (int i = 0; i < rules.Count - 1;i++ )
            {
                sep = new string[]{":"};
                List<string> pair = rules[i].Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                (pair.Count == 2).OrThrow("Css Syntax Error:A rule should be key-value pair");
                PropertyRule rule = new PropertyRule(pair[0], pair[0], pair[1]);
                propertyRules.Add(rule);
            }
            return propertyRules;
        }
    }
}
/*
if(sel.Contains("+"))
{
    type = CssSelectorType.AdjacentSiblingSelector;
}
else if(sel.Contains("~"))
{
    type = CssSelectorType.GeneralSiblingSelector;
}
else if(sel.Contains(">"))
{
    type = CssSelectorType.ChildSelector;
}
else if(sel.Contains("*"))
{
    type = CssSelectorType.UniversalSelector;
}
else if(sel.Contains(" "))
{
    type = CssSelectorType.DescendantSelector;
}
 */