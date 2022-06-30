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
    }
}
