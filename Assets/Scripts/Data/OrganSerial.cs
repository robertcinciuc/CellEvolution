using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class OrganSerial
{
    public float localPosX;
    public float localPosY;
    public float localPosZ;
    public float localRotW;
    public float localRotX;
    public float localRotY;
    public float localRotZ;
    public System.Type organType;
    public string organName;
    public System.Guid id;

    public OrganSerial(GameObject organ) {
        localPosX = organ.transform.localPosition.x;
        localPosY = organ.transform.localPosition.y;
        localPosZ = organ.transform.localPosition.z;
        localRotW = organ.transform.localRotation.w;
        localRotX = organ.transform.localRotation.x;
        localRotY = organ.transform.localRotation.y;
        localRotZ = organ.transform.localRotation.z;
        organType = organ.GetComponent<Organ>().organType;
        organName = organ.GetComponent<Organ>().organName;
        id = organ.GetComponent<Organ>().id;
    }
}
