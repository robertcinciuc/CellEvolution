using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Collider thisCollider;

    void Start(){
        thisCollider = GetComponent<Collider>();
    }

    void Update(){
    }

    private void OnCollisionEnter(Collision collision) {
        //print("collision with " + collision.gameObject);
        Collider firstCollider = collision.GetContact(0).thisCollider;

        if ( firstCollider.name == "Mouth" && collision.gameObject.name == Items.FOOD.ToString()) {
            Destroy(collision.gameObject);
        }
    }
}
