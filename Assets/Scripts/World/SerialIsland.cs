using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerialIsland {

    public Land landType = Land.Island;
    public int scale;
    public Vector3 pos;
    public System.Guid islandId;

    public SerialIsland(Vector3 pos, int scale, System.Guid islandId) {
        this.pos = pos;
        this.scale = scale;
        this.islandId = islandId;
    }
}
