using System;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	private Statistics statistics;

	#pragma warning restore 0649

	private String 	   autosaveName = "Autosave";

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
        	Load(autosaveName);
        }
    }

    void OnEnable()
    {
		statistics.ScoreChanged += OnScoreChange;
    }

    void OnDisable()
    {
    	statistics.ScoreChanged -= OnScoreChange;
    }

    private void OnScoreChange(int totalScore)
    {
		Autosave();
    }

    private void Autosave()
    {
    	Save(autosaveName);
    }

    private void Save(String saveName)
    {
    	statistics.SaveTo(out String jsonData);

		PlayerPrefs.SetString(saveName, jsonData);
    }

    private void Load(String saveName)
    {
    	bool saveIsMissing = !PlayerPrefs.HasKey(saveName);

    	if(saveIsMissing)
    	{
    		Debug.Log($"Save '{saveName}' is missing!");

    		return;
    	}

    	String jsonData = PlayerPrefs.GetString(saveName);

		statistics.LoadFrom(jsonData);
    }
}
