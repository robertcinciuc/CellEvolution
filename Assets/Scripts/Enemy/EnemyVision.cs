using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    void Start(){
        
    }

    void Update(){
        //TODO: fix vision rotation lock
        gameObject.transform.rotation = new Quaternion(0, transform.parent.transform.rotation.y, 0, transform.parent.transform.rotation.w); ;
    }

    private void OnTriggerEnter(Collider other) {
        //transform.parent.GetComponent<EnemyMovement>().goTowards(other.transform.position);
        if (other.transform.gameObject.name.Contains("layer")) {
            Debug.Log("Player is inside");
        }
        //Debug.Log("collision with enemy vision");
    }
}
