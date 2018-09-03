using System;
using System.Threading;

namespace RPS.Data.Elasticsearch.Tests.Helpers
{
    /// <summary>
    /// Provides access to system time while allowing it to be set to a fixed <see cref="DateTime"/> value.
    /// </summary>
    /// <remarks>
    /// This class is thread safe.
    /// </remarks>
    public static class SystemClock
    {
        private static readonly ThreadLocal<Func<DateTime>> GetTime =
            new ThreadLocal<Func<DateTime>>(() => () => DateTime.Now);

        /// <inheritdoc cref="DateTime.Today"/>
        public static DateTime Today => GetTime.Value().Date;

        /// <inheritdoc cref="DateTime.Now"/>
        public static DateTime Now => GetTime.Value();

        /// <inheritdoc cref="DateTime.UtcNow"/>
        public static DateTime UtcNow => GetTime.Value().ToUniversalTime();

        /// <summary>
        /// Sets a fixed (deterministic) time for the current thread to return by <see cref="SystemClock"/>.
        /// </summary>
        public static void Set(DateTime time)
        {
            if (time.Kind != DateTimeKind.Local)
            {
                time = time.ToLocalTime();
            }

            GetTime.Value = () => time;
        }

        /// <summary>
        /// Resets <see cref="SystemClock"/> to return the current <see cref="DateTime.Now"/>.
        /// </summary>
        public static void Reset()
        {
            GetTime.Value = () => DateTime.Now;
        }
    }
}
