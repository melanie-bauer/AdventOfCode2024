var XMAS = new char[4] { 'X', 'M', 'A', 'S' };
(int RowOffset, int ColOffset)[] directions =
    [
        (-1,  0),
        ( 1,  0),
        ( 0, -1),
        ( 0,  1),
        (-1, -1),
        (-1,  1),
        ( 1, -1),
        ( 1,  1)
    ];

var grid = File.ReadLines("Input.txt").Select(line => line.ToCharArray()).ToArray();

var silverStarSum = 0;
var indicesX = grid
            .SelectMany((row, rowIndex) => row
                .Select((ch, colIndex) => (Row: rowIndex, Column: colIndex, Char: ch))
                .Where(item => item.Char == 'X'));
foreach (var (Row, Column, Char) in indicesX)
{
    foreach (var direction in directions)
    {
        FindXmas(grid, Row, Column, direction, 0, ref silverStarSum);
    }
}

var goldStarSum = 0;
var indicesA = grid.SelectMany((row, rowIndex) => row
                        .Select((ch, colIndex) => (Row: rowIndex, Column: colIndex, Char: ch))
                        .Where(item => item.Char == 'A'
                                    && item.Row > 0 && item.Row < grid.Length - 1
                                    && item.Column > 0 && item.Column < grid[item.Row].Length - 1));
foreach (var (Row, Column, Char) in indicesA)
{
    CheckIfMAS(grid, Row, Column, ref goldStarSum);
}

Console.WriteLine($"Silver Star: {silverStarSum}");
Console.WriteLine($"Gold Star: {goldStarSum}");

void FindXmas(char[][] grid, int row, int col, (int RowOffset, int ColOffset) direction, int step, ref int sum)
{
    if (step == XMAS.Length - 1)
    {
        sum++;
        return;
    }

    int newRow = row + direction.RowOffset;
    int newCol = col + direction.ColOffset;

    if (newRow >= 0 && newRow < grid.Length &&
        newCol >= 0 && newCol < grid[newRow].Length &&
        grid[newRow][newCol] == XMAS[step + 1])
    {
        FindXmas(grid, newRow, newCol, direction, step + 1, ref sum);
    }
}

void CheckIfMAS(char[][] grid, int row, int col, ref int sum)
{
    var positionsToCheck = new[]{
        new[]{
            (Row: row - 1, Col: col - 1),
            (Row: row + 1, Col: col + 1)
        },
        [
            (Row: row - 1, Col: col + 1),
            (Row: row + 1, Col: col - 1)
        ]
    };
    var isValid = positionsToCheck.All(pos =>
                        (grid[pos[0].Row][pos[0].Col] == 'S' && grid[pos[1].Row][pos[1].Col] == 'M') ||
                        (grid[pos[0].Row][pos[0].Col] == 'M' && grid[pos[1].Row][pos[1].Col] == 'S'));

    if (isValid)
    { sum++; }
}
