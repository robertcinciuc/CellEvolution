using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour {

    private Dictionary<LocalPlanes, bool> islandsSpawned;
    private Dictionary<LocalPlanes, List<GameObject>> planeIslands;
    private float creationThreshold = 2 * 0.92f;
    private int scaleRange = 20;

    void Start(){
        
    }

    void Update(){
        
    }

    public void initializeIslandSpawner() {
        islandsSpawned = new Dictionary<LocalPlanes, bool>();
        islandsSpawned.Add(LocalPlanes.CURRENT_PLANE, false);
        islandsSpawned.Add(LocalPlanes.X_PLANE, false);
        islandsSpawned.Add(LocalPlanes.Z_PLANE, false);
        islandsSpawned.Add(LocalPlanes.XZ_PLANE, false);

        planeIslands = new Dictionary<LocalPlanes, List<GameObject>>();
        planeIslands.Add(LocalPlanes.CURRENT_PLANE, new List<GameObject>());
        planeIslands.Add(LocalPlanes.X_PLANE, new List<GameObject>());
        planeIslands.Add(LocalPlanes.Z_PLANE, new List<GameObject>());
        planeIslands.Add(LocalPlanes.XZ_PLANE, new List<GameObject>());
    }

    public void spawnIslandsOnPlane(LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
        if (islandsSpawned[plane] == true) {
            return;
        }

        List<SerialIsland> serialIslands = computeIslandData(planeCoord, planeSize);
        foreach (SerialIsland serialIsland in serialIslands) {
            GameObject island = Instantiate((GameObject)Resources.Load("Prefabs/Island", typeof(GameObject)), serialIsland.pos, Quaternion.identity);
            island.transform.localScale = new Vector3(serialIsland.scale, island.transform.localScale.y, serialIsland.scale);

            planeIslands[plane].Add(island);
        }
        islandsSpawned[plane] = true;
    }

    public void discardIslandsOnPlane(LocalPlanes plane) {
        islandsSpawned[plane] = false;

        foreach (GameObject island in planeIslands[plane]) {
            Destroy(island);
        }
    }
    public void switchIslandsOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        List<GameObject> tempFood = planeIslands[plane1];
        planeIslands[plane1] = planeIslands[plane2];
        planeIslands[plane2] = tempFood;
    }

    private List<SerialIsland> computeIslandData(Vector3 planeCoord, Vector3 planeSize) {
        int leftX = Mathf.CeilToInt(planeCoord.x - planeSize.x / 2);
        int botZ = Mathf.CeilToInt(planeCoord.z - planeSize.z / 2);
        int rightX = Mathf.FloorToInt(planeCoord.x + planeSize.x / 2);
        int topZ = Mathf.FloorToInt(planeCoord.z + planeSize.z / 2);
        List<SerialIsland> serialIslands = new List<SerialIsland>();

        for (int i = leftX; i < rightX; i += 2) {
            for(int j = botZ; j < topZ; j += 2) {
                if(i != 0 && j != 0) {
                    //Ensures the generation is deterministic
                    Random.InitState(i + j);
                    float randomI = Random.Range(0, Mathf.Abs(i)) * 1f / Mathf.Abs(i);
                    float randomJ = Random.Range(0, Mathf.Abs(j)) * 1f / Mathf.Abs(j);

                    //Add new island on this condition + generate rest of its data
                    if (randomI + randomJ > creationThreshold) {
                        Random.InitState(2 * i + 2 * j);
                        int scale = Random.Range(1, scaleRange);
                        System.Guid islandId = System.Guid.NewGuid();
                        SerialIsland serialIsland = new SerialIsland(new Vector3(i, 0, j), scale, islandId);

                        serialIslands.Add(serialIsland);
                    }
                }
            }
        }

        return serialIslands;
    }
}
