using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : BodyPart
{
    void Start(){
    }

    void Update(){
        
    }

    private void FixedUpdate() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == Items.FOOD.ToString()) {
            Destroy(collision.gameObject);
        }
    }
}
