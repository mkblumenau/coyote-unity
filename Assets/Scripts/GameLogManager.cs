using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script manages the game log.
//It replaces some of the functions that DisplayUIManagerScript used to do.

public class GameLogManager : MonoBehaviour
{
	public TextMeshProUGUI gameLog;
	public GameManagerScript gameManager;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void raiseBid(PlayerScript p){
		//Call this when a player raises a bid.
		string stringToOutput = p.name + " raised the bid to " + gameManager.currentBid.ToString() + ".";
		addToGameLog(stringToOutput + "\n");
	}
	
	public void challenge(PlayerScript challenger, PlayerScript defender, int bid, int total){
		//Call this to update the display with the results of a challenge.
		string stringToOutput = challenger.name + " challenged " + defender.name + "'s bid of " + bid.ToString() + ".\n";
		stringToOutput = string.Concat(stringToOutput, "The total was " + total.ToString() + ".\n");
		if(bid > total){
			stringToOutput = string.Concat(stringToOutput, challenger.name + " wins the challenge.\n");
			if(defender.numberOfEyes <= 0){
				stringToOutput = string.Concat(stringToOutput, defender.name + " was eliminated.\n");
			}
		} else {
			stringToOutput = string.Concat(stringToOutput, defender.name + " wins the challenge.\n");
			if(challenger.numberOfEyes <= 0){
				stringToOutput = string.Concat(stringToOutput, challenger.name + " was eliminated.\n");
			}
		}
		
		bool questionCardFound = false;
		stringToOutput = string.Concat(stringToOutput, "Cards in play this round: ");
		foreach(PlayerScript p in gameManager.playersList()){
			if(p.isAlive()){
				stringToOutput = string.Concat(stringToOutput, gameManager.cardTranslated(p.cardHeld) + ", ");
				if(p.cardHeld == gameManager.questionValue){
					questionCardFound = true;
				}
			}
		}
		
		if(gameManager.middleCard == gameManager.questionValue){
			questionCardFound = true;
		}
		
		if(questionCardFound){
			stringToOutput = stringToOutput + gameManager.cardTranslated(gameManager.middleCard) + ", " + gameManager.cardTranslated(gameManager.cardForQuestion) + "\n";
		} else {
			stringToOutput = string.Concat(stringToOutput, gameManager.cardTranslated(gameManager.middleCard) + "\n");
		}

		//gameLog.text = string.Concat(stringToOutput + "\n", gameLog.text);
		//gameLog.text = string.Concat("-----\n", gameLog.text);
		
		addToGameLog(stringToOutput + "\n");
		addToGameLog("-----\n");
	}
	
	public void victory(PlayerScript player){ //Call this when a player wins.
		//gameLog.text = string.Concat(player.name + " has won!\n\n", gameLog.text);
		addToGameLog(player.name + " has won!\n\n");
	}
	
	public void peek(PlayerScript player){
		addToGameLog(player.name + " peeked.\n");
	}
	
	public void addToGameLog(string s){
		gameLog.text = string.Concat(s, gameLog.text);
	}
}
