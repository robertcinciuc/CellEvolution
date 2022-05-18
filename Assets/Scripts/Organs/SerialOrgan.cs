using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerialOrgan
{
    public float localPosX;
    public float localPosY;
    public float localPosZ;
    public float localRotW;
    public float localRotX;
    public float localRotY;
    public float localRotZ;
    public System.Type organType;
    public string prefabName;
    public System.Guid id;

    public SerialOrgan(GameObject parentOrgan) {
        localPosX = parentOrgan.transform.localPosition.x;
        localPosY = parentOrgan.transform.localPosition.y;
        localPosZ = parentOrgan.transform.localPosition.z;
        localRotW = parentOrgan.transform.localRotation.w;
        localRotX = parentOrgan.transform.localRotation.x;
        localRotY = parentOrgan.transform.localRotation.y;
        localRotZ = parentOrgan.transform.localRotation.z;
        organType = parentOrgan.transform.GetChild(0).GetComponent<Organ>().organType;
        prefabName = parentOrgan.transform.GetChild(0).GetComponent<Organ>().name;
        id = parentOrgan.transform.GetChild(0).GetComponent<Organ>().id;
    }
}
