using UnityEngine;
using System.Collections;
/*
 * Class that represents spawn point and its behaviour
 */
public class SpawnerController : MonoBehaviour {

    public GameObject enemyPrefab; //prefab of an enemy to spawn
    WavePreset preset; //preset of a wave to spawn
    int spawnPoints; // counter to detect how many enemies has been spawned
    bool spawning; // tells if spawn in progress

    //method to spawn an enemy and wait
    IEnumerator spawnEnemy() {
        spawning = true;
        yield return new WaitForSeconds(0.5f);
        GameObject go = (Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.localPosition.z), transform.rotation)
            as GameObject);
        go.transform.parent = gameObject.transform;
        go.GetComponent<EnemyController>().setStats(preset.getSpeed(), preset.getArmor(), preset.getHp(), preset.getPointsPerCreep());
        spawnPoints--;
        spawning = false;
    }

    // method that starting coroutine
    public void spawn() {
        StartCoroutine(spawnEnemy());
    }

    //nethod that sets the wave to be spawned
    public void spawnWave(WavePreset _preset) {
        spawning = false;
        preset = _preset;
        spawnPoints = (int)preset.getCreepNumber(); ;
    }
  

    // Update is called once per frame. Spawns an enemy if wave is not empty and spawning not in progress
    void Update() {
        if (spawnPoints > 0 && !spawning) {
            spawn();
        }
    }
}
