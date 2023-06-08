using reMind_me;

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
String input;


// Class Imports
DateManager date = new DateManager();
FlatFileManager flat = new FlatFileManager();
InterfaceManager ui = new InterfaceManager(taskInstances, new Settings(false));
Constants statics = new Constants();


void setup() {
    /** Ask for name (base off US given then family format)
     * Create the database with their name in it, and create a folder for tasks to be placed. 
     * Create a manifest within the folder, and grab it's path. Store within database.
     * Ask if they have DeStress, and where it is on their system (maybe just embed DeStress within it)
     */
    ui.WriteAllLines(statics.GetWelcomeText(), 15, false);


    // function that will check over user input, used by askQuestion method of InputManager
    Func<string, bool> checkForValidName = (string x) => {
        if (x.Split(" ").Length == 2) { // makes sure the name contains two words
            return true;
        }
        return false;
    };

    string fullName = ui.hid.AskQuestion("What is your name (First and Last)", "? ", checkForValidName);


    ui.PleaseWait("creating reMind's database");
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
    flat.CreateNewFlatfile(DB_PATH, databaseContent);
    flat.CreateNewFlatfile($"{DIRPATH}\\manifest.db", manifestContent);

    Console.Clear();
    Console.WriteLine("Done! Starting reMind...");
}

/** Opens up and reads the database, importing data
 */
void parseDB() {
    List<List<string>> databaseParsed = flat.ParseFlatFile(DB_PATH, FlatFileManager.flatFileType.Database);
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
    manifestFile = flat.ParseFlatFile(MANIFEST, FlatFileManager.flatFileType.Manifest);

    for (int i = 0; i < manifestFile[0].Count; i++) {
        try {
            taskInstances.Add(new TaskInstance(manifestFile[0][i], int.Parse(manifestFile[1][i].Trim()), int.Parse(manifestFile[2][i].Trim()), manager.FromDatabaseString(manifestFile[3][i]), uint.Parse(manifestFile[4][i])));
        }
        catch {
            throw; // notify user later on, throwing for alpha stage debugging (database corruption/parsing issue)
        }
    }
    return Task.FromResult(manifestFile);
}
List<string> welcomeMessage = new List<string> {
    $"Good {date.TimeOfDay().ToLower()},",
    $"Let's take a look what tasks you have for today...",
    ""
};
void mainUI() {

    // sets up database if it doesn't exist
    if (!flat.CheckIfExists(DB_PATH)) {
        setup();
        parseDB();
    } else {
        parseDB();
    }

    Task<List<List<string>>> manifest = parseManifest(MANIFEST);
    Thread.Sleep(500);
    Console.Clear();

    ui.WriteAllLines(welcomeMessage.ToArray(), 5);
    manifestFile = manifest.GetAwaiter().GetResult();

    if (manifestFile[0].Count == 0) {
        ui.WriteLine("We don't seem to have any tasks. You have some time to relax!", 10);
        ui.WriteMainUI();
    } else {
        ui.PrintTasks();
        ui.WriteMainUI();
    }
    while (true) {
        input = ui.hid.GetUserInput();
        ui.PrintUI();
        Console.WriteLine("\n"); // adds 2 newlines
        ui.PrintTasks();
        ui.WriteMainUI();
        flat.WriteManifest(taskInstances, MANIFEST);
    }
}
flat.StartGarbageCollector();
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