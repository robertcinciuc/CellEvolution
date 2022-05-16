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
            playerFigure.GetComponent<PlayerBodyStructure>().removeOrgan(gameObject.GetComponent<Organ>().id);
            player.GetComponent<PlayerBodyStructure>().removeOrgan(gameObject.GetComponent<Organ>().id);
        
        } else if(Input.GetMouseButton(0) && !UpgradeMenuLogic.attachedOrganIsDragged && !UpgradeMenuLogic.organIsDragged) {
            clickPressedOnOrgan = true;
            UpgradeMenuLogic.attachedOrganIsDragged = true;
        }
    }

    private void moveOrgan() {
        if (clickPressedOnOrgan && Input.GetMouseButton(0)) {
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.5f));
            Vector3 deltaPos = gameObject.transform.position - playerFigure.transform.position;
            transform.parent.rotation = Quaternion.Slerp(transform.parent.transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endMoveable) {
            //Put parent where child is, reset child local pos 
            gameObject.transform.parent.transform.position = transform.position;
            gameObject.transform.localPosition = Vector3.zero;

            player.GetComponent<PlayerBodyStructure>().moveOrgan(gameObject.GetComponent<Organ>().id, transform.parent.localPosition, transform.parent.rotation);
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
