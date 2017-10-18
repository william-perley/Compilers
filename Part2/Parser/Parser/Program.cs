/*William Perley N00636615
 * SPECIFICATION:
Your project is to use the grammar definition in the appendix
of your text to guide the construction of a recursive descent parser.
The parser should follow the grammar as described in A.2 page 492.

You should enhance the grammar to include FLOAT as
appropriate throughout all the grammar rules.

Upon execution, your project should report 

ACCEPT

or 

REJECT

exactly. Failure to print ACCEPT or REJECT appropriately will
result penalty for the test file. 

Appropriate documentation as described in the Syllabus should 
be included. A shar file, including all files necessary, 
(makefile, source files, test files, documentation file
(p2.txt in ascii format), and any other files) should be submitted 
by the deadline using turnin as follows:

   turnin fn ree4620_2

By my typing    make    after unsharing your file I should see an
executable called p2 (if you did your project in C) that will 
perform the syntax analysis. The analyzer will be invoked with:

   p2 test_fn

where p2 is the executable resulting from the make command 
(if done in C or C++) or is a script that executes your project (if
done in anyother language) and test_fn is the test filename upon 
which parsing is to be done. You must supply a makefile for any 
language. If your project is written in a pure interpreter (python, 
ruby, perl, etc.), provide a makefile and indicate such. 
(that is,  print "No makefile necessary" from your makefile).

Note that turnin will report the 2 day late date, if the project
is submitted on this date a penalty will be assessed.

Thus, the makefile might be (as needed for python):

-------------------------------------------------
all:
	@echo "no makefile necessary, project in python"
-------------------------------------------------

the p1 script would then be:

-------------------------------------------------
#!/bin/bash
python myprj.py $1
-------------------------------------------------

The shar file can be created as follows:

shar makefile p1 myprj.py p2.txt  > fn

You should not shar a directory, ie when I unshar your project
a new subdirectory should not be created.

You should test the integrity of your shar by copying it to a
temporary directory, unsharing, make, and execute to see that
all files are present and that the project works
appropriately.

Note: you may have an additional project assigned before this one is
due.

You must enhance your symbol table in preparation for the semantic
analysis project (Project 3). You do not need to print the table.

You do not need to do error recovery, upon detection of the error,
simply report such and stop the program.*/
using System;
using System.Collections.Generic;

namespace Parser
{
	class Program
	{
		static void Main(string[] args)
		{
			BeginParsing();
			Console.ReadKey();
		}

