using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : CharacterScript {

    public bool alwaysFaceCursor;
	public float rotationSpeed;

    private GunScript gun;

    private bool teleporting = false;
    private bool teleported;

    public bool Teleported {
        get { return teleported; }
    }

    protected override void Start() {
        base.Start();

        gun = GetComponent<GunScript>();
        gun.ShooterTransform = transform;
        spawning = true;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if ( spawning ) {
            HandleSpawning();
        } else if ( teleporting ) {
            HandleTeleport();
        }
        HandleMovement();
        HandleShooting();
    }

    private void HandleTeleport() {
        spawnTimer--;
        float newVal = ( float ) spawnTimer / ( float ) spawnFrames;
        sr.color = new Color( newVal, sr.color.g, sr.color.b, newVal );
        if ( spawnTimer == 0 ) {
            teleporting = false;
            teleported = true;
            StopMovement();
        }
    }

    private void HandleShooting() {
        if ( Input.GetButton("Fire1") ) {
            Vector3 clickScreenPos = Input.mousePosition;
            Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint( clickScreenPos );
            clickWorldPos.z = 0.0f;
            gun.Shoot( clickWorldPos );
        }
    }

    private void HandleMovement() {
        Vector3 movement = new Vector3( Input.GetAxisRaw( "Horizontal" ),
                                        Input.GetAxisRaw( "Vertical" ), 0 );
        movement.Normalize();
        rb.AddForce( movement * movementSpeed );
        rb.angularVelocity = 0; 

        if ( ( Input.GetButton( "Fire1" ) ) || alwaysFaceCursor ){
        	FaceMouseDirection();
        } else if ( movement.magnitude > 0 ) {
        	FaceMovementDirection( new Vector2( movement.x, movement.y ) );
        }
    }

    private void FaceMovementDirection( Vector2 movementVector ) {
    	float currentAngle = transform.rotation.eulerAngles.z;
    	currentAngle *= Mathf.Deg2Rad;
    	Vector3 shipDirectionVec = new Vector2( -Mathf.Sin( currentAngle ),
    										 	Mathf.Cos( currentAngle ) );
        
        if ( Vector2.Angle( movementVector, shipDirectionVec ) < rotationSpeed ) {
            float movementAngle = - Vector2.SignedAngle( movementVector, Vector2.up );
            transform.eulerAngles = new Vector3( 0, 0, movementAngle );
        } else {
        	float turnDirection = ( shipDirectionVec.x * movementVector.y )
                                  - ( shipDirectionVec.y * movementVector.x );
        	if ( turnDirection > 0 ) {
        		transform.Rotate( 0, 0, rotationSpeed );
        	} else {
        		transform.Rotate( 0, 0, -rotationSpeed );
        	}
        }
    }

    private void FaceMouseDirection() {
    	Vector3 clickScreenPos = Input.mousePosition;
   		Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint( clickScreenPos );
   		clickWorldPos.z = 0.0f;
		Vector3 direction = clickWorldPos - transform.position;
   		if ( direction == direction * 0 ) {
   			direction.y = 1;
   		}

   		Vector3 up = new Vector3( 0, 1, 0 );
    	float dot = Vector3.Dot( up, direction );
    	float angle = Mathf.Acos( dot / ( up.magnitude * direction.magnitude ) ) * Mathf.Rad2Deg;

    	if ( direction.x > 0 ) {
    		angle *= -1;
    	}

        transform.eulerAngles = new Vector3( 0, 0, angle );
    }

    public override void TakeDamage( int damage ) {
        base.TakeDamage( damage );

        hittable = false;
        UpdateHealthBar();
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if ( other.gameObject.CompareTag( "Room" ) ) {
            other.gameObject.GetComponent<RoomScript>().ActivateRoom();
        }
    }

    void OnTriggerStay2D( Collider2D other ) {
        if ( other.gameObject.CompareTag( "Portal" ) && ( !teleporting ) && ( !teleported ) ) {
            if ( other.GetComponent<DropScript>().Active ) {
                teleporting = true;
                hittable = false;
            }
        } else if ( other.gameObject.CompareTag( "HealthDrop" ) ) {
            if ( other.GetComponent<DropScript>().Active ) {
                health = System.Math.Min( maxHealth,
                                          health + other.GetComponent<PickupScript>().value );
                UpdateHealthBar();
                other.GetComponent<DropScript>().Active = false;
                Destroy( other.gameObject );
            }
        } else if ( other.gameObject.CompareTag( "TreasureDrop" ) ) {
            if ( other.GetComponent<DropScript>().Active ) {
                GameObject.FindWithTag( "ScoreHolder" ).GetComponent<ScoreHolderScript>().AddScore(
                    other.GetComponent<PickupScript>().value );
                other.GetComponent<DropScript>().Active = false;
                Destroy( other.gameObject );
            }
        }
    }

    private void UpdateHealthBar() {
        GameObject.FindWithTag( "HealthBar" ).GetComponent<HealthBarScript>().UpdateBar( health, maxHealth );
    }

    public void StartSpawning() {
        teleported = false;
        spawning = true;
        GameObject.FindWithTag( "MainCamera" ).GetComponent<CameraScript>().SnapToPlayer();
    }

    private void StopMovement() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }
}
