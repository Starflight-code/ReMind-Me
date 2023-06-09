namespace reMind_me {
    internal class UserManager {
        private List<TaskInstance> TaskInstancePtr;
        private List<Task> taskList;
        public struct taskMetrics {
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

        public taskMetrics metrics;
        public UserManager(List<TaskInstance> taskInstances) {
            TaskInstancePtr = taskInstances;
            taskList = new List<Task>();
            metrics = new taskMetrics();
        }

        public void categorizeTasks() {
            metrics.resetMetrics();
            taskList.Add(Task.Run(() => {
                DateManager date = new DateManager();
                for (int i = 0; i < TaskInstancePtr.Count; i++) {
                    metrics.priorities[TaskInstancePtr[i].GetPriority()] += 1;
                    metrics.sizes[TaskInstancePtr[i].GetSize()] += 1;

                    int days = date.GetDaysFromNow(TaskInstancePtr[i].GetDueDate());

                    metrics.minDueDate = Math.Min(days, metrics.minDueDate);
                    metrics.maxDueDate = Math.Max(days, metrics.maxDueDate);

                    metrics.totalTasks += 1;
                    metrics.sum += days;
                    metrics.daysUntilDue.Add(days);
                }
                metrics.medianDueDate = metrics.sum / metrics.totalTasks;
                metrics.daysUntilDue.Sort();

                if (Math.Round((float)metrics.totalTasks / 4, 4) % 1 == 0) { // check if it's a decimal or an int
                    // it's an int
                    metrics.Q1DueDate = (metrics.daysUntilDue[metrics.totalTasks / 4 - 1] + metrics.daysUntilDue[metrics.totalTasks / 4]) / 2;
                    metrics.Q3DueDate = (metrics.daysUntilDue[metrics.totalTasks * 3 / 4 - 1] + metrics.daysUntilDue[metrics.totalTasks * 3 / 4]) / 2;
                } else { // it's not an int
                    metrics.Q1DueDate = metrics.daysUntilDue[(int)Math.Ceiling((double)metrics.totalTasks / 4) - 1];
                    metrics.Q3DueDate = metrics.daysUntilDue[(int)Math.Ceiling((double)metrics.totalTasks * 3 / 4) - 1];
                }
            }));

        }

    }
}
