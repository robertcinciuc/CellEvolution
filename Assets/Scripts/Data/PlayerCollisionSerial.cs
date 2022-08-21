using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerCollisionSerial {

    public System.Type playerCollisionType;

    public PlayerCollisionSerial(PlayerCollision playerCollision) {
        this.playerCollisionType = typeof(PlayerCollision);
    }

}
