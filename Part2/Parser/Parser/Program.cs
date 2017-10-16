using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Parser
{
    class Program
    {        
        static void Main(string[] args)
        {
            FileBeingRead inputFile = new FileBeingRead();
            Testing(inputFile);
            Testing(inputFile);
            Testing(inputFile);
            Testing(inputFile);
            Console.ReadKey();

        }
       
        public static void Testing(FileBeingRead inputFile)
        {
            Test2(inputFile);
            string test = inputFile.NextToken();
            Console.WriteLine("The Token is " + test);
            Test2(inputFile);
            
            
          //  Console.WriteLine("Next token is " +y);
        }   

        public static void Test2(FileBeingRead inputFile)
        {
            string test = inputFile.NextToken();
            Console.WriteLine("Next Token is " + test);
        }
    }

    class FileBeingRead
    {
        private int x = 0, y = 0;
        private string[] lines = System.IO.File.ReadAllLines(@"./tokens.txt");

        public string NextToken()
        {
            
            string token = lines[y];
            while(  (y < lines.Length) && (token == "" ))
            {
             
                y++;
                token = lines[y];
            }
            
            var returnedToken = token.Split(' ');



            y++;
            x = y;

            return returnedToken[0];
        }
        public string CurrentToken()
        {
            return lines[x];
        }
    }
    

}






