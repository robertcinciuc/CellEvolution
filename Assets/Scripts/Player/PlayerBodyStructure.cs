using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    private Dictionary<System.Guid, GameObject> playerOrgans;

    void Awake(){
        playerOrgans = new Dictionary<System.Guid, GameObject>();

        if (gameObject.name == "Player") {
            addPlayerOrgan(Bodies.PlayerBody.ToString(), "Prefabs/PlayerBody", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 0), Quaternion.identity, typeof(Bodies));
            addPlayerOrgan(Mouths.Mouth.ToString(), "Prefabs/Mouth", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 1), Quaternion.identity, typeof(Mouths));
            addPlayerOrgan(LocomotionOrgans.Flagella.ToString(), "Prefabs/Flagella", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, -1f), Quaternion.identity, typeof(LocomotionOrgans));
            addPlayerOrgan(AttackOrgans.Spike.ToString(), "Prefabs/Spike", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-0.7f, 0.3f, 0.5f), Quaternion.identity, typeof(AttackOrgans));
        }
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

    public GameObject addOrganWithPosition(System.Type organType, GameObject organ, Vector3 posDelta, Quaternion rotDelta, System.Guid organId) {
        GameObject newOrgan = Instantiate(organ, transform.position, transform.rotation);
        newOrgan.transform.SetParent(this.gameObject.transform);
        newOrgan.transform.localPosition = posDelta;
        newOrgan.transform.localRotation = rotDelta;

        //Update organ component
        Organ organComponent = newOrgan.transform.GetChild(0).GetComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = organId;
        organComponent.organName = organ.transform.GetChild(0).GetComponent<Organ>().organName;

        //Add serial organ to organ component
        SerialOrgan serialOrgan = new SerialOrgan(newOrgan);
        organComponent.serialOrgan = serialOrgan;

        if (organ.transform.GetChild(0).GetComponent<Organ>() != null) {
            newOrgan.name = organ.transform.GetChild(0).GetComponent<Organ>().organName;
        } else if (organ.transform.GetChild(0).GetComponent<SerialOrgan>() != null) {
            newOrgan.name = organ.transform.GetChild(0).GetComponent<SerialOrgan>().organName;
        }

        //Remove clickable organ behaviour
        if (newOrgan.transform.GetChild(0).GetComponent<ClickableOrgan>() != null) {
            Destroy(newOrgan.transform.GetChild(0).GetComponent<ClickableOrgan>());
        }

        playerOrgans.Add(newOrgan.transform.GetChild(0).GetComponent<Organ>().id, newOrgan);


        return newOrgan;
    }
    
    public void moveOrgan(System.Guid organId, Vector3 localPos, Quaternion rot) {
        playerOrgans[organId].transform.localPosition = localPos;
        playerOrgans[organId].transform.localRotation = rot;
    }

    public Dictionary<System.Guid, SerialOrgan> getPlayerSerialOrgans() {
        Dictionary<System.Guid, SerialOrgan> serialOrgans = new Dictionary<System.Guid, SerialOrgan>();
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerOrgans) {
            serialOrgans.Add(entry.Key, entry.Value.transform.GetChild(0).GetComponent<Organ>().getSerialOrgan());
        }

        return serialOrgans;
    }

    public void addAllOrgans(Dictionary<System.Guid, SerialOrgan> organs) {
        removeAllOrgans();

        foreach (KeyValuePair<System.Guid, SerialOrgan> entry in organs) {
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/" + entry.Value.organName, typeof(GameObject)), Vector3.zero, Quaternion.identity);

            //Add organ component
            Organ organComponent = organ.transform.GetChild(0).gameObject.AddComponent<Organ>();
            organComponent.organType = entry.Value.organType;
            organComponent.id = entry.Key;
            organComponent.organName = entry.Value.organName;

            //Add serial organ to organ component
            SerialOrgan serialOrgan = new SerialOrgan(organ);
            organComponent.serialOrgan = serialOrgan;

            Vector3 organLocalPos = new Vector3(entry.Value.localPosX, entry.Value.localPosY, entry.Value.localPosZ);
            organ.transform.localRotation = new Quaternion(entry.Value.localRotX, entry.Value.localRotY, entry.Value.localRotZ, entry.Value.localRotW);
            addOrganWithPosition(entry.Value.organType, organ, organLocalPos, organ.transform.rotation, entry.Key);
            Destroy(organ);
        }
    }

    public void removeAllOrgans() {
        foreach (KeyValuePair<System.Guid, GameObject> entry in playerOrgans) {
            Destroy(entry.Value);
        }
        playerOrgans.Clear();
    }

    private void addPlayerOrgan(string name, string prefabPath, Vector3 pos, Quaternion rot, Vector3 localPos, Quaternion localRot, System.Type organType) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.transform.SetParent(this.gameObject.transform);
        organ.transform.localPosition = localPos;
        organ.transform.localRotation = localRot;
        organ.name = name;

        //Add organ component
        Organ organComponent = organ.transform.GetChild(0).gameObject.AddComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = System.Guid.NewGuid();
        organComponent.organName = name;

        //Add serial organ to organ component
        SerialOrgan serialOrgan = new SerialOrgan(organ);
        organComponent.serialOrgan = serialOrgan;

        playerOrgans.Add(organComponent.id, organ);
    }

}
