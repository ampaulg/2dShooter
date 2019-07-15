using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {
    
    public GameObject musicPlayerPrefab;
    public GameObject scoreHolderPrefab;
    private GameObject player;
    private MazeBuilder mazeBuilder;

    private void Awake() {
        CreatePersistentObjects();

        GameObject.FindWithTag( "MusicPlayer" ).GetComponent<MusicPlayerScript>().StartBattleMusic();
        GameObject.FindWithTag( "ScoreHolder" ).GetComponent<ScoreHolderScript>().ResetScore();

        player = GameObject.FindWithTag( "Player" );
        mazeBuilder = GetComponent<MazeBuilder>();
    }

    private void Start() {
        mazeBuilder.MakeLevel();
    }

    private void Update() {
        if ( player.GetComponent<PlayerScript>().Health <= 0 ) {
            GameOver();
        } else if ( player.GetComponent<PlayerScript>().Teleported ) {
            mazeBuilder.LevelUp();
            mazeBuilder.MakeLevel();
        }

        if ( Input.GetKey( "escape" ) ) {
            Application.Quit();
        }
    }

    private void CreatePersistentObjects() {
        if ( !GameObject.FindWithTag( "MusicPlayer" ) ) {
            Instantiate( musicPlayerPrefab );
        }
        if ( !GameObject.FindWithTag( "ScoreHolder" ) ) {
            Instantiate( scoreHolderPrefab );
        }
    }

    private void GameOver() {
        GameObject.FindWithTag( "MusicPlayer" ).GetComponent<MusicPlayerScript>().StartMenuMusic();
        SceneManager.LoadScene( "GameOver" , LoadSceneMode.Single);
    }
}
