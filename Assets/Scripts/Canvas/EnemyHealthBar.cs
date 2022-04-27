using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    void Start(){
    }

    void Update(){
        updatePosAndRotation();
    }

    public void setMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void setHealth(float healthValue) {
        slider.value = healthValue;
    }

    private void updatePosAndRotation() {
        gameObject.transform.rotation = new Quaternion(0.71f, 0, 0, -0.71f);
        gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x, 2, gameObject.transform.parent.position.z + 1);
    }


}
