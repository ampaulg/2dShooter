using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyTailScript : BouncingEnemyScript {

	public Vector3 MovementDirection {
		get { return movementDirection; }
		set { movementDirection = value; }
	}

    protected override void Start() {
        base.Start();
        movementDirection = new Vector3( 0, 0, 0 );
    }

    protected override void FixedUpdate() {
    	base.FixedUpdate();
    	FaceMovementDirection();
    }

    protected override Color getSpawnColor( float newVal ) {
    	return new Color( 0, 0, 0, newVal );
    }

    public override void TakeDamage( int damage ) {
    	// The snake's tail is invincible
        return;
    }
}