		public static void BeginParsing()
		{
			//Creates an object that is the file of tokens that will be passed through the project.
			FileBeingRead inputFile = new FileBeingRead();
			FunProgram(inputFile);
			Console.WriteLine("ACCEPT");
			Console.ReadKey();
		}
		//Program -> Declaration-List
		public static void FunProgram(FileBeingRead inputFile)
		{
			string currentToken = inputFile.CurrentToken();
            string keywordToken = inputFile.KeyWord();
			List<string> firstTokens = new List<string>() { "int", "void", "float" };

			if (firstTokens.Contains(keywordToken))
			{
				DeclarationList(inputFile, currentToken);
			}
			else
			{

				Console.WriteLine("reject in funprogram");
				Reject();
			}
		}
		//Declaration-List -> Type-Specifier C Declaratioin-ListPrime
		public static void DeclarationList(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstTokens.Contains(keywordToken))
			{
				TypeSpecifier(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				RuleC(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				DeclarationListPrime(inputFile, currentToken);
			}
			else
			{
				Console.WriteLine("reject in declarationlist");
				Reject();
			}
		}
		//Declaration-ListPrime -> Type-Specifier C Declaration-ListPrime || Empty
		public static void DeclarationListPrime(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstTokens.Contains(keywordToken))
			{
                TypeSpecifier(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
				RuleC(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                DeclarationListPrime(inputFile, currentToken);
				
			}
            else if (inputFile.EndofFile()) {
				// Have accept here?
				return;
			}
			else
			{
				Console.WriteLine("reject in declarationlistprime");
				Reject();
			}
		}
		//C -> ID X
		public static void RuleC(FileBeingRead inputFile, string currentToken)
		{
			string token = "id";

			if (currentToken == token)
			{
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
				RuleX(inputFile, currentToken);
			}
			else
			{
                Console.WriteLine("reject in rulec");
				Reject();
			}
		}
		// X -> (Params) Compound Statement || Y
		public static void RuleX(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstTokens = new List<string> { ";", "[" };
			string secondToken = "(";
			string thirdToken = ")";

			if (firstTokens.Contains(currentToken))
			{
				RuleY(inputFile, currentToken);
			} else if (currentToken == secondToken)
			{
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
				Params(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				if (currentToken == thirdToken)
				{
                    inputFile.NextToken();
                    currentToken = inputFile.CurrentToken();
					CompoundStatement(inputFile, currentToken);
				}
				else
				{
                    Console.WriteLine("reject in rulex if statement");
					Reject();
				}
			}
			else
			{
                Console.WriteLine("reject rulex");
				Reject();
			}
		}
		//Y -> ; || [ NUM ] 
		public static void RuleY(FileBeingRead inputFile, string currentToken)
		{
			string firstToken = ";";
			string secondToken = "[";
			string thirdToken = "]";

			if (currentToken == firstToken)
			{
				inputFile.NextToken();

            }
            else if (currentToken == secondToken)
			{
                //For '['
                inputFile.NextToken();
                //For 'NUM'
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
                //'For ']'
				if (currentToken == thirdToken)
				{
					inputFile.NextToken();

				}
				else
				{
                    Console.WriteLine("reject ruley third token");
					Reject();
				}
                //For last ';'
				currentToken = inputFile.CurrentToken();
				if (currentToken == firstToken)
				{
					inputFile.NextToken();
				}
				else
				{
                    Console.WriteLine("reject ruley first token");
					Reject();
				}
			}
			else
			{
                Console.WriteLine("reject in ruley");
				Reject();
			}
		}
		//Type-Specifier -> int || void || float
		public static void TypeSpecifier(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstToken = new List<string> { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(keywordToken))
			{
				inputFile.NextToken();
			}
			else
			{
                Console.WriteLine("reject rule typespecifier");
				Reject();
			}
		}
		//Params -> Param Param-listPrime
		public static void Params(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstToken = new List<string> { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(keywordToken))
			{
				Param(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				ParamListPrime(inputFile, currentToken);
			}
			else
			{
                Console.WriteLine("reject rule params");
				Reject();
			}
		}
		//Param-listPrime -> , param || Empty
		public static void ParamListPrime(FileBeingRead inputFile, string currentToken)
		{
			string firstToken = ",";
			string secondToken = ")";

			if (currentToken == firstToken)
			{
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
				Param(inputFile, currentToken);
			} else if (currentToken == secondToken)
			{
				return;
			}
			else
			{
                Console.WriteLine("reject rule paramlistprime");
				Reject();
			}
		}
		//Param -> type-specifier Z
		public static void Param(FileBeingRead inputFile, string currentToken)
		{
			List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstTokens.Contains(keywordToken))
			{
				TypeSpecifier(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				RuleZ(inputFile, currentToken);
			}
			else
			{
                Console.WriteLine("reject rule param");
				Reject();
			}
		}
		//Z -> ID M
		public static void RuleZ(FileBeingRead inputFile, string currentToken)
		{
			string firstToken = "id";

			if (currentToken == firstToken)
			{
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
				RuleM(inputFile, currentToken);
			}
			else
			{
                Console.WriteLine("reject rulez");
				Reject();
			}
		}
		//M -> [ ] || Empty
		public static void RuleM(FileBeingRead inputFile, string currentToken)
		{
            List<string> secondToken = new List<string>() { ",", ")" };
            string firstToken = "[";
            string thirdToken = "]";
            if (firstToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rulem if statement");
                    Reject();
                }
            } else if (secondToken.Contains(currentToken))
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rulem");
                Reject();
            }
		}
		//Compound-Statement -> { local-declarationsPrime statement-listPrime }
		public static void CompoundStatement(FileBeingRead inputFile, string currentToken)
		{
            string firstToken = "{";
            string secondToken = "}";

            if (firstToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                LocalDeclarationsPrime(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                StatementListPrime(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                if(currentToken == secondToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule compoundstatement if");
                }
            }
            else
            {
                Console.WriteLine("reject rule compoundstatement");
                Reject();
            }
		}
        //Local-declarationsPrime -> type-specifier ID Y local-declarationPrime || Empty
        public static void LocalDeclarationsPrime(FileBeingRead inputFile, string currentToken)
        {
            List<string> firstToken = new List<string>() { "int", "void", "float" };
            List<string> secondToken = new List<string>() { "id", "(", ";", "{", "if", "return", "while", "int", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(keywordToken))
            {
                TypeSpecifier(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                if(currentToken == "id")
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule localdeclarationsprime if");
                    Reject();
                }
                currentToken = inputFile.CurrentToken();
                RuleY(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                LocalDeclarationsPrime(inputFile, currentToken); 

            }
            else if (secondToken.Contains(currentToken))
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule localdeclarationsprime");
                Reject();
            }
        }
        //Statement-ListPrime -> statement statement-listPrime || empty
        public static void StatementListPrime(FileBeingRead inputFile, string currentToken)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "{", "int", "float", "if", "return", "while" };
            string secondToken = "}";

            if (firstToken.Contains(currentToken))
            {
                Statement(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                StatementListPrime(inputFile, currentToken);
            }
            else if(currentToken == secondToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule statementlistprime");
                Reject();
            }
        }
        //Statement -> expression-statement || compound-statement || selection-statement || iteration-statement || return-statement
        public static void Statement(FileBeingRead inputFile, string currentToken)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "int", "float" };
            string secondToken = "{";
            string thirdToken = "if";
            string fourthToken = "return";
            string fifthToken = "while";

            if (firstToken.Contains(currentToken))
            {
                ExpressionStatement(inputFile, currentToken);
            }
            else if(currentToken == secondToken)
            {
                CompoundStatement(inputFile, currentToken);
            }
            else if (currentToken == thirdToken)
            {
                SelectionStatement(inputFile, currentToken);
            }
            else if (currentToken == fourthToken)
            {
                ReturnStatement(inputFile, currentToken);
            }
            else if (currentToken == fifthToken)
            {
                IterationStatement(inputFile, currentToken);
            }
            else
            {
                Console.WriteLine("reject rule statement");
                Reject();
            }
        }
        //Expression-Statement -> expression ; || ;
        public static void ExpressionStatement(FileBeingRead inputFile, string currentToken)
        {
            List<string> firstToken = new List<string>() { "id", "(", "int", "float" };
            string secondToken = ";";

            if (firstToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                if (currentToken == secondToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule expressionstatement if");
                }
            }
            else if (currentToken == secondToken)
            {
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule expressionstatement");
                Reject();
            }
        }
        //Selection-Statement -> if A
        public static void SelectionStatement(FileBeingRead inputFile, string currentToken)
        {
            string firstToken = "if";
            string keywordToken = inputFile.KeyWord();

            if (keywordToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleA(inputFile, currentToken);
            }
            else
            {
                Console.WriteLine("reject rule selectionstatement");
                Reject();
            }
        }
        //A -> ( D
        public static void RuleA(FileBeingRead inputFile, string currentToken)
        {
            string firstToken = "(";

            if (currentToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleD(inputFile, currentToken);
            }
            else
            {
                Console.WriteLine("reject rulea");
                Reject();
            }
        }
        //D -> expression R
        public static void RuleD(FileBeingRead inputFile, string currentToken)
        {
            List<string> firstToken = new List<string>() { "id", "(", "int", "float" };

            if (firstToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken);
                currentToken = inputFile.CurrentToken();
                RuleR(inputFile, currentToken);
            }
            else
            {
                Console.WriteLine("reject ruler");
                Reject();
            }
        }
		//Reject Statement and Terminates program
		public static void Reject()
		{
			Console.WriteLine("REJECT");
			Environment.Exit(0);
		}

	}
    
