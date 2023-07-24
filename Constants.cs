namespace reMind_me {
    internal class Constants {
        private DateManager date = new DateManager();
        public struct taskConstants {
            public int[] daysRequiredPerSize;
            public double[] priorityModifiers;
            public taskConstants() {
                daysRequiredPerSize = new int[] { 1, 5, 10, 20, 30 };
                priorityModifiers = new double[] { 0.2, 0.5, 1, 2, 5 };
            }
        }

        public taskConstants taskConst;
        public Constants() {
            taskConst = new taskConstants();
        }

        /** Generates welcome text using the current time of day, updates welcome text if fetched again (not fully constant)
         */
        public string[] GetWelcomeText() {
            return new string[]{
                $"Good {date.TimeOfDay().ToLower()},",
                "We're going to briefly set up reMind me, a tool",
                "designed for meeting deadlines while monitoring",
                "for potential burnout."
            };
        }
    };
}
