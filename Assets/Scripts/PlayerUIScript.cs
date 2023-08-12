using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script manages the UI that the human player interacts with.

public class PlayerUIScript : MonoBehaviour
{
	public GameManagerScript gm;
	public int playerTentativeBid;
	public TextMeshProUGUI PlayerUIBidText;
	public Button peekButton;
	public Button challengeButton;
	public TextMeshProUGUI PlayerTotalText;
	
    // Start is called before the first frame update
    void Start()
    {
        //PlayerUIBidText = GameObject.Find("Current bid").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	//These functions allow the player to enter a new bid and send it in.
	//They work as far as I can tell.
	public void changeTentativeBid(int i){
		playerTentativeBid += i;
		if(playerTentativeBid <= gm.currentBid){
			playerTentativeBid = gm.currentBid + 1;
		}
		updateTentativeBidInUI();
	}
	
	public void setTentativeBid(int i){
		playerTentativeBid = i;
		updateTentativeBidInUI();
	}
	
	public void updateTentativeBidInUI(){
		//GameObject UIText = PlayerUI.transform.Find("Current bid");
		//TextMeshProUGUI u = UIText.GetComponent<TextMeshProUGUI>();
		
		PlayerUIBidText.text = playerTentativeBid.ToString();
	}
	
	public void lockInBid(){
		//Use this to submit the tentative bid.
		hideUI();
		gm.raiseBid(playerTentativeBid);
	}
	
	public void lockInChallenge(){
		//Use this from a button to send in the challenge.
		//This and the resolve challenge script seem to work.
		hideUI();
		gm.resolveChallenge();
	}
	
	public void playerPeek(){
		//Have the player whose turn it is peek.
		PlayerScript currentPlayer = gm.currentPlayer;
		currentPlayer.peek();
		challengeButton.enabled = false;
	}
	
	[ContextMenu("Initialize UI")]
	public void initializeUI(){
		//Shows the UI and sets all the unusable components to inactive.
		playerTentativeBid = gm.currentBid + 1;
		changeTentativeBid(0); //call this just to update the text bid in the UI
		gameObject.SetActive(true); //show the game object and all its children
		if(gm.isFirstTurnOfRound){
			challengeButton.enabled = false;
		} else {
			challengeButton.enabled = true;
		}
		checkPeekActivity();
	}
	
	[ContextMenu("Hide UI")]
	public void hideUI(){
		//Hides the entire UI.
		gameObject.SetActive(false);
	}
	
	public void checkPeekActivity(){
		//Used to hide the peek button if the player can't use it.
		PlayerScript currentPlayer = gm.currentPlayer;
		if(currentPlayer.numOpenEyes > 0 && !gm.isFirstTurnOfRound && !currentPlayer.canSeeMiddleCard){
			peekButton.enabled = true;
		} else {
			peekButton.enabled = false;
		}
	}
}
