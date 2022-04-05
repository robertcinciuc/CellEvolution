using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float timeLeft = -1f;
    private Dictionary<int, Vector3> enemyDirections;
    

    void Start(){
        enemyDirections = new Dictionary<int, Vector3>();
    }

    void Update(){
        
    }

    void FixedUpdate() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            foreach (KeyValuePair<LocalPlanes, List<GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
                foreach (GameObject enemy in planeEnemies.Value) {
                    Vector3 randomDirection = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

                    if (enemyDirections.ContainsKey(enemy.GetInstanceID())) {
                        enemyDirections[enemy.GetInstanceID()] = randomDirection;
                    } else {
                        enemyDirections.Add(enemy.GetInstanceID(), randomDirection);
                    }
                }
            }
            timeLeft = 2f;
        }

        //Update the velocity each tick
        foreach (KeyValuePair<LocalPlanes, List<GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
            foreach (GameObject enemy in planeEnemies.Value) {
                enemy.GetComponent<Rigidbody>().velocity = enemyDirections[enemy.GetInstanceID()] * 5;
            }
        }
    }
}
