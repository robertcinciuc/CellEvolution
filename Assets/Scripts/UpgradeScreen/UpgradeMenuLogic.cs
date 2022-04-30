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
        player.transform.rotation = new Quaternion(1, 0, 0, 0);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        instMenuBodyParts();
    }

    public static void movePlayerToWorld() {
        player.transform.position = ogPlayerPos;
        player.transform.rotation = ogPlayerRot;
        ogPlayerPos = Vector3.zero;
        ogPlayerRot = Quaternion.identity;

        destroyMenuBodyParts();
    }

    private static void instMenuBodyParts() {
        playerMouth = Instantiate((GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject)), player.transform.position + new Vector3(2, 0, 2), Quaternion.identity);
        playerFlagella = Instantiate((GameObject)Resources.Load("Prefabs/Flagella", typeof(GameObject)), player.transform.position + new Vector3(2, 0, 0), new Quaternion(0.71f, 0, 0, 0.71f));
        playerSpike = Instantiate((GameObject)Resources.Load("Prefabs/Spike", typeof(GameObject)), player.transform.position + new Vector3(2, 0, -2), new Quaternion(0.71f, 0, 0, 0.71f));
    }

    private static void destroyMenuBodyParts() {
        Destroy(playerMouth);
        Destroy(playerFlagella);
        Destroy(playerSpike);
    }
}
