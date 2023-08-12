using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script manages stats and actions for each player.
//It also has some string functions to return variables in text form.
//The actions for raising the bid and challenging are currently in GameManagerScript.

public class PlayerScript : MonoBehaviour
{	
	public bool isHumanPlayer; //set to true if this player is a human player
	public bool isUIPlayer; //set to true if the player is viewing the UI, so we can hide their card
	public int cardHeld;
	public int numberOfEyes = 3;
	public int numOpenEyes = 2;
	public float aiWaitTime = 3; //in seconds
	public bool canSeeMiddleCard = false;
	public GameManagerScript gm;
	
	//These variables are used for the AI script.
	//Don't bother with them for a human player.
	public int AILowBound = -15;
	public int AIHighBound = 35;
	
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Game manager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void loseEye(){
		//Call this to make the player discard an Eye.
		//This will handle having them discard a closed Eye if possible, and an open Eye otherwise.
		if(numberOfEyes > 0){
			numberOfEyes--;
		}
		if(numOpenEyes > numberOfEyes){
			numOpenEyes = numberOfEyes;
		}
	}
	
	public void openEye(){
		//Call this when the player needs to reopen a closed Eye.
		if(numOpenEyes < numberOfEyes){
			numOpenEyes++;
		}
	}
	
	public void peek(){
		//Call this to make this player peek.
		if(numOpenEyes > 0){
			numOpenEyes--;
		}
		canSeeMiddleCard = true;
		gm.sendPeekToGameLog();
	}
	
	public bool isAlive(){
		//Checks if this player is still in the game.
		if(numberOfEyes > 0){
			return true;
		} else {
			return false;
		}
	}
	
	public int totalKnownToPlayer(){
		//This returns the total, excluding ?, of the cards that the player can see.
		//It takes Max 0 into account when calculating this.
		int runningTotal = 0;
		int maxSeenSoFar = -11;
		bool isMaxZero = false;
		
		foreach(PlayerScript p in gm.playersList()){
			if(p != this){ //if it's not this player
				if(p.isAlive() && p.cardHeld != gm.questionValue){
					//as long as the player is alive and the card isn't ?
					if(p.cardHeld == gm.maxZeroValue){
						isMaxZero = true;
					} else {
						runningTotal += p.cardHeld;
						if(p.cardHeld > maxSeenSoFar){
							maxSeenSoFar = p.cardHeld;
						}
					}
				}
			}
		}
		
		if(canSeeMiddleCard){
			if(gm.middleCard == gm.maxZeroValue){
				isMaxZero = true;
			} else {
				runningTotal += gm.middleCard;
				if(gm.middleCard > maxSeenSoFar){
					maxSeenSoFar = gm.middleCard;
				}
			}
		}
		
		if(isMaxZero){
			runningTotal -= maxSeenSoFar;
		}
		
		return runningTotal;
	}
	
	public string totalKnownToPlayerWithSpecials(){
		//Returns a string of everything known to a player.
		//This can be used for UI.
		string temp = totalKnownToPlayer().ToString();
		bool isQuestion = false;
		bool isMaxZero = false;
		foreach(PlayerScript p in gm.playersList()){
			if(p.isAlive() && p != this){
				if(p.cardHeld == gm.questionValue){
					isQuestion = true;
				}
				if(p.cardHeld == gm.maxZeroValue){
					isMaxZero = true;
				}
			}
		}
		
		if(canSeeMiddleCard){
			if(gm.middleCard == gm.questionValue){
				isQuestion = true;
			}
			if(gm.middleCard == gm.maxZeroValue){
				isMaxZero = true;
			}
		}
		
		if(isQuestion){
			temp = string.Concat(temp, "?");
		}
		if(isMaxZero){
			temp = string.Concat(temp, ", Max 0");
		}
		
		return temp;
	}
	
	public string playerEyesWrittenOut(){
		//Returns the number of eyes that a player has in written form.
		string temp = "";
		temp = temp + numberOfEyes.ToString() + " eyes, " + numOpenEyes.ToString() + " open";
		if(numberOfEyes <= 0){
			temp = "Out";
		}
		return temp;
	}
	
	public void RunAI(){
		//Put in whatever the AI does on its turns here
		StartCoroutine(AITakeAction());
	}
	
	IEnumerator AITakeAction(){
		//Wait for 4 seconds
		yield return new WaitForSeconds(aiWaitTime);
		
		/*
		This was an old system that was a placeholder.
		
		if(gm.currentBid >= 20){
			gm.resolveChallenge();
		} else {
			gm.raiseBid(gm.currentBid + Random.Range(1, 3));
		}
		*/
		
		//How this works: Find the total known to this player.
		//Create a range around that total based on AILowBound and AIHighBound.
		//Use that range to determine how likely the AI is to challenge.
		//The AI will never challenge below totalKnownToPlayer() + AILowBound,
		//while it will always challenge above totalKnownToPlayer() + AIHighBound.
		//I haven't implemented peeking for the AI yet. It just doesn't know that's a possibility.
		
		float chanceToChallenge = convertToNewRange((float)gm.currentBid, (float)(totalKnownToPlayer() + AILowBound), (float)(totalKnownToPlayer() + AIHighBound), 0, 1);
		float randomPick = Mathf.Pow(Random.value, 2);
		//Debug.Log("Chance to challenge: " + chanceToChallenge.ToString() + ", Random pick: " + randomPick.ToString());
		if(randomPick < chanceToChallenge){
			gm.resolveChallenge();
		} else {
			gm.raiseBid(gm.currentBid + Random.Range(1, 3));
		}
	}
	
	public float convertToNewRange(float numberToConvert, float iRangeMin, float iRangeMax, float fRangeMin, float fRangeMax){
		//This converts a number along one range to a number along another range.
		float fractionOfWayThrough = (numberToConvert - iRangeMin) / (iRangeMax - iRangeMin);
		return fRangeMin + (fRangeMax * fractionOfWayThrough);
	}
	
	public bool isTurn(){
		//Returns whether or not it's this player's turn.
		if(gm.currentPlayer == this){
			return true;
		} else {
			return false;
		}
	}
}
