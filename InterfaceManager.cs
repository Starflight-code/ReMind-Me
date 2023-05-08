using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reMind_me {
    internal class Interface_Manager {
        public Interface_Manager() { }
        public void writeAllLines(string[] x) {
            for (int i = 0; i < x.Length; i++) {
                Console.WriteLine(x[i]);
            }
        }
        public void writeAllLines(string[] x, int timeBetween, bool saveCPU = false) {
            for (int i = 0; i < x.Length; i++) {
                if (saveCPU) {
                    string[] words = x[i].Split(' ');
                    for (int j = 0; j < words.Length; j++) {
                        if (j != (words.Length - 1)) {
                            Console.Write(words[j] + " ");
                        } else {
                            Console.Write(words[j]);
                        }
                        Thread.Sleep(timeBetween);
                    }
                } else {
                    for (int j = 0; j < x[i].Length; j++) {
                        Console.Write(x[i][j]);
                        Thread.Sleep(timeBetween);
                    }
                }
                Console.WriteLine();
            }
        }
        public void writeLine(string x, int timeBetween, bool saveCPU = false) {
            if (saveCPU) {
                string[] words = x.Split(' ');
                for (int i = 0; i < words.Length; i++) {
                    if (i != (words.Length - 1)) {
                        Console.Write(words[i] + " ");
                    } else {
                        Console.Write(words[i]);
                    }
                    Thread.Sleep(timeBetween);
                }
            } else {
                for (int i = 0; i < x.Length; i++) {
                    Console.Write(x[i]);
                    Thread.Sleep(timeBetween);
                }
            }
        }
        public string inputSanitizer(string prompt, string ending = ": ", bool skipSpacing = false)
        /** @param prompt is the question, [prompt]: <User places input here>
         * @param ending is what ends the question. prompt[: ] <input>
         * @param spacing 0 to leave a newline before and after question
         *                1 to skip the first, 2 to skip the last, 3 to skip both
         */
        {
            Console.Write($"{(skipSpacing ? "" : "\n")}" + prompt + ending); // place newline in if spacing 
            string? input = Console.ReadLine();
            while (input == "" || input == null) {
                Console.Write("\nOops, your input appears to have been invalid.\n" + prompt + ending);
                input = Console.ReadLine();
            }
            return input;
        }
        public void pleaseWait(string waitingFor) {
            string[] waitLines = {
            "Just a moment",
            "Please wait",
            "We're almost there",
            "So close"
            };
            Random rand = new Random();
            Console.WriteLine($"{waitLines[rand.Next(waitLines.Length)]}, {waitingFor}...");
        }
        public void writeManifest(List<List<string>> manifest) {

        }
        /** Size 1: Very Small 2: Small 3: Medium 4: Large 5: Huge
         * Priority 1: None 2: Low 3: Medium 4: High 5: Urgent (Top Priority)
         * 
         */
        public string numberToInterfaceString(char type, int number) { 
            switch(type) {
                case 's': // Size
                    switch (number) {
                        case 1:
                            return "Very Small";
                        case 2:
                            return "Small     ";
                        case 3:
                            return "Medium    ";
                        case 4:
                            return "Large     ";
                        case 5:
                            return "Huge      ";
                    }
                    break;


                case 'p': // Priority
                    switch (number) {
                        case 1:
                            return "None  ";
                        case 2:
                            return "Low   ";
                        case 3:
                            return "Medium";
                        case 4:
                            return "High  ";
                        case 5:
                            return "Urgent";
                    }
                    break;
                default:
                    return "Error";

            }
            return "Error";
        }
    }
}
