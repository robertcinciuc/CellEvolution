using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 cameraPosition;
    public int cameraYOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition = player.transform.position;
        cameraPosition.y += cameraYOffset;
        transform.position = cameraPosition;
    }
}
