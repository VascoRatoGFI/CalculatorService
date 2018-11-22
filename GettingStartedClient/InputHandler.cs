//using GettingStartedLib;
using GettingStartedClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GettingStartedClient
{
    static class InputHandler
    {
        internal static double ParseInput(string input, double ans, ICalculator client)
        {
            double result = 0;
            while(input.Contains('('))
            {
                input = DoParenthesesFirst(input, ans, client);
            }
            string[] parser = { "" };
            
            string operations = "+-*/%";
            //if (operations.Any(c => input.Contains(c)))
            //{
                parser = Regex.Split(input, @"([*\-+/%]|ans)", new RegexOptions() { });
                char operation = ' ';

                double operand;
                foreach (string partial in parser.Where(s => s != String.Empty))
                {
                    if (operations.Any(c => partial.Contains(c)))
                    {
                        operation = partial[0];
                    }
                    else
                    {
                        if (partial.Equals("ans"))
                        {
                            operand = ans;
                        }
                        else
                        {
                            operand = double.Parse(partial);
                        }
                        switch (operation)
                        {
                            case ' ':
                                result = operand;
                                break;
                            case '+':
                                result = client.Add(result, operand);
                                break;
                            case '-':
                                result = client.Subtract(result, operand);
                                break;
                            case '*':
                                result = client.Multiply(result, operand);
                                break;
                            case '/':
                                result = client.Divide(result, operand);
                                break;
                            case '%':
                                result = client.Modulus(result, operand);
                                break;
                            default:
                                throw new Exception("Unknown operation: '" + operation + "'");
                        }
                    }
                }     
                ans = result;
            //}
            //else
            //{
            //    throw new Exception("Invalid syntax! >:( ");
            //}
            
            return result;
        }
        private static string DoParenthesesFirst(string input, double ans, ICalculator client)
        {
            int start = -1;
            int end = -1;
            int depth = 0;
            string beforeParentheses = "";
            string insideParentheses = "";
            string afterParentheses = "";
            for(int i=0;i<input.Length;i++)
            {  
                if (input[i] == '(')
                {
                    depth++;
                    if (start < 0)
                    {
                        start = i+1;
                    }
                } 
                if (input[i] == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        end = i;
                        beforeParentheses = input.Substring(0, start - 1);
                        insideParentheses = input.Substring(start, end - start);
                        if(end+1 < input.Length)
                            afterParentheses = input.Substring(end + 1);
                        break;
                    }
                }
            }
            if (depth != 0)
            {
                throw new Exception("Unbalanced parentheses.");
            }
            if (string.IsNullOrEmpty(insideParentheses))
            {
                throw new Exception("Empty parentheses.");
            }
            insideParentheses = ParseInput(insideParentheses, ans, client).ToString();
            return beforeParentheses+insideParentheses+afterParentheses;
        }

        internal static string Kappa()
        {
            return @"░░░░░░░░░
░░░░▄▀▀▀▀▀█▀▄▄▄▄░░░░
░░▄▀▒▓▒▓▓▒▓▒▒▓▒▓▀▄░░
▄▀▒▒▓▒▓▒▒▓▒▓▒▓▓▒▒▓█░
█▓▒▓▒▓▒▓▓▓░░░░░░▓▓█░
█▓▓▓▓▓▒▓▒░░░░░░░░▓█░
▓▓▓▓▓▒░░░░░░░░░░░░█░
▓▓▓▓░░░░▄▄▄▄░░░▄█▄▀░
░▀▄▓░░▒▀▓▓▒▒░░█▓▒▒░░
▀▄░░░░░░░░░░░░▀▄▒▒█░
░▀░▀░░░░░▒▒▀▄▄▒▀▒▒█░
░░▀░░░░░░▒▄▄▒▄▄▄▒▒█░
 ░░░▀▄▄▒▒░░░░▀▀▒▒▄▀░░
░░░░░▀█▄▒▒░░░░▒▄▀░░░
░░░░░░░░▀▀█▄▄▄▄▀";
        }
    }
}
