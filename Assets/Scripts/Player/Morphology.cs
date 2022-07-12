using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morphology : MonoBehaviour
{
    public SegmentedBody segmentedBody;
    public GameObject playerHead;

    private Dictionary<System.Guid, GameObject> playerOrgans;
    private Dictionary<System.Guid, GameObject> playerSegments;
    private Dictionary<System.Guid, Dictionary<System.Guid, GameObject>> segmentOrgans;
    private int nbSegments = 4;

    void Awake(){
        playerOrgans = new Dictionary<System.Guid, GameObject>();
        playerSegments = new Dictionary<System.Guid, GameObject>();
        segmentOrgans = new Dictionary<System.Guid, Dictionary<System.Guid, GameObject>>();
    }

    void Update(){
    }

    void FixedUpdate() {
    }

    public void removeOrgan(System.Guid segmentId, System.Guid organId) {
        if (playerSegments.ContainsKey(segmentId) && segmentOrgans.ContainsKey(segmentId) && segmentOrgans[segmentId].ContainsKey(organId)) {
            GameObject organToRemove = segmentOrgans[segmentId][organId];
            segmentOrgans[segmentId].Remove(organId);
            Destroy(organToRemove);
        }
    }

    public void removeOrganFromMapping(System.Guid segmentId, System.Guid organId) {
        if (playerSegments.ContainsKey(segmentId) && segmentOrgans[segmentId].ContainsKey(organId)) {
            segmentOrgans[segmentId].Remove(organId);
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
        SerialOrgan serialOrgan = new SerialOrgan(newOrgan);
        organComponent.serialOrgan = serialOrgan;

        if (organ.GetComponent<Organ>() != null) {
            newOrgan.name = organ.GetComponent<Organ>().organName;
        } else if (organ.GetComponent<SerialOrgan>() != null) {
            newOrgan.name = organ.GetComponent<SerialOrgan>().organName;
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
        SerialOrgan serialOrgan = new SerialOrgan(newOrgan);
        organComponent.serialOrgan = serialOrgan;

        if (organ.GetComponent<Organ>() != null) {
            newOrgan.name = organ.GetComponent<Organ>().organName;
        } else if (organ.GetComponent<SerialOrgan>() != null) {
            newOrgan.name = organ.GetComponent<SerialOrgan>().organName;
        }

        //Remove clickable organ behaviour
        if (newOrgan.GetComponent<ClickableOrgan>() != null) {
            Destroy(newOrgan.GetComponent<ClickableOrgan>());
        }

        System.Guid segmentId = segment.GetComponent<Segment>().segmentId;
        if (!segmentOrgans.ContainsKey(segmentId)) {
            segmentOrgans.Add(segmentId, new Dictionary<System.Guid, GameObject>());
        }

        segmentOrgans[segmentId].Add(organId, newOrgan);

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
        SerialOrgan serialOrgan = new SerialOrgan(organ);
        organComponent.serialOrgan = serialOrgan;

        //Remove clickable organ behaviour
        if (organ.GetComponent<ClickableOrgan>() != null) {
            Destroy(organ.GetComponent<ClickableOrgan>());
        }

        System.Guid segmentId = segment.GetComponent<Segment>().segmentId;

        if (!segmentOrgans.ContainsKey(segmentId)) {
            segmentOrgans.Add(segmentId, new Dictionary<System.Guid, GameObject>());
        }
        segmentOrgans[segmentId].Add(organId, organ);

        return organ;
    }
    
    public Dictionary<System.Guid, SerialOrgan> getPlayerSerialOrgans() {
        Dictionary<System.Guid, SerialOrgan> serialOrgans = new Dictionary<System.Guid, SerialOrgan>();
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerOrgans) {
            serialOrgans.Add(entry.Key, entry.Value.GetComponent<Organ>().getSerialOrgan());
        }

        return serialOrgans;
    }

    public void addAllOrgans(Dictionary<System.Guid, SerialOrgan> organs) {
        removeAllOrgans();

        foreach (KeyValuePair<System.Guid, SerialOrgan> entry in organs) {
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/" + entry.Value.organName, typeof(GameObject)), Vector3.zero, Quaternion.identity);

            //Add organ component
            Organ organComponent = organ.gameObject.AddComponent<Organ>();
            organComponent.organType = entry.Value.organType;
            organComponent.id = entry.Key;
            organComponent.organName = entry.Value.organName;

            //Add serial organ to organ component
            SerialOrgan serialOrgan = new SerialOrgan(organ);
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
        segmentOrgans.Add(segmentComponent.segmentId, new Dictionary<System.Guid, GameObject>());

        segmentedBody.initSegmentedBody(nbSegments);

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
        SerialOrgan serialOrgan = new SerialOrgan(organ);
        organComponent.serialOrgan = serialOrgan;

        playerOrgans.Add(organComponent.id, organ);
        System.Guid segmentId = parent.GetComponent<Segment>().segmentId;
        segmentOrgans[segmentId].Add(organComponent.id, organ);

        return organ;
    }

}
