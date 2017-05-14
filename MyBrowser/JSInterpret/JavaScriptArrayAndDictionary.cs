using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSInterpret
{
    /*
    END_DOCUMENT,

     * Beginning of object: {
    BEGIN_OBJECT,

     * End of object: }
    END_OBJECT,

     * Beginning of array: [
    BEGIN_ARRAY,

     * End of array: ]
    END_ARRAY,

     * Seperator ':'
    SEP_COLON,

     * Seperator ','
    SEP_COMMA,

     * String value: "xxx"
    STRING,

     * Boolean value: true or false.
    BOOLEAN,

     * Number value: 123.456
    NUMBER,

     * Null value: null.
    NULL}
     */
    enum ArrayState
    {
        State_Expect_End_Document = 0x0002,

        /**
         * Should read "{" for next token.
         */
        State_Expect_Begin_Object = 0x0004,

        /**
         * Should read "}" for next token.
         */
        State_Expect_End_Object = 0x0008,

        /**
         * Should read object key for next token.
         */
        State_Expect_Object_Key = 0x0010,

        /**
         * Should read object value for next token.
         */
        State_Expect_Object_Value = 0x0020,

        /**
         * Should read ":" for next token.
         */
        State_Expect_Colon = 0x0040,

        /**
         * Should read "," for next token.
         */
        State_Expect_Comma = 0x0080,

        /**
         * Should read "[" for next token.
         */
        State_Expect_Begin_Array = 0x0100,

        /**
         * Should read "]" for next token.
         */
        State_Expect_End_Array = 0x0200,

        /**
         * Should read array value for next token.
         */
        State_Expect_Array_Value = 0x0400,

        /**
         * Should read a single value for next token (must not be "{" or "[").
         */
        State_Expect_Single_Value = 0x0800
    }
    public static partial class JavaScript
    {
        public static Token typeEquals(this Token token,TokenType type)
        {
            if (token.type == type)
            {
                return token;
            }
            exceptions.Add(new Exception("Array item type error"));
            return null;
        }

        private static bool hasState(int currState,ArrayState state)
        {
            return (currState & (int)state) > 0;
        }
        //{'a':'1','b':'1'} [1,2,3,4]
        // 2016.8.7 fix bugs and change array ,dict format.
        public static JsExpression ParseDictArray(List<Token> list)
        {
            //栈上存的是当前的value，current存一个作用域，object，array之类的
            Stack<object> stack = new Stack<object>();
            int state =
                (int)ArrayState.State_Expect_Begin_Array |  //[
                (int)ArrayState.State_Expect_Begin_Object | //{
                (int)ArrayState.State_Expect_Single_Value;  //num|string|bool|null

            while (list[index].type != TokenType.SemiColon)
            {
                switch (list[index].type)
                {
                    case TokenType.TRUE:
                    case TokenType.FALSE: //bool string--> parsed bool
                    case TokenType.NULL: // null
                    case TokenType.IDENTIFY: //identy
                    case TokenType.NUMBER: //num
                        if(hasState(state,ArrayState.State_Expect_Single_Value))
                        {
                            stack.Push(list[index]);
                            state = (int)ArrayState.State_Expect_End_Document;
                            continue;
                        }
                        if (hasState(state, ArrayState.State_Expect_Object_Value))
                        {
                            JsExpression key = stack.Pop() as JsExpression;
                            //用带dict标识的JsExpression模拟Dictionary
                            var dict = new JsExpression("dict", current);
                            key.parent = dict;
                            dict.child.Add(key);
                            dict.child.Add(new JsExpression(list[index].name, dict));
                            (stack.Peek() as JsExpression).child.Add(dict);

                            state = (int)ArrayState.State_Expect_Comma |
                                (int)ArrayState.State_Expect_End_Object;
                        }
                        if(hasState(state,ArrayState.State_Expect_Array_Value))
                        {
                            JsExpression curr = stack.Peek() as JsExpression;
                            JsExpression val = new JsExpression(list[index].name, curr);
                            curr.child.Add(val);
                            stack.Pop();
                            state = (int)ArrayState.State_Expect_Comma |
                                (int)ArrayState.State_Expect_End_Array
                                ;
                        }
                        break;
                    case TokenType.STRING:
                        if (hasState(state, ArrayState.State_Expect_Single_Value))
                        {
                            stack.Push(list[index]);
                            state = (int)ArrayState.State_Expect_End_Document;
                            continue;
                        }
                        else if(hasState(state,ArrayState.State_Expect_Object_Key))
                        {
                            stack.Push(list[index]);
                            state = (int)ArrayState.State_Expect_Colon;
                            continue;
                        }
                        else if (hasState(state, ArrayState.State_Expect_Object_Value))
                        {
                            JsExpression key = stack.Pop() as JsExpression;
                            //用带dict标识的JsExpression模拟Dictionary
                            var dict = new JsExpression("dict",current);
                            key.parent = dict;
                            dict.child.Add(key);
                            dict.child.Add(new JsExpression(list[index].name, dict));
                            (stack.Peek() as JsExpression).child.Add(dict);
                            state = (int)ArrayState.State_Expect_Comma |
                                (int)ArrayState.State_Expect_End_Object;
                        }
                        else if (hasState(state, ArrayState.State_Expect_Array_Value))
                        {
                            JsExpression curr = stack.Peek() as JsExpression;
                            JsExpression val = new JsExpression(list[index].name, curr);
                            curr.child.Add(val);
                            stack.Pop();
                            state = (int)ArrayState.State_Expect_Comma | 
                                (int)ArrayState.State_Expect_End_Array
                                ;
                        }
                        break;
                    
                    case TokenType.OpenBrace: //{
                        if (hasState(state, ArrayState.State_Expect_Begin_Object))
                        {
                            JsExpression arrayExp = new JsExpression("dict", current);//[
                            current = arrayExp;
                            stack.Push(current);

                            state = (int)ArrayState.State_Expect_Object_Key |
                                (int)ArrayState.State_Expect_Begin_Object |
                                (int)ArrayState.State_Expect_End_Object;
                            continue;
                        }
                        break;
                    case TokenType.OpenBracket: //[
                        if(hasState(state,ArrayState.State_Expect_Begin_Array))
                        {
                            JsExpression arrayExp = new JsExpression("list", current);//[
                            current = arrayExp;
                            stack.Push(current);
                            //STATUS_EXPECT_ARRAY_VALUE | STATUS_EXPECT_BEGIN_OBJECT | STATUS_EXPECT_BEGIN_ARRAY | STATUS_EXPECT_END_ARRAY;

                            state = (int)ArrayState.State_Expect_Array_Value |
                                (int)ArrayState.State_Expect_Begin_Object |
                                (int)ArrayState.State_Expect_Begin_Array |
                                (int)ArrayState.State_Expect_End_Array;
                        }
                        break;
                    case TokenType.CloseBrace: //}
                        if (hasState(state, ArrayState.State_Expect_End_Object))
                        {
                            // 两种情况
                            JsExpression val = stack.Pop() as JsExpression;
                            if (stack.Count == 0)//如果栈为空，将结果放回站内，否则添加到栈顶元素的子列表中
                            {
                                stack.Push(val);
                                state = (int)ArrayState.State_Expect_End_Document;
                                break;
                            }
                            current = current.parent;
                            JsExpression top = stack.Peek() as JsExpression;
                            if (top.value == "list")
                            {
                                //[1,2,{ CURRENT } , or ]
                                top.child.Add(val);
                                state = (int)ArrayState.State_Expect_Comma |
                                    (int)ArrayState.State_Expect_End_Array;
                            }
                            else
                            {
                                //key: { CURRENT } , or }
                                stack.Pop();
                                // 如何添加键值对？
                                // obj.add(top,val)???
                                //用带dict标识的JsExpression模拟Dictionary
                                var dict = new JsExpression("dict", current);
                                top.parent = dict;
                                val.parent = dict;
                                dict.child.Add(top);
                                dict.child.Add(val);
                                current.child.Add(dict);
                                state = (int)ArrayState.State_Expect_Comma |
                                    (int)ArrayState.State_Expect_End_Object;
                            }
                        }
                        break;
                    case TokenType.CloseBracket: //]
                        if (hasState(state, ArrayState.State_Expect_End_Array))
                        {
                            // key: [ CURRENT ] , or }
                            // xx, xx, [CURRENT] , or ]
                            // 两种情况
                            JsExpression val = stack.Pop() as JsExpression;
                            if(stack.Count == 0)//如果栈为空，将结果放回站内，否则添加到栈顶元素的子列表中
                            {
                                stack.Push(val);
                                state = (int)ArrayState.State_Expect_End_Document;
                                break;
                            }
                            current = current.parent;
                            JsExpression top = stack.Peek() as JsExpression;
                            if (top.value == "list")
                            {
                                top.child.Add(val);
                                state = (int)ArrayState.State_Expect_Comma | 
                                    (int) ArrayState.State_Expect_End_Array;
                            }
                            else
                            {
                                stack.Pop();
                                JsExpression obj = stack.Peek() as JsExpression;
                                // 如何添加键值对？
                                // obj.add(top,val)???
                                //用带dict标识的JsExpression模拟Dictionary
                                var dict = new JsExpression("dict", current);
                                top.parent = dict;
                                val.parent = dict;
                                dict.child.Add(top);
                                dict.child.Add(val);
                                current.child.Add(dict);

                                state = (int)ArrayState.State_Expect_Comma |
                                    (int)ArrayState.State_Expect_End_Object;
                            }
                        }
                        break;
                    case TokenType.COLON: //:
                        if (hasState(state, ArrayState.State_Expect_Colon))
                        {
                            //status = STATUS_EXPECT_OBJECT_VALUE | STATUS_EXPECT_BEGIN_OBJECT | STATUS_EXPECT_BEGIN_ARRAY;
                            state = (int)ArrayState.State_Expect_Object_Value |
                                (int)ArrayState.State_Expect_Begin_Object |
                                (int)ArrayState.State_Expect_Begin_Array
                                ;
                        }
                        break;
                    case TokenType.COMMA: //,
                        if (hasState(state, ArrayState.State_Expect_Comma))
                        {
                            state = (int)ArrayState.State_Expect_Array_Value |
                                (int)ArrayState.State_Expect_Begin_Object |
                                (int)ArrayState.State_Expect_Begin_Array;
                        }
                        break;
                    default: // end of array or dict
                        if(hasState(state,ArrayState.State_Expect_End_Document))
                        {
                            current = stack.Pop() as JsExpression;
                            if (stack.Count == 0)
                            {
                                break;
                            }
                        }
                        break;
                }
                index++;
            }
            JsExpression res = current;
            current = current.parent;
            return res;
        }
        // Array -> '[' ArrayItems ']'
        public static JsExpression ParseBasicArray(List<Token> list)
        {
            JsExpression arrayExp = new JsExpression(list[index].name, current);//[
            current = arrayExp;
            index++;
            if (list[index].type != TokenType.CloseBracket)
            {
                List<JsExpression> items = ParseBasicArrayItems(list);
                foreach (var item in items)
                {
                    arrayExp.child.Add(item);
                }
            }
            current = arrayExp.parent;
            return arrayExp;
        }
        // ArrayItem -> T [,T]*
        public static List<JsExpression> ParseBasicArrayItems(List<Token> list)
        {
            List<JsExpression> items = new List<JsExpression>();
            do
            {
                JsExpression item = ParseBasicArrayItem(list);
                items.Add(item);
                index++;//match ,
                if (list[index].type == TokenType.CloseBracket)
                {
                    break;
                }
                else
                {
                    (list[index].type == TokenType.COMMA).OrThrows("Array items separate by ',',expect a ,");
                    index++;//match next item
                }
            } while (list[index].type != TokenType.CloseBracket);
            return items;
        }
        // T->num|bool|identifier|Array
        public static JsExpression ParseBasicArrayItem(List<Token> list)
        {
            if (list[index].type == TokenType.OpenBracket)
            {
                index++;
                return ParseBasicArray(list);
            }
            else
            {
                return GetSingleOrMathExpressionItem(list);
            }
        }
    }
}
