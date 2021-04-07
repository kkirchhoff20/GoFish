/*future modifications: make user HUD remain fixed and update in real time (write game state to a file and pull from that); give computer players real names (perhaps randomly draw from a 
list/file of names); improve AI to take into account whether player solicitations were successful; consider refactoring giveCards() method; add online capability*/

using System;
using System.Collections.Generic;
public class Program
{
	static UserPlayer p;
	static ComputerPlayer c1;
	static ComputerPlayer c2;
	static List<string> deck;
	static int numberOfBooks;

	public static void Main(string[] args)
	{
		Console.Clear();
		Console.WriteLine("Welcome to GO FISH!");
		Console.Write("Please enter your name: ");
		p = new UserPlayer(Console.ReadLine());
		c1 = new ComputerPlayer("computer1");
		c2 = new ComputerPlayer("computer2");
		p.getOpponents(c1.getName(), c2.getName());
		c1.getOpponents(p.getName(), c2.getName());
		c2.getOpponents(p.getName(), c1.getName());
		bool resume = true;
		while (resume)
		{
			Console.Clear();
			setUpGame();
			resume = gameloop();
		}

		Console.WriteLine("Thank you for playing!");
	}

	public static void setUpGame()
	{
		deck = createDeck();
		shuffle(deck);

		//deal cards
		for (int i = 0; i < 7; i++)
		{
			p.takeCard(deck[0]);
			deck.RemoveAt(0);
			c1.takeCard(deck[0]);
			deck.RemoveAt(0);
			c2.takeCard(deck[0]);
			deck.RemoveAt(0);
		}
	}

	public static bool gameloop()
	{
		bool inPlay = true;
		while (inPlay)
		{
			Console.Clear();
			//display user HUD
			p.displayBooks();
			c1.displayBooks();
			c2.displayBooks();
			Console.Write("Your cards: ");
			p.printHand();

			Console.WriteLine("");

			//user player's turn
			string[] userReq = p.getRequest();
			c1.storeRequest(p.getName(), userReq[1]);
			c2.storeRequest(p.getName(), userReq[1]);
			processRequest(p, userReq);

			//computerplayer1's turn
			string[] comp1Request = c1.getRequest();
			c2.storeRequest(c1.getName(), comp1Request[1]);
			displayRequest(c1, comp1Request);
			processRequest(c1, comp1Request);

			//computerplayer2's turn
			string[] comp2Request = c2.getRequest();
			c1.storeRequest(c2.getName(), comp2Request[1]);
			displayRequest(c2, comp2Request);
			processRequest(c2, comp2Request);

			//end the game if all books have been obtained
			tallyBooks();
			if (numberOfBooks == 13)
				inPlay = false;

		}
		Console.WriteLine("Would you like to play again? Type yes or no.");
		string response = Console.ReadLine();
		while (!response.Equals("yes") || !response.Equals("no"))
		{
			Console.WriteLine("Invalid input.");
			response = Console.ReadLine();
		}
		if (response.Equals("yes"))
			return true;
		else
			return false;
	}

	public static void processRequest(IPlayable solicitor, string[] request)
	{
		//a flag indicating whether a player was successful in their request
		bool receivedCards;
		//the following chain of selection statements determines which player is being solicited, and invokes their giveCards() method
		if (request[0].Equals(p.getName()))
		{
			receivedCards = p.giveCards(request[1], solicitor);
			Console.ReadKey(true);
		}
		else if (request[0].Equals(c1.getName()))
		{
			receivedCards = c1.giveCards(request[1], solicitor);
			Console.ReadKey(true);
		}
		else
		{
			receivedCards = c2.giveCards(request[1], solicitor);
			Console.ReadKey(true);
		}
		//simulates the player failing to receive cards and drawing from the deck
		if (!receivedCards)
		{
			if (deck.Count > 0)
			{
				solicitor.takeCard(deck[0]);
				deck.RemoveAt(0);
			}
			else
			{
				Console.WriteLine("The deck has been exhuasted of cards.");
				Console.ReadKey(true);
			}
		}
	}

	//displays computer player requests on screen
	public static void displayRequest(ComputerPlayer c, string[] request)
	{
		Console.WriteLine(c.getName() + " asks " + request[0] + " for any " + request[1] + " cards");
		Console.ReadKey(true);
	}

	//adds up all collected books
	public static void tallyBooks()
	{
		numberOfBooks = p.getNumberOfBooks() + c1.getNumberOfBooks() + c2.getNumberOfBooks();
	}

	public static List<string> createDeck()
	{
		List<string> cards = new List<string>();
		char suit;
		for (int i = 0; i < 4; i++)
		{
			if (i == 0)
				suit = 'S';
			else if (i == 1)
				suit = 'C';
			else if (i == 2)
				suit = 'H';
			else
				suit = 'D';
			for (int j = 0; j < 14; j++)
			{
				if (j == 0)
					cards.Add("" + 'A' + suit);
				if (j >= 2 && j < 11)
					cards.Add("" + j + suit);
				if (j == 11)
					cards.Add("" + 'J' + suit);
				if (j == 12)
					cards.Add("" + 'Q' + suit);
				if (j == 13)
					cards.Add("" + 'K' + suit);
			}
		}
		return cards;
	}

	//an implementation of the Fisher-Yates shuffle algorithm
	//https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
	static void shuffle(List<string> deck)
	{
		var rand = new Random();
		int index = deck.Count;
		while (index > 1)
		{
			index--;
			int random = rand.Next(index + 1);
			String card = deck[random];
			deck[random] = deck[index];
			deck[index] = card;
		}
	}
}