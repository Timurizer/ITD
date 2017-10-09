using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * Class that generates maps in a form of 2d array filled with 0, 1, 2, 3
 * 0 - plane
 * 1 - path
 * 2 - castle(final point)
 * 3 - spawn point
 * Map creator here is very similar to look ahead digger, however it places no rooms
 */
public class MapCreator {

    public int resolution; //map resolution

    int x; //x coordinate of the digger
    int y; //y coordinate of the digger

    int changeDirChance; //chance of changing the direction of the digger

    List<int> corridorSizes;

    List<int[]> path;

    int tempLifes;
    int lifes; //lifes of the digger

    char direction; //current direction of movement of the digger

    int[,] map; //map itself. 0 - rock, 1 - free space

    public MapCreator(int _resolution, int _lifes) {
        path = new List<int[]>();
        lifes = _lifes;
        tempLifes = _lifes;
        resolution = _resolution;
    }
    //sets coordinate of the digger to the _x, _y position
    public void setCoordinate(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void placeSpawnPoint() {
        map[x, y] = 3;
        if (direction == 'r') {
            setCoordinate(x + 1, y);
        }
        if (direction == 'l') {
            setCoordinate(x - 1, y);
        }
        if (direction == 'u') {
            setCoordinate(x, y + 1);
        }
        if (direction == 'd') {
            setCoordinate(x, y - 1);
        }
    }

    public List<int[]> getPath() {
        return path;
    }

    //makes a new map with resolution set as _resolution and fills it with 0
    //randomly places the digger.
    public void newMap() {

        path = new List<int[]>();
        corridorSizes = new List<int>();

        int minCorridorLength = 2;
        int maxCorridorLength = resolution / 4;

        for (int i = minCorridorLength; i < maxCorridorLength; i++) {
            corridorSizes.Add(i);
        }

        map = new int[resolution, resolution];

        direction = 'r';

        this.x = Random.Range(1, resolution - 1);
        this.y = Random.Range(1, resolution - 1);

    }

    public void setDirection(char _direction) {
        direction = _direction;
    }

    // Changes the direction of the digger to opposite direction
    public char setOppositeDirection(char current) {
        if (current == 'r') {
            direction = 'l';
            return direction;
        }
        if (current == 'l') {
            direction = 'r';
            return direction;
        }
        if (current == 'u') {
            direction = 'd';
            return direction;
        }
        if (current == 'd') {
            direction = 'u';
            return direction;
        }
        return 'r';
    }

    //changes direction of the digger to other than current direction is
    public void changeDirection(char current) {
        while (direction == current) {
            int choice = Random.Range(0, 4);
            switch (choice) {
                case 0:
                    direction = 'r';
                    break;
                case 1:
                    direction = 'l';
                    break;
                case 2:
                    direction = 'u';
                    break;
                case 3:
                    direction = 'd';
                    break;
            }
        }
    }

    //places the corridor
    public void placeCorridor() {

        List<int> temp = new List<int>(corridorSizes);
        int length = temp[Random.Range(0, temp.Count - 1)];
        while (!canPlaceCorridor(length)) {
            temp.Remove(length);
            if (temp.Count == 0) {
                break;
            }
            length = temp[Random.Range(0, temp.Count - 1)];
        }

        if (temp.Count != 0) {
            if (canPlaceCorridor(length)) {
                lifes = lifes - length;
                if (direction == 'r') {
                    for (int i = x; i < x + length; i++) {
                        int[] tmp = { i, y };
                        path.Add(tmp);
                        map[i, y] = 1;
                    }
                    setCoordinate(x + length, y);
                    return;
                }

                if (direction == 'l') {
                    List<int[]> backwards = new List<int[]>();
                    for (int i = x - length + 1; i <= x; i++) {
                        int[] tmp = { i, y };
                        backwards.Add(tmp);
                        map[i, y] = 1;
                    }
                    for (int i = backwards.Count - 1; i > 0; i--) {
                        path.Add(backwards[i]);
                    }
                    setCoordinate(x - length, y);
                    return;
                }

                if (direction == 'u') {
                    for (int i = y; i < y + length; i++) {
                        int[] tmp = { x, i };
                        path.Add(tmp);
                        map[x, i] = 1;
                    }
                    setCoordinate(x, y + length);
                    return;
                }

                if (direction == 'd') {
                    List<int[]> backwards = new List<int[]>();
                    for (int i = y - length + 1; i <= y; i++) {
                        int[] tmp = { x, i };
                        backwards.Add(tmp);
                        map[x, i] = 1;
                    }
                    for (int i = backwards.Count - 1; i > 0; i--) {
                        path.Add(backwards[i]);
                    }
                    setCoordinate(x, y - length);
                    return;
                }
            }
        } else {
            changeDirection(direction);
        }

    }


    // Checks if can place the corridor of given length in the current direction of digger
    public bool canPlaceCorridor(int length) {

        if (direction == 'r') {
            if (length + x > resolution - 2) {
                return false;
            }
            for (int i = x; i < x + length + 2; i++) {
                if (map[i, y] == 1 || map[i, y] == 3) {
                    return false;
                }
            }
        }

        if (direction == 'l') {
            if (x - length < 2) {
                return false;
            }
            for (int i = x - length - 2; i < x; i++) {
                if (map[i, y] == 1 || map[i, y] == 3) {
                    return false;
                }
            }
        }

        if (direction == 'u') {
            if (length + y > resolution - 2) {
                return false;
            }
            for (int i = y; i < y + length + 2; i++) {
                if (map[x, i] == 1 || map[x, i] == 3) {
                    return false;
                }
            }
        }

        if (direction == 'd') {
            if (y - length < 2) {
                return false;
            }
            for (int i = y - length - 2; i < y; i++) {
                if (map[x, i] == 1 || map[x, i] == 3) {
                    return false;
                }
            }
        }

        return true;
    }


    /*
     * Main generation method
     * While lifes > 0:
     * -check if should change direction, change if should and set probability of change to 30
     * -place corridor
     * -increase the probability of changing the direction
     * -check if stuck
     */
    public void generate() {
        int oldlifes;
        changeDirChance = 30;
        int stuck = 0;
        placeSpawnPoint();
        while (lifes > 0) {
            oldlifes = lifes;
            if (Random.Range(0, 101) < changeDirChance) {
                changeDirection(direction);
                changeDirChance = 30;
            }


            placeCorridor();

            changeDirChance += 10;
            if (oldlifes == lifes) {
                stuck++;
            } else {
                stuck = 0;
            }
            if (stuck > 10) {
                return;
            }
        }
        map[x, y] = 2;
        int[] tmp = { x, y };
        path.Add(tmp);
    }

    //generates a map of given resolution and returns it
    public int[,] takeMap() {
        while (lifes > 0) {
            newMap();
            lifes = tempLifes;
            generate();
        }
        return map;
    }

    // used before importing class to Unity
    public void showMap() {
        for (int i = 0; i < resolution; i++) {
            for (int j = 0; j < resolution; j++) {
                System.Console.Write(map[i, j] + " ");
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine();
    }


}