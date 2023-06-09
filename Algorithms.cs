namespace reMind_me {
    internal class Algorithms {
        public Algorithms() { }
        public void include_task() {

        }

        public Func<string, bool> checkTaskSize = (string x) => {
            string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "tiny", "small", "medium", "large", "huge" };
            return acceptedInputs.Contains(x.ToLower());
        };

        public Func<string, bool> checkTaskPriority = (string x) => {
            string[] acceptedInputs = new string[] { "0", "1", "2", "3", "4", "none", "low", "medium", "high", "urgent" };
            return acceptedInputs.Contains(x.ToLower());
        };

        public Func<string, bool> checkDueDate = (string x) => {
            return DateTime.TryParse(x, out _);
        };

    }
}
