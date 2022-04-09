using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    private GameObject playerMouth;
    private GameObject playerFlagel;

    void Start(){
        playerMouth = Instantiate((GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerMouth.transform.SetParent(this.gameObject.transform);
        playerMouth.transform.localPosition = new Vector3(0, 0, 1);
        playerMouth.transform.localRotation = Quaternion.identity;
        
        playerFlagel = Instantiate((GameObject)Resources.Load("Prefabs/Flagel", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerFlagel.transform.SetParent(this.gameObject.transform);
        playerFlagel.transform.localPosition = new Vector3(0, -0.3f, -0.5f);
        playerFlagel.transform.localRotation = new Quaternion(0.71f, 0, 0, -0.71f);
    }

    void Update(){
        
    }

    void FixedUpdate() {
    }

}