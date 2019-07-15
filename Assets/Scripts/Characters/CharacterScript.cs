using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterScript : MonoBehaviour {

	public int maxHealth;
    public int MaxHealth {
        get { return maxHealth; }
    }
	protected int health;
    public int Health {
        get { return health; }
    }

	public float damageRate;
	private float damageTimer = 0f;
	protected bool hittable = false;
	protected bool showingDamage = false;

	public float movementSpeed;

	protected SpriteRenderer sr;
	protected Rigidbody2D rb;

    protected bool spawning = true;
    public int spawnFrames;
    protected int spawnTimer = 0;

    protected Color healedColour = Color.white;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    protected virtual void FixedUpdate() {
        if ( showingDamage ) {
    		damageTimer += Time.deltaTime;
    		if ( damageTimer > damageRate ) {
    			hittable = true;
    			showingDamage = false;
    			sr.color = healedColour;
    		}
    	}
    }

    public virtual void TakeDamage( int damage ) {
    	if ( hittable ) {
    		health -= damage;
    		showingDamage = true;
    		damageTimer = 0f;
    		sr.color = Color.red;
    	}
    }

    protected void HandleSpawning() {
        spawnTimer++;
        float newVal = (float) spawnTimer / (float) spawnFrames;
        sr.color = getSpawnColor( newVal );
        if ( spawnTimer == spawnFrames ) {
            spawning = false;
            hittable = true;
        }
    }

    protected virtual Color getSpawnColor( float newVal ) {
        return new Color( newVal, sr.color.g, sr.color.b, newVal );
    }

    protected virtual void Move() {}
}
