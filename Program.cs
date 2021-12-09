using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace ZDS_Projekt2_Solver {
    public class Program {
        static double number1, number2;
        static int Qm, Qf;
        static int INT1, INT2;

        public static void Main(string[] args) {
            bool quit = false;
            
            while (!quit) {
                try {
                    Console.Clear();

                    GetInputs();

                    Console.WriteLine();

                    // Printing wanted information
                    PrintHexValues();
                    PrintAdditionSubtraction();
                } catch (Exception e) {
                    Console.WriteLine($"Error: {e.Message}");
                }

                // Stopping user input
                Console.Write("\nPress any key to continue (q to quit)...");
                switch (Console.ReadKey().Key) {
                    case ConsoleKey.Q: {
                        quit = true;
                    } break;
                }
            }
        }

        private static string HEX4(bool[] _bits, bool negative) {
            string hex = "0x";
            bool[] bits = new bool[16];

            // Copying the bits into a 16 bit variable (to fit 4 HEX values)
            Array.Fill(bits, false);
            Array.Copy(_bits, bits, _bits.Length);

            // If the input value was negative we have to get the Two's complement
            if (negative) {
                // Inverting all bits
                for (int i = 0; i < bits.Length; i++) {
                    bits[i] = !bits[i];
                }

                // Adding 1
                int carry = 1;
                for(int i = 0; i < bits.Length; i++) {
                    if (carry == 1) {
                        if (!bits[i]) {
                            carry = 0;
                        }

                        bits[i] = !bits[i];
                    } else {
                        break;
                    }
                }
            }

            // Adding every 4 bits into a single HEX value
            int value = 0;
            for (int i = bits.Length - 1; i >= 0; i--) {
                // Getting the index of the current bit
                int index = i - ((i / 4) * 4);

                // Adding the current bit value to the number
                if (bits[i]) {
                    value += (int)Math.Pow(2, index);
                }

                // If we hit the last bit (000x) then add the value into the HEX and reset the counter
                if (index == 0 || i == 0) {
                    hex += value.ToString("X1");
                    value = 0;
                }
            }

            return hex;
        }

        private static void GetInputs() {
            // Getting numbers 
            Console.Write("Enter number 1: ");
            number1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter number 2: ");
            number2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine();

            // Getting format
            Console.WriteLine("Enter the Qm.f format");

            Console.Write("Enter m: ");
            Qm = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter f: ");
            Qf = Convert.ToInt32(Console.ReadLine());

            // Getting INTEGER format of the floating point number
            INT1 = (int)Math.Round(number1 * Math.Pow(2, Qf));
            INT2 = (int)Math.Round(number2 * Math.Pow(2, Qf));
        }
        
        private static void PrintHexValues() {
            int bitCount = Qm + Qf;

            // Converting the INTEGERs to binary
            BitArray bits1 = new(new int[] { Math.Abs(INT1) });
            BitArray bits2 = new(new int[] { Math.Abs(INT2) });
            bool[] bitValues1 = bits1.Cast<bool>().Select(bit => bit).ToArray().Take(bitCount).ToArray();
            bool[] bitValues2 = bits2.Cast<bool>().Select(bit => bit).ToArray().Take(bitCount).ToArray();

            // Printing out HEX values
            Console.WriteLine($"Number 1\t\t= {HEX4(bitValues1, number1 < 0)}");
            Console.WriteLine($"Number 2\t\t= {HEX4(bitValues2, number2 < 0)}");
        }

        private static void PrintAdditionSubtraction() {
            int bitCount = Qm + Qf;

            // Adding and subtracting the values
            int add = INT1 + INT2;
            int sub = INT1 - INT2;

            // Converting the operations to binary
            BitArray addBits = new(new int[] { Math.Abs(add) });
            BitArray subBits = new(new int[] { Math.Abs(sub) });
            bool[] addBitValues = addBits.Cast<bool>().Select(bit => bit).ToArray().Take(bitCount).ToArray();
            bool[] subBitValues = subBits.Cast<bool>().Select(bit => bit).ToArray().Take(bitCount).ToArray();

            // Printing out DEC and HEX values
            double addReal = Math.Round(add * Math.Pow(2, -f), 2, MidpointRounding.AwayFromZero);
            double subReal = Math.Round(sub * Math.Pow(2, -f), 2, MidpointRounding.AwayFromZero);
            Console.WriteLine($"Number 1 + Number2\t= {HEX4(addBitValues, add < 0)}, {addReal}");
            Console.WriteLine($"Number 1 - Number2\t= {HEX4(subBitValues, sub < 0)}, {subReal}");
        }
    }
}