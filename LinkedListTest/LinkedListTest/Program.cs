using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinkedListTest
{
    class Program
    {
        static void Main(string[] args)
        {

            SymbolTable list = new SymbolTable();

            list.AddToken("this");
            list.AddToken("that");
            Console.WriteLine("First List");
            list.CurrentTokenList();
            list.AddNewDepth();
            list.AddToken("this1");
            list.AddToken("that1");
            list.ConcatToToken("that1");
            Console.WriteLine("second list");
            list.CurrentTokenList();
            list.AddNewDepth();
            list.AddToken("hi");
            list.AddToken("bye");
            Console.WriteLine("third list");
            list.CurrentTokenList();
            list.AddNewDepth();
            list.GoUpDepth();
            Console.WriteLine("second list?");
            list.CurrentTokenList(); 
            bool isDeclared = list.IsTokenDeclared("this");
            bool exist = list.DoesTokenExist("this1");
            Console.WriteLine(isDeclared + " " + exist);
            Console.ReadKey();
        }
    }

    
    class SymbolTable
    {
        int currentNodeIndex = 0;
        int currentTokenIndex = 0;
        List<List<string>> depth = new List<List<string>>();
        List<string> tokens = new List<string>();

        public void AddNewDepth()
        {

            depth.Add(new List<string>(tokens));
            tokens.Clear();
            currentNodeIndex++;
            currentTokenIndex = 0;
           

        }

        public void GoUpDepth()
        {
            depth.RemoveAt(currentNodeIndex -1);
            currentNodeIndex--;
            tokens.Clear();
            tokens = depth[currentNodeIndex -1];
            currentNodeIndex = tokens.Count() - 1;
        }

        public void AddToken(string token)
        {
            tokens.Add(token);
            currentTokenIndex++;
        }

        public void ConcatToToken(string token)
        {
            string test = tokens[currentTokenIndex - 1];
            test = "hi hi " + test;
            tokens[currentTokenIndex - 1] = test;
        }

        public bool IsTokenDeclared(string token)
        {
            foreach(List<string> t in depth)
            {
                if (t.Contains(token))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DoesTokenExist(string token)
        {
            if (tokens.Contains(token))
            {
                return true;
            }
            return false;
        }

        public bool IsTokenSameType(string typeOne, string typeTwo)
        {

            return false;
        }

        public void CurrentTokenList()
        {
            foreach(string t in tokens)
            {
                Console.WriteLine("Token in Tokens is " + t);
            }
        }
    }
}
