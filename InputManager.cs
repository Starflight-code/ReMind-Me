namespace reMind_me {
    internal class InputManager {

        public InterfaceUtils utils;
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
            utils = new InterfaceUtils(settings);
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
        private void HandleInput(string input) {
            UserInput returnVal;
            bool check = userInputToEnum.TryGetValue(input.ToLower(), out returnVal);
            if (!check) {
                currentUserInput = UserInput.ERROR;
            }
            currentUserInput = returnVal;
        }

        public string GetUserInput() {
            Console.WriteLine("\n");
            utils.WriteLine("ReMind\\> ", 5);
            string? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.WriteLine("\n");
                utils.WriteLine("Invalid Input Detected. Try again...", 5);
                utils.WriteLine("ReMind\\> ", 5);
                input = Console.ReadLine();
            }
            HandleInput(input);
            return input;
        }
    }
}
