using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedOrgan : MonoBehaviour
{
    public GameObject player;
    public GameObject playerFigure;
    public GameObject parentOrgan;
    public System.Type organType;

    void Start(){
        
    }

    void Update(){
            
    }

    private void OnMouseOver() {
        if (Input.GetKeyDown(KeyCode.R)) {
            playerFigure.GetComponent<PlayerBodyStructure>().removeOrgan(gameObject.GetComponent<Organ>().id);
            player.GetComponent<PlayerBodyStructure>().removeOrgan(gameObject.GetComponent<Organ>().id);
        }
    }

}
