using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedBody : MonoBehaviour{

    public PlayerBodyStructure playerBodyStructure;
    public List<GameObject> followers;
    private int nbFollowers = 4;

    void Start(){
        followers = new List<GameObject>();
    }

    void Update(){
        
    }

    public void initSegmentedBody() {
        GameObject playerHead = playerBodyStructure.getHead();

        for (int i = 0; i < nbFollowers; i++) {
            Vector3 followerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (i + 1) * 2);
            GameObject follower = Instantiate((GameObject)Resources.Load("Prefabs/PlayerBody", typeof(GameObject)), followerPos, transform.rotation);
            follower.transform.parent = transform;
            Rigidbody rigidbody = follower.AddComponent<Rigidbody>();

            rigidbody.drag = 10;
            rigidbody.mass = 0;

            HingeJoint hingeJoint = follower.AddComponent<HingeJoint>();
            if (i == 0) {
                hingeJoint.connectedBody = playerHead.GetComponent<Rigidbody>();
            } else {
                hingeJoint.connectedBody = ((GameObject)followers[i - 1]).GetComponent<Rigidbody>();

            }
            hingeJoint.axis = Vector3.up;
            hingeJoint.anchor = new Vector3(0, 0, 2);

            hingeJoint.useLimits = true;
            JointLimits limits = hingeJoint.limits;
            limits.max = 20;
            limits.min = -20;

            hingeJoint.limits = limits;

            followers.Add(follower);
        }
    }
}
