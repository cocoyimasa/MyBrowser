﻿using System;
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
    /*"for", "while", "function", 
    "if", "else", "var","return","new","in","this",
    "instanceof","typeof","undefined","break",
    "import","class","let","const","switch",
    "default","continue","true","false","null"
     * 
     * 
     */
    public enum TokenType
    {
        ST,
        NUMBER, STRING, BOOLEAN, IDENTIFY,
        OpenBracket/*[*/, CloseBracket, OpenBrace/*{*/, CloseBrace,
        OpenParenthese/*(*/, CloseParenthese,
        ADD, SUB, MUL, DIV, COLON, EQ/*==*/, BIND/*=*/, SemiColon/*;*/, COMMA/*;*/,
        GT, LT, GE, LE, AND, OR, UNEQ, NOT,
        BIT_AND,BIT_OR,PLUS_PLUS,SUB_SUB,
        POINT,/*.*/
        FOR, WHILE, FUNCTION,
        IF, ELSE, VAR, RETURN, NEW, IN, THIS,
        INSTANCEOF, TYPEOF, UNDEFINED, BREAK,
        IMPORT, CLASS, LET, CONST, SWITCH,
        DEFAULT, CONTINUE,TRUE,FALSE,NULL
    }
    public class Token
    {
        public TokenType type;
        public string name;
        public int line;
        public int column;
        public Token()
        { }
        public Token(TokenType _type, string _name)
        {
            type = _type;
            name = _name;
        }
        public Token(TokenType _type, string _name,int _line,int _column)
        {
            type = _type;
            name = _name;
            line = _line;
            column = _column;
        }
        public override string ToString()
        {
            return "name "+line+":"+column;
        }
    }
}
