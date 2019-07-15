using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : BouncingEnemyScript {

    private GunScript gun;

    protected override void Start() {
        base.Start();

        gun = GetComponent<GunScript>();
        gun.ShooterTransform = transform;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        FacePlayer();
        if ( !spawning ) {
            gun.Shoot( player.transform.position );
        }
    }
}
