using UnityEngine;

public class WorldPlaneRenderer : MonoBehaviour
{

    public GameObject currentPlanePrefab;
    public GameObject verticalPlanePrefab;
    public GameObject lateralPlanePrefab;
    public GameObject diagonalPlanePrefab;
    private bool isVerticalPlaneInstantiated = false;
    private bool isLateralPlaneInstantiated = false;
    private bool isDiagonalPlaneInstantiated = false;
    private GameObject player;


    // Start is called before the first frame update
    void Start() {
        Instantiate(currentPlanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        Vector3 currentPlaneSize = currentPlanePrefab.GetComponent<Renderer>().bounds.size;

        if (player.transform.position.x == currentPlanePrefab.transform.position.x + currentPlaneSize.x / 2 * 0.8) {
            Instantiate(lateralPlanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        
        if (player.transform.position.x > 3 && !isLateralPlaneInstantiated) {
            isLateralPlaneInstantiated = true;
            Instantiate(lateralPlanePrefab, new Vector3(currentPlaneSize.x, 0, 0), Quaternion.identity);
        }
    }
}
