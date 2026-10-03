namespace Test.Xunit
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Test.Shared;
    using Touchstone.Core;
    using Touchstone.XunitAdapter;
    using global::Xunit;

    /// <summary>
    /// Runs every shared descriptor sequentially in a single xUnit fact.
    /// </summary>
    public sealed class StringSimilarityFactTests : TouchstoneFactBase
    {
        /// <summary>
        /// Suites to execute.
        /// </summary>
        protected override IReadOnlyList<TestSuiteDescriptor> Suites
        {
            get { return StringSimilaritySuites.All; }
        }

        /// <summary>
        /// Run all shared descriptors.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RunAll()
        {
            await RunAllAsync();
        }
    }
}
