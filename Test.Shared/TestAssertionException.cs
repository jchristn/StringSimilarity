namespace Test.Shared
{
    using System;

    /// <summary>
    /// Thrown when a shared test assertion fails.
    /// </summary>
    public class TestAssertionException : Exception
    {
        /// <summary>
        /// Instantiate the exception.
        /// </summary>
        /// <param name="message">Description of the failed assertion.</param>
        public TestAssertionException(string message) : base(message)
        {
        }
    }
}
