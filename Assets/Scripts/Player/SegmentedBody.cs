using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedBody : MonoBehaviour{

    public ArrayList segments;
    private int nbSegments = 4;

    void Start(){
        segments = new ArrayList();

        for (int i = 0; i < nbSegments; i++) {
            Vector3 segmentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - i * 2); 
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/PlayerBody", typeof(GameObject)), segmentPos, transform.rotation);
            Rigidbody rigidbody = organ.AddComponent<Rigidbody>();
            
            if(i == 0) {
                organ.AddComponent<HeadMovement>();
            } else {
                rigidbody.drag = 10;
                rigidbody.mass = 0;

                HingeJoint hingeJoint = organ.AddComponent<HingeJoint>();
                hingeJoint.connectedBody = ((GameObject)segments[i - 1]).GetComponent<Rigidbody>();
                hingeJoint.axis = Vector3.up;
                hingeJoint.anchor = new Vector3(0, 0, 2);

                hingeJoint.useLimits = true;
                JointLimits limits = hingeJoint.limits;
                limits.max = 60;
                limits.min = -60;

                hingeJoint.limits = limits;
            }
            segments.Add(organ);
        }
    }

    void Update(){
        
    }
}
