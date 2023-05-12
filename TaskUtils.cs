namespace reMind_me {
    internal class TaskUtils {
        List<TaskInstance> taskInstanceListPtr;
        public TaskUtils(List<TaskInstance> instanceListPtr) {
            taskInstanceListPtr = instanceListPtr;
        }
        public int findTaskIndex(string nameOfTask) {
            for (int i = 0; i < taskInstanceListPtr.Count(); i++) {
                if (taskInstanceListPtr[i].getName().ToLower() == nameOfTask.ToLower()) {
                    return i;
                }
            }
            return -1;
        }

        public bool removeTask(string nameOfTask) {
            int index = findTaskIndex(nameOfTask);
            if (index == -1) {
                return false;
            }
            taskInstanceListPtr.RemoveAt(index);
            return true;
        }

        public bool removeTask(int index) {
            if (index == -1) {
                return false;
            }
            taskInstanceListPtr.RemoveAt(index);
            return true;
        }

        public void addTask(TaskInstance task) {
            for (int i = 0; i < taskInstanceListPtr.Count; i++) {
                if (taskInstanceListPtr[i].getID == task.getID) {
                    return;
                }
            }
            taskInstanceListPtr.Add(task);
        }
    }
}
