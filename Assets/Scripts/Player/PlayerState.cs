using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool isActive;
    public HealthBar healthBar;
    public float health = 100;
    public float maxHealth = 100;

    void Start(){
        isActive = true;
        healthBar.setMaxHealth(health);
    }

    void Update(){
    }

    public static void setInactive() {
        isActive = false;
    }

    public static void setActive() {
        isActive = true;     
    }

    public void takeDamage(float damage) {
        if(health - damage <= 0) {
            health = 0;
            Destroy(gameObject);
        }else{
            health -= damage;
        }

        healthBar.setHealth(health);
    }

    public void heal(float amount) {
        if (health + amount > maxHealth) {
            health = maxHealth;
        } else {
            health += amount;
        }

        healthBar.setHealth(health);
    }

    public void sethealth(float amount){
        health = amount;
        healthBar.setHealth(amount);
    }

    public void updatePlayerState(PlayerStateSerial playerStateSerial) {
        isActive = playerStateSerial.isActive;
        health = playerStateSerial.health;
        maxHealth = playerStateSerial.maxHealth;
    }

    public void resetPlayerState() {
        isActive = true;
        health = 100;
        maxHealth = 100;
    }
}
