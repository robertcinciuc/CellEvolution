using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {

    public static bool organIsDragged = false;
    public static bool attachedOrganIsDragged = false;
    public static bool figureInstantiated = false;
    public GameObject player;
    public Camera upgradeMenuCamera;
    public GameObject upgradeMenuPlane;

    private GameObject figure;
    private Vector3 safetyOffset = new Vector3(0, 5, 0);
    private Vector3 displayOffset = new Vector3(0, 0.5f, 0);
    private Dictionary<System.Guid, GameObject> organsOnDisplay;
    private Dictionary<System.Guid, Dictionary<System.Guid, GameObject>> movedOrgans;
    private Dictionary<System.Guid, Dictionary<System.Guid, GameObject>> addedOrgans;
    private Dictionary<System.Guid, HashSet<System.Guid>> removedOrgans;

    void Start() {
        organsOnDisplay = new Dictionary<System.Guid, GameObject>();
        movedOrgans = new Dictionary<System.Guid, Dictionary<System.Guid, GameObject>>();
        addedOrgans = new Dictionary<System.Guid, Dictionary<System.Guid, GameObject>>();
        removedOrgans = new Dictionary<System.Guid, HashSet<System.Guid>>();
    }

    void Update() {
    }

    private void FixedUpdate() {
    }

    public void renderFigure() {
        if (figure != null && figure.GetComponent<Morphology>() != null) {
            //figure.GetComponent<Morphology>().removeAllOrgans();
            Destroy(figure);
        }

        figure = new GameObject();
        figure.name = "PlayerCopy";
        figure.transform.position = upgradeMenuPlane.transform.position + displayOffset;
        Morphology figureMorphology = figure.AddComponent<Morphology>();
        Morphology playerMorphology = player.GetComponent<Morphology>();

        int i = 0;
        foreach (KeyValuePair<System.Guid, GameObject> segmentEntry in playerMorphology.getSegments()) {
            System.Guid segmentId = segmentEntry.Value.GetComponent<Segment>().segmentId;
            Vector3 segmentPos = figure.transform.position + new Vector3(0, 0, -i * 2);
            GameObject newSegment = figureMorphology.addSegmentWithPos(segmentEntry.Value, segmentId, segmentPos);

            //Render organs
            foreach (Transform organ in segmentEntry.Value.transform) {
                System.Type organType = organ.GetComponent<Organ>().organType;
                System.Guid organId = organ.GetComponent<Organ>().id;
                GameObject newOrgan = figureMorphology.addOrganOnSegmentWithPos(newSegment.gameObject, organType, organ.gameObject, organId);

                //Add attached organ behaviour
                AttachedOrgan attachedOrgan = newOrgan.gameObject.AddComponent<AttachedOrgan>();
                attachedOrgan.figure = figure;
                attachedOrgan.parentSegment = segmentEntry.Value;
                attachedOrgan.upgradeMenuCamera = upgradeMenuCamera;
                attachedOrgan.upgradeManager = this;
                attachedOrgan.upgradeMenuPlane = upgradeMenuPlane;
            }

            i++;
        }
    }

    public void protectPlayer() {
        //Protect original player with offsetting and immobilizing
        player.transform.position += safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void uprotectPlayer() {
        player.transform.position -= safetyOffset;
        player.GetComponent<Rigidbody>().useGravity = true;
    }

    public void instMenuOrgans() {
        Vector3 displayPosition = upgradeMenuPlane.transform.position + displayOffset;

        instMenuOrgan("Prefabs/Mouth", displayPosition + new Vector3(2, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.Mouth.ToString());
        instMenuOrgan("Prefabs/MouthClaw", displayPosition + new Vector3(4, 0, 2), Quaternion.identity, typeof(Mouths), Mouths.MouthClaw.ToString());
        instMenuOrgan("Prefabs/Flagella", displayPosition + new Vector3(2, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.Flagella.ToString());
        instMenuOrgan("Prefabs/TwinFlagella", displayPosition + new Vector3(4, 0, 0), Quaternion.identity, typeof(LocomotionOrgans), LocomotionOrgans.TwinFlagella.ToString());
        instMenuOrgan("Prefabs/Spike", displayPosition + new Vector3(2, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Spike.ToString());
        instMenuOrgan("Prefabs/Tooth", displayPosition + new Vector3(4, 0, -2), Quaternion.identity, typeof(AttackOrgans), AttackOrgans.Tooth.ToString());
    }

    public void destroyMenuBodyParts() {
        organsOnDisplay.Clear();
    }

    public GameObject instMenuOrgan(string prefabPath, Vector3 pos, Quaternion rot, System.Type organType, string organName) {
        GameObject organ = Instantiate((GameObject)Resources.Load(prefabPath, typeof(GameObject)), pos, rot);
        organ.name = organName;

        //Add organ component
        Organ organComponent = organ.gameObject.AddComponent<Organ>();
        organComponent.organType = organType;
        organComponent.id = System.Guid.NewGuid();
        organComponent.organName = organName;

        //Add clickable organ component
        ClickableOrgan clickableOrgan = organ.AddComponent<ClickableOrgan>();
        clickableOrgan.figure = figure;
        clickableOrgan.organType = organType;
        clickableOrgan.organ = organ;
        clickableOrgan.upgradeMenuCamera = upgradeMenuCamera;
        clickableOrgan.upgradeManager = this;
        clickableOrgan.organComponent = organComponent;
        clickableOrgan.upgradeMenuPlane = upgradeMenuPlane;

        //Add serial organ to organ component
        OrganSerial serialOrgan = new OrganSerial(organ);
        organComponent.serialOrgan = serialOrgan;

        //Add organ to display organs
        organsOnDisplay.Add(organComponent.id, organ);

        return organ;
    }

    public void applyUpgrade() {
        Morphology playerMorphology = player.GetComponent<Morphology>();

        //Apply removed organs
        foreach (KeyValuePair<System.Guid, HashSet<System.Guid>> segmentEntry in removedOrgans) {
            foreach (System.Guid organ in removedOrgans[segmentEntry.Key]) {
                playerMorphology.removeOrgan(segmentEntry.Key, organ);
            }
        }

        //Apply added organs
        foreach (KeyValuePair<System.Guid, Dictionary<System.Guid, GameObject>> segmentEntry in addedOrgans) {
            foreach (KeyValuePair<System.Guid, GameObject> organEntry in segmentEntry.Value) {
                GameObject playerSegment = playerMorphology.getSegment(segmentEntry.Key);
                GameObject organ = organEntry.Value;
                System.Type organType = organ.GetComponent<Organ>().organType;
                playerMorphology.addOrganOnSegmentWithPos(playerSegment, organType, organ, organEntry.Key);
            }
        }
    }

    public void putAddedOrgan(System.Guid segmentId, System.Guid organId, GameObject organ) {
        if (!addedOrgans.ContainsKey(segmentId)) {
            addedOrgans.Add(segmentId, new Dictionary<System.Guid, GameObject>());
        }
        addedOrgans[segmentId].Add(organId, organ);
    }

    public void putRemovedOrgan(System.Guid segmentId, System.Guid organId) {
        if (!removedOrgans.ContainsKey(segmentId)) {
            removedOrgans.Add(segmentId, new HashSet<System.Guid>());
        }
        removedOrgans[segmentId].Add(organId);

        if (addedOrgans.ContainsKey(segmentId) && addedOrgans[segmentId].ContainsKey(organId)) {
            addedOrgans[segmentId].Remove(organId);
        }
    }

    public void putMovedOrgan(System.Guid oldSegmentId, System.Guid newSegmentId, System.Guid organId, GameObject organ) {
        if (!removedOrgans.ContainsKey(oldSegmentId)) {
            removedOrgans.Add(oldSegmentId, new HashSet<System.Guid>());
        }
        if ((addedOrgans.ContainsKey(oldSegmentId) && !addedOrgans[oldSegmentId].ContainsKey(organId)) || !addedOrgans.ContainsKey(oldSegmentId)) {
            removedOrgans[oldSegmentId].Add(organId);
        }

        if (!addedOrgans.ContainsKey(newSegmentId)) {
            addedOrgans.Add(newSegmentId, new Dictionary<System.Guid, GameObject>());
        }
        if (addedOrgans.ContainsKey(oldSegmentId) && addedOrgans[oldSegmentId].ContainsKey(organId)) {
            addedOrgans[oldSegmentId].Remove(organId);
        }
        if (!addedOrgans[newSegmentId].ContainsKey(organId)) {
            addedOrgans[newSegmentId].Add(organId, organ);
        } else {
            addedOrgans[newSegmentId][organId] = organ;
        }
    }

    public void resetMovedAndAddedOrgans() {
        removedOrgans.Clear();
        addedOrgans.Clear();
        movedOrgans.Clear();
    }

    public void removeFromDisplayMap(System.Guid organId) {
        organsOnDisplay.Remove(organId);
    }
}
