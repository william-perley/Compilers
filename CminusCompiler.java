/*William Perley n00636615
Cminus Compiler part 1, Lexical Analyzer
This is meant to parse the input and tokenize
the inputs.
 */

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.Arrays;

public class CminusCompiler {

    public static void main(String[] args) {
        //Will return if there are no arguments
        if (args.length < 1) {
            System.out.println("No file name found");
            return;
        }

        String fileName = args[0];
        Compiler compiler = new Compiler(fileName);
        compiler.processFile();
    }
}

class Compiler {
	
	private String fileName;
	private int commentCount;
	
	public Compiler(String fileName) {
		
		this.fileName = fileName;
	}
	
	public void processFile() {
		
        BufferedReader bufferedReader = null;
        String nextLine;
        try {
            bufferedReader = new BufferedReader(new FileReader(fileName));
            
            while ((nextLine = bufferedReader.readLine()) != null) {
            	processLine(nextLine);
            }
        } catch (Exception e) {
        	
        }
	}
	
	private void processLine(String line) {
		
		String charSequence = null;
		String errorSequence = null;
		
		if (!line.isEmpty()) {
			System.out.println("INPUT: " + line);
		}
		
		for (int index = 0; index < line.length(); index++) {
			
			char c = line.charAt(index);
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
					if (charSequence == null) {
						charSequence = String.valueOf(c);
					} else {
						charSequence += String.valueOf(c);
					}
					continue;
				} else if (charSequence != null) {
					if (isKeyword(charSequence)) {
						System.out.println("keyword: " + charSequence);
					} else {
						System.out.println("ID: " + charSequence);
					}
					charSequence = null;
				}
				
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
					System.out.println(c);
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
	}
	
	private boolean isDelimeter(char possibleDelimeter) {

        Character[] delimeters = new Character[]
                {'+', '-', '/', '*',
                 '=', '>', '<', '(', 
                 ')', '[', ']', '{', 
                 '}', ',', ';'};

        return Arrays.asList(delimeters).contains(possibleDelimeter);
    }
	
	private boolean isSpecialOperator(String possibleOperator) {

		String[] specialOperators = new String[] {">=", "<=", "!=", "=="};
		
        return Arrays.asList(specialOperators).contains(possibleOperator);
    }
	
	private boolean isLetter(char possibleLetter) {
		
		return Character.isLetter(possibleLetter);
	}
	
	private boolean isKeyword(String possibleKeyword) {
		
		String[] keywords =  new String[] {"int", "void", "else", "return", "if", "while", "float"};
		
		return Arrays.asList(keywords).contains(possibleKeyword);
	}
	
	private boolean shouldStopError(char possibleStopper) {
		
		return isDelimeter(possibleStopper) || possibleStopper == ' ';
	}
}
