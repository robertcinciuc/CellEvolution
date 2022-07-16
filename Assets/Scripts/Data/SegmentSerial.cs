using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SegmentSerial{

    public float posX;
    public float posY;
    public float posZ;
    public float rotW;
    public float rotX;
    public float rotY;
    public float rotZ;
    public System.Guid segmentId;
    public string segmentName;
    public Dictionary<System.Guid, OrganSerial> organsSerial;
    public List<System.Type> classicComponents;
    public PlayerMovementSerial playerMovementSerial;
    public PlayerCollisionSerial playerCollisionSerial;

    public SegmentSerial(GameObject parentSegment) {
        organsSerial = new Dictionary<System.Guid, OrganSerial>();
        classicComponents = new List<System.Type>();
        Segment segmentComponent = parentSegment.GetComponent<Segment>();

        posX = parentSegment.transform.localPosition.x;
        posY = parentSegment.transform.localPosition.y;
        posZ = parentSegment.transform.localPosition.z;
        rotW = parentSegment.transform.localRotation.w;
        rotX = parentSegment.transform.localRotation.x;
        rotY = parentSegment.transform.localRotation.y;
        rotZ = parentSegment.transform.localRotation.z;
        segmentName = segmentComponent.segmentName;
        segmentId = segmentComponent.segmentId;
        playerCollisionSerial = new PlayerCollisionSerial(parentSegment.GetComponent<PlayerCollision>());

        foreach(KeyValuePair<System.Guid, GameObject> entry in segmentComponent.getOrgans()) {
            OrganSerial organSerial = new OrganSerial(entry.Value);
            organsSerial.Add(entry.Key, organSerial);
        }

        //Player movement component if head segment
        if(parentSegment.GetComponent<PlayerMovement>() != null) {
            playerMovementSerial = new PlayerMovementSerial(parentSegment.GetComponent<PlayerMovement>());
        }

        //Add classic components
        classicComponents.Add(parentSegment.GetComponent<Rigidbody>().GetType());
    }
}
