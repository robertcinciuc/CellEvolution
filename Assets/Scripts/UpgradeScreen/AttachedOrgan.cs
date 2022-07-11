using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedOrgan : MonoBehaviour
{
    public GameObject playerFigure;
    public GameObject parentSegment;
    public Camera upgradeMenuCamera;
    public UpgradeMenuLogic upgradeMenuLogic;
    public GameObject upgradeMenuPlane;

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
            System.Guid segmentId = parentSegment.GetComponent<Segment>().segmentId;
            System.Guid organId = gameObject.GetComponent<Organ>().id;

            playerFigure.GetComponent<PlayerBodyStructure>().removeOrgan(segmentId, organId);
            upgradeMenuLogic.putRemovedOrgan(segmentId, organId);
        
        } else if(Input.GetMouseButton(0) && !UpgradeMenuLogic.attachedOrganIsDragged && !UpgradeMenuLogic.organIsDragged) {
            clickPressedOnOrgan = true;
            UpgradeMenuLogic.attachedOrganIsDragged = true;
        }
    }

    private void moveOrgan() {
        PlayerBodyStructure figureBodyStructure = playerFigure.GetComponent<PlayerBodyStructure>();
        GameObject closestSegment = getClosestSegment(figureBodyStructure.getSegments());

        if (clickPressedOnOrgan && Input.GetMouseButton(0)) {
            float distCameraPlane = upgradeMenuCamera.transform.position.y - upgradeMenuPlane.transform.position.y;
            transform.position = upgradeMenuCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distCameraPlane));
            Vector3 deltaPos = transform.position - closestSegment.transform.position;
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPos), 50 * Time.deltaTime);
        }

        if (endMoveable) {
            System.Guid newSegmentId = closestSegment.GetComponent<Segment>().segmentId;

            System.Guid oldSegmentId = parentSegment.GetComponent<Segment>().segmentId;
            gameObject.transform.SetParent(closestSegment.transform);
            parentSegment = closestSegment;
            System.Guid organId = GetComponent<Organ>().id;
            System.Type organType = GetComponent<Organ>().organType;
            figureBodyStructure.removeOrganFromMapping(oldSegmentId, organId);
            figureBodyStructure.simpleAddOrganOnSegmentWithPos(closestSegment, organType, gameObject, organId);


            upgradeMenuLogic.putMovedOrgan(oldSegmentId, newSegmentId, organId, gameObject);
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

    private GameObject getClosestSegment(Dictionary<System.Guid, GameObject> segments) {
        float minDelta = 1000f;
        GameObject closestSegment = null;
        foreach (KeyValuePair<System.Guid, GameObject> entry in segments) {
            float deltaMagnitude = Mathf.Abs((entry.Value.transform.position - transform.position).magnitude);
            if (deltaMagnitude < minDelta) {
                minDelta = deltaMagnitude;
                closestSegment = entry.Value;
            }
        }

        return closestSegment;
    }

}
