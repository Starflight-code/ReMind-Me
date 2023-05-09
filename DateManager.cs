using System.Text;

namespace reMind_me {
    internal class DateManager {
        public DateManager() {
        }
        public DateTime getCurrentDateTime() {
            return DateTime.Now;
        }
        public int getDaysBetween(DateTime toCompare, DateTime toCompareTo) {
            return toCompare.Subtract(toCompareTo).Days;
        }
        public bool isPast(DateTime toCompare) {
            return DateTime.Now.CompareTo(toCompare) > 0 && !DateOnly.Equals(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(toCompare));
        }

        /**
         * Datebase string should be formatted as YEAR-MONTH-DAY-HOUR-MINUTE-SECOND
         */
        public DateTime fromDatabaseString(string dateString) {
            String[] brokenUpDate = dateString.Trim().Split('-');
            if (brokenUpDate.Length != 6) {
                return new DateTime();
            }
            List<int> dateComponents = new List<int>();
            try {
                for (int i = 0; i < brokenUpDate.Length; i++) {
                    dateComponents.Add(Int32.Parse(brokenUpDate[i]));
                }
            }
            catch {
                return new DateTime();
            }
            return new DateTime(dateComponents[0], dateComponents[1], dateComponents[2], dateComponents[3], dateComponents[4], dateComponents[5]);
        }

        public String toDatabaseString(DateTime instance) {
            StringBuilder sb = new StringBuilder();
            sb.Append(instance.Year);
            sb.Append('-');
            sb.Append(instance.Month);
            sb.Append('-');
            sb.Append(instance.Day);
            sb.Append('-');
            sb.Append(instance.Hour);
            sb.Append('-');
            sb.Append(instance.Minute);
            sb.Append('-');
            sb.Append(instance.Second);
            return sb.ToString();
        }

        public string timeOfDay() {
            string time;
            int timeNow = DateTime.Now.Hour;
            if (timeNow < 12) {
                time = "Morning";
            } else if (timeNow < 18) {
                time = "Afternoon";
            } else {
                time = "Evening";
            };
            return time;
        }
    }
}
