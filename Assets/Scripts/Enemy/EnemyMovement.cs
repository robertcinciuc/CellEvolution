using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool followingSomeone = false;
    private float timeLeft = -1f;
    private float maxSpeed = 5f;
    private Vector3 randomDirection;

    void Start(){
        randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-maxSpeed, maxSpeed));
    }

    private void FixedUpdate() {
        timeLeft -= Time.deltaTime;

        updateVelocityRotation();
    }
    
    public void startFollowing(Vector3 characterPos) {
        Quaternion rotAngle = Quaternion.LookRotation(characterPos - transform.position);
        this.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(this.gameObject.GetComponent<Rigidbody>().transform.rotation, rotAngle, 3 * Time.deltaTime);
        this.gameObject.GetComponent<Rigidbody>().velocity = characterPos - transform.position;
        followingSomeone = true;
    }

    public void stopFollowing(){
        followingSomeone = false;
        randomDirection = this.gameObject.GetComponent<Rigidbody>().velocity;
        this.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(this.gameObject.GetComponent<Rigidbody>().transform.rotation, Quaternion.LookRotation(randomDirection), 2 * Time.deltaTime);
    }

    private void updateVelocityRotation() {
        if (!followingSomeone) {
            if (timeLeft < 0) {
                randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-maxSpeed, maxSpeed));
                this.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(this.gameObject.GetComponent<Rigidbody>().transform.rotation, Quaternion.LookRotation(randomDirection), 50 * Time.deltaTime);
                timeLeft = 5f;
            }

            this.gameObject.GetComponent<Rigidbody>().velocity = randomDirection;
        }
    }

}
