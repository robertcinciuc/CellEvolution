using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public TerrainRenderer terrainRenderer;
    public EnemySpawner enemySpawner;
    public FoodSpawner foodSpawner;

    void Start(){
        terrainRenderer = gameObject.GetComponent<TerrainRenderer>();
        enemySpawner = gameObject.GetComponent<EnemySpawner>();
        foodSpawner = gameObject.GetComponent<FoodSpawner>();

        terrainRenderer.initializeTerrain();
        enemySpawner.initializeEnemySpawner();
        foodSpawner.initializeFoodSpawner();

        //Spawn elements on initial current plane
        foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.CURRENT_PLANE, terrainRenderer.currPlane.transform.position, terrainRenderer.planeSize);
        enemySpawner.spawnEnemiesOnPlane(LocalPlanes.CURRENT_PLANE, terrainRenderer.currPlane.transform.position, terrainRenderer.planeSize);
    }

    void Update(){
        
    }
}
