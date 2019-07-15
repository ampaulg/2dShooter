using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BouncingEnemyScript : EnemyScript {

	protected Vector3 movementDirection;

    protected override void Start() {
        base.Start();

        int initialAngle = Random.Range(0, 359);
        movementDirection.x = Mathf.Cos( initialAngle );
        movementDirection.y = Mathf.Sin( initialAngle );
    }

    protected override void Move() {
        rb.AddForce( movementDirection * movementSpeed );
    }

    protected void FaceMovementDirection () {
    	float movementAngle = - Vector2.SignedAngle( movementDirection, Vector2.up );
		transform.eulerAngles = new Vector3( 0, 0, movementAngle );
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        Vector3 velocity = rb.velocity;
        if ( other.gameObject.CompareTag( "WallTriggerU" ) ) {
            movementDirection.y = -Mathf.Abs( movementDirection.y );
            velocity.y = -Mathf.Abs( velocity.y );
        } else if ( other.gameObject.CompareTag( "WallTriggerD" ) ) {
            movementDirection.y = Mathf.Abs( movementDirection.y );
            velocity.y = Mathf.Abs( velocity.y );
        } else if ( other.gameObject.CompareTag( "WallTriggerL" ) ) {
            movementDirection.x = Mathf.Abs( movementDirection.x );
            velocity.x = Mathf.Abs( velocity.x );
        } else if ( other.gameObject.CompareTag( "WallTriggerR" ) ) {
            movementDirection.x = -Mathf.Abs( movementDirection.x );
            velocity.x = -Mathf.Abs( velocity.x );
        }
        rb.velocity = velocity;
    }
}
