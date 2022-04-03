using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool isActive;

    void Start(){
        isActive = true;
    }

    void Update(){
        
    }

    public static void setInactive() {
        isActive = false;
    }

    public static void setActive() {
        isActive = true;     
    }
}
