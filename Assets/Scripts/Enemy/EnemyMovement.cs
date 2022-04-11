using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float timeLeft = -1f;
    private float maxSpeed = 5f;
    private Dictionary<int, Vector3> enemyDirections;

    void Start(){
        enemyDirections = new Dictionary<int, Vector3>();
    }

    void Update(){
        
    }

    void FixedUpdate() {
        timeLeft -= Time.deltaTime;

        updateVelocityRotation();
    }

    private void updateVelocityRotation() {
        if (timeLeft < 0) {
            foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
                foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies.Value) {
                    Vector3 randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-maxSpeed, maxSpeed));

                    if (enemyDirections.ContainsKey(enemyEntry.Key)) {
                        enemyDirections[enemyEntry.Key] = randomDirection;
                    } else {
                        enemyDirections.Add(enemyEntry.Key, randomDirection);
                    }

                    enemyEntry.Value.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(enemyEntry.Value.GetComponent<Rigidbody>().transform.rotation, Quaternion.LookRotation(randomDirection), 50 * Time.deltaTime);
                }
            }
            timeLeft = 2f;
        } else {
            Dictionary<int, Vector3> enemyDirectionsTemp = new Dictionary<int, Vector3>(enemyDirections);
            enemyDirections.Clear();
            foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEnemies in EnemySpawner.planeEnemies) {
                foreach (KeyValuePair<int, GameObject> enemyEntry in planeEnemies.Value) {
                    Vector3 randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), 0, Random.Range(-5f, 5f));

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
                enemyEntry.Value.GetComponent<Rigidbody>().velocity = enemyDirections[enemyEntry.Key];
            }
        }
    }
}
