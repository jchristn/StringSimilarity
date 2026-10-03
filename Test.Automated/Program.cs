namespace Test.Automated
{
    using System.Threading.Tasks;
    using Test.Shared;
    using Touchstone.Cli;

    /// <summary>
    /// Console runner for the shared StringSimilarity test suites.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point.  Pass --results [path] to export results as JSON.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 if all tests passed, 1 otherwise.</returns>
        public static async Task<int> Main(string[] args)
        {
            string? resultsPath = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--results" && i + 1 < args.Length)
                {
                    resultsPath = args[i + 1];
                    break;
                }
            }

            return await ConsoleRunner.RunAsync(
                StringSimilaritySuites.All,
                resultsPath: resultsPath).ConfigureAwait(false);
        }
    }
}
