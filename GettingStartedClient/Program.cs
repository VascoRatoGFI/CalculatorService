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
            
            Console.WriteLine("Write an expression to evaluate or enter 'exit' to quit.");
            string input = "";
            double ans=0;
            while (input.Contains("exit")==false)
            {
                Console.Write("CalculatorService> ");
                input = Console.ReadLine().ToLower();
                string[] parser = { "" };
                
                try
                {
                    string operations = "+-*/%";
                    if(operations.Any(c=>input.Contains(c)))
                    {
                        parser = Regex.Split(input, @"([*\-+/%]|ans)",new RegexOptions() { });
                        //foreach (string partial in parser.Where(s => s != String.Empty))
                        //{
                        //    Console.WriteLine(partial);
                        //}
                        char operation = ' ';
                        double result = 0;
                        double operand;
                        foreach(string partial in parser.Where(s => s != String.Empty))
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
                        Console.WriteLine("ans = "+result.ToString());
                        ans = result;
                    }
                    else if (input.Contains("exit"))
                    {
                        Console.WriteLine("Bye! :)");
                    }
                    else
                    {
                        throw new Exception("Aww maaan ");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }

            }
            
            client.Close();
        }
    }
}