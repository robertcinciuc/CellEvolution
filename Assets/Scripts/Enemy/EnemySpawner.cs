using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    public static int nbEnemiesPerPlane = 20;
    public static Dictionary<LocalPlanes, Dictionary<int, GameObject>> planeEnemies;

    private static GameObject enemyBodyPrefab;
    private static GameObject enemyMouthPrefab;
    private static GameObject enemyFlagellaPrefab;
    private static GameObject enemySpikePrefab;
    private static Dictionary<LocalPlanes, bool> planeEnemyStatus;

    void Start() {
        enemyBodyPrefab = (GameObject)Resources.Load("Prefabs/Body", typeof(GameObject));
        enemyMouthPrefab = (GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject));
        enemyFlagellaPrefab = (GameObject)Resources.Load("Prefabs/Flagella", typeof(GameObject));
        enemySpikePrefab = (GameObject)Resources.Load("Prefabs/Spike", typeof(GameObject));

        initializeDictionaries();
    }

    void Update() {

    }

    public static void spawnEnemiesOnPlane(LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
        if (planeEnemyStatus[plane] == true) {
            return;
        }

        for (int i = 0; i < nbEnemiesPerPlane; i++) {
            List<int> signs = new List<int>() { -1, 1 };
            int xSign = Random.Range(0, 2);
            int zSign = Random.Range(0, 2);

            float xPos = planeCoord.x + signs[xSign] * Random.Range(0, planeSize.x / 2);
            float yPos = 0.2f;
            float zPos = planeCoord.z + signs[zSign] * Random.Range(0, planeSize.z / 2);

            GameObject enemy = instantiateEnemy(xPos, yPos, zPos, plane, planeCoord);

            planeEnemies[plane].Add(enemy.GetInstanceID(), enemy);
            planeEnemyStatus[plane] = true;
        }
    }

    public static void discardEnemiesOnPlane(LocalPlanes plane) {
        planeEnemyStatus[plane] = false;

        foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies[plane]) {
            Destroy(enemyEntry.Value);
        }
        planeEnemies[plane].Clear();

    }

    public static void switchEnemiesOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        Dictionary<int, GameObject> tempEnemyEntry = planeEnemies[plane1];
        planeEnemies[plane1] = planeEnemies[plane2];
        planeEnemies[plane2] = tempEnemyEntry;
    }

    public static void moveEnemyToPlane(int enemyID, LocalPlanes sourcePlane, LocalPlanes targetPlane) {
        GameObject tempEnemy = planeEnemies[sourcePlane][enemyID];
        planeEnemies[sourcePlane].Remove(enemyID);
        planeEnemies[targetPlane].Add(enemyID, tempEnemy);
    }

    public static void deleteEnemy(int enemyID) {
        foreach(KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> enemyEntry in planeEnemies){
            if (enemyEntry.Value.ContainsKey(enemyID)) {
                GameObject tempObj = planeEnemies[enemyEntry.Key][enemyID];
                planeEnemies[enemyEntry.Key].Remove(enemyID);
                Destroy(tempObj);
            }
        }
    }

    private static void initializeDictionaries() {
        planeEnemyStatus = new Dictionary<LocalPlanes, bool>();
        planeEnemyStatus.Add(LocalPlanes.CURRENT_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.X_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.Z_PLANE, false);
        planeEnemyStatus.Add(LocalPlanes.XZ_PLANE, false);

        planeEnemies = new Dictionary<LocalPlanes, Dictionary<int, GameObject>>();
        planeEnemies.Add(LocalPlanes.CURRENT_PLANE, new Dictionary<int, GameObject>());
        planeEnemies.Add(LocalPlanes.X_PLANE, new Dictionary<int, GameObject>());
        planeEnemies.Add(LocalPlanes.Z_PLANE, new Dictionary<int, GameObject>());
        planeEnemies.Add(LocalPlanes.XZ_PLANE, new Dictionary<int, GameObject>());
    }

    private static GameObject instantiateEnemy(float xPos, float yPos, float zPos, LocalPlanes localPlane, Vector3 localPlaneCoord) {

        GameObject enemy = new GameObject();
        enemy.name = Enemies.OG_ENEMY.ToString();
        enemy.transform.position = new Vector3(xPos, yPos, zPos);

        GameObject enemyBody = Instantiate(enemyBodyPrefab, new Vector3(xPos, yPos, zPos), new Quaternion(0.71f, 0, 0.71f, 0));
        GameObject enemyMouth = Instantiate(enemyMouthPrefab, new Vector3(xPos, yPos, zPos + 0.5f), Quaternion.identity);
        GameObject enemyFlagella = Instantiate(enemyFlagellaPrefab, new Vector3(xPos, yPos, zPos - 0.7f), new Quaternion(0.71f, 0, 0, -0.71f));
        GameObject enemySpike = Instantiate(enemySpikePrefab, new Vector3(xPos, yPos + 0.2f, zPos + 1f), new Quaternion(0.71f, 0, 0, 0.71f));
        GameObject enemyHealthBar = Instantiate((GameObject)Resources.Load("Prefabs/EnemyHealthBar", typeof(GameObject)), enemy.transform.position, Quaternion.identity);


        enemyBody.transform.SetParent(enemy.transform);
        enemyMouth.transform.SetParent(enemy.transform);
        enemyFlagella.transform.SetParent(enemy.transform);
        enemySpike.transform.SetParent(enemy.transform);
        enemyHealthBar.transform.SetParent(enemy.transform);

        enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        enemy.AddComponent<Rigidbody>();
        enemy.AddComponent<EnemyBoundChecker>().setLocalPlane(localPlane, localPlaneCoord);
        enemy.AddComponent<EnemyState>();

        enemyHealthBar.transform.Find("Canvas").Find("EnemyHealthBar").GetComponent<EnemyHealthBar>().setMaxHealth(100f);

        return enemy;
    }
}
