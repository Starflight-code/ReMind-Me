namespace reMind_me {
    internal class FlatFileManager {
        public enum flatFileType {
            Manifest,
            Database
        }

        List<Task> taskList = new List<Task>();
        List<Task> garbageCollectors = new List<Task>();
        public FlatFileManager() { }
        public void CreateNewFlatfile(string name, string[] toWrite) {
            taskList.Add(Task.Run(() => {
                var x = File.Create(name);
                x.Dispose();
                File.WriteAllLines(name, toWrite);
            }));
        }
        public string[] ReadFlatFile(string name) {
            return File.ReadAllLines(name);
        }
        public bool CheckIfExists(string name) {
            return File.Exists(name);
        }
        public void WriteFlatFile(string name, string[] toWrite, char mode = 'w') {
            taskList.Add(Task.Run(() => {
                switch (mode) {

                    case 'w':
                        File.WriteAllLines(name, toWrite);
                        break;
                    case 'a':
                        File.AppendAllLines(name, toWrite);
                        break;
                }
            }));
        }

        /** Generates the array used for writing the manifest flat file, contains
         * all the data needed to regenerate the current list of task objects
         */
        private string[] GenerateManifestToWrite(List<TaskInstance> tasks) {
            List<string> databaseToWrite = new List<string>();
            for (int i = 0; i < tasks.Count; i++) {
                databaseToWrite.AddRange(tasks[i].GetDatabaseEntry());
            }
            return databaseToWrite.ToArray();
        }

        /** Writes a new manifest, containing all the data required to regenerate the list of task objects
         */
        public void WriteManifest(List<TaskInstance> tasks, string manifestPath) {
            string[] manifestToWrite = GenerateManifestToWrite(tasks);
            WriteFlatFile(manifestPath, manifestToWrite, 'w');
        }
        public List<List<string>> ParseFlatFile(string name, flatFileType parseType) {
            string[] content = ReadFlatFile(name);

            // creates a multidimentional array, includes 5 List<string> subarrays
            List<List<string>> parsedData = new List<List<string>>() {
            new List<string>(),
            new List<string>(),
            new List<string>(),
            new List<string>(),
            new List<string>()
            };

            switch (parseType) {

                case flatFileType.Manifest:
                    for (int i = 0; i < (content.Length);) {
                        for (int j = 0; j < (5); j++, i++) {
                            parsedData[j].Add(content[i]);
                        }
                    }
                    break;

                case flatFileType.Database:
                    for (int i = 0; i < content.Length; i++) {
                        if (i is 0 or 1) {
                            parsedData[0].Add(content[i]);
                        }
                        if (i is 2) {
                            parsedData[1].Add(content[i]);
                        }
                    }
                    break;
            }
            return parsedData;
            // returns data in format 

        }
        public void StartGarbageCollector() {
            garbageCollectors.Add(Task.Run(async () => {

                for (int i = 0; i < taskList.Count; i++) {
                    if (taskList[i].IsCanceled || taskList[i].IsCompleted | taskList[i].IsFaulted) {
                        taskList[i].Dispose();
                        taskList.RemoveAt(i);
                    }
                }

                await Task.Delay(15000); // checks every 15 seconds
            }));
        }
    }
}
