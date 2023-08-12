using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed to display the text
using TMPro; //needed to work with the TextMeshPro
using UnityEngine.SceneManagement;

//This script handles showing and hiding menus,
//as well as changing scenes and pausing and unpausing the game.

public class MenuControlScript : MonoBehaviour
{
	public GameObject[] allMenus;
	public TextMeshProUGUI pausedText;
	public GameObject mainMenu;
	public TextFileOutputScript textOutput;
	public TextMeshProUGUI gameLogText;
	//public GameObject pausedTextGO;
	
    // Start is called before the first frame update
    void Start()
    {
        //pausedText = pausedTextGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
		//Use these to show and hide a game object.
	public void showMenu(GameObject menu){
		if(!anyMenusOpen()){
			menu.SetActive(true);
			pauseGame();
		}
		
		//Debug.Log(anyMenusOpen());
	}
	
	public void hideMenu(GameObject menu){
		menu.SetActive(false);
		unpauseGame();
	}
	
	public void showSubmenuOfMain(GameObject menu){
		mainMenu.SetActive(false);
		menu.SetActive(true);
	}
	
	public void hideSubmenuOfMain(GameObject menu){
		menu.SetActive(false);
		mainMenu.SetActive(true);
	}
	
	public void pauseButton(){
		//The pause button toggles whether or not the game is paused, but only if no menus are open.
		if(!anyMenusOpen()){
			if(Time.timeScale == 0){
				unpauseGame();
			} else {
				pauseGame();
			}
		}
	}
	
	public void pauseGame(){
		Time.timeScale = 0;
		//pausedText.SetActive(true);
		if (pausedText != null){
			//do this only if there's actually a component in pausedText
			pausedText.text = "Play";
		}
	}
	
	public void unpauseGame(){
		Time.timeScale = 1;
		//pausedText.SetActive(false);
		if (pausedText != null){
			pausedText.text = "Pause";
		}
	}
	
	public bool anyMenusOpen(){
		//This returns whether or not at least one menu is open.
		//Menus must be registered in allMenus for this to count them.
		
		bool holder = false;
		foreach(GameObject g in allMenus){
			if(g.activeSelf){
				holder = true;
			}
		}
		
		return holder;
	}
	
	public void loadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
	
	public void gameLogSaveAndMenu(GameObject menu){
		//file save function gets called here
		textOutput.createTextDated(gameLogText.text);
		showSubmenuOfMain(menu);
	}
	
	public void Quit(){
		Application.Quit();
		//UnityEditor.EditorApplication.isPlaying = false;
	}
}
