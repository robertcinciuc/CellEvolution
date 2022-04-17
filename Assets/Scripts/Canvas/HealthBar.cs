using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour {
    
    public Slider slider;

    public void setMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void setHealth(float healthValue) {
        slider.value = healthValue;
    }
}
