using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    void Start(){
    }

    void Update(){
        
    }

    public void setPosition() {
        Vector3 parentPos = gameObject.transform.position + new Vector3(0, 1, 0);

        Instantiate((GameObject)Resources.Load("Prefabs/TestPrefab", typeof(GameObject)), parentPos, Quaternion.identity);
    }
}
