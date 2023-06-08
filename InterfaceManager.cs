namespace reMind_me {
    internal class InterfaceManager {
        List<TaskInstance> taskInstancesPtr;
        public InputManager hid;
        public Settings settings;
        private DateManager date = new DateManager();
        private readonly Dictionary<string, int> priorityConverter = new Dictionary<string, int>();
        private readonly Dictionary<string, int> sizeConverter = new Dictionary<string, int>();
        private uint lastIdentifier;

        public InterfaceManager(List<TaskInstance> mainTaskInstances, Settings settings) {
            taskInstancesPtr = mainTaskInstances; // creates a pointer to mainTaskInstance

            // sets up input manager and interface manager with current settings
            hid = new InputManager(settings);
            this.settings = settings;

            // populates priority dictionary
            priorityConverter.Add("0", 0);
            priorityConverter.Add("1", 1);
            priorityConverter.Add("2", 2);
            priorityConverter.Add("3", 3);
            priorityConverter.Add("4", 4);
            priorityConverter.Add("none", 0);
            priorityConverter.Add("low", 1);
            priorityConverter.Add("medium", 2);
            priorityConverter.Add("high", 3);
            priorityConverter.Add("urgent", 4);

            // populates size dictionary
            sizeConverter.Add("0", 0);
            sizeConverter.Add("1", 1);
            sizeConverter.Add("2", 2);
            sizeConverter.Add("3", 3);
            sizeConverter.Add("4", 4);
            sizeConverter.Add("tiny", 0);
            sizeConverter.Add("small", 1);
            sizeConverter.Add("medium", 2);
            sizeConverter.Add("large", 3);
            sizeConverter.Add("huge", 4);
        }

        /// updates our and input manager's settings
        public void UpdateSettings(Settings settings) {
            this.settings = settings;
            hid.UpdateSettings(settings);
        }

        private void CreateTask(string taskName, string taskSize, string taskPriority, string taskDueDate) {
            lastIdentifier = taskInstancesPtr[taskInstancesPtr.Count - 1].GetID();
            Random rand = new Random();
            int sizeOfTask = -1;
            int priorityOfTask = -1;
            sizeConverter.TryGetValue(taskSize, out sizeOfTask);
            priorityConverter.TryGetValue(taskPriority, out priorityOfTask);
            taskInstancesPtr.Add(new TaskInstance(taskName, sizeOfTask, priorityOfTask, date.FromDatabaseString(taskDueDate), lastIdentifier + 1));
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
        public string GenerateTaskString(int index) {
            return new string($"{taskInstancesPtr[index].GetName()} | Size: {taskInstancesPtr[index].GetUiSize()} | " +
                    $"Priority: {taskInstancesPtr[index].GetUiPriority()} | Due Date: {taskInstancesPtr[index].GetDueDate()}");
        }

        public void PrintTasks() {
            for (int i = 0; i < taskInstancesPtr.Count; i++) {

                // taskName | Size: Small | Priority: Low | Due Date: 1/1/2020
                WriteLine($"{taskInstancesPtr[i].GetName()} | Size: {taskInstancesPtr[i].GetUiSize()} | " +
                    $"Priority: {taskInstancesPtr[i].GetUiPriority()} | Due Date: {taskInstancesPtr[i].GetDueDate()}", 5);
                Console.WriteLine();
            }
        }
        public void WriteLine(string x, int timeBetween, bool saveCPU = false) {
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
        public void PleaseWait(string waitingFor) {
            string[] waitLines = {
            "Just a moment",
            "Please wait",
            "We're almost there",
            "So close"
            };
            Random rand = new Random();
            Console.WriteLine($"{waitLines[rand.Next(waitLines.Length)]}, {waitingFor}...");
        }

        public void WriteMainUI() {
            Console.WriteLine('\n');
            WriteLine("ADD: Create a new task", 5);
        }
        public void EditTaskUI(TaskInstance task) {
            Console.Clear();
            String[] toWrite = {task.GetName(),
                    "Size: " + task.GetUiSize(),
                    "Priority: " + task.GetUiPriority(),
                    "Due: " + task.GetDueDate()
            };
            WriteAllLines(toWrite, 10);
        }

        public void PrintUI() {
            string name;
            bool foundName;

            switch (hid.currentUserInput) {

                case InputManager.UserInput.addTask:
                    Console.Clear();
                    name = hid.InputSanitizer("What is the name of this task", "?", true);
                    Console.Clear();

                    Func<string, bool> checkTaskSize = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "tiny", "small", "medium", "large", "huge" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    WriteLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                    string size = hid.AskQuestion("What is the size of this task", "?", checkTaskSize);
                    Console.Clear();

                    Func<string, bool> checkTaskPriority = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "none", "low", "medium", "high", "urgent" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    WriteLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                    string priority = hid.AskQuestion("What is the priority of this task", "?", checkTaskPriority);
                    Console.Clear();

                    Func<string, bool> checkDueDate = (string x) => {
                        return DateTime.TryParse(x, out _);
                    };
                    string dueDate = hid.AskQuestion("What is the due date for this task", "?", checkDueDate);
                    DateTime dueDateObject = DateTime.Parse(dueDate);
                    Console.Clear();

                    int sizeValue;
                    int priorityValue;
                    sizeConverter.TryGetValue(size, out sizeValue);
                    priorityConverter.TryGetValue(priority, out priorityValue);
                    taskInstancesPtr.Add(new TaskInstance(name, sizeValue, priorityValue, dueDateObject, lastIdentifier + 1));

                    WriteLine("Thank you, your task has been added!", 2);
                    break;

                case InputManager.UserInput.removeTask:
                    Console.Clear();
                    PrintTasks();
                    name = hid.InputSanitizer("What is the name of this task you'd like to remove", "?", true);
                    foundName = false;
                    for (int i = 0; i < taskInstancesPtr.Count(); i++) {
                        if (taskInstancesPtr[i].GetName().ToLower() == name.ToLower()) {
                            foundName = true;
                            taskInstancesPtr.RemoveAt(i);
                            break;
                        }
                    }
                    Console.Clear();
                    if (foundName) {
                        WriteLine($"Task '{name}' removed successfully!", 5);
                    } else {
                        WriteLine($"Task '{name}' not found. Removal failed!", 5);
                    }
                    break;

                case InputManager.UserInput.exitProgram:
                    System.Environment.Exit(0);
                    break;

                case InputManager.UserInput.editTask:
                    Console.Clear();
                    PrintTasks();
                    Console.WriteLine();

                    name = hid.InputSanitizer("What is the name of this task you'd like to edit", "?", true);
                    foundName = false;
                    int index = 0;
                    for (int i = 0; i < taskInstancesPtr.Count(); i++) {
                        if (taskInstancesPtr[i].GetName().ToLower() == name.ToLower()) {
                            foundName = true;
                            index = i;
                            break;
                        }
                    }

                    if (foundName == false) {
                        return;
                    }

                    // show "Task selected (taskName | Size: Medium | Priority: Urgent | Due Date: 1/1/2000 1:00:00 AM)" in console, with specific task info substituted
                    Console.WriteLine();
                    WriteLine("Task selected (", 10);
                    WriteLine(GenerateTaskString(index) + ")", 10);
                    Console.WriteLine();



                    break;

                case InputManager.UserInput.listCommands:
                    Console.Clear();
                    List<List<string>> listOfCommands = hid.FetchCommandList();
                    hid.WriteLine("Command List", 5);
                    Console.WriteLine("\n");

                    for (int i = 0; i < listOfCommands.Count(); i++) {
                        hid.WriteLine($"Command: {listOfCommands[i][0]}", 5);
                        for (int j = 0; j < (hid.GetMaxCommandLength() - listOfCommands[i][0].Length); j++) {
                            Console.Write(" ");
                        }

                        Console.Write(" | ");
                        hid.WriteLine("Aliases: ", 5);

                        for (int j = 1; j < listOfCommands[i].Count(); j++) {
                            hid.WriteLine("\"" + listOfCommands[i][j] + "\" ", 5);
                        }

                        Console.WriteLine();
                    }

                    break;


                default:
                    Console.Clear();
                    Console.WriteLine("Input error, your input was not valid!");
                    break;
            }
        }
    }
}
