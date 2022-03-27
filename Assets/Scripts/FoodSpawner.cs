using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public int nbFoodItemsPerPlane = 20;

    private WorldPlaneRenderer worldPlaneRenderer;
    private Dictionary<LocalPlanes, Vector3> allPlaneCoordinates;
    private HashSet<GameObject> currPlaneFoodItems;
    private HashSet<GameObject> xPlaneFoodItems;
    private HashSet<GameObject> zPlaneFoodItems;
    private HashSet<GameObject> xzPlaneFoodItems;
    private GameObject player;

    //TODO: Implement listener on world plane renderer - when the player crosses the lines to swich between food items
    void Start(){
        player = GameObject.Find("Player");
        worldPlaneRenderer = gameObject.GetComponent<WorldPlaneRenderer>();
        allPlaneCoordinates = worldPlaneRenderer.getAllPlaneCoordinates();

        for (int i = 0; i < nbFoodItemsPerPlane; i++) {
            float xPos = Random.Range(0, worldPlaneRenderer.planeSize.x/2);
            float zPos = Random.Range(0, worldPlaneRenderer.planeSize.z/2);
            List<int> signs = new List<int>(){ -1, 1 };
            int xSign = Random.Range(0, 2);
            int zSign = Random.Range(0, 2);

            Vector3 currPlaneCoord;
            allPlaneCoordinates.TryGetValue(LocalPlanes.CURRENT, out currPlaneCoord);

            Instantiate(foodPrefab, new Vector3(currPlaneCoord.x + signs[xSign] * xPos, 0, currPlaneCoord.z + signs[zSign] * zPos), Quaternion.identity);
        }
    }

    void Update(){
        
    }

    private void FixedUpdate() {

    }
}
