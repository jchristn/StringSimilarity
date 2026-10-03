# StringSimilarity

This class library calculates a score from 0 to 1 based on the similarity of two supplied strings.

StringSimilarity targets .NET Standard 2.1, .NET Core 3.1, .NET Framework 4.5.2, .NET 5.0, and .NET 6.0.

[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/StringSimilarity
[nuget-img]: https://badge.fury.io/nu/Object.svg
 
## Help or Feedback

First things first - do you need help or have feedback?  Contact me at joel dot christner at gmail dot com or file an issue here!

## New in v1.0.2.2

- Maintenance release; library behavior is unchanged
- Symbol package (`.snupkg`) now published alongside the NuGet package
- Test dependencies refreshed (Touchstone 0.2.0, xUnit runner 4.0.0, NUnit 5.0.0, Microsoft.NET.Test.Sdk 18.10.1, coverlet 10.1.0)
 
## Score Computation

- If one string is null/empty and the other is not, the score is 0
- If both strings are null/empty, the score is 1
- If both strings are equal, the score is 1
- Otherwise, a length score is multiplied by a character match score, where length score = (min / max) and character match score is (num matching / total)
  - The character match score compares the sets of distinct characters in each string; total is the number of distinct characters in whichever string has more
  - Comparison is case-sensitive and ordinal, so `ABC` versus `abc` scores 0
  - Character order and frequency are not considered, so anagrams of the same length (for example `listen` and `silent`) score 1

## Example

Refer to the Test project for an interactive example.
```csharp
using Similarity;

Console.Write("String 1 : ");
string str1 = Console.ReadLine(); 
Console.Write("String 2 : ");
string str2 = Console.ReadLine();
Console.WriteLine("Score    : " + StringSimilarity.Calculate(str1, str2));
```
 
## Running the Tests

Automated tests are built with [Touchstone](https://github.com/jchristn/touchstone).  All test cases live in `Test.Shared` and are executed by three runners:

- `Test.Automated` - console runner (`dotnet run --project Test.Automated -f net8.0`, add `-- --results results.json` to export JSON)
- `Test.Xunit` - xUnit runner (`dotnet test Test.Xunit`)
- `Test.Nunit` - NUnit runner (`dotnet test Test.Nunit`)

The `Test` project is an interactive console application for trying the library by hand.

## Version History

Refer to [CHANGELOG.md](CHANGELOG.md) for the full version history.
