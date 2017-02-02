/*William Perley n00636615
Cminus Compiler part 1, Lexical Analyzer
This is meant to parse the input and tokenize
the inputs.
 */
import java.util.*;
import java.io.*;

public class CminusCompiler {

    public static void main(String[] args) {
        //Will return if there are no arguments
        if (args.length < 1) {
            System.out.println("No file name found");
            return;
        }

        String fileName = args[0];
        BufferedReader bufferedReader = null;
        String nextLine;
        try {

            bufferedReader = new BufferedReader(new FileReader(fileName));

            //will keep scanning in lines until the end of file is reached
            while ((nextLine = bufferedReader.readLine()) != null) {
                System.out.println("Input " + nextLine);
                // scanNextInput(nextLine);

                int i;
                int commentCount = 0;

                // String nextInput = scanNextSingleInput(nextLine);
                char[] nextSingleInput = nextLine.toCharArray();
                for (i = 0; i < nextLine.length(); i++) {

                    nextSingleInput[i] = nextLine.charAt(i);
                    
                    String initialCheck = String.valueOf(nextSingleInput);
                    StringBuilder buildSingleInput = new StringBuilder();
                    
                    char firstInput = nextSingleInput[i];
                     buildSingleInput.append(firstInput);
                     String singleInput = buildSingleInput.toString();
                    
                    if (isItADelimeter(initialCheck) == true) {
                        StringBuilder buildString = new StringBuilder();
                         

                        
                        char followingInput = nextSingleInput[++i];

                        //combines the 2 characters from above
                        buildString.append(firstInput);
                        buildString.append(followingInput);

                       

                        //Converts Stringbuilder to String so can pass to methods to compare
                        String possibleComment = buildString.toString();
                        

                        if (isItAComment(possibleComment) == true) {

                            String startComment = "/*";
                            String endComment = "*/";

                            if (possibleComment.equals(startComment)) {
                                commentCount++;
                            } else if (possibleComment.equals(endComment)) {
                                commentCount--;
                            } else if (commentCount == 0) {
                                return;
                            }
                        }
                        if (commentCount < 1) {

                            if (isItASpecialOperator(possibleComment) == true) {
                                String gThanEqual = ">=";
                                String lThanEqual = "<=";
                                String notEqual = "!=";
                                String equivalent = "==";
                                if (possibleComment.equals(gThanEqual)) {
                                    System.out.println(gThanEqual);
                                } else if (possibleComment.equals(lThanEqual)) {
                                    System.out.println(lThanEqual);
                                } else if (possibleComment.equals(notEqual)) {
                                    System.out.println(notEqual);
                                } else {
                                    System.out.println(equivalent);
                                }
                            } else if (isItAnError(singleInput) == true) {
                                System.out.println("Error " + singleInput);
                            }

                        }
                    }
                }
                if (commentCount < 1) {
                    if(Character.isAlphabetic()
                }

            }

        } catch (IOException e) {
            System.out.println("Could not find this file");
        } finally {
            if (bufferedReader != null) {
                try {
                    bufferedReader.close();
                } catch (IOException e) {
                }
            }
        }

    }
    //    private static void scanNextInput(String nextLine) {
    //        int i;
    //        int commentCount = 0;
    //        // String nextInput = scanNextSingleInput(nextLine);
    //        char[] nextSingleInput = nextLine.toCharArray();
    //        for (i = 0; i < nextLine.length(); i++) {
    //            nextSingleInput[i] = nextLine.charAt(i);
    //            String initialCheck = String.valueOf(nextSingleInput);
    //
    //            if (isItADelimeter(initialCheck) == true) {
    //                StringBuilder buildString = new StringBuilder();
    //                //gets the first character and second
    //                char firstInput = nextSingleInput[i];
    //                char followingInput = nextSingleInput[++i];
    //                //combines the 2 characters from above
    //                buildString.append(firstInput);
    //                buildString.append(followingInput);
    //                //Converts Stringbuilder to String so can pass to methods to compare
    //                String possibleComment = buildString.toString();
    //
    //                if (isItAComment(possibleComment) == true){
    //                    
    //                    String lineComment = "//";
    //                    String startComment = "/*";
    //                    String endComment = "*/";
    //                    
    //                    if(possibleComment.equals(startComment)){
    //                        commentCount++;
    //                    }
    //                    else if(possibleComment.equals(endComment)){
    //                        commentCount--;
    //                    }
    //                    if(commentCount == 0){
    //                        return;
    //                    }
    //                }
    //                if(isItASpecialOperator(possibleComment) == true){
    //                    
    //                }
    //            }
    //        }
    //    }

    /* private static String scanNextSingleInput(String nextSingleInput){
        
            return;
        }*/
    //Checks if the input is a delimeter
    private static boolean isItADelimeter(String input) {
        String plus = "+";
        String minus = "-";
        String div = "/";
        String mul = "*";
        String equals = "=";
        String gThan = ">";
        String lThan = "<";
        String exclamation = "!";
        String lParen = "(";
        String rParen = ")";
        String lBracket = "[";
        String rBracket = "]";
        String lBrace = "{";
        String rBrace = "}";
        String comma = ",";
        String semiColon = ";";

        if (input.equals(plus)) {
            return true;
        } else if (input.equals(minus)) {
            return true;
        } else if (input.equals(div)) {
            return true;
        } else if (input.equals(mul)) {
            return true;
        } else if (input.equals(equals)) {
            return true;
        } else if (input.equals(gThan)) {
            return true;
        } else if (input.equals(lThan)) {
            return true;
        } else if (input.equals(exclamation)) {
            return true;
        } else if (input.equals(lParen)) {
            return true;
        } else if (input.equals(rParen)) {
            return true;
        } else if (input.equals(lBracket)) {
            return true;
        } else if (input.equals(rBracket)) {
            return true;
        } else if (input.equals(lBrace)) {
            return true;
        } else if (input.equals(rBrace)) {
            return true;
        } else if (input.equals(comma)) {
            return true;
        } else if (input.equals(semiColon)) {
            return true;
        } else {
            return false;
        }
    }

    private static boolean isItAComment(String possibleComment) {
        String lineComment = "//";
        String startComment = "/*";
        String endComment = "*/";

        if (possibleComment.equals(lineComment)) {
            return true;
        } else if (possibleComment.equals(startComment)) {
            return true;
        } else if (possibleComment.equals(endComment)) {
            return true;
        } else {
            return false;
        }
    }

    private static boolean isItASpecialOperator(String possibleOperator) {

        String gThanEqual = ">=";
        String lThanEqual = "<=";
        String notEqual = "!=";
        String equivalent = "==";

        if (possibleOperator.equals(gThanEqual)) {
            return true;
        } else if (possibleOperator.equals(lThanEqual)) {
            return true;
        } else if (possibleOperator.equals(notEqual)) {
            return true;
        } else if (possibleOperator.equals(equivalent)) {
            return true;
        } else {
            return false;
        }
    }

    private static boolean isItAnError(String possibleError) {
        String exclamation = "!";
        if (possibleError.equals(exclamation)) {
            return true;
        } else {
            return false;
        }
    }
}
