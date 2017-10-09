using UnityEngine;
using System.Collections;

/*
 * Class for representing the parameters of a wave
 */
public class WavePreset {

    float[] chromosome;
    int creepNumber;
    float pointsPerCreep;
    float speed;
    float armor;
    float hp;

    float points;

    // initializes using chromosome and points value
    public WavePreset(float[] _chromosome, float _points) {

        chromosome = _chromosome;
        points = _points;
        Debug.Log("points: " + points);
        creepNumber = (int)(chromosome[0] * 20 + 1);
        pointsPerCreep = points / (creepNumber + 2);
        float twf = chromosome[1] + chromosome[2] + chromosome[3];
        speed = (chromosome[1] / twf * points)/ 100 + 5;
        armor = (chromosome[2] / twf * points) / 20;
        hp = (chromosome[3] / twf * points) / 20;
    }

    public float[] getChromosome() {
        return chromosome;
    }

    public float getPoints() {
        return points;
    }

    public float getHp() {
        return hp;
    }

    public float getSpeed() {
        return speed;
    }

    public float getArmor() {
        return armor;
    }

    public float getPointsPerCreep() {
        return pointsPerCreep;
    }

    public float getCreepNumber() {
        return creepNumber;
    }

    public string getInfo() {
        string info = "Number of enemies: " + creepNumber + "\n"
            + "HP: " + hp.ToString("F2") + "\n"
            + "Speed: " + speed.ToString("F2") + "\n"
            + "Armor: " + armor.ToString("F2") + "\n"
            + "Points per creep: " + pointsPerCreep.ToString("F2") + "\n"
            + "Total wave points: " + points.ToString("F2");
        return info;
    }

}
