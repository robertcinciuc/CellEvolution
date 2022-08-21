using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public GameObject player;

    private GameObject playerHead;
    private Vector3 cameraPosition;
    public int cameraYOffset;

    void Start(){
        playerHead = player.GetComponent<Morphology>().getHead();
    }

    void Update(){
        if (playerHead != null) {
            cameraPosition = playerHead.transform.position;
            cameraPosition.y += cameraYOffset;
            transform.position = cameraPosition;
        }
    }

    public void updateHead(GameObject playerHead) {
        this.playerHead = playerHead;
    }
}
