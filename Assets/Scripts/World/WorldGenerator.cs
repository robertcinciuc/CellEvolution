using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public TerrainRenderer terrainRenderer;
    public EnemySpawner enemySpawner;
    public FoodSpawner foodSpawner;
    public IslandSpawner islandSpawner;

    void Start(){
        terrainRenderer = gameObject.GetComponent<TerrainRenderer>();
        islandSpawner = gameObject.GetComponent<IslandSpawner>();
        enemySpawner = gameObject.GetComponent<EnemySpawner>();
        foodSpawner = gameObject.GetComponent<FoodSpawner>();

        terrainRenderer.initializeTerrain();
        islandSpawner.initializeIslandSpawner();
        enemySpawner.initializeEnemySpawner();
        foodSpawner.initializeFoodSpawner();

        //Spawn elements on initial current plane
        islandSpawner.spawnIslandsOnPlane(LocalPlanes.CURRENT_PLANE, terrainRenderer.currPlane.transform.position, terrainRenderer.planeSize);
        foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.CURRENT_PLANE, terrainRenderer.currPlane.transform.position, terrainRenderer.planeSize);
        enemySpawner.spawnEnemiesOnPlane(LocalPlanes.CURRENT_PLANE, terrainRenderer.currPlane.transform.position, terrainRenderer.planeSize);
    }

    void Update(){
        
    }
}
