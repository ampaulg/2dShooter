using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicPlayerScript : MonoBehaviour
{

	public AudioSource menuMusic;
	public AudioSource battleMusic;

	void Awake() {
		DontDestroyOnLoad( this.gameObject );
	}

    public void StartBattleMusic() {
    	menuMusic.Stop();
    	battleMusic.Play( 0 );
    }

    public void StartMenuMusic() {
    	battleMusic.Stop();
    	menuMusic.Play( 0 );
    }
}
