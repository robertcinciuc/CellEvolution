using System.Collections.Generic;
using UnityEngine;

public class WorldPlaneRenderer : MonoBehaviour
{

    public GameObject planePrefab;

    public static GameObject currPlane;
    public static GameObject zPlane;
    public static GameObject xPlane;
    public static GameObject xzPlane;
    public static Vector3 planeSize;
    private GameObject player;
    private FoodSpawner foodSpawner;
    private bool startFoodCurrPlane = false;
    private bool startEnemyCurrPlane = false;


    void Start() {
        foodSpawner = this.gameObject.GetComponent<FoodSpawner>();
        planeSize = planePrefab.GetComponent<Renderer>().bounds.size;
        player = GameObject.Find("Player");


        currPlane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        xPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, 0), Quaternion.identity);
        zPlane = Instantiate(planePrefab, new Vector3(0, 0, planeSize.z), Quaternion.identity);
        xzPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, planeSize.z), Quaternion.identity);

    }

    void Update() {

    }

    private void FixedUpdate() {
        if (!startFoodCurrPlane) {
            foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.CURRENT_PLANE, currPlane.transform.position, planeSize);
        }

        if (!startEnemyCurrPlane) { 
            EnemySpawner.spawnEnemiesOnPlane(LocalPlanes.CURRENT_PLANE, currPlane.transform.position, planeSize);
        }

        if (player == null) {
            return;
        }

        renderXPlane();
        renderZPlane();
        renderXZPlane();
    }

    public static GameObject getPlaneObject(LocalPlanes planeName) {
        if (planeName == LocalPlanes.CURRENT_PLANE) {
            return currPlane;
        }
        if (planeName == LocalPlanes.X_PLANE) {
            return xPlane;
        }
        if (planeName == LocalPlanes.Z_PLANE) {
            return zPlane;
        }

        return xzPlane;
    }

    private void renderXPlane() {
        int sign = 1;
        if (player.transform.position.x < currPlane.transform.position.x) {
            sign = -1;
        }

        if(player.transform.position.x < currPlane.transform.position.x + planeSize.x / 2 && 
           player.transform.position.x > currPlane.transform.position.x - planeSize.x / 2) {
            
            if (xPlane.transform.position.x != currPlane.transform.position.x + sign * planeSize.x) {
                Destroy(xPlane);
                foodSpawner.discardFoodOnPlane(LocalPlanes.X_PLANE);
                EnemySpawner.discardEnemiesOnPlane(LocalPlanes.X_PLANE);
                xPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + sign * planeSize.x, 0, currPlane.transform.position.z), Quaternion.identity);
            }
        } else {

            GameObject tempPlane = currPlane;
            currPlane = xPlane;
            xPlane = tempPlane;
            tempPlane = zPlane;
            zPlane = xzPlane;
            xzPlane = tempPlane;

            //Switch food on planes
            foodSpawner.switchFoodOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.X_PLANE);
            foodSpawner.switchFoodOnPlanes(LocalPlanes.Z_PLANE, LocalPlanes.XZ_PLANE);
            
            //Switch enemies on planes
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.X_PLANE);
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.Z_PLANE, LocalPlanes.XZ_PLANE);
        }

        //Spawn food
        foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.X_PLANE, xPlane.transform.position, planeSize);

        //Spawn enemies
        EnemySpawner.spawnEnemiesOnPlane(LocalPlanes.X_PLANE, xPlane.transform.position, planeSize);
    }

    private void renderZPlane() {
        int sign = 1;
        if (player.transform.position.z < currPlane.transform.position.z) {
            sign = -1;
        }

        if (player.transform.position.z < currPlane.transform.position.z + planeSize.z / 2 &&
            player.transform.position.z > currPlane.transform.position.z - planeSize.z / 2) {
            
            if (zPlane.transform.position.z != currPlane.transform.position.z + sign * planeSize.z) {
                Destroy(zPlane);
                foodSpawner.discardFoodOnPlane(LocalPlanes.Z_PLANE);
                EnemySpawner.discardEnemiesOnPlane(LocalPlanes.Z_PLANE);
                zPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x, 0, currPlane.transform.position.z + sign * planeSize.z), Quaternion.identity);
            }
        } else {

            GameObject tempPlane = currPlane;
            currPlane = zPlane;
            zPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = xzPlane;
            xzPlane = tempPlane;

            //Switch food on planes
            foodSpawner.switchFoodOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.Z_PLANE);
            foodSpawner.switchFoodOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.XZ_PLANE);
            
            //Switch enemies on planes
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.Z_PLANE);
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.XZ_PLANE);
        }

        //Spawn food
        foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.Z_PLANE, zPlane.transform.position, planeSize);
        
        //Spawn enemies
        EnemySpawner.spawnEnemiesOnPlane(LocalPlanes.Z_PLANE, zPlane.transform.position, planeSize);
    }
    
    private void renderXZPlane() {
        int xSign = 1;
        int zSign = 1;

        if (player.transform.position.z < currPlane.transform.position.z) {
            zSign = -1;
        }
        if (player.transform.position.x < currPlane.transform.position.x) {
            xSign = -1;
        }

        if (player.transform.position.z < currPlane.transform.position.z + planeSize.z / 2 &&
            player.transform.position.z > currPlane.transform.position.z - planeSize.z / 2 &&
            player.transform.position.x < currPlane.transform.position.x + planeSize.x / 2 &&
            player.transform.position.x > currPlane.transform.position.x - planeSize.x / 2) {
            
            if (xzPlane.transform.position.z != (currPlane.transform.position.z + zSign * planeSize.z) ||
                xzPlane.transform.position.x != (currPlane.transform.position.x + xSign * planeSize.x)) {
                Destroy(xzPlane);
                foodSpawner.discardFoodOnPlane(LocalPlanes.XZ_PLANE);
                EnemySpawner.discardEnemiesOnPlane(LocalPlanes.XZ_PLANE);
                xzPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + xSign * planeSize.x, 0, currPlane.transform.position.z + zSign * planeSize.z), Quaternion.identity);
            }
        } else {
            GameObject tempPlane = currPlane;
            currPlane = xzPlane;
            xzPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = zPlane;
            zPlane = tempPlane;

            //Switch food on planes
            foodSpawner.switchFoodOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.XZ_PLANE);
            foodSpawner.switchFoodOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.Z_PLANE);

            //Switch food on planes
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.XZ_PLANE);
            EnemySpawner.switchEnemiesOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.Z_PLANE);
        }


        //Spawn food
        foodSpawner.spawnFoodItemsOnPlane(LocalPlanes.XZ_PLANE, xzPlane.transform.position, planeSize);
    
        //Spawn enemies
        EnemySpawner.spawnEnemiesOnPlane(LocalPlanes.XZ_PLANE, xzPlane.transform.position, planeSize);
    }
}
