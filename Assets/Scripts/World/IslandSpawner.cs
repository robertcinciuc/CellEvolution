using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour {

    public int gridPrecision = 2;
    public Dictionary<LocalPlanes, bool[,]> planeLandGrid;

    private Dictionary<LocalPlanes, bool> islandsSpawned;
    private Dictionary<LocalPlanes, List<GameObject>> planeIslands;
    private float creationThreshold = 2 * 0.94f;
    private int scaleMax = 20;
    private TerrainRenderer terrainRenderer;

    void Start(){
        
    }

    void Update(){
        
    }

    public void initializeIslandSpawner() {
        terrainRenderer = GetComponent<TerrainRenderer>();

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

        //Initialize grid with no land on plane
        int gridSizeX = Mathf.FloorToInt(terrainRenderer.planeSize.x) / gridPrecision;
        int gridSizeZ = Mathf.FloorToInt(terrainRenderer.planeSize.z) / gridPrecision;
        planeLandGrid = new Dictionary<LocalPlanes, bool[,]>();
        planeLandGrid.Add(LocalPlanes.CURRENT_PLANE, new bool[gridSizeX, gridSizeZ]);
        planeLandGrid.Add(LocalPlanes.X_PLANE, new bool[gridSizeX, gridSizeZ]);
        planeLandGrid.Add(LocalPlanes.Z_PLANE, new bool[gridSizeX, gridSizeZ]);
        planeLandGrid.Add(LocalPlanes.XZ_PLANE, new bool[gridSizeX, gridSizeZ]);
        
        foreach(KeyValuePair<LocalPlanes, bool[,]> entry in planeLandGrid) {
            for (int i = 0; i < entry.Value.GetLength(0); ++i) {
                for (int j = 0; j < entry.Value.GetLength(1); ++j) {
                    entry.Value[i, j] = false;
                }
            }
        }        
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
            markGridSpaceTakenByIsland(serialIsland.pos, serialIsland.scale, plane, planeCoord, planeSize);
        }
        islandsSpawned[plane] = true;
    }

    public void discardIslandsOnPlane(LocalPlanes plane) {
        islandsSpawned[plane] = false;

        foreach (GameObject island in planeIslands[plane]) {
            Destroy(island);
        }
        planeIslands[plane].Clear();

        //Clear plane land grid
        for (int i = 0; i < planeLandGrid[plane].GetLength(0); ++i) {
            for (int j = 0; j < planeLandGrid[plane].GetLength(1); ++j) {
                planeLandGrid[plane][i, j] = false;
            }
        }
    }

    public void switchIslandsOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        List<GameObject> tempFood = planeIslands[plane1];
        planeIslands[plane1] = planeIslands[plane2];
        planeIslands[plane2] = tempFood;

        bool[,] tempLandGrid = planeLandGrid[plane1];
        planeLandGrid[plane1] = planeLandGrid[plane2];
        planeLandGrid[plane2] = tempLandGrid;
    }

    public void resetGridsOnPlanes() {
        islandsSpawned[LocalPlanes.CURRENT_PLANE] = false;
        islandsSpawned[LocalPlanes.X_PLANE] = false;
        islandsSpawned[LocalPlanes.Z_PLANE] = false;
        islandsSpawned[LocalPlanes.XZ_PLANE] = false;

        foreach (KeyValuePair<LocalPlanes, bool[,]> entry in planeLandGrid) {
            for (int i = 0; i < entry.Value.GetLength(0); ++i) {
                for (int j = 0; j < entry.Value.GetLength(1); ++j) {
                    entry.Value[i, j] = false;
                }
            }
        }
    }

    private List<SerialIsland> computeIslandData(Vector3 planeCoord, Vector3 planeSize) {
        int leftX = Mathf.CeilToInt(planeCoord.x - planeSize.x / 2);
        int botZ = Mathf.CeilToInt(planeCoord.z - planeSize.z / 2);
        int rightX = Mathf.FloorToInt(planeCoord.x + planeSize.x / 2);
        int topZ = Mathf.FloorToInt(planeCoord.z + planeSize.z / 2);
        List<SerialIsland> serialIslands = new List<SerialIsland>();

        for (int i = leftX; i < rightX; i += gridPrecision) {
            for(int j = botZ; j < topZ; j += gridPrecision) {
                if(i != 0 && j != 0) {
                    //Ensures the generation is deterministic
                    Random.InitState(i + j);
                    float randomI = Random.Range(0, Mathf.Abs(i)) * 1f / Mathf.Abs(i);
                    float randomJ = Random.Range(0, Mathf.Abs(j)) * 1f / Mathf.Abs(j);

                    //Add new island on this condition + generate rest of its data
                    if (randomI + randomJ > creationThreshold) {
                        Random.InitState(2 * i + 2 * j);
                        int scale = Random.Range(5, scaleMax);
                        System.Guid islandId = System.Guid.NewGuid();
                        SerialIsland serialIsland = new SerialIsland(new Vector3(i, 0, j), scale, islandId);

                        serialIslands.Add(serialIsland);
                    }
                }
            }
        }

        return serialIslands;
    }

    private void markGridSpaceTakenByIsland(Vector3 islandCenter, int scale, LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
        int planeSizeXInGrid = (int)planeSize.x / gridPrecision;
        int planeSizeZInGrid = (int)planeSize.z / gridPrecision;
        int centerXInGrid = (int)(islandCenter.x - Mathf.CeilToInt(planeCoord.x - planeSize.x / 2))/gridPrecision;
        int centerZInGrid = (int)(islandCenter.z - Mathf.CeilToInt(planeCoord.z - planeSize.z / 2))/gridPrecision;
        float subjectiveScaleFactor = 1.5f;
        int leftXInGrid = Mathf.Max(centerXInGrid - scale / (int)(2 * subjectiveScaleFactor) , 0);
        int rightXInGrid = Mathf.Min(centerXInGrid + scale / (int)(2 * subjectiveScaleFactor), planeSizeXInGrid);
        int botZInGrid = Mathf.Max(centerZInGrid - scale / (int)(2 * subjectiveScaleFactor), 0);
        int topZInGrid = Mathf.Min(centerZInGrid + scale / (int)(2 * subjectiveScaleFactor), planeSizeZInGrid);

        for (int i = leftXInGrid; i < rightXInGrid; ++i) {
            for (int j = botZInGrid; j < topZInGrid; ++j) {
                planeLandGrid[plane][i, j] = true;
            }
        }
    }
}
