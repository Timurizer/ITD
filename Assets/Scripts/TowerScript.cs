using UnityEngine;
using System.Collections;
/*
 * Class for representing tower and it's behaviour
 */
public class TowerScript : MonoBehaviour {

    TowerPreset preset; // 3D model preset of a tower

    Transform turretTransform; // position of turret of the tower
    public GameObject BulletPrefab; // bullet model prefab

    float fireCooldownLeft; //fire cooldown counter

    //sets stats of a tower (basicly just a preset)
    public void setStats(TowerPreset _preset) {
        preset = _preset;
    }

    public TowerPreset getPreset() {
        return preset;
    }

    // Use this for initialization
    void Start() {
        turretTransform = transform.Find("Turret");
    }

    /*
     * Once per frame
     * -find nearest enemy
     * -check cooldown
     * -shoot
     */
    void Update() {
        EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();

        EnemyController nearestEnemy = null;
        float dist = Mathf.Infinity;

        foreach (EnemyController e in enemies) {
            float d = Vector3.Distance(this.transform.position, e.transform.position);
            if (nearestEnemy == null || d < dist) {
                nearestEnemy = e;
                dist = d;
            }
        }

        if (nearestEnemy == null) {
            //Debug.Log("No enemies?");
        }
        try {
            Vector3 dir = nearestEnemy.transform.position - this.transform.position;

            Quaternion lookRot = Quaternion.LookRotation(dir);

            turretTransform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);


            if (nearestEnemy != null) {
                fireCooldownLeft -= Time.deltaTime;
                if (fireCooldownLeft <= 0 && dir.magnitude <= preset.range) {
                    fireCooldownLeft = preset.shootingSpeed;
                    shootAt(nearestEnemy);
                }
            }
        } catch {

        }
    }

    // describes what must be done on shooting
    void shootAt(EnemyController e) {
        GameObject bulletGo = (GameObject)Instantiate(BulletPrefab, this.transform.position, this.transform.rotation);
        Bullet b = bulletGo.GetComponent<Bullet>();
        b.setDamage(preset.damage);
        Debug.Log("Damage: " + preset.damage);
        b.target = e.transform;
        b.enemy = e;
    }

}
