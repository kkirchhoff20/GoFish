using System;
using System.Collections.Generic;

public class ComputerPlayer : IPlayable
{
	string name;
	string opponent1;
	string opponent2;
	List<string> hand = new List<string>();
	List<string> books = new List<string>();
	List<string[]> memory = new List<string[]>();

	public ComputerPlayer(string n)
	{
		name = n;
	}

	public string getName()
	{
		return name;
	}

	public void getOpponents(string name, string name2)
	{
		opponent1 = name;
		opponent2 = name2;
	}

	public void takeCard(string card)
	{
		hand.Add(card);
		collectBooks(card);
	}

	//called whenever a player receives a card; checks to see if all four cards of given value have been collected, and removes them from the player's hand and adds them to books-list member variable if so
	private void collectBooks(string card)
	{
		//stores indices of cards whose value matches the parameter card
		int[] indices = new int[4];
		//running total of how many cards of given value the player has
		int count = 0;
		//represents value of card at a given index in player's hand
		char value = card[0];
		//traverse player's hand...
		for (int i = 0; i < hand.Count; i++)
		{
			//... checking to see if given card is the same as the newly acquired parameter card
			//if so, add its index to indices[] array and increment the count variable
			if (hand[i][0] == value)
			{
				indices[count] = i;
				count++;
			}
		}
		//should player have all 4 cards of the parameter card's value, remove them from hand and add card value to books[]
		if (count == 4)
		{
			for (int i = 0; i < 4; i++)
				hand.RemoveAt(indices[i - count]);
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

	//allows the computer player to "remember" who has asked for what
	public void storeRequest(string solicitor, string request)
	{
		//nerf computer's memory to just 11 requests
		if (memory.Count > 10)
			memory.RemoveAt(0);
		memory.Add(new string[] { solicitor, request });
	}

	public string[] getRequest()
	{
		var rand = new Random();
		//generate random index; analogous to a human choosing a card they wish to ask for
		int randCardIndex = rand.Next(hand.Count);
		//represents a particular memory; for use as following code iterates through computer player's memory list
		string[] currentMemory;
		//iterate through hand
		for (int i = 0; i < hand.Count; i++)
		{
			//for each card in hand, check if computer player remembers another player requesting current card
			for (int j = 0; j < memory.Count; j++)
			{
				currentMemory = memory[j];
				//if the current card is of the same value as current memory, return that memory as computer player's request
				//(modulo operator allows loop to cycle-back to cards in hand that precede index of initial random index)
				if (hand[(randCardIndex + i) % hand.Count][0] == currentMemory[1][0])
					return currentMemory;
			}
		}
		//if the computer has no recollection of anyone asking for a card that they currently have, make a request[] using the randomly generated card index above and a random player
		string[] request = new string[2];
		string player, card;
		card = hand[randCardIndex][0] + "";
		int randPlayer = rand.Next(2);
		if (randPlayer == 0)
			player = opponent1;
		else
			player = opponent2;
		request[0] = player;
		request[1] = card;
		return request;
	}

	//see description in UserPlayer
	public bool giveCards(string request, IPlayable solicitingPlayer)
	{
		char value = request[0];
		int count = 0;
		for (int i = 0; i < hand.Count; i++)
		{
			if (hand[i - count][0] == value)
			{
				solicitingPlayer.takeCard(hand[i - count]);
				hand.RemoveAt(i - count);
				count++;
			}
		}
		if (count == 0)
		{
			Console.WriteLine("Go fish!");
			return false;
		}
		else
		{
			string properPlurality = (count > 1 ? " cards" : " card");
			Console.WriteLine(this.getName() + " hands " + solicitingPlayer.getName() + " " + count + properPlurality);
			return true;
		}
	}
	//for testing/debugging
	public void printHand()
	{
		for (int i = 0; i < hand.Count; i++)
			Console.Write(hand[i] + " ");
		Console.WriteLine("");
	}
}