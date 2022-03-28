using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2 : MonoBehaviour {
    public Camera mainCamera;
    public Camera upgradeMenuCamera;

    void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        upgradeMenuCamera = GameObject.Find("UpgradeMenuCamera").GetComponent<Camera>();

        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;

        upgradeMenuCamera.enabled = false;
        upgradeMenuCamera.GetComponent<AudioListener>().enabled = false;
    }

    void Update() {

    }

    public void enableUpgradeCamera() {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        upgradeMenuCamera.GetComponent<AudioListener>().enabled = true;

        upgradeMenuCamera.enabled = true;
        mainCamera.enabled = false;
    }

    public void enableMainCamera() {
        upgradeMenuCamera.GetComponent<AudioListener>().enabled = false;
        mainCamera.GetComponent<AudioListener>().enabled = true;

        mainCamera.enabled = true;
        upgradeMenuCamera.enabled = false;
    }
}
