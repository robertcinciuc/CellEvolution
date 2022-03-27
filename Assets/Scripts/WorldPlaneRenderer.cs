using System.Collections.Generic;
using UnityEngine;

public class WorldPlaneRenderer : MonoBehaviour
{

    public GameObject planePrefab;
    public Vector3 planeSize;

    private GameObject currPlane;
    private GameObject zPlane;
    private GameObject xPlane;
    private GameObject xzPlane;
    private GameObject player;

    void Start() {
        currPlane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        planeSize = planePrefab.GetComponent<Renderer>().bounds.size;

        xPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, 0), Quaternion.identity);
        zPlane = Instantiate(planePrefab, new Vector3(0, 0, planeSize.z), Quaternion.identity);
        xzPlane = Instantiate(planePrefab, new Vector3(planeSize.x, 0, planeSize.z), Quaternion.identity);
        
        player = GameObject.Find("Player");
    }

    void Update() {

    }

    private void FixedUpdate() {
        renderXPlane();
        renderZPlane();
        renderXZPlane();
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
                xPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + sign * planeSize.x, 0, currPlane.transform.position.z), Quaternion.identity);
            }
        } else {

            GameObject tempPlane = currPlane;
            currPlane = xPlane;
            xPlane = tempPlane;
            tempPlane = zPlane;
            zPlane = xzPlane;
            xzPlane = tempPlane;
        }
    }
    
    private void renderZPlane() {
        int sign = 1;
        if (player.transform.position.z < currPlane.transform.position.z) {
            sign = -1;
        }

        if (player.transform.position.z < currPlane.transform.position.z + planeSize.z / 2 &&
            player.transform.position.z > currPlane.transform.position.z - planeSize.z / 2) {
            
            if (xPlane.transform.position.z != currPlane.transform.position.z + sign * planeSize.z) {
                Destroy(zPlane);
                zPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x, 0, currPlane.transform.position.z + sign * planeSize.z), Quaternion.identity);
            }
        } else {

            GameObject tempPlane = currPlane;
            currPlane = zPlane;
            zPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = xzPlane;
            xzPlane = tempPlane;
        }
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
                xzPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + xSign * planeSize.x, 0, currPlane.transform.position.z + zSign * planeSize.z), Quaternion.identity);
            }
        } else {
            GameObject tempPlane = currPlane;
            currPlane = xzPlane;
            xzPlane = tempPlane;
            tempPlane = xPlane;
            xPlane = zPlane;
            zPlane = tempPlane;
        }

    }
    public Dictionary<LocalPlanes, Vector3> getAllPlaneCoordinates() {
        Dictionary<LocalPlanes, Vector3> coordinates = new Dictionary<LocalPlanes, Vector3>();
        coordinates.Add(LocalPlanes.CURRENT, currPlane.transform.position);
        coordinates.Add(LocalPlanes.X_PLANE, xPlane.transform.position);
        coordinates.Add(LocalPlanes.Z_PLANE, zPlane.transform.position);
        coordinates.Add(LocalPlanes.XZ_PLANE, xzPlane.transform.position);

        return coordinates;
    }

}
