using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Parser
{
    class Program
    {
        int x = 0;
        string[] lines = System.IO.File.ReadAllLines(@"./tokens.txt");
        
        static void Main(string[] args)
        {
            Program Parser = new Program();
            //FileBeingRead file = new FileBeingRead();
            //file.getFile();
            //Console.WriteLine(file.nextToken()); 
            Console.WriteLine("The first token is " + Parser.nextToken());
            Console.WriteLine("The second token is " + Parser.nextToken());
            
        }
       
        public void testing()
        {
            
            
          //  Console.WriteLine("Next token is " +y);
        }
        public string nextToken()
        {
            while ((lines[x] == null) || (lines[x] == ""))
            {
                x++;
            }
            return lines[x++];
        }
            
    }
    

}
public static class globals
{

}

//class FileBeingRead
//{
    

//    public void getFile()
//    {
        
//        Console.WriteLine(lines[4]);
//    }

//    public string nextToken()
//    {
//        x++;
//        return lines[++x]; 
        
//    }

//}




