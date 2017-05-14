using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///
/// <author>Hesai Wang</author>
/// <date>2016.9.1</date>
/// <description>Tokenizer</description>
///

namespace HtmlParser
{
    public enum TokenType
    {
        // Place holder
        UnkownType,
        // <Element Name>
        HtmlElement,
        // </Element Name>
        HtmlElementClosed,
        // AttributeName =..
        HtmlAttributeName,
        // <!-- -->
        HtmlComment,
        // ..= AttributeValue
        HtmlAttributeValue,
        // <d> PlainText </d>
        HtmlInnerText, 
        // '<'
        HtmlElementBegin,
        // '>'
        HtmlElementEnd,
        // '/'
        HtmlSlash,
        // '='
        Equal,
    }
    public class HtmlToken
    {
        public TokenType type;
        public string value;
        public int line;
        public int row;

        public HtmlToken(TokenType _type,
                        string _value,
                        int _line,
                        int _row
                        )
        {
            this.type = _type;
            this.value = _value;
            this.line = _line;
            this.row = _row;
        }
        public HtmlToken(TokenType _type,string _value)
        {
            this.type = _type;
            this.value = _value;
        }
    }
    public class HtmlTokenizer
    {
        public enum State
        {
            StateBegin,
            StateEnd,
            StateElementBegin,
            StateElementEnd,
            StateComment,
            StateAttributeName,
            StateAttributeValue,
            StateInnerText,
            StateLessThan,
            StateRightThan
        }
        protected string code;
        public HtmlTokenizer(string _code)
        {
            this.code = _code;
        }

        public List<HtmlToken> Tokenizer()
        {
            int index = 0;
            State state = State.StateBegin;
            int line = 0;
            int row = 0;
            

            List<HtmlToken> tokenList = new List<HtmlToken>();

            while(index < code.Length)
            {
                bool isLexDone = false;
                bool isStyleOrScript = false;
                bool selfClosed = false;
                string tokenValue="";
                TokenType type = TokenType.UnkownType;
                if(Char.IsWhiteSpace(code[index]))
                {
                    row++;
                    index++;
                    continue;
                }
                else if(code[index]=='\r' && code[index+1] == '\n')
                {
                    line++;
                    row = 0;
                    index += 2;
                    continue;
                }

                switch (state)
                {
                    case State.StateBegin:
                    {
                        if(code[index] == '<')
                        {
                            isLexDone = true;
                            tokenValue += code[index];
                            type = TokenType.HtmlElementBegin;

                            if(code[index+1] == '/')// Element Close
                            {
                                state = State.StateBegin;
                            }
                            else if(code[index+1] == '!')// Comment
                            {
                                index += 2;
                                if(code[index]=='-' || Char.IsLetter(code[index]))
                                {
                                    index++;
                                    while(code[index] != '>')
                                    {
                                        tokenValue += code[index];
                                        index++;
                                    }
                                }
                                isLexDone = false;
                                state = State.StateBegin;
                            }
                            else if(Char.IsLetter(code[index+1])) // Element Start
                            {
                                state = State.StateElementBegin;
                            }
                        }
                        else if(code[index] == '>')
                        {
                            isLexDone = true;
                            tokenValue += code[index];
                            type = TokenType.HtmlElementEnd;
                            state = State.StateBegin;
                        }
                        else if(code[index] == '/')
                        {
                            isLexDone = true;
                            tokenValue += code[index];
                            type = TokenType.HtmlSlash;
                            state = State.StateElementEnd;
                        }
                        else if(Char.IsLetter(code[index]))
                        {
                            tokenValue += code[index]; 
                            index++;
                            while(code[index] != '<')
                            {
                                tokenValue += code[index];
                                index++;
                            }
                            index--;
                            state = State.StateBegin;
                            type = TokenType.HtmlInnerText;
                            isLexDone = true;
                        }
                        else if(code[index] == '=')
                        {
                            isLexDone = true;
                            tokenValue += code[index];
                            type = TokenType.Equal;
                            state = State.StateAttributeValue;
                        }
                    }
                    break;
                    case State.StateElementBegin:
                    {
                        if(Char.IsLetter(code[index]))
                        {
                            tokenValue += code[index]; 
                            index++;
                        }
                        while(Char.IsLetterOrDigit(code[index]))
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        if (code[index] == '/' && code[index + 1] == '>')//End of Element
                        {
                            index--;
                            state = State.StateBegin;
                        }
                        else if(code[index] == '>')//Element Start
                        {
                            index--;
                            state = State.StateBegin;
                        }
                        else // Attribute
                        {
                            state = State.StateAttributeName;
                        }
                        type = TokenType.HtmlElement;
                        isLexDone = true;
                        if(tokenValue.Equals("style") || tokenValue.Equals("script"))
                        {
                            isStyleOrScript = true;
                        }
                    }
                    break;
                    case State.StateAttributeName:
                    {
                        if (Char.IsLetter(code[index])) // Attribute
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        else if (code[index] == '/' && code[index + 1] == '>')//End of Element
                        {
                            index--;
                            state = State.StateBegin;
                            break;
                        }
                        else if (code[index] == '>') // Element Start
                        {
                            index--;
                            state = State.StateBegin;
                            break;
                        }

                        while (Char.IsLetterOrDigit(code[index]) && code[index] != '=')
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        if(code[index] == '=')
                        {
                            state = State.StateBegin;
                            index--;
                        }
                        type = TokenType.HtmlAttributeName;
                        isLexDone = true;
                    }
                    break;
                    case State.StateAttributeValue:
                    {
                        if(code[index]=='\"')
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        while (code[index] != '\"')
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        state = State.StateAttributeName;
                        type = TokenType.HtmlAttributeValue;
                        isLexDone = true;
                    }
                    break;

                    case State.StateElementEnd:
                    {
                        if(Char.IsLetter(code[index]))
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        while (code[index] != '>')
                        {
                            tokenValue += code[index];
                            index++;
                        }
                        if(code[index]== '>')
                        {
                            index--;
                        }
                        state = State.StateBegin;
                        type = TokenType.HtmlElementClosed;
                        isLexDone = true;
                    }
                    break;

                }
                if(isLexDone)
                {
                    tokenList.Add(new HtmlToken(type, tokenValue, line, row));
                    row = row + tokenValue.Length;
                }
                index++;
                row++;
            }
            return tokenList;
        }
    }
}
