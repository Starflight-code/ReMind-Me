namespace reMind_me {
    internal class InterfaceManager {
        List<TaskInstance> taskInstancesPtr;
        public InputManager hid = new InputManager();
        private DateManager date = new DateManager();
        private readonly Dictionary<string, int> priorityConverter = new Dictionary<string, int>();
        private readonly Dictionary<string, int> sizeConverter = new Dictionary<string, int>();
        private uint lastIdentifier;

        public InterfaceManager(List<TaskInstance> mainTaskInstances) {
            taskInstancesPtr = mainTaskInstances; // creates a pointer to mainTaskInstance

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

        private void createTask(string taskName, string taskSize, string taskPriority, string taskDueDate) {
            lastIdentifier = taskInstancesPtr[taskInstancesPtr.Count - 1].getID();
            Random rand = new Random();
            int sizeOfTask = -1;
            int priorityOfTask = -1;
            sizeConverter.TryGetValue(taskSize, out sizeOfTask);
            priorityConverter.TryGetValue(taskPriority, out priorityOfTask);
            taskInstancesPtr.Add(new TaskInstance(taskName, sizeOfTask, priorityOfTask, date.fromDatabaseString(taskDueDate), lastIdentifier + 1));
        }
        public void writeAllLines(string[] x) {
            for (int i = 0; i < x.Length; i++) {
                Console.WriteLine(x[i]);
            }
        }
        public void writeAllLines(string[] x, int timeBetween, bool saveCPU = false) {
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

        public void writeMainUI() {
            Console.WriteLine('\n');
            writeLine("ADD: Create a new task", 5);
        }
        public void editTaskUI(TaskInstance task) {
            Console.Clear();
            String[] toWrite = {task.getName(),
                    "Size: " + task.getUiSize(),
                    "Priority: " + task.getUiPriority(),
                    "Due: " + task.getDueDate()
            };
            writeAllLines(toWrite, 10);
        }

        public void printUI() {
            string name;

            switch (hid.currentUserInput) {

                case InputManager.userInput.addTask:
                    Console.Clear();
                    name = hid.inputSanitizer("What is the name of this task", "?", true);
                    Console.Clear();

                    Func<string, bool> checkTaskSize = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "tiny", "small", "medium", "large", "huge" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    writeLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                    string size = hid.askQuestion("What is the size of this task", "?", checkTaskSize);
                    Console.Clear();

                    Func<string, bool> checkTaskPriority = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "none", "low", "medium", "high", "urgent" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    writeLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                    string priority = hid.askQuestion("What is the priority of this task", "?", checkTaskPriority);
                    Console.Clear();

                    Func<string, bool> checkDueDate = (string x) => {
                        return DateTime.TryParse(x, out _);
                    };
                    string dueDate = hid.askQuestion("What is the due date for this task", "?", checkDueDate);
                    DateTime dueDateObject = DateTime.Parse(dueDate);
                    Console.Clear();

                    int sizeValue;
                    int priorityValue;
                    sizeConverter.TryGetValue(size, out sizeValue);
                    priorityConverter.TryGetValue(priority, out priorityValue);
                    taskInstancesPtr.Add(new TaskInstance(name, sizeValue, priorityValue, dueDateObject, lastIdentifier + 1));

                    writeLine("Thank you, your task has been added!", 2);
                    break;

                case InputManager.userInput.removeTask:
                    Console.Clear();
                    name = hid.inputSanitizer("What is the name of this task you'd like to remove", "?", true);
                    bool foundName = false;
                    for (int i = 0; i < taskInstancesPtr.Count(); i++) {
                        if (taskInstancesPtr[i].getName().ToLower() == name.ToLower()) {
                            foundName = true;
                            taskInstancesPtr.RemoveAt(i);
                            break;
                        }
                    }
                    Console.Clear();
                    if (foundName) {
                        writeLine($"Task '{name}' removed successfully!", 5);
                    } else {
                        writeLine($"Task '{name}' not found. Removal failed!", 5);
                    }
                    break;

                case InputManager.userInput.exitProgram:
                    System.Environment.Exit(0);
                    break;

                case InputManager.userInput.editTask:
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Input error, your input was not valid!");
                    break;
            }
        }
    }
}
