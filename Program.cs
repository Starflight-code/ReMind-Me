// See https://aka.ms/new-console-template for more information
using reMind_me;

/* Data required upon task creation
 * Size of task (qualitative): likely will be an int for user input. This int will mean 1: Very Small 2: Small 3: Medium 4: Large 5: Huge
 * Priority: if the task were to be skipped over, how serious would the fallout be? 1: None 2: Low 3: Medium 4: High 5: Urgent (Top Priority)
 * Due date: When must the task be completed by? 1/2/2034 (MM/DD/YYYY format, standard in the United States. Could be modularized for the other)
 * 
 */

// Static/Constant Variables

const string DB_PATH = ".\\database.db";
string PATH = Directory.GetCurrentDirectory();
string DIRPATH = $"{PATH}\\reMind";
string MANIFEST = $"{DIRPATH}\\manifest.db";

// Dynamic Variables
List<List<string>> manifestFile;


// Class Imports
DateManager date = new DateManager();
FlatFile_Manager flat = new FlatFile_Manager();
Interface_Manager ui = new Interface_Manager();

void setup() {
    /** Ask for name (base off US given then family format)
     * Create the database with their name in it, and create a folder for tasks to be placed. 
     * Create a manifest within the folder, and grab it's path. Store within database.
     * Ask if they have DeStress, and where it is on their system (maybe just embed DeStress within it)
     * 
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
void parseDB() {
    List<List<string>> databaseParsed = flat.parseFlatFile(DB_PATH, 'd');
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
    manifestFile = flat.parseFlatFile(MANIFEST, 'm');
    return Task.FromResult(manifestFile);
}
List<string> welcomeMessage = new List<string> {
    $"Good {date.timeOfDay().ToLower()},",
    $"Let's take a look what tasks you have for today...",
    ""
};
void mainUI() {

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
        Console.WriteLine();
    } else {
        for (int i = 0; i < manifestFile[0].Count; i++) {
            ui.writeLine($"{manifestFile[0][i]} | Size: {ui.numberToInterfaceString('s', Int32.Parse(manifestFile[1][i].ToString().Trim()))} | " +
                $"Priority: {ui.numberToInterfaceString('p', Int32.Parse(manifestFile[2][i].ToString().Trim()))} | Due Date: {manifestFile[3][i]}", 10);
        }
    }

    Thread.Sleep(5000);
}
flat.startGarbageCollector();
mainUI();