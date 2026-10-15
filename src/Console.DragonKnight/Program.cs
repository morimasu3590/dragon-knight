
using System.Text.RegularExpressions;

public static class DragonKnights
{
    private const int _capacity = 5;
    private const int _maxPerClass = 2;
    private enum KnightClassEnum
    {
        swordsmen, 
        archer, 
        mage,
        healer
    }

    private static readonly KnightClassEnum[] _priority =
    {
        KnightClassEnum.swordsmen,
        KnightClassEnum.archer,
        KnightClassEnum.mage,
        KnightClassEnum.healer
    };

    public static int[][] EstimateCapacity(int swordsmens, int archers, int mages, int healers)
    {
        // Knights still waiting for a seat, per class
        var remaining = new Dictionary<KnightClassEnum, int>
        {
            { KnightClassEnum.swordsmen, swordsmens },
            { KnightClassEnum.archer, archers },
            { KnightClassEnum.mage, mages },
            { KnightClassEnum.healer, healers }
        };

        var totalKnights = remaining.Values.Sum();
        if (totalKnights == 0) return [];

        int dragonTotal = (totalKnights + _capacity - 1) / _capacity;
        var teams = Enumerable.Range(0, dragonTotal).Select(_ => new int[4]).ToArray();

        // Pass 1: one of each class per dragon, until that class runs out
        foreach (var knightClass in _priority)
        {
            int col = (int)knightClass;
            for (int dragon = 0; dragon < dragonTotal && remaining[knightClass] > 0; dragon++)
            {
                teams[dragon][col]++;
                remaining[knightClass]--;
            }
        }

        // Pass 2: fill empty seats by priority, max 2 per class
        FillEmptySeats(teams, remaining, _maxPerClass);

        // Pass 3: anything still unplaced means other classes ran out, so drop the cap
        FillEmptySeats(teams, remaining, int.MaxValue);

        return teams;

    }

    private static void FillEmptySeats(int[][] teams, Dictionary<KnightClassEnum, int> remaining, int cap)
    {
        foreach (var knightClass in _priority)
        {
            int col = (int)knightClass;
            foreach (var team in teams)
            {
                while (remaining[knightClass] > 0 && team.Sum() < _capacity && team[col] < cap)
                {
                    team[col]++;
                    remaining[knightClass]--;
                }
            }
        }
    }

    public static string Format(int[][] teams) =>
        string.Join(Environment.NewLine, teams.Select((team, i) =>
            $"Dragon {i + 1}: " + string.Join(", ", _priority.Select(k => $"{team[(int)k]} {k}"))));
    
    public static void Main(string[] args)
    {
        var input = args.Length > 0 ? string.Join(" ", args) : Console.ReadLine() ?? "";
        var n = Regex.Matches(input, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
        if (n.Length != 4)
        {
            Console.WriteLine("Expected 4 input:");
            return;
        }

        var teams = EstimateCapacity(n[0], n[1], n[2], n[3]);
        Console.WriteLine(Format(teams));
    }
}
