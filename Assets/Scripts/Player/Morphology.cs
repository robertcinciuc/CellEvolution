using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morphology : MonoBehaviour
{
    public GameObject playerHead;

    private Dictionary<System.Guid, GameObject> playerOrgans;
    private Dictionary<System.Guid, GameObject> playerSegments;
    private int nbSegments = 4;

    void Awake(){
        playerOrgans = new Dictionary<System.Guid, GameObject>();
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

    public GameObject addOrganWithPos(System.Type organType, GameObject organ, System.Guid organId) {
        GameObject newOrgan = Instantiate(organ, transform.position, transform.rotation);
        newOrgan.transform.SetParent(this.gameObject.transform);
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

        playerOrgans.Add(newOrgan.GetComponent<Organ>().id, newOrgan);


        return newOrgan;
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
    
    public GameObject addSegmentWithPos(GameObject segment, System.Guid segmentId, Vector3 segmentPos) {
        string segmentName = segment.GetComponent<Segment>().segmentName;
        GameObject newSegment = Instantiate((GameObject)Resources.Load("Prefabs/" + segmentName, typeof(GameObject)), segmentPos, Quaternion.identity);
        newSegment.transform.SetParent(this.gameObject.transform);
        newSegment.transform.position = segmentPos;

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
    
    public Dictionary<System.Guid, OrganSerial> getPlayerSerialOrgans() {
        Dictionary<System.Guid, OrganSerial> serialOrgans = new Dictionary<System.Guid, OrganSerial>();
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerOrgans) {
            serialOrgans.Add(entry.Key, entry.Value.GetComponent<Organ>().getSerialOrgan());
        }

        return serialOrgans;
    }

    public void addAllOrgans(Dictionary<System.Guid, OrganSerial> organs) {
        removeAllOrgans();

        foreach (KeyValuePair<System.Guid, OrganSerial> entry in organs) {
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/" + entry.Value.organName, typeof(GameObject)), Vector3.zero, Quaternion.identity);

            //Add organ component
            Organ organComponent = organ.gameObject.AddComponent<Organ>();
            organComponent.organType = entry.Value.organType;
            organComponent.id = entry.Key;
            organComponent.organName = entry.Value.organName;

            //Add serial organ to organ component
            OrganSerial serialOrgan = new OrganSerial(organ);
            organComponent.serialOrgan = serialOrgan;

            organ.transform.localPosition = new Vector3(entry.Value.localPosX, entry.Value.localPosY, entry.Value.localPosZ);
            organ.transform.localRotation = new Quaternion(entry.Value.localRotX, entry.Value.localRotY, entry.Value.localRotZ, entry.Value.localRotW);
            addOrganWithPos(entry.Value.organType, organ, entry.Key);
            Destroy(organ);
        }
    }

    public void removeAllOrgans() {
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerOrgans) {
            DestroyImmediate(entry.Value);
        }
        playerOrgans.Clear();
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

        playerOrgans.Add(organComponent.id, organ);
        parent.GetComponent<Segment>().addOrganToMapping(organComponent.id, organ);

        return organ;
    }

    private void initSegmentedBody() {
        List<GameObject> segments = new List<GameObject>();

        for (int i = 0; i < nbSegments; i++) {
            Vector3 segmentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (i + 1) * 2);
            GameObject segment = Instantiate((GameObject)Resources.Load("Prefabs/PlayerBody", typeof(GameObject)), segmentPos, transform.rotation);
            segment.transform.parent = transform;

            //Add light rigidbody
            Rigidbody rigidbody = segment.AddComponent<Rigidbody>();
            rigidbody.drag = 10;
            rigidbody.mass = 0;

            addJoint(segment, segments, i);

            //Add segment component
            Segment segmentComponent = segment.AddComponent<Segment>();
            segmentComponent.segmentId = System.Guid.NewGuid();
            segmentComponent.segmentName = "PlayerBody";

            segment.AddComponent<PlayerCollision>();

            addSegmentToList(segmentComponent.segmentId, segment);

            segments.Add(segment);
        }
    }

    private void addJoint(GameObject segment, List<GameObject> segments, int index) {
        HingeJoint hingeJoint = segment.AddComponent<HingeJoint>();
        if (index == 0) {
            hingeJoint.connectedBody = playerHead.GetComponent<Rigidbody>();
        } else {
            hingeJoint.connectedBody = segments[index - 1].GetComponent<Rigidbody>();

        }
        hingeJoint.axis = Vector3.up;
        hingeJoint.anchor = new Vector3(0, 0, 2);

        hingeJoint.useLimits = true;
        JointLimits limits = hingeJoint.limits;
        limits.max = 20;
        limits.min = -20;

        hingeJoint.limits = limits;
    }
}
