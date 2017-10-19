using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No file name found.");
                return;
            }
            String fileName = args[0];
            LexicalAnalyzer lexical = new LexicalAnalyzer(fileName);
            lexical.processFile();


        }
    }
    class LexicalAnalyzer
    {
        private string fileName;
        private int commentCount;
        private List<string> allLines = new List<string>();

        public LexicalAnalyzer(String fileName)
        {

            this.fileName = fileName;
        }
        public void processFile()
        {

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                for (int x = 0; x < lines.Length; x++)
                {
                    processLine(lines[x]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
            WriteToFile();

        }
        public void processLine(string line)
        {
            String charSequence = null;
            String errorSequence = null;
            String numSequence = null;

            //        if (!line.isEmpty()) {
            //            System.out.println("INPUT: " + line);
            //        }

            for (int index = 0; index < line.Length; index++)
            {

                char b;
                char c = line[index];
                if (index > 0)
                {
                    b = line[index - 1];

                }
                else
                {
                    b = 'a';
                }
                bool isLastChar = index == line.Length - 1;

                if (errorSequence != null)
                {
                    if (ShouldStopError(c))
                    {
                        Console.WriteLine("Error: " + errorSequence);
                        errorSequence = null;
                    }
                    else
                    {
                        errorSequence += c.ToString();
                        continue;
                    }
                }

                if (commentCount == 0)
                {

                    // Check if the character is the beginning of a comment
                    if (c == '/')
                    {
                        if (!isLastChar)
                        {
                            char nextChar = line[index + 1];
                            if (nextChar == '/')
                            {
                                return;
                            }
                            else if (nextChar == '*')
                            {
                                commentCount++;
                                index++;
                                continue;
                            }
                        }
                    }

                    // Check if the character is part of a character sequence
                    if (IsLetter(c))
                    {
                        if (numSequence != null)
                        {
                            PrintNumber(numSequence);
                            numSequence = null;
                        }
                        if (charSequence == null)
                        {
                            charSequence = c.ToString();
                        }
                        else
                        {
                            charSequence += c.ToString();
                        }
                        continue;
                    }
                    else if ((charSequence != null) && (IsNumber(c) == true))
                    {
                        PrintCharSequence(charSequence);
                        charSequence = null;
                        continue;
                    }
                    else if (charSequence != null)
                    {
                        PrintCharSequence(charSequence);
                        charSequence = null;
                    }
                    else if (IsNumber(c))
                    {
                        if (charSequence != null)
                        {
                            PrintCharSequence(charSequence);
                            charSequence = null;
                        }
                        if (numSequence == null)
                        {
                            numSequence = c.ToString();
                        }
                        else
                        {
                            numSequence += c.ToString();
                        }
                        continue;
                    }
                    else if (((c == '-') || (c == '+')) && (b == 'E'))
                    {
                        numSequence += c.ToString();
                        continue;
                    }
                    else if (numSequence != null)
                    {
                        PrintNumber(numSequence);

                        numSequence = null;

                    }

                    else if ((c == '-') && (b == 'E'))
                    {
                        numSequence += c.ToString();
                    }
                    // Check if the character is part of a special operator
                    if (!isLastChar)
                    {
                        char nextChar = line[index + 1];
                        String possibleOperator = new StringBuilder().Append(c).Append(nextChar).ToString();
                        if (IsSpecialOperator(possibleOperator))
                        {
                            //Console.WriteLine(possibleOperator);
                            allLines.Add(possibleOperator);
                            index++;
                            continue;
                        }
                    }

                    // Check if the character is a delimeter
                    if (IsDelimeter(c))
                    {

                        allLines.Add(c.ToString());
                        //using (StreamWriter writer = new StreamWriter("tokens.txt", true))
                        //{
                        //    writer.WriteLine(c);
                        //}
                        continue;
                    }

                    // Ignore spaces
                    if (c == ' ')
                    {
                        continue;
                    }

                    // We have now encountered an error
                    errorSequence = c.ToString();
                }
                else
                {
                    // Check if the character is the start or end of a comment
                    if (!isLastChar)
                    {
                        char nextChar = line[index + 1];
                        if (c == '/' && nextChar == '*')
                        {
                            commentCount++;
                            index++;
                        }
                        else if (c == '*' && nextChar == '/')
                        {
                            commentCount--;
                            index++;
                        }
                    }
                }
            }

            if (errorSequence != null)
            {
                Console.WriteLine("Error: " + errorSequence);
            }
            if (charSequence != null)
            {
                PrintCharSequence(charSequence);
            }
            if (numSequence != null)
            {
                PrintNumber(numSequence);
            }
        }

        private void PrintCharSequence(string charSequence)
        {
            if (IsKeyword(charSequence))
            {
                //using (StreamWriter writer = new StreamWriter("tokens.txt", true))
                //{
                //    writer.WriteLine("keyword " + charSequence);
                //}
                allLines.Add("keyword " + charSequence);

            }
            else
            {
                //using (StreamWriter writer = new StreamWriter("tokens.txt", true))
                //{
                //    writer.WriteLine("id " + charSequence);
                //}
                allLines.Add("id " +charSequence);
            }
        }

        private void PrintNumber(string numSequence)
        {
            if (IsValidNumber(numSequence))
            {
                if (IsFloat(numSequence))
                {
                    //using (StreamWriter writer = new StreamWriter("tokens.txt", true))
                    //{
                    //    writer.WriteLine("float " + numSequence);
                    //}
                    allLines.Add("float " + numSequence);
                }
                else
                {
                    //using (StreamWriter writer = new StreamWriter("tokens.txt"))
                    //{
                    //    writer.WriteLine("int " + numSequence);
                    //}
                    allLines.Add("int " +numSequence);
                }
            }
            else
            {
                Console.WriteLine("Error: " + numSequence);
            }
        }

        private bool IsDelimeter(char possibleDelimeter)
        {

            char[] delimeters = new char[]{'+', '-', '/', '*',
                '=', '>', '<', '(',
                ')', '[', ']', '{',
                '}', ',', ';'};

            return delimeters.Contains(possibleDelimeter);
        }

        private bool IsSpecialOperator(string possibleOperator)
        {

            string[] specialOperators = new string[] { ">=", "<=", "!=", "==" };

            return specialOperators.Contains(possibleOperator);
        }

        private bool IsLetter(char possibleLetter)
        {

            return char.IsLower(possibleLetter);
        }

        private bool IsKeyword(string possibleKeyword)
        {

            string[] keywords = new string[] { "int", "void", "else", "return", "if", "while", "float" };

            return keywords.Contains(possibleKeyword);
        }

        private bool ShouldStopError(char possibleStopper)
        {

            return IsDelimeter(possibleStopper) || possibleStopper == ' ';
        }

        private bool IsNumber(char possibleNumber)
        {

            char[] numbers = new char[]{'1', '2', '3', '4', '5', '6',
                '7', '8', '9', '0', 'E', '.'};
            return numbers.Contains(possibleNumber);
        }

        private bool IsValidNumber(string validNumber)
        {
            int eCounter = 0;
            int periodCounter = 0;

            for (int i = validNumber.Length - 1; i >= 0; i--)
            {
                if (validNumber[i] == 'E')
                {
                    eCounter++;
                }
                if (validNumber[i] == '.')
                {
                    periodCounter++;
                }
            }
            if ((eCounter <= 1) && (periodCounter <= 1))
            {
                int en = validNumber.IndexOf('E');
                int pn = validNumber.IndexOf('.');

                if ((pn >= 0) && (en < 0))
                {
                    en = validNumber.Length + 1;
                }

                if (pn <= en)
                {
                    if (!(pn == 0) && !(pn == validNumber.Length - 1) && !(en == 0) && !(en == validNumber.Length - 1))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        private bool IsFloat(String floatNumber)
        {

            string E = "E";
            string Period = ".";

            if (floatNumber.Contains(E))
            {
                return true;
            }
            else if (floatNumber.Contains(Period))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void WriteToFile()
        {
            using (StreamWriter writer = new StreamWriter("tokens.txt"))
            {
                foreach (string line in allLines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}

