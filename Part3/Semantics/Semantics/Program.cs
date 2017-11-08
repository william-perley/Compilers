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

        }

        public static void BeginParsing()
        {
            //Creates an object that is the file of tokens that will be passed through the project.
            FileBeingRead inputFile = new FileBeingRead();
            SymbolTable symbolTable = new SymbolTable();
            FunProgram(inputFile, symbolTable);
            symbolTable.CurrentTokenList();
            Console.WriteLine("ACCEPT");


        }
        //Program -> Declaration-List
        public static void FunProgram(FileBeingRead inputFile, SymbolTable symbolTable)
        {
            string currentToken = inputFile.CurrentToken();
            string keywordToken = inputFile.KeyWord();
            List<string> firstTokens = new List<string>() { "int", "void", "float" };

            if (firstTokens.Contains(keywordToken))
            {
                DeclarationList(inputFile, currentToken, symbolTable);
            }
            else
            {

                Console.WriteLine("reject in funprogram and current token = " + currentToken);
                Reject();
            }
        }
        //Declaration-List -> Type-Specifier C Declaratioin-ListPrime
        public static void DeclarationList(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstTokens.Contains(keywordToken))
            {
                TypeSpecifier(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleC(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                DeclarationListPrime(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject in declarationlist and current token = " + currentToken);
                Reject();
            }
        }
        //Declaration-ListPrime -> Type-Specifier C Declaration-ListPrime || Empty
        public static void DeclarationListPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();
            currentToken = inputFile.CurrentToken();
            if (firstTokens.Contains(keywordToken))
            {
                TypeSpecifier(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleC(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                DeclarationListPrime(inputFile, currentToken, symbolTable);

            }
            else if (inputFile.EndofFile(currentToken))
            {

                return;
            }
            else
            {
                Console.WriteLine("reject in declarationlistprime and current token = " + currentToken);
                Reject();
            }
        }
        //C -> ID X
        public static void RuleC(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string token = "id";
            string idName = inputFile.IdName();
            if (currentToken == token)
            {
                //symbol table add type
                symbolTable.AddCurrentString(idName);
                symbolTable.AddType(inputFile);
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleX(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject in rulec and current token = " + currentToken);
                Reject();
            }
        }
        // X -> (Params) Compound Statement || Y
        public static void RuleX(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstTokens = new List<string> { ";", "[" };
            string secondToken = "(";
            string thirdToken = ")";

            if (firstTokens.Contains(currentToken))
            {
                RuleY(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {

                //Add number of arguments and concat
                int numbOfArguments = NumberOfArguments(inputFile);
                symbolTable.ConcatTokenAfter(numbOfArguments.ToString());
                AddArguments(inputFile, numbOfArguments, symbolTable);
                symbolTable.AddCurrentStringToTokens();
                symbolTable.AddNewDepth();
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Params(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();
                    currentToken = inputFile.CurrentToken();
                    CompoundStatement(inputFile, currentToken, symbolTable);
                }
                else
                {
                    Console.WriteLine("reject in rulex if statement and current token = " + currentToken);
                    Reject();
                }
            }
            else
            {
                Console.WriteLine("reject rulex and current token = " + currentToken);
                Reject();
            }
        }
        //Y -> ; || [ NUM ] 
        public static void RuleY(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = ";";
            string secondToken = "[";
            string thirdToken = "]";
            string fourthToken = "int";

            if (currentToken == firstToken)
            {
                symbolTable.AddCurrentStringToTokens();
                inputFile.NextToken();

            }
            else if (currentToken == secondToken)
            {
                //For '['
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                //For 'NUM'
                if (fourthToken == currentToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject ruley fourth token and current token = " + currentToken);
                    Reject();
                }

                currentToken = inputFile.CurrentToken();
                //'For ']'
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();

                }
                else
                {
                    Console.WriteLine("reject ruley third token and current token = " + currentToken);
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
                    Console.WriteLine("reject ruley first token and current token = " + currentToken);
                    Reject();
                }
            }
            else
            {
                Console.WriteLine("reject in ruley and current token = " + currentToken);
                Reject();
            }
        }
        //Type-Specifier -> int || void || float
        public static void TypeSpecifier(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string> { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(keywordToken))
            {
                //Symbol Table Modification here
                //symbolTable.ConcatTokenAfter(keywordToken);
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule typespecifier and current token = " + currentToken);
                Reject();
            }
        }
        //Params -> Param Param-listPrime
        public static void Params(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string> { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(keywordToken))
            {
                Param(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                ParamListPrime(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule params and current token = " + currentToken);
                Reject();
            }
        }
        //Param-listPrime -> , param || Empty
        public static void ParamListPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = ",";
            string secondToken = ")";

            if (currentToken == firstToken)
            {

                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Param(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule paramlistprime and current token = " + currentToken);
                Reject();
            }
        }
        //Param -> type-specifier Z
        public static void Param(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstTokens = new List<string>() { "int", "void", "float" };
            string keywordToken = inputFile.KeyWord();

            if (firstTokens.Contains(keywordToken))
            {
                TypeSpecifier(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleZ(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule param and current token = " + currentToken);
                Reject();
            }
        }
        //Z -> ID M || Empty
        public static void RuleZ(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "id";
            List<string> secondToken = new List<string>() { ",", ")" };
            string idName = inputFile.IdName();
            if (currentToken == firstToken)
            {
               //Symbol Table 
                symbolTable.AddCurrentString(idName);
                symbolTable.AddType(inputFile);
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleM(inputFile, currentToken, symbolTable);
            }
            else if (secondToken.Contains(currentToken))
            {

                return;
            }
            else
            {
                Console.WriteLine("reject rulez and current token = " + currentToken);
                Reject();
            }
        }
        //M -> [ ] || Empty
        public static void RuleM(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
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
                    Console.WriteLine("reject rulem if statement and current token = " + currentToken);
                    Reject();
                }
            }
            else if (secondToken.Contains(currentToken))
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rulem and current token = " + currentToken);
                Reject();
            }
        }
        //Compound-Statement -> { local-declarationsPrime statement-listPrime }
        public static void CompoundStatement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "{";
            string secondToken = "}";

            if (firstToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                LocalDeclarationsPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                StatementListPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == secondToken)
                {
                    
                    //need to add new depth before going up so does not delete the state before the one currently working on
                    symbolTable.AddNewDepth();
                    //See a "}", so need to decrease depth
                    symbolTable.GoUpDepth();
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule compoundstatement if and current token = " + currentToken);
                    Reject();
                }
            }
            else
            {
                Console.WriteLine("reject rule compoundstatement and current token = " + currentToken);
                Reject();
            }
        }
        //Local-declarationsPrime -> type-specifier ID Y local-declarationPrime || Empty
        public static void LocalDeclarationsPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "int", "void", "float" };
            List<string> secondToken = new List<string>() { "id", "(", ";", "{", "int", "float" };
            List<string> thirdToken = new List<string>() { "if", "while", "return" };
            string fourthToken = "}";
            string keywordToken = inputFile.KeyWord();
            //Add symbol table call in first token 
            if (firstToken.Contains(keywordToken))
            {
                TypeSpecifier(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == "id")
                {
                    string idToken = inputFile.IdName();
                    symbolTable.AddCurrentString(idToken);
                    symbolTable.AddType(inputFile);
                    symbolTable.AddCurrentStringToTokens();
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule localdeclarationsprime if and current token = " + currentToken);
                    Reject();
                }
                currentToken = inputFile.CurrentToken();
                RuleY(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                LocalDeclarationsPrime(inputFile, currentToken, symbolTable);

            }
            else if (secondToken.Contains(currentToken) || thirdToken.Contains(keywordToken))
            {
                return;
            }
            else if (currentToken == fourthToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule localdeclarationsprime and current token = " + currentToken);
                Reject();
            }
        }
        //Statement-ListPrime -> statement statement-listPrime || empty
        public static void StatementListPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "{", "int", "float", "if", "return", "while" };
            string secondToken = "else";
            string thirdToken = "}";
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(currentToken) || firstToken.Contains(keywordToken))
            {
                Statement(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                StatementListPrime(inputFile, currentToken, symbolTable);
            }
            else if (keywordToken == secondToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Statement(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                StatementListPrime(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == thirdToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule statementlistprime and current token = " + currentToken);
                Reject();
            }
        }
        //Statement -> expression-statement || compound-statement || selection-statement || iteration-statement || return-statement
        public static void Statement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "int", "float" };
            string secondToken = "{";
            string thirdToken = "if";
            string fourthToken = "return";
            string fifthToken = "while";
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(currentToken))
            {
                ExpressionStatement(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {
                CompoundStatement(inputFile, currentToken, symbolTable);
            }
            else if (keywordToken == thirdToken)
            {
                SelectionStatement(inputFile, currentToken, symbolTable);
            }
            else if (keywordToken == fourthToken)
            {
                ReturnStatement(inputFile, currentToken, symbolTable);
            }
            else if (keywordToken == fifthToken)
            {
                IterationStatement(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule statement and current token = " + currentToken);
                Reject();
            }
        }
        //Expression-Statement -> expression ; || ;
        public static void ExpressionStatement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", "int", "float" };
            string secondToken = ";";

            if (firstToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == secondToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule expressionstatement if and current token = " + currentToken);
                    Reject();
                }
            }
            else if (currentToken == secondToken)
            {
                symbolTable.AddCurrentStringToTokens();
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule expressionstatement and current token = " + currentToken);
                Reject();
            }
        }
        //Selection-Statement -> if A
        public static void SelectionStatement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "if";
            string keywordToken = inputFile.KeyWord();

            if (keywordToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleA(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule selectionstatement and current token = " + currentToken);
                Reject();
            }
        }
        //A -> ( D
        public static void RuleA(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "(";

            if (currentToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleD(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rulea and current token = " + currentToken);
                Reject();
            }
        }
        //D -> Expression R
        public static void RuleD(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", "int", "float" };

            if (firstToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleR(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruler and current token = " + currentToken);
                Reject();
            }
        }
        //R -> ) T
        public static void RuleR(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = ")";

            if (currentToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleT(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruler and current token = " + currentToken);
                Reject();
            }
        }
        //T -> Statement || else Statement
        public static void RuleT(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "{", "int", "float", "if", "return", "while" };
            string secondToken = "else";
            string keywordToken = inputFile.KeyWord();

            if (firstToken.Contains(currentToken) || firstToken.Contains(keywordToken))
            {
                Statement(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Statement(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruleT and current token = " + currentToken);
                Reject();
            }
        }
        //Iteration-Statement -> while (Expression) Statement
        public static void IterationStatement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "while";
            string secondToken = "(";
            string thirdToken = ")";
            string keywordToken = inputFile.KeyWord();

            if (firstToken == keywordToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();

            }
            else
            {
                Console.WriteLine("reject rule iterationstatement first token and current token = " + currentToken);
                Reject();
            }
            if (secondToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
            }
            else
            {
                Console.WriteLine("reject rule iterationstatement second token and current token = " + currentToken);
                Reject();
            }
            if (currentToken == thirdToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Statement(inputFile, currentToken, symbolTable);
            }
        }
        //Return-Statement -> return U
        public static void ReturnStatement(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "return";
            string keywordToken = inputFile.KeyWord();
            if (keywordToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleU(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject returnstatement and current token = " + currentToken);
                Reject();
            }
        }
        //U -> ExpressionStatement
        public static void RuleU(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(", ";", "[", "int", "float" };

            if (firstToken.Contains(currentToken))
            {
                ExpressionStatement(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruleu and current token = " + currentToken);
                Reject();
            }
        }
        //Expression -> ID F || ( Expression ) TermPrime B S || num TermPrime B S
        public static void Expression(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = "id";
            string secondToken = "(";
            string thirdToken = ")";
            string fourthToken = "int";
            string fifthToken = "float";

            if (currentToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleF(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();
                    currentToken = inputFile.CurrentToken();
                    TermPrime(inputFile, currentToken, symbolTable);
                    currentToken = inputFile.CurrentToken();
                    RuleB(inputFile, currentToken, symbolTable);
                    currentToken = inputFile.CurrentToken();
                    RuleS(inputFile, currentToken, symbolTable);
                }
                else
                {
                    Console.WriteLine("reject rule expression third token and current token = " + currentToken);
                    Reject();
                }
            }
            else if (currentToken == fourthToken || currentToken == fifthToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                TermPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleB(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleS(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule expression and current token = " + currentToken);
                Reject();
            }
        }
        //F -> P G || ( Args ) TermPrime B || empty
        public static void RuleF(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "[", "=", "*", "/", "<=", "<", ">", ">=", "==", "!=", "+", "-" };
            List<string> secondToken = new List<string>() { ";", ",", ")", "]" };
            string thirdToken = "(";
            string fourthToken = ")";

            if (firstToken.Contains(currentToken))
            {
                RuleP(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleG(inputFile, currentToken, symbolTable);
            }
            else if (secondToken.Contains(currentToken))
            {
                return;
            }
            else if (currentToken == thirdToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Args(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == fourthToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rulef fourth token " + currentToken);
                    Reject();
                }
                currentToken = inputFile.CurrentToken();
                TermPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleB(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleS(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rulef");
                Reject();
            }
        }
        //G -> = Expression || TermPrime B S
        public static void RuleG(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { ";", ",", ")", "]" };
            List<string> secontToken = new List<string>() { "*", "/", "<=", "<", ">", ">=", "==", "!=", "+", "-" };
            string thirdToken = "=";

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (secontToken.Contains(currentToken))
            {
                TermPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleB(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleS(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == thirdToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruleg");
                Reject();
            }
        }
        //P -> empty || [ Expression ]
        public static void RuleP(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { ";", ",", ")", "=", "*", "/", "<=", "<", ">", ">=", "==", "!=", "+", "-" };
            string secondToken = "[";
            string thirdToken = "]";

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (currentToken == secondToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rulep third token " + currentToken);
                    Reject();
                }
            }
            else
            {
                Console.WriteLine("reject rulep");
                Reject();
            }
        }
        //S -> Relop Factor TermPrime B || empty
        public static void RuleS(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { ";", ",", ")", "]" };
            List<string> secondToken = new List<string>() { "<=", "<", ">", ">=", "==", "!=", "+", "-" };

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (secondToken.Contains(currentToken))
            {
                Relop(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                Factor(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                TermPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleB(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rules");
                Reject();
            }
        }
        //Relop -> <= || < || > || >= || == || !=
        public static void Relop(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "<=", "<", ">", ">=", "==", "!=" };

            if (firstToken.Contains(currentToken))
            {
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule relop");
                Reject();
            }
        }
        //B -> Addop Factor TermPrime || empty
        public static void RuleB(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "<=", "<", ">", ">=", "==", "!=", "]", ")", ",", ";" };
            List<string> secondToken = new List<string>() { "+", "-" };

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (secondToken.Contains(currentToken))
            {
                Addop(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                Factor(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                TermPrime(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                RuleB(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject ruleb");
                Reject();
            }
        }
        //Addop -> + || -
        public static void Addop(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "+", "-" };

            if (firstToken.Contains(currentToken))
            {
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule addop");
                Reject();
            }
        }
        //TermPrime -> Mulop Factor TermPrime B || empty
        public static void TermPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { ";", ",", ")", "=", "<=", "<", ">", ">=", "==", "!=", "+", "-", "]" };
            List<string> secondToken = new List<string>() { "*", "/" };

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (secondToken.Contains(currentToken))
            {
                Mulop(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                Factor(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                TermPrime(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule termprime");
                Reject();
            }
        }
        //Mulop -> * || /
        public static void Mulop(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "*", "/" };
            if (firstToken.Contains(currentToken))
            {
                inputFile.NextToken();
            }
            else
            {
                Console.WriteLine("reject rule mulop");
                Reject();
            }
        }
        //Factor -> ( Expression ) || id E || num
        public static void Factor(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "float", "int" };
            string secondToken = "(";
            string thirdToken = "id";
            string fourthToken = ")";

            if (firstToken.Contains(currentToken))
            {
                inputFile.NextToken();
            }
            else if (secondToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == fourthToken)
                {
                    inputFile.NextToken();
                }
                else
                {
                    Console.WriteLine("reject rule factor fourthtoken");
                    Reject();
                }
            }
            else if (thirdToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                RuleE(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rule factor");
                Reject();
            }
        }
        //E -> P || ( Args )
        public static void RuleE(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { ";", ",", ")", "=", "*", "/", "<=", "<", ">", ">=", "==", "!=", "+", "-", "]" };
            string secondToken = "(";
            string thirdToken = ")";
            string fourthToken = "[";

            if (firstToken.Contains(currentToken))
            {
                return;
            }
            else if (secondToken == currentToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Args(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                if (currentToken == thirdToken)
                {
                    inputFile.NextToken();
                    return;
                }
                else
                {
                    Console.WriteLine("reject rulee third token");
                    Reject();
                }
            }
            else if (currentToken == fourthToken)
            {
                RuleP(inputFile, currentToken, symbolTable);
            }
            else
            {
                Console.WriteLine("reject rulee");
                Reject();
            }
        }
        //Args -> Expression ArgsListPrime || empty
        public static void Args(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            List<string> firstToken = new List<string>() { "id", "(" };
            List<string> secondToken = new List<string>() { "int", "float" };
            string thirdToken = ")";

            if (firstToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                ArgsListPrime(inputFile, currentToken, symbolTable);
            }
            else if (secondToken.Contains(currentToken))
            {
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                ArgsListPrime(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == thirdToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule args");
                Reject();
            }
        }
        //ArgsListPrime -> , Expression ArgsListPrime || empty
        public static void ArgsListPrime(FileBeingRead inputFile, string currentToken, SymbolTable symbolTable)
        {
            string firstToken = ",";
            string secondToken = ")";

            if (currentToken == firstToken)
            {
                inputFile.NextToken();
                currentToken = inputFile.CurrentToken();
                Expression(inputFile, currentToken, symbolTable);
                currentToken = inputFile.CurrentToken();
                ArgsListPrime(inputFile, currentToken, symbolTable);
            }
            else if (currentToken == secondToken)
            {
                return;
            }
            else
            {
                Console.WriteLine("reject rule argslistprime");
                Reject();
            }
        }
        public static int NumberOfArguments(FileBeingRead inputFile)
        {
            string endToken = ")";

            int numberOfArguments = 0;
            int currentIndex = inputFile.CurrentIndex();
            currentIndex++;
            string currentToken = inputFile.PeekAhead(currentIndex);
            while (currentToken != endToken)
            {
                if (currentToken.CompareTo("keyword") == 0)
                {
                    currentToken = inputFile.PeekAheadKeyword(currentIndex);
                    if(currentToken != "void")
                    {
                        numberOfArguments++;
                        currentIndex += 2;
                    }
                }

                currentIndex += 2;
                currentToken = inputFile.PeekAhead(currentIndex);

            }
            return numberOfArguments;
        }
        public static void AddArguments(FileBeingRead inputFile, int numbOfArguments, SymbolTable symbolTable)
        {
            string endToken = ")";
            int currentIndex = inputFile.CurrentIndex();
            currentIndex++;
            string currentToken = inputFile.PeekAhead(currentIndex);

            while (currentToken != endToken)
            {
                if (currentToken == "id")
                {
                    string id = inputFile.PeekAheadIdName(currentIndex);
                    symbolTable.ConcatTokenAfter(id);
                }
                if (currentToken == "keyword")
                {
                    string keyword = inputFile.PeekAheadKeyword(currentIndex);
                    if(keyword != "void")
                    {
                        symbolTable.ConcatTokenAfter(keyword);
                    }
                    
                }

                currentIndex += 2;
                currentToken = inputFile.PeekAhead(currentIndex);
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

        public int CurrentIndex()
        {
            return x;
        }
        public void NextToken()
        {
            x++;
            string token = lines[x];
            while ((x < lines.Length - 1) && (token == ""))
            {

                x++;
                token = lines[x];
            }

        }

        public string CurrentToken()
        {
            string[] token = lines[x].Split(' ');
            return token[0];
        }

        public string PreviousToken()
        {
            string[] token = lines[x - 2].Split(' ');
            return token[1];
        }

        public string PeekAhead(int y)
        {

            string token = lines[y];
            string[] returnToken;
            while ((y < lines.Length - 1) && (token == ""))
            {

                y++;
                token = lines[y];
                returnToken = lines[y].Split(' ');
                if (returnToken[0] == "keyword")
                {
                    return returnToken[0];
                }
                if (returnToken[0] == "id")
                {
                    return returnToken[0];
                }
                if (returnToken[0] == ")")
                {
                    return returnToken[0];
                }
            }
            return "";
        }

        public string IdName()
        {
            string[] id = lines[x].Split(' ');
            if (id[0] == "id")
            {
                return id[1];
            }
            return "error";
        }

        public string PeekAheadIdName(int y)
        {
            y++;
            string[] id = lines[y].Split(' ');
            if (id[0] == "id")
            {
                return id[1];
            }
            return "error";
        }

        public string KeyWord()
        {

            string[] keyword = lines[x].Split(' ');
            if (keyword[0] == "keyword")
            {
                return keyword[1];
            }
            return "error";
        }

        public string PeekAheadKeyword(int y)
        {
            y++;
            string[] keyword = lines[y].Split(' ');
            if (keyword[0] == "keyword")
            {
                return keyword[1];
            }
            return "error";
        }

        public bool EndofFile(string currentToken)
        {
            if (x < lines.Length && currentToken == "")
            {
                return true;
            }
            return false;
        }
        public bool IsVariable()
        {
            string[] token = lines[x].Split(' ');
            var t = token[1].GetType().ToString();
            if (t != "String")
            {
                return false;
            }
            return true;
        }
    }
    class SymbolTable
    {
        int currentNodeIndex = 0;
        int currentTokenIndex = 0;
        List<List<string>> depth = new List<List<string>>();
        List<string> tokens = new List<string>();
        string currentString = "";
        string intermittentString = "t";
        int intermittentNumber = 0;

        public void AddNewDepth()
        {
            depth.Add(new List<string>(tokens));
            tokens.Clear();
            currentNodeIndex++;
        }

        public void GoUpDepth()
        {
            depth.RemoveAt(currentNodeIndex - 1);
            currentNodeIndex--;
            tokens.Clear();
            tokens = depth[currentNodeIndex - 1];
        }

        public void AddCurrentStringToTokens()
        {
            tokens.Add(currentString);
            currentString = "";
        }

        public void AddCurrentString(string input)
        {
            currentString = input;
        }

        public void AddType(FileBeingRead inputfile)
        {
            string tok = inputfile.PreviousToken();
            //string[] keyword = tok.Split(' ');
            //string temp = keyword[1];
            ConcatTokenAfter(tok);
        }

        public void ConcatTokenBefore(string token)
        {
            currentString = token + " " + currentString;
        }

        public void ConcatTokenAfter(string token)
        {
            currentString = currentString + " " + token;
        }

        public bool IsTokenDeclared()
        {
            foreach (List<string> t in depth)
            {

                if (t.Contains(currentString))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DoesTokenExist()
        {
            if (tokens.Contains(currentString))
            {
                return true;
            }
            return false;
        }

        public bool IsTokenSameType(string typeOne, string typeTwo)
        {

            return false;
        }

        public string TempStringCounter()
        {
            intermittentString = intermittentString + intermittentNumber;
            return intermittentString;
        }

        public void CurrentTokenList()
        {
            foreach (string t in tokens)
            {
                Console.WriteLine("Token in Tokens is " + t);
            }
        }

        public bool IsMainLast()
        {
            int length = tokens.Count;
            length--;
            string isMain = tokens[length];
            string[] main = isMain.Split(' ');
            if(main[0] == "main")
            {
                return true;
            }
            return false;
        }
    }
}




