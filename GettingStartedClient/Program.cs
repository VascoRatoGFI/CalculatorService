using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GettingStartedClient.ServiceReference1;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            CalculatorClient client = new CalculatorClient();
            
            Console.WriteLine("Write an expression to evaluate or enter 'exit' to quit.\nType 'help' for additional information.");
            string input = "";
            double ans=0;
            bool exitProgram = false;

            string[] validCommands = { "exit", "help", "x", "h"};

            while (!exitProgram)
            {
                Console.Write("CalculatorService> ");
                input = Console.ReadLine();
                //remove uppercase letters and whitespaces from user input
                input = Regex.Replace(input.ToLower(), @"\s", "");

                if (validCommands.Any(s => input.Equals(s)))
                {
                    switch (input)
                    {
                        case "x":
                        case "exit":
                            exitProgram = true;
                            Console.WriteLine("Bye! :)");
                            break;
                        case "h":
                        case "help":
                            Console.WriteLine("Supported commands:");
                            Console.WriteLine("  exit , x : Terminate this application;");
                            Console.WriteLine("  help , h : Show this message.");
                            Console.WriteLine("");
                            Console.WriteLine("Supported operations:");
                            Console.WriteLine("  + : Addition;");
                            Console.WriteLine("  - : Subtraction;");
                            Console.WriteLine("  * : Multiplication;");
                            Console.WriteLine("  / : Division;");
                            Console.WriteLine("  % : Modulus.");
                            Console.WriteLine("");
                            Console.WriteLine("Supported operands:");
                            Console.WriteLine("  Any positive number;");
                            Console.WriteLine("  ans : Previous result. Starts as 0.");
                            Console.WriteLine("");
                            Console.WriteLine("Examples:");
                            Console.WriteLine("  CalculatorService> 1+1");
                            Console.WriteLine("  ans = 2");
                            Console.WriteLine("  CalculatorService> 1-1");
                            Console.WriteLine("  ans = 0");
                            Console.WriteLine("  CalculatorService> 2*3");
                            Console.WriteLine("  ans = 6");
                            Console.WriteLine("  CalculatorService> 6/3");
                            Console.WriteLine("  ans = 2");
                            Console.WriteLine("  CalculatorService> 5%4");
                            Console.WriteLine("  ans = 1");
                            Console.WriteLine("  CalculatorService> ans+1");
                            Console.WriteLine("  ans = 2");
                            Console.WriteLine("");
                            Console.WriteLine("");
                            break;
                        default:
                            Console.WriteLine("Sorry, the command '{0}' hasn't been implemented yet! :(", input);
                            break;
                    }

                }
                else
                {
                    string[] parser = { "" };
                    try
                    {
                        string operations = "+-*/%";
                        if (operations.Any(c => input.Contains(c)))
                        {
                            parser = Regex.Split(input, @"([*\-+/%]|ans)", new RegexOptions() { });
                            //foreach (string partial in parser.Where(s => s != String.Empty))
                            //{
                            //    Console.WriteLine(partial);
                            //}
                            char operation = ' ';
                            double result = 0;
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
                                    //Console.Write("Input: {0}" + operation + "{1}", result, operand);
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
                                    //Console.WriteLine("={0}", result);
                                }
                            }
                            Console.WriteLine("ans = " + result.ToString());
                            ans = result;
                        }
                        else
                        {
                            throw new Exception("Invalid syntax! >:( ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                }
            }
            
            client.Close();
        }
    }
}