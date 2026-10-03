namespace Test.Nunit
{
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Test.Shared;
    using Touchstone.Core;
    using Touchstone.NunitAdapter;

    /// <summary>
    /// Runs each shared descriptor as a separate NUnit test case.
    /// </summary>
    [TestFixture]
    public sealed class StringSimilarityNunitTests
    {
        private static IEnumerable TestCases()
        {
            return new TouchstoneTestCaseSource(StringSimilaritySuites.All);
        }

        /// <summary>
        /// Run a single shared descriptor.
        /// </summary>
        /// <param name="testCase">Descriptor.</param>
        /// <returns>Task.</returns>
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task RunTest(TestCaseDescriptor testCase)
        {
            await testCase.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
