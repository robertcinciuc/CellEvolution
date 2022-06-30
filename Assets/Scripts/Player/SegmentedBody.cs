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
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/PlayerBody", typeof(GameObject)), segmentPos, transform.rotation);
            organ.AddComponent<Rigidbody>();
            
            if(i == 0) {
                organ.AddComponent<HeadMovement>();
            } else {
                CharacterJoint characterJoint = organ.AddComponent<CharacterJoint>();
                characterJoint.connectedBody = ((GameObject)segments[i - 1]).GetComponent<Rigidbody>();
            }
            segments.Add(organ);
        }
    }

    void Update(){
        
    }
}