using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadUpgradeMenu : MonoBehaviour
{
    private Button upgradeButton;
    private int count = 0;

    void Start(){
        //cameraManager = gameObject.GetComponent<CameraManager2>();

        upgradeButton = GameObject.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(switchCamera);

        MainCameraState.setActive();
        UpgradeMenuCameraState.setInactive();

    }

    void Update(){

    }

    private void switchCamera() {
        count++;

        if (PlayerState.isActive) {
            PlayerState.setInactive();
            UpgradeMenuCameraState.setActive();
            MainCameraState.setInactive();
        } else {
            PlayerState.setActive();
            MainCameraState.setActive();
            UpgradeMenuCameraState.setInactive();
        }
    }
}
