using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCopy;
    public System.Type organType;
    public GameObject organ;

    void Start(){
    }

    void Update(){
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
            playerCopy.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
        } else {
            if (Input.GetMouseButton(1)) {
                Camera upgradeMenuCamera = GameObject.Find("UpgradeMenuCamera").GetComponent<Camera>();
                transform.position = upgradeMenuCamera.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.5f));
            }
        }
    }

}
