using UnityEngine;
using System.Collections;
/*
 * Class that represents the parameters of a tower
 */
public class TowerPreset {

    public float range;
    public float shootingSpeed;
    public float splashRange;
    public float damage;
    public float slow;
    public float resourseIncrease;
    public float tripleDamage;
    public float armorReduce;
    public float poison;
    public float shock;
    public float chainHit;
    bool set;

    float[] chromosome;

    public TowerPreset(float _range, float _shootingSpeed, float _splashRange, float _damage, float _slow,
        float _resourseIncrease, float _tripleDamage, float _armorReduce, float _poison, float _shock, float _chainHit) {

        range = _range;
        shootingSpeed = _shootingSpeed;
        splashRange = _splashRange;
        damage = _damage;
        slow = _slow;
        resourseIncrease = _resourseIncrease;
        tripleDamage = _tripleDamage;
        armorReduce = _armorReduce;
        poison = _poison;
        shock = _shock;
        chainHit = _chainHit;
    }

    public TowerPreset() {
        set = false;
    }

    // method for setting parameters from chromosome
    public void TowerPresetFromChromosome(float[] _chromosome) {
        set = false;
        chromosome = _chromosome;
        range = chromosome[0] * (20 - 5) + 5;
        shootingSpeed = chromosome[1] * (4 - 1) + 1;
        splashRange = chromosome[2] * 80;
        damage = chromosome[3] * (100 - 3) + 3;
        slow = chromosome[4] * 80;
        resourseIncrease = chromosome[5] * 10;
        tripleDamage = chromosome[6] * 80;
        armorReduce = chromosome[7] * 80;
        poison = chromosome[8] * 50;
        shock = chromosome[9] * (80 - 10) + 10;
        chainHit = chromosome[10] * 8;
    }

    public float[] getChromosome() {
        return chromosome;
    }

    public void setTower() {
        set = true;
    }

    public bool isSet() {
        return set;
    }

    public float[] getPreset() {
        return chromosome;
    }

    public string getInfo() {
        string description = "Range:" + range.ToString("F2") + "\n"
            + "Shooting Speed:" + shootingSpeed.ToString("F2") + "\n"
            + "Splash Range:" + splashRange.ToString("F2") + "\n"
            + "Damage:" + damage.ToString("F2") + "\n"
            + "Slow:" + slow.ToString("F2") + "\n"
            + "Resouese Increase:" + resourseIncrease.ToString("F2") + "\n"
            + "tripleDamage:" + tripleDamage.ToString("F2") + "\n"
            + "armorReduce:" + armorReduce.ToString("F2") + "\n"
            + "poison:" + poison.ToString("F2") + "\n"
            + "shock:" + shock.ToString("F2") + "\n"
            + "chainHit:" + chainHit.ToString("F2") + "\n"
            + "already set:" + set + "\n";
        return description;
    }

}
