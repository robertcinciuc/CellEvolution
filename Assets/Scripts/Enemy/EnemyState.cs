using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float maxHealth = 100;
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

        gameObject.transform.Find("EnemyHealthBar(Clone)").transform.Find("Canvas").Find("EnemyHealthBar").GetComponent<EnemyHealthBar>().setHealth(health);
    }

    public void heal(float amount) {
        if (health + amount > maxHealth) {
            health = maxHealth;
        } else {
            health += amount;
        }

        gameObject.transform.GetComponent<EnemyHealthBar>().setHealth(health);
    }
}
