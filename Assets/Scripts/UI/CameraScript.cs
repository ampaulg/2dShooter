using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float maxRadius;
	private GameObject player;

    private void Awake() {
    	player = GameObject.FindWithTag( "Player" );
    }

    private void Update() {
    	float newX = transform.position.x;
    	float newY = transform.position.y;
		
		Vector2 diff = new Vector2( 
			player.transform.position.x - transform.position.x,
			player.transform.position.y - transform.position.y );


		if ( diff.magnitude > maxRadius ) {
			diff.Normalize();
			diff *= maxRadius;
			newX = player.transform.position.x - diff.x;
			newY = player.transform.position.y - diff.y;
		}

    	// if difference in Y is greater than the max radius, move camera
    	Vector3 newPosition = new Vector3( newX, newY, transform.position.z );
        transform.position  = newPosition;
    }

    public void SnapToPlayer() {
    	transform.position  = new Vector3( player.transform.position.x,
    									   player.transform.position.y,
    									   transform.position.z );
    }
}