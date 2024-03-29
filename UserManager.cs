﻿namespace reMind_me {
    internal class UserManager {
        private List<TaskInstance> TaskInstancePtr;
        private List<Task> taskList;
        public List<float> currentTaskPriorityValues;
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
                // initializes arrays
                priorities = new int[5];
                sizes = new int[5];
                daysUntilDue = new List<int>();

                // sets values of all 5 slots in the arrays to zero
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
        public UserManager(List<TaskInstance> taskInstances, List<Task> taskList) {
            currentTaskPriorityValues = new List<float>();
            TaskInstancePtr = taskInstances;
            this.taskList = taskList;
            metrics = new taskMetrics();
        }

        public void calculateTaskPriorities() {
            taskList.Add(Task.Run(() => {
                currentTaskPriorityValues.Clear();
                Constants statics = new Constants();
                categorizeTasks();
                taskList[^1].Wait(); // waits for the last task in the array to complete (should be categorizeTasks)
                for (int i = 0; i < TaskInstancePtr.Count; i++) {
                    int daysBehind = statics.taskConst.daysRequiredPerSize[TaskInstancePtr[i].GetSize()] - metrics.daysUntilDue[i];
                    double priorityModifier = statics.taskConst.priorityModifiers[TaskInstancePtr[i].GetPriority()];
                    int negativeCarry = daysBehind < 0 ? -1 : 1;
                    currentTaskPriorityValues.Add((float)(Math.Pow(Math.Abs(daysBehind), 1.5) * priorityModifier) * negativeCarry);
                }
            }));
        }
        public void categorizeTasks() {
            metrics.resetMetrics();
            taskList.Add(Task.Run(() => {
                DateManager date = new DateManager();
                for (int i = 0; i < TaskInstancePtr.Count; i++) {

                    // counts the task's priority and size values
                    metrics.priorities[TaskInstancePtr[i].GetPriority()] += 1;
                    metrics.sizes[TaskInstancePtr[i].GetSize()] += 1;

                    int days = date.GetDaysFromNow(TaskInstancePtr[i].GetDueDate());

                    // sets the minimum and max to the minimum value of current min/max and the parsed task min/max
                    metrics.minDueDate = Math.Min(days, metrics.minDueDate);
                    metrics.maxDueDate = Math.Max(days, metrics.maxDueDate);

                    // counts the total tasks (used in median and Q1/Q3 calculations)
                    metrics.totalTasks += 1;

                    // sum of all days until due
                    metrics.sum += days;

                    // adds days until due to a list, allowing use in post parse analysis
                    metrics.daysUntilDue.Add(days);
                }

                metrics.medianDueDate = metrics.sum / metrics.totalTasks;

                List<int> tempArray = new List<int>();
                for (int i = 0; i < metrics.daysUntilDue.Count; i++) { // deep copy of array to keep datapoints in order
                    tempArray.Add(metrics.daysUntilDue[i]);
                }
                tempArray.Sort();

                // calculate Quartile 1 and Quartile 3 values for the days until due dataset
                if (Math.Round((float)metrics.totalTasks / 4, 4) % 1 == 0) { // check if it's a decimal or an integer (use math/non-programming definitions for this)
                    // it's an int
                    metrics.Q1DueDate = (float)(tempArray[metrics.totalTasks / 4 - 1] + (float)tempArray[metrics.totalTasks / 4]) / 2;
                    metrics.Q3DueDate = (float)(tempArray[metrics.totalTasks * 3 / 4 - 1] + (float)tempArray[metrics.totalTasks * 3 / 4]) / 2;
                } else { // it's not an int
                    metrics.Q1DueDate = tempArray[(int)Math.Ceiling((double)metrics.totalTasks / 4) - 1];
                    metrics.Q3DueDate = tempArray[(int)Math.Ceiling((double)metrics.totalTasks * 3 / 4) - 1];
                }
            }));

        }

    }
}
