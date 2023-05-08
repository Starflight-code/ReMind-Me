using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reMind_me {
    internal class FlatFile_Manager {
        List<Task> taskList = new List<Task>();
        List<Task> garbageCollectors = new List<Task>();
        public FlatFile_Manager() { }
        public void createNewFlatfile(string name, string[] toWrite) {
            taskList.Add(Task.Run(() => {
                var x = File.Create(name);
                x.Dispose();
                File.WriteAllLines(name, toWrite);
            }));
        }
        public string[] readFlatFile(string name) {
            return File.ReadAllLines(name);
        }
        public bool checkIfExists(string name) {
            return File.Exists(name);
        }
        public void writeFlatFile(string name, string[] toWrite, char mode = 'w') {
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
        public List<List<string>> parseFlatFile(string name, char parseType) {
            string[] content = readFlatFile(name);
            //string[,] parsedData = new string[5, 2];
            List<List<string>> parsedData = new List<List<string>>() {
            new List<string>(),
            new List<string>(),
            new List<string>(),
            new List<string>(),
            new List<string>()
            };
            switch (parseType) {
                case 'm': // Manifest File
                    for (int i = 0; i < (content.Length);) {
                        for (int j = 0; j < (5); j++, i++) {
                            parsedData[j].Add(content[i]);
                        }
                    }
                    break;
                case 'd': //Database File
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
        }
        public void startGarbageCollector() {
            garbageCollectors.Add(Task.Run(async () => {
                for (int i = 0; i < taskList.Count; i++) {
                    if (taskList[i].IsCanceled || taskList[i].IsCompleted | taskList[i].IsFaulted) {
                        taskList[i].Dispose();
                        taskList.RemoveAt(i);
                    }
                }
                await Task.Delay(15000);
            }));
        }
    }
}
