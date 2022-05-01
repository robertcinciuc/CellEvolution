using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuLogic : MonoBehaviour
{
    private static GameObject player;
    private static  GameObject upgradeMenuPlane;
    private static Vector3 ogPlayerPos;
    private static Quaternion ogPlayerRot;
    private static GameObject playerMouth;
    private static GameObject playerMouthClaw;
    private static GameObject playerFlagella;
    private static GameObject playerSpike;

    void Start(){
        player = GameObject.Find("Player");
        upgradeMenuPlane = this.gameObject.transform.parent.gameObject.transform.Find("UpgradeMenuPlane").gameObject;
    }

    void Update(){
        
    }

    private void FixedUpdate() {
        
    }
    public static void movePlayerToUpgradeMenu() {
        ogPlayerPos = player.transform.position;
        ogPlayerRot = player.transform.rotation;
        player.transform.position = upgradeMenuPlane.transform.position + new Vector3(0, 1, 0);
        player.transform.rotation = new Quaternion(0, 0, 0, 1);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().useGravity = false;

        instMenuOrgans();
    }

    public static void movePlayerToWorld() {
        player.transform.position = ogPlayerPos;
        player.transform.rotation = ogPlayerRot;
        ogPlayerPos = Vector3.zero;
        ogPlayerRot = Quaternion.identity;
        player.GetComponent<Rigidbody>().useGravity = true;

        destroyMenuBodyParts();
    }

    private static void instMenuOrgans() {
        playerMouth = instMenuOrgan("Prefabs/Mouth", new Vector3(2, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.Mouth.ToString());
        playerMouthClaw = instMenuOrgan("Prefabs/MouthClaw", new Vector3(4, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.MouthClaw.ToString());
        playerFlagella = instMenuOrgan("Prefabs/Flagella", new Vector3(2, 0, 0), new Quaternion(0.71f, 0, 0, 0.71f), typeof(LocomotionOrgans), LocomotionOrgans.Flagella.ToString());
        playerSpike = instMenuOrgan("Prefabs/Spike", new Vector3(2, 0, -2), new Quaternion(0.71f, 0, 0, 0.71f), typeof(AttackOrgans), AttackOrgans.Spike.ToString());
    }

    private static GameObject instMenuOrgan(string prefabPath, Vector3 relativePos, Quaternion rot, System.Type organType, string organName) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), player.transform.position + relativePos, rot);
        organ.name = organName;

        //Take the mesh collider from child for prefabs from ProBuilder
        if (organ.transform.childCount > 0) {
            MeshCollider meshCollider = organ.transform.GetChild(0).gameObject.GetComponent<MeshCollider>();
            MeshCollider meshCollider1 = organ.AddComponent<MeshCollider>();
            meshCollider1.sharedMesh = meshCollider.sharedMesh;
            meshCollider1.convex = true;
        }

        ClickableOrgan clickableOrgan = organ.AddComponent<ClickableOrgan>();
        clickableOrgan.player = player;
        clickableOrgan.organType = organType;

        return organ;
    }

    private static void destroyMenuBodyParts() {
        Destroy(playerMouth);
        Destroy(playerMouthClaw);
        Destroy(playerFlagella);
        Destroy(playerSpike);
    }

}
