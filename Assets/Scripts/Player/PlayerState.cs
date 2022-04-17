using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool isActive;
    public HealthBar healthBar;

    private float health = 100;

    void Start(){
        isActive = true;
        healthBar.setMaxHealth(health);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)) {
            health -= 20;
            healthBar.setHealth(health);
        }
    }

    public static void setInactive() {
        isActive = false;
    }

    public static void setActive() {
        isActive = true;     
    }
}
