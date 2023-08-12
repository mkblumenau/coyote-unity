using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script runs the game, including dealing out cards, telling whose turn it is,
//and making gameplay actions happen.

public class GameManagerScript : MonoBehaviour
{
	public PlayerUIScript PlayerUI;
	//public DisplayUIManagerScript displayManager;
	public UpdateAllPlayerInfo update;
	public GameLogManager gameLog;
	
	//These are values that will be interpreted as the Max Zero and ? cards.
	//Make sure these aren't the values of any of the cards in the deck.
	public int maxZeroValue = 100;
	public int questionValue = 99;
	
	List<int> deck = new List<int>();
	public PlayerScript currentPlayer; //the player whose turn it is
	public bool isFirstTurnOfRound = false;
	public int currentBid = -16;
	public int defaultFirstBid = -16;
	public int middleCard;
	public bool allCardsVisible = false;
	public float challengeWaitTime = 4f; //in seconds
	public int cardForQuestion = 0; //This is a card dealt from the deck to be used if ? is in play.
	
	public GameObject playersContainer; //this is a GameObject that all the players are children of, including the human player
	private CreateComputerPlayersScript playerCreator;
	
    // Start is called before the first frame update
    void Start()
    {
		//This runs the script to create the number of computer players specified in the main menu. 
		playerCreator = playersContainer.GetComponent<CreateComputerPlayersScript>();
		playerCreator.createComputerPlayers(PlayerPrefs.GetInt("Computer player count"));
		
		update.loadInfoDisplays(); //loads in the info displays for each player, plus one for the middle card
		
		currentPlayer = playersList()[0];
		
        setUpRound();
		startTurn();
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public void setUpRound(){
		//Call this to set up the game before starting a new round,
		//either after a challenge or at the very beginning of the game.
		makeDeck();
		dealOutCards();
		resetPeeks();
		currentBid = defaultFirstBid;
		isFirstTurnOfRound = true;
	}
	
	public void makeDeck(){
		//This makes the list for the full deck.
		deck.Clear();
		
		deck.Add(-10);
		deck.Add(-5);
		
		for(int i = 0; i <= 10; i++){
			deck.Add(i);
		}
		
		deck.Add(15);
		deck.Add(20);
		deck.Add(maxZeroValue);
		deck.Add(questionValue);
		
		//Debug.Log(deck.Count);
		//if this works that debug should yield 17, and it does
	}
	
	public int drawRandomCard(){
		//This function draws a card from the deck and returns it as an integer.
		//It checks to make sure there are still cards in the deck, and returns 0 if not.
		int drawnCard = 0;
		if(deck.Count <= 0){
			return 0;
		} else {
			int randomPositionInDeck = Random.Range(0, deck.Count - 1);
			
			drawnCard = deck[randomPositionInDeck];
			deck.RemoveAt(randomPositionInDeck);
			
			//Debug.Log(deck.Count);
			//Debug.Log(drawnCard);
			return drawnCard;
		}
	}
	
	public void dealOutCards(){
		//Deals out random cards to the players.
		foreach(PlayerScript p in playersList()){
			if(p.isAlive()){
				p.cardHeld = drawRandomCard();
			} else {
				p.cardHeld = 0;
				//reset to 0 if the player isn't alive
				//this doesn't really need to happen since the functions that use cardHeld ignore dead players now
				//but I just wanted to do it
			}
		}
		
		middleCard = drawRandomCard();
		cardForQuestion = drawRandomCard();
	}
	
	public string cardTranslated(int i){
		//This script translates the integer value of a card into a string,
		//including putting ? or Max 0 if it's the value for that.
		
		if(i == maxZeroValue){
			return "Max 0";
		} else if (i == questionValue){
			return "?";
		} else {
			return i.ToString();
		}
	}
	
	public PlayerScript nextPlayer(PlayerScript p, bool forward){
		//Return the next living player after a specific player.
		//Set forward to false if you want the previous player instead.
		
		PlayerScript fooPlayer = p;
		int changeIDAmount = 0;
		int tempPlayerID = 0;
		if(numPlayersAlive() == 0){
			return fooPlayer;
		} else {
			if(forward){
				changeIDAmount = 1;
			} else {
				changeIDAmount = -1;
			}
			
			tempPlayerID = changeIDAmount + getPlayerID(fooPlayer);
			while(tempPlayerID < 0){
				tempPlayerID += playersList().Length;
			}
			tempPlayerID = tempPlayerID % playersList().Length;
			
			fooPlayer = getPlayerFromID(tempPlayerID);
			
			while(!fooPlayer.isAlive()){
				//Make sure it wraps around the list.
				tempPlayerID = changeIDAmount + getPlayerID(fooPlayer);
				while(tempPlayerID < 0){
					tempPlayerID += playersList().Length;
				}
				tempPlayerID = tempPlayerID % playersList().Length;
				
				fooPlayer = getPlayerFromID(tempPlayerID);
			}
			
			return fooPlayer;
		}
	}
	
	/*
	public PlayerScript ClockwiseFromPlayer(PlayerScript p, int i){
		int j = getPlayerID(p) + i;
		while(j < 0){
			j += playersList().Length;
		}
		j = j % playersList().Length;
		return PlayerScript getPlayerFromID(j);
	}
	*/
	
	public int getPlayerID(PlayerScript p){
		return p.transform.GetSiblingIndex();
	}
	
	public PlayerScript getPlayerFromID(int i){
		return playersContainer.transform.GetChild(i).GetComponent<PlayerScript>();
	}
	
	public int numPlayersAlive(){
		//Gets the number of players still alive.
		int holder = 0;
		foreach(PlayerScript p in playersList()){
			if(p.isAlive()){
				holder++;
			}
		}
		return holder;
	}
	
	public int cardsTotal(){
		//Totals the cards.
		//I need to clean up some of the duplicate code in this one.
		int runningTotal = 0;
		int currentCard = 0;
		int maxCard = -16;
		bool questionCardFound = false;
		bool maxZeroFound = false;
		
		//For each card that a living player has, evaluate it.
		foreach(PlayerScript p in playersList()){
			if(p.isAlive()){
				currentCard = p.cardHeld;
				if(currentCard == maxZeroValue){
					maxZeroFound = true;
				} else if (currentCard == questionValue){
					questionCardFound = true;
				} else {
					runningTotal += currentCard;
					if (currentCard > maxCard){
						maxCard = currentCard;
					}
				}
			}
		}
		
		//Do the same to the middle card.
		currentCard = middleCard;
		if(currentCard == maxZeroValue){
			maxZeroFound = true;
		} else if (currentCard == questionValue){
			questionCardFound = true;
		} else {
			runningTotal += currentCard;
			if (currentCard > maxCard){
				maxCard = currentCard;
			}
		}
		
		//If there was a question card, do it again with max 0.
		if(questionCardFound){
			currentCard = cardForQuestion;
			if(currentCard == maxZeroValue){
				maxZeroFound = true;
			} else {
				runningTotal += currentCard;
				if (currentCard > maxCard){
					maxCard = currentCard;
				}
			}
		}
		
		//If there was a max 0, subtract the maximum.
		if(maxZeroFound){
			runningTotal -= maxCard;
		}
		
		return runningTotal;
	}
	
	public void resolveChallenge(){
		
		allCardsVisible = true;
		
		StartCoroutine(FinishResolvingChallenge()); //a coroutine is used so it can pause to show all the cards
	}
	
	IEnumerator FinishResolvingChallenge(){
		//Reference the challenging player and the previous player.
		PlayerScript challengingPlayer = currentPlayer;
		PlayerScript biddingPlayer = nextPlayer(currentPlayer, false);
		
		gameLog.challenge(challengingPlayer, biddingPlayer, currentBid, cardsTotal()); //update the display manager with the results of the challenge
		
		yield return new WaitForSeconds(challengeWaitTime);
		if(currentBid > cardsTotal()){
			challengingPlayer.openEye();
			biddingPlayer.loseEye();
			if(biddingPlayer.isAlive()){
				currentPlayer = nextPlayer(currentPlayer, false); //set the player turn back one to go back to that player
			}
		} else {
			biddingPlayer.openEye();
			challengingPlayer.loseEye();
			if(!challengingPlayer.isAlive()){
				currentPlayer = nextPlayer(currentPlayer, true); //set the player turn ahead one if the challenging player is now dead, otherwise it stays with them
			}
		}
		
		//check for victory
		if(numPlayersAlive() == 1){
			//if only one player is left alive, run the victory script for that player
			foreach(PlayerScript p in playersList()){
				if(p.isAlive()){
					victory(p);
				}
			}
		} else {
			//otherwise next turn
			setUpRound();
			startTurn();
		}
		
		allCardsVisible = false;
	}
	
	public void raiseBid(int newBid){
		//Call this function to submit a bid.
		currentBid = newBid;
		isFirstTurnOfRound = false;
		gameLog.raiseBid(currentPlayer); //tell the display manager to update saying the bid was raised
		nextTurn();
	}
	
	public void startTurn(){
		//Starts a turn without changing the turn counter.
		//PlayerScript currentPlayer = playersList[playerTurn];
		//updatePlayerIsTurn();
		if(currentPlayer.isHumanPlayer){
			PlayerUI.initializeUI(); //runs a script to show the player UI
		} else {
			currentPlayer.RunAI();
		}
	}
	
	public void nextTurn(){
		//Starts a turn, first changing the turn marker so the next player goes.
		//playerTurn = nextPlayer(playerTurn, true);
		currentPlayer = nextPlayer(currentPlayer, true);
		startTurn();
	}
	
	public void resetPeeks(){
		//Call this before starting a new round to reset the canSeeMiddleCard for everybody.
		foreach(PlayerScript p in playersList()){
			p.canSeeMiddleCard = false;
		}
	}
	
	public void victory(PlayerScript p){
		//This script is run after a challenge if only one player is still alive.
		gameLog.victory(p);
	}
	
	public bool UIPlayerPeeked(){
		//This returns whether or not the middle card can be shown.
		bool temp = true;
		foreach(PlayerScript p in playersList()){
			if(p.isUIPlayer && !p.canSeeMiddleCard){
				temp = false;
			}
		}
		return temp;
	}
	
	public void sendPeekToGameLog(){
		gameLog.peek(currentPlayer);
	}
	
	public PlayerScript[] playersList(){
		//A list of all the players.
		return playersContainer.GetComponentsInChildren<PlayerScript>();
	}
}
