//William Perley n00636615

//Cminus Compiler part 1, Lexical Analyzer

//This is meant to parse the input and tokenize

//the inputs.

//

import java.io.*;
import java.util.Arrays;


public class CminusCompiler {

    public static void main(String[] args) {
        //Will return if there are no arguments
        if (args.length < 1) {
            System.out.println("No file name found");
            return;
        }

        String fileName = args[0];
        LexicalAnalyzer lexical = new LexicalAnalyzer(fileName);
        lexical.processFile();
    }
}

class LexicalAnalyzer {

    private String fileName;
    private int commentCount;

    public LexicalAnalyzer(String fileName) {

        this.fileName = fileName;
    }

    public void processFile() {
        BufferedWriter writer = null;
        BufferedReader bufferedReader = null;
        String nextLine;
        try {
            bufferedReader = new BufferedReader(new FileReader(fileName));
            writer = new BufferedWriter(new OutputStreamWriter(
                    new FileOutputStream("tokens.txt"), "utf-8"));
            while ((nextLine = bufferedReader.readLine()) != null) {
                processLine(nextLine, writer);
            }
        } catch (Exception e) {

        }
        try{
            writer.close();
        }
        catch (IOException e){

        }
    }

    private void processLine(String line, BufferedWriter writer) {

        String charSequence = null;
        String errorSequence = null;
        String numSequence = null;

//        if (!line.isEmpty()) {
//            System.out.println("INPUT: " + line);
//        }

        for (int index = 0; index < line.length(); index++) {

            char b;
            char c = line.charAt(index);
            if (index > 0) {
                b = line.charAt(index - 1);

            } else {
                b = 'a';
            }
            boolean isLastChar = index == line.length() - 1;

            if (errorSequence != null) {
                if (shouldStopError(c)) {
                    System.out.println("Error: " + errorSequence);
                    errorSequence = null;
                } else {
                    errorSequence += String.valueOf(c);
                    continue;
                }
            }

            if (commentCount == 0) {

                // Check if the character is the beginning of a comment
                if (c == '/') {
                    if (!isLastChar) {
                        char nextChar = line.charAt(index + 1);
                        if (nextChar == '/') {
                            return;
                        } else if (nextChar == '*') {
                            commentCount++;
                            index++;
                            continue;
                        }
                    }
                }

                // Check if the character is part of a character sequence
                if (isLetter(c)) {
                    if(numSequence != null){
                        printNumber(numSequence, writer);
                        numSequence = null;
                    }
                    if (charSequence == null) {
                        charSequence = String.valueOf(c);
                    } else {
                        charSequence += String.valueOf(c);
                    }
                    continue;
                } else if ((charSequence != null) && (isNumber(c) == true)) {
                    printCharSequence(charSequence, writer);
                    charSequence = null;
                    continue;
                } else if (charSequence != null) {
                    printCharSequence(charSequence, writer);
                    charSequence = null;
                }else if (isNumber(c)) {
                    if(charSequence != null){
                        printCharSequence(charSequence, writer);
                        charSequence = null;
                    }
                    if (numSequence == null) {
                        numSequence = String.valueOf(c);
                    } else {
                        numSequence += String.valueOf(c);
                    }
                    continue;
                } else if (((c == '-') || (c == '+')) && (b == 'E')) {
                    numSequence += String.valueOf(c);
                    continue;
                } else if (numSequence != null) {
                    printNumber(numSequence, writer);

                    numSequence = null;

                }

//                else if ((c == '-') && (b == 'E')){
//                    numSequence += String.valueOf(c);
//                }
                // Check if the character is part of a special operator
                if (!isLastChar) {
                    char nextChar = line.charAt(index + 1);
                    String possibleOperator = new StringBuilder().append(c).append(nextChar).toString();
                    if (isSpecialOperator(possibleOperator)) {
                        System.out.println(possibleOperator);
                        index++;
                        continue;
                    }
                }

                // Check if the character is a delimeter
                if (isDelimeter(c)) {
                    //System.out.println(c);
                    try{writer.write(c +"\n");
                        writer.newLine();
                    }
                    catch (IOException e){

                    }
                    continue;
                }

                // Ignore spaces
                if (c == ' ') {
                    continue;
                }

                // We have now encountered an error
                errorSequence = String.valueOf(c);
            } else {
                // Check if the character is the start or end of a comment
                if (!isLastChar) {
                    char nextChar = line.charAt(index + 1);
                    if (c == '/' && nextChar == '*') {
                        commentCount++;
                        index++;
                    } else if (c == '*' && nextChar == '/') {
                        commentCount--;
                        index++;
                    }
                }
            }
        }

        if (errorSequence != null) {
            System.out.println("Error: " + errorSequence);
        }
        if (charSequence != null) {
            printCharSequence(charSequence, writer);
        }
        if (numSequence != null) {
            printNumber(numSequence, writer);
        }
    }

