using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static int nbEnemiesPerPlane = 20;

    private static GameObject enemyBodyPrefab;
    private static GameObject enemyMouthPrefab;
    private static GameObject enemyFlagelPrefab;
    private static Dictionary<LocalPlanes, bool> planeEnemyStatus;
    private static Dictionary<LocalPlanes, List<GameObject>> planeEnemies;

    void Start() {
        enemyBodyPrefab = (GameObject)Resources.Load("Prefabs/Body", typeof(GameObject));
        enemyMouthPrefab = (GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject));
        enemyFlagelPrefab = (GameObject)Resources.Load("Prefabs/Flagel", typeof(GameObject));

        initializeDictionaries();
    }

    void Update() {

    }

    public static void spawnEnemiesOnPlane(LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
        if (planeEnemyStatus[plane] == true) {
            return;
        }

        for (int i = 0; i < nbEnemiesPerPlane; i++) {
            float xPos = Random.Range(0, planeSize.x / 2);
            float zPos = Random.Range(0, planeSize.z / 2);
            List<int> signs = new List<int>() { -1, 1 };
            int xSign = Random.Range(0, 2);
            int zSign = Random.Range(0, 2);

            float finalXPos = planeCoord.x + signs[xSign] * xPos;
            float finalYPos = 0.2f;
            float finalZPos = planeCoord.z + signs[zSign] * zPos;

            GameObject enemy = instantiateEnemy(finalXPos, finalYPos, finalZPos);

            enemy.name = Enemies.OG_ENEMY.ToString();
            planeEnemies[plane].Add(enemy);
            planeEnemyStatus[plane] = true;
        }
    }

    public static void discardEnemiesOnPlane(LocalPlanes plane) {
        planeEnemyStatus[plane] = false;

        foreach (GameObject enemy in planeEnemies[plane]) {
            Destroy(enemy);
        }
    }

    public static void switchEnemiesOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        List<GameObject> tempFood = planeEnemies[plane1];
        planeEnemies[plane1] = planeEnemies[plane2];
        planeEnemies[plane2] = tempFood;
    }

    private static void initializeDictionaries() {
        planeEnemyStatus = new Dictionary<LocalPlanes, bool>();
        planeEnemyStatus.Add(LocalPlanes.CURRENT_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.X_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.Z_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.XZ_PLANE, false);

        planeEnemies = new Dictionary<LocalPlanes, List<GameObject>>();
        planeEnemies.Add(LocalPlanes.CURRENT_PLANE, new List<GameObject>());
        planeEnemies.Add(LocalPlanes.X_PLANE, new List<GameObject>());
        planeEnemies.Add(LocalPlanes.Z_PLANE, new List<GameObject>());
        planeEnemies.Add(LocalPlanes.XZ_PLANE, new List<GameObject>());
    }

    private static GameObject instantiateEnemy(float xPos, float yPos, float zPos) {

        GameObject enemy = new GameObject();
        GameObject enemyBody = Instantiate(enemyBodyPrefab, new Vector3(xPos, yPos, zPos), new Quaternion(0.71f, 0, 0.71f, 0));
        GameObject enemyMouth = Instantiate(enemyMouthPrefab, new Vector3(xPos, yPos, zPos + 0.5f), Quaternion.identity);
        GameObject enemyFlagel = Instantiate(enemyFlagelPrefab, new Vector3(xPos, yPos, zPos - 0.7f), new Quaternion(0.71f, 0, 0, -0.71f));

        enemyBody.transform.SetParent(enemy.transform);
        enemyMouth.transform.SetParent(enemy.transform);
        enemyFlagel.transform.SetParent(enemy.transform);

        return enemy;
    }
}
