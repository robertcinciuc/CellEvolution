using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class OrganSerial
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotW;
    public float rotX;
    public float rotY;
    public float rotZ;
    public System.Type organType;
    public string organName;
    public System.Guid id;

    public OrganSerial(GameObject organ) {
        posX = organ.transform.localPosition.x;
        posY = organ.transform.localPosition.y;
        posZ = organ.transform.localPosition.z;
        rotW = organ.transform.localRotation.w;
        rotX = organ.transform.localRotation.x;
        rotY = organ.transform.localRotation.y;
        rotZ = organ.transform.localRotation.z;
        organType = organ.GetComponent<Organ>().organType;
        organName = organ.GetComponent<Organ>().organName;
        id = organ.GetComponent<Organ>().id;
    }
}
