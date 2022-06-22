using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedBody : MonoBehaviour{

    public ArrayList segments;
    public int nbSegments = 4;

    void Start(){
        segments = new ArrayList();

        for (int i = 0; i < nbSegments; i++) {
            Vector3 segmentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - i * 2); 
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/Body", typeof(GameObject)), segmentPos, transform.rotation);
            organ.AddComponent<Rigidbody>();
            
            if(i == 0) {
                organ.AddComponent<HeadMovement>();
            } else {
                FollowerSegmentMovement followerSegmentMovement = organ.AddComponent<FollowerSegmentMovement>();
                followerSegmentMovement.head = ((GameObject)segments[i-1]).transform;
            }
            segments.Add(organ);
        }
    }

    void Update(){
        
    }
}
