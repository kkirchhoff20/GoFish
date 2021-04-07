using System;
using System.Collections.Generic;

public class UserPlayer : IPlayable
{
	private string name;
	private List<string> hand = new List<string>();
	private List<string> books = new List<string>();
	//a list of acceptable user input for when requesting cards
	private List<string> validUserInput = new List<string>();
	private string opponent1;
	private string opponent2;

	public UserPlayer(string n)
	{
		name = n;
	}

	public string getName()
	{
		return name;
	}

	public void getOpponents(string o1, string o2)
	{
		opponent1 = o1;
		opponent2 = o2;
	}

	public void takeCard(string card)
	{
		hand.Add(card);
		addToValidRequests(card);
		collectBooks(card);
	}

	//called whenever a player receives a card; checks to see if all four cards of given value have been collected, and removes them from the player's hand and adds them to books-list member variable if so
	private void collectBooks(string card)
	{
		int[] indices = new int[4];
		int count = 0;
		char value = card[0];
		for (int i = 0; i < hand.Count; i++)
		{
			if (hand[i][0] == value)
			{
				indices[count] = i;
				count++;
			}
		}
		if (count == 4)
		{
			for (int i = 0; i < 4; i++)
				hand.RemoveAt(indices[i] - i);
			books.Add(card);
		}
	}

	public void displayBooks()
	{
		Console.Write(getName() + ": ");
		if (books.Count > 0)
		{
			for (int i = 0; i < books.Count; i++)
				Console.Write(books[i] + " ");
		}
		Console.WriteLine("");
	}

	public int getNumberOfBooks()
	{
		return books.Count;
	}

	public string[] getRequest()
	{
		string player;
		string card;
		Console.Write("Enter the player whom you wish to solicit: ");
		player = Console.ReadLine();
		//ensure user enters valid opponent player name
		while (!player.Equals(opponent1) && !player.Equals(opponent2))
		{
			Console.WriteLine("Invalid input.");
			Console.Write("Enter the player's name: ");
			player = Console.ReadLine();
		}
		Console.Write("Enter the card you wish to solicit: ");
		card = Console.ReadLine();
		//ensure user enters valid card; user may not ask for a card that they do not currently possess
		while (!(validUserInput.Contains(card)))
		{
			Console.WriteLine("Invalid input.");
			Console.Write("Enter a valid card: ");
			card = Console.ReadLine();
		}
		return new string[] { player, card };
	}

	public bool giveCards(string request, IPlayable solicitingPlayer)
	{
		char value = request[0];
		//used to adjust hand index should more than one card given to soliciting player
		int count = 0;
		for (int i = 0; i < hand.Count; i++)
		{
			//compare the card the player is requesting to each card in user's hand
			//if they match, give to soliciting player (and remove from user's hand)
			if (hand[i - count][0] == value)
			{
				solicitingPlayer.takeCard(hand[i - count]);
				hand.RemoveAt(i - count);
				count++;
			}
		}
		//if user has no cards of the requested value tell player Go Fish
		if (count == 0)
		{
			Console.Write("Go fish!");
			return false;
		}
		else
		{
			string properPlurality = (count > 1 ? " cards" : " card");
			Console.WriteLine(this.getName() + " hands " + solicitingPlayer.getName() + " " + count + " cards.");
			return true;
		}
	}

	public void addToValidRequests(string card)
	{
		string value = card[0] + "";
		if (validUserInput.Contains(value))
			return;
		validUserInput.Add(value);
	}

	public void printHand()
	{
		for (int i = 0; i < hand.Count; i++)
			Console.Write(hand[i] + " ");
		Console.WriteLine("");
	}
}