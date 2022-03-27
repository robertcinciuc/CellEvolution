using UnityEngine;

public class WorldPlaneRenderer : MonoBehaviour
{

    public GameObject planePrefab;
    private GameObject currPlane;
    private GameObject zPlane;
    private GameObject xPlane;
    private GameObject xzPlane;
    private GameObject player;
    private Vector3 currPlaneSize;

    void Start() {
        currPlane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        currPlaneSize = planePrefab.GetComponent<Renderer>().bounds.size;

        xPlane = Instantiate(planePrefab, new Vector3(currPlaneSize.x, 0, 0), Quaternion.identity);
        zPlane = Instantiate(planePrefab, new Vector3(0, 0, currPlaneSize.z), Quaternion.identity);
        xzPlane = Instantiate(planePrefab, new Vector3(currPlaneSize.x, 0, currPlaneSize.z), Quaternion.identity);
        
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

        if(player.transform.position.x < currPlane.transform.position.x + currPlaneSize.x / 2 && 
           player.transform.position.x > currPlane.transform.position.x - currPlaneSize.x / 2) {
            
            if (xPlane.transform.position.x != currPlane.transform.position.x + sign * currPlaneSize.x) {
                Destroy(xPlane);
                xPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + sign * currPlaneSize.x, 0, currPlane.transform.position.z), Quaternion.identity);
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

        if (player.transform.position.z < currPlane.transform.position.z + currPlaneSize.z / 2 &&
            player.transform.position.z > currPlane.transform.position.z - currPlaneSize.z / 2) {
            
            if (xPlane.transform.position.z != currPlane.transform.position.z + sign * currPlaneSize.z) {
                Destroy(zPlane);
                zPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x, 0, currPlane.transform.position.z + sign * currPlaneSize.z), Quaternion.identity);
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

        if (player.transform.position.z < currPlane.transform.position.z + currPlaneSize.z / 2 &&
            player.transform.position.z > currPlane.transform.position.z - currPlaneSize.z / 2 &&
            player.transform.position.x < currPlane.transform.position.x + currPlaneSize.x / 2 &&
            player.transform.position.x > currPlane.transform.position.x - currPlaneSize.x / 2) {
            
            if (xzPlane.transform.position.z != (currPlane.transform.position.z + zSign * currPlaneSize.z) ||
                xzPlane.transform.position.x != (currPlane.transform.position.x + xSign * currPlaneSize.x)) {
                Destroy(xzPlane);
                xzPlane = Instantiate(planePrefab, new Vector3(currPlane.transform.position.x + xSign * currPlaneSize.x, 0, currPlane.transform.position.z + zSign * currPlaneSize.z), Quaternion.identity);
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

}
