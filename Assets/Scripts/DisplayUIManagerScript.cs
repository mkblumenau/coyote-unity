using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script is deprecated.
//It was used to run the old text-based UI as well as to update the game log.
//The text-based UI has been removed from the game in favor of the new UI.
//The game log functions have been removed from this and moved to GameLogManager.

public class DisplayUIManagerScript : MonoBehaviour
{
	public TextMeshProUGUI cardsDisplay;
	public TextMeshProUGUI playersInfoDisplay;
	public GameManagerScript gameManager;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateDisplayText();
		updatePlayersInfoDisplay();
    }
	
	public void updateDisplayText(){
		//This updates the text display.
		
		string displayOutput = "";
		
		//For each player, add their name and the card.
		foreach(PlayerScript p in gameManager.playersList()){
			if(p.isAlive()){
				displayOutput = string.Concat(displayOutput, p.name);
				displayOutput = string.Concat(displayOutput, "'s card: ");
				if(p.isUIPlayer){
					displayOutput = string.Concat(displayOutput, "hidden");
				} else{
					displayOutput = string.Concat(displayOutput, gameManager.cardTranslated(p.cardHeld));
				}
				displayOutput = string.Concat(displayOutput, "\n");
			}
		}
		
		//If the player marked as the UI player can see the middle card, set it to be hidden.
		bool canSeeMiddleCard = true;
		foreach(PlayerScript p in gameManager.playersList()){
			if(p.isUIPlayer && !p.canSeeMiddleCard){
				canSeeMiddleCard = false;
			}
		}
		
		//Then add the middle card.
		displayOutput = string.Concat(displayOutput, "Middle card: ");
		if(canSeeMiddleCard){
				displayOutput = string.Concat(displayOutput, gameManager.cardTranslated(gameManager.middleCard));
			} else{
				displayOutput = string.Concat(displayOutput, "hidden");
			}
		displayOutput = string.Concat(displayOutput, "\n");
		
		cardsDisplay.text = displayOutput; //set the text display to the text created here
	}
	
	public void updatePlayersInfoDisplay(){
		string displayOutput = "";
		
		foreach(PlayerScript p in gameManager.playersList()){
			if(p.isAlive()){
				displayOutput = string.Concat(displayOutput, p.name);
				displayOutput = string.Concat(displayOutput, ": ");
				displayOutput = string.Concat(displayOutput, p.numberOfEyes);
				displayOutput = string.Concat(displayOutput, " eyes, ");
				displayOutput = string.Concat(displayOutput, p.numOpenEyes);
				displayOutput = string.Concat(displayOutput, " face-up\n");
			}
		}
		
		playersInfoDisplay.text = displayOutput;
	}
}