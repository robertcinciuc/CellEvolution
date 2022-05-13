using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuLogic : MonoBehaviour
{
    public static bool organIsDragged = false;

    private static GameObject player;
    private static GameObject playerCopy;
    private static GameObject upgradeMenuPlane;
    private static Vector3 safetyOffset = new Vector3(0, 5, 0);
    private static GameObject playerMouth;
    private static GameObject playerMouthClaw;
    private static GameObject playerFlagella;
    private static GameObject playerTwinFlagella;
    private static GameObject playerSpike;
    private static GameObject playerTooth;
    private static Vector3 displayOffset = new Vector3(0, 0.5f, 0);

    void Start(){
        player = GameObject.Find("Player");
        upgradeMenuPlane = this.gameObject.transform.parent.gameObject.transform.Find("UpgradeMenuPlane").gameObject;
    }

    void Update(){
        
    }

    private void FixedUpdate() {
        
    }
    public static void copyPlayerToUpgradeMenu() {

        //Protect original player with offsetting and immobilizing
        player.transform.position += safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //Render organs
        playerCopy = new GameObject();
        playerCopy.name = "PlayerCopy";
        playerCopy.transform.position = upgradeMenuPlane.transform.position + displayOffset;
        PlayerBodyStructure playerCopyBodyStructure = playerCopy.AddComponent<PlayerBodyStructure>();
        foreach(Transform child in player.transform) {
            Vector3 meshPos = upgradeMenuPlane.transform.position + child.transform.localPosition + displayOffset;

            Quaternion childModelRot = child.GetChild(0).transform.localRotation;
            playerCopyBodyStructure.addOrganFromMeshByType(child.GetChild(0).GetComponent<MeshRenderer>(), meshPos, childModelRot, child.name, child.gameObject.GetComponent<Organ>().organType);
        }
    }

    public static void copyPlayerToWorld() {
        Destroy(playerCopy);

        player.transform.position -= safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = true;
    }

    public static void instMenuOrgans() {
        Vector3 displayPosition = upgradeMenuPlane.transform.position + displayOffset;

        playerMouth = instMenuOrgan("Prefabs/Mouth", displayPosition + new Vector3(2, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.Mouth.ToString());
        playerMouthClaw = instMenuOrgan("Prefabs/MouthClaw", displayPosition + new Vector3(4, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.MouthClaw.ToString());
        playerFlagella = instMenuOrgan("Prefabs/Flagella", displayPosition + new Vector3(2, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.Flagella.ToString());
        playerTwinFlagella = instMenuOrgan("Prefabs/TwinFlagella", displayPosition + new Vector3(4, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.TwinFlagella.ToString());
        playerSpike = instMenuOrgan("Prefabs/Spike", displayPosition + new Vector3(2, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Spike.ToString());
        playerTooth = instMenuOrgan("Prefabs/Tooth", displayPosition + new Vector3(4, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Tooth.ToString());
    }

    public static void destroyMenuBodyParts() {
        Destroy(playerMouth);
        Destroy(playerMouthClaw);
        Destroy(playerFlagella);
        Destroy(playerTwinFlagella);
        Destroy(playerSpike);
        Destroy(playerTooth);
        Destroy(playerCopy);
    }

    private static GameObject instMenuOrgan(string prefabPath, Vector3 pos, Quaternion rot, System.Type organType, string organName) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        GameObject organModel = organ.transform.GetChild(0).gameObject;
        organ.name = organName;

        ClickableOrgan clickableOrgan = organModel.AddComponent<ClickableOrgan>();
        clickableOrgan.player = player;
        clickableOrgan.playerCopy = playerCopy;
        clickableOrgan.organType = organType;
        clickableOrgan.organ = organ;

        return organ;
    }

}
