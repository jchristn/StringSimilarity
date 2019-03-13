# StringSimilarity

This class library calculates a score from 0 to 1 based on the similarity of two supplied strings.

Effective v1.0.1, StringSimilarity is now targeted to both .NET Core 2.0 and .NET Framework 4.5.2.

[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/StringSimilarity
[nuget-img]: https://badge.fury.io/nu/Object.svg
 
## Help or Feedback

First things first - do you need help or have feedback?  Contact me at joel dot christner at gmail dot com or file an issue here!

## New in v1.0.1

- Retarget to .NET Core 2.0 and .NET Framework 4.5.2
 
## Score Computation

- If one string is null/empty and the other is not, the score is 0
- If both strings are null/empty, the score is 1
- Otherwise, a length score is multiplied by a character match score, where length score = (min / max) and character match score is (num matching / total)

## Example

Refer to the Test project for an example.
```
using Similarity;
...
Console.Write("String 1 : ");
string str1 = Console.ReadLine(); 
Console.Write("String 2 : ");
string str2 = Console.ReadLine();
Console.WriteLine("Score    : " + StringSimilarity.Calculate(str1, str2));
```
 
## Version History

Notes from previous versions (starting with v1.0.0) will be moved here.
v1.0.0
- First release