    private void printCharSequence(String charSequence, BufferedWriter writer) {
        if (isKeyword(charSequence)) {
            //System.out.println("keyword: " + charSequence);
            try{writer.write("keyword " + charSequence +"\n");
                writer.newLine();
            }
            catch (IOException e){

            }
        } else {
            //System.out.println("ID: " + charSequence);
            try{writer.write("id " + charSequence +"\n");
                writer.newLine();
            }
            catch (IOException e){

            }
        }
    }

    private void printNumber(String numSequence, BufferedWriter writer) {
        if (isValidNumber(numSequence)) {
            if (isFloat(numSequence)) {
                //System.out.println("Float: " + numSequence);
                try{writer.write("float " + numSequence);
                    writer.newLine();
                }
                catch (IOException e){

                }
            } else {
                //System.out.println("Int: " + numSequence);
                try{writer.write("int " + numSequence +"\n");
                    writer.newLine();}
                catch (IOException e){

                }
            }
        } else {
            System.out.println("Error: " + numSequence);
        }
    }

    private boolean isDelimeter(char possibleDelimeter) {

        Character[] delimeters = new Character[]{'+', '-', '/', '*',
                '=', '>', '<', '(',
                ')', '[', ']', '{',
                '}', ',', ';'};

        return Arrays.asList(delimeters).contains(possibleDelimeter);
    }

    private boolean isSpecialOperator(String possibleOperator) {

        String[] specialOperators = new String[]{">=", "<=", "!=", "=="};

        return Arrays.asList(specialOperators).contains(possibleOperator);
    }

    private boolean isLetter(char possibleLetter) {

        return Character.isLowerCase(possibleLetter);
    }

    private boolean isKeyword(String possibleKeyword) {

        String[] keywords = new String[]{"int", "void", "else", "return", "if", "while", "float"};

        return Arrays.asList(keywords).contains(possibleKeyword);
    }

    private boolean shouldStopError(char possibleStopper) {

        return isDelimeter(possibleStopper) || possibleStopper == ' ';
    }

    private boolean isNumber(char possibleNumber) {

        Character[] numbers = new Character[]{'1', '2', '3', '4', '5', '6',
                '7', '8', '9', '0', 'E', '.'};
        return Arrays.asList(numbers).contains(possibleNumber);
    }

    private boolean isValidNumber(String validNumber) {

        CharSequence E = "E";
        CharSequence Period = ".";
        CharSequence Negative = "-";
        int eCounter = 0;
        int periodCounter = 0;

        for (int i = validNumber.length() - 1; i >= 0; i--) {
            if (validNumber.charAt(i) == 'E') {
                eCounter++;
            }
            if (validNumber.charAt(i) == '.') {
                periodCounter++;
            }
        }
        if ((eCounter <= 1) && (periodCounter <= 1)) {
            int en = validNumber.indexOf('E');
            int pn = validNumber.indexOf('.');

            if ((pn >= 0) && (en < 0)) {
                en = validNumber.length() + 1;
            }

            if (pn <= en) {
                if (!(pn == 0) && !(pn == validNumber.length() - 1) && !(en == 0) && !(en == validNumber.length() - 1)) {
                    return true;
                }
            }else {
                return false;
            }
        } else {
            return false;
        }
        return false;
    }

    private boolean isFloat(String floatNumber) {

        CharSequence E = "E";
        CharSequence Period = ".";

        if (floatNumber.contains(E)) {
            return true;
        } else if (floatNumber.contains(Period)) {
            return true;
        } else {
            return false;
        }
    }
}

