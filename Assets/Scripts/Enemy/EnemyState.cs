using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float maxHealth = 100;

    private float health = 100;
    private EnemySpawner enemySpawner;

    void Start(){
        
    }

    void Update(){
        
    }

    public void setEnemySpawner(EnemySpawner enemySpawner) {
        this.enemySpawner = enemySpawner;
    }

    public void takeDamage(float damage, System.Enum source) {
        if (health - damage <= 0) {
            health = 0;
            enemySpawner.deleteEnemy(gameObject.GetInstanceID());

            if (Characters.Player.Equals(source)) {
                ProgressionData.nbEnemiesKilled++;
            }

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
