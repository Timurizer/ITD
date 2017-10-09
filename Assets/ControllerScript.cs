using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour {

    public GameObject map; //MapGenerator prefab

    List<TowerPreset> towerPresets = new List<TowerPreset>(); // current presets for towers
    WavePreset wavePreset; // preset for current wave
    WavePreset finalWave; // the wave that we get after wave simuation and all genetic stuff
    int enemyPopulation; // current wave population

    float score = 0; // score of a player
    int selectedPreset; // selected prefab of a tower
    int waveNumber; // number of a current wave
    float points; // evolutionary points

    bool waveEnded = true; // tells if wave is over

    List<WavePreset> historyWaves = new List<WavePreset>();

    int enemiesPassed = 0; // counts the amount of enemies that reached end
    float enemiesRemain; // counts the remainig enemies

    int towersGenerated; // counts how many towers has been generated

    public Text text; // prefab for Text object displaying info on towers
    public Text waveDescription; // prefab for Text object for describing a wave
    public Text scoreText; //prefab for Text object fot score displaying

    //Method that draws all buttons and controls what is going on after each button press
    void OnGUI() {
        if (GUI.Button(new Rect(110, 10, 100, 50), "NewGame")) {
            newGame();
            waveDescription.text = "Wave " + waveNumber + "\n"
                + wavePreset.getInfo();
        }

        //Dynamically generated buttons for towers
        for (int i = 0; i < towerPresets.Count; i++) {
            if (GUI.Button(new Rect(10, 10 + i * 25, 100, 30), "Tower " + i)) {
                text.text = towerPresets[i].getInfo();
                selectedPreset = i;
            }
        }


        if (GUI.Button(new Rect(110, 410, 100, 30), "Place Tower")) {
            if (selectedPreset != -1) {
                Debug.Log(selectedPreset);
                placeTower();
            }
        }

        if (GUI.Button(new Rect(10, 410, 100, 30), "Cancel")) {
            selectedPreset = -1;
            text.text = "";
        }
        if (GUI.Button(new Rect(210, 410, 100, 30), "Delete Tower")) {
            deleteTower();
        }
        if (GUI.Button(new Rect(530, 410, 100, 30), "Start Wave")) {
            if (waveEnded) {
                enemiesRemain = wavePreset.getCreepNumber();
                startWave();
                waveEnded = false;
            }
        }

        if (GUI.Button(new Rect(390, 410, 130, 30), "GenerateFinalWave")) {
            if (waveEnded) {
                getFinalWave();
            }
        }

    }

    // method for ending a wave
    public void endWave() {
        waveEnded = true;
        upgradeWave();
        enemiesPassed = 0;
    }

    // getting an enemy death notification
    public void notifyEnemyKilled() {
        enemiesRemain -= 1;
    }

    public void getFinalWave() {
        float futurePoints = 0;
        foreach (WavePreset wp in historyWaves) {
            futurePoints += wp.getPoints();
        }
        futurePoints /= historyWaves.Count;
        float max0 = 0;
        float max1 = 0;
        float max2 = 0;
        float max3 = 0;
        float mid0 = 0;
        float mid1 = 0;
        float mid2 = 0;
        float mid3 = 0;

        foreach (WavePreset wp in historyWaves) {
            mid0 += wp.getChromosome()[0];
        }
        mid0 /= historyWaves.Count;

        foreach (WavePreset wp in historyWaves) {
            mid2 += wp.getChromosome()[2];
        }
        mid2 /= historyWaves.Count;

        foreach (WavePreset wp in historyWaves) {
            mid1 += wp.getChromosome()[1];
        }
        mid1 /= historyWaves.Count;

        foreach (WavePreset wp in historyWaves) {
            mid3 += wp.getChromosome()[3];
        }
        mid3 /= historyWaves.Count;

        foreach (WavePreset wp in historyWaves) {
            if (wp.getChromosome()[0] > max0) {
                max0 = wp.getChromosome()[0];
            }
        }
        foreach (WavePreset wp in historyWaves) {
            if (wp.getChromosome()[1] > max1) {
                max1 = wp.getChromosome()[1];
            }
        }
        foreach (WavePreset wp in historyWaves) {
            if (wp.getChromosome()[2] > max2) {
                max2 = wp.getChromosome()[2];
            }
        }
        foreach (WavePreset wp in historyWaves) {
            if (wp.getChromosome()[3] > max3) {
                max3 = wp.getChromosome()[3];
            }
        }

        float[] finalChromosome = new float[4];


        finalChromosome[0] = max0 - Random.Range(0f, mid0);
        finalChromosome[1] = max1 - Random.Range(0f, mid1);
        finalChromosome[2] = max2 - Random.Range(0f, mid2);
        finalChromosome[3] = max3- Random.Range(0f, mid3);

        finalWave = new WavePreset(finalChromosome, futurePoints);
        string history = "FinalChromosome: " + finalChromosome[0] + ", " 
            + finalChromosome[1] + ", " + finalChromosome[2] + ", " 
            + finalChromosome[3] + ", points: " + futurePoints + "\r\n";
        System.IO.File.AppendAllText("Data.txt", history);

    }

    //method for starting a wave
    public void startWave() {
        GameObject go = GameObject.FindGameObjectWithTag("Spawner");
        go.GetComponent<SpawnerController>().spawnWave(wavePreset);
        waveNumber++;
    }


    //in case of enemy killed
    public void enemyKilled(float _score) {
        score += _score;
    }

    //in case of enemy reached the castle
    public void enemyReachedEnd() {
        enemiesPassed += 1;
    }

    /*
     * Generates a new wave preset with evolutionary
     * points increased by 5% in case if
     * no enemy reached the end in the previous run
     */
    public void upgradeWave() {
        if (enemiesPassed < 1) {
            /*
            points = points + points * 5f / 100;
            float[] chromosome = new float[4];
            for (int i = 0; i < 4; i++) {
                chromosome[i] = Random.Range((float)0, (float)1);
            }*/
            points = points + points * 5f / 100;
            wavePreset = new WavePreset(wavePreset.getChromosome(), points);
            waveDescription.text = "Wave " + waveNumber + "\n"
                + wavePreset.getInfo();
        } else {
            historyWaves.Add(wavePreset);
            float[] ch = wavePreset.getChromosome();
            string history = "Chromosome: " + ch[0] + ", " + ch[1] + ", " + ch[2] + ", " + ch[3] + ", points: " + points + "\r\n";
            System.IO.File.AppendAllText("Data.txt", history);
            points = 2000;
            float[] chromosome = new float[4];
            for (int i = 0; i < 4; i++) {
                chromosome[i] = Random.Range((float)0, (float)1);
            }
            waveNumber = 0;
            wavePreset = new WavePreset(chromosome, 2000);
            waveDescription.text = "Wave " + waveNumber + "\n"
               + wavePreset.getInfo();
        }
    }

    // method for placing a tower
    public void deleteTower() {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject go in gos) {
            //Debug.Log(go.GetComponent<PlaneController>().isSelected() + " " + go.GetComponent<PlaneController>().isTowerSet());
            if (go.GetComponent<PlaneController>().isSelected() && go.GetComponent<PlaneController>().isTowerSet()) {
                TowerPreset oldPreset = go.GetComponent<PlaneController>().deleteTower();
                int index = towerPresets.IndexOf(oldPreset);
                towerPresets[index] = null;
                towerPresets[index] = generateNewTowerPreset(oldPreset.getChromosome());
                break;
            }
        }
    }

    //method for deleting a tower
    public void placeTower() {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Plane");
        Debug.Log(gos.Length);
        foreach (GameObject go in gos) {
            if (go.GetComponent<PlaneController>().isSelected() && !towerPresets[selectedPreset].isSet()) {
                go.GetComponent<PlaneController>().setTower(towerPresets[selectedPreset]);
                towerPresets[selectedPreset].setTower();
                break;
            }
        }
    }

    // method for reseting waves and towers
    public void clearAll() {
        waveNumber = 1;
        towerPresets.Clear();
        enemyPopulation = 0;
    }

    // starts a new game
    public void newGame() {
        clearAll();
        selectedPreset = -1;
        enemiesPassed = 0;
        enemiesRemain = 0;
        points = 2000;
        waveEnded = true;
        map.GetComponent<MapGenerator>().CreateMap();
        setStartingTowers();
        setStartingWave();
    }

    //sets a starting wave with 2000 evolutionary points
    public void setStartingWave() {
        float[] chromosome = new float[4];
        for (int i = 0; i < 4; i++) {
            chromosome[i] = Random.Range((float)0, (float)1);
        }
        wavePreset = new WavePreset(chromosome, 2000);
    }

    // sets a starting set of tower presets
    public void setStartingTowers() {
        List<float[]> presets = new List<float[]>();

        for (int i = 0; i < 14; i++) {
            float[] chromosome = new float[11];
            for (int j = 0; j < 11; j++) {
                chromosome[j] = Random.Range((float)0, (float)0.2);
            }
            presets.Add(chromosome);
        }
        for (int i = 0; i < 2; i++) {
            float[] chromosome = new float[11];
            for (int j = 0; j < 11; j++) {
                chromosome[j] = Random.Range((float)0, (float)1);
            }
            presets.Add(chromosome);
        }

        foreach (float[] chromosome in presets) {
            TowerPreset tp = new TowerPreset();
            tp.TowerPresetFromChromosome(chromosome);
            towerPresets.Add(tp);
        }
    }

    /*
     * If tower was deleted, new tower must
     * be created using crossover of tower that
     * was deleted and two random towers in the
     * population
     */
    public void towerDeleted(TowerPreset tp) {
        towersGenerated = 0;
        float[] chromosome = tp.getChromosome();
        towerPresets.Remove(tp);
        generateNewTowerPreset(chromosome);
    }

    /*
     * Method that generates a new tower using crossover over 
     * given chromosome and two random chromosomes from population of towers
     * Every 15'th tower is generated using random.
     */
    public TowerPreset generateNewTowerPreset(float[] _chromosome) {
        if (!(towersGenerated % 15 == 0)) {
            float[] individ1 = towerPresets[Random.Range(0, towerPresets.Count)].getChromosome();
            float[] individ2 = towerPresets[Random.Range(0, towerPresets.Count)].getChromosome();

            float[] newChromosome = new float[11];
            int point1 = Random.Range(0, 11);
            int point2 = Random.Range(point1, 11);
            int swap = Random.Range(0, 1);
            if (swap == 0) {
                float[] temp = _chromosome;
                _chromosome = individ1;
                individ1 = temp;
            } else {
                float[] temp = _chromosome;
                _chromosome = individ2;
                individ1 = temp;
            }
            for (int i = 0; i < point1; i++) {
                newChromosome[i] = _chromosome[i];
            }
            for (int i = point1; i < point2; i++) {
                newChromosome[i] = individ1[i];
            }
            for (int i = point2; i < 11; i++) {
                newChromosome[i] = individ2[i];
            }

            TowerPreset generated = new TowerPreset();
            generated.TowerPresetFromChromosome(newChromosome);
            return generated;
        } else {
            float[] chromosome = new float[11];
            for (int j = 0; j < 11; j++) {
                chromosome[j] = Random.Range((float)0, (float)1);
            }
            TowerPreset tp = new TowerPreset();
            tp.TowerPresetFromChromosome(chromosome);
            return tp;
        }
    }


    // Update is called once per frame. Updates the scoreText and notices the end of the wave
    void Update() {
        scoreText.text = "Score: " + score.ToString("F2") + "\n"
            + "Enemies remain: " + enemiesRemain + "\n"
            + "Enemies reached end: " + enemiesPassed;

        if (enemiesRemain <= 0 && waveEnded == false) {
            endWave();
        }
    }
}
