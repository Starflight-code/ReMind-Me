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

        public struct command {
            public string commandName;
            public List<string> aliases;
            public command(string command) {
                commandName = command;
                aliases = new List<string>();
            }
        }
        public Settings settings;
        public UserInput currentUserInput;
        // dictionary converting commands to enums, allowing them to be processed more easily
        Dictionary<string, UserInput> userInputToEnum = new Dictionary<string, UserInput>();

        // Sublist = {Master Command, Alias1, Alias2}
        List<command> masterCommandsAndAliases = new List<command>();

        int maxCommandLength = 0;

        public void addCommand(string command, UserInput mappedTo) {
            masterCommandsAndAliases.Add(new command(command));
            userInputToEnum.Add(command.Trim().ToLower(), mappedTo);
            if (maxCommandLength < command.Trim().Length) {
                maxCommandLength = command.Trim().Length;
            }
        }

        public int GetMaxCommandLength() {
            return maxCommandLength;
        }
        public void AddAlias(string alias, string toCommand) {
            UserInput mappedTo;
            bool found = false;
            bool result = userInputToEnum.TryGetValue(toCommand.Trim().ToLower(), out mappedTo);
            for (int i = 0; i < masterCommandsAndAliases.Count(); i++) { // finds the sublist for the master command and appends the alias to it
                if (masterCommandsAndAliases[i].commandName == toCommand.Trim().ToLower()) {
                    found = true;
                    masterCommandsAndAliases[i].aliases.Add(alias);
                    break;
                }
            }
            if (found == false) {
                return;
            }
            userInputToEnum.Add(alias.Trim().ToLower(), mappedTo);
        }

        public List<command> FetchCommandList() {
            return masterCommandsAndAliases;
        }

        public InputManager(Settings settings) {
            this.settings = settings;
            addCommand("add", UserInput.addTask);
            AddAlias("a", "add");
            addCommand("remove", UserInput.removeTask);
            AddAlias("r", "remove");
            addCommand("exit", UserInput.exitProgram);
            AddAlias("e", "exit");
            addCommand("edit", UserInput.editTask);
            addCommand("help", UserInput.listCommands);
            AddAlias("h", "help");
        }

        /// Updates current settings to a new instance of the settings class
        public void UpdateSettings(Settings settings) {
            this.settings = settings;
        }

        public void WriteLine(string x, int timeBetween, bool saveCPU = false) {
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
        private void HandleInput(string input) {
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
        public string AskQuestion(string question, string ending, Func<string, bool> approvalCondition) {
            string? output = null;
            output = InputSanitizer(question, ending);
            while (!approvalCondition(output)) {
                Console.WriteLine();
                WriteLine("We've detected an error with your input. Try again...", 5);
                output = InputSanitizer(question, ending);
            }
            return output;
        }

        public string GetUserInput() {
            Console.WriteLine("\n");
            WriteLine("ReMind\\> ", 5);
            string? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.WriteLine("\n");
                WriteLine("Invalid Input Detected. Try again...", 5);
                WriteLine("ReMind\\> ", 5);
                input = Console.ReadLine();
            }
            HandleInput(input);
            return input;
        }
        public string InputSanitizer(string prompt, string ending = ":", bool skipSpacing = false)
        /** @param prompt is the question, [prompt]: <User places input here>
         * @param ending is what ends the question. prompt[: ] <input>
         * @param spacing 0 to leave a newline before and after question
         *                1 to skip the first, 2 to skip the last, 3 to skip both
         */
        {
            if (!skipSpacing) {
                Console.WriteLine();
            }
            WriteLine(prompt + ending + " ", 5); // place newline in if spacing 
            string? input = Console.ReadLine();
            while (input == "" || input == null) {
                Console.Write("\nOops, your input appears to have been invalid.\n" + prompt + ending);
                input = Console.ReadLine();
            }
            return input.Trim();
        }
    }
}
