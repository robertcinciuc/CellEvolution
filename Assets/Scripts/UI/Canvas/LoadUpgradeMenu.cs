using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadUpgradeMenu : MonoBehaviour
{
    private Button upgradeButton;

    void Start(){
        upgradeButton = GameObject.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(switchContext);

        MainCameraState.setActive();
        UpgradeMenuCameraState.setInactive();
    }

    void Update(){

    }

    private void switchContext() {
        if (PlayerState.isActive) {
            PlayerState.setInactive();
            UpgradeMenuCameraState.setActive();
            MainCameraState.setInactive();
            UpgradeMenuLogic.protectPlayer();
            UpgradeMenuLogic.renderPlayerFigure();
            UpgradeMenuLogic.instMenuOrgans();

        } else {
            PlayerState.setActive();
            MainCameraState.setActive();
            UpgradeMenuCameraState.setInactive();
            UpgradeMenuLogic.uprotectPlayer();
            UpgradeMenuLogic.destroyMenuBodyParts();
        }
    }
}
