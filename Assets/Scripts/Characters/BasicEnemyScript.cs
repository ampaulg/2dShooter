using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : EnemyScript {
	
    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        FacePlayer();
    }

    protected override void Move() {
        Vector3 movement = player.transform.position - transform.position;
        movement.Normalize();
        rb.AddForce( movement * movementSpeed );
    }
}
