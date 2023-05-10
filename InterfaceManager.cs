namespace reMind_me {
    internal class InterfaceManager {
        List<TaskInstance> taskInstancesPtr;
        public InputManager hid = new InputManager();
        private DateManager date = new DateManager();
        private Dictionary<string, int> priority = new Dictionary<string, int>();
        private Dictionary<string, int> size = new Dictionary<string, int>();
        private uint lastIdentifier;

        public InterfaceManager(List<TaskInstance> mainTaskInstances) {
            taskInstancesPtr = mainTaskInstances; // creates a pointer to mainTaskInstance

            // populates priority dictionary
            priority.Add("0", 0);
            priority.Add("1", 1);
            priority.Add("2", 2);
            priority.Add("3", 3);
            priority.Add("4", 4);
            priority.Add("none", 0);
            priority.Add("low", 1);
            priority.Add("medium", 2);
            priority.Add("high", 3);
            priority.Add("urgent", 4);

            // populates size dictionary
            size.Add("0", 0);
            size.Add("1", 1);
            size.Add("2", 2);
            size.Add("3", 3);
            size.Add("4", 4);
            size.Add("tiny", 0);
            size.Add("small", 1);
            size.Add("medium", 2);
            size.Add("large", 3);
            size.Add("huge", 4);
        }

        private void createTask(string taskName, string taskSize, string taskPriority, string taskDueDate) {
            lastIdentifier = taskInstancesPtr[taskInstancesPtr.Count - 1].getID();
            Random rand = new Random();
            int sizeOfTask = -1;
            int priorityOfTask = -1;
            size.TryGetValue(taskSize, out sizeOfTask);
            priority.TryGetValue(taskPriority, out priorityOfTask);
            taskInstancesPtr.Add(new TaskInstance(taskName, sizeOfTask, priorityOfTask, date.fromDatabaseString(taskDueDate)), lastIdentifier + 1);
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
        //public void writeManifest(List<TaskInstance> tasks) {

        //}

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
            switch (hid.currentUserInput) {
                case InputManager.userInput.addTask:
                    Console.Clear();
                    String name = hid.inputSanitizer("What is the name of this task", "?", true);
                    Console.Clear();

                    Func<string, bool> checkTaskSize = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "tiny", "small", "medium", "large", "huge" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    writeLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                    String size = hid.askQuestion("What is the size of this task", "?", checkTaskSize);
                    Console.Clear();

                    Func<string, bool> checkTaskPriority = (string x) => {
                        string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "none", "low", "medium", "high", "urgent" };
                        return acceptedInputs.Contains(x.ToLower());
                    };
                    writeLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                    String priority = hid.askQuestion("What is the priority of this task", "?", checkTaskPriority);
                    Console.Clear();
                    writeLine("Thank you, your task has been added!", 2);
                    // show task creation wizard
                    break;
                default:
                    Console.WriteLine("Input error, your input was not valid!");
                    break;
            }
        }
    }
}
