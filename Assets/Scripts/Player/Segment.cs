using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public System.Guid segmentId;
    public string segmentName;
    
    private Dictionary<System.Guid, GameObject> organs;

    void Awake(){
        organs = new Dictionary<System.Guid, GameObject>();
    }

    void Update(){
        
    }

    public void removeOrgan(System.Guid organId) {
        if (organs.ContainsKey(organId)) {
            GameObject organToRemove = organs[organId];
            organs.Remove(organId);
            Destroy(organToRemove);
        }
    }

    public void removeOrganFromMapping(System.Guid organId) {
        if (organs.ContainsKey(organId)) {
            organs.Remove(organId);
        }
    }

    public void addOrganToMapping(System.Guid organId, GameObject organ) {
        organs.Add(organId, organ);
    }

    public Dictionary<System.Guid, GameObject> getOrgans() {
        return organs;
    }

    public void updateSegment(SegmentSerial segmentSerial) {
        //Add classic components
        foreach(System.Type componentType in segmentSerial.classicComponents) {
            gameObject.AddComponent(componentType);
        }
        gameObject.AddComponent<PlayerCollision>();

        //Add player movement if head
        if (segmentSerial.playerMovementSerial != null) {
            PlayerMovement playerMovement = gameObject.AddComponent<PlayerMovement>();
            playerMovement.playerSpeed = segmentSerial.playerMovementSerial.playerSpeed;
        }

        //Add organs
        foreach (KeyValuePair<System.Guid, OrganSerial> entry in segmentSerial.organsSerial) {
            Vector3 organPos = new Vector3(entry.Value.posX, entry.Value.posY, entry.Value.posZ);
            Quaternion organRot = new Quaternion(entry.Value.rotW, entry.Value.rotX, entry.Value.rotY, entry.Value.rotZ);
            GameObject organ = Instantiate((GameObject)Resources.Load("Prefabs/" + entry.Value.organName, typeof(GameObject)), organPos, organRot);

            //Temporary workaround
            Organ oldOrganComponent = organ.AddComponent<Organ>();
            oldOrganComponent.organName = entry.Value.organName;

            gameObject.transform.parent.GetComponent<Morphology>().addOrganOnSegmentWithPos(gameObject, entry.Value.organType, organ, entry.Value.id);
        }
    }
}
