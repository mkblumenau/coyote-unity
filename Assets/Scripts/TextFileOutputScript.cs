using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //needed for exporting text files

//This script is used for exporting the game logs.

public class TextFileOutputScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void createText(string path, string input){
		//Create file if it doesn't exist at the given path
		if(!File.Exists(path)){
			FileInfo file = new FileInfo(path);
			file.Directory.Create();
			File.WriteAllText(path, "");
		}
		
		//Add text to it from the given input
		File.AppendAllText(path, input);
	}
	
	public string dateAndTime(){
		/*
		string temp = "";
		
		System.DateTime myTime =  System.DateTime.Now;
        //Debug.Log(myTime.TimeOfDay);
		double year = myTime.Year;
		double month = myTime.Month;
		double day = myTime.Day;
		double hour = myTime.Hour;
		double minute = myTime.Minute;
		double second = myTime.Second;
		
		temp = temp + year.ToString() + "-" + month.ToString() + "-" + day.ToString();
		temp = temp + "_" + hour.ToString() + "_" + minute.ToString() + "_" + second.ToString();
		return temp;
		*/
		return System.DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
	}
	
	public void createTextDated(string input){
		createText(Application.dataPath + "/GameLogs/Game_Log_" + dateAndTime() + ".txt", input);
	}
}
