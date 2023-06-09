namespace reMind_me {
    internal class UserManager {
        private List<TaskInstance> TaskInstancePtr;
        private List<Task> taskList;
        struct taskMetrics {
            public int[] priorities; // 0: None 1: Low 2: Medium 3: High 4: Urgent

            public int[] sizes; // 0: Tiny 1: Small 2: Medium 3: Large 4: Huge

            public int minDueDate;
            public float Q1DueDate;
            public float medianDueDate;
            public float Q3DueDate;
            public int maxDueDate;
            public int sum;
            public List<int> daysUntilDue;

            public int totalTasks;

            public taskMetrics() {
                priorities = new int[5];
                sizes = new int[5];
                daysUntilDue = new List<int>();

                for (int i = 0; i < priorities.Length; i++) {
                    priorities[i] = 0;
                }

                for (int i = 0; i < sizes.Length; i++) {
                    sizes[i] = 0;
                }

                minDueDate = int.MaxValue;
                Q1DueDate = 0;
                medianDueDate = 0;
                Q3DueDate = 0;
                maxDueDate = int.MinValue;
                sum = 0;

                totalTasks = 0;
            }

            public void resetMetrics() {
                for (int i = 0; i < priorities.Length; i++) {
                    priorities[i] = 0;
                }

                for (int i = 0; i < sizes.Length; i++) {
                    sizes[i] = 0;
                }

                minDueDate = int.MaxValue;
                Q1DueDate = 0;
                medianDueDate = 0;
                Q3DueDate = 0;
                maxDueDate = int.MinValue;
                sum = 0;

                totalTasks = 0;
            }
        }

        taskMetrics currentTaskMetrics;
        public UserManager(List<TaskInstance> taskInstances) {
            TaskInstancePtr = taskInstances;
            taskList = new List<Task>();
        }

        public void categorizeTasks() {
            currentTaskMetrics.resetMetrics();
            taskList.Add(Task.Run(() => {
                DateManager date = new DateManager();
                for (int i = 0; i < TaskInstancePtr.Count; i++) {
                    currentTaskMetrics.priorities[TaskInstancePtr[i].GetPriority()] += 1;
                    currentTaskMetrics.sizes[TaskInstancePtr[i].GetSize()] += 1;

                    int days = date.GetDaysFromNow(TaskInstancePtr[i].GetDueDate());

                    currentTaskMetrics.minDueDate = Math.Min(days, currentTaskMetrics.minDueDate);
                    currentTaskMetrics.maxDueDate = Math.Max(days, currentTaskMetrics.maxDueDate);

                    currentTaskMetrics.totalTasks += 1;
                    currentTaskMetrics.sum += days;
                    currentTaskMetrics.daysUntilDue.Add(days);
                }
                currentTaskMetrics.medianDueDate = currentTaskMetrics.sum / currentTaskMetrics.totalTasks;
                currentTaskMetrics.daysUntilDue.Sort();

                if (Math.Round((float)currentTaskMetrics.totalTasks / 4, 4) % 1 == 0) { // check if it's a decimal or an int
                    // it's an int
                    currentTaskMetrics.Q1DueDate = (currentTaskMetrics.daysUntilDue[currentTaskMetrics.totalTasks / 4] + currentTaskMetrics.daysUntilDue[currentTaskMetrics.totalTasks / 4 + 1]) / 2;
                    currentTaskMetrics.Q3DueDate = (currentTaskMetrics.daysUntilDue[currentTaskMetrics.totalTasks * 3 / 4] + currentTaskMetrics.daysUntilDue[currentTaskMetrics.totalTasks * 3 / 4 + 1]) / 2;
                } else { // it's not an int
                    currentTaskMetrics.Q1DueDate = currentTaskMetrics.daysUntilDue[(int)Math.Ceiling((double)currentTaskMetrics.totalTasks / 4)];
                    currentTaskMetrics.Q3DueDate = currentTaskMetrics.daysUntilDue[(int)Math.Ceiling((double)currentTaskMetrics.totalTasks * 3 / 4)];
                }
            }));

        }

    }
}
