using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace ZDSProjekt {
    public class Program {
        public static void Main(string[] args) {
            while (true) {
                Console.Clear();

                Console.Write("Enter number 1: ");
                double number1 = Convert.ToDouble(Console.ReadLine());
                Console.Write("Enter number 2: ");
                double number2 = Convert.ToDouble(Console.ReadLine());
                Console.Write("Enter m: ");
                int m = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter f: ");
                int f = Convert.ToInt32(Console.ReadLine());
                int bitCount = m + f;

                int INT1 = (int)Math.Round(number1 * Math.Pow(2, f));
                int INT2 = (int)Math.Round(number2 * Math.Pow(2, f));

                BitArray bits1 = new BitArray(new int[] { Math.Abs(INT1) });
                int[] bitValues1 = bits1.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray().Take(bitCount).ToArray();
                BitArray bits2 = new BitArray(new int[] { Math.Abs(INT2) });
                int[] bitValues2 = bits2.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray().Take(bitCount).ToArray();

                Console.WriteLine($"Number 1: {number1} in Q{m}.{f} = HEX: {HEX4(bitValues1, number1 < 0)}");
                Console.WriteLine($"Number 2: {number2} in Q{m}.{f} = HEX: {HEX4(bitValues2, number2 < 0)}");

                int add = INT1 + INT2;
                BitArray addBits = new BitArray(new int[] { Math.Abs(add) });
                int[] addBitValues = addBits.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray().Take(bitCount).ToArray();

                int sub = INT1 - INT2;
                BitArray subBits = new BitArray(new int[] { Math.Abs(sub) });
                int[] subBitValues = subBits.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray().Take(bitCount).ToArray();

                Console.WriteLine($"Number 1 + Number2 = HEX: {HEX4(addBitValues, add < 0)}\tDEC: {Round(add * Math.Pow(2, -f), 2)}");
                Console.WriteLine($"Number 1 - Number2 = HEX: {HEX4(subBitValues, sub < 0)}\tDEC: {Round(sub * Math.Pow(2, -f), 2)}");

                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static string HEX4(int[] _bits, bool negative) {
            string hex = "";
            int[] bits = new int[16];

            for (int i = 0; i < 16; i++) {
                if (i < _bits.Length) {
                    bits[i] = _bits[i];
                } else {
                    bits[i] = 0;
                }
            }

            if (negative) {
                for (int i = 0; i < bits.Length; i++) {
                    bits[i] = (bits[i] == 0) ? 1 : 0;
                }

                int carry = 1;
                for(int i = 0; i < bits.Length; i++) {
                    if (bits[i] == 0 && carry == 1) {
                        bits[i] = 1;
                        carry = 0;
                    } else if (bits[i] == 1 && carry == 1){
                        bits[i] = 0;
                    }

                    if (carry == 0) {
                        break;
                    }
                }
            }

            int value = 0;
            for (int i = 0; i < 16; i++) {
                int index = i - ((i / 4) * 4);

                value += bits[i] * (int)Math.Pow(2, index);

                if (index == 3 || i == bits.Length - 1) {
                    hex += value.ToString("X1");
                    value = 0;
                }
            }

            return new string(hex.Reverse().ToArray());
        }
        public static double Round(double value, int precision) {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }
    }
}