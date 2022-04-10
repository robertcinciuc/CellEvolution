using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float timeLeft = -1f;
    private Dictionary<int, Vector3> enemyDirections;
    private float enemyMaxSpeed = 5f;
    

    void Start(){
        enemyDirections = new Dictionary<int, Vector3>();
    }

    void Update(){
        
    }

    void FixedUpdate() {
        timeLeft -= Time.deltaTime;
        
        if (timeLeft < 0) {
            foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
                foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies.Value) {
                    Vector3 randomDirection = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

                    if (enemyDirections.ContainsKey(enemyEntry.Value.GetInstanceID())) {
                        enemyDirections[enemyEntry.Value.GetInstanceID()] = randomDirection;
                    } else {
                        enemyDirections.Add(enemyEntry.Value.GetInstanceID(), randomDirection);
                    }
                }
            }
            timeLeft = 2f;
        } else {
            Dictionary<int, Vector3> enemyDirectionsTemp = new Dictionary<int, Vector3>(enemyDirections);
            enemyDirections.Clear();
            foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
                foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies.Value) {
                    Vector3 randomDirection = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

                    if (enemyDirectionsTemp.ContainsKey(enemyEntry.Key)) {
                        enemyDirections.Add(enemyEntry.Key, enemyDirectionsTemp[enemyEntry.Key]);
                    } else {
                        enemyDirections.Add(enemyEntry.Key, randomDirection);
                    }
                }
            }
        }

        //Update the velocity each tick
        foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
            foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies.Value) {
                enemyEntry.Value.GetComponent<Rigidbody>().velocity = enemyDirections[enemyEntry.Key] * enemyMaxSpeed;
            }
        }
    }
}
