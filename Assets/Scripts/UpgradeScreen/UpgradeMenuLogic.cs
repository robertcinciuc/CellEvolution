using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuLogic : MonoBehaviour
{
    public static bool organIsDragged = false;
    public static bool attachedOrganIsDragged = false;
    public static bool playerFigureInstantiated = false;
    public GameObject player;
    public Camera upgradeMenuCamera;
    public GameObject upgradeMenuPlane;

    private GameObject playerFigure;
    private Vector3 safetyOffset = new Vector3(0, 5, 0);
    private GameObject playerMouth;
    private GameObject playerMouthClaw;
    private GameObject playerFlagella;
    private GameObject playerTwinFlagella;
    private GameObject playerSpike;
    private GameObject playerTooth;
    private Vector3 displayOffset = new Vector3(0, 0.5f, 0);

    void Start(){
    }

    void Update(){
        
    }

    private void FixedUpdate() {
        
    }
    public void renderPlayerFigure() {
        if (playerFigure != null && playerFigure.GetComponent<PlayerBodyStructure>() != null) {
            playerFigure.GetComponent<PlayerBodyStructure>().removeAllOrgans();
            Destroy(playerFigure);
        }

        //Render organs
        playerFigure = new GameObject();
        playerFigure.name = "PlayerCopy";
        playerFigure.transform.position = upgradeMenuPlane.transform.position + displayOffset;
        PlayerBodyStructure playerCopyBodyStructure = playerFigure.AddComponent<PlayerBodyStructure>();
            
        foreach(Transform child in player.transform) {
            System.Type organType = child.GetComponent<Organ>().organType;
            System.Guid organId = child.GetComponent<Organ>().id;
            GameObject newOrgan = playerCopyBodyStructure.addOrganWithPos(organType, child.gameObject, organId);

            //Add attached organ behaviour
            AttachedOrgan attachedOrgan = newOrgan.gameObject.AddComponent<AttachedOrgan>();
            attachedOrgan.player = player;
            attachedOrgan.playerFigure = playerFigure;
            attachedOrgan.parentOrgan = child.gameObject;
            attachedOrgan.organType = child.GetComponent<Organ>().organType;
            attachedOrgan.upgradeMenuCamera = upgradeMenuCamera;
        }
    }

    public void protectPlayer() {
        //Protect original player with offsetting and immobilizing
        player.transform.position += safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void uprotectPlayer() {
        player.transform.position -= safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = true;
    }

    public void instMenuOrgans() {
        Vector3 displayPosition = upgradeMenuPlane.transform.position + displayOffset;

        playerMouth = instMenuOrgan("Prefabs/Mouth", displayPosition + new Vector3(2, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.Mouth.ToString());
        playerMouthClaw = instMenuOrgan("Prefabs/MouthClaw", displayPosition + new Vector3(4, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.MouthClaw.ToString());
        playerFlagella = instMenuOrgan("Prefabs/Flagella", displayPosition + new Vector3(2, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.Flagella.ToString());
        playerTwinFlagella = instMenuOrgan("Prefabs/TwinFlagella", displayPosition + new Vector3(4, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.TwinFlagella.ToString());
        playerSpike = instMenuOrgan("Prefabs/Spike", displayPosition + new Vector3(2, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Spike.ToString());
        playerTooth = instMenuOrgan("Prefabs/Tooth", displayPosition + new Vector3(4, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Tooth.ToString());
    }

    public void destroyMenuBodyParts() {
        Destroy(playerMouth);
        Destroy(playerMouthClaw);
        Destroy(playerFlagella);
        Destroy(playerTwinFlagella);
        Destroy(playerSpike);
        Destroy(playerTooth);
    }

    public GameObject instMenuOrgan(string prefabPath, Vector3 pos, Quaternion rot, System.Type organType, string organName) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.name = organName;

        //Add clickable organ component
        ClickableOrgan clickableOrgan = organ.AddComponent<ClickableOrgan>();
        clickableOrgan.player = player;
        clickableOrgan.playerFigure = playerFigure;
        clickableOrgan.organType = organType;
        clickableOrgan.organ = organ;
        clickableOrgan.upgradeMenuCamera = upgradeMenuCamera;
        clickableOrgan.upgradeMenuLogic = this;

        //Add organ component
        Organ organComponent = organ.gameObject.AddComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = System.Guid.NewGuid();
        organComponent.organName = organName;

        //Add serial organ to organ component
        SerialOrgan serialOrgan = new SerialOrgan(organ);
        organComponent.serialOrgan = serialOrgan;

        return organ;
    }

}
