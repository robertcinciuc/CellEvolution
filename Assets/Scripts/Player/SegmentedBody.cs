using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedBody : MonoBehaviour{

    public PlayerBodyStructure playerBodyStructure;
    public List<GameObject> segments;

    void Start(){
        segments = new List<GameObject>();
    }

    void Update(){
        
    }

    public void initSegmentedBody(int nbFollowers) {
        GameObject playerHead = playerBodyStructure.getHead();

        for (int i = 0; i < nbFollowers; i++) {
            Vector3 segmentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (i + 1) * 2);
            GameObject segment = Instantiate((GameObject)Resources.Load("Prefabs/PlayerBody", typeof(GameObject)), segmentPos, transform.rotation);
            segment.transform.parent = transform;
            Rigidbody rigidbody = segment.AddComponent<Rigidbody>();

            rigidbody.drag = 10;
            rigidbody.mass = 0;

            HingeJoint hingeJoint = segment.AddComponent<HingeJoint>();
            if (i == 0) {
                hingeJoint.connectedBody = playerHead.GetComponent<Rigidbody>();
            } else {
                hingeJoint.connectedBody = ((GameObject)segments[i - 1]).GetComponent<Rigidbody>();

            }
            hingeJoint.axis = Vector3.up;
            hingeJoint.anchor = new Vector3(0, 0, 2);

            hingeJoint.useLimits = true;
            JointLimits limits = hingeJoint.limits;
            limits.max = 20;
            limits.min = -20;

            hingeJoint.limits = limits;

            //Add segment component
            Segment segmentComponent = segment.AddComponent<Segment>();
            segmentComponent.segmentId = System.Guid.NewGuid();
            segmentComponent.segmentName = "PlayerBody";

            playerBodyStructure.addSegmentToList(segmentComponent.segmentId, segment);

            segments.Add(segment);
        }
    }
}
