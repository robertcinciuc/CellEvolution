using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSegmentMovement : MonoBehaviour {
    public float playerSpeed = 5.0f;
    public Transform head;

    private Rigidbody rigidBody;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update() {

    }

    void FixedUpdate() {
        translateToLeader();
        rotateToLeader2();
    }

    private void translateToLeader() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 moveVector = this.transform.TransformDirection(movement) * playerSpeed;
        rigidBody.velocity = new Vector3(moveVector.x, rigidBody.velocity.y, moveVector.z);
    }

    private void rotateToLeader() {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 headOnScreen = new Vector2(head.position.x, head.position.z);
        float angle = AngleBetweenTwoPoints(posOnScreen, headOnScreen);
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        rigidBody.rotation = Quaternion.Slerp(rigidBody.transform.rotation, rotation, 7 * Time.deltaTime);
    }
    private void rotateToLeader2() {
        Vector3 targetDirection = head.position - transform.position;
        rigidBody.rotation = Quaternion.LookRotation(targetDirection);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
    }
}
