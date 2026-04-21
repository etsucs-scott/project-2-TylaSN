using System;
using System.Collections.Generic;
using System.Linq;

namespace WarGame.Core
{
    // Enum for the suits
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    // Enum for card ranks
    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    // Represents a single playing card
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        // Print card as "Rank"
        public override string ToString() => $"{Rank}";
    }

    //class for deck
    public class Deck
    {
        private Stack<Card> cards;

        public Deck()
        {
            var allCards = new List<Card>();

            // Generate standard card deck
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    allCards.Add(new Card(suit, rank));
                }
            }

            cards = new Stack<Card>(allCards);
        }

        // Shuffles deck
        public void Shuffle()
        {
            var array = cards.ToArray();
            Random rng = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            cards = new Stack<Card>(array);
        }

        // Draws top card
        public Card DrawCard() => cards.Pop();
        public int Count => cards.Count;
    }

    // Represents a player
    public class Player
    {
        public string Name { get; set; }
        public Queue<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new Queue<Card>();
        }

        public bool HasCards() => Hand.Count > 0;

        //Play top card
        public Card PlayCard()
        {
            return HasCards() ? Hand.Dequeue() : null;
        }

        // Collect cards to back of hand
        public void CollectCards(List<Card> cards)
        {
            foreach (var card in cards)
                Hand.Enqueue(card);
        }

        public int CardCount() => Hand.Count;
    }

    // Holds all player hands
    public class PlayerHands
    {
        public Dictionary<string, Player> Players { get; set; } = new Dictionary<string, Player>();

        public void AddPlayer(Player player)
        {
            Players[player.Name] = player;
        }

        // Returns list of players still in game
        public List<Player> ActivePlayers()
        {
            return Players.Values.Where(p => p.HasCards()).ToList();
        }
    }

    // War game engine
    public class WarCardGame
    {
        private Deck deck;
        private PlayerHands playerHands;
        private List<Card> pot;
        private const int ROUND_LIMIT = 10000;

        public WarCardGame(List<string> names)
        {
            deck = new Deck();
            deck.Shuffle();

            playerHands = new PlayerHands();
            foreach (var name in names)
            {
                playerHands.AddPlayer(new Player(name));
            }

            DealCards();
        }

        // Deal cards round-robin
        private void DealCards()
        {
            var players = playerHands.Players.Values.ToList();
            int i = 0;

            while (deck.Count > 0)
            {
                players[i % players.Count].Hand.Enqueue(deck.DrawCard());
                i++;
            }
        }

        // Start the game
        public void Start()
        {
            pot = new List<Card>();
            int round = 0;

            while (playerHands.ActivePlayers().Count > 1 && round < ROUND_LIMIT)
            {
                round++;
                Console.WriteLine($"\nRound {round}");

                PlayRound(playerHands.ActivePlayers());
            }

            EndGame();
        }

        // Play a single round
        private void PlayRound(List<Player> players)
        {
            pot.Clear();
            Dictionary<Player, Card> played = new Dictionary<Player, Card>();

            foreach (var player in players)
            {
                var card = player.PlayCard();
                if (card != null)
                {
                    played[player] = card;
                    pot.Add(card);
                    Console.WriteLine($"{player.Name}: {card}");
                }
            }

            ResolveRound(played);
        }

        // Determine winner(s) of round
        private void ResolveRound(Dictionary<Player, Card> played)
        {
            int max = played.Max(x => (int)x.Value.Rank);

            var winners = played
                .Where(x => (int)x.Value.Rank == max)
                .Select(x => x.Key)
                .ToList();

            if (winners.Count == 1)
            {
                var winner = winners[0];
                winner.CollectCards(pot);

                PrintCounts(winner);
            }
            else
            {
                Console.WriteLine($"Tie between {string.Join(" & ", winners.Select(w => w.Name))}");
                Tiebreaker(winners);
            }
        }

        // Handle tiebreakers
        private void Tiebreaker(List<Player> tied)
        {
            Dictionary<Player, Card> played = new Dictionary<Player, Card>();

            foreach (var player in tied)
            {
                if (player.HasCards())
                {
                    var card = player.PlayCard();
                    played[player] = card;
                    pot.Add(card);
                    Console.WriteLine($"Tiebreaker - {player.Name}: {card}");
                }
            }

            if (played.Count == 0)
            {
                Console.WriteLine("All tied players out of cards!");
                return;
            }

            int max = played.Max(x => (int)x.Value.Rank);

            var winners = played
                .Where(x => (int)x.Value.Rank == max)
                .Select(x => x.Key)
                .ToList();

            if (winners.Count == 1)
            {
                var winner = winners[0];
                winner.CollectCards(pot);

                Console.WriteLine($"Tiebreaker Winner: {winner.Name}");
                PrintCounts(winner);
            }
            else
            {
                Console.WriteLine("Another tie!");
                Tiebreaker(winners);
            }
        }

        // Print current card counts
        private void PrintCounts(Player winner)
        {
            Console.Write($"Winner: {winner.Name} | Cards: ");
            foreach (var p in playerHands.Players.Values)
            {
                Console.Write($"{p.Name}={p.CardCount()} ");
            }
            Console.WriteLine();
        }

        // End of game summary
        private void EndGame()
        {
            var players = playerHands.Players.Values.ToList();
            int max = players.Max(p => p.CardCount());

            var winners = players.Where(p => p.CardCount() == max).ToList();

            if (winners.Count == 1)
            {
                Console.WriteLine($"\nFinal Winner: {winners[0].Name}");
            }
            else
            {
                Console.WriteLine("\nGame ended in a draw.");
            }
        }
    }
}