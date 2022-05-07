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
        Collider firstThisCollider = collision.GetContact(0).thisCollider;
        Collider firstOtherCollider = collision.GetContact(0).otherCollider;

        if (enumContainsElem(typeof(Mouths), firstThisCollider.name) && enumContainsElem(typeof(Foods), collision.gameObject.name)) {
            Destroy(collision.gameObject);
            playerState.heal(10);
            ProgressionData.nbMeatsEaten++;

        }else if (enumContainsElem(typeof(AttackOrgans), firstThisCollider.name) &&  enumContainsElem(typeof(Enemies), collision.gameObject.name)) {
            collision.gameObject.GetComponent<EnemyState>().takeDamage(30, Characters.Player);

        }else if (enumContainsElem(typeof(AttackOrgans), firstOtherCollider.name) && !enumContainsElem(typeof(AttackOrgans), firstThisCollider.name) ) {
            playerState.takeDamage(10);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger enter");
        other.transform.parent.transform.parent.gameObject.GetComponent<EnemyMovement>().goTowards(this.transform.position);
    }

    private bool enumContainsElem(System.Type enumType, string word){
        return System.Enum.IsDefined(enumType, word);
    }
}
