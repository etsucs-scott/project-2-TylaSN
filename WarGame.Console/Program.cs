using System;
using System.Collections.Generic;
using WarGame.Core;

namespace WarGame.ConsoleApp
{
    class Program
    {
        //Main Program 
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to War Card Game!");

            List<string> playerNames = new List<string>();
            int playerCount = 0;

            // Ask for number of players (2-4)
            while (playerCount < 2 || playerCount > 4)
            {
                Console.Write("Enter number of players (2-4): ");
                int.TryParse(Console.ReadLine(), out playerCount);
            }

            // Ask for player names or use defaults
            for (int i = 1; i <= playerCount; i++)
            {
                Console.Write($"Enter name for Player {i} (or press Enter for default): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    name = $"Player {i}";
                playerNames.Add(name);
            }

            // Initialize and start the game
            WarCardGame game = new WarCardGame(playerNames);
            game.Start();

            Console.WriteLine("\nGame Over! Press any key to exit.");
            Console.ReadKey();
        }
    }
}