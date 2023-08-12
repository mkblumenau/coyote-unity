using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script updates the text that tells the player how many eyes they have
//and what the total visible to them is.

public class UpdatePlayerTotalText : MonoBehaviour
{
	public GameManagerScript gm;
	public TextMeshProUGUI PlayerTotalText;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerTotalText();
    }
	
	public void updatePlayerTotalText(){
		string temp = "";
		string eyes = "";
		foreach(PlayerScript p in gm.playersList()){
			if(p.isUIPlayer){
				temp = p.totalKnownToPlayerWithSpecials();
				eyes = p.playerEyesWrittenOut();
			}
		}
		
		PlayerTotalText.text = "Visible total: " + temp + "\n" + eyes;
	}
}
