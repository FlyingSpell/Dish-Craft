using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
	#pragma warning disable 0649

	[SerializeField]
	private Statistics statistics;

	#pragma warning restore 0649

	private Text 	   scoreText;

    void Awake()
    {
        scoreText = GetComponent<Text>();

        scoreText.text = statistics.scoreString;
    }

    void OnEnable()
    {
		statistics.ScoreStringChanged += OnScoreStringChange;
    }

    void OnDisable()
    {
    	statistics.ScoreStringChanged -= OnScoreStringChange;
    }

    void OnScoreStringChange(String scoreString)
    {
    	scoreText.text = scoreString;
    }
}
