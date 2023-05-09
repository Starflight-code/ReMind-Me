namespace reMind_me {
    internal class TaskInstance {
        private String name;
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

        public TaskInstance(String taskName, int sizeOfTask, int priorityOfTask, DateTime dueDate, uint identifier) {
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

        public String getName() { return name; }
        public taskPriority getPriority() { return priority; }
        public taskSize getSize() { return size; }
        public String getUiPriority() { return priority.ToString(); }
        public String getUiSize() { return size.ToString(); }
        /** Returns the due date in the correct format for the culture detected by .NET
         */
        public String getDueDate() {
            return due.ToString();
        }

    }
}
