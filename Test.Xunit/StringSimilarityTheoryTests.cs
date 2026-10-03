namespace Test.Xunit
{
    using System.Threading;
    using System.Threading.Tasks;
    using Test.Shared;
    using Touchstone.Core;
    using global::Xunit;
    using global::Xunit.Abstractions;

    /// <summary>
    /// Runs each shared descriptor as a separate xUnit theory row.
    /// </summary>
    public sealed class StringSimilarityTheoryTests
    {
        private readonly ITestOutputHelper _Output;

        /// <summary>
        /// Instantiate the theory tests.
        /// </summary>
        /// <param name="output">xUnit output helper.</param>
        public StringSimilarityTheoryTests(ITestOutputHelper output)
        {
            _Output = output;
        }

        /// <summary>
        /// Enumerate all non-skipped shared descriptors.
        /// </summary>
        /// <returns>Theory data.</returns>
        public static TheoryData<TestCaseDescriptor> TestCases()
        {
            TheoryData<TestCaseDescriptor> data = new TheoryData<TestCaseDescriptor>();

            foreach (TestSuiteDescriptor suite in StringSimilaritySuites.All)
            {
                foreach (TestCaseDescriptor testCase in suite.Cases)
                {
                    if (!testCase.Skip)
                        data.Add(testCase);
                }
            }

            return data;
        }

        /// <summary>
        /// Run a single shared descriptor.
        /// </summary>
        /// <param name="testCase">Descriptor.</param>
        /// <returns>Task.</returns>
        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task RunTest(TestCaseDescriptor testCase)
        {
            _Output.WriteLine($"Running: {testCase.DisplayName}");
            await testCase.ExecuteAsync(CancellationToken.None);
        }
    }
}
