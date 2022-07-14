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
}
