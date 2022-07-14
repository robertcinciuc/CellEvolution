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
    Dictionary<System.Guid, OrganSerial> organs;

    public SegmentSerial(GameObject parentSegment) {
        posX = parentSegment.transform.localPosition.x;
        posY = parentSegment.transform.localPosition.y;
        posZ = parentSegment.transform.localPosition.z;
        rotW = parentSegment.transform.localRotation.w;
        rotX = parentSegment.transform.localRotation.x;
        rotY = parentSegment.transform.localRotation.y;
        rotZ = parentSegment.transform.localRotation.z;
        segmentName = parentSegment.GetComponent<Organ>().organName;
        segmentId = parentSegment.GetComponent<Organ>().id;

        //Morphology morphology = parentSegment.transform.parent.GetComponent<Morphology>();
        //TODO: get all organs
    }
}
