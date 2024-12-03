using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("TestInput.txt")
            .Select(line => line.Split("   "))
            .Select(parts => (Left: int.Parse(parts[0]), Right: int.Parse(parts[1])))
            .ToList();

var silverStarSum = input
    .OrderBy(x => x.Left).Zip(input.OrderBy(x => x.Right),
        (l, r) => Math.Abs(l.Left - r.Right))
    .Sum();

var goldStarSum = input
    .Select(x => x.Left * input.Count(y => y.Right == x.Left))
    .Sum();

Console.WriteLine($"Silver Star: {silverStarSum}");
Console.WriteLine($"Gold Star: {goldStarSum}");