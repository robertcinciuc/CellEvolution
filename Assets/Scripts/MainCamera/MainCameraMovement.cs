using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public GameObject player;

    private GameObject playerHead;
    private Vector3 cameraPosition;
    public int cameraYOffset;

    void Start(){
        playerHead = player.GetComponent<PlayerBodyStructure>().getHead();
    }

    void Update(){
        cameraPosition = playerHead.transform.position;
        cameraPosition.y += cameraYOffset;
        transform.position = cameraPosition;
    }
}
