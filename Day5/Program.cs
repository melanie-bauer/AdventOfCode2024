var path = "Input.txt";

var orderingRules = File.ReadAllText(path).Split("\r\n\r\n")[0].Split("\r\n")
                        .Select(x => x.Split('|'))
                        .Select(x => new Rule(int.Parse(x[0]), int.Parse(x[1]))).ToList();

var inputs = File.ReadAllText(path).Split("\r\n\r\n")[1].Split("\r\n").Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();

var silverStarSum = inputs.Where(line =>
        line
            .Select((number, index) => new
            {
                Number = number,
                Required = orderingRules
                    .Where(rule => rule.After == number)
                    .Select(rule => rule.Before).Where(rule => line.Contains(rule)).ToList()
            })
            .All(item =>
                item.Required.All(required =>
                    line.TakeWhile(n => n != item.Number).Contains(required))))
    .Sum(line => line[line.Count / 2]);

var goldStarSum = inputs.Where(line =>
        !line
            .Select((number, index) => new
            {
                Number = number,
                Required = orderingRules
                    .Where(rule => rule.After == number)
                    .Select(rule => rule.Before).Where(rule => line.Contains(rule)).ToList()
            })
            .All(item =>
                item.Required.All(required =>
                    line.TakeWhile(n => n != item.Number).Contains(required))))
    .Select(line =>
    {
        var sortedLine = new List<int>();
        var remainingNumbers = new HashSet<int>(line);

        while (remainingNumbers.Count > 0)
        {
            var nextNumber = remainingNumbers.FirstOrDefault(num =>
                orderingRules
                    .Where(rule => rule.After == num)
                    .Select(rule => rule.Before)
                    .Where(required => remainingNumbers.Contains(required))
                    .All(required => sortedLine.Contains(required)));

            sortedLine.Add(nextNumber);
            remainingNumbers.Remove(nextNumber);
        }

        return sortedLine;
    }).Sum(line => line[line.Count / 2]);

Console.WriteLine($"Silver Star: {silverStarSum}");
Console.WriteLine($"Gold Star: {goldStarSum}");

record Rule(int Before, int After);