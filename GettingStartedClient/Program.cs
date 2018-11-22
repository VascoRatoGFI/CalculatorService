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
            double ans = 0;
            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.Write("CalculatorService> ");
                input = Console.ReadLine();
                //remove uppercase letters and whitespaces from user input
                input = Regex.Replace(input.ToLower(), @"\s", "");
                switch (input)
                {
                    case "":
                        break;
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
                    case "kappa":
                        Console.WriteLine(InputHandler.Kappa());
                        break;
                    default:
                        try
                        {
                            ans = InputHandler.ParseInput(input, ans, client);
                            Console.WriteLine("ans = " + ans.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("  Error: " + e.Message);
                        }
                        break;
                }
            }
            
            client.Close();
        }
    }
}