using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float timeLeft = -1f;
    private float maxSpeed = 5f;
    private Vector3 randomDirection;

    void Start(){
        randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-maxSpeed, maxSpeed));
    }

    private void FixedUpdate() {
        timeLeft -= Time.deltaTime;

        //updateVelocityRotation();
    }
    
    public void goTowards(Vector3 characterPos) {
        this.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(this.gameObject.GetComponent<Rigidbody>().transform.rotation, Quaternion.LookRotation(characterPos), 50 * Time.deltaTime);
        this.gameObject.GetComponent<Rigidbody>().velocity = characterPos;
    }
    private void updateVelocityRotation() {
        if (timeLeft < 0) {
            randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-maxSpeed, maxSpeed));
            this.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(this.gameObject.GetComponent<Rigidbody>().transform.rotation, Quaternion.LookRotation(randomDirection), 50 * Time.deltaTime);
            timeLeft = 5f;
        }

        this.gameObject.GetComponent<Rigidbody>().velocity = randomDirection;
    }


}
