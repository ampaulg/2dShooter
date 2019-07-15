using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtboxScript : MonoBehaviour {

	private PlayerScript playerScript;

    void Awake() {
    	playerScript = GameObject.FindWithTag( "Player" ).GetComponent<PlayerScript>();     
    }

    void OnTriggerStay2D( Collider2D other ) {
        if ( ( other.gameObject.CompareTag( "Enemy" ) )
        	|| ( other.gameObject.CompareTag( "Boss" ) ) ) {
        	playerScript.TakeDamage( 1 );
        }
    }
}
