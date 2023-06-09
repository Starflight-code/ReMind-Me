namespace reMind_me {
    internal class InterfaceManager {
        List<TaskInstance> taskInstancesPtr;
        public InputManager hid;
        public Settings settings;
        public InterfaceUtils utils;
        private DateManager date = new DateManager();
        private readonly Dictionary<string, int> priorityConverter = new Dictionary<string, int>();
        private readonly Dictionary<string, int> sizeConverter = new Dictionary<string, int>();
        private uint lastIdentifier;

        public InterfaceManager(List<TaskInstance> mainTaskInstances, Settings settings) {
            taskInstancesPtr = mainTaskInstances; // creates a pointer to mainTaskInstance

            // sets up input manager and interface manager with current settings
            hid = new InputManager(settings);
            this.settings = settings;
            utils = new InterfaceUtils(settings);

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
        public string GenerateTaskString(int index) {
            return new string($"{taskInstancesPtr[index].GetName()} | Size: {taskInstancesPtr[index].GetUiSize()} | " +
                    $"Priority: {taskInstancesPtr[index].GetUiPriority()} | Due Date: {taskInstancesPtr[index].GetUiDueDate()}");
        }

        public void PrintTasks() {
            for (int i = 0; i < taskInstancesPtr.Count; i++) {

                // taskName | Size: Small | Priority: Low | Due Date: 1/1/2020
                utils.WriteLine($"{GenerateTaskString(i)}", 3, true);
                Console.WriteLine();
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
            utils.WriteLine("HELP: Get a list of available commands and aliases", 5);
        }
        public void EditTaskUI(TaskInstance task) {
            Console.Clear();
            String[] toWrite = {task.GetName(),
                    "Size: " + task.GetUiSize(),
                    "Priority: " + task.GetUiPriority(),
                    "Due: " + task.GetUiDueDate()
            };
            utils.WriteAllLines(toWrite, 10);
        }

        public void AcceptCommands() {
            string name;
            bool foundName;
            Algorithms algo = new Algorithms();

            switch (hid.currentUserInput) {

                case InputManager.UserInput.addTask:
                    Console.Clear();
                    name = hid.utils.InputSanitizer("What is the name of this task", "?", true);
                    Console.Clear();

                    utils.WriteLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                    string size = hid.utils.AskQuestion("What is the size of this task", "?", algo.checkTaskSize);

                    Console.Clear();

                    utils.WriteLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                    string priority = hid.utils.AskQuestion("What is the priority of this task", "?", algo.checkTaskPriority);

                    Console.Clear();

                    string dueDate = hid.utils.AskQuestion("What is the due date for this task", "?", algo.checkDueDate);
                    DateTime dueDateObject = DateTime.Parse(dueDate);

                    Console.Clear();
                    int sizeValue;
                    int priorityValue;
                    sizeConverter.TryGetValue(size, out sizeValue);
                    priorityConverter.TryGetValue(priority, out priorityValue);
                    taskInstancesPtr.Add(new TaskInstance(name, sizeValue, priorityValue, dueDateObject, lastIdentifier + 1));

                    utils.WriteLine("Thank you, your task has been added!", 2);
                    break;

                case InputManager.UserInput.removeTask:
                    Console.Clear();
                    PrintTasks();
                    name = hid.utils.InputSanitizer("What is the name of this task you'd like to remove", "?", true);
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
                        utils.WriteLine($"Task '{name}' removed successfully!", 5);
                    } else {
                        utils.WriteLine($"Task '{name}' not found. Removal failed!", 5);
                    }
                    break;

                case InputManager.UserInput.exitProgram:
                    System.Environment.Exit(0);
                    break;

                case InputManager.UserInput.editTask:
                    Console.Clear();
                    PrintTasks();
                    Console.WriteLine();

                    name = hid.utils.InputSanitizer("What is the name of this task you'd like to edit", "?", true);
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
                    utils.WriteLine("Task selected (", 10);
                    utils.WriteLine(GenerateTaskString(index) + ")", 10);
                    Console.WriteLine();

                    utils.WriteAllLines(new string[] {
                    "1. Edit Task Name",
                    "2. Edit Task Size",
                    "3. Edit Task Priority",
                    "4. Edit Task Due Date"
                    }, 10);
                    Console.WriteLine();

                    string? input = hid.utils.AskQuestion("Which task component would you like to edit", "?", algo.getIntegerValueOneToFour);
                    int editRequested = int.Parse(input);
                    Console.WriteLine();

                    switch (editRequested) {
                        case 1:
                            name = hid.utils.InputSanitizer("What is the new name of this task", "?", true);

                            taskInstancesPtr[index].SetName(name);
                            Console.Clear();
                            break;
                        case 2:
                            utils.WriteLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                            size = hid.utils.AskQuestion("What is the new size of this task", "?", algo.checkTaskSize);

                            sizeConverter.TryGetValue(size, out sizeValue);
                            taskInstancesPtr[index].SetSize(sizeValue);
                            break;
                        case 3:
                            utils.WriteLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                            priority = hid.utils.AskQuestion("What is the new priority of this task", "?", algo.checkTaskPriority);

                            priorityConverter.TryGetValue(priority, out priorityValue);
                            taskInstancesPtr[index].SetPriority(priorityValue);
                            break;
                        case 4:
                            dueDate = hid.utils.AskQuestion("What is the new due date for this task", "?", algo.checkDueDate);
                            dueDateObject = DateTime.Parse(dueDate);

                            taskInstancesPtr[index].SetDueDate(dueDateObject);
                            break;
                    }


                    break;

                case InputManager.UserInput.listCommands:
                    Console.Clear();
                    List<InputManager.command> listOfCommands = hid.FetchCommandList(); // list of command struct objects
                    hid.utils.WriteLine("Command List", 5);
                    Console.WriteLine("\n");

                    for (int i = 0; i < listOfCommands.Count(); i++) {
                        hid.utils.WriteLine($"Command: {listOfCommands[i].commandName}", 5);
                        for (int j = 0; j < (hid.GetMaxCommandLength() - listOfCommands[i].commandName.Length); j++) {
                            Console.Write(" ");
                        }

                        Console.Write(" | ");
                        hid.utils.WriteLine("Aliases: ", 5);

                        for (int j = 0; j < listOfCommands[i].aliases.Count(); j++) {
                            hid.utils.WriteLine($"\"{listOfCommands[i].aliases[j]}\" ", 5);
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
