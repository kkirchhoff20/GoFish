using System;
public interface IPlayable
{
	//returns player's name member variable
	string getName();

	//adds names of opponent's to player's opponents member variables
	void getOpponents(string o1, string o2);

	//adds card parameter to player's hand
	void takeCard(string card);

	//displays player's collected books to user HUD
	void displayBooks();

	//returns the number of books the player has collected
	int getNumberOfBooks();

	//gets player's request and returns it as the array [RequestedPlayer, CardValue]
	string[] getRequest();

	//checks the player's hand for the card being requested of them; if they have the card, give all to solicitingPlayer; if not, tell solicitingPlayer to Go Fish; method returns flag indicating if 
	//cards were exchanged or not
	bool giveCards(string request, IPlayable solicitingPlayer);

}