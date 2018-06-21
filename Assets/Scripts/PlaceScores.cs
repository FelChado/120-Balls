using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceScores : MonoBehaviour 
{
    [SerializeField]
    Text scoreText, bestText;

	void Start ()
    {
        this.bestText.text = PlayerPrefs.GetInt("Best").ToString();
        this.scoreText.text = PlayerInfo.score.ToString();
	}
	
}
