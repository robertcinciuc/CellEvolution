using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuLogic : MonoBehaviour
{
    public static bool organIsDragged = false;
    public static bool attachedOrganIsDragged = false;
    public static bool playerFigureInstantiated = false;
    public GameObject player;
    public Camera upgradeMenuCamera;
    public GameObject upgradeMenuPlane;

    private GameObject playerFigure;
    private Vector3 safetyOffset = new Vector3(0, 5, 0);
    private Vector3 displayOffset = new Vector3(0, 0.5f, 0);
    private Dictionary<System.Guid, GameObject> organsOnDisplay;
    private Dictionary<System.Guid, Dictionary<System.Guid, GameObject>> movedOrgans;
    private Dictionary<System.Guid, Dictionary<System.Guid, GameObject>> addedOrgans;
    private Dictionary<System.Guid, System.Guid> removedOrgans;

    void Start(){
        organsOnDisplay = new Dictionary<System.Guid, GameObject>();
        movedOrgans = new Dictionary<System.Guid, Dictionary<System.Guid, GameObject>>();
        addedOrgans = new Dictionary<System.Guid, Dictionary<System.Guid, GameObject>>();
        removedOrgans = new Dictionary<System.Guid, System.Guid>();
    }

    void Update(){
        
    }

    private void FixedUpdate() {
        
    }

    public void renderPlayerFigure() {
        if (playerFigure != null && playerFigure.GetComponent<PlayerBodyStructure>() != null) {
            playerFigure.GetComponent<PlayerBodyStructure>().removeAllOrgans();
            Destroy(playerFigure);
        }
        
        playerFigure = new GameObject();
        playerFigure.name = "PlayerCopy";
        playerFigure.transform.position = upgradeMenuPlane.transform.position + displayOffset;
        PlayerBodyStructure playerCopyBodyStructure = playerFigure.AddComponent<PlayerBodyStructure>();
        PlayerBodyStructure playerBodyStructure = player.GetComponent<PlayerBodyStructure>();

        int i = 0;
        foreach (KeyValuePair<System.Guid, GameObject> segmentEntry in playerBodyStructure.getSegments()) {
            System.Guid segmentId = segmentEntry.Value.GetComponent<Segment>().segmentId;
            Vector3 segmentPos = playerFigure.transform.position + new Vector3(0, 0, -i*2);
            GameObject newSegment = playerCopyBodyStructure.addSegmentWithPos(segmentEntry.Value, segmentId, segmentPos);

            //Render organs
            foreach (Transform organ in segmentEntry.Value.transform) {
                System.Type organType = organ.GetComponent<Organ>().organType;
                System.Guid organId = organ.GetComponent<Organ>().id;
                GameObject newOrgan = playerCopyBodyStructure.addOrganOnSegmentWithPos(newSegment.gameObject, organType, organ.gameObject, organId);

                //Add attached organ behaviour
                AttachedOrgan attachedOrgan = newOrgan.gameObject.AddComponent<AttachedOrgan>();
                attachedOrgan.playerFigure = playerFigure;
                attachedOrgan.parentSegment = segmentEntry.Value;
                attachedOrgan.upgradeMenuCamera = upgradeMenuCamera;
                attachedOrgan.upgradeMenuLogic = this;
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
        clickableOrgan.playerFigure = playerFigure;
        clickableOrgan.organType = organType;
        clickableOrgan.organ = organ;
        clickableOrgan.upgradeMenuCamera = upgradeMenuCamera;
        clickableOrgan.upgradeMenuLogic = this;
        clickableOrgan.organComponent = organComponent;
        clickableOrgan.upgradeMenuPlane = upgradeMenuPlane;

        //Add serial organ to organ component
        SerialOrgan serialOrgan = new SerialOrgan(organ);
        organComponent.serialOrgan = serialOrgan;

        //Add organ to display organs
        organsOnDisplay.Add(organComponent.id, organ);

        return organ;
    }

    public void applyUpgrade() {
        PlayerBodyStructure playerBodyStructure = player.GetComponent<PlayerBodyStructure>();

        //Apply removed organs
        foreach (KeyValuePair<System.Guid, System.Guid> segmentEntry in removedOrgans) {
            playerBodyStructure.removeOrgan(segmentEntry.Key, segmentEntry.Value);
        }

        //Apply moved organs
        foreach (KeyValuePair<System.Guid, Dictionary<System.Guid, GameObject>> segmentEntry in movedOrgans) {
            foreach (KeyValuePair<System.Guid, GameObject> organEntry in segmentEntry.Value) {
                GameObject organ = organEntry.Value;
                System.Guid organId = organ.GetComponent<Organ>().id;
                Vector3 localPos = organ.transform.localPosition;
                Quaternion localRot = organ.transform.localRotation;
                player.GetComponent<PlayerBodyStructure>().moveOrgan(segmentEntry.Key, organId, localPos, localRot);
            }
        }

        //Apply added organs
        foreach (KeyValuePair<System.Guid, Dictionary<System.Guid, GameObject>> segmentEntry in addedOrgans) {
            foreach (KeyValuePair<System.Guid, GameObject> organEntry in segmentEntry.Value) {
                GameObject playerSegment = playerBodyStructure.getSegment(segmentEntry.Key);
                GameObject organ = organEntry.Value;
                System.Type organType = organ.GetComponent<Organ>().organType;
                playerBodyStructure.addOrganOnSegmentWithPos(playerSegment, organType, organ, organEntry.Key);
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
        removedOrgans.Add(segmentId, organId);
    }

    public void putMovedOrgan(System.Guid oldSegmentId, System.Guid newSegmentId, System.Guid organId, GameObject organ) {
        removedOrgans.Add(oldSegmentId, organId);

        if (!addedOrgans.ContainsKey(newSegmentId)) {
            addedOrgans.Add(newSegmentId, new Dictionary<System.Guid, GameObject>());
        }
        if (!addedOrgans[newSegmentId].ContainsKey(organId)) {
            addedOrgans[newSegmentId].Add(organId, organ);
        } else {
            addedOrgans[newSegmentId][organId] = organ;
        }
    }

    public void resetMovedAndAddedOrgans() {
        addedOrgans.Clear();
        movedOrgans.Clear();
    }

    public void removeFromDisplayMap(System.Guid organId) {
        organsOnDisplay.Remove(organId);
    }
}
