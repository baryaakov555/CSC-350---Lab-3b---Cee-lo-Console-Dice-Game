using System;
using System.Threading;

public class Die
{
    public int NumberOfSides { get; private set; }
    public int TopSide { get; private set; }

    private static readonly Random random = new Random();

    public Die()
    {
        NumberOfSides = 6;
    }

    public Die(int numberOfSides)
    {
        NumberOfSides = numberOfSides;
    }

    public int Roll()
    {
        TopSide = random.Next(1, NumberOfSides + 1);
        return TopSide;
    }
}

public class Cee_lo
{
    static int money = 1000;

    public static void Main(string[] main)
    {
        while (true)
        {
            Console.WriteLine($"Balance: ${money}");
            Console.WriteLine();
            Console.WriteLine("=== Cee-lo ===");
            Console.WriteLine("1) Play ");
            Console.WriteLine("2) Rules ");
            Console.WriteLine("3) Quit ");
            Console.Write("\nChoose option: ");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Input is not a number, try again.\n");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Pause(500);
                    if (money <= 0)
                    {
                        Console.WriteLine("You’re broke. quit.");
                        break;
                    }
                    PlayRound();
                    break;
                case 2:
                    ShowRules();
                    break;
                case 3:
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option.\n");
                    break;
            }
        }

        void PlayRound()
        {
            Die die1 = new Die();
            Die die2 = new Die();
            Die die3 = new Die();

            int[] bankerDice = new int[3];
            int[] playerDice = new int[3];

            var bankerOutcome = CeeloRoundOutcomes.NoCombo;
            var playerOutcome = CeeloRoundOutcomes.NoCombo;

            int bankerPoint = -1;
            int playerPoint = -1;
            int bankerTriple = -1;
            int playerTriple = -1;

            int betAmount = AskForBetAmount();
            Pause(500);

            // ===== Banker's turn =====

            while (true)
            {
                Console.WriteLine($"\nBanker rolling...");
                Pause(1000);
                bankerDice[0] = die1.Roll();
                bankerDice[1] = die2.Roll();
                bankerDice[2] = die3.Roll();
                Console.WriteLine($"\nBanker rolled: {bankerDice[0]} {bankerDice[1]} {bankerDice[2]}");
                Pause(1000);

                bankerOutcome = RollCheck(bankerDice);

                switch (bankerOutcome)
                {
                    case CeeloRoundOutcomes.AutoWin456:
                        Console.WriteLine("\nBanker rolled 4-5-6, Banker wins!");
                        Pause(1000);
                        SubtractFunds(betAmount);
                        return;
                    case CeeloRoundOutcomes.AutoLose123:
                        Console.WriteLine("\nBanker rolled 1-2-3, you win!");
                        Pause(1000);
                        AddFunds(betAmount);
                        return;
                    case CeeloRoundOutcomes.Triples:
                        bankerTriple = GetTripleValue(bankerDice);
                        Console.WriteLine($"\nBanker rolled triples with the value of {bankerTriple}.");
                        Pause(1000);
                        break;
                    case CeeloRoundOutcomes.Point:
                        bankerPoint = GetPoint(bankerDice);
                        Console.WriteLine($"\nBanker set a point at the value of {bankerPoint}.");
                        Pause(1000);
                        break;
                    case CeeloRoundOutcomes.NoCombo:
                        Console.WriteLine("\nBanker rolled No combo. Rolling again…");
                        Pause(1000);
                        continue;
                }
                break;
            }

            // ===== Player's turn =====

            while (true)
            {
                Console.WriteLine($"\nIt's your turn, press any key to roll the dice");
                Console.ReadKey();
                Console.WriteLine($"\nRolling...");
                Pause(1000);
                playerDice[0] = die1.Roll();
                playerDice[1] = die2.Roll();
                playerDice[2] = die3.Roll();
                Console.WriteLine($"\nYou rolled: {playerDice[0]} {playerDice[1]} {playerDice[2]}");
                Pause(1000);

                playerOutcome = RollCheck(playerDice);

                switch (playerOutcome)
                {
                    case CeeloRoundOutcomes.AutoWin456:
                        Console.WriteLine("\nYou rolled a 4-5-6, You win!");
                        Pause(1000);
                        AddFunds(betAmount);
                        return;
                    case CeeloRoundOutcomes.AutoLose123:
                        Console.WriteLine("\nYou rolled a 1-2-3, You lose!");
                        Pause(1000);
                        SubtractFunds(betAmount);
                        return;
                    case CeeloRoundOutcomes.Triples:
                        playerTriple = GetTripleValue(playerDice);
                        Console.WriteLine($"\nYou rolled triples with the value of {playerTriple}");
                        break;
                    case CeeloRoundOutcomes.Point:
                        playerPoint = GetPoint(playerDice);
                        Console.WriteLine($"\nYou set a point at the value of {playerPoint}");
                        break;
                    case CeeloRoundOutcomes.NoCombo:
                        Console.WriteLine("\nNo combo. Rolling again…");
                        Pause(1000);
                        continue;
                }
                break;
            }

            Pause(1000);

            // ===== Results for triples and points =====

            if (bankerOutcome == CeeloRoundOutcomes.Triples && playerOutcome == CeeloRoundOutcomes.Triples)
            {
                if (playerTriple > bankerTriple)
                {
                    Console.WriteLine("\nYou win! (higher triples)");
                    Pause(1000);
                    AddFunds(betAmount);
                    return;
                }
                else if (playerTriple < bankerTriple)
                {
                    Console.WriteLine("\nBanker wins! (higher triples)!");
                    Pause(1000);
                    SubtractFunds(betAmount);
                    return;
                }
                else
                {
                    Console.WriteLine("\nTie on triples, restarting round...");
                    Pause(1000);
                    PlayRound();
                    return;
                }
            }

            if (bankerOutcome == CeeloRoundOutcomes.Triples && playerOutcome == CeeloRoundOutcomes.Point)
            {
                Console.WriteLine("\nBanker's triples beat your point, Banker wins!");
                Pause(1000);
                SubtractFunds(betAmount);
                return;
            }

            if (bankerOutcome == CeeloRoundOutcomes.Point && playerOutcome == CeeloRoundOutcomes.Triples)
            {
                Console.WriteLine("\nYour triples beat the banker's point, You win!");
                Pause(1000);
                AddFunds(betAmount);
                return;
            }

            if (bankerOutcome == CeeloRoundOutcomes.Point && playerOutcome == CeeloRoundOutcomes.Point)
            {
                if (playerPoint > bankerPoint)
                {
                    Console.WriteLine("\nYou win! (higher point)");
                    Pause(1000);
                    AddFunds(betAmount);
                    return;
                }
                else if (playerPoint < bankerPoint)
                {
                    Console.WriteLine("\nBanker wins! (higher point)");
                    Pause(1000);
                    SubtractFunds(betAmount);
                    return;
                }
                else
                {
                    Console.WriteLine("\nTie on points, restarting round...");
                    Pause(1000);
                    PlayRound();
                    return;
                }
            }
        }

