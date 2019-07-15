using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float startingDistance;
	public float speed;
	private Vector3 velocity;
	private bool friendly;
    public bool Friendly {
        get { return friendly; }
        set { friendly = value; }
    }

    void FixedUpdate() {
        transform.position += velocity;
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if ( ( friendly && ( other.gameObject.CompareTag( "Enemy" ) ) )
            || ( !friendly && ( other.gameObject.CompareTag( "Player" ) ) ) ) {
            other.gameObject.GetComponent<CharacterScript>().TakeDamage( 1 );
            Destroy( this.gameObject );
        }

        if ( other.gameObject.CompareTag( "Wall" ) ) {
        	Destroy( this.gameObject );
        }
    }

    public void SetDirection( Vector3 direction ) {
    	direction.Normalize();
    	velocity = direction * speed;

    	Vector3 up = new Vector3( 0, 1, 0 );
    	float dot = Vector3.Dot( up, direction );
    	float angle = Mathf.Acos( dot/( up.magnitude * direction.magnitude ) ) * Mathf.Rad2Deg;

    	if ( direction.x > 0 ) {
    		angle *= -1;
    	}

    	transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
