var reports = File.ReadAllLines("TestInput.txt");

var silverStarSum = reports
    .Select(report => report.Split(' ').Select(int.Parse).ToList())
    .Count(values => CheckReport(values));

Console.WriteLine($"Silver Star: {silverStarSum}");

var goldStarSum = reports
    .Select(report => report.Split(' ').Select(int.Parse).ToList())
    .Count(values => CheckReport(values) ||
                     values.Select((_, i) => values.Where((_, j) => j != i).ToList())
                           .Any(newValues => CheckReport(newValues)));

Console.WriteLine($"Gold Star: {goldStarSum}");

bool CheckReport(List<int> parts)
{
    var differences = parts.Zip(parts.Skip(1), (a, b) => b - a).ToList();

    if (differences.Any(diff => Math.Abs(diff) < 1 || Math.Abs(diff) > 3))
        return false;

    bool isIncreasing = differences.All(diff => diff > 0);
    bool isDecreasing = differences.All(diff => diff < 0);

    return isIncreasing || isDecreasing;
}