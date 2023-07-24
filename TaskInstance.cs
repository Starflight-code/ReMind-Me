namespace reMind_me {
    internal class TaskInstance {
        private string name;
        public enum taskSize {
            Tiny, Small, Medium, Large, Huge
        }
        public enum taskPriority {
            None, Low, Medium, High, Urgent
        }
        private taskSize size;
        private taskPriority priority;
        private DateTime due;
        private uint id;

        public TaskInstance(string taskName, int sizeOfTask, int priorityOfTask, DateTime dueDate, uint identifier) {
            name = taskName;

            switch (sizeOfTask) {
                case 0:
                    size = taskSize.Tiny;
                    break;
                case 1:
                    size = taskSize.Small;
                    break;
                case 2:
                    size = taskSize.Medium;
                    break;
                case 3:
                    size = taskSize.Large;
                    break;
                case 4:
                    size = taskSize.Huge;
                    break;
            }

            switch (priorityOfTask) {
                case 0:
                    priority = taskPriority.None;
                    break;
                case 1:
                    priority = taskPriority.Low;
                    break;
                case 2:
                    priority = taskPriority.Medium;
                    break;
                case 3:
                    priority = taskPriority.High;
                    break;
                case 4:
                    priority = taskPriority.Urgent;
                    break;
            }
            due = dueDate;
            id = identifier;

        }

        public string GetName() { return name; }
        public int GetPriority() { return (int)priority; }
        public int GetSize() { return (int)size; }
        public string GetUiPriority() { return priority.ToString(); }
        public string GetUiSize() { return size.ToString(); }
        public uint GetID() { return id; }

        public void SetName(string newName) {
            name = newName;
        }
        public void SetSize(int newSize) {
            switch (newSize) {
                case 0:
                    size = taskSize.Tiny;
                    break;
                case 1:
                    size = taskSize.Small;
                    break;
                case 2:
                    size = taskSize.Medium;
                    break;
                case 3:
                    size = taskSize.Large;
                    break;
                case 4:
                    size = taskSize.Huge;
                    break;
            }
        }
        public void SetPriority(int newPriority) {
            switch (newPriority) {
                case 0:
                    priority = taskPriority.None;
                    break;
                case 1:
                    priority = taskPriority.Low;
                    break;
                case 2:
                    priority = taskPriority.Medium;
                    break;
                case 3:
                    priority = taskPriority.High;
                    break;
                case 4:
                    priority = taskPriority.Urgent;
                    break;
            }
        }

        public void SetDueDate(DateTime newDueDate) {
            due = newDueDate;
        }

        public DateTime GetDueDate() {
            return due;
        }

        /** Returns the due date in the correct format for the culture detected by .NET
         */
        public string GetUiDueDate() {
            return due.ToString();
        }
        public string[] GetDatabaseEntry() {
            DateManager date = new DateManager();
            return new string[] {
                name,
                ((int)size).ToString(),
                ((int)priority).ToString(),
                date.ToDatabaseString(due),
                id.ToString()
            };
        }

    }
}
