using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private float maxHealth = 100;
    private float health = 100;

    void Start(){
        
    }

    void Update(){
        
    }
    public void takeDamage(float damage) {
        if (health - damage <= 0) {
            health = 0;
            EnemySpawner.deleteEnemy(gameObject.GetInstanceID());
        } else {
            health -= damage;
        }

        //healthBar.setHealth(health);
    }

    public void heal(float amount) {
        if (health + amount > maxHealth) {
            health = maxHealth;
        } else {
            health += amount;
        }

        //healthBar.setHealth(health);
    }
}
