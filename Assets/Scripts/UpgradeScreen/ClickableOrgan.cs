using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerFigure;
    public System.Type organType;
    public GameObject organ;
    public Camera upgradeMenuCamera;
    public UpgradeMenuLogic upgradeMenuLogic;

    private bool clickPressedOnOrgan = false;
    private bool endDragable = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start(){
    }

    void Update(){
        detectEndClick();
        moveOrganToMouse();
    }

    void OnMouseOver() {
        if (Input.GetMouseButton(0) && !UpgradeMenuLogic.organIsDragged && !UpgradeMenuLogic.attachedOrganIsDragged) {
            if (!clickPressedOnOrgan) {
                //Save initial pos & rot
                initialPosition = transform.position;
                initialRotation = transform.rotation;
            }

            clickPressedOnOrgan = true;
            UpgradeMenuLogic.organIsDragged = true;
        }
    }

    private void moveOrganToMouse() {
        if (clickPressedOnOrgan & Input.GetMouseButton(0)) {
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.5f));
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endDragable) {
            //Get delta pos & rot of clicked organ
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            Quaternion deltaRot = organ.transform.rotation;

            //Update player structures
            System.Guid organId = System.Guid.NewGuid();
            GameObject playerFigureOrgan = playerFigure.GetComponent<PlayerBodyStructure>().addOrganWithPosition(organType, organ, deltaPos, deltaRot, organId);
            player.GetComponent<PlayerBodyStructure>().addOrganWithPosition(organType, organ, deltaPos, deltaRot, organId);

            //Add attached behaviour to attached organ
            AttachedOrgan attachedOrgan = playerFigureOrgan.AddComponent<AttachedOrgan>();
            attachedOrgan.playerFigure = playerFigure;
            attachedOrgan.player = player;
            attachedOrgan.parentOrgan = playerFigureOrgan;
            attachedOrgan.organType = organType;
            attachedOrgan.upgradeMenuCamera = upgradeMenuCamera;

            //Create new organ at the initial spot
            string organName = organ.GetComponent<Organ>().organName;
            upgradeMenuLogic.instMenuOrgan("Prefabs/" + organName, initialPosition, initialRotation, organType, organName);


            endDragable = false;
            clickPressedOnOrgan = false;
            UpgradeMenuLogic.organIsDragged = false;

            Destroy(gameObject);
        }
    }

    private void detectEndClick() {
        if (clickPressedOnOrgan && !Input.GetMouseButton(0)) {
            endDragable = true;
        }
    }
}
