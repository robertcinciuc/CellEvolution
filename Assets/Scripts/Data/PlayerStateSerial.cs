using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStateSerial {

    public bool isActive;
    public float health;
    public float maxHealth;

    public PlayerStateSerial(PlayerState playerState) {
        this.isActive = PlayerState.isActive;
        this.health = playerState.health;
        this.maxHealth = playerState.maxHealth;
    }
}
