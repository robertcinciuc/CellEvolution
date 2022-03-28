using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadUpgradeMenu : MonoBehaviour
{
    private Button upgradeButton;
    private CameraManager2 cameraManager;

    void Start(){
        upgradeButton = GameObject.Find("UpgradeButton").GetComponent<Button>();
        //cameraManager = gameObject.GetComponent<CameraManager2>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager2>();
    }

    void Update(){
        if (upgradeButton != null) {
            upgradeButton.onClick.AddListener(goToUpgradeScene);
        }
    }

    private void goToUpgradeScene() {
        PlayerState.setInactive();
        cameraManager.enableUpgradeCamera();
    }
}
