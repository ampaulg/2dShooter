using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : DropScript {
    
	public float rotationSpeed;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        transform.Rotate( 0, 0, rotationSpeed );
    }
}