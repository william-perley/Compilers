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

            FileBeingRead file = new FileBeingRead();
            file.getFile();

        }
        

    }
   

    class FileBeingRead
    {
        public void getFile()
        {
            string[] lines = System.IO.File.ReadAllLines(@"./tokens.txt");
            Console.WriteLine(lines[0]);
        }
    }
}



