﻿// See https://aka.ms/new-console-template for more information
using reMind_me;
using System.Runtime.CompilerServices;

/* Data required upon task creation
 * Size of task (qualitative): likely will be an int for user input. This int will mean 1: Very Small 2: Small 3: Medium 4: Large 5: Huge
 * Priority: if the task were to be skipped over, how serious would the fallout be? 1: None 2: Low 3: Medium 4: High 5: Urgent (Top Priority)
 * Due date: When must the task be completed by? 1/2/2034 (MM/DD/YYYY format, standard in the United States. Could be modularized for the other)
 * 
 */

// Enumerations

// Static/Constant Variables

const string DB_PATH = ".\\database.db";
string PATH = Directory.GetCurrentDirectory();
string DIRPATH = $"{PATH}\\reMind";
string MANIFEST = $"{DIRPATH}\\manifest.db";



// Dynamic Variables
List<List<string>> manifestFile = new List<List<string>>();
List<TaskInstance> taskInstances = new List<TaskInstance>();
String input = "NULL";


// Class Imports
DateManager date = new DateManager();
FlatFile_Manager flat = new FlatFile_Manager();
Interface_Manager ui = new Interface_Manager();


void setup() {
    /** Ask for name (base off US given then family format)
     * Create the database with their name in it, and create a folder for tasks to be placed. 
     * Create a manifest within the folder, and grab it's path. Store within database.
     * Ask if they have DeStress, and where it is on their system (maybe just embed DeStress within it)
     */
    string[] welcomeText = {
    $"Good {date.timeOfDay().ToLower()},",
    "We're going to briefly set up reMind me, a tool",
    "designed for meeting deadlines while monitoring",
    "for potential burnout."
    };
    ui.writeAllLines(welcomeText, 15, false);

    string? fullName = null;
    fullName = ui.inputSanitizer("What is your name (First and Last)", "? ");
    while (fullName == null || fullName.Split(" ").Length != 2) {
        Console.WriteLine("\nOops, that may not be a valid (First and Last) name. Please try again...");
        fullName = ui.inputSanitizer("What is your name (First and Last)", "? ", true);
    }

    ui.pleaseWait("creating reMind's database");
    string[] databaseContent = {
        fullName.Split(" ")[0],
        fullName.Split(" ")[1],
        DIRPATH
    };

    string[] manifestContent =
    {
        ""
    };
    Directory.CreateDirectory(DIRPATH);
    flat.createNewFlatfile(DB_PATH, databaseContent);
    flat.createNewFlatfile($"{DIRPATH}\\manifest.db", manifestContent);

    Console.Clear();
    Console.WriteLine("Done! Starting reMind...");
}

/** Opens up and reads the database, importing data
 */
void parseDB() {
    List<List<string>> databaseParsed = flat.parseFlatFile(DB_PATH, FlatFile_Manager.flatFileType.Database);
    string[] fullName = {
        databaseParsed[0][0],
        databaseParsed[0][1]
    };
    DIRPATH = databaseParsed[1][0];
    MANIFEST = $"{DIRPATH}\\manifest.db";
}
/** Manifest File Design
 * <Name of Task>
 * <Size of Task>
 * <Priority>
 * <Due Date>
 * <Idenifier (8 int digits, randomized upon creation)>
 */
Task<List<List<string>>> parseManifest(string MANIFEST) {
    DateManager manager = new DateManager();
    manifestFile = flat.parseFlatFile(MANIFEST, FlatFile_Manager.flatFileType.Manifest);

    for (int i = 0; i < manifestFile[0].Count; i++) {
        try {
            taskInstances.Add(new TaskInstance(manifestFile[0][i], int.Parse(manifestFile[1][i].Trim()), int.Parse(manifestFile[2][i].Trim()), manager.fromDatabaseString(manifestFile[3][i]), uint.Parse(manifestFile[4][i])));
        }
        catch (Exception e) {
            throw e; // notify user later on, throwing for alpha stage debugging
        }
        }
    return Task.FromResult(manifestFile);
}
List<string> welcomeMessage = new List<string> {
    $"Good {date.timeOfDay().ToLower()},",
    $"Let's take a look what tasks you have for today...",
    ""
};

void printTasks() {
    for (int i = 0; i < taskInstances.Count; i++) {
        ui.writeLine($"{taskInstances[i].getName()} | Size: {taskInstances[i].getUiSize()} | " +
            $"Priority: {taskInstances[i].getUiPriority()} | Due Date: {taskInstances[i].getDueDate()}", 10);
    }
}
void mainUI() {

    // sets up database if it doesn't exist
    if (!flat.checkIfExists(DB_PATH)) {
        setup();
        parseDB();
    } else {
        parseDB();
    }

    Task<List<List<string>>> manifest = parseManifest(MANIFEST);
    Thread.Sleep(500);
    Console.Clear();

    ui.writeAllLines(welcomeMessage.ToArray(), 15);
    manifestFile = manifest.GetAwaiter().GetResult();

    if (manifestFile[0].Count == 0) {
        ui.writeLine("We don't seem to have any tasks. You have some time to relax!", 10);
        ui.writeMainUI();
    } else {
        printTasks();
        ui.writeMainUI();
    }
    while (true) {
        ui.getUserInput();
    }
}
flat.startGarbageCollector();
mainUI();

/* Main UI design
 * Create a dynamic UI....?
 * 
 * 1. Create a new task
 * 2. Modify an existing task
 * 3. ... etc.
 * 
 * Enter commands like
 * 'create <Task Title>'
 * 'edit <Task Title>'
 * Perhaps add alias support
 * 
 * Directly interface with posted tasks...
 * 1. Finish history assignment 1.25
 * 2. Bake a cake
 * Context menus added when task is selected
 * <Title>
 * <Due Date>
 * <Priority>
 * <Task Size>
 * 
 * 1. Change Title
 * 2. Change Due Date
 * 3. ... etc.
 */