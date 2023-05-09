namespace reMind_me {
    internal class InputManager {
        InterfaceManager ui = new InterfaceManager();
        public enum userInput {
            ERROR,
            addTask
        }
        public userInput currentUserInput;
        // dictionary converting commands to enums, allowing them to be processed more easily
        Dictionary<String, userInput> userInputToEnum = new Dictionary<String, userInput>();

        public InputManager() {
            userInputToEnum.Add("add", userInput.addTask);
        }

        private void handleInput(String input) {
            userInput returnVal;
            if (!userInputToEnum.TryGetValue(input.ToLower().Trim(), out returnVal)) {
                currentUserInput = userInput.ERROR;
            }
            currentUserInput = returnVal;
        }

        public String getUserInput() {
            Console.WriteLine("\n");
            ui.writeLine("ReMind\\> ", 5);
            String? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.WriteLine("\n");
                ui.writeLine("Invalid Input Detected. Try again...", 5);
                ui.writeLine("ReMind\\> ", 5);
                input = Console.ReadLine();
            }
            handleInput(input);
            return input;
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
    }
}
