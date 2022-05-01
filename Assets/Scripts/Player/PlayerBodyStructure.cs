using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyStructure : MonoBehaviour
{
    //Temporary map while there's only 1 organ of each type on body
    private Dictionary<System.Type, GameObject> playerOrgansByType;

    void Start(){
        playerOrgansByType = new Dictionary<System.Type, GameObject>();

        addPlayerOrgan(Mouths.Mouth.ToString(), "Prefabs/Mouth", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, 0, 1), Quaternion.identity, typeof(Mouths));
        addPlayerOrgan(LocomotionOrgans.Flagella.ToString(), "Prefabs/Flagella", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(0, -0.3f, -0.5f), new Quaternion(0.71f, 0, 0, -0.71f), typeof(LocomotionOrgans));
        addPlayerOrgan(AttackOrgans.Spike.ToString(), "Prefabs/Spike", new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-0.5f, 0.3f, 0.5f), new Quaternion(0, 0, 0.71f, 0.71f), typeof(AttackOrgans));
    }

    void Update(){
        
    }

    void FixedUpdate() {
    }

    public void setOrganByType(System.Type organType, GameObject organ) {
        GameObject oldOrgan = playerOrgansByType[organType];
        GameObject newOrgan = Instantiate(organ, oldOrgan.transform.position, oldOrgan.transform.rotation);
        newOrgan.transform.SetParent(this.gameObject.transform);
        newOrgan.transform.localPosition = oldOrgan.transform.localPosition;
        newOrgan.transform.localRotation = oldOrgan.transform.localRotation;
        newOrgan.name = organ.name;

        Destroy(oldOrgan);

        playerOrgansByType[organType] = newOrgan;
    }

    private void addPlayerOrgan(string name, string prefabPath, Vector3 pos, Quaternion rot, Vector3 localPos, Quaternion localRot, System.Type organType) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.transform.SetParent(this.gameObject.transform);
        organ.transform.localPosition = localPos;
        organ.transform.localRotation = localRot;
        organ.name = name;
        playerOrgansByType.Add(organType, organ);
    }

}
