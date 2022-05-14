using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerFigure;
    public System.Type organType;
    public GameObject organ;

    private bool rightClickPressedOnOrgan = false;
    private bool endDragable = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start(){
    }

    void Update(){
        detectEndRightClick();
        moveOrganToMouse();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
            playerFigure.GetComponent<PlayerBodyStructure>().setOrganByType(organType, organ);
        } else {
            if (Input.GetMouseButton(1) && !UpgradeMenuLogic.organIsDragged) {
                if (!rightClickPressedOnOrgan) {
                    //Save initial pos & rot
                    initialPosition = gameObject.transform.parent.transform.position;
                    initialRotation = gameObject.transform.parent.transform.rotation;

                    player.GetComponent<PlayerBodyStructure>().removeOrganByType(organType);
                    playerFigure.GetComponent<PlayerBodyStructure>().removeOrganByType(organType);
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
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            transform.parent.rotation = Quaternion.Slerp(transform.parent.transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endDragable) {
            //Put parent where child is, reset child local pos 
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            gameObject.transform.parent.transform.position = organ.transform.position;
            gameObject.transform.localPosition = Vector3.zero;

            //Update player structures
            GameObject playerFigureOrgan = playerFigure.GetComponent<PlayerBodyStructure>().addOrganByTypeWithPosition(organType, organ, deltaPos);
            player.GetComponent<PlayerBodyStructure>().addOrganByTypeWithPosition(organType, organ, deltaPos);

            //Add attached behaviour to attached organ
            AttachedOrgan attachedOrgan = playerFigureOrgan.transform.GetChild(0).gameObject.AddComponent<AttachedOrgan>();
            attachedOrgan.playerFigure = playerFigure;
            attachedOrgan.player = player;
            attachedOrgan.organ = playerFigureOrgan;
            attachedOrgan.organType = organType;

            //Reset object and parent to original display position
            gameObject.transform.parent.transform.position = initialPosition;
            gameObject.transform.parent.transform.rotation = initialRotation;

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
