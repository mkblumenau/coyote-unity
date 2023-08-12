using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro

//This script operates the part of the main menu that controls the number of computer players.

public class NumPlayersMenuScript : MonoBehaviour
{
	public int numComputerPlayers = 2;
	public int maxComputerPlayers = 5;
	public int minComputerPlayers = 1;
	public TextMeshProUGUI counter;
	
    // Start is called before the first frame update
    void Start()
    {
        putInNewPlayerCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void addPlayer(){
		numComputerPlayers++;
		if(numComputerPlayers > maxComputerPlayers){
			numComputerPlayers = maxComputerPlayers;
		}
		putInNewPlayerCount();
	}
	
	public void removePlayer(){
		numComputerPlayers--;
		if(numComputerPlayers < minComputerPlayers){
			numComputerPlayers = minComputerPlayers;
		}
		putInNewPlayerCount();
	}
	
	public void putInNewPlayerCount(){
		counter.text = numComputerPlayers.ToString();
		PlayerPrefs.SetInt("Computer player count", numComputerPlayers);
	}
}