	class FileBeingRead
	{
		private int x = 0;
		private string[] lines = System.IO.File.ReadAllLines(@"./tokens.txt");

		public void NextToken()
		{
			
			string token = lines[x];
			while(  (x < lines.Length) && (token == "" ))
			{

				x++;
			}

		}

		public string CurrentToken()
		{
			return lines[x];
		}

        
        public string KeyWord()
        {
            
            string[] keyword = lines[x].Split(' ');
            if(keyword[0] == "keyword")
            {
                return keyword[1];
            }
            return "error";
        }

		public bool EndofFile()
		{
			if(x < lines.Length)
			{
				return true;
			}

			return false;
			
		}

	}
	

}


//static void Main(string[] args)
//{
//    FileBeingRead inputFile = new FileBeingRead();
//    Testing(inputFile);
//    Testing(inputFile);
//    Testing(inputFile);
//    Testing(inputFile);
//    Console.ReadKey();

//}

//public static void Testing(FileBeingRead inputFile)
//{
//    Test2(inputFile);
//    string test = inputFile.NextToken();
//    Console.WriteLine("The Token is " + test);
//    Test2(inputFile);
//}

//public static void Test2(FileBeingRead inputFile)
//{
//    string test = inputFile.NextToken();
//    Console.WriteLine("Next Token is " + test);
//}
//    }



