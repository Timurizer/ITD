using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * Class for representing plane and its behaviour
 * Used to select, disselect a plane and to set or destroy a tower on it.
 */
public class PlaneController : MonoBehaviour {

    public GameObject selector; //prefab for selector
    public GameObject towerPrefab; //prefab for tower

    public bool selected; // tells if plane is selected
    bool towerSet = true; // tells if plane has tower set on it

    /*public bool canSelect() {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject go in gos) {
            if (go.GetComponent<PlaneController>().isSelected()) {
                return false;
            }
        }
        return true;
    }*/

    public bool isSelected() {
        return selected;
    }

    // Behaviour on left and right mouse click.
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Plane");
            foreach (GameObject g in gos) {
                if (g.GetComponent<PlaneController>().isSelected()) {
                    disselect(g);
                }
            }
            selected = true;
            GameObject go = selector;
            (Instantiate(go, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.localPosition.z), transform.rotation)
                as GameObject).transform.parent = transform;
            /*if (canSelect()) {
                if (Input.GetMouseButtonDown(0)) {
                    selected = true;
                    GameObject go = selector;
                    (Instantiate(go, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.localPosition.z), transform.rotation)
                        as GameObject).transform.parent = transform;
                }*/
        }

        if (Input.GetMouseButtonDown(1)) {
            selected = false;
            int childNum = transform.childCount;
            //Debug.Log(childNum);
            for (int i = 0; i < childNum; i++) {
                if (transform.GetChild(i).tag == "Selector") {
                    GameObject go = transform.GetChild(i).gameObject;
                    Destroy(go);
                    break;
                }
            }

        }
    }

    public void disselect(GameObject go) {
        go.GetComponent<PlaneController>().selected = false;
        int childNum = go.transform.childCount;
        //Debug.Log(childNum);
        for (int i = 0; i < childNum; i++) {
            if (go.transform.GetChild(i).tag == "Selector") {
                GameObject selector = go.transform.GetChild(i).gameObject;
                Destroy(selector);
                break;
            }
        }

    }

    // sets the tower
    public void setTower(TowerPreset preset) {
        GameObject go = (Instantiate(towerPrefab, new Vector3(transform.position.x, transform.position.y, transform.localPosition.z), transform.rotation)
            as GameObject);
        go.transform.parent = gameObject.transform;
        go.GetComponent<TowerScript>().setStats(preset);
        towerSet = true;
    }

    public bool isTowerSet() {
        return towerSet;
    }

    //deletes the tower
    public TowerPreset deleteTower() {
        int childNum = transform.childCount;
        TowerPreset remember = null;
        for (int i = 0; i < childNum; i++) {
            if (transform.GetChild(i).tag == "Tower") {
                GameObject go = transform.GetChild(i).gameObject;
                remember = go.GetComponent<TowerScript>().getPreset();
                Destroy(go);
                break;
            }
        }
        towerSet = false;
        return remember;
    }
}
