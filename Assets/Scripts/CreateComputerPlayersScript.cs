using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the object that contains the Players.
//It creates the computer players.

public class CreateComputerPlayersScript : MonoBehaviour
{
	public GameObject playerTemplate;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void createComputerPlayers(int num){
		//Create a bunch of players under this.
		//This script also renames them.
		//This formula can be used to change other properties of the new variable.
		
		for(int i = 1; i <= num; i++){
			var newPlayer = Instantiate(playerTemplate);
			newPlayer.transform.parent = gameObject.transform;
			newPlayer.name = "Comp" + i.ToString();
		}
	}
}
