using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            this.name = taskName;

            switch (sizeOfTask) {
                case 0:
                    this.size = taskSize.Tiny;
                    break;
                case 1:
                    this.size = taskSize.Small;
                    break;
                case 2:
                    this.size = taskSize.Medium;
                    break;
                case 3:
                    this.size = taskSize.Large;
                    break;
                case 4:
                    this.size = taskSize.Huge;
                    break;
            }

            switch (priorityOfTask) {
                case 0:
                    this.priority = taskPriority.None;
                    break;
                case 1:
                    this.priority = taskPriority.Low;
                    break;
                case 2:
                    this.priority = taskPriority.Medium;
                    break;
                case 3:
                    this.priority = taskPriority.High;
                    break;
                case 4:
                    this.priority = taskPriority.Urgent;
                    break;
            }
            this.due = dueDate;
            this.id = identifier;

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
