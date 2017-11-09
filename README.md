# StringSimilarity

[![][nuget-img]][nuget]

[nuget]:     https://www.nuget.org/packages/StringSimilarity
[nuget-img]: https://badge.fury.io/nu/Object.svg

## Simple US-English String Similarity Score Calculator

This class library calculates a score from 0 to 1 based on the similarity of two supplied strings.

## Help or Feedback

First things first - do you need help or have feedback?  Contact me at joel at maraudersoftware.com dot com or file an issue here!

## New in v1.0.0

- First release
 
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
