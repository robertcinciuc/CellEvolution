using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MorphologySerial {

    public int nbSegments;
    public float segmentDist;
    public Dictionary<System.Guid, SegmentSerial> segmentsSerial;

    public MorphologySerial(Morphology morphology) {
        segmentsSerial = new Dictionary<System.Guid, SegmentSerial>();

        nbSegments = morphology.nbSegments;
        segmentDist = morphology.segmentDistance;

        foreach (KeyValuePair<System.Guid, GameObject> entry in morphology.getSegments()) {
            segmentsSerial.Add(entry.Key, new SegmentSerial(entry.Value));
        }
    }
}
