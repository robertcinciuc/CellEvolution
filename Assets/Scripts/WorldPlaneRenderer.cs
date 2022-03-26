using UnityEngine;

public class WorldPlaneRenderer : MonoBehaviour
{

    public GameObject planePrefab;
    private GameObject currPlane;
    private GameObject vertPlane;
    private GameObject latPlane;
    private GameObject diagPlane;
    private GameObject player;
    private Vector3 currPlaneSize;

    void Start() {
        currPlane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        currPlaneSize = planePrefab.GetComponent<Renderer>().bounds.size;

        latPlane = Instantiate(planePrefab, new Vector3(currPlaneSize.x, 0, 0), Quaternion.identity);
        vertPlane = Instantiate(planePrefab, new Vector3(0, 0, currPlaneSize.z), Quaternion.identity);
        diagPlane = Instantiate(planePrefab, new Vector3(currPlaneSize.x, 0, currPlaneSize.z), Quaternion.identity);
        
        player = GameObject.Find("Player");
    }

    void Update() {

    }

    private void FixedUpdate() {

        renderLatPlane();

    }

    private void renderLatPlane() {
        if (player.transform.position.x > currPlane.transform.position.x) {

            if(player.transform.position.x < currPlaneSize.x/2) {
                destroyAndInstantiateLatPlane(1);
            } else {
                //TODO: Swap current plane with lateral plane (positive direction)
            }
        } else {
            if (player.transform.position.x > -currPlaneSize.x / 2) {
                destroyAndInstantiateLatPlane(-1);
            } else {
                //TODO: Swap current plane with lateral plane (negative direction)
            }
        }
    }

    //Sign : 1 = positive sign; -1 = negative sign
    private void destroyAndInstantiateLatPlane(int sign) {
        if (latPlane.transform.position.x != currPlane.transform.position.x + sign * currPlaneSize.x / 2) {
            Destroy(latPlane);
            latPlane = Instantiate(planePrefab, new Vector3(sign*currPlaneSize.x, 0, 0), Quaternion.identity);
        }
    }
}
