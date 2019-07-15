using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

	public GameObject musicPlayerPrefab;
    public GameObject scoreHolderPrefab;
    private GameObject scoreHolder;

	private void Start() {
		CreatePersistentObjects();
        if ( SceneManager.GetActiveScene().name == "GameOver" ) {
            GameObject.FindWithTag( "ScoreHolder" ).GetComponent<ScoreHolderScript>().UpdateScoreText();
        }
    }

    public void StartGame() {
        SceneManager.LoadScene( "Level", LoadSceneMode.Single );
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ToMainMenu() {
     	SceneManager.LoadScene( "MainMenu", LoadSceneMode.Single );
    }

    private void CreatePersistentObjects() {
        if ( !GameObject.FindWithTag( "MusicPlayer" ) ) {
            GameObject newMusicPlayer = Instantiate( musicPlayerPrefab );
            newMusicPlayer.GetComponent<MusicPlayerScript>().StartMenuMusic();
        }
        if ( !GameObject.FindWithTag( "ScoreHolder" ) ) {
            Instantiate( scoreHolderPrefab );
        }
    }
}
