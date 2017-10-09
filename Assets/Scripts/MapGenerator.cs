using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * Class that generates maps from array map get from MapCreator
 */
public class MapGenerator : MonoBehaviour {

    public GameObject castle; // casstle prefab
    public GameObject plane; // plane prefab
    public GameObject spawnPoint; // spawn point prefab
    public GameObject[] path; // path prefab

    public GameObject pathSystem; // path systme prefab
    public GameObject pathNode; // path node prefab

    [Range(1, 3)]
    public int difficulty = 1;

    [Range(16, 64)]
    public int resolution = 32;

    Vector3 size;

    private GameObject[,] grid; // grid that represents map itself

    //float currentSize = plane.transform.

    void Awake() {
        grid = new GameObject[resolution, resolution];
        for (int x = 0; x < resolution; x++) {
            for (int z = 0; z < resolution; z++) {
                GameObject gridPlane = (GameObject)Instantiate(plane);
                gridPlane.transform.parent = this.transform;
                gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x,
                                                            gridPlane.transform.position.y,
                                                            gridPlane.transform.position.z + z);
                grid[x, z] = gridPlane;
            }
        }
    }

    // creates a map
    public void CreateMap() {
        clearArea();
        int length = 8;
        if (difficulty == 1) {
            length = resolution * 3;
        }
        if (difficulty == 2) {
            length = resolution * 2;
        }
        if (difficulty == 3) {
            length = resolution * 1;
        }

        //creating a creator
        MapCreator mapCreator = new MapCreator(resolution, length);
        int[,] map = mapCreator.takeMap();

        //filling grid
        for (int x = 0; x < resolution; x++) {
            for (int z = 0; z < resolution; z++) {

                if (map[x, z] == 1) {
                    GameObject gridPlane = (GameObject)Instantiate(path[Random.Range(0, path.Length)]);
                    gridPlane.transform.parent = this.transform;
                    //gridPlane.renderer.   
                    gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x,
                                                                gridPlane.transform.position.y + 0.5f,
                                                                gridPlane.transform.position.z + z);
                    grid[x, z] = gridPlane;
                } else if (map[x, z] == 2) {
                    GameObject gridPlane = (GameObject)Instantiate(castle);
                    gridPlane.transform.parent = this.transform;
                    //gridPlane.renderer.
                    gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x,
                                                                gridPlane.transform.position.y,
                                                                gridPlane.transform.position.z + z);
                }

                if (map[x, z] == 0) {
                    GameObject gridPlane = (GameObject)Instantiate(plane);
                    gridPlane.transform.parent = this.transform;
                    gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x,
                                                                gridPlane.transform.position.y + 0.5f,
                                                                gridPlane.transform.position.z + z);
                    grid[x, z] = gridPlane;
                }
                if (map[x, z] == 3) {
                    GameObject gridPlane = (GameObject)Instantiate(spawnPoint);
                    gridPlane.transform.parent = this.transform;
                    gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x,
                                                                gridPlane.transform.position.y + 0.5f,
                                                                gridPlane.transform.position.z + z);
                    grid[x, z] = gridPlane;
                }
            }
        }

        //making a path system
        List<int[]> pathNodesList = mapCreator.getPath();
        GameObject PS = Instantiate(pathSystem);
        foreach (int[] node in pathNodesList) {
            GameObject pNode = (GameObject)Instantiate(pathNode);
            pNode.transform.parent = PS.transform;
            pNode.transform.position = new Vector3(pNode.transform.position.x + node[0],
                                                                pNode.transform.position.y + 1f,
                                                                pNode.transform.position.z + node[1]);
        }
    }


    // deletes all objects on the map
    public void clearArea() {
        try {
            GameObject[] planes = GameObject.FindGameObjectsWithTag("Plane");
            foreach (GameObject go in planes) {
                GameObject.Destroy(go);
            }
        } catch {
            Debug.Log("No plane?");
        }

        try {
            GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            foreach (GameObject go in paths) {
                GameObject.Destroy(go);
            }
        } catch {
            Debug.Log("No paths?");
        }

        try {
            GameObject[] castle = GameObject.FindGameObjectsWithTag("Castle");
            foreach (GameObject go in castle) {
                GameObject.Destroy(go);
            }
        } catch {
            Debug.Log("No castles?");
        }
        try {
            GameObject[] spawn = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject go in spawn) {
                GameObject.Destroy(go);
            }
        } catch {
            Debug.Log("No spawns?");
        }
        try {
            GameObject[] spawn = GameObject.FindGameObjectsWithTag("PathSystem");
            foreach (GameObject go in spawn) {
                GameObject.Destroy(go);
            }
        } catch {
            Debug.Log("No Path system?");
        }
    }
}
