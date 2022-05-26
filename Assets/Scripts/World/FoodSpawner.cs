using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    public GameObject foodPrefab;
    public int nbFoodItemsPerPlane = 50;

    private Dictionary<LocalPlanes, bool> planeFedStatus;
    private Dictionary<LocalPlanes, List<GameObject>> planeFood;

    void Start(){
        initializeDictionaries();
    }

    void Update(){
        
    }

    private void FixedUpdate() {

    }
    public void spawnFoodItemsOnPlane(LocalPlanes plane, Vector3 planeCoord, Vector3 planeSize) {
        if(planeFedStatus[plane] == true) {
            return;
        }

        for (int i = 0; i < nbFoodItemsPerPlane; i++) {
            float xPos = Random.Range(0, planeSize.x / 2);
            float zPos = Random.Range(0, planeSize.z / 2);
            List<int> signs = new List<int>() { -1, 1 };
            int xSign = Random.Range(0, 2);
            int zSign = Random.Range(0, 2);

            GameObject foodItem = Instantiate(
                foodPrefab,
                new Vector3(planeCoord.x + signs[xSign] * xPos, 0, planeCoord.z + signs[zSign] * zPos),
                Quaternion.identity);

            foodItem.name = Foods.Meat.ToString();
            planeFood[plane].Add(foodItem);
            planeFedStatus[plane] = true;
        }
    }

    public void discardFoodOnPlane(LocalPlanes plane) {
        planeFedStatus[plane] = false;

        foreach(GameObject foodItem in planeFood[plane]) {
            Destroy(foodItem);
        }
    }

    public void switchFoodOnPlanes(LocalPlanes plane1, LocalPlanes plane2) {
        List<GameObject> tempFood = planeFood[plane1];
        planeFood[plane1] = planeFood[plane2];
        planeFood[plane2] = tempFood;
    }

    private void initializeDictionaries() {
        planeFedStatus = new Dictionary<LocalPlanes, bool>();
        planeFedStatus.Add(LocalPlanes.CURRENT_PLANE, false);
        planeFedStatus.Add(LocalPlanes.X_PLANE, false);
        planeFedStatus.Add(LocalPlanes.Z_PLANE, false);
        planeFedStatus.Add(LocalPlanes.XZ_PLANE, false);

        planeFood = new Dictionary<LocalPlanes, List<GameObject>>();
        planeFood.Add(LocalPlanes.CURRENT_PLANE, new List<GameObject>());
        planeFood.Add(LocalPlanes.X_PLANE, new List<GameObject>());
        planeFood.Add(LocalPlanes.Z_PLANE, new List<GameObject>());
        planeFood.Add(LocalPlanes.XZ_PLANE, new List<GameObject>());
    }
}
