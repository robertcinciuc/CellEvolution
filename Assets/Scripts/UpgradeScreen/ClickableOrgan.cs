using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCopy;
    public System.Type organType;
    public GameObject organ;


    private Vector3 mousePosition;
    private float moveSpeed = 0.1f;

    void Start(){
        mousePosition = new Vector3(0, 0, 0);        
    }

    void Update(){
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
            playerCopy.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
        } else {
            if (Input.GetMouseButton(1)) {
                Camera camera1 = GameObject.Find("UpgradeMenuCamera").GetComponent<Camera>();
                mousePosition = camera1.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
            }
        }
    }

}
