using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