        void ShowRules()
        {
            Console.WriteLine("\n=== Cee-lo Rules ===");
            Console.WriteLine("1. The game uses three dice.");
            Console.WriteLine("2. One player is the Banker. The other is the Player.");
            Console.WriteLine("3. Both roll until they get a valid outcome for their turn.");
            Console.WriteLine();
            Console.WriteLine("Valid outcomes:");
            Console.WriteLine("  roll 4-5-6: Automatic win.");
            Console.WriteLine("  roll 1-2-3: Automatic loss.");
            Console.WriteLine("  roll Triples: Higher triples beats lower triples. And triples beat any point");
            Console.WriteLine("  roll Point: A pair plus another die, the odd die becomes the point and a higher point wins.");
            Console.WriteLine();
            Console.WriteLine("Comparison rules:");
            Console.WriteLine("  Banker rolls first, if they don't win nor lose, they set the target.");
            Console.WriteLine("  Player then tries to beat the Banker's target result.");
            Console.WriteLine();
            Console.WriteLine("Betting:");
            Console.WriteLine("  You wager an amount before rolling.");
            Console.WriteLine("  If you win, you gain that amount.");
            Console.WriteLine("  If you lose, you lose that amount.");
            Console.WriteLine("======================");
            Console.WriteLine();
        }

        int AskForBetAmount()
        {
            while (true)
            {
                Console.WriteLine("\nHow much do you want to bet? ");
                Console.Write("Amount: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int betAmount))
                {
                    if (betAmount <= 0)
                    {
                        Console.WriteLine("Bet must be greater than 0.\n");
                        continue;
                    }
                    if (betAmount > money)
                    {
                        Console.WriteLine("You can’t bet more than your balance.\n");
                        continue;
                    }
                    return betAmount;
                }
                else
                {
                    Console.WriteLine("Input is not a number, try again.\n");
                }
            }
        }
    }

    public enum CeeloRoundOutcomes
    {
        AutoWin456,
        AutoLose123,
        Triples,
        Point,
        NoCombo
    }

    public static CeeloRoundOutcomes RollCheck(int[] diceResult)
    {
        Array.Sort(diceResult);

        if (diceResult[0] == 4 && diceResult[1] == 5 && diceResult[2] == 6)
        {
            return CeeloRoundOutcomes.AutoWin456;
        }

        if (diceResult[0] == 1 && diceResult[1] == 2 && diceResult[2] == 3)
        {
            return CeeloRoundOutcomes.AutoLose123;
        }

        if (diceResult[0] == diceResult[1] && diceResult[1] == diceResult[2])
        {
            return CeeloRoundOutcomes.Triples;
        }

        if (diceResult[0] == diceResult[1] || diceResult[1] == diceResult[2] || diceResult[0] == diceResult[2])
        {
            return CeeloRoundOutcomes.Point;
        }

        return CeeloRoundOutcomes.NoCombo;
    }

    static int GetPoint(int[] diceResult)
    {
        if (diceResult[0] == diceResult[1])
        {
            return diceResult[2];
        }
        if (diceResult[1] == diceResult[2])
        {
            return diceResult[0];
        }
        if (diceResult[0] == diceResult[2])
        {
            return diceResult[1];
        }

        return -1;
    }

    static int GetTripleValue(int[] diceResult)
    {
        if (diceResult[0] == diceResult[1] && diceResult[1] == diceResult[2])
        {
            return diceResult[0];
        }

        return -1;
    }

    static void AddFunds(int amount)
    {
        money += amount;
        Console.WriteLine($"\nYou won {amount}$ Balance is now {money}$\n");
        Pause(1000);


    }

    static void SubtractFunds(int amount)
    {
        money -= amount;
        Console.WriteLine($"\nYou lost {amount}$ Balance is now {money}$\n");
        Pause(1000);
    }

    static void Pause(int ms)
    {
        Thread.Sleep(ms);
    }
}