using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace TestApp
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Input the string to find out if it is a good binary string or not. \nOr Press enter to exit without pressing any other key.");
                    Console.WriteLine();
                   string input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        GoodBinary(input);
                    }
                    else break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static bool GoodBinary(string input)
        {
            bool isGoodBinary = false;
            try
            {
                int countOfZeros = 0;
                int countOfOnes = 0;
                foreach (var character in input)
                {
                    if(character != '0' && character != '1')
                    {
                        Console.WriteLine("Input " + input + " is not a binary string. Please enter a valid binary string\n");
                        return isGoodBinary;
                    }
                    if (character == '0')
                        countOfZeros++;
                    else
                        countOfOnes++;
                }

                if (countOfZeros != countOfOnes)
                {
                    isGoodBinary = false;
                }
                else
                {
                    int length = input.Length;
                    List<string> prefixes = new List<string>();
                    for (int counter = 1; counter < length; counter++)
                    {
                        string prefix = input.Substring(0, counter);
                        prefixes.Add(prefix);
                        int countOfZerosInPrefix = 0;
                        int countOfOnesInPrefix = 0;
                        foreach (var character in prefix)
                        {
                            if (character == '0')
                            {
                                countOfZerosInPrefix++;
                            }
                            else
                            {
                                countOfOnesInPrefix++;
                            }
                        }
                        if (countOfZerosInPrefix > countOfOnesInPrefix)
                        {
                            isGoodBinary = false;
                            break;
                        }
                        else
                        {
                            isGoodBinary = true;
                        }
                    }

                    Console.Write("Prefixes of input " + input + " are ");
                    foreach (string prefix in prefixes)
                    {
                        Console.Write(prefix + "|");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (isGoodBinary)
                Console.WriteLine("input " + input + " is Good Binary\n");
            else
                Console.WriteLine("input " + input + " is not a Good Binary\n");
            return isGoodBinary;
        }
    }
}

