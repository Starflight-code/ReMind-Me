namespace reMind_me {
    internal class InterfaceUtils {
        bool fastMode;
        public InterfaceUtils(bool fastMode) {
            this.fastMode = fastMode;
        }

        public void writeAllLines(string[] x) {
            for (int i = 0; i < x.Length; i++) {
                Console.WriteLine(x[i]);
            }
        }
        public void WriteAllLines(string[] x, int timeBetween, bool saveCPU = false) {
            for (int i = 0; i < x.Length; i++) {
                if (saveCPU) { // prints one word at a time instead of characters
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
        public void WriteLine(string x) {
            Console.Write(x);
            return;
        }
        public void WriteLine(string x, int timeBetween, bool saveCPU = false) {
            if (fastMode) { // disables animation for fast mode
                Console.Write(x);
                return;
            }
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

        /** Get user information, ask a question with an ending (Question (What color are your shoes) + Ending (? ) = What color are your shoes?)
         * approvalConditon is a wrapped lambda accepting the string the user gives. This should check the string against some requirements and
         * return a bool. True, string is ready to be returned. False, ask the user again.
         */
        public string AskQuestion(string question, string ending, Func<string, bool> approvalCondition) {
            string? output = null;
            output = InputSanitizer(question, ending, true);
            while (!approvalCondition(output)) {
                Console.WriteLine();
                WriteLine("We've detected an error with your input. Try again...", 5);
                output = InputSanitizer(question, ending);
            }
            return output;
        }

        public string InputSanitizer(string prompt, string ending = ":", bool skipSpacing = false)
        /** @param prompt is the question, [prompt]: <User places input here>
         * @param ending is what ends the question. prompt[: ] <input>
         * @param spacing 0 to leave a newline before and after question
         *                1 to skip the first, 2 to skip the last, 3 to skip both
         */
        {
            if (!skipSpacing) {
                Console.WriteLine(); // place newline in if spacing 
            }
            WriteLine(prompt + ending + " ", 5);
            string? input = Console.ReadLine();
            while (input == "" || input == null) {
                Console.Write("\nOops, your input appears to have been invalid.\n" + prompt + ending);
                input = Console.ReadLine();
            }
            return input.Trim();
        }

        public void UpdateFastMode(bool newFastModeSetting) {
            fastMode = newFastModeSetting;
        }
    }
}
