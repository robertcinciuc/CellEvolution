using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadUpgradeMenu : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public UpgradeCameraState upgradeMenuCameraState;

    private Button upgradeButton;

    void Start(){
        upgradeButton = GameObject.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(switchContext);

        MainCameraState.setActive();
        upgradeMenuCameraState.setInactive();
    }

    void Update(){

    }

    private void switchContext() {
        if (PlayerState.isActive) {
            PlayerState.setInactive();
            upgradeMenuCameraState.setActive();
            MainCameraState.setInactive();
            //upgradeManager.protectPlayer();
            upgradeManager.instMenuOrgans();

        } else {
            PlayerState.setActive();
            MainCameraState.setActive();
            upgradeMenuCameraState.setInactive();
            //upgradeManager.uprotectPlayer();
            upgradeManager.applyUpgrade();
            upgradeManager.destroyMenuBodyParts();
            upgradeManager.resetMovedAndAddedOrgans();
        }
    }
}
