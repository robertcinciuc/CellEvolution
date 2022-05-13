using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCopy;
    public System.Type organType;
    public GameObject organ;

    private bool rightClickPressedOnOrgan = false;
    private bool endDragable = false;
    private Vector3 initialPosition;

    void Start(){
    }

    void Update(){
        detectEndRightClick();
        moveOrganToMouse();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
            playerCopy.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
        } else {
            if (Input.GetMouseButton(1) && !UpgradeMenuLogic.organIsDragged) {
                if (!rightClickPressedOnOrgan) {
                    initialPosition = gameObject.transform.position;
                    player.GetComponent<PlayerBodyStructure>().removeOrganByType(organType);
                    playerCopy.GetComponent<PlayerBodyStructure>().removeOrganByType(organType);
                }

                rightClickPressedOnOrgan = true;
                UpgradeMenuLogic.organIsDragged = true;
            }
        }
    }

    private void moveOrganToMouse() {
        if (rightClickPressedOnOrgan & Input.GetMouseButton(1)) {
            Camera upgradeMenuCamera = GameObject.Find("UpgradeMenuCamera").GetComponent<Camera>();
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.5f));
        }

        if (endDragable) {
            //Get delta, put parent where child is, reset child local pos 
            Vector3 deltaPos = gameObject.transform.position - playerCopy.transform.position;
            gameObject.transform.parent.transform.position = organ.transform.position;
            gameObject.transform.localPosition = Vector3.zero;

            //Update player structures
            playerCopy.GetComponent<PlayerBodyStructure>().addOrganByTypeWithPosition(organType, organ, deltaPos);
            player.GetComponent<PlayerBodyStructure>().addOrganByTypeWithPosition(organType, organ, deltaPos);

            //Reset object and parent to original display position
            gameObject.transform.position = initialPosition;
            gameObject.transform.parent.transform.position = initialPosition;

            endDragable = false;
            rightClickPressedOnOrgan = false;
            UpgradeMenuLogic.organIsDragged = false;
        }
    }

    private void detectEndRightClick() {
        if (rightClickPressedOnOrgan && !Input.GetMouseButton(1)) {
            endDragable = true;
        }
    }
}
