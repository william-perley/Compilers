using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedListTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    class LinkedList
    {
        int currentNode = 0;
        Link[] funcitonList = new Link[20];

        public void AddNewList(string id, string type)
        {
            string stringToAdd = id + " " + type;
         
            
            
            
        }
        
    }
    class Link
    {
        List<string> inputAndType = new List<string>();

        public void AddToList(string input)
        {
            
            inputAndType.Add(input);
        }
    }
}
