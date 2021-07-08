using System;
using System.Collections.Generic;

namespace MorseCode
{
    class Program
    {
        static void Main(string[] args)
        {
            const int longPressLength = 300; // Number of milliseconds a spacebar should be pressed to count as a long press ("1")
            const int noPressForSpaceLength = 2000; // Number of milliseconds a spacebar should not be pressed to count as a space (" ")
            const int noPressForNextCode = 500; // Number of milliseconds a spacebar should not be pressed to consider a code for a char finished
            int spacePressSecondsCount = 0;

            string code = string.Empty;
            string message = string.Empty;

            bool wasSpacePressedAlready = false;

            DateTime pressStart = DateTime.Now;
            DateTime lastTimeReleased = DateTime.Now;

            Dictionary<string, string> codes = new Dictionary<string, string>
            {
                {"01", "A"},
                {"1000", "B"},
                {"1010", "C"},
                {"100", "D"},
                {"0", "E"},
                {"0010", "F"},
                {"110", "G"},
                {"0000", "H"},
                {"00", "I"},
                {"0111", "J"},
                {"101", "K"},
                {"0100", "L"},
                {"11", "M"},
                {"10", "N"},
                {"111", "O"},
                {"0110", "P"},
                {"1101", "Q"},
                {"010", "R"},
                {"000", "S"},
                {"1", "T"},
                {"001", "U"},
                {"0001", "V"},
                {"011", "W"},
                {"1001", "X"},
                {"1011", "Y"},
                {"1100", "Z"},
                {"01111", "1"},
                {"00111", "2"},
                {"00011", "3"},
                {"00001", "4"},
                {"00000", "5"},
                {"10000", "6"},
                {"11000", "7"},
                {"11100", "8"},
                {"11110", "9"},
                {"11111", "0"},
                {"010101", "."},
                {"110011", ","}
            };

            while (true)
            {
                if (NativeKeyboard.IsKeyDown(KeyCode.Backspace) || message == string.Empty)
                {
                    Console.Clear();
                    PrintInstruction(codes);
                    message = ":"; // : indicates the first char in the message 
                }

                bool isSpacePressed = NativeKeyboard.IsKeyDown(KeyCode.Space);

                // Checking space press time 
                if (isSpacePressed)
                {
                    if (!wasSpacePressedAlready)
                    {
                        pressStart = DateTime.Now;
                        spacePressSecondsCount = 0;
                    }

                    wasSpacePressedAlready = true;
                    lastTimeReleased = DateTime.Now;
                }
                else
                {
                    if (wasSpacePressedAlready)
                    {
                        spacePressSecondsCount = (int)(DateTime.Now - pressStart).TotalMilliseconds;
                        wasSpacePressedAlready = false;
                    }
                }
                
                // 1 for a long press, 0 for a short one
                if (spacePressSecondsCount != 0 && spacePressSecondsCount > longPressLength)
                {
                    spacePressSecondsCount = 0;
                    code += "1";
                }
                else if (spacePressSecondsCount != 0 && spacePressSecondsCount < longPressLength)
                {
                    spacePressSecondsCount = 0;
                    code += "0";
                }
                
                // Add a space (" ") if a spacebar is not pressed for a long time
                int secondsSinceLastRelease = (int)(DateTime.Now - lastTimeReleased).TotalMilliseconds;

                if (secondsSinceLastRelease > noPressForSpaceLength && !message.EndsWith(" ") && message != ":")
                {
                    Console.Write(" ");
                    message += " ";
                }

                // Printing a char
                if (codes.TryGetValue(code, out string letter) && secondsSinceLastRelease > noPressForNextCode)
                {
                    Console.Write(letter);
                    message += letter;
                    code = string.Empty;
                }

                // If a code is not found (in case of a mistake, for example), clear the code
                if (code.Length > 6)
                    code = string.Empty;
            }
        }
        public static void PrintInstruction(Dictionary<string, string> codes)
        {
            const int maxCodesPerLine = 13;
            int codesPerLineCount = 0;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press Backspace to clear");
            Console.WriteLine("Long no press is a space (at least 2 seconds)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("0 is a short Spacebar press");
            Console.WriteLine("1 is a long Spacebar press (at least 1/3 of a second)");
            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (KeyValuePair<string, string> entry in codes)
            {
                Console.Write(entry.Value + "=" + entry.Key + " ");
                codesPerLineCount++;

                if (codesPerLineCount == maxCodesPerLine)
                {
                    Console.WriteLine();
                    codesPerLineCount = 0;
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Or just google it :)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
