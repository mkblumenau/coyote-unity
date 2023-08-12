using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script is attached to the prefab for each player info display.

public class PlayerInfoDisplayScript : MonoBehaviour
{
	public TextMeshProUGUI playerName;
	public TextMeshProUGUI eyesCount;
	public SpriteRenderer sr;
	public GameManagerScript gm;
	public PlayerScript assignedPlayer;
	public UpdateAllPlayerInfo cardsInfo;
	public SpriteRenderer turnIndicator;
	
    // Start is called before the first frame update
    void Start()
    {
		//Find everything that this script needs.
		//assignedPlayer isn't included: UpdateAllPlayerInfo will take care of that itself.
		//If assignedPlayer isn't set, the script works for the middle card.
        playerName = gameObject.transform.Find("Player name").GetComponent<TextMeshProUGUI>();
		eyesCount = gameObject.transform.Find("Eyes count").GetComponent<TextMeshProUGUI>();
		sr = gameObject.transform.Find("Card renderer").GetComponent<SpriteRenderer>();
		gm = GameObject.Find("Game manager").GetComponent<GameManagerScript>();
		cardsInfo = gameObject.transform.parent.GetComponent<UpdateAllPlayerInfo>();
		turnIndicator = gameObject.transform.Find("Turn indicator").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedPlayer == null){
			updatePlayerName("Middle card");
			clearEyesCount();
			turnIndicator.enabled = false;
			sr.enabled = true;
			
			//Update sprite based on whether or not the UI player peeked.
			if(gm.UIPlayerPeeked() || gm.allCardsVisible){
				sr.sprite = cardsInfo.getCardSpriteByName(cardsInfo.cardSpriteName(gm.middleCard));
			} else{
				sr.sprite = cardsInfo.cardBack;
			}
		} else {
			//if this is for a player
			updatePlayerName(assignedPlayer.name);
			updateEyesCount();
			turnIndicator.enabled = false; //the turn indicator is turned off by default
			
			if(assignedPlayer.isAlive()){
				sr.enabled = true;
				
				//Show the card only if this player isn't the UI player.
				if(assignedPlayer.isUIPlayer && !gm.allCardsVisible){
					sr.sprite = cardsInfo.cardBack;
				} else {
					sr.sprite = cardsInfo.getCardSpriteByName(cardsInfo.cardSpriteName(assignedPlayer.cardHeld));
				}
				
				if(assignedPlayer.isTurn()){ //show the turn indicator if it's their turn
					turnIndicator.enabled = true;
				}
			} else {
				sr.enabled = false;
			}
		}
    }
	
	public void updatePlayerName(string s){
		if (playerName != null){
			playerName.text = s;
		}
	}
	
	public void updateEyesCount(){
		if (eyesCount != null){
			eyesCount.text = assignedPlayer.playerEyesWrittenOut();
		}
	}
	
	public void clearEyesCount(){
		if (eyesCount != null){
			eyesCount.text = "";
		}
	}
	
	public void Destroy(){
		Destroy(gameObject);
	}
}
