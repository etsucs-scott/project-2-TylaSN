# War Game (Console App)

## Intro  
This project is a console-based simulation of the card game War, built in C#. The goal of this assignment was to model the game using core data structures like stacks, queues, lists, and dictionaries while keeping the game logic separate from the console layer.

The program runs entirely in the console and prints each round so the game can be followed step by step.

---

## Project Structure  

The solution is split into two parts:

- WarGame.Core:  
  Contains all game logic, including:
  - Card and deck creation  
  - Player handling  
  - Round logic and tiebreakers  

- WarGame.Console:  
  Handles user input and prints output to the console.

All rules and game state are kept inside the core project as required.

---

## Features  

- Standard 52-card deck using enums for Suit and Rank  
- Deck implemented using a `Stack<Card>`  
- Player hands implemented using `Queue<Card>`  
- Players stored in a `Dictionary<string, Player>`  
- Shared pot stored in a `List<Card>`  
- Round-by-round simulation with readable console output  
- Recursive tiebreaker system for handling ties  
- Automatic handling of players running out of cards  
- Round limit of 10,000 to prevent infinite games  

---

## How the Game Works  

- A deck is created and shuffled.
- Cards are dealt one at a time in round-robin order.
- Each round:
  - Players draw the top card from their hand
  - The highest rank wins the round
  - All played cards go into a shared pot
- If there is a tie:
  - Only tied players continue
  - Each plays another card
  - This repeats until a winner is found
- The winner collects all cards in the pot and adds them to the back of their hand

---

## Key Classes  

- Card– represents a playing card with Suit and Rank  
- Deck – creates and shuffles a stack of cards  
- Player – stores player name and hand (queue of cards)  
- PlayerHand– manages all players using a dictionary  
- WarCardGame – main game engine that controls rounds and game flow  

---

## Build & Run  

Build the project:

```bash
dotnet build