namespace reMind_me {
    internal class InputManager {
        public enum UserInput {
            ERROR,
            addTask,
            removeTask,
            exitProgram,
            editTask,
            listCommands
        }
        public Settings settings;
        public UserInput currentUserInput;
        // dictionary converting commands to enums, allowing them to be processed more easily
        Dictionary<string, UserInput> userInputToEnum = new Dictionary<string, UserInput>();

        // Sublist = {Master Command, Alias1, Alias2}
        List<List<string>> masterCommandsAndAliases = new List<List<string>>();

        int maxCommandLength = 0;

        public void addCommand(string command, UserInput mappedTo) {
            masterCommandsAndAliases.Add(new List<string> { command.Trim().ToLower() });
            userInputToEnum.Add(command.Trim().ToLower(), mappedTo);
            if (maxCommandLength < command.Trim().Length) {
                maxCommandLength = command.Trim().Length;
            }
        }

        public int getMaxCommandLength() {
            return maxCommandLength;
        }
        public void addAlias(string alias, string toCommand, UserInput mappedTo) {
            UserInput mappedToFetched;
            bool result = userInputToEnum.TryGetValue(toCommand.Trim().ToLower(), out mappedToFetched);
            if (mappedToFetched == mappedTo) {
                for (int i = 0; i < masterCommandsAndAliases.Count(); i++) { // finds the sublist for the master command and appends the alias to it
                    if (masterCommandsAndAliases[i][0] == toCommand.Trim().ToLower()) {
                        masterCommandsAndAliases[i].Add(alias);
                        break;
                    }
                }
                userInputToEnum.Add(alias.Trim().ToLower(), mappedTo);
            } else {
                // ERROR, if this was executed it means the alias was registered with invalid data
                throw new Exception($"A command was registered incorrectly! Debug: Alias was {alias}, Command was {toCommand}, enum value was {mappedTo}, result of fetch was {result}");
            }
        }

        public List<List<string>> fetchListOfAliasesAndCommands() {
            return masterCommandsAndAliases;
        }

        public InputManager(Settings settings) {
            this.settings = settings;
            addCommand("add", UserInput.addTask);
            addAlias("a", "add", UserInput.addTask);
            addCommand("remove", UserInput.removeTask);
            addAlias("r", "remove", UserInput.removeTask);
            addCommand("exit", UserInput.exitProgram);
            addAlias("e", "exit", UserInput.exitProgram);
            addCommand("edit", UserInput.editTask);
            addCommand("help", UserInput.listCommands);
            addAlias("h", "help", UserInput.listCommands);
        }

        /// Updates current settings to a new instance of the settings class
        public void updateSettings(Settings settings) {
            this.settings = settings;
        }

        public void writeLine(string x, int timeBetween, bool saveCPU = false) {
            if (settings.fastMode) { // disables animation for fast mode
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
        private void handleInput(string input) {
            UserInput returnVal;
            bool check = userInputToEnum.TryGetValue(input.ToLower(), out returnVal);
            if (!check) {
                currentUserInput = UserInput.ERROR;
            }
            currentUserInput = returnVal;
        }

        /** Get user information, ask a question with an ending (Question (What color are your shoes) + Ending (? ) = What color are your shoes?)
         * approvalConditon is a wrapped lambda accepting the string the user gives. This should check the string against some requirements and
         * return a bool. True, string is ready to be returned. False, ask the user again.
         */
        public string askQuestion(string question, string ending, Func<string, bool> approvalCondition) {
            string? output = null;
            output = inputSanitizer(question, ending);
            while (!approvalCondition(output)) {
                Console.WriteLine();
                writeLine("We've detected an error with your input. Try again...", 5);
                output = inputSanitizer(question, ending);
            }
            return output;
        }

        public string getUserInput() {
            Console.WriteLine("\n");
            writeLine("ReMind\\> ", 5);
            string? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.WriteLine("\n");
                writeLine("Invalid Input Detected. Try again...", 5);
                writeLine("ReMind\\> ", 5);
                input = Console.ReadLine();
            }
            handleInput(input);
            return input;
        }
        public string inputSanitizer(string prompt, string ending = ":", bool skipSpacing = false)
        /** @param prompt is the question, [prompt]: <User places input here>
         * @param ending is what ends the question. prompt[: ] <input>
         * @param spacing 0 to leave a newline before and after question
         *                1 to skip the first, 2 to skip the last, 3 to skip both
         */
        {
            if (!skipSpacing) {
                Console.WriteLine();
            }
            writeLine(prompt + ending + " ", 5); // place newline in if spacing 
            string? input = Console.ReadLine();
            while (input == "" || input == null) {
                Console.Write("\nOops, your input appears to have been invalid.\n" + prompt + ending);
                input = Console.ReadLine();
            }
            return input.Trim();
        }
    }
}
