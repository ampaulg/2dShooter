using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHolderScript : MonoBehaviour {

    private int score = 0;
    public int Score {
        get { return score; }
    }
	private const string SCORE_BASE = "Score: ";
    
	void Awake() {
		DontDestroyOnLoad( this.gameObject );
	}

    public void ResetScore() {
    	score = 0;
    	UpdateScoreText();
    }

    public void AddScore( int points ) {
    	score += points;
    	UpdateScoreText();
    }

    public void UpdateScoreText() {
    	GameObject.FindWithTag( "ScoreText" ).GetComponent<Text>().text = SCORE_BASE + score.ToString();
    }
}
