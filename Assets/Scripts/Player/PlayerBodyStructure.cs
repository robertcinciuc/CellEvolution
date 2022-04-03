using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    private GameObject playerMouth;
    private Mouth playerRealMouth;

    void Start(){
        playerMouth = Instantiate((GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerMouth.transform.SetParent(this.gameObject.transform);
        playerMouth.transform.localPosition = new Vector3(0, 0, 1);
        playerMouth.transform.localRotation = Quaternion.identity;
    }

    void Update(){
        
    }

    void FixedUpdate() {
    }

}
