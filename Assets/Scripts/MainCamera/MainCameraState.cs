using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraState : MonoBehaviour
{
    private static Camera thisCamera;

    void Start(){
        thisCamera = GetComponent<Camera>();
    }

    void Update(){

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
