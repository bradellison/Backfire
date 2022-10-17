using UnityEngine;

public class SaveData : MonoBehaviour
{

    public int[] highScores = new int[5];

    private int _intToSave;
    private float _floatToSave;
    private string _stringToSave;

    private void SaveGame()
    {
    	PlayerPrefs.SetInt("SavedInteger", _intToSave);
    	PlayerPrefs.SetFloat("SavedFloat", _floatToSave);
    	PlayerPrefs.SetString("SavedString", _stringToSave);
    	PlayerPrefs.Save();
    	Debug.Log("Game data saved!");
    }

}
