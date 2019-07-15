using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour {

	public int appearFrames;
    private int appearTimer = 0;

    private bool active = false;
    public bool Active {
        get { return active; }
        set { active = value; }
    }
    private SpriteRenderer sr;

    void Start() {
    	sr = GetComponent<SpriteRenderer>();
        sr.color = new Color( sr.color.r, sr.color.g, sr.color.b, 0 );
    }

    protected virtual void FixedUpdate() {
        if ( !active ) {
            Appear();
        }
    }

    private void Appear() {
        appearTimer++;
        float newVal = ( float ) appearTimer / ( float ) appearFrames;
        sr.color = new Color( sr.color.r, sr.color.g, sr.color.b, newVal );
        if ( appearTimer == appearFrames ) {
            active = true;
        }
    }
}
