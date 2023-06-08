namespace reMind_me {
    internal class TaskUtils {
        List<TaskInstance> taskInstanceListPtr;
        public TaskUtils(List<TaskInstance> instanceListPtr) {
            taskInstanceListPtr = instanceListPtr;
        }
        public int FindTaskIndex(string nameOfTask) {
            for (int i = 0; i < taskInstanceListPtr.Count(); i++) {
                if (taskInstanceListPtr[i].GetName().ToLower() == nameOfTask.ToLower()) {
                    return i;
                }
            }
            return -1;
        }

        public bool RemoveTask(string nameOfTask) {
            int index = FindTaskIndex(nameOfTask);
            if (index == -1) {
                return false;
            }
            taskInstanceListPtr.RemoveAt(index);
            return true;
        }

        public bool RemoveTask(int index) {
            if (index == -1) {
                return false;
            }
            taskInstanceListPtr.RemoveAt(index);
            return true;
        }

        public void AddTask(TaskInstance task) {
            for (int i = 0; i < taskInstanceListPtr.Count; i++) {
                if (taskInstanceListPtr[i].GetID == task.GetID) {
                    return;
                }
            }
            taskInstanceListPtr.Add(task);
        }
    }
}
