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
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public float playerRotW;
    public float playerRotX;
    public float playerRotY;
    public float playerRotZ;
}
