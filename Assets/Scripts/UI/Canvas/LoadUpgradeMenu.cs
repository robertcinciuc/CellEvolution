using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadUpgradeMenu : MonoBehaviour
{
    public UpgradeMenuLogic upgradeMenuLogic;
    public UpgradeMenuCameraState upgradeMenuCameraState;

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
            upgradeMenuLogic.protectPlayer();
            upgradeMenuLogic.instMenuOrgans();

        } else {
            PlayerState.setActive();
            MainCameraState.setActive();
            upgradeMenuCameraState.setInactive();
            upgradeMenuLogic.uprotectPlayer();
            upgradeMenuLogic.applyUpgrade();
            upgradeMenuLogic.destroyMenuBodyParts();
            upgradeMenuLogic.resetMovedAndAddedOrgans();
        }
    }
}
