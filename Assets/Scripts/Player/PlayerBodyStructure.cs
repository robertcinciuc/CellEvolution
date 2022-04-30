using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    private GameObject playerMouth;
    private GameObject playerFlagella;
    private GameObject playerSpike;

    void Start(){
        playerMouth = Instantiate((GameObject)Resources.Load("Prefabs/Mouth", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerMouth.transform.SetParent(this.gameObject.transform);
        playerMouth.transform.localPosition = new Vector3(0, 0, 1);
        playerMouth.transform.localRotation = Quaternion.identity;
        playerMouth.name = Mouths.Mouth.ToString();
        
        playerFlagella = Instantiate((GameObject)Resources.Load("Prefabs/Flagella", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerFlagella.transform.SetParent(this.gameObject.transform);
        playerFlagella.transform.localPosition = new Vector3(0, -0.3f, -0.5f);
        playerFlagella.transform.localRotation = new Quaternion(0.71f, 0, 0, -0.71f);
        
        playerSpike = Instantiate((GameObject)Resources.Load("Prefabs/Spike", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
        playerSpike.transform.SetParent(this.gameObject.transform);
        playerSpike.transform.localPosition = new Vector3(-0.5f, 0.3f, 0.5f);
        playerSpike.transform.localRotation = new Quaternion(0, 0, 0.71f, 0.71f);

    }

    void Update(){
        
    }

    void FixedUpdate() {
    }

}
