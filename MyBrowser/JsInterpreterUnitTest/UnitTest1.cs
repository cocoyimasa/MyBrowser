using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSInterpret;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
/*
 *@author wanghesai
 *@time 2014/11/13-2014/11/15 
 *@time 2016/6/21 fix bugs and add library
 *@name JSInterpret
 ***/
namespace JsInterpreterUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPointExpression()
        {
            string[] code = {
                "var a=new Object();\n" ,
                "a.b=1;\n" ,
                "a.b;"
                };
            Env env = JSInterpret.JavaScript.InitEnv();
            List<string> results =new List<string>();
            foreach(var codeFragment in code)
            {
                string result =env.Run(codeFragment);
                results.Add(result);
            }
            string errors = "";
            if (JavaScript.exceptions.Count > 0)
            {
                foreach (var excep in JavaScript.exceptions)
                {
                    errors += excep.Message + '\n';
                }
                throw new Exception(errors);
            }
            Assert.AreEqual(results[0], "Object");
            Assert.AreEqual(results[1], "1");
            Assert.AreEqual(results[2], "1");

        }
        [TestMethod]
        public void TestFunctionStatementAndCall()
        {
            string[] code ={
                               "function testExpList(a,b){\n"+
                               "a=10;\n"+
                               "b=100;\n"+
                               "var c=0;"+
                               "if(a==b){\n"+
                               "c=1;}"+
                               "return c;}",
                               "testExpList(10,20);"
                           };
            Env env = JSInterpret.JavaScript.InitEnv();
            List<string> results = new List<string>();
            foreach (var codeFragment in code)
            {
                string result = env.Run(codeFragment);
                results.Add(result);
            }
            string errors = "";
            if (JavaScript.exceptions.Count > 0)
            {
                foreach (var excep in JavaScript.exceptions)
                {
                    errors += excep.Message + '\n';
                }
                throw new Exception(errors);
            }
            Assert.AreEqual(results[0], "Function");
            Assert.AreEqual(results[1], "0");
        }
        [TestMethod]
        public void TestBoolExpression()
        {
            string[] code ={
                               "2<=4;",
                               "3000==3000;",
                               "false || true;"
                           };
            Env env = JSInterpret.JavaScript.InitEnv();
            List<string> results = new List<string>();
            foreach (var codeFragment in code)
            {
                string result = env.Run(codeFragment);
                results.Add(result);
            }
            string errors = "";
            if (JavaScript.exceptions.Count > 0)
            {
                foreach (var excep in JavaScript.exceptions)
                {
                    errors += excep.Message + '\n';
                }
                throw new Exception(errors);
            }
            Assert.AreEqual(results[0], "True");
            Assert.AreEqual(results[1], "True");
            Assert.AreEqual(results[2], "True");
        }
        [TestMethod]
        public void TestTokenizer()
        {
            string[] code ={
                               "2<=4;",
                               "3000==3000;",
                               "false || true;",
                               "var str = '';",
                               "var str = '1111';",
                               "function testExpList(a,b){\r\n"+
                               "a=10;\r\n"+
                               "b=100;\r\n"+
                               "var c=0;\r\n"+
                               "if(a==b){\r\n"+
                               "c=1;\r\n}\r\n"+
                               "return c;\r\n}\r\n",
                               "testExpList(10,20);\n",
                               "var a=new Object();\n" ,
                               "a.b=1;\n" ,
                               "a.b;\n"
                           };
            Env env = JSInterpret.JavaScript.InitEnv();
            foreach (var codeFragment in code)
            {
                List<Token> tokenList = JavaScript.Tokenizer(codeFragment);
                Console.WriteLine(
                    tokenList.Select(item => item.name)
                    .Aggregate("", (res, str) => res + " " + str).Substring(1)
                    );
            }
        }
    }
}
