using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 cameraPosition;
    public int cameraYOffset;

    void Start(){
        player = GameObject.Find("Player");
    }

    void Update(){
        if (player != null) {
            cameraPosition = player.transform.position;
            cameraPosition.y += cameraYOffset;
            transform.position = cameraPosition;
        }
    }
}
