using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataPacket {
    public int nbEnemiesKilled;
    public int nbMeatsEaten;
    public float health;
    public Dictionary<System.Guid, SerialOrgan> playerSerialOrgans;
}
