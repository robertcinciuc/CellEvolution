using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morphology : MonoBehaviour
{
    public MainCameraMovement mainCameraMovement;
    public GameObject playerHead;
    public int nbSegments = 4;
    public float segmentDistance = 2;

    private Dictionary<System.Guid, GameObject> playerSegments;

    void Awake(){
        playerSegments = new Dictionary<System.Guid, GameObject>();
    }

    void Update(){
    }

    void FixedUpdate() {
    }

    public void removeOrgan(System.Guid segmentId, System.Guid organId) {
        if (playerSegments.ContainsKey(segmentId)) {
            playerSegments[segmentId].GetComponent<Segment>().removeOrgan(organId);
        }
    }

    public void removeOrganFromMapping(System.Guid segmentId, System.Guid organId) {
        if (playerSegments.ContainsKey(segmentId)) {
            playerSegments[segmentId].GetComponent<Segment>().removeOrganFromMapping(organId);
        }
    }

    public GameObject addOrganOnSegmentWithPos(GameObject segment, System.Type organType, GameObject organ, System.Guid organId) {
        GameObject newOrgan = Instantiate(organ, transform.position, transform.rotation);
        newOrgan.transform.SetParent(segment.transform);
        newOrgan.transform.localPosition = organ.transform.localPosition;
        newOrgan.transform.localRotation = organ.transform.localRotation;

        //Update organ component
        Organ organComponent = newOrgan.GetComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = organId;
        organComponent.organName = organ.GetComponent<Organ>().organName;

        //Add serial organ to organ component
        OrganSerial serialOrgan = new OrganSerial(newOrgan);
        organComponent.serialOrgan = serialOrgan;

        if (organ.GetComponent<Organ>() != null) {
            newOrgan.name = organ.GetComponent<Organ>().organName;
        } else if (organ.GetComponent<OrganSerial>() != null) {
            newOrgan.name = organ.GetComponent<OrganSerial>().organName;
        }

        //Remove clickable organ behaviour
        if (newOrgan.GetComponent<ClickableOrgan>() != null) {
            Destroy(newOrgan.GetComponent<ClickableOrgan>());
        }

        segment.GetComponent<Segment>().addOrganToMapping(organId, newOrgan);

        return newOrgan;
    }
    
    public GameObject addSegmentWithPosRot(GameObject segment, System.Guid segmentId, Vector3 segmentPos, Quaternion segmentRot) {
        string segmentName = segment.GetComponent<Segment>().segmentName;
        GameObject newSegment = Instantiate((GameObject)Resources.Load("Prefabs/" + segmentName, typeof(GameObject)), segmentPos, Quaternion.identity);
        newSegment.transform.SetParent(this.gameObject.transform);
        newSegment.transform.localRotation = segmentRot;

        //Add segment component
        Segment segmentComponent = newSegment.AddComponent<Segment>();
        segmentComponent.segmentId = segmentId;
        segmentComponent.segmentName = segmentName;

        playerSegments.Add(segmentComponent.segmentId, newSegment);

        return newSegment;
    }

    public GameObject simpleAddOrganOnSegmentWithPos(GameObject segment, System.Type organType, GameObject organ, System.Guid organId) {
        //Update organ component
        Organ organComponent = organ.GetComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = organId;
        organComponent.organName = organ.GetComponent<Organ>().organName;

        //Add serial organ to organ component
        OrganSerial serialOrgan = new OrganSerial(organ);
        organComponent.serialOrgan = serialOrgan;

        //Remove clickable organ behaviour
        if (organ.GetComponent<ClickableOrgan>() != null) {
            Destroy(organ.GetComponent<ClickableOrgan>());
        }

        segment.GetComponent<Segment>().addOrganToMapping(organId, organ);

        return organ;
    }
    
    public void updateMorphology(MorphologySerial morphologySerial) {
        //Removing the old segments
        removeAllSegments();

        nbSegments = morphologySerial.nbSegments;
        GameObject prevSegment = null;
        int segmentIndex = 0;
        foreach(KeyValuePair<System.Guid, SegmentSerial> entry in morphologySerial.segmentsSerial) {
            //Spawn character in default pos & rot
            Vector3 segmentPos = new Vector3(entry.Value.posX, entry.Value.posY, entry.Value.posZ);
            Quaternion segmentRot = new Quaternion(entry.Value.rotX, entry.Value.rotY, entry.Value.rotZ, entry.Value.rotW);
            if(segmentIndex > 0) {
                segmentPos = playerHead.transform.position + playerHead.transform.rotation * Vector3.forward * -1 * segmentDistance * segmentIndex;
                segmentRot = playerHead.transform.rotation;
            }

            GameObject segment = Instantiate((GameObject)Resources.Load("Prefabs/" + entry.Value.segmentName, typeof(GameObject)), segmentPos, Quaternion.identity);

            //Temporary workaround
            Segment oldSegmentComponent = segment.AddComponent<Segment>();
            oldSegmentComponent.segmentName = entry.Value.segmentName;

            GameObject newSegment = addSegmentWithPosRot(segment, entry.Value.segmentId, segmentPos, segmentRot);
            newSegment.GetComponent<Segment>().updateSegment(entry.Value);

            if (newSegment.GetComponent<Segment>().GetComponent<PlayerMovement>() != null) {
                playerHead = newSegment;
                mainCameraMovement.updateHead(newSegment);
            }

            addJoint(newSegment, prevSegment);
            prevSegment = newSegment;

            DestroyImmediate(segment);
            segmentIndex += 1;
        }
    }

    public void resetMorphology() {

    }

    public void removeAllSegments() {
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerSegments) {
            DestroyImmediate(entry.Value);
        }
        playerSegments.Clear();
    }

    public void initPlayerStructure() {
        //playerHead = initPlayerOrgan(Bodies.PlayerHead.ToString(), "Prefabs/PlayerBody", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 0), Quaternion.identity, typeof(Bodies));
        //initPlayerOrgan(LocomotionOrgans.Flagella.ToString(), "Prefabs/Flagella", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, -1f), Quaternion.identity, typeof(LocomotionOrgans));
        //initPlayerOrgan(AttackOrgans.Spike.ToString(), "Prefabs/Spike", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-0.7f, 0.3f, 0.5f), Quaternion.identity, typeof(AttackOrgans));

        //Add segment component
        Segment segmentComponent = playerHead.AddComponent<Segment>();
        segmentComponent.segmentId = System.Guid.NewGuid();
        segmentComponent.segmentName = "PlayerBody";

        playerSegments.Add(segmentComponent.segmentId, playerHead);

        initSegmentedBody();

        initPlayerOrgan(Mouths.Mouth.ToString(), "Prefabs/Mouth", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 1), Quaternion.identity, typeof(Mouths), playerHead);

    }

    public GameObject getHead() {
        return playerHead;
    }

    public Dictionary<System.Guid, GameObject> getSegments() {
        return playerSegments;
    }

    public GameObject getSegment(System.Guid segmentId) { 
        return playerSegments[segmentId];
    }

    public void addSegmentToList(System.Guid segmentId, GameObject segment) {
        playerSegments.Add(segmentId, segment);
    }

    private GameObject initPlayerOrgan(string name, string prefabPath, Vector3 pos, Quaternion rot, Vector3 localPos, Quaternion localRot, System.Type organType, GameObject parent) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.transform.SetParent(parent.transform);
        organ.transform.localPosition = localPos;
        //organ.transform.localRotation = localRot;
        organ.name = name;

        //Add organ component
        Organ organComponent = organ.AddComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = System.Guid.NewGuid();
        organComponent.organName = name;

        //Add serial organ to organ component
        OrganSerial serialOrgan = new OrganSerial(organ);
        organComponent.serialOrgan = serialOrgan;

        parent.GetComponent<Segment>().addOrganToMapping(organComponent.id, organ);

        return organ;
    }

    private void initSegmentedBody() {
        List<GameObject> segments = new List<GameObject>();

        GameObject prevSegment = playerHead;
        for (int i = 0; i < nbSegments; i++) {
            Vector3 segmentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (i + 1) * segmentDistance);
            // TODO: Change transform.rotation into appropriate slice rotation
            // TODO: Change the initial rotation of the head slice (of the object in unity)
            GameObject segment = Instantiate((GameObject)Resources.Load("Prefabs/Slice", typeof(GameObject)), segmentPos, transform.rotation);
            segment.transform.parent = transform;

            //Add light rigidbody
            Rigidbody rigidbody = segment.AddComponent<Rigidbody>();
            rigidbody.drag = 10;
            rigidbody.mass = 0;

            addJoint(segment, prevSegment);
            prevSegment = segment;

            //Add segment component
            Segment segmentComponent = segment.AddComponent<Segment>();
            segmentComponent.segmentId = System.Guid.NewGuid();
            segmentComponent.segmentName = "PlayerBody";

            segment.AddComponent<PlayerCollision>();

            addSegmentToList(segmentComponent.segmentId, segment);

            segments.Add(segment);
        }
    }

    private void addJoint(GameObject segment, GameObject prevSegment) {
        if (prevSegment != null) {
            HingeJoint hingeJoint = segment.AddComponent<HingeJoint>();
            hingeJoint.connectedBody = prevSegment.GetComponent<Rigidbody>();

            hingeJoint.axis = Vector3.up;
            hingeJoint.anchor = new Vector3(0, 0, segmentDistance);

            hingeJoint.useLimits = true;
            JointLimits limits = hingeJoint.limits;
            limits.max = 20;
            limits.min = -20;

            hingeJoint.limits = limits;
        }
    }

    private void getMeshOnPoints(GameObject segment1, GameObject segment2){
        Vector3[] vertices1 = segment1.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] vertices2 = segment2.GetComponent<MeshFilter>().mesh.vertices;
        
        MeshGenerator meshGenerator = gameObject.transform.parent.GetComponent<MeshGenerator>();
        meshGenerator.drawCylinder(vertices1, vertices2);
    }
}
