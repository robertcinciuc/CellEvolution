using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOrgan : MonoBehaviour
{
    public GameObject player;
    public System.Type organType;

    void Start(){
        
    }

    void Update(){
        
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerBodyStructure>().setOrganByType(organType, gameObject);
        }
    }
}
