using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerFigure;
    public GameObject parentOrgan;
    public System.Type organType;
    public Camera upgradeMenuCamera;
    public UpgradeMenuLogic upgradeMenuLogic;

    private bool clickPressedOnOrgan = false;
    private bool endMoveable = false;

    void Start(){
        
    }

    void Update(){
        detectEndClick();
        moveOrgan();
    }

    private void OnMouseOver() {
        if (Input.GetKeyDown(KeyCode.R)) {
            System.Guid organId = gameObject.GetComponent<Organ>().id;
            playerFigure.GetComponent<PlayerBodyStructure>().removeOrgan(organId);
            upgradeMenuLogic.putRemovedOrgan(organId);
        
        } else if(Input.GetMouseButton(0) && !UpgradeMenuLogic.attachedOrganIsDragged && !UpgradeMenuLogic.organIsDragged) {
            clickPressedOnOrgan = true;
            UpgradeMenuLogic.attachedOrganIsDragged = true;
        }
    }

    private void moveOrgan() {
        if (clickPressedOnOrgan && Input.GetMouseButton(0)) {
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.5f));
            Vector3 deltaPos = transform.position - playerFigure.transform.position;
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endMoveable) {
            upgradeMenuLogic.putMovedOrgan(GetComponent<Organ>().id, gameObject);
            endMoveable = false;
            clickPressedOnOrgan = false;
            UpgradeMenuLogic.attachedOrganIsDragged = false;
        }
    }

    private void detectEndClick() {
        if (clickPressedOnOrgan && !Input.GetMouseButton(0)) {
            endMoveable = true;
        }
    }

}
