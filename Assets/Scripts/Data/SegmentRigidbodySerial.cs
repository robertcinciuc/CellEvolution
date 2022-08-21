using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SegmentRigidbodySerial{

    public float mass;
    public float drag;

    public SegmentRigidbodySerial(Rigidbody rigidbody) {
        mass = rigidbody.mass;
        drag = rigidbody.drag;
    }

}
