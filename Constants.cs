namespace reMind_me {
    internal class Constants {
        private DateManager date = new DateManager();
        private string[] welcomeText;
        public Constants() {
            welcomeText = new string[]{
                $"Good {date.timeOfDay().ToLower()},",
                "We're going to briefly set up reMind me, a tool",
                "designed for meeting deadlines while monitoring",
                "for potential burnout."


            };
        }

        public string[] getWelcomeText() {
            return welcomeText;
        }
    };
}
