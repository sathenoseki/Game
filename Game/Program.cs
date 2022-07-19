// See https://aka.ms/new-console-template for more information

var l = new List<Player>()
{
    new Player1(),
    new Player2()
};

var games = CreateGames(l);

foreach (var (p1, p2) in games)
{
   Console.WriteLine($"Player 1 {p1.Name}");
   Console.WriteLine($"Player 2 {p2.Name}");
   var p1Result = 0;
   var p2Result = 0;
   for (int i = 0; i < 5; i++)
   {
       var results = PlayGame(p1, p2).Select(Calculate);
       var p1R = results.Sum(r => r.Item1);
       var p2R = results.Sum(r => r.Item2);
       Console.WriteLine($"Player 1 Game Total = {p1R}");
       Console.WriteLine($"Player 2 Game Total = {p2R}");
       p1Result += p1R;
       p2Result += p2R;
       p1.Reset();
       p2.Reset();
   }
   
   Console.WriteLine($"Player 1 Total = {p1Result}");
   Console.WriteLine($"Player 2 Total = {p2Result}");
   Console.WriteLine(p1Result > p2Result ? "Player 1 Wins" : p1Result < p2Result ? "Player 2 Wins" : "We are all tied."  );
}

IEnumerable<(T, T)> CreateGames<T>(List<T> list) where T: class
{

    var l = new List<(T, T)>();
    for (int i = 0; i < list.Count; i++)
    {
        for (int j = 0; j < list.Count - 1; j++)
        {
            l.Add((list[i], list[j + 1]));
        }
    }
    
    return l;
}

IEnumerable<Result> PlayGame(Player p1, Player p2)
{
    var plays = new List<Result>();
    bool? p1Result = null;
    bool? p2Result = null;
    var random = new Random();

    for (int i = 0; i < random.Next(1000, 100_000); i++)
    {
        var temp1 = p1Result;
        p1Result = p1.Go(p2Result);
        p2Result = p2.Go(temp1);
        plays.Add(new Result(p1Result.Value, p2Result.Value));
    }

    return plays;
}

(int, int) Calculate(Result result)
{
    return (result.P1, result.P2) switch
    {
        (true, true) => (2, 2),
        (true, false) => (-1, 4),
        (false, true) => (4, -1),
        (false, false) => (0, 0)
    };
}

public record Result(bool P1, bool P2);


public abstract class Player
{
    public abstract bool Go(bool? lastPlay);
    public abstract string Name { get; }
    public abstract void Reset();
}


public class Player1 : Player
{
    private readonly Random _random = new Random();
    public override bool Go(bool? lastPlay)
    {
       var n= _random.Next(500);
       return n % 2 == 0;
    }

    public override void Reset()
    {
        
    }

    public override string Name { get; } = "Player 1";
}

public class Player2 : Player
{
    private readonly Random _random = new Random();
    public override bool Go(bool? lastPlay)
    {
        var n= _random.Next(500);
        return n % 2 != 0;
    }

    public override void Reset()
    {
    }

    public override string Name { get; } = "Player 2";
}