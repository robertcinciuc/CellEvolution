using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerMovementSerial {

    public float playerSpeed;

    public PlayerMovementSerial(PlayerMovement playerMovement) {
        this.playerSpeed = playerMovement.playerSpeed;
    }

}
