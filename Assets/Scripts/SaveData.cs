using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{

    public int[] highScores = new int[5];

	int intToSave;
	float floatToSave;
	string stringToSave;

    void SaveGame()
    {
    	PlayerPrefs.SetInt("SavedInteger", intToSave);
    	PlayerPrefs.SetFloat("SavedFloat", floatToSave);
    	PlayerPrefs.SetString("SavedString", stringToSave);
    	PlayerPrefs.Save();
    	Debug.Log("Game data saved!");
    }

}
