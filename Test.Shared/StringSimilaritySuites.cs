namespace Test.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Similarity;
    using Touchstone.Core;

    /// <summary>
    /// Shared test suite descriptors for the StringSimilarity library.  This is the single
    /// source of truth consumed by the Test.Automated, Test.Xunit, and Test.Nunit runners.
    /// </summary>
    public static class StringSimilaritySuites
    {
        /// <summary>
        /// All test suites.
        /// </summary>
        public static IReadOnlyList<TestSuiteDescriptor> All
        {
            get
            {
                return new List<TestSuiteDescriptor>
                {
                    NullAndEmptySuite(),
                    IdentitySuite(),
                    ScoringSuite(),
                    CaseAndCharacterSuite(),
                    PropertySuite(),
                    ScaleSuite()
                };
            }
        }

        /// <summary>
        /// Null and empty input handling.  None of these inputs may throw.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor NullAndEmptySuite()
        {
            const string suite = "NullAndEmpty";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Null and Empty Inputs",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "BothNull", "Both null returns 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate(null, null), "Calculate(null, null)")),

                    Case(suite, "BothEmpty", "Both empty returns 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("", ""), "Calculate(\"\", \"\")")),

                    Case(suite, "NullAndEmpty", "Null and empty are treated as equal",
                        () =>
                        {
                            Expect.Equal(1m, StringSimilarity.Calculate(null, ""), "Calculate(null, \"\")");
                            Expect.Equal(1m, StringSimilarity.Calculate("", null), "Calculate(\"\", null)");
                        }),

                    Case(suite, "FirstNullSecondValue", "Null first argument with non-empty second returns 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate(null, "abc"), "Calculate(null, \"abc\")")),

                    Case(suite, "FirstValueSecondNull", "Non-empty first argument with null second returns 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("abc", null), "Calculate(\"abc\", null)")),

                    Case(suite, "FirstEmptySecondValue", "Empty first argument with non-empty second returns 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("", "abc"), "Calculate(\"\", \"abc\")")),

                    Case(suite, "FirstValueSecondEmpty", "Non-empty first argument with empty second returns 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("abc", ""), "Calculate(\"abc\", \"\")")),

                    Case(suite, "WhitespaceVersusEmpty", "Whitespace is not treated as empty",
                        () =>
                        {
                            Expect.Equal(0m, StringSimilarity.Calculate(" ", ""), "Calculate(\" \", \"\")");
                            Expect.Equal(0m, StringSimilarity.Calculate(null, " "), "Calculate(null, \" \")");
                        })
                });
        }

        /// <summary>
        /// Identical inputs always score 1.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor IdentitySuite()
        {
            const string suite = "Identity";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Identical Inputs",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "SingleCharacter", "Identical single character returns 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("a", "a"), "Calculate(\"a\", \"a\")")),

                    Case(suite, "Word", "Identical word returns 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("hello", "hello"), "Calculate(\"hello\", \"hello\")")),

                    Case(suite, "Sentence", "Identical sentence with punctuation returns 1",
                        () =>
                        {
                            string s = "The quick brown fox, jumped over the lazy dog!";
                            Expect.Equal(1m, StringSimilarity.Calculate(s, s), "Calculate(sentence, sentence)");
                        }),

                    Case(suite, "WhitespaceOnly", "Identical whitespace-only strings return 1",
                        () =>
                        {
                            Expect.Equal(1m, StringSimilarity.Calculate(" ", " "), "Calculate(\" \", \" \")");
                            Expect.Equal(1m, StringSimilarity.Calculate("\t\r\n", "\t\r\n"), "Calculate(tab-crlf, tab-crlf)");
                        }),

                    Case(suite, "ControlCharacters", "Identical control characters return 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("\0\u0001", "\0\u0001"), "Calculate(control, control)")),

                    Case(suite, "EqualButDistinctInstances", "Equal content in distinct string instances returns 1",
                        () =>
                        {
                            string a = new string(new char[] { 'x', 'y', 'z' });
                            string b = new StringBuilder().Append("xy").Append('z').ToString();
                            Expect.True(!ReferenceEquals(a, b), "Test setup expected distinct string instances");
                            Expect.Equal(1m, StringSimilarity.Calculate(a, b), "Calculate(xyz, xyz)");
                        }),

                    Case(suite, "Emoji", "Identical emoji (surrogate pairs) return 1",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("\U0001F600", "\U0001F600"), "Calculate(emoji, emoji)"))
                });
        }

        /// <summary>
        /// Known scores for representative inputs.  The score is the product of the length ratio
        /// (shorter length divided by longer length) and the fraction of the larger set of distinct
        /// characters that also appears in the other string.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor ScoringSuite()
        {
            const string suite = "Scoring";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Score Calculation",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "CompletelyDifferent", "Strings with no characters in common return 0",
                        () =>
                        {
                            Expect.Equal(0m, StringSimilarity.Calculate("abc", "xyz"), "Calculate(\"abc\", \"xyz\")");
                            Expect.Equal(0m, StringSimilarity.Calculate("a", "b"), "Calculate(\"a\", \"b\")");
                        }),

                    Case(suite, "DisjointDifferentLengths", "Disjoint strings of different lengths return 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("aaaaaaaa", "bc"), "Calculate(\"aaaaaaaa\", \"bc\")")),

                    Case(suite, "OneCharacterDifferent", "Same length with one character different scores 2/3",
                        () => Expect.Approximately(2m / 3m, StringSimilarity.Calculate("abc", "abd"), "Calculate(\"abc\", \"abd\")")),

                    Case(suite, "HalfCharactersShared", "Same length with half the characters shared scores 0.5",
                        () => Expect.Equal(0.5m, StringSimilarity.Calculate("abcdef", "abcxyz"), "Calculate(\"abcdef\", \"abcxyz\")")),

                    Case(suite, "Prefix", "Prefix string scores length ratio times character ratio",
                        () => Expect.Equal(0.5625m, StringSimilarity.Calculate("abc", "abcd"), "Calculate(\"abc\", \"abcd\")")),

                    Case(suite, "TrailingWhitespace", "Trailing whitespace reduces the score",
                        () => Expect.Equal(0.5625m, StringSimilarity.Calculate("abc ", "abc"), "Calculate(\"abc \", \"abc\")")),

                    Case(suite, "RepeatedCharacterLength", "Repeated character differing only in length scores length ratio",
                        () =>
                        {
                            Expect.Equal(0.25m, StringSimilarity.Calculate("a", "aaaa"), "Calculate(\"a\", \"aaaa\")");
                            Expect.Equal(0.5m, StringSimilarity.Calculate("abc", "aabbcc"), "Calculate(\"abc\", \"aabbcc\")");
                        }),

                    Case(suite, "SubstringOfSentence", "Word contained in a longer phrase scores 5/22",
                        () => Expect.Approximately(5m / 22m, StringSimilarity.Calculate("hello world", "hello"), "Calculate(\"hello world\", \"hello\")")),

                    Case(suite, "AnagramScoresOne", "Anagrams score 1 (character order is not considered)",
                        () =>
                        {
                            Expect.Equal(1m, StringSimilarity.Calculate("abc", "cba"), "Calculate(\"abc\", \"cba\")");
                            Expect.Equal(1m, StringSimilarity.Calculate("listen", "silent"), "Calculate(\"listen\", \"silent\")");
                        }),

                    Case(suite, "CharacterFrequencyIgnored", "Same length and same distinct characters score 1 regardless of frequency",
                        () => Expect.Equal(1m, StringSimilarity.Calculate("aab", "abb"), "Calculate(\"aab\", \"abb\")")),

                    Case(suite, "CloserMatchScoresHigher", "A closer match scores higher than a more distant match",
                        () =>
                        {
                            decimal close = StringSimilarity.Calculate("abcdef", "abcdeg");
                            decimal far = StringSimilarity.Calculate("abcdef", "abcxyz");
                            decimal none = StringSimilarity.Calculate("abcdef", "uvwxyz");
                            Expect.GreaterThan(close, far, "close versus far");
                            Expect.GreaterThan(far, none, "far versus none");
                            Expect.Approximately(5m / 6m, close, "Calculate(\"abcdef\", \"abcdeg\")");
                        }),

                    Case(suite, "NonIdenticalBelowOne", "Non-identical strings that differ in length score below 1",
                        () => Expect.True(StringSimilarity.Calculate("test", "tests") < 1m, "Calculate(\"test\", \"tests\") should be below 1"))
                });
        }

        /// <summary>
        /// Case sensitivity and non-alphanumeric character handling.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor CaseAndCharacterSuite()
        {
            const string suite = "CaseAndCharacters";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Case and Character Handling",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "CaseSensitiveFullMismatch", "Comparison is case sensitive (ABC versus abc returns 0)",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("ABC", "abc"), "Calculate(\"ABC\", \"abc\")")),

                    Case(suite, "CaseSensitivePartialMismatch", "Single case difference reduces the score",
                        () => Expect.Equal(0.75m, StringSimilarity.Calculate("Hello", "hello"), "Calculate(\"Hello\", \"hello\")")),

                    Case(suite, "PunctuationCounts", "Punctuation is considered when scoring",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("!!!", "???"), "Calculate(\"!!!\", \"???\")")),

                    Case(suite, "Digits", "Digits are scored like any other character",
                        () =>
                        {
                            Expect.Equal(1m, StringSimilarity.Calculate("12345", "54321"), "Calculate(\"12345\", \"54321\")");
                            Expect.Equal(0m, StringSimilarity.Calculate("123", "456"), "Calculate(\"123\", \"456\")");
                        }),

                    Case(suite, "AccentedCharacters", "Accented characters are distinct from unaccented characters",
                        () => Expect.Equal(0.75m, StringSimilarity.Calculate("café", "cafe"), "Calculate(\"cafe-acute\", \"cafe\")")),

                    Case(suite, "NonLatinScript", "Non-Latin scripts are scored",
                        () =>
                        {
                            Expect.Equal(1m, StringSimilarity.Calculate("日本", "本日"), "Calculate(CJK, CJK reversed)");
                            Expect.Equal(0m, StringSimilarity.Calculate("日本", "ab"), "Calculate(CJK, \"ab\")");
                        }),

                    Case(suite, "EmojiVersusAscii", "Emoji versus ASCII text returns 0",
                        () => Expect.Equal(0m, StringSimilarity.Calculate("\U0001F600", "ab"), "Calculate(emoji, \"ab\")"))
                });
        }

        /// <summary>
        /// General properties that must hold for any input pair.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor PropertySuite()
        {
            const string suite = "Properties";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Score Properties",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "Symmetric", "Score is symmetric for known pairs",
                        () =>
                        {
                            string[][] pairs = new string[][]
                            {
                                new string[] { "abc", "abcd" },
                                new string[] { "hello world", "hello" },
                                new string[] { "Hello", "hello" },
                                new string[] { "abc", "abd" },
                                new string[] { "a", "aaaa" },
                                new string[] { "abc", "" }
                            };

                            foreach (string[] pair in pairs)
                            {
                                decimal forward = StringSimilarity.Calculate(pair[0], pair[1]);
                                decimal reverse = StringSimilarity.Calculate(pair[1], pair[0]);
                                Expect.Equal(forward, reverse, "Symmetry for (\"" + pair[0] + "\", \"" + pair[1] + "\")");
                            }
                        }),

                    Case(suite, "RandomPairsInRangeAndSymmetric", "Random pairs score between 0 and 1 and are symmetric",
                        () =>
                        {
                            Random random = new Random(12345);

                            for (int i = 0; i < 500; i++)
                            {
                                string a = RandomString(random, random.Next(0, 40));
                                string b = RandomString(random, random.Next(0, 40));
                                decimal forward = StringSimilarity.Calculate(a, b);
                                decimal reverse = StringSimilarity.Calculate(b, a);
                                Expect.InUnitRange(forward, "Calculate(\"" + a + "\", \"" + b + "\")");
                                Expect.Equal(forward, reverse, "Symmetry for (\"" + a + "\", \"" + b + "\")");
                            }
                        }),

                    Case(suite, "RandomSelfComparisonIsOne", "Any string compared with itself scores 1",
                        () =>
                        {
                            Random random = new Random(54321);

                            for (int i = 0; i < 200; i++)
                            {
                                string a = RandomString(random, random.Next(0, 40));
                                Expect.Equal(1m, StringSimilarity.Calculate(a, a), "Calculate(\"" + a + "\", self)");
                            }
                        }),

                    Case(suite, "Deterministic", "Repeated calls return the same score",
                        () =>
                        {
                            decimal first = StringSimilarity.Calculate("similarity", "similar");
                            for (int i = 0; i < 100; i++)
                                Expect.Equal(first, StringSimilarity.Calculate("similarity", "similar"), "Repeated Calculate call " + i);
                        }),

                    Case(suite, "ThreadSafe", "Concurrent calls return consistent scores",
                        () =>
                        {
                            decimal expected = StringSimilarity.Calculate("concurrency", "currency");
                            decimal[] results = new decimal[64];
                            Parallel.For(0, results.Length, i =>
                            {
                                results[i] = StringSimilarity.Calculate("concurrency", "currency");
                            });

                            for (int i = 0; i < results.Length; i++)
                                Expect.Equal(expected, results[i], "Concurrent result " + i);
                        })
                });
        }

        /// <summary>
        /// Large input handling.
        /// </summary>
        /// <returns>Suite descriptor.</returns>
        public static TestSuiteDescriptor ScaleSuite()
        {
            const string suite = "Scale";

            return new TestSuiteDescriptor(
                suiteId: suite,
                displayName: "Large Inputs",
                cases: new List<TestCaseDescriptor>
                {
                    Case(suite, "LargeIdentical", "Large identical strings return 1",
                        () =>
                        {
                            string a = new string('x', 100000) + "end";
                            string b = new string('x', 100000) + "end";
                            Expect.Equal(1m, StringSimilarity.Calculate(a, b), "Calculate(large, large)");
                        }),

                    Case(suite, "LargeVersusSmall", "Large versus single character scores the length ratio",
                        () => Expect.Equal(0.001m, StringSimilarity.Calculate(new string('a', 1000), "a"), "Calculate(1000 x 'a', \"a\")")),

                    Case(suite, "LargeDistinctCharacterSet", "Strings with many distinct characters are scored",
                        () =>
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < 5000; i++) sb.Append((char)(0x4E00 + i));
                            string a = sb.ToString();
                            string b = a.Substring(0, 2500);
                            Expect.Equal(0.25m, StringSimilarity.Calculate(a, b), "Calculate(5000 distinct, first 2500)");
                        })
                });
        }

        private static TestCaseDescriptor Case(string suiteId, string caseId, string displayName, Action body)
        {
            return new TestCaseDescriptor(
                suiteId: suiteId,
                caseId: caseId,
                displayName: displayName,
                executeAsync: ct =>
                {
                    ct.ThrowIfCancellationRequested();
                    body();
                    return Task.CompletedTask;
                });
        }

        private static string RandomString(Random random, int length)
        {
            const string alphabet = "abcdeABCDE012 !é";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++) chars[i] = alphabet[random.Next(alphabet.Length)];
            return new string(chars);
        }
    }
}
