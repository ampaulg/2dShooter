using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyHeadScript : BouncingEnemyScript {

	public int tailSegmentCount;
	public int tailSegmentDelay;
	public GameObject tailPrefab;

	private int tailStartTimer = 0;
	private bool generatingTail = true;
	private int tailsStarted = 0;
	private List<GameObject> tailSegments;

	private Vector3 initialMovementDirection;

    protected override void Start() {
    	base.Start();

        initialMovementDirection = movementDirection;
        tailSegments = new List<GameObject>();
        for ( int i = 0; i < tailSegmentCount; i++ ) {
        	GameObject newTailSegment = Instantiate( tailPrefab, transform.position, Quaternion.identity );
            tailSegments.Add( newTailSegment );
        }

        healedColour = Color.magenta;
    }

    protected override void FixedUpdate() {
    	base.FixedUpdate();
        
    	FaceMovementDirection();
    	if ( ( !spawning ) && generatingTail ) {
    		StartTails();
    	}
    }

    private void StartTails() {
    	if ( tailStartTimer == tailSegmentDelay ) {
    		tailSegments[ tailsStarted ].GetComponent<SnakeEnemyTailScript>().MovementDirection = initialMovementDirection;
    		tailsStarted++;
    		tailStartTimer = 0;
    		if ( tailsStarted == tailSegmentCount ) {
    			generatingTail = false;
    		}
    	}
    	tailStartTimer++;
    }

    public override void TakeDamage( int damage ) {
        if ( health - damage <= 0 ) {
            foreach ( GameObject segment in tailSegments ) {
            	Destroy( segment );
            }
        }
        base.TakeDamage( damage );
    }
}
