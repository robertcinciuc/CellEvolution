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

    public GameObject addOrganFromMesh(MeshRenderer organMeshRend, Vector3 pos, Quaternion rot, string organName, System.Type organType) {
        GameObject organCopy = new GameObject();
        organCopy.name = organName;
        organCopy.transform.localPosition = pos;
        organCopy.transform.SetParent(gameObject.transform);

        MeshRenderer organCopyMeshRend = Instantiate(organMeshRend, pos, rot);
        organCopyMeshRend.name = organName + "Renderer";
        organCopyMeshRend.transform.SetParent(organCopy.transform);
        organCopyMeshRend.transform.localRotation = rot;

        //Refresh organ component
        Organ organComponent = organCopyMeshRend.GetComponent<Organ>();
        organComponent.id = organMeshRend.GetComponent<Organ>().id;

        playerOrgans.Add(organComponent.id, organCopy);

        return organCopy;
    }

    public void removeOrgan(System.Guid organId) {
        if (playerOrgans.ContainsKey(organId)) {
            GameObject organToRemove = playerOrgans[organId];
            playerOrgans.Remove(organId);
            Destroy(organToRemove);
        }
    }

    public GameObject addOrganWithPosition(System.Type organType, GameObject organ, Vector3 posDelta, System.Guid organId) {
        GameObject newOrgan = Instantiate(organ, transform.position, transform.rotation);
        newOrgan.transform.SetParent(this.gameObject.transform);
        newOrgan.transform.localPosition = posDelta;
        newOrgan.transform.localRotation = organ.transform.rotation;
        newOrgan.name = organ.name;
        
        //Add organ component
        Organ organComponent = newOrgan.transform.GetChild(0).gameObject.AddComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = organId;

        //Remove clickable organ behaviour
        Destroy(newOrgan.transform.GetChild(0).GetComponent<ClickableOrgan>());

        playerOrgans.Add(organComponent.id, newOrgan);


        return newOrgan;
    }
    
    public void moveOrgan(System.Guid organId, Vector3 localPos, Quaternion rot) {
        playerOrgans[organId].transform.localPosition = localPos;
        playerOrgans[organId].transform.localRotation = rot;
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

        playerOrgans.Add(organComponent.id, organ);
    }

}
