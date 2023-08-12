using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script creates the player info displays.
//It doesn't actually update them; it's called that because it used to before I reworked the code.

public class UpdateAllPlayerInfo : MonoBehaviour
{
	//public PlayerInfoDisplayScript test;
	public GameManagerScript gm;
	//public PlayerScript player;
	public Sprite[] cards;
	public Sprite cardBack;
	
	public GameObject infoDisplay;
	
	public PlayerInfoDisplayScript[] infoDisplaysList;
	
	//These are used in makeNewInfoDisplay to set up the layout of the displays.
	public float startingX = -350;
	public float startingY = 150;
	public float gapX = 150;
	public float gapY = 50;
	public float limitY = -50;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public Sprite getCardSpriteByName(string spriteName){
		//Use this to convert a sprite name to an actual sprite in the cards array.
		//If none is found, return the back of the card.
       for( int i = 0 ; i < cards.Length ; i++){
           if(cards[i] != null && string.Equals(cards[i].name, spriteName)){
			   return cards[i];
		   }
       }
       //Debug.LogWarningFormat( "No sprite representing mood '{0}' has been found", spriteName);
       return cardBack;
  }
  
	public string cardSpriteName(int i){
	  //Convert an integer to the name of a sprite.
	  //This isn't the same as cardTranslated in gm, since that returns ? instead of spelling it out.
	  //This difference is important since you can't name a file ?.
		string temp = "";
		if(i == gm.questionValue){
			temp = "Question mark";
		} else {
			temp = gm.cardTranslated(i);
		}
		
		return temp;
	}
	
	//These two functions were designed with the goal of making a new info display for each player.
	public void makeNewInfoDisplay(float xPosition, float yPosition){
		var newDisplay = Instantiate(infoDisplay, new Vector3(0, 0, 0), Quaternion.identity);
		//newDisplay.transform.parent = gameObject.transform;
		newDisplay.transform.SetParent(gameObject.transform);
		newDisplay.transform.localScale = new Vector3(1, 1, 1);
		newDisplay.transform.position = transform.TransformPoint(xPosition, yPosition, 0);
		//TransformPoint converts local display to world display, so they go to the right places.
	}
	
	public void assignDisplaysToPlayers(){
		int i = 0;
		infoDisplaysList = gameObject.GetComponentsInChildren<PlayerInfoDisplayScript>();
		foreach(PlayerInfoDisplayScript display in infoDisplaysList){
			if(i < gm.playersList().Length){
				display.assignedPlayer = gm.playersList()[i];
			}
			i++;
		}
	}
	
	public void loadInfoDisplays(){
		//Makes the right number of info displays at the start of the game.
		float currentX = startingX;
		float currentY = startingY;
		
		foreach(PlayerScript p in gm.playersList()){
			makeNewInfoDisplay(currentX, currentY);
			currentY -= gapY;
			if(currentY < limitY){
				currentX += gapX;
				currentY = startingY;
			}
		}
		makeNewInfoDisplay(currentX, currentY);
		
		assignDisplaysToPlayers();
	}
}
