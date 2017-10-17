
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
			List<string> movingTokens = new List<string>() { "int", "void", "float" };

			if (movingTokens.Contains(currentToken))
			{
				DeclarationList(inputFile, currentToken);
			}
			else
			{
				Reject();
			}
		}
		//Declaration-List -> Type-Specifier C Declaratioin-ListPrime
		public static void DeclarationList(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			List<string> firstTokens = new List<string>() { "int", "void", "float" };

			if (firstTokens.Contains(currentToken))
			{
				TypeSpecifier(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				RuleC(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				DeclarationListPrime(inputFile, currentToken);
			}
			else
			{
				Reject();
			}
		}
		//Declaration-ListPrime -> Type-Specifier C Declaration-ListPrime || Empty
		public static void DeclarationListPrime(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			List<string> firstTokens = new List<string>() { "int", "void", "float" };

			if (firstTokens.Contains(currentToken))
			{
				RuleC(inputFile, currentToken);
				return;
			} else if (inputFile.EndofFile()) {
				// Have accept here?
				return;
			}
			else
			{
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
				Reject();
			}
		}
		// X -> (Params) Compound Statement || Y
		public static void RuleX(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
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
					CompoundStatement(inputFile);
				}
				else
				{
					Reject();
				}
			}
			else
			{
				Reject();
			}
		}
		//Y -> ; || [ NUM ] 
		public static void RuleY(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			string firstToken = ";";
			string secondToken = "[";
			string thirdToken = "]";

			if (currentToken == firstToken)
			{
				inputFile.NextToken();
			} else if (currentToken == secondToken)
			{
				inputFile.NextToken();
				//Need to figure out what to do with "NUM"
				//inputFile.NextToken();???
				currentToken = inputFile.CurrentToken();

				if (currentToken == thirdToken)
				{
					inputFile.NextToken();
				}
				else
				{
					Reject();
				}

				currentToken = inputFile.CurrentToken();
				if (currentToken == firstToken)
				{
					inputFile.NextToken();
				}
				else
				{
					Reject();
				}
			}
			else
			{
				Reject();
			}
		}
		//Type-Specifier -> int || void || float
		public static void TypeSpecifier(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			List<string> firstToken = new List<string> { "int", "void", "float" };

			if (firstToken.Contains(currentToken))
			{
				inputFile.NextToken();
			}
			else
			{
				Reject();
			}
		}
		//Params -> Param Param-listPrime
		public static void Params(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			List<string> firstToken = new List<string> { "int", "void", "float" };

			if (firstToken.Contains(currentToken))
			{
				Param(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				ParamListPrime(inputFile, currentToken);
			}
			else
			{
				Reject();
			}
		}
		//Param-listPrime -> , param || Empty
		public static void ParamListPrime(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
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
				Reject();
			}
		}
		//Param -> type-specifier Z
		public static void Param(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			List<string> firstTokens = new List<string>() { "int", "void", "float" };

			if (firstTokens.Contains(currentToken))
			{
				TypeSpecifier(inputFile, currentToken);
				currentToken = inputFile.CurrentToken();
				RuleZ(inputFile, currentToken);
			}
			else
			{
				Reject();
			}
		}
		//Z -> ID M
		public static void RuleZ(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
			string firstToken = "id";

			if (currentToken == firstToken)
			{
				inputFile.NextToken();
				currentToken = inputFile.CurrentToken();
				RuleM(inputFile, currentToken);
			}
			else
			{
				Reject();
			}
		}
		//M -> [ ] || Empty
		public static void RuleM(FileBeingRead inputFile, string currentToken)
		{
			//string currentToken = inputFile.CurrentToken();
		}
		//Compound-Statement -> { local-declarationsPrime statement-listPrime }
		public static void CompoundStatement(FileBeingRead inputFile)
		{

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



