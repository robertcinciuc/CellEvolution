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
        gameObject.transform.rotation = new Quaternion(0.71f, 0, 0, -0.71f);    
    }

    public void setMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void setHealth(float healthValue) {
        slider.value = healthValue;
    }
}
