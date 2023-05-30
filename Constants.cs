namespace reMind_me {
    internal class Constants {
        private DateManager date = new DateManager();
        public Constants() {
        }

        /** Generates welcome text using the current time of day, updates welcome text if fetched again (not fully constant)
         */
        public string[] getWelcomeText() {
            return new string[]{
                $"Good {date.timeOfDay().ToLower()},",
                "We're going to briefly set up reMind me, a tool",
                "designed for meeting deadlines while monitoring",
                "for potential burnout."
            };
        }
    };
}
