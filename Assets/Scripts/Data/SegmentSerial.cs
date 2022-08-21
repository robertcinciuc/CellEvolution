using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SegmentSerial{

    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;
    public System.Guid segmentId;
    public string segmentName;
    public Dictionary<System.Guid, OrganSerial> organsSerial;
    public PlayerMovementSerial playerMovementSerial;
    public PlayerCollisionSerial playerCollisionSerial;
    public SegmentRigidbodySerial segmentRigidbodySerial;

    public SegmentSerial(GameObject segment) {
        organsSerial = new Dictionary<System.Guid, OrganSerial>();
        Segment segmentComponent = segment.GetComponent<Segment>();

        posX = segment.transform.localPosition.x;
        posY = segment.transform.localPosition.y;
        posZ = segment.transform.localPosition.z;
        rotX = segment.transform.localRotation.x;
        rotY = segment.transform.localRotation.y;
        rotZ = segment.transform.localRotation.z;
        rotW = segment.transform.localRotation.w;
        segmentName = segmentComponent.segmentName;
        segmentId = segmentComponent.segmentId;
        playerCollisionSerial = new PlayerCollisionSerial(segment.GetComponent<PlayerCollision>());
        segmentRigidbodySerial = new SegmentRigidbodySerial(segment.GetComponent<Rigidbody>());

        foreach(KeyValuePair<System.Guid, GameObject> entry in segmentComponent.getOrgans()) {
            OrganSerial organSerial = new OrganSerial(entry.Value);
            organsSerial.Add(entry.Key, organSerial);
        }

        //Player movement component if head segment
        if(segment.GetComponent<PlayerMovement>() != null) {
            playerMovementSerial = new PlayerMovementSerial(segment.GetComponent<PlayerMovement>());
        }
    }
}
