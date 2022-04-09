using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuCameraState : MonoBehaviour
{
    public float cameraYOffset = 60; 

    private Vector3 cameraPosition;
    private static Camera thisCamera;

    void Start() {
        thisCamera = GetComponent<Camera>();
    }

    void Update() {
        cameraPosition = transform.position;
        cameraPosition.y = cameraYOffset;
        transform.position = cameraPosition;
    }

    public static void setActive() {
        thisCamera.GetComponent<AudioListener>().enabled = true;
        thisCamera.enabled = true;
    }

    public static void setInactive() {
        thisCamera.GetComponent<AudioListener>().enabled = false;
        thisCamera.enabled = false;
    }
}