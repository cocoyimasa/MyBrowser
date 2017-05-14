using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 *@author wanghesai
 *@time 2014/11/13-2014/11/15 
 *@time 2016/6/21 fix bugs and add library
 *@name JSInterpret
 ***/
namespace JSInterpret
{
    enum State
    {
        START, END, NUM, STRING, BOOLEAN, IDENTIFY
    }
    public static partial class JavaScript
    {
        public static Dictionary<string, TokenType> keyword = new Dictionary<string, TokenType>();
        public static string[] keywords = { 
                                              "for", "while", "function", 
                                              "if", "else", "var","return","new","in","this",
                                              "instanceof","typeof","undefined","break",
                                              "import","class","let","const","switch",
                                              "default","continue","true","false","null"
                                              /* "try","catch","throw","public","private","with" */
                                              /* "static","native","extends","enum","abstract" */
                                          };
        public static TokenType[] tokenType = { 
                                        TokenType.FOR, TokenType.WHILE, 
                                        TokenType.FUNCTION, TokenType.IF, 
                                        TokenType.ELSE, TokenType.VAR ,
                                        TokenType.RETURN,TokenType.NEW,
                                        TokenType.IN,TokenType.THIS,
                                        TokenType.INSTANCEOF,TokenType.TYPEOF,
                                        TokenType.UNDEFINED,TokenType.BREAK,
                                        TokenType.IMPORT,TokenType.CLASS,
                                        TokenType.LET,TokenType.CONST,
                                        TokenType.SWITCH,TokenType.DEFAULT,
                                        TokenType.CONTINUE,TokenType.TRUE,
                                        TokenType.FALSE,TokenType.NULL
                                              };
        public static string Join(this string sep, IEnumerable<Object> tokens)
        {
            return string.Join(sep, tokens);
        }
        public static string PrettyPrint(string[] tokens)
        {
            return "[" + ", ".Join(tokens.Select(s => "'" + s + "'")) + "]";
        }
        public static bool isKeyword(string identify)
        {
            foreach (var item in keywords)
            {
                if (identify.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isDelim(char ch)
        {
            char[] cDelims = { ',', '(', ')', '[', ']', ';', ':', '=', '!', '|', '<', '>', '+', '-', '*', '/', '&', '{', '}' };
            foreach (var item in cDelims)
            {
                if (ch == item)
                {
                    return true;
                }
            }
            return false;
        }
        public static List<Token> Tokenizer(string code)
        {
            List<Token> tokenList = new List<Token>();
            int count = -1;
            int line = 0;
            int column = -1;
            StringBuilder sb = new StringBuilder();
            State state = State.START;
            while (count < code.Length-1)
            {
                bool isLexeme = false;
                TokenType currentType = 0;
                count++;
                column++;
                switch (state)
                {
                    case State.START:
                        if(code[count]=='\r')
                        {
                            if (code[count + 1] != '\n')
                            {
                                line++;
                                column = -1;
                                continue;
                            }
                        }
                        else if (Char.IsWhiteSpace(code[count]))
                        {
                            continue;
                        }
                        else if (Char.IsLetter(code[count]) || code[count] == '_')
                        {
                            state = State.IDENTIFY;///
                            sb.Append(code[count]);
                            continue;
                        }
                        else if (Char.IsDigit(code[count]))
                        {
                            state = State.NUM;///
                            sb.Append(code[count]);
                            continue;
                        }
                        else if (code[count] == '\'' || code[count] == '"')
                        {
                            state = State.STRING;///
                            sb.Append(code[count]);
                            continue;
                        }
                        else
                        {
                            switch ((code[count]))
                            {
                                case '.':
                                    currentType = TokenType.POINT;
                                    break;
                                case '+':
                                    if (code[count + 1] != '+')
                                        currentType = TokenType.ADD;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.PLUS_PLUS;
                                    }
                                    break;
                                case '-':
                                    if (code[count + 1] != '-')
                                        currentType = TokenType.SUB;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.SUB_SUB;
                                    }
                                    break;
                                case '*':
                                    currentType = TokenType.MUL;
                                    break;
                                case '/':
                                    currentType = TokenType.DIV;
                                    break;
                                case '{':
                                    currentType = TokenType.OpenBrace;
                                    break;
                                case '}':
                                    currentType = TokenType.CloseBrace;
                                    break;
                                case '[':
                                    currentType = TokenType.OpenBracket;
                                    break;
                                case ']':
                                    currentType = TokenType.CloseBracket;
                                    break;
                                case '(':
                                    currentType = TokenType.OpenParenthese;
                                    break;
                                case ')':
                                    currentType = TokenType.CloseParenthese;
                                    break;
                                case ':':
                                    currentType = TokenType.COLON;
                                    break;
                                case ';':
                                    currentType = TokenType.SemiColon;
                                    break;
                                case '=':
                                    if (code[count + 1] != '=')
                                        currentType = TokenType.BIND;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.EQ;
                                    }
                                    break;
                                case '>':
                                    if (code[count + 1] != '=')
                                        currentType = TokenType.GT;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.GE;
                                    }
                                    break;
                                case '<':
                                    if (code[count + 1] != '=')
                                        currentType = TokenType.LT;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.LE;
                                    }
                                    break;
                                case '&':
                                    if (code[count + 1] != '&')
                                        currentType = TokenType.BIT_AND;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.AND;
                                    }
                                    break;
                                case '!':
                                    if (code[count + 1] != '=')
                                        currentType = TokenType.NOT;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.UNEQ;
                                    }
                                    break;
                                case '|':
                                    if (code[count + 1] != '|')
                                        currentType = TokenType.BIT_OR;
                                    else
                                    {
                                        sb.Append(code[count]);
                                        count++;
                                        column++;
                                        currentType = TokenType.OR;
                                    }
                                    break;
                                case ',':
                                    currentType = TokenType.COMMA;
                                    break;
                            }
                            isLexeme = true;
                        }
                        sb.Append(code[count]);
                        break;
                    case State.NUM:
                        if (Char.IsDigit(code[count]))
                        {
                            sb.Append(code[count]);
                            state = State.NUM;
                        }
                        else if (isDelim(code[count]) || Char.IsWhiteSpace(code[count]))
                        {
                            isLexeme = true;
                            count--;
                            column--;
                        }
                        break;
                    case State.IDENTIFY:
                        if (Char.IsLetterOrDigit(code[count]) || code[count] == '_')
                        {
                            sb.Append(code[count]);
                            state = State.IDENTIFY;
                        }
                        else if (isDelim(code[count]) || code[count] == '.' || Char.IsWhiteSpace(code[count]))
                        {
                            isLexeme = true;
                            count--;
                            column--;
                        }
                        break;
                    case State.STRING:
                        while (code[count] != '\'')
                        {
                            //stay this state
                            state = State.STRING;
                            sb.Append(code[count++]);
                            column++;
                        }
                        if (code[count] == '\'')
                        {
                            isLexeme = true;
                        }
                        break;
                }
                if (isLexeme)
                {
                    switch (state)
                    {
                        case State.START:
                            break;
                        case State.END:
                            break;
                        case State.NUM:
                            currentType = TokenType.NUMBER;
                            break;
                        case State.STRING:
                            currentType = TokenType.STRING;
                            break;
                        case State.IDENTIFY:
                            if (isKeyword(sb.ToString()))
                            {
                                currentType = getTokenType(sb.ToString());
                            }
                            else
                                currentType = TokenType.IDENTIFY;
                            break;
                        default:
                            break;
                    }
                    tokenList.Add(new Token(currentType, sb.ToString(),line,column - sb.Length+1));
                    sb.Clear();
                    state = State.START;
                }
            }
            return tokenList;
        }
    }
}
