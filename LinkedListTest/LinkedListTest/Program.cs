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
            list.AddNewDepth();
            list.AddToken("this1");
            list.AddToken("that1");
            list.AddNewDepth();
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    
    class SymbolTable
    {
        int currentNodeIndex = 0;
        List<List<string>> depth = new List<List<string>>();
        List<string> tokens = new List<string>();

        public void AddNewDepth()
        {

            depth.Add(new List<string>(tokens));
            tokens.Clear();
            currentNodeIndex++;
           

        }

        public void GoUpDepth()
        {
            depth.RemoveAt(currentNodeIndex);
            currentNodeIndex--;
            tokens.Clear();
            tokens = depth[currentNodeIndex];
        }

        public void AddToken(string token)
        {
            tokens.Add(token);
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

            //var myLoL = new List<List<string>>();

            //List<string> cNode = myLoL[cNodeIndex];
            //myLoL.RemoveAt(cNodeIndex);
            //cNode = myLoL[cNodeIndex];
            //cNode.add


            //var inner = new List<string>();
            //inner.Add("afjiopf");
            //myLoL.Add(inner);

            //myLoL.Add(inner);

            //myLoL.Add(inner);

            //myLoL.Add(inner);

            //int count = myLoL.Count;
            //for (int i=0; i<count; i++)
            //{
                
            //}

            //var myEnumerator = myLoL.GetEnumerator();
            //while (myEnumerator.MoveNext())
            //{
            //    if (myEnumerator.Current.Contains("awjief"))
            //    {

            //    }
            //}


            //foreach (string innerList in myLoL)
            //{
            //    if (innerList.Contains("myStrofjaepje"))
            //    {
            //        // jefaop break; return true;
            //    }
            //}



        
    }
       
}
