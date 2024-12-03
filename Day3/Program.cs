using System.Text.RegularExpressions;

var input = File.ReadAllText("Input.txt");
var pattern = @"mul\((\d{1,3}),\s*(\d{1,3})\)";
var dontPattern = @"don't\(\)";
var doPattern = @"do\(\)";

var mulMatches = Regex.Matches(input, pattern)
                              .Cast<Match>()
                              .Select(m => new
                              {
                                  Number1 = m.Groups[1].Value,
                                  Number2 = m.Groups[2].Value,
                                  Index = m.Index
                              }).ToList();
var silverStarSum = mulMatches.Sum(m => int.Parse(m.Number1) * int.Parse(m.Number2));
var goldStarSum = mulMatches
        .Where(m => IsMulValid(m.Index, input, dontPattern, doPattern))
        .Sum(m => int.Parse(m.Number1) * int.Parse(m.Number2));

Console.WriteLine($"Silver Star: {silverStarSum}");
Console.WriteLine($"Gold Star: {goldStarSum}");

static bool IsMulValid(int mulIndex, string input, string dontPattern, string doPattern)
{
    var dontMatch = Regex.Matches(input.Substring(0, mulIndex), dontPattern)
                         .Cast<Match>()
                         .LastOrDefault();

    var doMatch = Regex.Matches(input.Substring(0, mulIndex), doPattern)
                       .Cast<Match>()
                       .LastOrDefault();

    return dontMatch == null || doMatch != null && doMatch.Index > dontMatch.Index;
}