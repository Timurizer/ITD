using UnityEngine;
using System.Collections;

/*
 * Class for representing and controlling an enemy
 */
public class EnemyController : MonoBehaviour {

    float speed;
    float armor;
    float hp;
    float points;

    GameObject pathGO; // GameObject that stores a path system
    int pathNodeIndex = 0; // counter for path nodes

    Transform targetPathNode; // Transform that represents the current destination of the enemy

    // method for setting stats of the enemy
    public void setStats(float _speed, float _armor, float _hp, float _points) {
        speed = _speed;
        armor = _armor;
        hp = _hp;
        points = _points;
    }

    //method that destroys enemy object and notifies the main controller that enemy has died
    public void die() {
        Destroy(gameObject);
        Destroy(this);

        GameObject.FindObjectOfType<ControllerScript>().notifyEnemyKilled();

    }
    
    // method that describes behaviour after taking damage
    public void takeDamage(float _damage) {

        hp -= (_damage - (_damage * armor / 100));
        Debug.Log("received: " + (_damage - (_damage * armor / 100)));
        if (hp <= 0) {
            GameObject.FindObjectOfType<ControllerScript>().enemyKilled(points);
            die();
        }
    }

    // Start is used for initialization. Enemy gets the PathSystem.
    void Start() {
        pathGO = GameObject.FindGameObjectWithTag("PathSystem");
        //Debug.Log(pathGO.ToString());
    }

    // method to search next path node
    void getNextPathNode() {

        try {
            targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
        } catch {
            targetPathNode = null;
        }
        pathNodeIndex++;
    }

    // basic Unity method that describes bahaviour of object every frame. Movement described here.
    void Update() {
        if (targetPathNode == null) {
            getNextPathNode();
            if (targetPathNode == null) {
                reachedGoal();
            }
        }

        try {
            Vector3 dir = targetPathNode.position - this.transform.position;

            float distThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame) {
                targetPathNode = null;
            } else {
                transform.Translate(dir.normalized * distThisFrame, Space.World);
                //this.transform.rotation = Quaternion.LookRotation(dir); 
            }
        } catch {

        }
    }

    // method that destroys the enemy after it reaches the end and notifies main controller about it
    void reachedGoal() {
        GameObject.FindObjectOfType<ControllerScript>().enemyReachedEnd();
        die();
    }

}
