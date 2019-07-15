using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyScript : CharacterScript {
    
    public int powerLevel;
    public int scoreValue;

	protected GameObject player;
    private RoomScript owner;
    public RoomScript Owner {
        get { return owner; }
        set { owner = value; }
    }

    protected override void Start() {
        base.Start();

        player = GameObject.FindWithTag( "Player" );
        sr.color = new Color( 0, sr.color.g, sr.color.b, 0 );
    }

    protected override void FixedUpdate() {
    	base.FixedUpdate();

        if ( spawning ) {
            HandleSpawning();
        } else {
            Move();
        }
    }

    protected void FacePlayer() {
    	Vector3 toPlayer = player.transform.position - transform.position;
        float dot = Vector3.Dot( Vector3.up, toPlayer );
        float angle = Mathf.Acos( dot/ toPlayer.magnitude ) * Mathf.Rad2Deg;

        if ( toPlayer.x > 0 ) {
            angle *= -1;
        }
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public override void TakeDamage( int damage ) {
        base.TakeDamage( damage );
        if ( health <= 0 ) {
            GameObject.FindWithTag( "ScoreHolder" ).GetComponent<ScoreHolderScript>().AddScore( scoreValue );
            owner.DecrementEnemyCount();
            Destroy( this.gameObject );
        }
    }
}
