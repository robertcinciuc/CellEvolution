using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataPacket {
    public ProgressionDataSerial progressionDataSerial;
    public MorphologySerial morphologySerial;
    
    public DataPacket(ProgressionData progressionData, GameObject player) {
        progressionDataSerial = new ProgressionDataSerial(progressionData, player);
        morphologySerial = new MorphologySerial(player.GetComponent<Morphology>());
    }
}
