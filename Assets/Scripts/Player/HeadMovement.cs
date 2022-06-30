using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour {
    public float playerSpeed = 5.0f;
    public Vector3 mouseOnScreen;

    private Rigidbody rigidBody;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update() {

    }

    void FixedUpdate() {
        translateToMouse();
        rotateOnForwardMovement();
    }

    private void translateToMouse() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 moveVector = this.transform.TransformDirection(movement) * playerSpeed;
        rigidBody.velocity = new Vector3(moveVector.x, rigidBody.velocity.y, moveVector.z);
    }

    private void rotateToMouse() {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(posOnScreen, mouseOnScreen);
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        rigidBody.rotation = Quaternion.Slerp(rigidBody.transform.rotation, rotation, 7 * Time.deltaTime);
    }

    private void rotateToMouse2() {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 targetDirection = mouseOnScreen - posOnScreen;
        Vector3 targetDirection3d = new Vector3(targetDirection.x, 0, targetDirection.y);
        rigidBody.rotation = Quaternion.LookRotation(targetDirection3d);
    }

    private void rotateOnForwardMovement() {
        if (Input.GetKey(KeyCode.W)) {
            rotateToMouse();
        }
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
    }
}
