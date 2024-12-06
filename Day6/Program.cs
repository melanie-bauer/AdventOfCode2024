(int RowOffset, int ColOffset)[] directions =
    [
        (-1,  0),
        ( 0,  1),
        ( 1,  0),
        ( 0, -1),
        
    ];

var grid = File.ReadLines("Input.txt").Select(line => line.ToCharArray()).ToArray();

var sentinelPosition = grid
    .SelectMany((row, rowIndex) => row
        .Select((ch, colIndex) => (Row: rowIndex, Column: colIndex, Char: ch)))
    .First(pos => pos.Char == '^');

var visitedPositions = SimulateSentinel(grid, (sentinelPosition.Row, sentinelPosition.Column), 0);

var silverStarSum = visitedPositions.Count;

var possiblePositions = grid
    .SelectMany((row, rowIndex) => row
        .Select((ch, colIndex) => (Row: rowIndex, Column: colIndex, Char: ch)))
    .Where(pos => pos.Char == '.') 
    .ToList();

var goldStarSum = possiblePositions.Count(pos =>
{
    grid[pos.Row][pos.Column] = '#';
    var causesLoop = IsInfiniteLoop(grid, (sentinelPosition.Row, sentinelPosition.Column), 0);
    grid[pos.Row][pos.Column] = '.';
    return causesLoop;
});

Console.WriteLine($"Silver Star: {silverStarSum}");
Console.WriteLine($"Gold Star: {goldStarSum}");

HashSet<(int Row, int Column)> SimulateSentinel(char[][] grid, (int Row, int Column) startPosition, int startDirection)
{
    var visitedPositions = new HashSet<(int Row, int Column)> { startPosition };
    var currentPosition = startPosition;
    var directionIndex = startDirection;

    while (true)
    {
        var (rowOffset, colOffset) = directions[directionIndex];
        var newPosition = (Row: currentPosition.Row + rowOffset, Column: currentPosition.Column + colOffset);

        if (newPosition.Row < 0 || newPosition.Row >= grid.Length ||
            newPosition.Column < 0 || newPosition.Column >= grid[newPosition.Row].Length)
        {
            break; 
        }

        if (grid[newPosition.Row][newPosition.Column] == '#')
        {
            directionIndex = (directionIndex + 1) % 4; 
        }
        else
        {
            currentPosition = newPosition;
            visitedPositions.Add(currentPosition);
        }
    }

    return visitedPositions;
}

bool IsInfiniteLoop(char[][] grid, (int Row, int Column) startPosition, int startDirection)
{
    var visitedStates = new HashSet<(int Row, int Column, int DirectionIndex)>();
    var currentPosition = startPosition;
    var directionIndex = startDirection;

    while (true)
    {
        if (!visitedStates.Add((currentPosition.Row, currentPosition.Column, directionIndex)))
        {
            return true; 
        }

        var (rowOffset, colOffset) = directions[directionIndex];
        var newPosition = (Row: currentPosition.Row + rowOffset, Column: currentPosition.Column + colOffset);

        if (newPosition.Row < 0 || newPosition.Row >= grid.Length ||
            newPosition.Column < 0 || newPosition.Column >= grid[newPosition.Row].Length)
        {
            return false; 
        }

        if (grid[newPosition.Row][newPosition.Column] == '#')
        {
            directionIndex = (directionIndex + 1) % 4; 
        }
        else
        {
            currentPosition = newPosition;
        }
    }
}
