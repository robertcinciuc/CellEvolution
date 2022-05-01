using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    //Temporary map while there's only 1 organ of each type on body
    private Dictionary<System.Type, GameObject> playerOrgansByType;

    void Awake(){
        playerOrgansByType = new Dictionary<System.Type, GameObject>();

        if (gameObject.name == "Player") {
            addPlayerOrgan(Bodies.PlayerBody.ToString(), "Prefabs/PlayerBody", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 0), Quaternion.identity, typeof(Bodies));
            addPlayerOrgan(Mouths.Mouth.ToString(), "Prefabs/Mouth", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 1), Quaternion.identity, typeof(Mouths));
            addPlayerOrgan(LocomotionOrgans.Flagella.ToString(), "Prefabs/Flagella", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, -1f), new Quaternion(0.71f, 0, 0, -0.71f), typeof(LocomotionOrgans));
            addPlayerOrgan(AttackOrgans.Spike.ToString(), "Prefabs/Spike", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-0.5f, 0.3f, 0.5f), new Quaternion(0, 0, 0.71f, 0.71f), typeof(AttackOrgans));
        }
    }

    void Update(){
        
    }

    void FixedUpdate() {
    }

    public void setOrganByType(System.Type organType, GameObject organ) {
        GameObject oldOrgan = playerOrgansByType[organType];
        GameObject newOrgan = Instantiate(organ, oldOrgan.transform.position, oldOrgan.transform.rotation);
        newOrgan.AddComponent<Organ>().organType = organType;
        newOrgan.transform.SetParent(this.gameObject.transform);
        newOrgan.transform.localPosition = oldOrgan.transform.localPosition;
        newOrgan.transform.localRotation = oldOrgan.transform.localRotation;
        newOrgan.name = organ.name;

        Destroy(oldOrgan);

        playerOrgansByType[organType] = newOrgan;
    }

    public void addOrganFromMeshByType(MeshRenderer organMeshRend, Vector3 pos, Quaternion rot, string organName, System.Type organType) {
        GameObject organCopy = new GameObject();
        organCopy.name = organName;
        organCopy.transform.position = pos;
        organCopy.transform.rotation = rot;
        organCopy.transform.SetParent(gameObject.transform);

        MeshRenderer organCopyMeshRend = Instantiate(organMeshRend, pos, rot);
        organCopyMeshRend.name = organName + "Renderer";
        organCopyMeshRend.transform.SetParent(organCopy.transform);

        playerOrgansByType.Add(organType, organCopy);
    }

    private void addPlayerOrgan(string name, string prefabPath, Vector3 pos, Quaternion rot, Vector3 localPos, Quaternion localRot, System.Type organType) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.AddComponent<Organ>().organType = organType;
        organ.transform.SetParent(this.gameObject.transform);
        organ.transform.localPosition = localPos;
        organ.transform.localRotation = localRot;
        organ.name = name;
        playerOrgansByType.Add(organType, organ);
    }

}
