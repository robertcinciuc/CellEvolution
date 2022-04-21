using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool isActive;
    public HealthBar healthBar;

    private float health = 100;
    private float maxHealth = 100;

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
}
