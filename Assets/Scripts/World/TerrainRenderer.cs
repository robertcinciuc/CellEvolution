using System.Collections.Generic;
using UnityEngine;

public class TerrainRenderer : MonoBehaviour {

    public GameObject planePrefab;
    public GameObject currPlane;
    public GameObject zPlane;
    public GameObject xPlane;
    public GameObject xzPlane;
    public Vector3 planeSize;

    private GameObject player;
    private FoodSpawner foodSpawner;
    private EnemySpawner enemySpawner;

    void Start() {
    }

    void Update() {

    }

    private void FixedUpdate() {
        if (player == null) {
            return;
        }

        renderXPlane();
        renderZPlane();
        renderXZPlane();
    }

    public GameObject getPlaneObject(LocalPlanes planeName) {
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

    public void initializeTerrain() {
        foodSpawner = this.gameObject.GetComponent<FoodSpawner>();
        enemySpawner = this.gameObject.GetComponent<EnemySpawner>();
        planeSize = planePrefab.GetComponent<Renderer>().bounds.size;
        player = GameObject.Find("Player");


        currPlane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        xPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, 0), Quaternion.identity);
        zPlane = Instantiate(planePrefab, new Vector3(0, 0, planeSize.z), Quaternion.identity);
        xzPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, planeSize.z), Quaternion.identity);
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
                discardElementsOnPlane(LocalPlanes.X_PLANE);
                xPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + sign * planeSize.x, 0, currPlane.transform.position.z), Quaternion.identity);
            }

        } else {
            GameObject tempPlane = currPlane;
            currPlane = xPlane;
            xPlane = tempPlane;
            tempPlane = zPlane;
            zPlane = xzPlane;
            xzPlane = tempPlane;

            switchElementsOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.X_PLANE);
            switchElementsOnPlanes(LocalPlanes.Z_PLANE, LocalPlanes.XZ_PLANE);
        }

        spawnElementsOnPlane(LocalPlanes.X_PLANE, xPlane.transform.position, planeSize);
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
                discardElementsOnPlane(LocalPlanes.Z_PLANE);
                zPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x, 0, currPlane.transform.position.z + sign * planeSize.z), Quaternion.identity);
            }

        } else {
            GameObject tempPlane = currPlane;
            currPlane = zPlane;
            zPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = xzPlane;
            xzPlane = tempPlane;

            switchElementsOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.Z_PLANE);
            switchElementsOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.XZ_PLANE);
        }

        spawnElementsOnPlane(LocalPlanes.Z_PLANE, zPlane.transform.position, planeSize);
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
                discardElementsOnPlane(LocalPlanes.XZ_PLANE);
                xzPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + xSign * planeSize.x, 0, currPlane.transform.position.z + zSign * planeSize.z), Quaternion.identity);
            }

        } else {
            GameObject tempPlane = currPlane;
            currPlane = xzPlane;
            xzPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = zPlane;
            zPlane = tempPlane;

            switchElementsOnPlanes(LocalPlanes.CURRENT_PLANE, LocalPlanes.XZ_PLANE);
            switchElementsOnPlanes(LocalPlanes.X_PLANE, LocalPlanes.Z_PLANE);
        }

        spawnElementsOnPlane(LocalPlanes.XZ_PLANE, xzPlane.transform.position, planeSize);
    }

    private void switchElementsOnPlanes(LocalPlanes plane1, LocalPlanes plane2){
        //Switch food on planes
        foodSpawner.switchFoodOnPlanes(plane1, plane2);

        //Switch enemies on planes
        enemySpawner.switchEnemiesOnPlanes(plane1, plane2);
    }

    private void spawnElementsOnPlane(LocalPlanes plane, Vector3 planePos, Vector3 planeSize) {
        //Spawn food
        foodSpawner.spawnFoodItemsOnPlane(plane, planePos, planeSize);

        //Spawn enemies
        enemySpawner.spawnEnemiesOnPlane(plane, planePos, planeSize);
    }

    private void discardElementsOnPlane(LocalPlanes plane) {
        //Discard food
        foodSpawner.discardFoodOnPlane(plane);
        
        //Discard enemies
        enemySpawner.discardEnemiesOnPlane(plane);
    }
}
