using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Collider thisCollider;
    private PlayerState playerState;

    void Start(){
        thisCollider = GetComponent<Collider>();
        playerState = GetComponent<PlayerState>();
    }

    void Update(){
    }

    private void OnCollisionEnter(Collision collision) {
        Collider firstCollider = collision.GetContact(0).thisCollider;

        if ( System.Enum.IsDefined(typeof(Mouths), firstCollider.name) && collision.gameObject.name == Items.FOOD.ToString()) {
            Destroy(collision.gameObject);
            playerState.heal(10);
        }else if (firstCollider.name.Contains(BodyPartTypes.Spike.ToString()) && System.Enum.IsDefined(typeof(Enemies), collision.gameObject.name)) {
            collision.gameObject.GetComponent<EnemyState>().takeDamage(30);
        }else if (collision.GetContact(0).otherCollider.name.Contains(BodyPartTypes.Spike.ToString()) && !firstCollider.name.Contains(BodyPartTypes.Spike.ToString()) ) {
            playerState.takeDamage(10);
        }
    }
}
