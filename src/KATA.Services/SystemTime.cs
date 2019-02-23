using System;

namespace KATA.Services {
    public static class SystemTime {
        /// <summary>
        ///     Provides a mockable <see cref="DateTime" /> function.
        ///     Use as follows to override current time,
        ///     and reference time with SystemTime.Now in code.
        ///     SystemTime.Now = () => new DateTime(2009, 1, 1);
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;

        /// <summary>
        ///     Resets the static method to use the current time
        /// </summary>
        public static void Reset() {
            Now = () => DateTime.Now;
        }
    }
}