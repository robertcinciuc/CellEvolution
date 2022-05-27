using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    
    public int nbEnemiesPerPlane = 20;
    public Dictionary<LocalPlanes, Dictionary<int, GameObject>> planeEnemies;

    private GameObject enemyBodyPrefab;
    private GameObject enemyMouthPrefab; 
    private GameObject enemyFlagellaPrefab;
    private GameObject enemySpikePrefab;
    private Dictionary<LocalPlanes, bool> planeEnemyStatus;
    private TerrainRenderer terrainRenderer;
    private IslandSpawner islandSpawner;

    void Start() {
    }

    void Update() {

    }

    public void spawnEnemiesOnPlane(LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
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

            //Checking if spawn position is not occupied by land
            int xPosInt = Mathf.FloorToInt(xPos);
            int zPosInt = Mathf.FloorToInt(zPos);
            int xPosInGrid = (int)(xPosInt - Mathf.CeilToInt(planeCoord.x - planeSize.x / 2)) / islandSpawner.gridPrecision;
            int zPosInGrid = (int)(zPosInt - Mathf.CeilToInt(planeCoord.z - planeSize.z / 2)) / islandSpawner.gridPrecision;

            if (xPosInGrid < 0 || xPosInGrid >= 50 || zPosInGrid < 0 || zPosInGrid >= 50) {
                Debug.Log("s");
            }

            if(!islandSpawner.planeGridsWithLand[plane][xPosInGrid, zPosInGrid]) {
                GameObject enemy = instantiateEnemy(xPos, yPos, zPos, plane, planeCoord);
                planeEnemies[plane].Add(enemy.GetInstanceID(), enemy);
            }
        }
        planeEnemyStatus[plane] = true;
    }

    public void discardEnemiesOnPlane(LocalPlanes plane) {
        planeEnemyStatus[plane] = false;

        foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies[plane]) {
            Destroy(enemyEntry.Value);
        }
        planeEnemies[plane].Clear();

    }

    public void switchEnemiesOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        Dictionary<int, GameObject> tempEnemyEntry = planeEnemies[plane1];
        planeEnemies[plane1] = planeEnemies[plane2];
        planeEnemies[plane2] = tempEnemyEntry;
    }

    public void moveEnemyToPlane(int enemyID, LocalPlanes sourcePlane, LocalPlanes targetPlane) {
        GameObject tempEnemy = planeEnemies[sourcePlane][enemyID];
        planeEnemies[sourcePlane].Remove(enemyID);
        planeEnemies[targetPlane].Add(enemyID, tempEnemy);
    }

    public void deleteEnemy(int enemyID) {
        foreach(KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> enemyEntry in planeEnemies){
            if (enemyEntry.Value.ContainsKey(enemyID)) {
                GameObject tempObj = planeEnemies[enemyEntry.Key][enemyID];
                planeEnemies[enemyEntry.Key].Remove(enemyID);
                Destroy(tempObj);
            }
        }
    }

    public void initializeEnemySpawner() {
        terrainRenderer = gameObject.GetComponent<TerrainRenderer>();
        islandSpawner = gameObject.GetComponent<IslandSpawner>();

        enemyBodyPrefab = (GameObject)Resources.Load("Prefabs/Body", typeof(GameObject));
        enemyMouthPrefab = (GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject));
        enemyFlagellaPrefab = (GameObject)Resources.Load("Prefabs/Flagella", typeof(GameObject));
        enemySpikePrefab = (GameObject)Resources.Load("Prefabs/Spike", typeof(GameObject));

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

    private GameObject instantiateEnemy(float xPos, float yPos, float zPos, LocalPlanes localPlane, Vector3 localPlaneCoord) {

        GameObject enemy = new GameObject();
        enemy.name = Enemies.OG_ENEMY.ToString();
        enemy.transform.position = new Vector3(xPos, yPos, zPos);

        GameObject enemyBody = Instantiate(enemyBodyPrefab, new Vector3(xPos, yPos, zPos), new Quaternion(0.71f, 0, 0.71f, 0));
        GameObject enemyMouth = Instantiate(enemyMouthPrefab, new Vector3(xPos, yPos, zPos + 1f), Quaternion.identity);
        GameObject enemyFlagella = Instantiate(enemyFlagellaPrefab, new Vector3(xPos, yPos, zPos - 0.7f), Quaternion.identity);
        GameObject enemySpike = Instantiate(enemySpikePrefab, new Vector3(xPos, yPos, zPos + 2f), Quaternion.identity);
        GameObject enemyHealthBar = Instantiate((GameObject)Resources.Load("Prefabs/EnemyHealthBar", typeof(GameObject)), enemy.transform.position, Quaternion.identity);
        GameObject enemyVisionCone = Instantiate((GameObject)Resources.Load("Prefabs/EnemyVisionCone", typeof(GameObject)), enemy.transform.position + new Vector3(0, 0, 3), Quaternion.identity);
        enemyVisionCone.GetComponent<MeshRenderer>().enabled = false;

        enemyBody.transform.SetParent(enemy.transform);
        enemyBody.name = Bodies.OriginalEnemyBody.ToString();
        enemyMouth.transform.SetParent(enemy.transform);
        enemyMouth.name = Mouths.Mouth.ToString();
        enemyFlagella.transform.SetParent(enemy.transform);
        enemyFlagella.name = LocomotionOrgans.Flagella.ToString();
        enemySpike.transform.SetParent(enemy.transform);
        enemySpike.name = AttackOrgans.Spike.ToString();
        enemyHealthBar.transform.SetParent(enemy.transform);
        enemyVisionCone.transform.SetParent(enemy.transform);

        enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        enemy.AddComponent<Rigidbody>();

        EnemyBoundChecker enemyBoundChecker = enemy.AddComponent<EnemyBoundChecker>();
        enemyBoundChecker.setLocalPlane(localPlane, localPlaneCoord);
        enemyBoundChecker.setTerrainRenderer(terrainRenderer);
        enemyBoundChecker.setEnemySpawner(this);
        
        EnemyState enemyState = enemy.AddComponent<EnemyState>();
        enemyState.setEnemySpawner(this);

        enemy.AddComponent<EnemyMovement>();

        enemyHealthBar.transform.Find("Canvas").Find("EnemyHealthBar").GetComponent<EnemyHealthBar>().setMaxHealth(100f);

        return enemy;
    }
}
