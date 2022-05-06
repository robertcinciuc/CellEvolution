using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    void Start(){
        
    }

    void Update(){
        
    }

    private void OnTriggerEnter(Collider other) {
        //transform.parent.GetComponent<EnemyMovement>().goTowards(other.transform.position);
        //if (other.transform.parent.name.Contains("layer")) {
        //    Debug.Log("Player is inside");
        //}
    }
}
