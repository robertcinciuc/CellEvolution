using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuLogic : MonoBehaviour
{
    private static GameObject player;
    private static  GameObject upgradeMenuPlane;
    private static Vector3 ogPlayerPos;
    private static Quaternion ogPlayerRot;

    void Start(){
        player = GameObject.Find("Player");
        //Transform[] transforms = player.GetComponentsInChildren<Transform>();
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
    }

    public static void movePlayerToWorld() {
        player.transform.position = ogPlayerPos;
        player.transform.rotation = ogPlayerRot;
        ogPlayerPos = Vector3.zero;
        ogPlayerRot = Quaternion.identity;
    }
}
