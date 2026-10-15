
using System.Text.RegularExpressions;

public static class DragonKnights
{
    private const int _capacity = 5;
    private const int _maxPerClass = 2;
    private enum knigtClass
    {
        swordsmen, 
        archer, 
        mage,
        healer
    }

    public static void Main(string[] args)
    {
        var input = args.Length > 0 ? string.Join(" ", args) : Console.ReadLine() ?? "";
        var n = Regex.Matches(input, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
        if (n.Length != 4)
        {
            Console.WriteLine("Expected 4 input:");
            return;
        }

        Console.WriteLine("return test");
    }
}
