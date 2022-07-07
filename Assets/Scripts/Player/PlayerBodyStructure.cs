using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    public SegmentedBody segmentedBody;

    private GameObject playerHead;
    private Dictionary<System.Guid, GameObject> playerOrgans;

    void Awake(){
        playerOrgans = new Dictionary<System.Guid, GameObject>();
    }

    void Update(){
    }

    void FixedUpdate() {
    }

    public void removeOrgan(System.Guid organId) {
        if (playerOrgans.ContainsKey(organId)) {
            GameObject organToRemove = playerOrgans[organId];
            playerOrgans.Remove(organId);
            Destroy(organToRemove);
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

    public GameObject simpleAddOrganWithPos(System.Type organType, GameObject organ, System.Guid organId) {
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

        playerOrgans.Add(organ.GetComponent<Organ>().id, organ);

        return organ;
    }

    public void moveOrgan(System.Guid organId, Vector3 localPos, Quaternion rot) {
        playerOrgans[organId].transform.localPosition = localPos;
        playerOrgans[organId].transform.localRotation = rot;
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
        playerHead = initPlayerOrgan(Bodies.PlayerHead.ToString(), "Prefabs/PlayerBody", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 0), Quaternion.identity, typeof(Bodies));
        playerHead.AddComponent<Rigidbody>();
        initPlayerOrgan(Mouths.Mouth.ToString(), "Prefabs/Mouth", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 1), Quaternion.identity, typeof(Mouths));
        initPlayerOrgan(LocomotionOrgans.Flagella.ToString(), "Prefabs/Flagella", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, -1f), Quaternion.identity, typeof(LocomotionOrgans));
        initPlayerOrgan(AttackOrgans.Spike.ToString(), "Prefabs/Spike", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-0.7f, 0.3f, 0.5f), Quaternion.identity, typeof(AttackOrgans));

        segmentedBody.initSegmentedBody();
    }

    public GameObject getHead() {
        return playerHead;
    }

    private GameObject initPlayerOrgan(string name, string prefabPath, Vector3 pos, Quaternion rot, Vector3 localPos, Quaternion localRot, System.Type organType) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.transform.SetParent(this.gameObject.transform);
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

        return organ;
    }

}
