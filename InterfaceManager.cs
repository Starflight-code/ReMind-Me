namespace reMind_me {
    internal class InterfaceManager {
        List<TaskInstance> taskInstancesPtr;
        public InputManager hid = new InputManager();

        public InterfaceManager(List<TaskInstance> mainTaskInstances) {
            taskInstancesPtr = mainTaskInstances; // creates a pointer to mainTaskInstance
        }
        public InterfaceManager() {
            taskInstancesPtr = new List<TaskInstance>();
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
            writeLine("NEW: Create a new task", 5);
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
                    String name = hid.inputSanitizer("What is the name of this task", "? ");
                    Console.Clear();
                    writeLine("You have a few options for the next prompt. 0/Tiny 1/Small 2/Medium 3/Large 4/Huge", 5);
                    String size = hid.inputSanitizer("What is the size of this task", "? ");
                    Console.Clear();
                    writeLine("You have a few options for the next prompt. 0/None 1/Low 2/Medium 3/High 4/Urgent", 5);
                    String priority = hid.inputSanitizer("What is the priority of this task", "? ");
                    Console.Clear();
                    writeLine("Thank you, we are now creating your task.", 2);
                    // show task creation wizard
                    break;
            }
        }
    }
}
