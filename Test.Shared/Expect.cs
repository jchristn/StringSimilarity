namespace Test.Shared
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Assertion helpers for shared test descriptors.  Failures throw TestAssertionException.
    /// </summary>
    public static class Expect
    {
        /// <summary>
        /// Default tolerance used when comparing non-terminating decimal scores.
        /// </summary>
        public static decimal DefaultTolerance { get; } = 0.0000000001m;

        /// <summary>
        /// Assert that two decimal values are exactly equal.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="context">Description of what was being compared.</param>
        /// <exception cref="TestAssertionException">Thrown when the values differ.</exception>
        public static void Equal(decimal expected, decimal actual, string context)
        {
            if (expected != actual)
                throw new TestAssertionException(
                    context + ": expected " + Format(expected) + " but got " + Format(actual));
        }

        /// <summary>
        /// Assert that two decimal values are within DefaultTolerance of each other.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="context">Description of what was being compared.</param>
        /// <exception cref="TestAssertionException">Thrown when the values differ by more than the tolerance.</exception>
        public static void Approximately(decimal expected, decimal actual, string context)
        {
            if (Math.Abs(expected - actual) > DefaultTolerance)
                throw new TestAssertionException(
                    context + ": expected approximately " + Format(expected) + " but got " + Format(actual));
        }

        /// <summary>
        /// Assert that a score lies within the inclusive range 0 to 1.
        /// </summary>
        /// <param name="actual">Score.</param>
        /// <param name="context">Description of what was being scored.</param>
        /// <exception cref="TestAssertionException">Thrown when the score is out of range.</exception>
        public static void InUnitRange(decimal actual, string context)
        {
            if (actual < 0m || actual > 1m)
                throw new TestAssertionException(
                    context + ": expected score between 0 and 1 but got " + Format(actual));
        }

        /// <summary>
        /// Assert that the first value is strictly greater than the second.
        /// </summary>
        /// <param name="greater">Value expected to be greater.</param>
        /// <param name="lesser">Value expected to be lesser.</param>
        /// <param name="context">Description of what was being compared.</param>
        /// <exception cref="TestAssertionException">Thrown when the ordering does not hold.</exception>
        public static void GreaterThan(decimal greater, decimal lesser, string context)
        {
            if (greater <= lesser)
                throw new TestAssertionException(
                    context + ": expected " + Format(greater) + " to be greater than " + Format(lesser));
        }

        /// <summary>
        /// Assert that a condition is true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="message">Failure message.</param>
        /// <exception cref="TestAssertionException">Thrown when the condition is false.</exception>
        public static void True(bool condition, string message)
        {
            if (!condition) throw new TestAssertionException(message);
        }

        private static string Format(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
