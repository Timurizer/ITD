using UnityEngine;
using System.Collections;

/*
 * Class for representing bullet and its behaviour
 */

public class Bullet : MonoBehaviour {

    public float speed = 75f; // speed of a bullet
    public Transform target; // position of target
    public EnemyController enemy; // enemies characteristics

    float damage;

    //sets the damage bullet can deal
    public void setDamage(float _damage) {
        damage = _damage;
    }

	// Update is called once per frame. Moves to the target.
	void Update () {
        try {
            Vector3 dir = target.position - this.transform.position;

            float distThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame) {
                doBulletHit();
            } else {
                transform.Translate(dir.normalized * distThisFrame, Space.World);
                //this.transform.rotation = Quaternion.LookRotation(dir); 
            }
        } catch {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    
    // Method that describes what happened on bullet hit     
    public void doBulletHit() {
        enemy.takeDamage(damage);
        Destroy(gameObject);
        Destroy(this);
    }
}
