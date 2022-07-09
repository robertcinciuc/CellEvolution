using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject playerFigure;
    public System.Type organType;
    public GameObject organ;
    public Camera upgradeMenuCamera;
    public UpgradeMenuLogic upgradeMenuLogic;
    public Organ organComponent;
    public GameObject upgradeMenuPlane;

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
            float distCameraPlane = upgradeMenuCamera.transform.position.y - upgradeMenuPlane.transform.position.y;
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCameraPlane));
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endDragable) {
            organ.transform.SetParent(playerFigure.transform);

            //Update player structures
            System.Guid organId = organComponent.id;
            GameObject playerFigureOrgan = playerFigure.GetComponent<PlayerBodyStructure>().simpleAddOrganWithPos(organType, organ, organId);

            //Add attached behaviour to attached organ
            AttachedOrgan attachedOrgan = playerFigureOrgan.AddComponent<AttachedOrgan>();
            attachedOrgan.playerFigure = playerFigure;
            attachedOrgan.parentOrgan = playerFigureOrgan;
            attachedOrgan.upgradeMenuCamera = upgradeMenuCamera;
            attachedOrgan.upgradeMenuLogic = upgradeMenuLogic;
            attachedOrgan.upgradeMenuPlane = upgradeMenuPlane;

            //Update upgradeMenu with new organ state
            upgradeMenuLogic.putAddedOrgan(organId, organ);
            upgradeMenuLogic.removeFromDisplayMap(organId);

            //Create new organ at the initial spot
            string organName = organComponent.organName;
            upgradeMenuLogic.instMenuOrgan("Prefabs/" + organName, initialPosition, initialRotation, organType, organName);

            endDragable = false;
            clickPressedOnOrgan = false;
            UpgradeMenuLogic.organIsDragged = false;
        }
    }

    private void detectEndClick() {
        if (clickPressedOnOrgan && !Input.GetMouseButton(0)) {
            endDragable = true;
        }
    }
}
