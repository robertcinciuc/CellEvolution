using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void Start(){
        
    }

    void Update(){
        
    }

    private void OnCollisionEnter(Collision collision) {
        //print("coliision with " + collision.gameObject);

        if(collision.gameObject.name == Items.FOOD.ToString()) {
            Destroy(collision.gameObject);
        }
    }
}
