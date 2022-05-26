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
    }

    void Update(){
        
    }
}
