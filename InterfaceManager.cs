﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace reMind_me {
    internal class Interface_Manager {
        List<TaskInstance>* taskInstancesPtr;

        public enum userInput {
            addTask,
            ERROR
        }

        public userInput currentUserInput;
        // dictionary converting commands to enums, allowing them to be processed more easily
        // 
        Dictionary<String, userInput> userInputToEnum = new Dictionary<String, userInput>();

        public Interface_Manager() {
            userInputToEnum.Add("add", userInput.addTask);
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

        public userInput handleInput(String input) {
            userInput returnVal;
            if (!userInputToEnum.TryGetValue(input.ToLower().Trim(), out returnVal)) {
                this.currentUserInput = userInput.ERROR;
                return userInput.ERROR;
            }
            this.currentUserInput = returnVal;
            return returnVal;
        }

        public String getUserInput() {
            Console.WriteLine("\n");
            writeLine("ReMind\\> ", 5);
            String? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.WriteLine("\n");
                writeLine("Invalid Input Detected. Try again...", 5);
                writeLine("ReMind\\> ", 5);
                input = Console.ReadLine();
            }
            return input;
        }

        public void printUI() {
            switch (currentUserInput) {
                case userInput.addTask:
                    Console.Clear();
                    AddNew
                    break;
            }
        }
    }
}
