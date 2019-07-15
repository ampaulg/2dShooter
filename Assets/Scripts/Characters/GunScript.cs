using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {
    
    public bool friendly;
	public GameObject bulletPrefab;
	public float bulletRate;
	private bool allowFire = true;
	private float bulletTimer = 0f;

	private Transform shooterTransform;
	public Transform ShooterTransform {
        get { return shooterTransform; }
        set { shooterTransform = value; }
    }

    private void FixedUpdate() {
        if ( !allowFire ) {
            bulletTimer += Time.deltaTime;
            if ( bulletTimer > bulletRate ) {
                allowFire = true;
            }
        }
    }

    public void Shoot( Vector3 targetPos ) {
    	if ( !allowFire ) return;

    	Vector3 direction = targetPos - shooterTransform.position;
   		direction.Normalize();
   		if ( direction == direction * 0 ) {
   			direction.y = 1;
   		}

        GameObject newBullet = Instantiate( bulletPrefab,
            shooterTransform.position + ( direction * bulletPrefab.GetComponent<BulletScript>().startingDistance ),
            Quaternion.identity );
        newBullet.GetComponent<BulletScript>().Friendly = friendly;

   		newBullet.GetComponent<BulletScript>().SetDirection( direction );
   		allowFire = false;
   		bulletTimer = 0f;
    }
}